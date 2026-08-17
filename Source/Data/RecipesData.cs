using CoI.MetallurgyPlus.Extensions;
using Mafi;
using Mafi.Base;
using Mafi.Core.Factory.Machines;
using Mafi.Core.Factory.Recipes;
using Mafi.Core.Mods;
using Mafi.Core.Products;
using Mafi.Core.Prototypes;

namespace CoI.MetallurgyPlus.Data;

internal class RecipesData : IModData
{
    public bool KeepIronCasting { get; set; }

    public void RegisterData(ProtoRegistrator registrator)
    {
        AddCharcoalRecipes(registrator);
        AddSmeltingRecipes(registrator);
        AddSecondaryRecipes(registrator);
        AddAssemblyRecipes(registrator);

        // Apply overrides for existing recipes.
        ApplyOverrides(registrator.PrototypesDb);
        RegisterRecipeMigrations(registrator);
    }

    private void RegisterRecipeMigrations(ProtoRegistrator registrator)
    {
        const int V_U42 = 316;

        // Shared recipes added with Update 4.2 (0.8.6+).
        registrator.RegisterRecipeMigration("Recipe_MP_CpAssemblySteelT1", Ids.Recipes.CpAssemblySteel, V_U42);
        registrator.RegisterRecipeMigration("Recipe_MP_CpAssemblySteelT2", Ids.Recipes.CpAssemblySteel, V_U42);
        registrator.RegisterRecipeMigration("Recipe_MP_MechPartsAssemblyT1Steel", ModIDs.Recipes.MechPartsAssemblySteel, V_U42);
        registrator.RegisterRecipeMigration("Recipe_MP_MechPartsAssemblyT2Steel", ModIDs.Recipes.MechPartsAssemblySteel, V_U42);
        registrator.RegisterRecipeMigration("Recipe_MP_SteelCastingT2", ModIDs.Recipes.SteelCasting, V_U42);
        registrator.RegisterRecipeMigration("Recipe_MP_SteelCastingCooledT2", ModIDs.Recipes.SteelCastingCooled, V_U42);
    }

