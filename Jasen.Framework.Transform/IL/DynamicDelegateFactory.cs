using System;
using System.Reflection;
using System.Reflection.Emit;

namespace Jasen.Framework.Transform.IL
{
    public delegate TResult MethodCall<T, TResult>(T target, params object[] args);

    public class DynamicDelegateFactory 
    {
        public static readonly DynamicDelegateFactory Instance = new DynamicDelegateFactory();

        public Func<T, object> CreateGet<T>(MemberInfo memberInfo)
        {
            if (memberInfo == null)
            {
                return null;
            }

            PropertyInfo propertyInfo = memberInfo as PropertyInfo;
            if (propertyInfo != null)
            {
                return CreateGet<T>(propertyInfo);
            }

            FieldInfo fieldInfo = memberInfo as FieldInfo;
            if (fieldInfo != null)
            {
                return CreateGet<T>(fieldInfo);
            }

            return null;
        }

        public Action<T, object> CreateSet<T>(MemberInfo memberInfo)
        {
            if (memberInfo == null)
            {
                return null;
            }

            PropertyInfo propertyInfo = memberInfo as PropertyInfo;
            if (propertyInfo != null)
            {
                return CreateSet<T>(propertyInfo);
            }

            FieldInfo fieldInfo = memberInfo as FieldInfo;
            if (fieldInfo != null)
            {
                return CreateSet<T>(fieldInfo);
            }

            return null;
        }

        public MethodCall<T, object> CreateMethodCall<T>(MethodBase method)
        {
            if (method == null)
            {
                return null;
            }

            DynamicMethod dynamicMethod = CreateDynamicMethod(method.ToString(), typeof(object),
                                                              new[] { typeof(object), typeof(object[]) },
                                                              method.DeclaringType);
            ILGenerator generator = dynamicMethod.GetILGenerator();
            InvokeMethod(method, generator);

            return dynamicMethod.CreateDelegate(typeof(MethodCall<T, object>)) as MethodCall<T, object>;
        }

        public static void InvokeMethod(MethodBase method, ILGenerator generator)
        {
            if (method == null || generator == null)
            {
                return;
            }

            ParameterInfo[] args = method.GetParameters();

            Label argsOk = generator.DefineLabel();

            generator.Emit(OpCodes.Ldarg_1);
            generator.Emit(OpCodes.Ldlen);
            generator.Emit(OpCodes.Ldc_I4, args.Length);
            generator.Emit(OpCodes.Beq, argsOk);

            generator.Emit(OpCodes.Newobj, typeof(TargetParameterCountException).GetConstructor(ReflectionUtility.EmptyTypes));
            generator.Emit(OpCodes.Throw);

            generator.MarkLabel(argsOk);

            if (!method.IsConstructor && !method.IsStatic)
            {
                generator.LoadInstance(method.DeclaringType);
            }

            for (int i = 0; i < args.Length; i++)
            {
                generator.Emit(OpCodes.Ldarg_1);
                generator.Emit(OpCodes.Ldc_I4, i);
                generator.Emit(OpCodes.Ldelem_Ref);

                generator.Unbox(args[i].ParameterType);
            }

            if (method.IsConstructor)
            {
                generator.Emit(OpCodes.Newobj, (ConstructorInfo)method);
            }
            else if (method.IsFinal || !method.IsVirtual)
            {
                generator.CallMethod((MethodInfo)method);
            }

            Type returnType = method.IsConstructor
                                ? method.DeclaringType
                                : ((MethodInfo)method).ReturnType;

            if (returnType != typeof(void))
            {
                generator.Box(returnType);
            }
            else
            {
                generator.Emit(OpCodes.Ldnull);
            }

            generator.Return();
        }

        public Func<T> CreateDefaultConstructor<T>(Type type)
        {
            if (type == null)
            {
                return null;
            }

            DynamicMethod dynamicMethod = CreateDynamicMethod("Create" + type.FullName, typeof(T),
                                                              ReflectionUtility.EmptyTypes, type);
            dynamicMethod.InitLocals = true;
            ILGenerator generator = dynamicMethod.GetILGenerator();
            InvokeDefaultConstructor(type, generator);

            return dynamicMethod.CreateDelegate(typeof(Func<T>)) as Func<T>;
        }

        public static void InvokeDefaultConstructor(Type type, ILGenerator generator)
        {
            if (type == null || generator == null)
            {
                return;
            }

            if (type.IsValueType)
            {
                generator.DeclareLocal(type);
                generator.Emit(OpCodes.Ldloc_0);
                generator.Emit(OpCodes.Box, type);
            }
            else
            {
                ConstructorInfo constructorInfo =
                    type.GetConstructor(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance, null,
                                        ReflectionUtility.EmptyTypes, null);

                if (constructorInfo == null)
                {
                    return;
                }

                generator.Emit(OpCodes.Newobj, constructorInfo);
            }

            generator.Return();
        }

