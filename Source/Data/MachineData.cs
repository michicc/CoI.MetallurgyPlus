using CoI.MetallurgyPlus.Extensions;
using Mafi.Base;
using Mafi.Core.Mods;
using Mafi.Core.Prototypes;
using Mafi.Localization;

namespace CoI.MetallurgyPlus.Data;

internal class MachineData : IModData
{
    public void RegisterData(ProtoRegistrator registrator)
    {
        OverrideCharcoalMaker(registrator.PrototypesDb);
    }

    private void OverrideCharcoalMaker(ProtosDb protosDb)
    {
        var name = Loc.Str(Ids.Machines.CharcoalMaker.Value + "_MP__name", "Charcoal maker", "name of a machine");
        var desc = Loc.Str(Ids.Machines.CharcoalMaker.Value + "_MP__desc", "Uses wood to create charcoal.", "short description of a machine");

        var proto = protosDb.GetOrThrow<Proto>(Ids.Machines.CharcoalMaker);
        proto.SetProperty(nameof(Proto.Strings), new Proto.Str(name, desc));
    }
}
