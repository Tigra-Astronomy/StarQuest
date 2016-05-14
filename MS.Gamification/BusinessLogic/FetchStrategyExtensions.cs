using System;
using System.Linq.Expressions;

namespace MS.Gamification.BusinessLogic
    {
    /// <summary>
    /// Extension methods used by <see cref="GenericFetchStrategy{TEntity}"/>
    /// </summary>
    /// <remarks>
    /// Borrowed from http://blog.willbeattie.net/2011/02/specification-pattern-entity-framework.html
    /// </remarks>
    public static class FetchStrategyExtensions
        {
        public static string ToPropertyName<T>(this Expression<Func<T, object>> selector)
            {
            var me = selector.Body as MemberExpression;
            if (me == null)
                {
                throw new ArgumentException("MemberException expected.");
                }

            var propertyName = me.ToString().Remove(0, 2);
            return propertyName;
            }
        }
    }