        public Func<T, object> CreateGet<T>(PropertyInfo propertyInfo)
        {
            if (propertyInfo == null)
            {
                return null;
            }

            DynamicMethod dynamicMethod = CreateDynamicMethod("Get" + propertyInfo.Name, typeof(T), new[] { typeof(object) }, propertyInfo.DeclaringType);
            ILGenerator generator = dynamicMethod.GetILGenerator();
            InvokeGetMethod(propertyInfo, generator);

            return dynamicMethod.CreateDelegate(typeof(Func<T, object>)) as Func<T, object>;
        }

        public static void InvokeGetMethod(PropertyInfo propertyInfo, ILGenerator generator)
        {
            if (propertyInfo == null || generator == null)
            {
                return;
            }

            MethodInfo getMethod = propertyInfo.GetGetMethod(true);
            if (getMethod == null)
            {
                return;
            }

            if (!getMethod.IsStatic)
            {
                generator.LoadInstance(propertyInfo.DeclaringType);
            }

            generator.CallMethod(getMethod);
            generator.Box(propertyInfo.PropertyType);
            generator.Return();
        }

        public Func<T, object> CreateGet<T>(FieldInfo fieldInfo)
        {
            if (fieldInfo == null)
            {
                return null;
            }

            DynamicMethod dynamicMethod = CreateDynamicMethod("Get" + fieldInfo.Name, typeof(T),
                                                              new[] { typeof(object) }, fieldInfo.DeclaringType);
            ILGenerator generator = dynamicMethod.GetILGenerator();
            InvokeGetMethod(fieldInfo, generator);

            return dynamicMethod.CreateDelegate(typeof(Func<T, object>)) as Func<T, object>;
        }

        public static void InvokeGetMethod(FieldInfo fieldInfo, ILGenerator generator)
        {
            if (fieldInfo == null || generator == null)
            {
                return;
            }

            if (!fieldInfo.IsStatic)
            {
                generator.LoadInstance(fieldInfo.DeclaringType);
            }

            generator.Emit(OpCodes.Ldfld, fieldInfo);
            generator.Box(fieldInfo.FieldType);
            generator.Return();
        }

        public Action<T, object> CreateSet<T>(FieldInfo fieldInfo)
        {
            if (fieldInfo == null)
            {
                return null;
            }

            DynamicMethod dynamicMethod = CreateDynamicMethod("Set" + fieldInfo.Name, null,
                                                              new[] { typeof(T), typeof(object) },
                                                              fieldInfo.DeclaringType);
            ILGenerator generator = dynamicMethod.GetILGenerator();
            InvokeSetMethod(fieldInfo, generator);

            return dynamicMethod.CreateDelegate(typeof(Action<T, object>)) as Action<T, object>;
        }

        public static void InvokeSetMethod(FieldInfo fieldInfo, ILGenerator generator)
        {
            if (fieldInfo == null || generator == null)
            {
                return;
            }

            if (!fieldInfo.IsStatic)
            {
                generator.LoadInstance(fieldInfo.DeclaringType);
            }

            generator.Emit(OpCodes.Ldarg_1);
            generator.Unbox(fieldInfo.FieldType);
            generator.Emit(OpCodes.Stfld, fieldInfo);
            generator.Return();
        }

        public Action<T, object> CreateSet<T>(PropertyInfo propertyInfo)
        {
            if (propertyInfo == null)
            {
                return null;
            }

            DynamicMethod dynamicMethod = CreateDynamicMethod("Set" + propertyInfo.Name, null,
                                                              new[] { typeof(T), typeof(object) },
                                                              propertyInfo.DeclaringType);
            ILGenerator generator = dynamicMethod.GetILGenerator();
            InvokeSetMethod(propertyInfo, generator);

            return dynamicMethod.CreateDelegate(typeof(Action<T, object>)) as Action<T, object>;
        }

        public static void InvokeSetMethod(PropertyInfo propertyInfo, ILGenerator generator)
        {
            if (propertyInfo == null || generator == null)
            {
                return;
            }

            MethodInfo setMethod = propertyInfo.GetSetMethod(true);

            if (setMethod == null)
            {
                return;
            }

            if (!setMethod.IsStatic)
            {
                generator.LoadInstance(propertyInfo.DeclaringType);
            }

            generator.Emit(OpCodes.Ldarg_1);
            generator.Unbox(propertyInfo.PropertyType);
            generator.CallMethod(setMethod);
            generator.Return();
        }

        private static DynamicMethod CreateDynamicMethod(string name, Type returnType, Type[] parameterTypes, Type owner)
        {
            DynamicMethod dynamicMethod = !owner.IsInterface
              ? new DynamicMethod(name, returnType, parameterTypes, owner, true)
              : new DynamicMethod(name, returnType, parameterTypes, owner.Module, true);

            return dynamicMethod;
        }
    }
}