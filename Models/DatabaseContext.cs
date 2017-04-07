using Microsoft.EntityFrameworkCore;

namespace Starterpack.Models
{
    //public class GenericRepository<T> : IRepository<T> where T : BaseModel
    public class DatabaseContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        { 
            
        }
    }
}