using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jasen.Framework.Transform
{
    public class EnumItem<T> where T : struct
    {
        public EnumItem(T entity, string desc)
        {
            this.Value = entity;
            this.Desc = desc;
        }

        public T Value
        {
            get;
            set;
        }

        public string Desc
        {
            get;
            set;
        }

        public override string ToString()
        {
            return this.Desc;
        }
    }
}
