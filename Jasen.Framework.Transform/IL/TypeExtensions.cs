using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Jasen.Framework.Transform;

namespace Jasen.Framework.Transform
{
    public static class TypeExtensions
    {
        public static bool CanImplicitTransfer(this Type sourceType, Type targetType)
        {
            return TransferTable.CanImplicitTransfer(sourceType, targetType);
        }
    }
}
