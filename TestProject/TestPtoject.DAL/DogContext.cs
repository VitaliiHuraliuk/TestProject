using Microsoft.EntityFrameworkCore;
using TestProject.Models;

namespace TestPtoject.DAL
{
    public class DogContext : DbContext
    {
        public DbSet<DogEntity> Dogs { get; set; }

        public DogContext()
        {
            Database.EnsureCreated();
        }
        public DogContext(DbContextOptions<DogContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=.;Database=Dogs;Trusted_Connection=SSPI;Encrypt=false;TrustServerCertificate=true;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            SeedData(modelBuilder);
        }
            void SeedData(ModelBuilder modelBuilder)
            {
                modelBuilder.Entity<DogEntity>().HasData(new DogEntity
                {
                    Id = 1,
                    Name = "Neo",
                    Color = "red & amber",
                    TailLength = 22,
                    Weight = 32
                });

                modelBuilder.Entity<DogEntity>().HasData(new DogEntity
                {
                    Id = 2,
                    Name = "Jessy",
                    Color = "black & white",
                    TailLength = 7,
                    Weight = 14
                });
            }

        }
    }