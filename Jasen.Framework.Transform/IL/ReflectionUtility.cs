using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Jasen.Framework.Transform.IL
{
    public static class ReflectionUtility
    {
        public static readonly Type[] EmptyTypes;

        static ReflectionUtility()
        {
            EmptyTypes = Type.EmptyTypes;
        }

        public static PropertyInfo GetProperty<T>(string key, bool ignoreCase = false)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                return null;
            }

            key = key.Trim();

            if (!ignoreCase)
            {
                return typeof(T).GetProperty(key.Trim());
            }
            else
            {
                return typeof(T).GetProperty(key.Trim(), BindingFlags.IgnoreCase
                               | BindingFlags.Public | BindingFlags.Instance);
            }
        }

        public static bool IsVirtual(this PropertyInfo propertyInfo)
        {
            if (propertyInfo == null)
            {
                throw new ArgumentNullException("propertyInfo");
            }

            MethodInfo method = propertyInfo.GetGetMethod();
            if (method != null && method.IsVirtual)
            {
                return true;
            }

            method = propertyInfo.GetSetMethod();
            if (method != null && method.IsVirtual)
            {
                return true;
            }

            return false;
        }

        public static bool HasDefaultConstructor(Type type)
        {
            return HasDefaultConstructor(type, false);
        }

        public static bool HasDefaultConstructor(Type type, bool nonPublic)
        {
            if (type == null)
            {
                throw new ArgumentNullException("type");
            }

            if (type.IsValueType)
            {
                return true;
            }

            return (GetDefaultConstructor(type, nonPublic) != null);
        }

        public static ConstructorInfo GetDefaultConstructor(Type type)
        {
            return GetDefaultConstructor(type, false);
        }

        public static ConstructorInfo GetDefaultConstructor(Type type, bool nonPublic)
        {
            if (type == null)
            {
                throw new ArgumentNullException("type");
            }

            BindingFlags bindingFlags = BindingFlags.Instance;

            if (!nonPublic)
            {
                bindingFlags = bindingFlags | BindingFlags.Public;
            }
            else
            {
                bindingFlags = bindingFlags | BindingFlags.NonPublic;
            }

            return type.GetConstructors(bindingFlags).SingleOrDefault(c => !c.GetParameters().Any());
        }

        public static bool IsNullable(Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException("type");
            }

            if (type.IsValueType)
            {
                return IsNullableType(type);
            }

            return true;
        }

        public static bool IsNullableType(Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException("type");
            }

            return (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>));
        }

        public static bool ImplementsGenericDefinition(Type type, Type genericInterfaceDefinition)
        {
            Type implementingType;
            return ImplementsGenericDefinition(type, genericInterfaceDefinition, out implementingType);
        }

        public static bool ImplementsGenericDefinition(Type type, Type genericInterfaceDefinition, out Type implementingType)
        {
            if (type == null)
            {
                throw new ArgumentNullException("type");
            }

            if (genericInterfaceDefinition == null)
            {
                throw new ArgumentNullException("genericInterfaceDefinition");
            }

            if (!genericInterfaceDefinition.IsInterface || !genericInterfaceDefinition.IsGenericTypeDefinition)
            {
                throw new ArgumentException("genericInterfaceDefinition is not a generic interface definition.");
            }

            if (type.IsInterface)
            {
                if (type.IsGenericType)
                {
                    Type interfaceDefinition = type.GetGenericTypeDefinition();

                    if (genericInterfaceDefinition == interfaceDefinition)
                    {
                        implementingType = type;
                        return true;
                    }
                }
            }

            foreach (Type i in type.GetInterfaces())
            {
                if (i.IsGenericType)
                {
                    Type interfaceDefinition = i.GetGenericTypeDefinition();

                    if (genericInterfaceDefinition == interfaceDefinition)
                    {
                        implementingType = i;
                        return true;
                    }
                }
            }

            implementingType = null;
            return false;
        }

        public static bool InheritsGenericDefinition(Type type, Type genericClassDefinition)
        {
            Type implementingType;
            return InheritsGenericDefinition(type, genericClassDefinition, out implementingType);
        }

        public static bool InheritsGenericDefinition(Type type, Type genericClassDefinition, out Type implementingType)
        {           
            if (!genericClassDefinition.IsClass || !genericClassDefinition.IsGenericTypeDefinition)
            {
                throw new ArgumentNullException(string.Format("'{0}' is not a generic class definition.",genericClassDefinition));
            }

            return InheritsGenericDefinitionInternal(type, genericClassDefinition, out implementingType);
        }

        private static bool InheritsGenericDefinitionInternal(Type currentType, Type genericClassDefinition, out Type implementingType)
        {
            if (currentType.IsGenericType)
            {
                Type currentGenericClassDefinition = currentType.GetGenericTypeDefinition();

                if (genericClassDefinition == currentGenericClassDefinition)
                {
                    implementingType = currentType;
                    return true;
                }
            }

            if (currentType.BaseType == null)
            {
                implementingType = null;
                return false;
            }

            return InheritsGenericDefinitionInternal(currentType.BaseType, genericClassDefinition, out implementingType);
        }

        public static void GetDictionaryKeyValueTypes(Type dictionaryType, out Type keyType, out Type valueType)
        {
            if(dictionaryType==null)
            {
               
            }

            Type genericDictionaryType;
            if (ImplementsGenericDefinition(dictionaryType, typeof(IDictionary<,>), out genericDictionaryType))
            {
                if (genericDictionaryType.IsGenericTypeDefinition)
                {
                    throw new ArgumentException();
                }

                Type[] dictionaryGenericArguments = genericDictionaryType.GetGenericArguments();

                keyType = dictionaryGenericArguments[0];
                valueType = dictionaryGenericArguments[1];
                return;
            }
            else if (typeof(IDictionary).IsAssignableFrom(dictionaryType))
            {
                keyType = null;
                valueType = null;
                return;
            }
            else
            {
                throw new ArgumentException("Type {0} is not a dictionary."); 
            }
        }

        public static Type GetDictionaryValueType(Type dictionaryType)
        {
            Type keyType;
            Type valueType;
            GetDictionaryKeyValueTypes(dictionaryType, out keyType, out valueType);

            return valueType;
        }

        public static Type GetDictionaryKeyType(Type dictionaryType)
        {
            Type keyType;
            Type valueType;
            GetDictionaryKeyValueTypes(dictionaryType, out keyType, out valueType);

            return keyType;
        }

        public static Type GetMemberUnderlyingType(MemberInfo member)
        {
            if (member == null)
            {
                throw new ArgumentNullException("member");
            }

            switch (member.MemberType)
            {
                case MemberTypes.Field:
                    return ((FieldInfo) member).FieldType;
                case MemberTypes.Property:
                    return ((PropertyInfo) member).PropertyType;
                case MemberTypes.Event:
                    return ((EventInfo) member).EventHandlerType;
                default:
                    return null;
            }
        }

        public static bool CanReadMemberValue(MemberInfo member, bool nonPublic)
        {
            switch (member.MemberType)
            {
                case MemberTypes.Field:
                    FieldInfo fieldInfo = (FieldInfo)member;
                    return nonPublic || fieldInfo.IsPublic;
                case MemberTypes.Property:
                    PropertyInfo propertyInfo = (PropertyInfo)member;
                    return propertyInfo.CanRead || nonPublic || (propertyInfo.GetGetMethod(nonPublic) != null);
                default:
                    return false;
            }
        }

        public static bool CanSetMemberValue(MemberInfo member, bool nonPublic, bool canSetReadOnly)
        {
            switch (member.MemberType)
            {
                case MemberTypes.Field:
                    FieldInfo fieldInfo = (FieldInfo)member;

                    if (fieldInfo.IsInitOnly && !canSetReadOnly)
                    {
                        return false;
                    }
                    if (nonPublic)
                    { 
                        return true;
                    }
                    else if (fieldInfo.IsPublic)
                    {
                        return true;
                    }

                    return false;
                case MemberTypes.Property:
                    PropertyInfo propertyInfo = (PropertyInfo)member;

                    if (!propertyInfo.CanWrite)
                    {
                        return false;
                    }
                    if (nonPublic)
                    {
                        return true;
                    }
                    return (propertyInfo.GetSetMethod(nonPublic) != null);
                default:
                    return false;
            }
        }

        public static BindingFlags RemoveFlag(this BindingFlags bindingAttr, BindingFlags flag)
        {
            return ((bindingAttr & flag) == flag) ? bindingAttr ^ flag : bindingAttr;
        }
    }
}
