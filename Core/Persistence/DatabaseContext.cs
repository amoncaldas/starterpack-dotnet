using System;
using Microsoft.EntityFrameworkCore;
using StarterPack.Core.Helpers;
using StarterPack.Models.Config;

namespace StarterPack.Core.Persistence
{
    public partial class DatabaseContext : DbContext
    {       
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        { 
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            ModelConfig.Set(modelBuilder);            
        } 

        public static DbContext Context(Type dbContextType) {
           return (DbContext) Services.Instance.GetService(dbContextType);
        }
              
    }
}