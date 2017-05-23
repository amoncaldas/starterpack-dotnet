using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace StarterPack.Core.Extensions
{
    public static class IQueryableExtensions
    {
        public static List<T> Fetch<T>(this IQueryable<T> query) where T : Persistence.Model<T> {
             return query.ToList();
        }

        public static IQueryable<T> Paginate<T>(this IQueryable<T> query, int page, int perPage) where T : Persistence.Model<T> {
            return query.Skip((page-1) * perPage).Take(perPage);
        }

        public static IOrderedQueryable<TSource> OrderBy<TSource>(this IQueryable<TSource> source, string propertyName)
        {
            return BaseOrderBy(source, propertyName, "OrderBy");
        }

        public static IOrderedQueryable<TSource> OrderByDescending<TSource>(this IQueryable<TSource> source, string propertyName)
        {
           return BaseOrderBy(source, propertyName, "OrderByDescending");
        }

        private static IOrderedQueryable<TSource> BaseOrderBy<TSource>(IQueryable<TSource> source, string propertyName, string orderMethod) {
            // LAMBDA: x => x.[PropertyName]
            var parameter = Expression.Parameter(typeof(TSource), "x");
            Expression property = Expression.Property(parameter, propertyName);
            var lambda = Expression.Lambda(property, parameter);

            // REFLECTION: source.OrderBy(x => x.Property)
            var orderByMethod = typeof(Queryable).GetMethods().First(x => x.Name == orderMethod && x.GetParameters().Length == 2);
            var orderByGeneric = orderByMethod.MakeGenericMethod(typeof(TSource), property.Type);
            var result = orderByGeneric.Invoke(null, new object[] { source, lambda });

            return (IOrderedQueryable<TSource>)result;
        }

    }
}
