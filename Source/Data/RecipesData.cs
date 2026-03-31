using CoI.MetallurgyPlus.Extensions;
using Mafi;
using Mafi.Base;
using Mafi.Core.Factory.Recipes;
using Mafi.Core.Mods;
using Mafi.Core.Products;
using Mafi.Core.Prototypes;

namespace CoI.MetallurgyPlus.Data;

internal class RecipesData : IModData
{
    public void RegisterData(ProtoRegistrator registrator)
    {
        AddCharcoalRecipes(registrator);
        AddSmeltingRecipes(registrator);
        AddSecondaryRecipes(registrator);
        AddAssemblyRecipes(registrator);

        // Apply overrides for existing recipes.
        ApplyOverrides(registrator.PrototypesDb);
    }

    private void AddCharcoalRecipes(ProtoRegistrator registrator)
    {
        // Steam generation from charcoal.
        registrator.RecipeProtoBuilder
            .Start("Steam generation", ModIDs.Recipes.SteamGenerationCharcoal, Ids.Machines.BoilerCoal)
            .AddInput(8, Ids.Products.Water, RecipeProtoBuilder.ANY_COMPATIBLE_PORT)
            .AddInput(5, ModIDs.Products.Charcoal, RecipeProtoBuilder.ANY_COMPATIBLE_PORT)
            .SetDurationSeconds(10)
            .AddOutput(8, Ids.Products.SteamHi, "X")
            .AddOutput(5, Ids.Products.Exhaust, "Y")
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

        // Rubber production (charcoal)
        registrator.RecipeProtoBuilder
            .Start("Rubber production (charcoal)", ModIDs.Recipes.RubberProductionNaphthaCharcoal, Ids.Machines.VacuumDistillationTower)
            .AddInput(4, Ids.Products.Naphtha, RecipeProtoBuilder.ANY_COMPATIBLE_PORT)
            .AddInput(2, ModIDs.Products.Charcoal, RecipeProtoBuilder.ANY_COMPATIBLE_PORT)
            .SetDurationSeconds(30)
            .AddOutput(8, Ids.Products.Rubber, "X")
            .AddOutput(1, Ids.Products.WasteWater, "Y")
            .BuildAndAdd();

        // Rubber production (charcoal) (alt)
        registrator.RecipeProtoBuilder
            .Start("Rubber production (charcoal) (alt)", ModIDs.Recipes.RubberProductionDieselWithCharcaol, Ids.Machines.VacuumDistillationTower)
            .AddInput(4, Ids.Products.Diesel, RecipeProtoBuilder.ANY_COMPATIBLE_PORT)
            .AddInput(2, ModIDs.Products.Charcoal, RecipeProtoBuilder.ANY_COMPATIBLE_PORT)
            .SetDurationSeconds(30)
            .AddOutput(8, Ids.Products.Rubber, "X")
            .AddOutput(3, Ids.Products.WasteWater, "Y")
            .BuildAndAdd();

        // Copper smelting (charcoal).
        registrator.RecipeProtoBuilder
            .Start("Copper smelting (charcoal)", ModIDs.Recipes.CopperSmeltingT1Charcoal, Ids.Machines.SmeltingFurnaceT1)
            .AddInput(10, Ids.Products.CopperOre, RecipeProtoBuilder.ANY_COMPATIBLE_PORT)
            .AddInput(5, ModIDs.Products.Charcoal, RecipeProtoBuilder.ANY_COMPATIBLE_PORT)
            .SetDurationSeconds(20)
            .AddOutput(8, Ids.Products.MoltenCopper, RecipeProtoBuilder.ANY_COMPATIBLE_PORT)
            .AddOutput(4, Ids.Products.Slag, RecipeProtoBuilder.ANY_COMPATIBLE_PORT)
            .AddOutput(8, Ids.Products.Exhaust, RecipeProtoBuilder.ANY_COMPATIBLE_PORT, true)
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

        // Iron ore smelting (coke).
        registrator.RecipeProtoBuilder
            .Start("Iron smelting (coke)", ModIDs.Recipes.IronSmeltingT1Coke, Ids.Machines.SmeltingFurnaceT1)
            .AddInput(10, Ids.Products.IronOre, RecipeProtoBuilder.ANY_COMPATIBLE_PORT)
            .AddInput(3, ModIDs.Products.Coke, RecipeProtoBuilder.ANY_COMPATIBLE_PORT)
            .SetDurationSeconds(20)
            .AddOutput(5, Ids.Products.MoltenIron, RecipeProtoBuilder.ANY_COMPATIBLE_PORT)
            .AddOutput(5, Ids.Products.Slag, RecipeProtoBuilder.ANY_COMPATIBLE_PORT)
            .AddOutput(4, Ids.Products.Exhaust, RecipeProtoBuilder.ANY_COMPATIBLE_PORT, true)
            .BuildAndAdd();

        // Copper scrap smelting (charcoal).
        registrator.RecipeProtoBuilder
            .Start("Copper scrap smelting (charcoal)", ModIDs.Recipes.CopperSmeltingT1ScrapCharcoal, Ids.Machines.SmeltingFurnaceT1)
            .AddInput(8, Ids.Products.CopperScrap, RecipeProtoBuilder.ANY_COMPATIBLE_PORT)
            .AddInput(3, ModIDs.Products.Charcoal, RecipeProtoBuilder.ANY_COMPATIBLE_PORT)
            .SetDurationSeconds(20)
            .AddOutput(8, Ids.Products.MoltenCopper, RecipeProtoBuilder.ANY_COMPATIBLE_PORT)
            .AddOutput(6, Ids.Products.Exhaust, RecipeProtoBuilder.ANY_COMPATIBLE_PORT, true)
            .BuildAndAdd();

        // Iron ore smelting (T2).
        registrator.RecipeProtoBuilder
            .Start("Iron smelting (lime)", ModIDs.Recipes.IronSmeltingT2, Ids.Machines.SmeltingFurnaceT2)
            .AddInput(16, Ids.Products.IronOreCrushed, RecipeProtoBuilder.ANY_COMPATIBLE_PORT)
            .AddInput(2, Ids.Products.Limestone, RecipeProtoBuilder.ANY_COMPATIBLE_PORT)
            .AddInput(4, ModIDs.Products.Coke, RecipeProtoBuilder.ANY_COMPATIBLE_PORT)
            .SetDurationSeconds(20)
            .AddOutput(10, Ids.Products.MoltenIron, RecipeProtoBuilder.ANY_COMPATIBLE_PORT)
            .AddOutput(6, Ids.Products.Slag, RecipeProtoBuilder.ANY_COMPATIBLE_PORT)
            .AddOutput(5, Ids.Products.CarbonDioxide, RecipeProtoBuilder.ANY_COMPATIBLE_PORT, true)
            .BuildAndAdd();

        // Iron ore smelting (arc).
        registrator.RecipeProtoBuilder
            .Start("Iron smelting (arc)", ModIDs.Recipes.IronSmeltingArc, Ids.Machines.ArcFurnace2)
            .AddInput(20, Ids.Products.IronOreCrushed, RecipeProtoBuilder.ANY_COMPATIBLE_PORT)
            .AddInput(3, Ids.Products.Limestone, RecipeProtoBuilder.ANY_COMPATIBLE_PORT)
            .AddInput(1, Ids.Products.Graphite, RecipeProtoBuilder.ANY_COMPATIBLE_PORT)
            .AddInput(6, Ids.Products.Hydrogen, "D")
            .AddInput(2, ModIDs.Products.Argon, "F")
            .SetDurationSeconds(20)
            .AddOutput(10, Ids.Products.MoltenSteel, RecipeProtoBuilder.ANY_COMPATIBLE_PORT)
            .AddOutput(12, Ids.Products.Slag, RecipeProtoBuilder.ANY_COMPATIBLE_PORT)
            .AddOutput(2, Ids.Products.SteamLo, "Z")
            .SetPowerMultiplier(140.Percent())
            .BuildAndAdd();

        // Iron scrap smelting (arc).
        registrator.RecipeProtoBuilder
            .Start("Iron scrap smelting (arc)", ModIDs.Recipes.SteelFromScrapT1Arc, Ids.Machines.ArcFurnace)
            .AddInput(5, Ids.Products.IronScrap, RecipeProtoBuilder.ANY_COMPATIBLE_PORT)
            .AddInput(1, Ids.Products.Graphite, RecipeProtoBuilder.ANY_COMPATIBLE_PORT)
            .SetDurationSeconds(20)
            .AddOutput(5, Ids.Products.MoltenSteel, RecipeProtoBuilder.ANY_COMPATIBLE_PORT)
            .AddOutput(1, Ids.Products.CarbonDioxide, "E", true)
            .SetPowerMultiplier(60.Percent())
            .BuildAndAdd();
        // Sponge iron smelting (arc).
        registrator.RecipeProtoBuilder
            .Start("Steel smelting (arc)", ModIDs.Recipes.SteelFromSpongeT1Arc, Ids.Machines.ArcFurnace)
            .AddInput(10, ModIDs.Products.SpongeIron, RecipeProtoBuilder.ANY_COMPATIBLE_PORT)
            .AddInput(1, Ids.Products.Limestone, RecipeProtoBuilder.ANY_COMPATIBLE_PORT)
            .AddInput(1, Ids.Products.Graphite, RecipeProtoBuilder.ANY_COMPATIBLE_PORT)
            .SetDurationSeconds(20)
            .AddOutput(5, Ids.Products.MoltenSteel, RecipeProtoBuilder.ANY_COMPATIBLE_PORT)
            .AddOutput(5, Ids.Products.Slag, RecipeProtoBuilder.ANY_COMPATIBLE_PORT)
            .AddOutput(3, Ids.Products.Exhaust, "E", true)
            .SetPowerMultiplier(70.Percent())
            .BuildAndAdd();

        // Iron scrap smelting (arc 2).
        registrator.RecipeProtoBuilder
            .Start("Iron scrap smelting (arc)", ModIDs.Recipes.SteelFromScrapT2Arc, Ids.Machines.ArcFurnace2)
            .AddInput(10, Ids.Products.IronScrap, RecipeProtoBuilder.ANY_COMPATIBLE_PORT)
            .AddInput(1, Ids.Products.Graphite, RecipeProtoBuilder.ANY_COMPATIBLE_PORT)
            .AddInput(2, Ids.Products.Water, "D")
            .SetDurationSeconds(20)
            .AddOutput(10, Ids.Products.MoltenSteel, RecipeProtoBuilder.ANY_COMPATIBLE_PORT)
            .AddOutput(2, Ids.Products.SteamLo, "Z")
            .AddOutput(1, Ids.Products.CarbonDioxide, "E", true)
            .SetPowerMultiplier(60.Percent())
            .BuildAndAdd();
        // Sponge iron smelting (arc 2).
        registrator.RecipeProtoBuilder
            .Start("Steel smelting (arc)", ModIDs.Recipes.SteelFromSpongeT2Arc, Ids.Machines.ArcFurnace2)
            .AddInput(20, ModIDs.Products.SpongeIron, RecipeProtoBuilder.ANY_COMPATIBLE_PORT)
            .AddInput(2, Ids.Products.Limestone, RecipeProtoBuilder.ANY_COMPATIBLE_PORT)
            .AddInput(1, Ids.Products.Graphite, RecipeProtoBuilder.ANY_COMPATIBLE_PORT)
            .AddInput(2, Ids.Products.Water, "D")
            .SetDurationSeconds(20)
            .AddOutput(10, Ids.Products.MoltenSteel, RecipeProtoBuilder.ANY_COMPATIBLE_PORT)
            .AddOutput(10, Ids.Products.Slag, RecipeProtoBuilder.ANY_COMPATIBLE_PORT)
            .AddOutput(2, Ids.Products.SteamLo, "Z")
            .AddOutput(6, Ids.Products.Exhaust, "E", true)
            .SetPowerMultiplier(70.Percent())
            .BuildAndAdd();

        // Steel smelting.
        registrator.RecipeProtoBuilder
            .Start("Steel smelting", ModIDs.Recipes.SteelSmelting, Ids.Machines.OxygenFurnace)
            .AddInput(10, Ids.Products.MoltenIron, "A")
            .AddInput(2, Ids.Products.IronScrap, "C")
            .AddInput(12, Ids.Products.Oxygen, "B")
            .SetDurationSeconds(40)
            .AddOutput(10, Ids.Products.MoltenSteel, "X")
            .AddOutput(2, Ids.Products.Slag, "Y")
            .AddOutput(16, Ids.Products.Exhaust, "S")
            .BuildAndAdd();
        // Steel smelting DRI.
        registrator.RecipeProtoBuilder
            .Start("Steel smelting", ModIDs.Recipes.SteelSmeltingDIR, Ids.Machines.OxygenFurnace)
            .AddInput(10, Ids.Products.MoltenIron, "A")
            .AddInput(2, ModIDs.Products.SpongeIron, "C")
            .AddInput(12, Ids.Products.Oxygen, "B")
            .SetDurationSeconds(40)
            .AddOutput(10, Ids.Products.MoltenSteel, "X")
            .AddOutput(2, Ids.Products.Slag, "Y")
            .AddOutput(16, Ids.Products.Exhaust, "S")
            .BuildAndAdd();

        // Steel smelting (T2).
        registrator.RecipeProtoBuilder
            .Start("Steel smelting", ModIDs.Recipes.SteelSmeltingT2, Ids.Machines.OxygenFurnaceT2)
            .AddInput(20, Ids.Products.MoltenIron, "A")
            .AddInput(4, Ids.Products.IronScrap, "C")
            .AddInput(12, Ids.Products.Oxygen, "B")
            .SetDurationSeconds(40)
            .AddOutput(20, Ids.Products.MoltenSteel, "X")
            .AddOutput(4, Ids.Products.Slag, "Y")
            .AddOutput(24, Ids.Products.Exhaust, "S")
            .BuildAndAdd();
        // Steel smelting DRI (T2).
        registrator.RecipeProtoBuilder
            .Start("Steel smelting", ModIDs.Recipes.SteelSmeltingT2DRI, Ids.Machines.OxygenFurnaceT2)
            .AddInput(20, Ids.Products.MoltenIron, "A")
            .AddInput(4, ModIDs.Products.SpongeIron, "C")
            .AddInput(12, Ids.Products.Oxygen, "B")
            .SetDurationSeconds(40)
            .AddOutput(20, Ids.Products.MoltenSteel, "X")
            .AddOutput(4, Ids.Products.Slag, "Y")
            .AddOutput(24, Ids.Products.Exhaust, "S")
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
        // Cooled steel casting.
        registrator.PrototypesDb.RemoveRecipeFromMachine(Ids.Machines.CasterCooled, Ids.Recipes.SteelCastingCooled);
        registrator.RecipeProtoBuilder
            .Start("Steel casting (cooled)", ModIDs.Recipes.SteelCastingCooled, Ids.Machines.CasterCooled)
            .AddInput(10, Ids.Products.MoltenSteel, "A")
            .AddInput(2, Ids.Products.Water, "B")
            .SetDurationSeconds(40)
            .AddOutput(10, Ids.Products.Steel, "X")
            .BuildAndAdd();
        // Cooled steel casting (T2).
        registrator.PrototypesDb.RemoveRecipeFromMachine(Ids.Machines.CasterCooledT2, Ids.Recipes.SteelCastingCooledT2);
        registrator.RecipeProtoBuilder
            .Start("Steel casting (cooled)", ModIDs.Recipes.SteelCastingCooledT2, Ids.Machines.CasterCooledT2)
            .AddInput(10, Ids.Products.MoltenSteel, "A")
            .AddInput(2, Ids.Products.Water, "B")
            .SetDurationSeconds(20)
            .AddOutput(10, Ids.Products.Steel, "X")
            .BuildAndAdd();
    }

