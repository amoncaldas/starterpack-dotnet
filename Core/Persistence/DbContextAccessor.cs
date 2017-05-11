using System;
using Microsoft.EntityFrameworkCore;
using StarterPack.Core.Helpers;

namespace StarterPack.Core.Persistence
{
    public class DbContextAccessor
    {     
        public static DbContext GetContext(Type dbContextType) {
            if(dbContextType == typeof(DefaultDbContext)) {
                return Services.DefaultDbContext;
            }
            return (DbContext) Services.Resolve(dbContextType);
        }

    }
}