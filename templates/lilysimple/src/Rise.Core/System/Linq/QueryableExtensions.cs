using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace System.Linq
{
    public static class QueryableExtensions
    {
        public static IQueryable<T> WhereIf<T>(this IQueryable<T> query, bool condition, Expression<Func<T, bool>> predicate)
        {
            return condition ? query.Where(predicate) : query;
        }

        public static IQueryable<T> PageByOffset<T>(this IQueryable<T> query, int skipCount, int maxResultCount)
        {
            return query.Skip(skipCount).Take(maxResultCount);
        }

        public static IQueryable<T> PageByNumber<T>(this IQueryable<T> query, int pageNumber, int pageSize)
        {
            var (skipCount, maxResultCount) = ((pageNumber - 1) * pageSize, pageSize);
            return query.PageByOffset(skipCount, maxResultCount);
        }

        public static IQueryable<T> Count<T>(this IQueryable<T> query, out long count)
        {
            count = query.Count();
            return query;
        }
    }
}
