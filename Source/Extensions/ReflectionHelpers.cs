using System;
using System.Reflection;

namespace CoI.MetallurgyPlus.Extensions;

internal static class ReflectionHelpers
{
    /// <summary>
    /// Set a (private) field in an instance.
    /// </summary>
    /// <typeparam name="T">Instance type</typeparam>
    /// <param name="obj">Instance to set the field on</param>
    /// <param name="fieldName">Name of the field</param>
    /// <param name="value">Value to set the field to</param>
    public static void SetField<T>(this T obj, string fieldName, object? value) where T : class
    {
        if (obj is null) throw new ArgumentNullException(nameof(obj));

        var type = obj.GetType();
        while (type is not null) {
            var field = type.GetField(fieldName, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
            if (field is not null) {
                field.SetValue(obj, value);
                return;
            }

            type = type.BaseType;
        }

        throw new InvalidOperationException($"Field '{fieldName}' not found on the provided object of type '{obj.GetType().FullName}'.");
    }

    /// <summary>
    /// Get a (private) field in an instance.
    /// </summary>
    /// <typeparam name="V">Instance type</typeparam>
    /// <typeparam name="T">Type of the field</typeparam>
    /// <param name="obj">Instance to get the field from</param>
    /// <param name="fieldName">Name of the field</param>
    /// <returns>Value of the field</returns>
    public static V GetField<V, T>(this T obj, string fieldName) where T : class
    {
        if (obj is null) throw new ArgumentNullException(nameof(obj));

        var field = obj.GetType().GetField(fieldName, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
        return (V)field.GetValue(obj);
    }

    /// <summary>
    /// Set a (private) static field.
    /// </summary>
    /// <param name="type">Type the field belongs to</param>
    /// <param name="fieldName">Name of the field</param>
    /// <param name="value">Value to set the field to</param>
    public static void SetStaticField(Type type, string fieldName, object value)
    {
        if (type is null) throw new ArgumentNullException(nameof(type));

        var field = type.GetField(fieldName, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static);
        field.SetValue(null, value);
    }

    /// <summary>
    /// Get a (private) static field.
    /// </summary>
    /// <typeparam name="V">Type of the field</typeparam>
    /// <param name="type">Type the field belongs to</param>
    /// <param name="fieldName">Name of the field</param>
    /// <returns>Value of the field</returns>
    public static V GetStaticField<V>(Type type, string fieldName)
    {
        if (type is null) throw new ArgumentNullException(nameof(type));

        var field = type.GetField(fieldName, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static);
        return (V)field.GetValue(null);
    }

    /// <summary>
    /// Set a (private) property in an instance.
    /// </summary>
    /// <typeparam name="T">Instance type</typeparam>
    /// <param name="obj">Instance to set the property on</param>
    /// <param name="propName">Name of the property</param>
    /// <param name="value">Value to set the property to</param>
    public static void SetProperty<T>(this T obj, string propName, object value) where T : class
    {
        if (obj is null) throw new ArgumentNullException(nameof(obj));

        var type = obj.GetType();
        var propInfo = type.GetProperty(propName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic) ?? throw new InvalidOperationException($"Property '{propName}' not found on the provided object.");

        if (propInfo.CanWrite) {
            propInfo.SetValue(obj, value, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.SetProperty, null, null, null);
        } else {
            while (type != null) {
                var backingField = type.GetField($"<{propName}>k__BackingField", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                if (backingField != null) {
                    backingField.SetValue(obj, value);
                    return;
                }

                type = type.BaseType;
            }

            throw new InvalidOperationException($"Backing field of property '{propName}' not found on the provided object.");
        }
    }
}
