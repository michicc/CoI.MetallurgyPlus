using Mafi.Core.Factory.Machines;
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

    public static MachineProto GetMachine(this ProtosDb db, Proto.ID id) => db.GetOrThrow<MachineProto>(id);
}