    private void AddCharcoalRecipes(ProtoRegistrator registrator)
    {
        // Steam generation from charcoal.
        registrator.RecipeProtoBuilder
            .Start(ModIDs.Recipes.SteamGenerationCharcoal)
            .AddInput(8, Ids.Products.Water)
            .AddInput(5, ModIDs.Products.Charcoal)
            .AddOutput(8, Ids.Products.SteamHi, "X")
            .AddOutput(5, Ids.Products.Exhaust, "Y")
            .BuildAndAdd().BindTo(registrator.PrototypesDb.GetMachine(Ids.Machines.BoilerCoal), 10.Seconds());

        // Basic concrete (charcoal).
        registrator.RecipeProtoBuilder
            .Start(ModIDs.Recipes.SimpleConcreteCharcoal)
            .AddInput(12, Ids.Products.Limestone)
            .AddInput(3, ModIDs.Products.Charcoal)
            .AddInput(4, Ids.Products.Water)
            .AddOutput(8, Ids.Products.ConcreteSlab)
            .AddOutput(8, Ids.Products.Exhaust)
            .BuildAndAdd().BindTo(registrator.PrototypesDb.GetMachine(Ids.Machines.BricksMaker), 40.Seconds());

        // Diesel distillation (basic)
        registrator.RecipeProtoBuilder
            .Start(ModIDs.Recipes.DieselDistillationCharcoal)
            .AddInput(20, Ids.Products.CrudeOil)
            .AddInput(3, ModIDs.Products.Charcoal)
            .AddOutput(9, Ids.Products.Diesel, "X")
            .AddOutput(5, Ids.Products.WasteWater, "Z")
            .AddOutput(12, Ids.Products.Exhaust, "S")
            .BuildAndAdd().BindTo(registrator.PrototypesDb.GetMachine(Ids.Machines.BasicDieselDistiller), 20.Seconds());

        // Water desalination (basic)
        registrator.RecipeProtoBuilder
            .Start(ModIDs.Recipes.WaterDesalinationCharcoal)
            .AddInput(10, Ids.Products.Seawater)
            .AddInput(1, ModIDs.Products.Charcoal)
            .AddOutput(6, Ids.Products.Water, "X")
            .AddOutput(4, Ids.Products.Brine, "Z")
            .AddOutput(2, Ids.Products.Exhaust, "S", outputAtStart: true)
            .BuildAndAdd().BindTo(registrator.PrototypesDb.GetMachine(Ids.Machines.BasicDieselDistiller), 10.Seconds());

        // Rubber production (charcoal)
        registrator.RecipeProtoBuilder
            .Start(ModIDs.Recipes.RubberProductionNaphthaCharcoal)
            .AddInput(4, Ids.Products.Naphtha)
            .AddInput(2, ModIDs.Products.Charcoal)
            .AddOutput(8, Ids.Products.Rubber, "X")
            .AddOutput(1, Ids.Products.WasteWater, "Y")
            .BuildAndAdd().BindTo(registrator.PrototypesDb.GetMachine(Ids.Machines.VacuumDistillationTower), 30.Seconds());

        // Rubber production (charcoal) (alt)
        registrator.RecipeProtoBuilder
            .Start(ModIDs.Recipes.RubberProductionDieselWithCharcaol)
            .AddInput(4, Ids.Products.Diesel)
            .AddInput(2, ModIDs.Products.Charcoal)
            .AddOutput(8, Ids.Products.Rubber, "X")
            .AddOutput(3, Ids.Products.WasteWater, "Y")
            .BuildAndAdd().BindTo(registrator.PrototypesDb.GetMachine(Ids.Machines.VacuumDistillationTower), 30.Seconds());

        // Copper smelting (charcoal).
        registrator.RecipeProtoBuilder
            .Start(ModIDs.Recipes.CopperSmeltingT1Charcoal)
            .AddInput(10, Ids.Products.CopperOre)
            .AddInput(5, ModIDs.Products.Charcoal)
            .AddOutput(8, Ids.Products.MoltenCopper)
            .AddOutput(4, Ids.Products.Slag)
            .AddOutput(8, Ids.Products.Exhaust, outputAtStart: true)
            .BuildAndAdd().BindTo(registrator.PrototypesDb.GetMachine(Ids.Machines.SmeltingFurnaceT1), 20.Seconds());
    }

