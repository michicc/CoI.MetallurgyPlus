using CoI.MetallurgyPlus.Extensions;
using Mafi;
using Mafi.Base;
using Mafi.Core.Factory.Recipes;
using Mafi.Core.Mods;
using Mafi.Core.Prototypes;
using Mafi.Localization;

namespace CoI.MetallurgyPlus.Data;

internal class MachineData : IModData
{
    public void RegisterData(ProtoRegistrator registrator)
    {
        OverrideCharcoalMaker(registrator);
    }

    private void OverrideCharcoalMaker(ProtoRegistrator registrator)
    {
        var name = Loc.Str(Ids.Machines.CharcoalMaker.Value + "_MP__name", "Charcoal maker", "name of a machine");
        var desc = Loc.Str(Ids.Machines.CharcoalMaker.Value + "_MP__desc", "Uses wood to create charcoal.", "short description of a machine");

        var proto = registrator.PrototypesDb.GetOrThrow<Proto>(Ids.Machines.CharcoalMaker);
        proto.SetProperty(nameof(Proto.Strings), new Proto.Str(name, desc));

        // Wood to charcoal.
        registrator.RecipeProtoBuilder
            .Start("Charcoal making", ModIDs.Recipes.CharcoalFromWood, Ids.Machines.CharcoalMaker)
            .AddInput(10, Ids.Products.Wood, RecipeProtoBuilder.ANY_COMPATIBLE_PORT)
            .SetDurationSeconds(30)
            .AddOutput(6, ModIDs.Products.Charcoal, RecipeProtoBuilder.ANY_COMPATIBLE_PORT)
            .AddOutput(6, Ids.Products.Exhaust, RecipeProtoBuilder.ANY_COMPATIBLE_PORT, true)
            .BuildAndAdd();

        // Remove coal to wood recipe.
        registrator.PrototypesDb.GetOrThrow<RecipeProto>(Ids.Recipes.CharcoalBurning).MarkAsObsolete();
    }
}
