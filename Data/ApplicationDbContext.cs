
using DataNex.Model.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using StoreApi.Data.Models;

namespace StoreApi.Data
{
    public class ApplicationDbContext : IdentityDbContext<User, Role, Guid>
    {
        private string _connectionString;
        public ApplicationDbContext(string connectionString)
        {
            _connectionString = connectionString;
        }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> contextOptions) : base(contextOptions)
        {

        }

        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<Brand> Brands { get; set; }
        public virtual DbSet<AffiliateProgram> AffiliatePromgrams { get; set; }
        public virtual DbSet<ImportSetting> ImportSettings { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<Analytics> Analytics { get; set; }
        public virtual DbSet<Log> Logs { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                if (!string.IsNullOrEmpty(_connectionString))
                {
                    optionsBuilder.UseSqlServer(_connectionString);
                }

            }
        }
    }
}