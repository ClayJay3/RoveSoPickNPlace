using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Newtonsoft.Json.Bson;
using RoveSoPickNPlace.Models.Entities;

namespace RoveSoPickNPlace.Api.Entities
{
    public class RoveSoPickNPlaceDatabase : DbContext
    {
        private readonly IConfiguration Configuration;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="configuration">The configuration that contains the DB connection string and params. (Implicitly passed in)</param>
        public RoveSoPickNPlaceDatabase(IConfiguration configuration)
        {
            // Assign member variables.
            Configuration = configuration;
        }

        /// <summary>
        /// This is used by the Entity Framework Core to configure the database context.
        /// </summary>
        /// <param name="optionsBuilder"></param>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite(Configuration.GetConnectionString("ROVESOPICKNPLACE_DB"));
            // optionsBuilder.ConfigureWarnings(warnings => warnings.Ignore(RelationalEventId.PendingModelChangesWarning));
        }

        /// <summary>
        /// This is used by the Entity Framework Core and represents a collection of 
        /// entities in a context. In this case is corresponds to a database table.
        /// </summary>
        /// <value></value>
        public DbSet<Job> Jobs { get; set; }
        public DbSet<UploadedFile> UploadedFiles { get; set; }
        public DbSet<ComponentDefinition> ComponentDefinitions { get; set; }
        public DbSet<Feeder> Feeders { get; set; }
        public DbSet<ComponentPlacementRecord> ComponentPlacements { get; set; }
        public DbSet<InspectionResult> InspectionResults { get; set; }
        public DbSet<BOMEntry> BomEntries { get; set; }
        public DbSet<VisionCalibration> VisionCalibrations { get; set; }
        public DbSet<CameraFeed> CameraFeeds { get; set; }
        public DbSet<LogEntry> LogEntries { get; set; }
        public DbSet<Notification> Notifications { get; set; }

        /// <summary>
        /// Configure the primary key for the UserProfiles table.
        /// </summary>
        /// <param name="modelBuilder"></param>
        // public void Configure(EntityTypeBuilder<UserProfile> modelBuilder)
        // {
        //     modelBuilder.HasKey(x => x.ID);
        //     modelBuilder.Property(x => x.ID)
        //         .HasColumnName(@"ID")
        //         .IsRequired()
        //         .ValueGeneratedOnAdd()
        //         ;
        // }

        /// <summary>
        /// This is used by the Entity Framework Core to configure the database context.
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            /// <summary>
            /// Configure the primary key for each table.
            /// </summary>

            // --- Job ---
            modelBuilder.Entity<Job>(entity =>
            {
                entity.HasKey(x => x.ID);
                entity.Property(x => x.ID)
                    .HasColumnName("ID")
                    .IsRequired()
                    .ValueGeneratedOnAdd();

                // Job -> Placements (1:N)
                entity.HasMany(j => j.Placements)
                    .WithOne(p => p.Job)
                    .HasForeignKey(p => p.JobID)
                    .OnDelete(DeleteBehavior.Cascade);

                // Job -> BOM entries (1:N)
                entity.HasMany(j => j.BomEntries)
                    .WithOne(b => b.Job)
                    .HasForeignKey(b => b.JobID)
                    .OnDelete(DeleteBehavior.Cascade);

                // Job -> LogEntries (1:N) [optional]
                entity.HasMany(j => j.LogEntries)
                    .WithOne(l => l.Job)
                    .HasForeignKey(l => l.JobID)
                    .OnDelete(DeleteBehavior.SetNull);

                // Job -> UploadedFile (CPL) (0..1)
                entity.HasOne(j => j.CplFile)
                    .WithMany(f => f.JobsReferencing)
                    .HasForeignKey(j => j.CplFileId)
                    .OnDelete(DeleteBehavior.SetNull);
            });

            // --- UploadedFile ---
            modelBuilder.Entity<UploadedFile>(entity =>
            {
                entity.HasKey(x => x.ID);
                entity.Property(x => x.ID)
                    .HasColumnName("ID")
                    .IsRequired()
                    .ValueGeneratedOnAdd();
            });

            // --- BOMEntry ---
            modelBuilder.Entity<BOMEntry>(entity =>
            {
                entity.HasKey(x => x.ID);
                entity.Property(x => x.ID)
                    .HasColumnName("ID")
                    .IsRequired()
                    .ValueGeneratedOnAdd();
            });

            // --- LogEntry ---
            modelBuilder.Entity<LogEntry>(entity =>
            {
                entity.HasKey(x => x.ID);
                entity.Property(x => x.ID)
                    .HasColumnName("ID")
                    .IsRequired()
                    .ValueGeneratedOnAdd();

                // allow JobId to be null; if job removed, set JobId null
                entity.HasOne(l => l.Job)
                    .WithMany(j => j.LogEntries)
                    .HasForeignKey(l => l.JobID)
                    .OnDelete(DeleteBehavior.SetNull);
            });

