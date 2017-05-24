using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace StarterPack.Core.Extensions
{
    public static class IQueryableExtensions
    {
        /// <summary>
        /// Executa a query e retorna a lista referente ao resultado
        /// </summary>
        /// <returns>Lista com os recursos</returns>
        public static List<T> Fetch<T>(this IQueryable<T> query) where T : Persistence.Model<T> {
             return query.ToList();
        }

        /// <summary>
        /// Realiza a paginação dos recursos
        /// </summary>
        /// <param name="page">Página</param>
        /// <param name="perPage">Quantidade por Página</param>
        /// <returns>Objeto Queryable</returns>
        public static IQueryable<T> Paginate<T>(this IQueryable<T> query, int page, int perPage) where T : Persistence.Model<T> {
            return query.Skip((page-1) * perPage).Take(perPage);
        }

        /// <summary>
        /// Realiza o OrderBy dos recursos de forma ascendente
        /// </summary>
        /// <param name="propertyName">Nome do atributo</param>
        /// <returns>Objeto Queryable Ordenado</returns>
        public static IOrderedQueryable<TSource> OrderBy<TSource>(this IQueryable<TSource> source, string propertyName)
        {
            return BaseOrderBy(source, propertyName, "OrderBy");
        }

        /// <summary>
        /// Realiza o OrderBy dos recursos de forma descendente
        /// </summary>
        /// <param name="propertyName">Nome do atributo</param>
        /// <returns>Objeto Queryable Ordenado</returns>
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
