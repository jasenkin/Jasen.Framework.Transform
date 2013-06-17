using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Jasen.Framework.Transform
{
    public class DictionaryTransfer
    {
        public static List<T> Transfer<T>(Dictionary<string, object> dictionary) where T : new()
        {
            if (dictionary == null || dictionary.Keys.Count == 0)
            {
                return new List<T>();
            }

            var currDictionary = new Dictionary<string, ArrayList>();
            ArrayList array = null;
            object currValue = null;
            ICollection collection = null;

            foreach (string key in dictionary.Keys)
            {
                array = new ArrayList();
                currValue = dictionary[key];
                collection = currValue as ICollection;

                if (collection != null)
                {
                    array.AddRange(collection);
                }

                currDictionary.Add(key, array);
            }

            return Transfer<T>(currDictionary);
        }

        public static List<T> Transfer<T>(Dictionary<string, ArrayList> dictionary) where T : new()
        {
            if (dictionary == null || dictionary.Keys.Count == 0)
            {
                return new List<T>();
            }

            IList<T> entities = null;
            int count = 0;

            foreach (string key in dictionary.Keys)
            {
                if (dictionary[key].Count > count)
                {
                    count = dictionary[key].Count;
                }
            }

            entities = CreateInstance<T>(count);
            object currentValue = null;
            PropertyInfo property = null;
            var properties = typeof(T).GetProperties();

            foreach (string key in dictionary.Keys)
            {
                property =
                    properties.FirstOrDefault(p => string.Equals(p.Name, key, StringComparison.CurrentCultureIgnoreCase));

                if (property == null || !property.CanWrite)
                {
                    continue;
                }

                int index = 0;
                foreach (object value in dictionary[key])
                {
                    currentValue = value;
                    if (value.GetType() != property.PropertyType)
                    {
                        currentValue = new FuncProvider().DynamicInvoke(property.PropertyType, value.ToString());
                    }

                    property.SetValue(entities[index], currentValue, null);
                    index++;
                }
            }

            return entities.ToList();
        }

        public static IList<T> Transfer<T>(IDictionary<string, ICollection> dictionary,
            bool ignoreCase = true, bool isMax = true) where T : new()
        {
            if (dictionary == null || dictionary.Keys.Count == 0)
            {
                return new List<T>();
            }

            int count = CalculateValueCount(dictionary, isMax);

            if (count <= 0)
            {
                return new List<T>();
            }

            return TransferValue<T>(dictionary, count, ignoreCase);
        }

        private static IList<T> TransferValue<T>(IDictionary<string, ICollection> dictionary,
            int count, bool ignoreCase) where T : new()
        {
            IList<T> entities = CreateInstance<T>(count);

            foreach (string key in dictionary.Keys)
            {
                ResetEntityValue(dictionary, ignoreCase, key, entities);
            }

            return entities;
        }

        private static void ResetEntityValue<T>(IDictionary<string, ICollection> dictionary,
            bool ignoreCase, string key, IList<T> entities)
        {
            PropertyInfo property = GetProperty<T>(ignoreCase, key);
            bool isClass = typeof(T).IsClass;

            if (property == null || !property.CanWrite)
            {
                return;
            }

            int index = 0;
            object currentValue;
            object entity = null;
            bool canImplicitTransfer = false;
            bool isLoaded = false;

            foreach (object value in dictionary[key])
            {
                currentValue = value;

                if (currentValue == null)
                {
                    index++;
                    continue;
                }

                if (!isLoaded)
                {
                    canImplicitTransfer = TransferTable.CanImplicitTransfer(value.GetType(), property.PropertyType);
                    isLoaded = true;
                }

                if (value.GetType() != property.PropertyType && !canImplicitTransfer)
                {
                    currentValue = new FuncProvider().DynamicInvoke(property.PropertyType, value.ToString());
                }

                if (isClass)
                {
                    property.SetValue(entities[index], currentValue, null);
                }
                else
                {
                    entity = entities[index];
                    property.SetValue(entity, currentValue, null);
                    entities[index] = (T)entity;
                }

                index++;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ignoreCase"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        private static PropertyInfo GetProperty<T>(bool ignoreCase, string key)
        {
            if (!ignoreCase)
            {
                return typeof(T).GetProperty(key.Trim());
            }
            else
            {
                return typeof(T).GetProperty(key.Trim(), BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dictionary"></param>
        /// <param name="isMax"></param>
        /// <returns></returns>
        private static int CalculateValueCount(IDictionary<string, ICollection> dictionary, bool isMax = true)
        {
            if (dictionary == null || dictionary.Keys.Count == 0)
            {
                return 0;
            }

            int count = 0;
            ICollection collection = null;
            int currentIndex = 0;

            foreach (string key in dictionary.Keys)
            {
                collection = dictionary[key];

                if (collection == null)
                {
                    if (!isMax)
                    {
                        return count = 0;
                    }

                    continue;
                }

                if (currentIndex == 0)
                {
                    count = collection.Count;
                    currentIndex++;
                    continue;
                }

                if (isMax && collection.Count > count)
                {
                    count = collection.Count;
                }

                if (!isMax && collection.Count < count)
                {
                    count = collection.Count;
                }

                currentIndex++;
            }

            return count;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="count"></param>
        /// <returns></returns>
        private static IList<T> CreateInstance<T>(int count) where T : new()
        {
            IList<T> entities = new List<T>();

            for (int index = 0; index < count; index++)
            {
                entities.Add(Activator.CreateInstance<T>());
            }

            return entities;
        }
    }
}
