using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Jasen.Framework.Transform
{
    /// <summary>
    /// 
    /// </summary>
    public class DataMemberAttributeCollection : IEnumerable<DataMemberAttribute>
    {
        private IList<DataMemberAttribute> _memberAttributes = new List<DataMemberAttribute>();

        public DataMemberAttributeCollection(Type type, params string[] propertyNames)
        {
            this.GetConfiguration(type, propertyNames);
        }

        public IEnumerator<DataMemberAttribute> GetEnumerator()
        {
            return this._memberAttributes.GetEnumerator();
        }

        public DataMemberAttribute this[int index]
        {
            get
            {
                if (index < 0 || index > this.Count - 1)
                {
                    throw new IndexOutOfRangeException();
                }

                return this._memberAttributes[index];
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        public void GetConfiguration(Type type, params string[] propertyNames)
        {
            if (type == null || type.GetProperties().Length <= 0)
            {
                return;
            }

            if (propertyNames == null || propertyNames.Length == 0)
            {
                AddAllDataMemberAttributes(type);
            }
            else
            {
                AddDataMemberAttributes(type, propertyNames);
            }

            this._memberAttributes = this._memberAttributes.OrderBy(p => p.Order).ToList();
        }

        private void AddDataMemberAttributes(Type type, string[] propertyNames)
        {
            IList<PropertyInfo> validPropertyInfos = new List<PropertyInfo>();
            PropertyInfo tempPropertyInfo;

            foreach (string propertyName in propertyNames)
            {
                if (string.IsNullOrWhiteSpace(propertyName))
                {
                    continue;
                }

                tempPropertyInfo = type.GetProperty(propertyName.Trim());

                if (tempPropertyInfo == null)
                {
                    throw new ArgumentException(string.Format(@"Contains Invalid Property Name Arg : {0}.", propertyName.Trim()));
                }

                validPropertyInfos.Add(tempPropertyInfo);
            }

            if (validPropertyInfos.Count() > 0)
            {
                foreach (var property in validPropertyInfos)
                {
                    AddAttributes(new DataMemberAttribute(), property);
                }
            }
        }

        private void AddAllDataMemberAttributes(Type type)
        {
            DataMemberAttribute attr = null;
            foreach (PropertyInfo propertyInfo in type.GetProperties())
            {
                attr = AttributeUtility.GetCustomAttribute<DataMemberAttribute>(propertyInfo);

                if (attr == null)
                {
                    continue;
                }

                if (!attr.IsRequire)
                {
                    continue;
                }

                if (this._memberAttributes.Count(p => p.Order == attr.Order) > 0)
                {
                    throw new ArgumentException(string.Format(@"Contains Same Order {0}.Please Look Up DataMemberAttribute
                            Of The Type {1}", attr.Order, type.Name));
                }

                AddAttributes(attr, propertyInfo);
            }
        }

        private void AddAttributes(DataMemberAttribute attr, PropertyInfo propertyInfo)
        {
            if (string.IsNullOrWhiteSpace(attr.Name))
            {
                attr.Name = propertyInfo.Name;
            }

            attr.PropertyName = propertyInfo.Name;
            attr.PropertyType = propertyInfo.PropertyType;
            attr.PropertyInfo = propertyInfo;

            this._memberAttributes.Add(attr);
        }

        /// <summary>
        /// 
        /// </summary>
        public int Count
        {
            get
            {
                return this._memberAttributes.Count;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this._memberAttributes.GetEnumerator();
        }
    }
}
