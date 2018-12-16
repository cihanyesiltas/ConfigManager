using System;
using System.ComponentModel;

namespace ConfigManager.Core.Extensions
{
    public static class StringExtensions
    {
        public static T ToEnum<T>(this string value, T defaultValue) where T : struct
        {
            if (string.IsNullOrEmpty(value))
            {
                return defaultValue;
            }

            return Enum.TryParse(value, true, out T result) ? result : defaultValue;
        }

        /// <summary>
        /// try to convert string to specified type. if not, return type default value.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="input"></param>
        /// <returns></returns>
        public static T Cast<T>(this string input, string type)
        {
            try
            {
                var converter = TypeDescriptor.GetConverter(GetType(type));
                return (T)converter.ConvertFromString(input);
            }
            catch (Exception)
            {
                return default(T);
            }
        }

        private static Type GetType(string type)
        {
            switch (type)
            {
                case "String":
                    return typeof(string);
                case "Boolean":
                    return typeof(bool);
                case "Int":
                    return typeof(int);
                case "Double":
                    return typeof(double);
                default:
                    return typeof(string);
            }
        }
    }
}