    private void AddSmeltingRecipes(ProtoRegistrator registrator)
    {
        // Iron ore smelting (charcoal).
        registrator.RecipeProtoBuilder
            .Start(ModIDs.Recipes.IronSmeltingT1Charcoal)
            .AddInput(10, Ids.Products.IronOre)
            .AddInput(3, ModIDs.Products.Charcoal)
            .AddOutput(5, Ids.Products.MoltenIron)
            .AddOutput(5, Ids.Products.Slag)
            .AddOutput(5, Ids.Products.Exhaust, outputAtStart: true)
            .BuildAndAdd().BindTo(registrator.PrototypesDb.GetMachine(Ids.Machines.SmeltingFurnaceT1), 20.Seconds());

        // Iron ore smelting (coke).
        registrator.RecipeProtoBuilder
            .Start(ModIDs.Recipes.IronSmeltingT1Coke)
            .AddInput(10, Ids.Products.IronOre)
            .AddInput(3, ModIDs.Products.Coke)
            .AddOutput(5, Ids.Products.MoltenIron)
            .AddOutput(5, Ids.Products.Slag)
            .AddOutput(4, Ids.Products.Exhaust, outputAtStart: true)
            .BuildAndAdd().BindTo(registrator.PrototypesDb.GetMachine(Ids.Machines.SmeltingFurnaceT1), 20.Seconds());

        // Copper scrap smelting (charcoal).
        registrator.RecipeProtoBuilder
            .Start(ModIDs.Recipes.CopperSmeltingT1ScrapCharcoal)
            .AddInput(8, Ids.Products.CopperScrap)
            .AddInput(3, ModIDs.Products.Charcoal)
            .AddOutput(8, Ids.Products.MoltenCopper)
            .AddOutput(6, Ids.Products.Exhaust, outputAtStart: true)
            .BuildAndAdd().BindTo(registrator.PrototypesDb.GetMachine(Ids.Machines.SmeltingFurnaceT1), 20.Seconds());

        // Iron ore smelting (T2).
        registrator.RecipeProtoBuilder
            .Start(ModIDs.Recipes.IronSmeltingT2)
            .AddInput(16, Ids.Products.IronOreCrushed)
            .AddInput(2, Ids.Products.Limestone)
            .AddInput(4, ModIDs.Products.Coke)
            .AddOutput(10, Ids.Products.MoltenIron)
            .AddOutput(6, Ids.Products.Slag)
            .AddOutput(5, Ids.Products.CarbonDioxide, outputAtStart: true)
            .BuildAndAdd().BindTo(registrator.PrototypesDb.GetMachine(Ids.Machines.SmeltingFurnaceT2), 20.Seconds());

        // Iron ore smelting (arc).
        registrator.RecipeProtoBuilder
            .Start(ModIDs.Recipes.IronSmeltingArc)
            .AddInput(20, Ids.Products.IronOreCrushed)
            .AddInput(3, Ids.Products.Limestone)
            .AddInput(1, Ids.Products.Graphite)
            .AddInput(6, Ids.Products.Hydrogen, "D")
            .AddInput(2, ModIDs.Products.Argon, "F")
            .AddOutput(10, Ids.Products.MoltenSteel)
            .AddOutput(12, Ids.Products.Slag)
            .AddOutput(2, Ids.Products.SteamLo, "Z")
            .SetPowerMultiplier(140.Percent())
            .BuildAndAdd().BindTo(registrator.PrototypesDb.GetMachine(Ids.Machines.ArcFurnace2), 20.Seconds());

        // Iron scrap smelting (arc).
        registrator.RecipeProtoBuilder
            .Start(ModIDs.Recipes.SteelFromScrapT1Arc)
            .AddInput(5, Ids.Products.IronScrap)
            .AddInput(1, Ids.Products.Graphite)
            .AddOutput(5, Ids.Products.MoltenSteel)
            .AddOutput(1, Ids.Products.CarbonDioxide, "E", true)
            .SetPowerMultiplier(60.Percent())
            .BuildAndAdd().BindTo(registrator.PrototypesDb.GetMachine(Ids.Machines.ArcFurnace), 20.Seconds());
        // Sponge iron smelting (arc).
        registrator.RecipeProtoBuilder
            .Start(ModIDs.Recipes.SteelFromSpongeT1Arc)
            .AddInput(10, ModIDs.Products.SpongeIron)
            .AddInput(1, Ids.Products.Limestone)
            .AddInput(1, Ids.Products.Graphite)
            .AddOutput(5, Ids.Products.MoltenSteel)
            .AddOutput(5, Ids.Products.Slag)
            .AddOutput(3, Ids.Products.Exhaust, "E", true)
            .SetPowerMultiplier(70.Percent())
            .BuildAndAdd().BindTo(registrator.PrototypesDb.GetMachine(Ids.Machines.ArcFurnace), 20.Seconds());

        // Iron scrap smelting (arc 2).
        registrator.RecipeProtoBuilder
            .Start(ModIDs.Recipes.SteelFromScrapT2Arc)
            .AddInput(10, Ids.Products.IronScrap)
            .AddInput(1, Ids.Products.Graphite)
            .AddInput(2, Ids.Products.Water, "D")
            .AddOutput(10, Ids.Products.MoltenSteel)
            .AddOutput(2, Ids.Products.SteamLo, "Z")
            .AddOutput(1, Ids.Products.CarbonDioxide, "E", true)
            .SetPowerMultiplier(60.Percent())
            .BuildAndAdd().BindTo(registrator.PrototypesDb.GetMachine(Ids.Machines.ArcFurnace2), 20.Seconds());
        // Sponge iron smelting (arc 2).
        registrator.RecipeProtoBuilder
            .Start(ModIDs.Recipes.SteelFromSpongeT2Arc)
            .AddInput(20, ModIDs.Products.SpongeIron)
            .AddInput(2, Ids.Products.Limestone)
            .AddInput(1, Ids.Products.Graphite)
            .AddInput(2, Ids.Products.Water, "D")
            .AddOutput(10, Ids.Products.MoltenSteel)
            .AddOutput(10, Ids.Products.Slag)
            .AddOutput(2, Ids.Products.SteamLo, "Z")
            .AddOutput(6, Ids.Products.Exhaust, "E", true)
            .SetPowerMultiplier(70.Percent())
            .BuildAndAdd().BindTo(registrator.PrototypesDb.GetMachine(Ids.Machines.ArcFurnace2), 20.Seconds());

        // Steel smelting.
        registrator.RecipeProtoBuilder
            .Start(ModIDs.Recipes.SteelSmelting)
            .AddInput(10, Ids.Products.MoltenIron, "A")
            .AddInput(2, Ids.Products.IronScrap, "C")
            .AddInput(12, Ids.Products.Oxygen, "B")
            .AddOutput(10, Ids.Products.MoltenSteel, "X")
            .AddOutput(2, Ids.Products.Slag, "Y")
            .AddOutput(16, Ids.Products.Exhaust, "S")
            .BuildAndAdd().BindTo(registrator.PrototypesDb.GetMachine(Ids.Machines.OxygenFurnace), 40.Seconds());
        // Steel smelting DRI.
        registrator.RecipeProtoBuilder
            .Start(ModIDs.Recipes.SteelSmeltingDIR)
            .AddInput(10, Ids.Products.MoltenIron, "A")
            .AddInput(2, ModIDs.Products.SpongeIron, "C")
            .AddInput(12, Ids.Products.Oxygen, "B")
            .AddOutput(10, Ids.Products.MoltenSteel, "X")
            .AddOutput(2, Ids.Products.Slag, "Y")
            .AddOutput(16, Ids.Products.Exhaust, "S")
            .BuildAndAdd().BindTo(registrator.PrototypesDb.GetMachine(Ids.Machines.OxygenFurnace), 40.Seconds());

        // Steel smelting (T2).
        registrator.RecipeProtoBuilder
            .Start(ModIDs.Recipes.SteelSmeltingT2)
            .AddInput(20, Ids.Products.MoltenIron, "A")
            .AddInput(4, Ids.Products.IronScrap, "C")
            .AddInput(12, Ids.Products.Oxygen, "B")
            .AddOutput(20, Ids.Products.MoltenSteel, "X")
            .AddOutput(4, Ids.Products.Slag, "Y")
            .AddOutput(24, Ids.Products.Exhaust, "S")
            .BuildAndAdd().BindTo(registrator.PrototypesDb.GetMachine(Ids.Machines.OxygenFurnaceT2), 40.Seconds());
        // Steel smelting DRI (T2).
        registrator.RecipeProtoBuilder
            .Start(ModIDs.Recipes.SteelSmeltingT2DRI)
            .AddInput(20, Ids.Products.MoltenIron, "A")
            .AddInput(4, ModIDs.Products.SpongeIron, "C")
            .AddInput(12, Ids.Products.Oxygen, "B")
            .AddOutput(20, Ids.Products.MoltenSteel, "X")
            .AddOutput(4, Ids.Products.Slag, "Y")
            .AddOutput(24, Ids.Products.Exhaust, "S")
            .BuildAndAdd().BindTo(registrator.PrototypesDb.GetMachine(Ids.Machines.OxygenFurnaceT2), 40.Seconds());

        // Basic steel casting.
        registrator.RecipeProtoBuilder
            .Start(ModIDs.Recipes.SteelCasting)
            .AddInput(6, Ids.Products.MoltenSteel, RecipeProtoBuilder.ANY_COMPATIBLE_PORT)
            .AddOutput(6, Ids.Products.Steel, RecipeProtoBuilder.ANY_COMPATIBLE_PORT)
            .BuildAndAdd()
            .BindTo(registrator.PrototypesDb.GetMachine(Ids.Machines.Caster), 60.Seconds())
            .BindTo(registrator.PrototypesDb.GetMachine(Ids.Machines.CasterT2), 30.Seconds());

        // Cooled steel casting.
        registrator.PrototypesDb.RemoveRecipeFromMachine(Ids.Machines.CasterCooled, Ids.Recipes.SteelCastingCooled);
        registrator.PrototypesDb.RemoveRecipeFromMachine(Ids.Machines.CasterCooledT2, Ids.Recipes.SteelCastingCooled);
        registrator.RecipeProtoBuilder
            .Start(ModIDs.Recipes.SteelCastingCooled)
            .AddInput(10, Ids.Products.MoltenSteel, "A")
            .AddInput(2, Ids.Products.Water, "B")
            .AddOutput(10, Ids.Products.Steel, "X")
            .BuildAndAdd()
            .BindTo(registrator.PrototypesDb.GetMachine(Ids.Machines.CasterCooled), 40.Seconds())
            .BindTo(registrator.PrototypesDb.GetMachine(Ids.Machines.CasterCooledT2), 20.Seconds());
    }

