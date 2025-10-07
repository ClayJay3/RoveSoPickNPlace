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
            optionsBuilder.UseSqlite(Configuration.GetConnectionString("KROFT_DB"));
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

            // Job
            modelBuilder.Entity<Job>(entity =>
            {
                entity.HasKey(x => x.ID);
                entity.Property(x => x.ID)
                    .HasColumnName("ID")
                    .IsRequired()
                    .ValueGeneratedOnAdd();
            });

            // UploadedFile
            modelBuilder.Entity<UploadedFile>(entity =>
            {
                entity.HasKey(x => x.ID);
                entity.Property(x => x.ID)
                    .HasColumnName("ID")
                    .IsRequired()
                    .ValueGeneratedOnAdd();
            });

            // ComponentDefinition
            modelBuilder.Entity<ComponentDefinition>(entity =>
            {
                entity.HasKey(x => x.ID);
                entity.Property(x => x.ID)
                    .HasColumnName("ID")
                    .IsRequired()
                    .ValueGeneratedOnAdd();
            });

            // Feeder
            modelBuilder.Entity<Feeder>(entity =>
            {
                entity.HasKey(x => x.ID);
                entity.Property(x => x.ID)
                    .HasColumnName("ID")
                    .IsRequired()
                    .ValueGeneratedOnAdd();
            });

            // ComponentPlacementRecord
            modelBuilder.Entity<ComponentPlacementRecord>(entity =>
            {
                entity.HasKey(x => x.ID);
                entity.Property(x => x.ID)
                    .HasColumnName("ID")
                    .IsRequired()
                    .ValueGeneratedOnAdd();
            });

            // InspectionResult
            modelBuilder.Entity<InspectionResult>(entity =>
            {
                entity.HasKey(x => x.ID);
                entity.Property(x => x.ID)
                    .HasColumnName("ID")
                    .IsRequired()
                    .ValueGeneratedOnAdd();
            });

            // BomEntry
            modelBuilder.Entity<BOMEntry>(entity =>
            {
                entity.HasKey(x => x.ID);
                entity.Property(x => x.ID)
                    .HasColumnName("ID")
                    .IsRequired()
                    .ValueGeneratedOnAdd();
            });

            // VisionCalibration
            modelBuilder.Entity<VisionCalibration>(entity =>
            {
                entity.HasKey(x => x.ID);
                entity.Property(x => x.ID)
                    .HasColumnName("ID")
                    .IsRequired()
                    .ValueGeneratedOnAdd();
            });

            // CameraFeed
            modelBuilder.Entity<CameraFeed>(entity =>
            {
                entity.HasKey(x => x.ID);
                entity.Property(x => x.ID)
                    .HasColumnName("ID")
                    .IsRequired()
                    .ValueGeneratedOnAdd();
            });

            // LogEntry
            modelBuilder.Entity<LogEntry>(entity =>
            {
                entity.HasKey(x => x.ID);
                entity.Property(x => x.ID)
                    .HasColumnName("ID")
                    .IsRequired()
                    .ValueGeneratedOnAdd();
            });

            // Notification
            modelBuilder.Entity<Notification>(entity =>
            {
                entity.HasKey(x => x.ID);
                entity.Property(x => x.ID)
                    .HasColumnName("ID")
                    .IsRequired()
                    .ValueGeneratedOnAdd();
            });

            // ManualControlCommand
            modelBuilder.Entity<ManualControlCommand>(entity =>
            {
                entity.HasKey(x => x.ID);
                entity.Property(x => x.ID)
                    .HasColumnName("ID")
                    .IsRequired()
                    .ValueGeneratedOnAdd();
            });

            modelBuilder.Owned<Position>();
        }
    }
}