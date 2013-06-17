using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jasen.Framework.Transform.Test
{
    public enum ExchangeType
    {
        /// <summary>
        ///  不限      
        /// </summary>
        [Enum("不限", true)]
        All = 0,
        /// <summary>
        ///  深圳证券交易所      
        /// </summary>
        [Enum("深圳证券交易所")]
        SZSE = 1
    }
}