    private void AddSecondaryRecipes(ProtoRegistrator registrator)
    {
        // Flaring of coal tar.
        registrator.RecipeProtoBuilder
            .Start(ModIDs.Recipes.FlareCoalTar)
            .SetProductsDestroyReason(DestroyReason.DumpedOnTerrain)
            .AddInput(8, ModIDs.Products.CoalTar)
            .AddOutput(8, Ids.Products.PollutedAir)
            .BuildAndAdd().BindTo(registrator.PrototypesDb.GetMachine(Ids.Machines.Flare), 20.Seconds(), minPartialUtilization: 1.Percent());

        // Steam generation from coke.
        registrator.RecipeProtoBuilder
            .Start(ModIDs.Recipes.SteamGenerationCoke)
            .AddInput(8, Ids.Products.Water)
            .AddInput(4, ModIDs.Products.Coke)
            .AddOutput(8, Ids.Products.SteamHi, "X")
            .AddOutput(5, Ids.Products.CarbonDioxide, "Y")
            .BuildAndAdd().BindTo(registrator.PrototypesDb.GetMachine(Ids.Machines.BoilerCoal), 10.Seconds());

        // Super steam from coke.
        registrator.RecipeProtoBuilder
            .Start(ModIDs.Recipes.SuperSteamGenerationCoke)
            .AddInput(4, Ids.Products.Water)
            .AddInput(4, ModIDs.Products.Coke)
            .AddOutput(4, Ids.Products.SteamSp, "X")
            .AddOutput(5, Ids.Products.CarbonDioxide, "Y")
            .BuildAndAdd().BindTo(registrator.PrototypesDb.GetMachine(Ids.Machines.BoilerCoal), 10.Seconds());

        // Coal tar processing.
        registrator.RecipeProtoBuilder
            .Start(ModIDs.Recipes.CoalTarProcessing)
            .AddInput(8, ModIDs.Products.CoalTar, "B")
            .AddInput(3, Ids.Products.SteamHi, "A")
            .AddOutput(4, Ids.Products.MediumOil, "X")
            .AddOutput(4, Ids.Products.HeavyOil, "Z")
            .AddOutput(6, Ids.Products.SourWater, "Y")
            .BuildAndAdd().BindTo(registrator.PrototypesDb.GetMachine(Ids.Machines.DistillationTowerT1), 20.Seconds());

        // Synthetic morphine.
        registrator.RecipeProtoBuilder
            .Start(ModIDs.Recipes.SyntheticMorphine)
            .AddInput(4, ModIDs.Products.CoalTar, "A")
            .AddInput(3, Ids.Products.Chlorine, "B")
            .AddInput(2, Ids.Products.SteamHi, "C")
            .AddInput(2, Ids.Products.Glass)
            .AddOutput(8, Ids.Products.Morphine)
            .AddOutput(4, Ids.Products.SourWater)
            .BuildAndAdd().BindTo(registrator.PrototypesDb.GetMachine(Ids.Machines.ChemicalPlant2), 20.Seconds());

        // Syngas reforming.
        registrator.RecipeProtoBuilder
            .Start(ModIDs.Recipes.SyngasProduction)
            .AddInput(18, ModIDs.Products.SyngasUsed, "A")
            .AddInput(18, Ids.Products.FuelGas, "B")
            .AddOutput(18, ModIDs.Products.Syngas, "X")
            .AddOutput(18, Ids.Products.CarbonDioxide, "Y")
            .BuildAndAdd().BindTo(registrator.PrototypesDb.GetMachine(Ids.Machines.HydrogenReformer), 30.Seconds());
        registrator.RecipeProtoBuilder
            .Start(ModIDs.Recipes.SyngasProductionInitial)
            .AddInput(18, Ids.Products.FuelGas, "B")
            .AddOutput(9, ModIDs.Products.Syngas, "X")
            .AddOutput(9, Ids.Products.CarbonDioxide, "Y")
            .BuildAndAdd().BindTo(registrator.PrototypesDb.GetMachine(Ids.Machines.HydrogenReformer), 30.Seconds());

        // Smoke stack for used syngas.
        registrator.RecipeProtoBuilder
            .Start(ModIDs.Recipes.SyngasSmokeStack)
            .SetProductsDestroyReason(DestroyReason.DumpedOnTerrain)
            .AddInput(20, ModIDs.Products.SyngasUsed)
            .AddOutput(10, Ids.Products.PollutedAir)
            .BuildAndAdd().BindTo(registrator.PrototypesDb.GetMachine(Ids.Machines.SmokeStack), 20.Seconds(),  minPartialUtilization: 25.Percent());
        registrator.RecipeProtoBuilder
            .Start(ModIDs.Recipes.SyngasSmokeStackLarge)
            .SetProductsDestroyReason(DestroyReason.DumpedOnTerrain)
            .AddInput(20 * 15, ModIDs.Products.SyngasUsed, RecipeProtoBuilder.ANY_COMPATIBLE_PORT)
            .AddOutput(10 * 15, Ids.Products.PollutedAir, RecipeProtoBuilder.VIRTUAL_PORT)
            .BuildAndAdd().BindTo(registrator.PrototypesDb.GetMachine(Ids.Machines.SmokeStackLarge), 20.Seconds(), minPartialUtilization: 25.Percent());
    }

