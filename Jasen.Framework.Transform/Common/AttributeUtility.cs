using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Jasen.Framework.Transform
{
    public static class AttributeUtility
    {
        public static Dictionary<PropertyInfo, TAttribute> GetPropertyNameDictionary<T,TAttribute>() 
            where TAttribute: Attribute
        {
            var propertyNameDic = new Dictionary<PropertyInfo, TAttribute>();

            IList<PropertyInfo> propertyInfos = typeof (T).GetProperties();
            TAttribute attribute;

            foreach (PropertyInfo propertyInfo in propertyInfos)
            {
                attribute = GetCustomAttribute<TAttribute>(propertyInfo);

                if (attribute != null)
                {
                    propertyNameDic.Add(propertyInfo, attribute);
                }
            }

            return propertyNameDic;
        }

        public static TAttribute GetCustomAttribute<TAttribute>(PropertyInfo propertyInfo) 
            where TAttribute : Attribute
        {
            if (propertyInfo == null)
            {
                return null;
            }

            object[] customAttributes = propertyInfo.GetCustomAttributes(typeof(TAttribute), false);

            if (customAttributes.Length == 0)
            {
                return null;
            }

            return customAttributes[0] as TAttribute;
        }

        public static ColumnAttribute GetColumnAttribute(PropertyInfo propertyInfo)
        {
            object[] customAttributes = propertyInfo.GetCustomAttributes(typeof(ColumnAttribute), true);
            ColumnAttribute parameterAttribute;

            foreach (object attr in customAttributes)
            {
                parameterAttribute = attr as ColumnAttribute;

                if (parameterAttribute != null)
                {
                    return parameterAttribute;
                }
            }

            return null;
        }

        public static ColumnAttribute GetColumnAttribute(Type type, string propertyName)
        {
            if (type == null || string.IsNullOrEmpty(propertyName))
            {
                return null;
            }
            foreach (PropertyInfo propertyInfo in type.GetProperties())
            {
                if (string.Equals(propertyInfo.Name, propertyName))
                {
                    return GetColumnAttribute(propertyInfo);
                }
            }
            return null;
        }

        public static TableAttribute GetTableAttribute(Type type)
        {
            object[] customAttributes = type.GetCustomAttributes(typeof(TableAttribute), true);
            TableAttribute tableAttribute = null;
            foreach (object attr in customAttributes)
            {
                tableAttribute = attr as TableAttribute;
                if (tableAttribute != null)
                {
                    return tableAttribute;
                }
            }
            return null;
        }

    }
}
