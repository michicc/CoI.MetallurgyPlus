using CoI.MetallurgyPlus.Extensions;
using Mafi;
using Mafi.Base;
using Mafi.Core.Entities.Static.Layout;
using Mafi.Core.Factory.Machines;
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
        OverrideRotaryKiln(registrator);
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

    private void OverrideRotaryKiln(ProtoRegistrator registrator)
    {
        // Change port layout of rotary kiln to match gas rotary kiln.
        var layoutString = new string[] {
            "A~>[7][7][7][6][6][2][2][2][2][2][2][2][2][2]   ",
            "   [7][7][7][6][6][2][2][2][2][2][2][2][2][2]>#X",
            "   [2][2][2][2][2][2][2][2][2][2][2][3][2][2]>~Y",
            "B~>[2][2][2][2][2][2][2][2][2][2][2][3][2][2]>@E"
        };

        var kilnProto = registrator.PrototypesDb.GetOrThrow<MachineProto>(Ids.Machines.RotaryKiln);
        kilnProto.UpdateLayout(new EntityLayoutParser(registrator.PrototypesDb).ParseLayoutOrThrow(layoutString));

        // Direct iron reduction.
        registrator.RecipeProtoBuilder
            .Start("Direct iron reduction", ModIDs.Recipes.IronReductionT1, Ids.Machines.RotaryKiln)
            .AddInput(6, Ids.Products.IronOre, "A")
            .AddInput(2, Ids.Products.Coal, "B")
            .SetDurationSeconds(15)
            .AddOutput(6, ModIDs.Products.SpongeIron, RecipeProtoBuilder.ANY_COMPATIBLE_PORT)
            .AddOutput(8, Ids.Products.Exhaust, RecipeProtoBuilder.ANY_COMPATIBLE_PORT)
            .BuildAndAdd();
    }
}
