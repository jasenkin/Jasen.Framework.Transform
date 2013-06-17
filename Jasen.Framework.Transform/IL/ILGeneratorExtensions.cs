using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;

namespace Jasen.Framework.Transform.IL
{
    public static class ILGeneratorExtensions
    {
        public static void LoadInstance(this ILGenerator generator, Type type)
        {
            if (generator == null || type == null)
            {
                return;
            }

            generator.Emit(OpCodes.Ldarg_0);

            if (type.IsValueType)
            {
                generator.Emit(OpCodes.Unbox, type);
            }
            else
            {
                generator.Emit(OpCodes.Castclass, type);
            }
        }

        public static void Box(this ILGenerator generator, Type type)
        {
            if (generator == null || type == null)
            {
                return;
            }

            if (type.IsValueType)
            {
                generator.Emit(OpCodes.Box, type);
            }
            else
            {
                generator.Emit(OpCodes.Castclass, type);
            }
        }

        public static void Unbox(this ILGenerator generator, Type type)
        {
            if (generator == null || type == null)
            {
                return;
            }

            if (type.IsValueType)
            {
                generator.Emit(OpCodes.Unbox_Any, type);
            }
            else
            {
                generator.Emit(OpCodes.Castclass, type);
            }
        }

        public static void CallMethod(this ILGenerator generator, MethodInfo methodInfo)
        {
            if (generator == null || methodInfo == null)
            {
                return;
            }

            if (methodInfo.IsFinal || !methodInfo.IsVirtual)
            {
                generator.Emit(OpCodes.Call, methodInfo);
            }
            else
            {
                generator.Emit(OpCodes.Callvirt, methodInfo);
            }
        }

        public static void Return(this ILGenerator generator)
        {
            if(generator == null)
            {
                return;
            }

            generator.Emit(OpCodes.Ret);
        }
    }
}
