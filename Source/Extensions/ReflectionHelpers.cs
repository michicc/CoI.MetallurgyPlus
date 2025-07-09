using System;
using System.Reflection;

namespace CoI.MetallurgyPlus.Extensions;

internal static class ReflectionHelpers
{
    /// <summary>
    /// Set a readonly field in an instance.
    /// </summary>
    /// <typeparam name="T">Instance type</typeparam>
    /// <param name="obj">Instance to set the field on</param>
    /// <param name="fieldName">Name of the field</param>
    /// <param name="value">Value to set the field to</param>
    /// <exception cref="ArgumentNullException"></exception>
    public static void SetField<T>(this T obj, string fieldName, object value) where T : class
    {
        if (obj is null) throw new ArgumentNullException(nameof(obj));

        var field = obj.GetType().GetField(fieldName, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
        field.SetValue(obj, value);
    }
}
