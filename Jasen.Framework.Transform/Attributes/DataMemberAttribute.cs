using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace Jasen.Framework.Transform
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class DataMemberAttribute : Attribute
    {
        public bool IsRequire
        {
            get;
            set;
        }

        public string Name
        {
            get;
            set;
        }

        public int Order
        {
            get;
            set;
        }

        public DataMemberAttribute()
        {
            this.IsRequire = true;
        }

        public DataMemberAttribute(int order)
        {
            this.IsRequire = true;
            this.Order = order;
        }

        public DataMemberAttribute(string name, int order, bool isRequire = true)
        {
            this.IsRequire = isRequire;
            this.Name = name;
            this.Order = order;
        }

        public string PropertyName 
        { 
            get; 
            set;
        }

        public Type PropertyType
        {
            get; 
            set;
        }

        public PropertyInfo PropertyInfo
        {
            get;
            set; 
        }
    }
}
