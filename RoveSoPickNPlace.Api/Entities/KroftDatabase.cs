using RoveSoPickNPlace.Models.Accounts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Newtonsoft.Json.Bson;

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
        public DbSet<UserProfile> UserProfiles { get; set; }

        /// <summary>
        /// Configure the primary key for the UserProfiles table.
        /// </summary>
        /// <param name="modelBuilder"></param>
        public void Configure(EntityTypeBuilder<UserProfile> modelBuilder)
        {
            modelBuilder.HasKey(x => x.ID);
            modelBuilder.Property(x => x.ID)
                .HasColumnName(@"ID")
                .IsRequired()
                .ValueGeneratedOnAdd()
                ;
        }

        /// <summary>
        /// This is used by the Entity Framework Core to configure the database context.
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            /*
                Always make sure a default admin user is created.
            */
            modelBuilder.Entity<UserProfile>().HasData(
                new UserProfile
                {
                    ID = new Guid("61889689-5a72-40ec-bf80-560b85d5775b"),
                    Username = "admin",
                    Email = "admin@example.com",
                    HashedPassword = "JGSbvfD04Sc5pxiNgfb4ow==",
                    Salt = "bJZ9DdwaYkrQQ8/S+Opj/A==",
                    Argon2DegreeOfParallelism = 4,
                    Argon2Iterations = 4,
                    Argon2MemorySize = 1024 * 64,
                    FirstName = "Admin",
                    LastName = "User",
                    DateOfBirth = new DateTime(1980, 1, 1),
                    PhoneNumber = "123-456-7890",
                    Address = "123 Admin St",
                    City = "Admin City",
                    State = "Admin State",
                    PostalCode = "12345",
                    Country = "Admin Country",
                    CreatedAt = DateTime.MinValue,
                    UpdatedAt = DateTime.MinValue,
                    PermissionLevel = PermissionLevel.Admin
                }
            );
        }
    }
}