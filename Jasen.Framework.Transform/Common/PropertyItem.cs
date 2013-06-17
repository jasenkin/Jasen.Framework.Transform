using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Jasen.Framework.Transform
{
    public struct PropertyItem : IComparable<PropertyItem>
    {
        private PropertyInfo _property;
        private string _name;
        private int _id;

        public PropertyItem(PropertyInfo property)
            :this(property, 0)
        {
        }

        public PropertyItem(PropertyInfo property,int id)
            : this(property, id, string.Empty)
        {
             
        }

        public PropertyItem(PropertyInfo property, int id, string name)
        {
            this._property = property;
            this._id = id;
            this._name = name;
        }

        public PropertyInfo PropertyInfo
        {
            get
            {
                return this._property;
            }
            set
            {
                this._property = value;
            }
        }

        public string Name
        {
            get
            {
                return this._name;
            }
            set
            {
                this._name = value;
            }
        }

        public int Id
        {
            get
            {
                return this._id;
            }
            set
            {
                this._id = value;
            }
        }

        public int CompareTo(PropertyItem other)
        {
            return this.Id - other.Id;
        }
    }
}
