using System;
using System.Collections.Generic;

namespace Jasen.Framework.Transform
{
    public class FuncProvider
    {
        public static IDictionary<Type, Delegate> Dictionary
        {
            get;
            private set;
        }

        static FuncProvider()
        {
            if (FuncProvider.Dictionary == null)
            {
                FuncProvider.Dictionary = CreateDictionary();
            }
        }

        public object DynamicInvoke(Type type, string arg)
        {
            if (type == null)
            {
                return null;
            }

            if (FuncProvider.Dictionary == null)
            {
                FuncProvider.Dictionary = CreateDictionary();
            }

            if (!FuncProvider.Dictionary.ContainsKey(type))
            {
                return null;
            }

            Delegate action = FuncProvider.Dictionary[type];

            return action.DynamicInvoke(new object[] { arg });
        }

        public static IDictionary<Type, Delegate> CreateDictionary()
        {
            var dictionary = new Dictionary<Type, Delegate>();

            dictionary.Add(typeof(string), new Func<string, string>(p => p));
            dictionary.Add(typeof(decimal), new Func<string, decimal>(p => p.AsDecimal()));
            dictionary.Add(typeof(DateTime), new Func<string, DateTime>(p => p.AsDateTime()));
            dictionary.Add(typeof(float), new Func<string, float>(p => p.AsFloat()));
            dictionary.Add(typeof(double), new Func<string, double>(p => p.AsDouble()));
            dictionary.Add(typeof(int), new Func<string, int>(p => p.AsInt()));
            dictionary.Add(typeof(byte), new Func<string, byte>(p => p.AsByte()));
            dictionary.Add(typeof(sbyte), new Func<string, sbyte>(p => p.AsSbyte()));
            dictionary.Add(typeof(short), new Func<string, short>(p => p.AsShort()));
            dictionary.Add(typeof(ushort), new Func<string, ushort>(p => p.AsUshort()));
            dictionary.Add(typeof(uint), new Func<string, uint>(p => p.AsUint()));
            dictionary.Add(typeof(long), new Func<string, long>(p => p.AsLong()));
            dictionary.Add(typeof(ulong), new Func<string, ulong>(p => p.AsUlong()));
            dictionary.Add(typeof(char), new Func<string, char>(p => p.AsChar()));
            dictionary.Add(typeof(bool), new Func<string, bool>(p => p.AsBool()));

            dictionary.Add(typeof(decimal?), new Func<string, decimal?>(p => p.AsNullableDecimal()));
            dictionary.Add(typeof(DateTime?), new Func<string, DateTime?>(p => p.AsNullableDateTime()));
            dictionary.Add(typeof(float?), new Func<string, float?>(p => p.AsNullableFloat()));
            dictionary.Add(typeof(double?), new Func<string, double?>(p => p.AsNullableDouble()));
            dictionary.Add(typeof(int?), new Func<string, int?>(p => p.AsNullableInt()));
            dictionary.Add(typeof(byte?), new Func<string, byte?>(p => p.AsNullableByte()));
            dictionary.Add(typeof(sbyte?), new Func<string, sbyte?>(p => p.AsNullableSbyte()));
            dictionary.Add(typeof(short?), new Func<string, short?>(p => p.AsNullableShort()));
            dictionary.Add(typeof(ushort?), new Func<string, ushort?>(p => p.AsNullableUshort()));
            dictionary.Add(typeof(uint?), new Func<string, uint?>(p => p.AsNullableUint()));
            dictionary.Add(typeof(long?), new Func<string, long?>(p => p.AsNullableLong()));
            dictionary.Add(typeof(ulong?), new Func<string, ulong?>(p => p.AsNullableUlong()));
            dictionary.Add(typeof(char?), new Func<string, char?>(p => p.AsNullableChar()));
            dictionary.Add(typeof(bool?), new Func<string, bool?>(p => p.AsNullableBool()));

            return dictionary;
        }
    }
}
