using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.Configuration;
using Kroft.Api.Entities;
using Microsoft.Data.Sqlite;
using System.Data;
using System.Data.Common;
using LasSharp;
using MathNet.Numerics.LinearAlgebra;
using Microsoft.CodeAnalysis.Scripting.Hosting;


namespace Kroft.Api.Entities
{
    public class LiDARDatabase : DbContext
    {
        private readonly IConfiguration Configuration;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="configuration">The configuration that contains the DB connection string and params. (Implicitly passed in)</param>
        public LiDARDatabase(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        /// <summary>
        /// This is used by the Entity Framework Core to configure the database context.
        /// </summary>
        /// <param name="optionsBuilder"></param>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Use the named connection string "LIDAR_DB" (set in appsettings.json or environment)
            optionsBuilder.UseSqlite(Configuration.GetConnectionString("LIDAR_DB"));
        }

        /// <summary>
        /// Processed LiDAR points table (matches schema from your python pipeline).
        /// </summary>
        public DbSet<LiDARPoint> LiDARPoints { get; set; }


        /// <summary>
        /// Configure the LiDARPoint entity mapping and indexes.
        /// </summary>
        public void Configure(EntityTypeBuilder<LiDARPoint> b)
        {
            // Primary key
            b.HasKey(x => x.ID);
            b.Property(x => x.ID)
             .HasColumnName("id")
             .IsRequired()
             .ValueGeneratedOnAdd();

            // Column mapping and constraints
            b.Property(x => x.Easting).HasColumnName("easting").IsRequired();
            b.Property(x => x.Northing).HasColumnName("northing").IsRequired();
            b.Property(x => x.Altitude).HasColumnName("altitude");
            b.Property(x => x.Zone).HasColumnName("zone").HasMaxLength(8);
            b.Property(x => x.Classification).HasColumnName("classification").HasMaxLength(64);

            b.Property(x => x.NormalX).HasColumnName("normal_x");
            b.Property(x => x.NormalY).HasColumnName("normal_y");
            b.Property(x => x.NormalZ).HasColumnName("normal_z");

            b.Property(x => x.Slope).HasColumnName("slope");
            b.Property(x => x.Rough).HasColumnName("rough");
            b.Property(x => x.Curvature).HasColumnName("curvature");
            b.Property(x => x.TravScore).HasColumnName("trav_score");
        }

        /// <summary>
        /// OnModelCreating - register entity configurations.
        /// </summary>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Register LiDARPoint configuration
            modelBuilder.Entity<LiDARPoint>(Configure);
        }

