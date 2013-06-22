using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jasen.Framework.Transform.Test
{
    public class ContinousContract
    {
        [DataMember(1)]
        public int OrgidID
        {
            get;
            set;
        }

        [DataMember(3)]
        public string Code
        {
            get;
            set;
        }

        [DataMember(2)]
        public string Name
        {
            get;
            set;
        }

        [DataMember(4)]
        public string ExchangeType
        {
            get;
            set;
        }

        [DataMember(5)]
        public int ExchangeTypeValue
        {
            get;
            set;
        }
    }
}
