﻿using System;
using System.Drawing;
using System.Text;
using System.Globalization;

namespace Jasen.Framework.Transform
{
    public static class StringExtension
    {   
        public static Color ToColor(this string argb)
        { 
            string[] split = argb.Split(',');

            if (split.Length != 4 || split.Length != 3)
            {
                throw new ArgumentException("Invalid Value for Color");
            }

            if (split.Length == 3)
            {
                return Color.FromArgb(split[0].AsInt(), split[1].AsInt(), split[2].AsInt());
            }

            return Color.FromArgb(split[0].AsInt(), split[1].AsInt(), split[2].AsInt(), split[3].AsInt());
        }

        public static decimal? AsNullableDecimal(this string inputValue)
        {
            if(string.IsNullOrWhiteSpace(inputValue))
            {
                return null;
            }

            return inputValue.AsDecimal();
        }

        public static decimal AsDecimal(this string inputValue)
        {
            decimal result;
            if (!decimal.TryParse(inputValue, out result))
            {
                result = 0;
            }
            return result;
        }

        public static DateTime? AsNullableDateTime(this string inputValue)
        {
            if(string.IsNullOrWhiteSpace(inputValue))
            {
                return null;
            }

            return inputValue.AsDateTime();
        }

        public static DateTime AsDateTime(this string inputValue)
        {
            DateTime result;
            
            if (!DateTime.TryParse(inputValue, out result))
            {
                result = DateTime.MinValue;
            }
            return result;
        }

        public static string AsDateTimeString(this string inputValue, string format)
        {
            DateTime result;

            if (!DateTime.TryParse(inputValue, out result))
            {
                return string.Empty;
            }

            return result.ToString(format);
        }

        public static float? AsNullableFloat(this string inputValue)
        {
            if (string.IsNullOrWhiteSpace(inputValue))
            {
                return null;
            }

            return inputValue.AsFloat();
        }

        public static float AsFloat(this string inputValue)
        {
            float result;
            if (!float.TryParse(inputValue, out result))
            {
                result = 0;
            }
            return result;
        }

        public static double? AsNullableDouble(this string inputValue)
        {
            if (string.IsNullOrWhiteSpace(inputValue))
            {
                return null;
            }

            return inputValue.AsDouble();
        }

        public static double AsDouble(this string inputValue)
        {
            double result;
            if (!double.TryParse(inputValue, out result))
            {
                result = 0;
            }
            return result;
        }

        public static int? AsNullableInt(this string inputValue)
        {
            if (string.IsNullOrWhiteSpace(inputValue))
            {
                return null;
            }

            return inputValue.AsInt();
        }

        public static int AsInt(this string inputValue)
        {
            int result;
            if (!int.TryParse(inputValue, out result))
            {
                result = 0;
            }
            return result;
        }

        public static byte? AsNullableByte(this string inputValue)
        {
            if (string.IsNullOrWhiteSpace(inputValue))
            {
                return null;
            }

            return inputValue.AsByte();
        }

        public static byte AsByte(this string inputValue)
        {
            byte result;
            if (!byte.TryParse(inputValue, out result))
            {
                result = 0;
            }
            return result;
        }

        public static sbyte? AsNullableSbyte(this string inputValue)
        {
            if (string.IsNullOrWhiteSpace(inputValue))
            {
                return null;
            }

            return inputValue.AsSbyte();
        }

        public static sbyte AsSbyte(this string inputValue)
        {
            sbyte result;
            if (!sbyte.TryParse(inputValue, out result))
            {
                result = 0;
            }
            return result;
        }

        public static short? AsNullableShort(this string inputValue)
        {
            if (string.IsNullOrWhiteSpace(inputValue))
            {
                return null;
            }

            return inputValue.AsShort();
        }

        public static short AsShort(this string inputValue)
        {
            short result;
            if (!short.TryParse(inputValue, out result))
            {
                result = 0;
            }
            return result;
        }

        public static ushort? AsNullableUshort(this string inputValue)
        {
            if (string.IsNullOrWhiteSpace(inputValue))
            {
                return null;
            }

            return inputValue.AsUshort();
        }

        public static ushort AsUshort(this string inputValue)
        {
            ushort result;
            if (!ushort.TryParse(inputValue, out result))
            {
                result = 0;
            }
            return result;
        }

        public static uint? AsNullableUint(this string inputValue)
        {
            if (string.IsNullOrWhiteSpace(inputValue))
            {
                return null;
            }

            return inputValue.AsUint();
        }

        public static uint AsUint(this string inputValue)
        {
            uint result;
            if (!uint.TryParse(inputValue, out result))
            {
                result = 0;
            }
            return result;
        }

        public static long? AsNullableLong(this string inputValue)
        {
            if (string.IsNullOrWhiteSpace(inputValue))
            {
                return null;
            }

            return inputValue.AsLong();
        }

        public static long AsLong(this string inputValue)
        {
            long result;
            if (!long.TryParse(inputValue, out result))
            {
                result = 0;
            }
            return result;
        }

        public static ulong? AsNullableUlong(this string inputValue)
        {
            if (string.IsNullOrWhiteSpace(inputValue))
            {
                return null;
            }

            return inputValue.AsUlong();
        }

        public static ulong AsUlong(this string inputValue)
        {
            ulong result;
            if (!ulong.TryParse(inputValue, out result))
            {
                result = 0;
            }
            return result;
        }

        public static char? AsNullableChar(this string inputValue)
        {
            if (string.IsNullOrWhiteSpace(inputValue))
            {
                return null;
            }

            return inputValue.AsChar();
        }

        public static char AsChar(this string inputValue)
        {
            char result;
            if (!char.TryParse(inputValue, out result))
            {
                result = '\0';
            }
            return result;
        }

        public static bool? AsNullableBool(this string inputValue)
        {
            if (string.IsNullOrWhiteSpace(inputValue))
            {
                return null;
            }

            return inputValue.AsBool();
        }

        public static bool AsBool(this string inputValue)
        {
            bool result;
            if (!bool.TryParse(inputValue, out result))
            {
                result = false;
            }
            return result;
        }
    }
}
