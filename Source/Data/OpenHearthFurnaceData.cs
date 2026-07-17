using Mafi;
using Mafi.Base;
using Mafi.Core.Factory.Recipes;
using Mafi.Core.Mods;

namespace CoI.MetallurgyPlus.Data;

internal class OpenHearthFurnaceData : IModData
{
    public void RegisterData(ProtoRegistrator registrator)
    {
        // Machine: Open-hearth furnace.
        var proto = registrator.MachineProtoBuilder
            .Start("Open-hearth furnace", ModIDs.Machines.OpenHearthFurnace)
            .Description("Produces steel from reduced iron.")
            .SetCategories([Ids.ToolbarCategories.Smelting_Iron])
            .SetCost(Costs.Build.CP(30).Workers(4))
            .SetLayout([
                "   [3][3][3][3][3][3]>@X",
                "   [3][3][3][3][3][3]   ",
                "C~>[4][4][4][4][4][4]>~Y",
                "A'>[4][4][4][4][4][4]   ",
                "B~>[4][4][4][4][4][4]>'Z",
                "   [3][3][3][3][3][3]   ",
                "   [3][3][3][3]         ",
                "   [3][3][3][3]         ",
                "D~>[3][3][3][3]<@E      "
                ])
            .SetPrefabPath("Assets/CoI.Metallurgy+/OpenHearthFurnace.prefab")
            .SetEmissionWhenWorking(3)
            .SetMachineSound("Assets/Base/Machines/MetalWorks/BlastFurnaceT1/BlastFurnace_Sound.prefab")
            .SetCustomIconPath("Assets/CoI.Metallurgy+/Icons/OpenHearthFurnace.png")
            .BuildAndAdd();

        // Recipe: Molten steel from scrap.
        registrator.RecipeProtoBuilder
            .Start(ModIDs.Recipes.SteelFromScrapT1)
            .AddInput(24, Ids.Products.IronScrap, "BC")
            .AddInput(12, ModIDs.Products.Charcoal, "D")
            .AddOutput(24, Ids.Products.MoltenSteel)
            .AddOutput(24, Ids.Products.Exhaust, outputAtStart: true)
            .BuildAndAdd().BindTo(proto, 120.Seconds());

        // Recipe: Molten steel from scrap (coal).
        registrator.RecipeProtoBuilder
            .Start(ModIDs.Recipes.SteelFromScrapT1Coal)
            .AddInput(24, Ids.Products.IronScrap, "BC")
            .AddInput(16, Ids.Products.Coal, "D")
            .AddOutput(24, Ids.Products.MoltenSteel)
            .AddOutput(48, Ids.Products.Exhaust, outputAtStart: true)
            .BuildAndAdd().BindTo(proto, 120.Seconds());

        // Recipe: Molten steel from scrap (FG).
        registrator.RecipeProtoBuilder
            .Start(ModIDs.Recipes.SteelFromScrapT1FG)
            .AddInput(24, Ids.Products.IronScrap, "BC")
            .AddInput(36, Ids.Products.FuelGas, "E")
            .AddOutput(24, Ids.Products.MoltenSteel)
            .AddOutput(36, Ids.Products.CarbonDioxide, outputAtStart: true)
            .BuildAndAdd().BindTo(proto, 120.Seconds());

        // Recipe: Molten steel from molten iron.
        registrator.RecipeProtoBuilder
            .Start(ModIDs.Recipes.SteelFromIronT1)
            .AddInput(30, Ids.Products.MoltenIron, RecipeProtoBuilder.ANY_COMPATIBLE_PORT)
            .AddInput(6, ModIDs.Products.Charcoal, "D")
            .AddOutput(24, Ids.Products.MoltenSteel)
            .AddOutput(6, Ids.Products.Slag)
            .AddOutput(16, Ids.Products.Exhaust, outputAtStart: true)
            .BuildAndAdd().BindTo(proto, 120.Seconds());

        // Recipe: Molten steel from molten iron (coal).
        registrator.RecipeProtoBuilder
            .Start(ModIDs.Recipes.SteelFromIronT1Coal)
            .AddInput(30, Ids.Products.MoltenIron, RecipeProtoBuilder.ANY_COMPATIBLE_PORT)
            .AddInput(8, Ids.Products.Coal, "D")
            .AddOutput(24, Ids.Products.MoltenSteel)
            .AddOutput(6, Ids.Products.Slag)
            .AddOutput(36, Ids.Products.Exhaust, outputAtStart: true)
            .BuildAndAdd().BindTo(proto, 120.Seconds());

        // Recipe: Molten steel from molten iron (fuel gas).
        registrator.RecipeProtoBuilder
            .Start(ModIDs.Recipes.SteelFromIronT1FG)
            .AddInput(30, Ids.Products.MoltenIron, RecipeProtoBuilder.ANY_COMPATIBLE_PORT)
            .AddInput(24, Ids.Products.FuelGas, "E")
            .AddOutput(24, Ids.Products.MoltenSteel)
            .AddOutput(6, Ids.Products.Slag)
            .AddOutput(24, Ids.Products.CarbonDioxide, outputAtStart: true)
            .BuildAndAdd().BindTo(proto, 120.Seconds());

        // Recipe: Molten steel from sponge iron.
        registrator.RecipeProtoBuilder
            .Start(ModIDs.Recipes.SteelFromSpongeT1)
            .AddInput(48, ModIDs.Products.SpongeIron, "BC")
            .AddInput(6, Ids.Products.Limestone, "BC")
            .AddInput(12, ModIDs.Products.Charcoal, "D")
            .AddOutput(24, Ids.Products.MoltenSteel)
            .AddOutput(30, Ids.Products.Slag)
            .AddOutput(24, Ids.Products.Exhaust, outputAtStart: true)
            .BuildAndAdd().BindTo(proto, 120.Seconds());

        // Recipe: Molten steel from sponge iron (coal).
        registrator.RecipeProtoBuilder
            .Start(ModIDs.Recipes.SteelFromSpongeT1Coal)
            .AddInput(48, ModIDs.Products.SpongeIron, "BC")
            .AddInput(6, Ids.Products.Limestone, "BC")
            .AddInput(16, Ids.Products.Coal, "D")
            .AddOutput(24, Ids.Products.MoltenSteel)
            .AddOutput(30, Ids.Products.Slag)
            .AddOutput(48, Ids.Products.Exhaust, outputAtStart: true)
            .BuildAndAdd().BindTo(proto, 120.Seconds());

        // Recipe: Molten steel from sponge iron (fuel gas).
        registrator.RecipeProtoBuilder
            .Start(ModIDs.Recipes.SteelFromSpongeT1FG)
            .AddInput(48, ModIDs.Products.SpongeIron, "BC")
            .AddInput(6, Ids.Products.Limestone, "BC")
            .AddInput(48, Ids.Products.FuelGas, "E")
            .AddOutput(24, Ids.Products.MoltenSteel)
            .AddOutput(30, Ids.Products.Slag)
            .AddOutput(48, Ids.Products.CarbonDioxide, outputAtStart: true)
            .BuildAndAdd().BindTo(proto, 120.Seconds());
    }
}
