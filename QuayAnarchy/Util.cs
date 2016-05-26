using System;
using System.Collections.Generic;
using System.Reflection;

namespace QuayAnarchy
{
    public static class Util
    {
        public static void AddRange<T>(this ICollection<T> target, IEnumerable<T> source)
        {
            if (target == null)
                throw new ArgumentNullException(nameof(target));
            if (source == null)
            {
                return;
            }
            foreach (var element in source)
                target.Add(element);
        }

        public static T GetPrivate<T>(object o, string fieldName)
        {
            var field = o.GetType().GetField(fieldName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            return (T)field.GetValue(o);
        }
    }
}