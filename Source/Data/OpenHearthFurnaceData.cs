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
            .SetCustomIconPath("Assets/CoI.Metallurgy+/OpenHearthFurnace/OpenHearthFurnace.png")
            .BuildAndAdd();

        // Recipe: Molten steel from scrap.
        registrator.RecipeProtoBuilder
            .Start("Iron scrap smelting", ModIDs.Recipes.SteelFromScrapT1, proto)
            .AddInput(24, Ids.Products.IronScrap, "BC")
            .AddInput(12, ModIDs.Products.Charcoal, "D")
            .SetDurationSeconds(120)
            .AddOutput(24, Ids.Products.MoltenSteel, RecipeProtoBuilder.ANY_COMPATIBLE_PORT)
            .AddOutput(24, Ids.Products.Exhaust, RecipeProtoBuilder.ANY_COMPATIBLE_PORT, true)
            .BuildAndAdd();

        // Recipe: Molten steel from scrap (coal).
        registrator.RecipeProtoBuilder
            .Start("Iron scrap smelting (coal)", ModIDs.Recipes.SteelFromScrapT1Coal, proto)
            .AddInput(24, Ids.Products.IronScrap, "BC")
            .AddInput(16, Ids.Products.Coal, "D")
            .SetDurationSeconds(120)
            .AddOutput(24, Ids.Products.MoltenSteel, RecipeProtoBuilder.ANY_COMPATIBLE_PORT)
            .AddOutput(48, Ids.Products.Exhaust, RecipeProtoBuilder.ANY_COMPATIBLE_PORT, true)
            .BuildAndAdd();

        // Recipe: Molten steel from scrap (FG).
        registrator.RecipeProtoBuilder
            .Start("Iron scrap smelting (fuel gas)", ModIDs.Recipes.SteelFromScrapT1FG, proto)
            .AddInput(24, Ids.Products.IronScrap, "BC")
            .AddInput(36, Ids.Products.FuelGas, "E")
            .SetDurationSeconds(120)
            .AddOutput(24, Ids.Products.MoltenSteel, RecipeProtoBuilder.ANY_COMPATIBLE_PORT)
            .AddOutput(36, Ids.Products.CarbonDioxide, RecipeProtoBuilder.ANY_COMPATIBLE_PORT, true)
            .BuildAndAdd();

        // Recipe: Molten steel from molten iron.
        registrator.RecipeProtoBuilder
            .Start("Steel smelting", ModIDs.Recipes.SteelFromIronT1, proto)
            .AddInput(30, Ids.Products.MoltenIron, RecipeProtoBuilder.ANY_COMPATIBLE_PORT)
            .AddInput(8, ModIDs.Products.Charcoal, "D")
            .SetDurationSeconds(120)
            .AddOutput(24, Ids.Products.MoltenSteel, RecipeProtoBuilder.ANY_COMPATIBLE_PORT)
            .AddOutput(6, Ids.Products.Slag, RecipeProtoBuilder.ANY_COMPATIBLE_PORT)
            .AddOutput(16, Ids.Products.Exhaust, RecipeProtoBuilder.ANY_COMPATIBLE_PORT, true)
            .BuildAndAdd();

        // Recipe: Molten steel from molten iron (coal).
        registrator.RecipeProtoBuilder
            .Start("Steel smelting (coal)", ModIDs.Recipes.SteelFromIronT1Coal, proto)
            .AddInput(30, Ids.Products.MoltenIron, RecipeProtoBuilder.ANY_COMPATIBLE_PORT)
            .AddInput(12, Ids.Products.Coal, "D")
            .SetDurationSeconds(120)
            .AddOutput(24, Ids.Products.MoltenSteel, RecipeProtoBuilder.ANY_COMPATIBLE_PORT)
            .AddOutput(6, Ids.Products.Slag, RecipeProtoBuilder.ANY_COMPATIBLE_PORT)
            .AddOutput(36, Ids.Products.Exhaust, RecipeProtoBuilder.ANY_COMPATIBLE_PORT, true)
            .BuildAndAdd();

        // Recipe: Molten steel from molten iron (fuel gas).
        registrator.RecipeProtoBuilder
            .Start("Steel smelting (fuel gas)", ModIDs.Recipes.SteelFromIronT1FG, proto)
            .AddInput(30, Ids.Products.MoltenIron, RecipeProtoBuilder.ANY_COMPATIBLE_PORT)
            .AddInput(24, Ids.Products.FuelGas, "E")
            .SetDurationSeconds(120)
            .AddOutput(24, Ids.Products.MoltenSteel, RecipeProtoBuilder.ANY_COMPATIBLE_PORT)
            .AddOutput(6, Ids.Products.Slag, RecipeProtoBuilder.ANY_COMPATIBLE_PORT)
            .AddOutput(24, Ids.Products.CarbonDioxide, RecipeProtoBuilder.ANY_COMPATIBLE_PORT, true)
            .BuildAndAdd();

        // Recipe: Molten steel from sponge iron.
        registrator.RecipeProtoBuilder
            .Start("Iron sponge smelting", ModIDs.Recipes.SteelFromSpongeT1, proto)
            .AddInput(48, ModIDs.Products.SpongeIron, "BC")
            .AddInput(6, Ids.Products.Limestone, "BC")
            .AddInput(12, ModIDs.Products.Charcoal, "D")
            .SetDurationSeconds(120)
            .AddOutput(24, Ids.Products.MoltenSteel, RecipeProtoBuilder.ANY_COMPATIBLE_PORT)
            .AddOutput(30, Ids.Products.Slag, RecipeProtoBuilder.ANY_COMPATIBLE_PORT)
            .AddOutput(24, Ids.Products.Exhaust, RecipeProtoBuilder.ANY_COMPATIBLE_PORT, true)
            .BuildAndAdd();

        // Recipe: Molten steel from sponge iron (coal).
        registrator.RecipeProtoBuilder
            .Start("Iron sponge smelting (coal)", ModIDs.Recipes.SteelFromSpongeT1Coal, proto)
            .AddInput(48, ModIDs.Products.SpongeIron, "BC")
            .AddInput(6, Ids.Products.Limestone, "BC")
            .AddInput(16, Ids.Products.Coal, "D")
            .SetDurationSeconds(120)
            .AddOutput(24, Ids.Products.MoltenSteel, RecipeProtoBuilder.ANY_COMPATIBLE_PORT)
            .AddOutput(30, Ids.Products.Slag, RecipeProtoBuilder.ANY_COMPATIBLE_PORT)
            .AddOutput(48, Ids.Products.Exhaust, RecipeProtoBuilder.ANY_COMPATIBLE_PORT, true)
            .BuildAndAdd();

        // Recipe: Molten steel from sponge iron (fuel gas).
        registrator.RecipeProtoBuilder
            .Start("Iron sponge smelting (fuel gas)", ModIDs.Recipes.SteelFromSpongeT1FG, proto)
            .AddInput(48, ModIDs.Products.SpongeIron, "BC")
            .AddInput(6, Ids.Products.Limestone, "BC")
            .AddInput(36, Ids.Products.FuelGas, "E")
            .SetDurationSeconds(120)
            .AddOutput(24, Ids.Products.MoltenSteel, RecipeProtoBuilder.ANY_COMPATIBLE_PORT)
            .AddOutput(30, Ids.Products.Slag, RecipeProtoBuilder.ANY_COMPATIBLE_PORT)
            .AddOutput(36, Ids.Products.CarbonDioxide, RecipeProtoBuilder.ANY_COMPATIBLE_PORT, true)
            .BuildAndAdd();
    }
}
