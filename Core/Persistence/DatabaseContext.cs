using Microsoft.EntityFrameworkCore;
using StarterPack.Models.Config;

namespace StarterPack.Models
{
    //public class GenericRepository<T> : IRepository<T> where T : BaseModel
    public class DatabaseContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }

        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        { 
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            ModelConfig.Set(modelBuilder);            
        }        
    }
}