    private void AddAssemblyRecipes(ProtoRegistrator registrator)
    {
        // CP assembly steel.
        var cpSteelBinder = new RecipeBindingBuilder(registrator.PrototypesDb.GetOrThrow<RecipeProto>(Ids.Recipes.CpAssemblySteel), default, default);
        cpSteelBinder.BindTo(registrator.PrototypesDb.GetMachine(Ids.Machines.AssemblyManual), 40.Seconds(), 1);
        cpSteelBinder.BindTo(registrator.PrototypesDb.GetMachine(Ids.Machines.AssemblyElectrified), 20.Seconds(), 1);

        // Mech. parts assembly steel.
        registrator.RecipeProtoBuilder
            .Start(ModIDs.Recipes.MechPartsAssemblySteel)
            .AddInput(2, Ids.Products.Steel, RecipeProtoBuilder.ANY_COMPATIBLE_PORT)
            .AddOutput(4, Ids.Products.MechanicalParts, RecipeProtoBuilder.ANY_COMPATIBLE_PORT)
            .BuildAndAdd()
            .BindTo(registrator.PrototypesDb.GetMachine(Ids.Machines.AssemblyManual), 40.Seconds(), 1)
            .BindTo(registrator.PrototypesDb.GetMachine(Ids.Machines.AssemblyElectrified), 40.Seconds(), 2);
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
        if (!KeepIronCasting) protosDb.GetOrThrow<RecipeProto>(Ids.Recipes.IronCasting).MarkAsObsolete();
        protosDb.GetOrThrow<RecipeProto>(Ids.Recipes.SteelCastingCooled).MarkAsObsolete();

        // Remove iron construction part recipes.
        protosDb.GetOrThrow<RecipeProto>(Ids.Recipes.CpAssembly).MarkAsObsolete();

        // Remove iron mechanical components recipes.
        protosDb.GetOrThrow<RecipeProto>(Ids.Recipes.MechPartsIronAssembly).MarkAsObsolete();
    }
}
