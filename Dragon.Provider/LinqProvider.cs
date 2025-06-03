using System.Linq.Expressions;

namespace Dragon.Provider
{
    public static class AsyncProvider
    {
        public static async Task<List<T>> ToListAsync<T>(this IAsyncEnumerable<T> items, CancellationToken cancellationToken = default)
        {
            List<T> results = [];
            await foreach (T item in items.WithCancellation(cancellationToken).ConfigureAwait(false)) { results.Add(item); }
            return results;
        }
        public static IQueryable<T> OrFilter<T>(this IQueryable<T> source, Dictionary<string, object> filter)
        {
            Type type = typeof(T);
            ParameterExpression parameter = Expression.Parameter(typeof(T));
            IEnumerable<string> propertyList = typeof(T).GetProperties().Select(d => d.Name);
            List<Expression> expressionList = [];
            List<KeyValuePair<string, object>> filterList = filter.Where(d => d.Value != null && propertyList.Any(p => p.ToCamelCase() == d.Key)).ToList();
            if (filterList != null && filterList.Count > 0)
            {
                foreach (KeyValuePair<string, object> pair in filterList)
                {
                    expressionList.Add(MakeBinary(Expression.Property(parameter, propertyList.FirstOrDefault(p => p.ToCamelCase() == pair.Key)), Convert.ToString(pair.Value)));
                }
            }
            return expressionList != null && expressionList.Count > 0 ? source.Where(Expression.Lambda<Func<T, bool>>(expressionList.Aggregate(Expression.OrElse), parameter)) : source;
        }
        private static Expression MakeBinary(Expression key, string value)
        {
            object typedValue = null;
            if (key.Type != typeof(string))
            {
                Type valueType = Nullable.GetUnderlyingType(key.Type) ?? key.Type;
                typedValue = valueType.IsEnum ? Enum.Parse(valueType, value) : valueType == typeof(Guid) ? Guid.Parse(value) : Convert.ChangeType(value, valueType);
            }
            return key.Type != typeof(string) ? Expression.MakeBinary(ExpressionType.Equal, key, Expression.Constant(typedValue, key.Type)) : Expression.Call(key, "Contains", Type.EmptyTypes, Expression.Constant(value, typeof(string)));
        }
    }
}
