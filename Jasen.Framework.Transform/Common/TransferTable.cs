using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jasen.Framework.Transform
{
    /// <summary>
    /// 隐式转换表如下：
    /// sbyte  short、int、long、float、double 或 decimal
    /// byte   short、ushort、int、uint、long、ulong、float、double 或 decimal
    /// short  int、long、float、double 或 decimal
    /// ushort int、uint、long、ulong、float、double 或 decimal
    /// int    long、float、double 或 decimal
    /// uint   long、ulong、float、double 或 decimal
    /// long   float、double 或 decimal
    /// char   ushort、int、 uint、 long、ulong、 float、double 或 decimal
    /// float  double
    /// ulong  float、 double 或 decimal
    /// </summary>
    public class TransferTable
    {
        public static Dictionary<Type, IList<Type>> TransferDictionary
        {
            get;
            private set;
        }

        static TransferTable()
        {
            if (TransferDictionary == null || TransferDictionary.Count == 0)
            {
                CreateDictionary();
            }
        }

        public static bool CanImplicitTransfer(Type originalType, Type sourceType)
        {
            if (originalType == null || sourceType == null)
            {
                return false;
            }

            if (!TransferDictionary.ContainsKey(originalType) || TransferDictionary[originalType] == null)
            {
                return false;
            }

            return TransferDictionary[originalType].Contains(sourceType);
        }

        private static void CreateDictionary()
        {
            Type sbyteType = typeof(sbyte);
            Type shortType = typeof(short);
            Type intType = typeof(int);
            Type floatType = typeof(float);
            Type longType = typeof(long);
            Type doubleType = typeof(double);
            Type decimalType = typeof(decimal);
            Type byteType = typeof(byte);
            Type ushortType = typeof(ushort);
            Type uintType = typeof(uint);
            Type ulongType = typeof(ulong);
            Type charType = typeof(char);

            TransferDictionary = new Dictionary<Type, IList<Type>>();
            TransferDictionary.Add(sbyteType,
                                   new List<Type>() { shortType, intType, floatType, longType, doubleType, decimalType });
            TransferDictionary.Add(byteType,
                                   new List<Type>() { shortType, ushortType, intType, uintType, longType, ulongType, floatType, doubleType, decimalType });
            TransferDictionary.Add(shortType,
                                   new List<Type>() { intType, longType, floatType, doubleType, decimalType });
            TransferDictionary.Add(ushortType,
                                   new List<Type>() { intType, uintType, longType, ulongType, floatType, doubleType, decimalType });
            TransferDictionary.Add(intType,
                                   new List<Type>() { longType, floatType, doubleType, decimalType });
            TransferDictionary.Add(uintType,
                                   new List<Type>() { longType, ulongType, floatType, doubleType, decimalType });
            TransferDictionary.Add(longType,
                                   new List<Type>() { floatType, doubleType, decimalType });
            TransferDictionary.Add(charType,
                                   new List<Type>() { ushortType, intType, uintType, longType, ulongType, floatType, doubleType, decimalType });
            TransferDictionary.Add(floatType,
                                   new List<Type>() { doubleType });
            TransferDictionary.Add(ulongType,
                                   new List<Type>() { floatType, doubleType, decimalType });
        }

    }
}