        /// <summary>
        /// Convenient one-call setup you can call at app startup.
        /// - Ensures DB is created or migrated
        /// - Sets PRAGMAs
        /// - Creates RTREE virtual table, triggers, and indexes
        /// - Optionally populates the RTREE from existing rows (do this after bulk inserts)
        /// </summary>
        public async Task EnsureDatabasePrepared(bool populateRtree = false, bool useMigrations = false)
        {
            // create or migrate schema
            if (useMigrations)
            {
                await this.Database.MigrateAsync();
            }
            else
            {
                await this.Database.EnsureCreatedAsync();
            }

            // Use the raw connection for executing SQLite-specific SQL/PRAGMA
            var conn = this.Database.GetDbConnection();
            try
            {
                if (conn.State != System.Data.ConnectionState.Open) conn.Open();

                // Small helper to execute single SQL statement
                void Exec(string sql)
                {
                    using var cmd = conn.CreateCommand();
                    cmd.CommandText = sql;
                    cmd.ExecuteNonQuery();
                }

                // PRAGMAs for performance (match your python script)
                Exec("PRAGMA journal_mode = WAL;");
                Exec("PRAGMA synchronous = NORMAL;");

                // Create RTREE virtual table (if not exists)
                Exec(@"
                    CREATE VIRTUAL TABLE IF NOT EXISTS LiDARPoints_idx
                    USING rtree(
                        id,
                        min_x,
                        max_x,
                        min_y,
                        max_y
                    );
                ");

                // Optionally populate the RTREE from existing rows (fast to do after bulk insertion)
                if (populateRtree)
                {
                    Exec(@"
                        INSERT OR IGNORE INTO LiDARPoints_idx (id, min_x, max_x, min_y, max_y)
                        SELECT id, easting, easting, northing, northing FROM LiDARPoints;
                    ");
                }

                // Drop triggers if they already exist (safe to call repeatedly)
                Exec("DROP TRIGGER IF EXISTS rp_ai;");
                Exec("DROP TRIGGER IF EXISTS rp_ad;");
                Exec("DROP TRIGGER IF EXISTS rp_au;");

                // Create triggers to keep RTREE in sync (useful for incremental updates)
                Exec(@"
                    CREATE TRIGGER rp_ai
                    AFTER INSERT ON LiDARPoints
                    BEGIN
                        INSERT INTO LiDARPoints_idx(id, min_x, max_x, min_y, max_y)
                        VALUES (NEW.id, NEW.easting, NEW.easting, NEW.northing, NEW.northing);
                    END;
                ");
                Exec(@"
                    CREATE TRIGGER rp_ad
                    AFTER DELETE ON LiDARPoints
                    BEGIN
                        DELETE FROM LiDARPoints_idx WHERE id = OLD.id;
                    END;
                ");
                Exec(@"
                    CREATE TRIGGER rp_au
                    AFTER UPDATE ON LiDARPoints
                    BEGIN
                        UPDATE LiDARPoints_idx
                        SET min_x = NEW.easting,
                            max_x = NEW.easting,
                            min_y = NEW.northing,
                            max_y = NEW.northing
                        WHERE id = OLD.id;
                    END;
                ");

                // Create B-tree indexes (IF NOT EXISTS is supported)
                Exec("CREATE INDEX IF NOT EXISTS idx_plp_classification ON LiDARPoints(classification);");
                Exec("CREATE INDEX IF NOT EXISTS idx_plp_trav_score     ON LiDARPoints(trav_score);");
                Exec("CREATE INDEX IF NOT EXISTS idx_plp_rough          ON LiDARPoints(rough);");
                Exec("CREATE INDEX IF NOT EXISTS idx_plp_slope          ON LiDARPoints(slope);");
                Exec("CREATE INDEX IF NOT EXISTS idx_plp_curvature      ON LiDARPoints(curvature);");
                Exec("CREATE INDEX IF NOT EXISTS idx_plp_altitude       ON LiDARPoints(altitude);");
                Exec("CREATE INDEX IF NOT EXISTS idx_plp_normal_x       ON LiDARPoints(normal_x);");
                Exec("CREATE INDEX IF NOT EXISTS idx_plp_normal_y       ON LiDARPoints(normal_y);");
                Exec("CREATE INDEX IF NOT EXISTS idx_plp_normal_z       ON LiDARPoints(normal_z);");
                Exec("CREATE INDEX IF NOT EXISTS idx_plp_coord          ON LiDARPoints(easting, northing);");
            }
            finally
            {
                if (conn.State == System.Data.ConnectionState.Open) conn.Close();
            }
        }

        /// <summary>
        /// Fast bulk insert into ProcessedLiDARPoints using prepared statements and batched transactions.
        /// - batchSize: number of rows per transaction commit (10k is a good default).
        /// - dropTriggersDuringImport: if true, temporary drops RTREE triggers to avoid per-row trigger overhead.
        /// After import, call EnsureDatabasePrepared(populateRtree: true) or re-create triggers manually.
        /// </summary>
        public void BulkInsertPoints(IEnumerable<LiDARPoint> points, int batchSize = 10000, bool dropTriggersDuringImport = true)
        {
            // Get the underlying connection (cast to SqliteConnection so we can use SqliteTransaction)
            var conn = (SqliteConnection)this.Database.GetDbConnection();
            var openedHere = false;
            if (conn.State != ConnectionState.Open)
            {
                conn.Open();
                openedHere = true;
            }

            try
            {
                // Optional: temporarily drop triggers to speed up bulk insert
                if (dropTriggersDuringImport)
                {
                    using (var dropCmd = conn.CreateCommand())
                    {
                        dropCmd.CommandText = @"
                            DROP TRIGGER IF EXISTS rp_ai;
                            DROP TRIGGER IF EXISTS rp_ad;
                            DROP TRIGGER IF EXISTS rp_au;
                        ";
                        dropCmd.ExecuteNonQuery();
                    }
                }

                // Prepare insert command (list every column you intend to populate)
                using (var insertCmd = conn.CreateCommand())
                {
                    insertCmd.CommandText = @"
                        INSERT INTO ProcessedLiDARPoints
                          (easting, northing, altitude, zone, classification,
                           normal_x, normal_y, normal_z, slope, rough, curvature, trav_score)
                        VALUES
                          (@easting, @northing, @altitude, @zone, @classification,
                           @normal_x, @normal_y, @normal_z, @slope, @rough, @curvature, @trav_score);
                    ";

                    // Create & add parameters once (DbParameter from the command)
                    DbParameter pE = insertCmd.CreateParameter(); pE.ParameterName = "@easting"; insertCmd.Parameters.Add(pE);
                    DbParameter pN = insertCmd.CreateParameter(); pN.ParameterName = "@northing"; insertCmd.Parameters.Add(pN);
                    DbParameter pZ = insertCmd.CreateParameter(); pZ.ParameterName = "@altitude"; insertCmd.Parameters.Add(pZ);
                    DbParameter pZone = insertCmd.CreateParameter(); pZone.ParameterName = "@zone"; insertCmd.Parameters.Add(pZone);
                    DbParameter pClass = insertCmd.CreateParameter(); pClass.ParameterName = "@classification"; insertCmd.Parameters.Add(pClass);

                    DbParameter pNx = insertCmd.CreateParameter(); pNx.ParameterName = "@normal_x"; insertCmd.Parameters.Add(pNx);
                    DbParameter pNy = insertCmd.CreateParameter(); pNy.ParameterName = "@normal_y"; insertCmd.Parameters.Add(pNy);
                    DbParameter pNz = insertCmd.CreateParameter(); pNz.ParameterName = "@normal_z"; insertCmd.Parameters.Add(pNz);

                    DbParameter pSlope = insertCmd.CreateParameter(); pSlope.ParameterName = "@slope"; insertCmd.Parameters.Add(pSlope);
                    DbParameter pRough = insertCmd.CreateParameter(); pRough.ParameterName = "@rough"; insertCmd.Parameters.Add(pRough);
                    DbParameter pCurv = insertCmd.CreateParameter(); pCurv.ParameterName = "@curvature"; insertCmd.Parameters.Add(pCurv);
                    DbParameter pTrav = insertCmd.CreateParameter(); pTrav.ParameterName = "@trav_score"; insertCmd.Parameters.Add(pTrav);

                    // Prepare statement for speed
                    insertCmd.Prepare();

                    // Start the first transaction
                    using (var trans = conn.BeginTransaction())
                    {
                        insertCmd.Transaction = trans;
                        int count = 0;

                        foreach (var pt in points)
                        {
                            // Set parameter values (use DBNull.Value for nullables)
                            pE.Value = pt.Easting;
                            pN.Value = pt.Northing;
                            pZ.Value = pt.Altitude == null ? (object)DBNull.Value : pt.Altitude;
                            pZone.Value = string.IsNullOrEmpty(pt.Zone) ? (object)DBNull.Value : pt.Zone;
                            pClass.Value = string.IsNullOrEmpty(pt.Classification) ? (object)DBNull.Value : pt.Classification;

                            pNx.Value = pt.NormalX == null ? (object)DBNull.Value : pt.NormalX;
                            pNy.Value = pt.NormalY == null ? (object)DBNull.Value : pt.NormalY;
                            pNz.Value = pt.NormalZ == null ? (object)DBNull.Value : pt.NormalZ;

                            pSlope.Value = pt.Slope == null ? (object)DBNull.Value : pt.Slope;
                            pRough.Value = pt.Rough == null ? (object)DBNull.Value : pt.Rough;
                            pCurv.Value = pt.Curvature == null ? (object)DBNull.Value : pt.Curvature;
                            pTrav.Value = pt.TravScore == null ? (object)DBNull.Value : pt.TravScore;

                            insertCmd.ExecuteNonQuery();
                            count++;

                            // Commit and start a new transaction each batchSize rows
                            if (count % batchSize == 0)
                            {
                                trans.Commit();
                                trans.Dispose();
                                // Begin a fresh transaction
                                var newTrans = conn.BeginTransaction();
                                insertCmd.Transaction = newTrans;
                                // continue in the using-block by replacing the reference (can't reassign 'trans' inside using),
                                // so use an explicit block: for simplicity we call Commit/Begin/assign above and continue.
                            }
                        }

                        // Final commit if needed (if last batch wasn't a round multiple)
                        try
                        {
                            if (insertCmd.Transaction != null)
                            {
                                insertCmd.Transaction.Commit();
                            }
                        }
                        catch
                        {
                            // ignore commit exceptions here - let caller handle errors/logging if desired
                            throw;
                        }
                    } // end using transaction
                } // end using insertCmd

                // Optionally: repopulate RTREE now that the bulk load is finished
                // (If you dropped triggers, you should repopulate and then re-create triggers)
                if (dropTriggersDuringImport)
                {
                    using (var populateCmd = conn.CreateCommand())
                    {
                        // Recreate triggers and populate rtree in a simple and safe way:
                        populateCmd.CommandText = @"
                            INSERT OR IGNORE INTO ProcessedLiDARPoints_idx (id, min_x, max_x, min_y, max_y)
                            SELECT id, easting, easting, northing, northing FROM ProcessedLiDARPoints;
                        ";
                        populateCmd.ExecuteNonQuery();

                        // Recreate triggers (same definitions as EnsureDatabasePrepared)
                        populateCmd.CommandText = @"
                            CREATE TRIGGER IF NOT EXISTS rp_ai
                            AFTER INSERT ON ProcessedLiDARPoints
                            BEGIN
                                INSERT INTO ProcessedLiDARPoints_idx(id, min_x, max_x, min_y, max_y)
                                VALUES (NEW.id, NEW.easting, NEW.easting, NEW.northing, NEW.northing);
                            END;
                            CREATE TRIGGER IF NOT EXISTS rp_ad
                            AFTER DELETE ON ProcessedLiDARPoints
                            BEGIN
                                DELETE FROM ProcessedLiDARPoints_idx WHERE id = OLD.id;
                            END;
                            CREATE TRIGGER IF NOT EXISTS rp_au
                            AFTER UPDATE ON ProcessedLiDARPoints
                            BEGIN
                                UPDATE ProcessedLiDARPoints_idx
                                SET min_x = NEW.easting, max_x = NEW.easting, min_y = NEW.northing, max_y = NEW.northing
                                WHERE id = OLD.id;
                            END;
                        ";
                        populateCmd.ExecuteNonQuery();
                    }
                }
            }
            finally
            {
                if (openedHere) conn.Close();
            }
        }


        public async Task ComputeMetrics(CancellationToken cancellationToken = default)
        {
            // Iterate through points in lasData and and compute metrics for each point. Store results in LiDARPoints table.
            
        }

        /// <summary>
        /// Compute metrics for a single point (x,y,z) within a given radius. This will query the RTREE index for fast neighbor lookup.
        /// </summary>
        /// <param name="x">The x-coordinate of the point.</param>
        /// <param name="y">The y-coordinate of the point.</param>
        /// <param name="z">The z-coordinate of the point.</param>
        /// <param name="radius">The radius within which to search for neighboring points.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A task representing the asynchronous operation, with a <see cref="LiDARPoint"/> as the result.</returns>
        public async Task<LiDARPoint?> ComputeMetricsForPoint(double x, double y, double z, double radius, CancellationToken cancellationToken = default)
        {
            // Create instance variables.
            LiDARPoint? result = null;
            double normalX = 0.0;
            double normalY = 0.0;
            double normalZ = 0.0;
            double slope = 0.0;
            double rough = 0.0;
            double curvature = 0.0;
            double travScore = 0.0;


            // Query the database for points within the radius using the RTREE index.
            var points = await this.LiDARPoints
                .FromSqlRaw(@"
                    SELECT p.*
                    FROM LiDARPoints AS p
                    JOIN LiDARPoints_idx AS idx
                    ON p.id = idx.id
                    WHERE idx.min_x BETWEEN {0} AND {1}
                    AND idx.min_y BETWEEN {2} AND {3};
                ", x - radius, x + radius, y - radius, y + radius)
                .ToListAsync(cancellationToken);

            // If no points found, return null.
            if (points.Count == 0)
            {
                return null;
            }

            // Fit plane: Z = aX + bY + c
            var A = Matrix<double>.Build.Dense(points.Count, 3);
            var B = Vector<double>.Build.Dense(points.Count);
            for (int i = 0; i < points.Count; i++)
            {
                A[i, 0] = points[i].Easting ?? 0.0;
                A[i, 1] = points[i].Northing ?? 0.0;
                A[i, 2] = 1.0;
                B[i] = points[i].Altitude ?? 0.0;
            }
            var AtA = A.TransposeThisAndMultiply(A);
            var AtB = A.TransposeThisAndMultiply(B);
            var coeffs = AtA.Solve(AtB);
            double a = coeffs[0];
            double b = coeffs[1];
            double c = coeffs[2];

            // Compute slope (degrees) store in method variable.
            slope = Math.Atan(Math.Sqrt(a * a + b * b)) * (180.0 / Math.PI);

            // Compute roughness (RMS of residuals) store in method variable.
            double sumSq = 0.0;
            foreach (var pt in points)
            {
                double pointEasting = pt.Easting ?? 0.0;
                double pointNorthing = pt.Northing ?? 0.0;
                double zFit = a * (pointEasting) + b * (pointNorthing) + c;
                double res = (pt.Altitude ?? 0.0) - zFit;
                sumSq += res * res;
            }
            rough = Math.Sqrt(sumSq / points.Count);

            // Compute curvature (mean of absolute residuals) store in method variable.
            double sumAbs = 0.0;
            foreach (var pt in points)
            {
                double pointEasting = pt.Easting ?? 0.0;
                double pointNorthing = pt.Northing ?? 0.0;
                double zFit = a * (pointEasting) + b * (pointNorthing) + c;
                double res = (pt.Altitude ?? 0.0) - zFit;
                sumAbs += Math.Abs(res);
            }
            curvature = sumAbs / points.Count;

            // Compute traversability score (example formula) store in method variable. Normalize from 0 to 1 and base off of slope, roughness, and curvature.
            double maxSlope = 90.0; // degrees 
            double maxRough = 1.0;  // meters   
            double sNorm = Math.Min(1.0, slope / maxSlope);
            double rNorm = Math.Min(1.0, rough / maxRough);
            travScore = Math.Max(0.0, 1.0 - (sNorm + rNorm + curvature));

            // Create and return result LiDARPoint with computed metrics.
            result = new LiDARPoint
            {
                Easting = x,
                Northing = y,
                Altitude = z,
                NormalX = normalX,
                NormalY = normalY,
                NormalZ = normalZ,
                Slope = slope,
                Rough = rough,
                Curvature = curvature,
                TravScore = travScore
            }; 

            return result;
        }
    }
}
