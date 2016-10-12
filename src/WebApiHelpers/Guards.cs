using System;

namespace WebApiHelpers
{
    public static class Guards
    {
        public static void CheckArgumentNull(this object o, string name)
        {
            if (o == null)
                throw new ArgumentNullException(name);
        }

        public static void CheckMandatoryOption(this string s, string name)
        {
            if (string.IsNullOrEmpty(s))
                throw new ArgumentException($"Option '{name}' must be provided");
        }
    }
}
