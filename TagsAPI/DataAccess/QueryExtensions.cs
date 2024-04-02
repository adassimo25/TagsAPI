using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using TagsAPI.Contracts.Common;

namespace TagsAPI.DataAccess
{
    public static class QueryExtensions
    {
        public const int MinPageSize = 1;
        public const int MaxPageSize = 100;

        public static IQueryable<TResult> Paginated<TSource, TResult>(
            this IQueryable<TResult> queryable,
            PaginatedQuery<TSource> query)
        {
            var pageNumber = Math.Max(query.PageNumber, 1);
            var pageSize = Math.Clamp(query.PageSize, MinPageSize, MaxPageSize);
            var skipCount = (pageNumber - 1) * pageSize;

            return queryable.Skip(skipCount).Take(pageSize);
        }

        public static async Task<PaginatedResult<TResult>> ToPaginatedResultAsync<TSource, TResult>(
            this IQueryable<TResult> queryable,
            PaginatedQuery<TSource> query)
        {
            return await queryable.ToPaginatedResultAsync(queryable, query);
        }

        private static async Task<PaginatedResult<TResult>> ToPaginatedResultAsync<TSource, TResult, TIntermediate>(
            this IQueryable<TResult> queryable,
            IQueryable<TIntermediate> countQuery,
            PaginatedQuery<TSource> query)
        {
            var totalElements = await countQuery.CountAsync();
            return await queryable.ToPaginatedResultAsync(totalElements, query);
        }

        public static async Task<PaginatedResult<TResult>> ToPaginatedResultAsync<TSource, TResult>(
            this IQueryable<TResult> queryable,
            int totalElements,
            PaginatedQuery<TSource> query)
        {
            return new()
            {
                Elements = await queryable.Paginated(query).ToListAsync(),
                TotalElements = totalElements,
            };
        }

        public static IOrderedQueryable<T> OrderBy<T, TKey>(this IQueryable<T> queryable, Expression<Func<T, TKey>> sortBy, bool isDescending)
            => isDescending ? queryable.OrderByDescending(sortBy) : queryable.OrderBy(sortBy);

        public static IQueryable<T> ConditionalWhere<T>(
            this IQueryable<T> queryable, Expression<Func<T, bool>> predicate, bool condition)
        {
            return condition
                ? queryable.Where(predicate)
                : queryable;
        }
    }
}