            // --- MachineState ---
            modelBuilder.Entity<MachineState>(entity =>
            {
                entity.HasKey(x => x.ID);
                entity.Property(x => x.ID)
                    .HasColumnName("ID")
                    .IsRequired()
                    .ValueGeneratedOnAdd();

                // Owned Position
                entity.OwnsOne(m => m.CurrentPosition, p =>
                {
                    p.Property(pp => pp.X).HasColumnName("CurrentPosition_X");
                    p.Property(pp => pp.Y).HasColumnName("CurrentPosition_Y");
                    p.Property(pp => pp.Z).HasColumnName("CurrentPosition_Z");
                    p.Property(pp => pp.Rotation).HasColumnName("CurrentPosition_Rotation");
                });

                // Active job (optional)
                entity.HasOne(ms => ms.ActiveJob)
                    .WithMany()                 // we don't create reverse navigation
                    .HasForeignKey(ms => ms.ActiveJobID)
                    .OnDelete(DeleteBehavior.SetNull);
            });

            // --- ManualControlCommand ---
            modelBuilder.Entity<ManualControlCommand>(entity =>
            {
                entity.HasKey(x => x.ID);
                entity.Property(x => x.ID)
                    .HasColumnName("ID")
                    .IsRequired()
                    .ValueGeneratedOnAdd();
            });

            // --- Notification ---
            modelBuilder.Entity<Notification>(entity =>
            {
                entity.HasKey(x => x.ID);
                entity.Property(x => x.ID)
                    .HasColumnName("ID")
                    .IsRequired()
                    .ValueGeneratedOnAdd();
            });

            // --- CameraFeed ---
            modelBuilder.Entity<CameraFeed>(entity =>
            {
                entity.HasKey(x => x.ID);
                entity.Property(x => x.ID)
                    .HasColumnName("ID")
                    .IsRequired()
                    .ValueGeneratedOnAdd();

                entity.HasMany(c => c.Calibrations)
                    .WithOne(vc => vc.Camera)
                    .HasForeignKey(vc => vc.CameraID)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // --- VisionCalibration ---
            modelBuilder.Entity<VisionCalibration>(entity =>
            {
                entity.HasKey(x => x.ID);
                entity.Property(x => x.ID)
                    .HasColumnName("ID")
                    .IsRequired()
                    .ValueGeneratedOnAdd();

                entity.HasOne(vc => vc.Camera)
                    .WithMany(c => c.Calibrations)
                    .HasForeignKey(vc => vc.CameraID)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // --- ComponentDefinition ---
            modelBuilder.Entity<ComponentDefinition>(entity =>
            {
                entity.HasKey(x => x.ID);
                entity.Property(x => x.ID)
                    .HasColumnName("ID")
                    .IsRequired()
                    .ValueGeneratedOnAdd();
            });

            // --- Feeder ---
            modelBuilder.Entity<Feeder>(entity =>
            {
                entity.HasKey(x => x.ID);
                entity.Property(x => x.ID)
                    .HasColumnName("ID")
                    .IsRequired()
                    .ValueGeneratedOnAdd();

                // Owned pickup position
                entity.OwnsOne(f => f.PickupPosition, p =>
                {
                    p.Property(pp => pp.X).HasColumnName("PickupPosition_X");
                    p.Property(pp => pp.Y).HasColumnName("PickupPosition_Y");
                    p.Property(pp => pp.Z).HasColumnName("PickupPosition_Z");
                    p.Property(pp => pp.Rotation).HasColumnName("PickupPosition_Rotation");
                });

                // Feeder -> ComponentDefinition (FK) (no reverse navigation on ComponentDefinition)
                entity.HasOne(f => f.ComponentDefinition)
                    .WithMany() // no reverse nav property on ComponentDefinition
                    .HasForeignKey(f => f.ComponentDefinitionId)
                    .OnDelete(DeleteBehavior.Restrict); 
            });

            // --- ComponentPlacementRecord ---
            modelBuilder.Entity<ComponentPlacementRecord>(entity =>
            {
                entity.HasKey(x => x.ID);
                entity.Property(x => x.ID)
                    .HasColumnName("ID")
                    .IsRequired()
                    .ValueGeneratedOnAdd();

                // Placement -> Job
                entity.HasOne(p => p.Job)
                    .WithMany(j => j.Placements)
                    .HasForeignKey(p => p.JobID)
                    .OnDelete(DeleteBehavior.Cascade);

                // Placement -> ComponentDefinition (optional)
                entity.HasOne(p => p.ComponentDefinition)
                    .WithMany(cd => cd.PlacementRecords) // not perfect semantically but ensures nav exists; if undesired, remove WithMany
                    .HasForeignKey(p => p.ComponentDefinitionID)
                    .OnDelete(DeleteBehavior.SetNull);

                // Placement -> InspectionResult (1:1)
                entity.HasOne(p => p.InspectionResult)
                    .WithOne(ir => ir.ComponentPlacementRecord)
                    .HasForeignKey<InspectionResult>(ir => ir.ComponentPlacementRecordID)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // --- InspectionResult ---
            modelBuilder.Entity<InspectionResult>(entity =>
            {
                entity.HasKey(x => x.ID);
                entity.Property(x => x.ID)
                    .HasColumnName("ID")
                    .IsRequired()
                    .ValueGeneratedOnAdd();

                // FK configured in ComponentPlacementRecord mapping above (HasForeignKey<InspectionResult>)
            });
        }
    }
}