using System;
using System.Collections.Generic;
using System.Reflection;
using Jasen.Framework.Transform;

namespace Jasen.Framework.Transform.IL
{
    public class DynamicValueProvider<T> where T : class, new()
    {
        private readonly FuncProvider _transfer = new FuncProvider();
        private Func<T, object> _getter;
        private Action<T, object> _setter;
        private readonly Dictionary<string, Action<T, object>> _setterCache;
        private readonly Dictionary<string, Func<T, object>> _getterCache;

        public DynamicValueProvider()
        {
            _setterCache = new Dictionary<string, Action<T, object>>();
            _getterCache = new Dictionary<string, Func<T, object>>();
        }

        public void SetValue(T target, MemberInfo memberInfo, object value)
        {
            if (target == null)
            {
                throw new ArgumentNullException("target");
            }

            if (memberInfo == null)
            {
                throw new ArgumentNullException("memberInfo");
            }

            try
            {
                string key = string.Concat(memberInfo.ReflectedType.FullName, memberInfo.Name);
                if (_setterCache.ContainsKey(key))
                {
                    _setter = _setterCache[key];
                }
                else
                {
                    _setter = DynamicDelegateFactory.Instance.CreateSet<T>(memberInfo);
                    _setterCache.Add(key, _setter);
                }

                if (_setter == null)
                {
                    return;
                }

                Type memberType = ReflectionUtility.GetMemberUnderlyingType(memberInfo);

                if (value == null)
                {
                    if (!ReflectionUtility.IsNullable(memberType))
                    {
                        return;
                    }
                }
                else if (!memberType.IsAssignableFrom(value.GetType()) && !memberType.CanImplicitTransfer(value.GetType()))
                {
                    value = _transfer.DynamicInvoke(memberType, value.ToString());

                    if (value == null)
                    {
                        return;
                    }
                }
                
                _setter(target, value);
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("Error setting value to '{0}' on '{1}'.{2}", memberInfo.Name,
                                                  target.GetType().Name, ex.Message));
            }
        }

        public object GetValue(T target, MemberInfo memberInfo)
        {
            if (target == null)
            {
                throw new ArgumentNullException("target");
            }

            if (memberInfo == null)
            {
                throw new ArgumentNullException("memberInfo");
            }

            try
            {
                string key = string.Concat(memberInfo.ReflectedType.FullName, memberInfo.Name);
                if (_getterCache.ContainsKey(key))
                {
                    _getter = _getterCache[key];
                }
                else
                {
                    _getter = DynamicDelegateFactory.Instance.CreateGet<T>(memberInfo);
                    _getterCache.Add(key, _getter);
                }

                if (_getter == null)
                {
                    return null;
                }

                return _getter(target);
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("Error getting value to '{0}' on '{1}'.{2}", memberInfo.Name,
                                                  target.GetType().Name, ex.Message));
            }
        }
    }
}
