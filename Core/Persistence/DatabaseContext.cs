using System;
using Microsoft.EntityFrameworkCore;
using StarterPack.Core.Helpers;

namespace StarterPack.Core.Persistence
{
    public partial class DatabaseContext : DbContext
    {              
        public DatabaseContext(DbContextOptions options) : base(options)
        { 
            
        }

        public static DbContext Context(Type dbContextType) {           
            return (DbContext) Services.Instance.GetService(dbContextType);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
           ConfigureModels(modelBuilder);        
        } 
              
    }
}