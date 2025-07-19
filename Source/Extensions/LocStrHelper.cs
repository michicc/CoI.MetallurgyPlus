using Mafi.Localization;
using System;

namespace CoI.MetallurgyPlus.Extensions;

internal static class LocStrHelper
{
    public static LocStr2 GetExistingLocalizedString2Arg(string id, string enUs, string comment) => GetExistingLocString(() => LocalizationManager.GetLocalizedString2Arg(id, enUs, comment));
    public static LocStr3 GetExistingLocalizedString3Arg(string id, string enUs, string comment) => GetExistingLocString(() => LocalizationManager.GetLocalizedString3Arg(id, enUs, comment));

    private static T GetExistingLocString<T>(Func<T> cb)
    {
        bool oldIgnore = ReflectionHelpers.GetStaticField<bool>(typeof(LocalizationManager), "s_ignoreDuplicates");
        try {
            LocalizationManager.IgnoreDuplicates();

            return cb();
        } finally {
            ReflectionHelpers.SetStaticField(typeof(LocalizationManager), "s_ignoreDuplicates", oldIgnore);
        }
    }
}
