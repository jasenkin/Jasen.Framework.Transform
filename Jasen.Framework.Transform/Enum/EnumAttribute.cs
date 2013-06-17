using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jasen.Framework.Transform
{
    /// <summary>
    ///  
    /// </summary>
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public sealed class EnumAttribute : Attribute
    { 
        public EnumAttribute()
        {
        }

        public EnumAttribute(string columnDesc, bool ispecialRequired = false)
        {
             this.Desc = columnDesc;
             this.IsSpecialRequired = ispecialRequired;
        }
         
        public string Desc
        {
            get;
            set;
        }

        public bool IsSpecialRequired
        {
            get;
            set;
        }
    }
}
