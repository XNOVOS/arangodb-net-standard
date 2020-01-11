using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq.Expressions;
using System.Reflection;

namespace ArangoDBNetStandard
{
    public abstract class RequestOptionsBase
    {
        public IReadOnlyDictionary<string, string> ToQueryStringValues()
        {
            Dictionary<string, string> values = new Dictionary<string, string>();
            PrepareQueryStringValues(values);
            return new ReadOnlyDictionary<string, string>(values);
        }

        protected abstract void PrepareQueryStringValues(IDictionary<string,string> values);

        protected void AddPropertyToQueryStringValues<TSource, TProperty>(IDictionary<string,string> values, Expression<Func<TSource, TProperty>> expression, bool camelCase = true, Func<string> valueToStringOverride = null) where TSource : class
        {
            if (!(expression.Body is MemberExpression member))
                throw new ArgumentException($"Expression '{expression}' refers to a method, not a property.");

            PropertyInfo propInfo = member.Member as PropertyInfo;
            if (propInfo == null)
                throw new ArgumentException($"Expression '{expression}' refers to a field, not a property.");

            if (typeof(TSource) != propInfo.ReflectedType && !typeof(TSource).IsSubclassOf(propInfo.ReflectedType))
                throw new ArgumentException(
                    $"Expression '{expression}' refers to a property that is not from type {typeof(TSource)}.");
            string value = string.Empty;
            if (valueToStringOverride == null)
            {
                object propertyValue = expression.Compile().Invoke(this as TSource);
                if (propertyValue == null)
                    return;
                value = propertyValue.ToString().ToLowerInvariant();
            }
            else
            {
                value = valueToStringOverride();
            }

            values.Add(propInfo.Name.ToCamelCase(), value);
        }
    }
}