    private void AddSecondaryRecipes(ProtoRegistrator registrator)
    {
        // Flaring of coal tar.
        registrator.RecipeProtoBuilder
            .Start("Coal tar disposal", ModIDs.Recipes.FlareCoalTar, Ids.Machines.Flare)
            .SetProductsDestroyReason(DestroyReason.DumpedOnTerrain)
            .AddInput(8, ModIDs.Products.CoalTar, RecipeProtoBuilder.ANY_COMPATIBLE_PORT)
            .AddOutput(RecipeProtoBuilder.VIRTUAL_PORT, Ids.Products.PollutedAir, 6.Quantity())
            .EnablePartialExecution(1.Percent())
            .SetDurationSeconds(20)
            .BuildAndAdd();

        // Steam generation from coke.
        registrator.RecipeProtoBuilder
            .Start("Steam generation", ModIDs.Recipes.SteamGenerationCoke, Ids.Machines.BoilerCoal)
            .AddInput(8, Ids.Products.Water, RecipeProtoBuilder.ANY_COMPATIBLE_PORT)
            .AddInput(4, ModIDs.Products.Coke, RecipeProtoBuilder.ANY_COMPATIBLE_PORT)
            .SetDurationSeconds(10)
            .AddOutput(8, Ids.Products.SteamHi, "X")
            .AddOutput(5, Ids.Products.CarbonDioxide, "Y")
            .BuildAndAdd();

        // Super steam from coke.
        registrator.RecipeProtoBuilder
            .Start("Super steam (coke)", ModIDs.Recipes.SuperSteamGenerationCoke, Ids.Machines.BoilerCoal)
            .AddInput(4, Ids.Products.Water, RecipeProtoBuilder.ANY_COMPATIBLE_PORT)
            .AddInput(4, ModIDs.Products.Coke, RecipeProtoBuilder.ANY_COMPATIBLE_PORT)
            .SetDurationSeconds(10)
            .AddOutput(4, Ids.Products.SteamSp, "X")
            .AddOutput(5, Ids.Products.CarbonDioxide, "Y")
            .BuildAndAdd();

        // Coal tar processing.
        registrator.RecipeProtoBuilder
            .Start("Coal tar distillation", ModIDs.Recipes.CoalTarProcessing, Ids.Machines.DistillationTowerT1)
            .AddInput(8, ModIDs.Products.CoalTar, "B")
            .AddInput(3, Ids.Products.SteamHi, "A")
            .SetDurationSeconds(20)
            .AddOutput(4, Ids.Products.MediumOil, "X")
            .AddOutput(4, Ids.Products.HeavyOil, "Z")
            .AddOutput(6, Ids.Products.SourWater, "Y")
            .BuildAndAdd();

        // Synthetic morphine.
        registrator.RecipeProtoBuilder
            .Start("Synthetic morphine", ModIDs.Recipes.SyntheticMorphine, Ids.Machines.ChemicalPlant2)
            .AddInput(4, ModIDs.Products.CoalTar, "A")
            .AddInput(3, Ids.Products.Chlorine, "B")
            .AddInput(2, Ids.Products.SteamHi, "C")
            .AddInput(2, Ids.Products.Glass, RecipeProtoBuilder.ANY_COMPATIBLE_PORT)
            .SetDurationSeconds(20)
            .AddOutput(8, Ids.Products.Morphine, RecipeProtoBuilder.ANY_COMPATIBLE_PORT)
            .AddOutput(4, Ids.Products.SourWater, RecipeProtoBuilder.ANY_COMPATIBLE_PORT)
            .BuildAndAdd();

        // Syngas reforming.
        registrator.RecipeProtoBuilder
            .Start("Syngas reformation (cycle)", ModIDs.Recipes.SyngasProduction, Ids.Machines.HydrogenReformer)
            .AddInput(18, ModIDs.Products.SyngasUsed, "A")
            .AddInput(18, Ids.Products.FuelGas, "B")
            .SetDurationSeconds(30)
            .AddOutput(18, ModIDs.Products.Syngas, "X")
            .AddOutput(18, Ids.Products.CarbonDioxide, "Y")
            .BuildAndAdd();
        registrator.RecipeProtoBuilder
            .Start("Syngas reformation", ModIDs.Recipes.SyngasProductionInitial, Ids.Machines.HydrogenReformer)
            .AddInput(18, Ids.Products.FuelGas, "B")
            .SetDurationSeconds(30)
            .AddOutput(9, ModIDs.Products.Syngas, "X")
            .AddOutput(9, Ids.Products.CarbonDioxide, "Y")
            .BuildAndAdd();

        // Smoke stack for used syngas.
        registrator.RecipeProtoBuilder
            .Start("Product disposal", ModIDs.Recipes.SyngasSmokeStack, Ids.Machines.SmokeStack)
            .SetProductsDestroyReason(DestroyReason.DumpedOnTerrain)
            .SetDurationSeconds(20)
            .EnablePartialExecution(25.Percent())
            .AddInput(20, ModIDs.Products.SyngasUsed, RecipeProtoBuilder.ANY_COMPATIBLE_PORT)
            .AddOutput(10, Ids.Products.PollutedAir, RecipeProtoBuilder.VIRTUAL_PORT)
            .BuildAndAdd();
        registrator.RecipeProtoBuilder
            .Start("Product disposal", ModIDs.Recipes.SyngasSmokeStackLarge, Ids.Machines.SmokeStackLarge)
            .SetProductsDestroyReason(DestroyReason.DumpedOnTerrain)
            .SetDurationSeconds(20)
            .EnablePartialExecution(25.Percent())
            .AddInput(20 * 15, ModIDs.Products.SyngasUsed, RecipeProtoBuilder.ANY_COMPATIBLE_PORT)
            .AddOutput(10 * 15, Ids.Products.PollutedAir, RecipeProtoBuilder.VIRTUAL_PORT)
            .BuildAndAdd();
    }

