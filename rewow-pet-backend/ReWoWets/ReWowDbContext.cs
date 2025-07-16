using Microsoft.EntityFrameworkCore;
using ReWoWets.Models;

namespace ReWoWets
{
    public class ReWowDbContext : DbContext
    {
        public ReWowDbContext(DbContextOptions<ReWowDbContext> options) : base(options) { }

        public DbSet<PetOwner> PetOwners { get; set; }
        public DbSet<Pet> Pets { get; set; }
        public DbSet<Vaccination> Vaccinations { get; set; }
        public DbSet<MedicalService> MedicalServices { get; set; }
        public DbSet<Admin> Admins { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
} 