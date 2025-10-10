using System;

namespace SS
{
    public static class TypeExtentions
    {
        public static string ToShortname(this Type type)
        {
            var split = type.ToString().Split('.');
            return split[split.Length - 1];
        }
    }
}
