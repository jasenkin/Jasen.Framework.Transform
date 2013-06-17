using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace Jasen.Framework.Transform
{
    /// <summary>
    /// 
    /// </summary>
    public static class EnumFieldProvider
    {

        public static T ToEnum<T>(int value,T defaultT) where T : struct
        {
            string enumName = Enum.GetName(typeof(T), value);

            return ToEnum<T>(enumName, defaultT);
        }

        public static T ToEnum<T>(string enumName, T defaultT) where T : struct
        {
            if (string.IsNullOrWhiteSpace(enumName))
            {
                return defaultT;
            }

            T result;

            if (!Enum.TryParse<T>(enumName.Trim(), out result))
            {
                return defaultT;
            }

            if (Enum.IsDefined(typeof(T), result))
            {
                return result;
            }

            return defaultT;
        }

        public static string ToEnumString<T>(this T enumItem) where T : struct
        {
            return typeof(T).Name + "." + enumItem;
        }

        public static T TryParse<T>(string typeValue, T defaultValue,bool containsTypeName = false) where T : struct
        { 
            typeValue = (typeValue ?? string.Empty).Trim();

            if (containsTypeName)
            {
                int startIndex = typeValue.IndexOf(".");

                if (startIndex > 0 && typeValue.Length > startIndex + 1)
                {
                    typeValue = typeValue.Substring(startIndex + 1);
                }
            }

            T enumType;
            Enum.TryParse(typeValue, out enumType);

            if (Enum.IsDefined(typeof(T), enumType))
            {
                return enumType;
            }

            return defaultValue;
        }

        public static EnumAttribute GetEnumAttribute(Type type)
        {
            if (type == null)
            {
                return null;
            }

            object[] customAttributes = type.GetCustomAttributes(typeof(EnumAttribute), true);
            EnumAttribute attribute = null;

            foreach (object attr in customAttributes)
            {
                attribute = attr as EnumAttribute;

                if (attribute != null)
                {
                    return attribute;
                }
            }

            return null;
        }

        public static IList<EnumItem<T>> GetItems<T>(bool isSpecialRequired = false) where T : struct
        {
            var fields = typeof(T).GetFields(BindingFlags.Static | BindingFlags.Public);

            if (fields == null || fields.Count() == 0)
            {
                return new List<EnumItem<T>>();
            }

            var enumItems = new List<EnumItem<T>>();
            EnumAttribute attribute = null;
            T currentValue;

            foreach (var field in fields)
            {
                object[] customAttributes = field.GetCustomAttributes(typeof(EnumAttribute), true);
                foreach (object attr in customAttributes)
                {
                    attribute = attr as EnumAttribute;

                    if (attribute != null)
                    {
                        if (attribute.IsSpecialRequired)
                        {
                            if (!isSpecialRequired)
                            {
                                continue;
                            }
                        }

                        currentValue = (T)field.GetValue(null);

                        enumItems.Add(new EnumItem<T>(currentValue, attribute.Desc));
                    }
                } 
            }

            return enumItems;
        }

        public static IList<EnumItem<T>> GetItems<T>(IList<T> entities) where T : struct
        {
            if (entities == null || entities.Count() == 0)
            {
                return new List<EnumItem<T>>();
            }

            var enumItems = new List<EnumItem<T>>();

            foreach (var entity in entities)
            {
                enumItems.Add(GetItem<T>(entity));
            }

            return enumItems;
        }

        public static string GetItemDescription<T>(T enumItem) where T : struct
        {
            var field = typeof(T).GetField(enumItem.ToString());

            if (field == null)
            {
                return string.Empty; 
            }

            object[] customAttributes = field.GetCustomAttributes(typeof(EnumAttribute), true);
            foreach (object attr in customAttributes)
            {
                EnumAttribute attribute = attr as EnumAttribute;

                if (attribute != null)
                {
                    return attribute.Desc;
                }
            }

            return string.Empty; 
        }

        public static EnumItem<T> GetItem<T>(T enumItem) where T : struct
        {
            var field = typeof(T).GetField(enumItem.ToString());

            if (field == null)
            {
                return new EnumItem<T>(enumItem, enumItem.ToString());
            }

            object[] customAttributes = field.GetCustomAttributes(typeof(EnumAttribute), true);
            foreach (object attr in customAttributes)
            {
                EnumAttribute attribute = attr as EnumAttribute;

                if (attribute != null)
                {
                    T currentValue = (T)field.GetValue(null);

                    return new EnumItem<T>(currentValue, attribute.Desc);
                }
            }

            return new EnumItem<T>(enumItem, enumItem.ToString());
        }
    }
}
