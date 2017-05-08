using System.Collections.Generic;
using System.Linq;

namespace StarterPack.Core.Extensions
{
    public static class GetIQueryable
    {
         public static List<T> Fetch<T>(this IQueryable<T> query) where T : Models.Model<T> {
             return query.AsEnumerable().ToList();
         }
    }
}