using System;
using System.Collections.Generic;
using System.Text;

namespace System.Collections.Generic
{
    public static class CollectionExtensions
    {
        public static bool IsNullOrEmpty<T>(this ICollection<T> source)
        {
            return source == null || source.Count <= 0;
        }
    }
}
