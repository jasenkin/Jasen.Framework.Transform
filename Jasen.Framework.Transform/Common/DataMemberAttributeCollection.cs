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
        private IList<DataMemberAttribute> _columnAttributes = new List<DataMemberAttribute>();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        public DataMemberAttributeCollection(Type type)
        {
            this.GetConfiguration(type);
        }

        public IEnumerator<DataMemberAttribute> GetEnumerator()
        {
            return this._columnAttributes.GetEnumerator();

        }

        public DataMemberAttribute this[int index]
        {
            get
            {
                if (index < 0 || index > this.Count - 1)
                {
                    throw new IndexOutOfRangeException();
                }

                return this._columnAttributes[index];
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        public void GetConfiguration(Type type)
        {
            if (type == null || type.GetProperties().Length <= 0)
            {
                return;
            }

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

                if (string.IsNullOrWhiteSpace(attr.Name))
                {
                    attr.Name = propertyInfo.Name;
                }

                attr.PropertyName = propertyInfo.Name;
                attr.PropertyType = propertyInfo.PropertyType;
                attr.PropertyInfo = propertyInfo;

                this._columnAttributes.Add(attr);
            }

            this._columnAttributes = this._columnAttributes.OrderBy(p => p.Order).ToList();
        }

        /// <summary>
        /// 
        /// </summary>
        public int Count
        {
            get
            {
                return _columnAttributes.Count;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _columnAttributes.GetEnumerator();
        }
    }
}
