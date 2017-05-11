using System;
using Microsoft.EntityFrameworkCore;
using StarterPack.Core.Helpers;

namespace StarterPack.Core.Persistence
{
    public partial class DefaultDbContext : DbContext
    {              
        public DefaultDbContext(DbContextOptions options) : base(options){}

        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
           ConfigureModels(modelBuilder);        
        } 
              
    }
}