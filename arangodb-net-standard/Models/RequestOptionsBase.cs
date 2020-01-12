using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace ArangoDBNetStandard.Models
{
    public class ContentSerializationOptions
    {
        public ContentSerializationOptions(bool camelCasePropertyNames, bool ignoreNullValues)
        {
            CamelCasePropertyNames = camelCasePropertyNames;
            IgnoreNullValues = ignoreNullValues;
        }

        public bool CamelCasePropertyNames { get; set; }
        public bool IgnoreNullValues { get; set; }
    }

    public abstract class RequestOptionsBase
    {
        private IList<PropertyInfo> _propertyInfoCache;

        public ContentSerializationOptions ContentSerializationOptions { get; set; }

        public IReadOnlyDictionary<string, string> ToQueryStringValues()
        {
            Dictionary<string, string> values = new Dictionary<string, string>();
            PrepareQueryStringValues(values);
            return new ReadOnlyDictionary<string, string>(values);
        }

        protected virtual void PrepareQueryStringValues(IDictionary<string, string> values)
        {
            AddPropertiesToQueryStringValues(values);
        }

        protected void AddPropertiesToQueryStringValues(IDictionary<string, string> values)
        {
            if (_propertyInfoCache == null)
                _propertyInfoCache = GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance).ToList();
            foreach (PropertyInfo propertyInfo in _propertyInfoCache)
            {
                if (propertyInfo.DeclaringType == typeof(RequestOptionsBase))
                    continue;

                object value = propertyInfo.GetValue(this);
                if (value != null)
                {
                    values.Add(propertyInfo.Name.ToCamelCase(), value.ToString());
                }
            }
        }
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