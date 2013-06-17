using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jasen.Framework.Transform
{
    public static class ObjectBuilder
    {
        public static List<T> CreateInstances<T>(int count)
        {
            List<T> items = new List<T>();
            for (int i = 0; i < count; i++)
            {
                items.Add(CreateInstance<T>());
            }
            return items;
        }

        public static T CreateInstance<T>()
        {
            T item;
            item = System.Activator.CreateInstance<T>();
            return item;
        }

        public static object[] CreateInstances(Type type, int count)
        {
            object[] items = new object[count];
            for (int i = 0; i < items.Length; i++)
            {
                items[i] = CreateInstance(type);
            }
            return items;
        }

        public static object CreateInstance(Type type)
        {
            return System.Activator.CreateInstance(type);
        }

    }
}
