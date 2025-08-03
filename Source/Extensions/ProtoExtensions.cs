using Mafi.Core.Prototypes;
using Mafi.Localization;

namespace CoI.MetallurgyPlus.Extensions;

internal static class ProtoExtensions
{
    public static void SetName(this Proto proto, string id, string name, string comment)
    {
        var str = Loc.Str(id, name, comment);
        proto.SetProperty(nameof(Proto.Strings), new Proto.Str(str, proto.Strings.DescShort));
    }
}
