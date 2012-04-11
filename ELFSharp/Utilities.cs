using System;

namespace ELFSharp
{
    internal static class Utilities
    {
        internal static T To<T>(this object source)
        {
            return (T)Convert.ChangeType(source, typeof(T));
        }
    }
}

