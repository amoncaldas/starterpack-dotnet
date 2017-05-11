using System.Collections.Generic;
using System.Linq;

namespace StarterPack.Core.Extensions
{
    public static class IQueryableExtensions
    {
         public static List<T> Fetch<T>(this IQueryable<T> query) where T : Models.Model<T> {
             return query.ToList();
         }

        public static IQueryable<T> Paginate<T>(this IQueryable<T> query, int page, int perPage) where T : Models.Model<T> {
            return query.Skip((page-1) * perPage).Take(perPage);
        }       
        
    }
}