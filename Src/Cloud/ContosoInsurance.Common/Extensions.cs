using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace System
{
    public static class Extensions
    {
        public static bool In<T>(this T t, IEnumerable<T> source)
        {
            return source.Contains(t);
        }

        public static bool In<T>(this T t, params T[] source)
        {
            return source.Contains(t);
        }

        public static bool IgnoreCaseEqualsTo(this string s, string other)
        {
            return StringComparer.OrdinalIgnoreCase.Equals(s, other);
        }
        public static bool IsNullOrEmpty(this string s)
        {
            return string.IsNullOrEmpty(s);
        }

        public static bool IsNotNullAndEmpty(this string s)
        {
            return !string.IsNullOrEmpty(s);
        }

        public static IQueryable<T> WhereIf<T>(this IQueryable<T> queryable, Expression<Func<T, bool>> predicate, bool condition)
        {
            return condition ? queryable.Where(predicate) : queryable;
        }

        public static TValue GetValue<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, TValue defaultValue = default(TValue))
        {
            var v = defaultValue;
            dictionary.TryGetValue(key, out v);
            return v;
        }
    }
}