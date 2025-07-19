using CoI.MetallurgyPlus.Extensions;
using Mafi;
using Mafi.Base;
using Mafi.Core.Factory.Recipes;
using Mafi.Core.Mods;
using Mafi.Core.Prototypes;

namespace CoI.MetallurgyPlus.Data;

internal class RecipesData : IModData
{
    public void RegisterData(ProtoRegistrator registrator)
    {
        AddCharcoalRecipes(registrator);
        AddSmeltingRecipes(registrator);

        // CP assembly steel.
        registrator.RecipeProtoBuilder
            .Start("CP assembly steel (T1)", ModIDs.Recipes.CpAssemblySteelT1, Ids.Machines.AssemblyManual)
            .AddInput(2, Ids.Products.Steel, RecipeProtoBuilder.ANY_COMPATIBLE_PORT)
            .AddInput(2, Ids.Products.Wood, RecipeProtoBuilder.ANY_COMPATIBLE_PORT)
            .AddInput(3, Ids.Products.ConcreteSlab, RecipeProtoBuilder.ANY_COMPATIBLE_PORT)
            .SetDurationSeconds(40)
            .AddOutput(4, Ids.Products.ConstructionParts, RecipeProtoBuilder.ANY_COMPATIBLE_PORT)
            .BuildAndAdd();

        registrator.RecipeProtoBuilder
            .Start("CP assembly steel (T2)", ModIDs.Recipes.CpAssemblySteelT2, Ids.Machines.AssemblyElectrified)
            .AddInput(2, Ids.Products.Steel, RecipeProtoBuilder.ANY_COMPATIBLE_PORT)
            .AddInput(2, Ids.Products.Wood, RecipeProtoBuilder.ANY_COMPATIBLE_PORT)
            .AddInput(3, Ids.Products.ConcreteSlab, RecipeProtoBuilder.ANY_COMPATIBLE_PORT)
            .SetDurationSeconds(20)
            .AddOutput(4, Ids.Products.ConstructionParts, RecipeProtoBuilder.ANY_COMPATIBLE_PORT)
            .BuildAndAdd();

        // Mech. parts assembly steel.
        registrator.RecipeProtoBuilder
            .Start("Mech. parts assembly (T1-2)", ModIDs.Recipes.MechPartsAssemblyT1Steel, Ids.Machines.AssemblyManual)
            .AddInput(2, Ids.Products.Steel, RecipeProtoBuilder.ANY_COMPATIBLE_PORT)
            .SetDurationSeconds(40)
            .AddOutput(4, Ids.Products.MechanicalParts, RecipeProtoBuilder.ANY_COMPATIBLE_PORT)
            .BuildAndAdd();

        registrator.RecipeProtoBuilder
            .Start("Mech. parts assembly (T2-2)", ModIDs.Recipes.MechPartsAssemblyT2Steel, Ids.Machines.AssemblyElectrified)
            .AddInput(4, Ids.Products.Steel, RecipeProtoBuilder.ANY_COMPATIBLE_PORT)
            .SetDurationSeconds(40)
            .AddOutput(8, Ids.Products.MechanicalParts, RecipeProtoBuilder.ANY_COMPATIBLE_PORT)
            .BuildAndAdd();

        // Apply overrides for existing recipes.
        ApplyOverrides(registrator.PrototypesDb);
    }

    private void AddCharcoalRecipes(ProtoRegistrator registrator)
    {
        // Wood to charcoal.
        registrator.RecipeProtoBuilder
            .Start("Charcoal making", ModIDs.Recipes.CharcoalFromWood, Ids.Machines.CharcoalMaker)
            .AddInput(10, Ids.Products.Wood, RecipeProtoBuilder.ANY_COMPATIBLE_PORT)
            .SetDurationSeconds(30)
            .AddOutput(6, ModIDs.Products.Charcoal, RecipeProtoBuilder.ANY_COMPATIBLE_PORT)
            .AddOutput(3, Ids.Products.Exhaust, RecipeProtoBuilder.ANY_COMPATIBLE_PORT, true)
            .BuildAndAdd();

        // Basic concrete (charcoal).
        registrator.RecipeProtoBuilder
            .Start("Simple concrete making (charcaol)", ModIDs.Recipes.SimpleConcreteCharcoal, Ids.Machines.BricksMaker)
            .AddInput(12, Ids.Products.Limestone, RecipeProtoBuilder.ANY_COMPATIBLE_PORT)
            .AddInput(3, ModIDs.Products.Charcoal, RecipeProtoBuilder.ANY_COMPATIBLE_PORT)
            .AddInput(4, Ids.Products.Water, RecipeProtoBuilder.ANY_COMPATIBLE_PORT)
            .SetDurationSeconds(40)
            .AddOutput(8, Ids.Products.ConcreteSlab, RecipeProtoBuilder.ANY_COMPATIBLE_PORT)
            .AddOutput(8, Ids.Products.Exhaust, RecipeProtoBuilder.ANY_COMPATIBLE_PORT)
            .BuildAndAdd();

        // Diesel distillation (basic)
        registrator.RecipeProtoBuilder
            .Start("Diesel distillation (basic)", ModIDs.Recipes.DieselDistillationCharcoal, Ids.Machines.BasicDieselDistiller)
            .AddInput(20, Ids.Products.CrudeOil, RecipeProtoBuilder.ANY_COMPATIBLE_PORT)
            .AddInput(3, ModIDs.Products.Charcoal, RecipeProtoBuilder.ANY_COMPATIBLE_PORT)
            .SetDurationSeconds(20)
            .AddOutput(9, Ids.Products.Diesel, "X")
            .AddOutput(5, Ids.Products.WasteWater, "Z")
            .AddOutput(12, Ids.Products.Exhaust, "S")
            .BuildAndAdd();

        // Water desalination (basic)
        registrator.RecipeProtoBuilder
            .Start("Water desalination (basic)", ModIDs.Recipes.WaterDesalinationCharcoal, Ids.Machines.BasicDieselDistiller)
            .AddInput(10, Ids.Products.Seawater, RecipeProtoBuilder.ANY_COMPATIBLE_PORT)
            .AddInput(1, ModIDs.Products.Charcoal, RecipeProtoBuilder.ANY_COMPATIBLE_PORT)
            .SetDurationSeconds(10)
            .AddOutput(6, Ids.Products.Water, "X")
            .AddOutput(4, Ids.Products.Brine, "Z")
            .AddOutput(2, Ids.Products.Exhaust, "S", true)
            .BuildAndAdd();
    }