    private void AddAssemblyRecipes(ProtoRegistrator registrator)
    {
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
    }

    private void ApplyOverrides(ProtosDb protosDb)
    {
        // Remove smelting recipes.
        protosDb.GetOrThrow<RecipeProto>(Ids.Recipes.IronSmeltingT1Coal).MarkAsObsolete();
        protosDb.GetOrThrow<RecipeProto>(Ids.Recipes.IronSmeltingT1Scrap).MarkAsObsolete();
        protosDb.GetOrThrow<RecipeProto>(Ids.Recipes.IronSmeltingArcT1).MarkAsObsolete();
        protosDb.GetOrThrow<RecipeProto>(Ids.Recipes.IronSmeltingArcScrapT1).MarkAsObsolete();
        protosDb.GetOrThrow<RecipeProto>(Ids.Recipes.IronSmeltingT2).MarkAsObsolete();
        protosDb.GetOrThrow<RecipeProto>(Ids.Recipes.IronSmeltingT2Scrap).MarkAsObsolete();
        protosDb.GetOrThrow<RecipeProto>(Ids.Recipes.IronSmeltingArc).MarkAsObsolete();
        protosDb.GetOrThrow<RecipeProto>(Ids.Recipes.IronSmeltingArcScrap).MarkAsObsolete();
        protosDb.GetOrThrow<RecipeProto>(Ids.Recipes.SteelSmelting).MarkAsObsolete();
        protosDb.GetOrThrow<RecipeProto>(Ids.Recipes.SteelSmeltingT2).MarkAsObsolete();

        // Remove iron casting recipes.
        protosDb.GetOrThrow<RecipeProto>(Ids.Recipes.IronCasting).MarkAsObsolete();
        protosDb.GetOrThrow<RecipeProto>(Ids.Recipes.IronCastingT2).MarkAsObsolete();
        protosDb.GetOrThrow<RecipeProto>(Ids.Recipes.SteelCastingCooled).MarkAsObsolete();
        protosDb.GetOrThrow<RecipeProto>(Ids.Recipes.SteelCastingCooledT2).MarkAsObsolete();

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