    private void AddSmeltingRecipes(ProtoRegistrator registrator)
    {
        // Iron ore smelting (charcoal).
        registrator.RecipeProtoBuilder
            .Start("Iron smelting (charcoal)", ModIDs.Recipes.IronSmeltingT1Charcoal, Ids.Machines.SmeltingFurnaceT1)
            .AddInput(10, Ids.Products.IronOre, RecipeProtoBuilder.ANY_COMPATIBLE_PORT)
            .AddInput(3, ModIDs.Products.Charcoal, RecipeProtoBuilder.ANY_COMPATIBLE_PORT)
            .SetDurationSeconds(20)
            .AddOutput(5, Ids.Products.MoltenIron, RecipeProtoBuilder.ANY_COMPATIBLE_PORT)
            .AddOutput(5, Ids.Products.Slag, RecipeProtoBuilder.ANY_COMPATIBLE_PORT)
            .AddOutput(5, Ids.Products.Exhaust, RecipeProtoBuilder.ANY_COMPATIBLE_PORT, true)
            .BuildAndAdd();

        // Basic steel casting.
        registrator.RecipeProtoBuilder
            .Start("Steel casting", ModIDs.Recipes.SteelCasting, Ids.Machines.Caster)
            .AddInput(6, Ids.Products.MoltenSteel, RecipeProtoBuilder.ANY_COMPATIBLE_PORT)
            .SetDurationSeconds(60)
            .AddOutput(6, Ids.Products.Steel, RecipeProtoBuilder.ANY_COMPATIBLE_PORT)
            .BuildAndAdd();
        // Basic steel casting (T2).
        registrator.RecipeProtoBuilder
            .Start("Steel casting", ModIDs.Recipes.SteelCastingT2, Ids.Machines.CasterT2)
            .AddInput(6, Ids.Products.MoltenSteel, RecipeProtoBuilder.ANY_COMPATIBLE_PORT)
            .SetDurationSeconds(30)
            .AddOutput(6, Ids.Products.Steel, RecipeProtoBuilder.ANY_COMPATIBLE_PORT)
            .BuildAndAdd();
    }

    private void ApplyOverrides(ProtosDb protosDb)
    {
        // Remove coal to wood recipe.
        protosDb.GetOrThrow<RecipeProto>(Ids.Recipes.CharcoalBurning).MarkAsObsolete();

        // Remove smelting recipes.
        protosDb.GetOrThrow<RecipeProto>(Ids.Recipes.IronSmeltingT1Coal).MarkAsObsolete();
        protosDb.GetOrThrow<RecipeProto>(Ids.Recipes.IronSmeltingT1Scrap).MarkAsObsolete();

        // Remove iron casting recipes.
        protosDb.GetOrThrow<RecipeProto>(Ids.Recipes.IronCasting).MarkAsObsolete();
        protosDb.GetOrThrow<RecipeProto>(Ids.Recipes.IronCastingT2).MarkAsObsolete();

        // Remove iron construction part recipes.
        protosDb.GetOrThrow<RecipeProto>(Ids.Recipes.CpAssemblyT1).MarkAsObsolete();
        protosDb.GetOrThrow<RecipeProto>(Ids.Recipes.CpAssemblyT2).MarkAsObsolete();
        protosDb.GetOrThrow<RecipeProto>(Ids.Recipes.CpAssemblyT3).MarkAsObsolete();
        protosDb.GetOrThrow<RecipeProto>(Ids.Recipes.CpAssemblyT4).MarkAsObsolete();
        protosDb.GetOrThrow<RecipeProto>(Ids.Recipes.CpAssemblyT5).MarkAsObsolete();

        // Remove iron mechanical components recipes.
        protosDb.GetOrThrow<RecipeProto>(Ids.Recipes.MechPartsAssemblyT1).MarkAsObsolete();
        protosDb.GetOrThrow<RecipeProto>(Ids.Recipes.MechPartsAssemblyT2).MarkAsObsolete();
        protosDb.GetOrThrow<RecipeProto>(Ids.Recipes.MechPartsAssemblyT3Iron).MarkAsObsolete();
        protosDb.GetOrThrow<RecipeProto>(Ids.Recipes.MechPartsAssemblyT4Iron).MarkAsObsolete();
        protosDb.GetOrThrow<RecipeProto>(Ids.Recipes.MechPartsAssemblyT5Iron).MarkAsObsolete();
    }
}
