using CoI.MetallurgyPlus.Extensions;
using Mafi;
using Mafi.Base;
using Mafi.Core.Factory.Machines;
using Mafi.Core.Mods;
using Mafi.Core.Products;
using Mafi.Core.Prototypes;
using Mafi.Core.Research;

namespace CoI.MetallurgyPlus.Data;

internal class ResearchData : IResearchNodesData
{
    public void RegisterData(ProtoRegistrator registrator)
    {
        AddIronSmeltingOre(registrator);
        AddIronDirectReduction(registrator);
        AddGasPoweredFurnace(registrator);
        AddCokingOven(registrator);
        AddCharcoalRecipes(registrator.PrototypesDb);

        OverrideIronSmelting(registrator.PrototypesDb);
        OverrideCopperRefinement(registrator.PrototypesDb);
        OverrideVehicleAndMining(registrator.PrototypesDb);
        OverrideConstruction(registrator.PrototypesDb);

        // Adjust positions of existing research items.
        registrator.PrototypesDb.GetOrThrow<ResearchNodeProto>(Ids.Research.Beacon).GridPosition = new Vector2i(12, 5);
        registrator.PrototypesDb.GetOrThrow<ResearchNodeProto>(Ids.Research.RepairDock).GridPosition = new Vector2i(16, 5);

        registrator.PrototypesDb.GetOrThrow<ResearchNodeProto>(Ids.Research.FuelStation).GridPosition = new Vector2i(24, 15);
        registrator.PrototypesDb.GetOrThrow<ResearchNodeProto>(Ids.Research.AdvancedLogisticsControl).GridPosition = new Vector2i(28, 15);

        registrator.PrototypesDb.GetOrThrow<ResearchNodeProto>(Ids.Research.ConveyorBelts).GridPosition = new Vector2i(24, 24);
        registrator.PrototypesDb.GetOrThrow<ResearchNodeProto>(Ids.Research.TransportsLifts).GridPosition = new Vector2i(28, 24);
        registrator.PrototypesDb.GetOrThrow<ResearchNodeProto>(Ids.Research.TransportsBalancing).GridPosition = new Vector2i(28, 20);
        registrator.PrototypesDb.GetOrThrow<ResearchNodeProto>(Ids.Research.ConveyorRouting).GridPosition = new Vector2i(32, 20);

        registrator.PrototypesDb.GetOrThrow<ResearchNodeProto>(Ids.Research.UndergroundWater).GridPosition = new Vector2i(24, 28);
        registrator.PrototypesDb.GetOrThrow<ResearchNodeProto>(Ids.Research.SettlementWater).GridPosition = new Vector2i(28, 28);
        registrator.PrototypesDb.GetOrThrow<ResearchNodeProto>(Ids.Research.Housing2).GridPosition = new Vector2i(32, 28);

        registrator.PrototypesDb.GetOrThrow<ResearchNodeProto>(Ids.Research.CargoDepot2).GridPosition = new Vector2i(56, -1);
        registrator.PrototypesDb.GetOrThrow<ResearchNodeProto>(Ids.Research.ConveyorBeltsT2).GridPosition = new Vector2i(56, 3);
        registrator.PrototypesDb.GetOrThrow<ResearchNodeProto>(Ids.Research.Trains).GridPosition = new Vector2i(56, 14);
        registrator.PrototypesDb.GetOrThrow<ResearchNodeProto>(Ids.Research.TrainDepotAddon).GridPosition = new Vector2i(60, 14);
        registrator.PrototypesDb.GetOrThrow<ResearchNodeProto>(Ids.Research.VehicleCapIncrease3).GridPosition = new Vector2i(60, 41);
        registrator.PrototypesDb.GetOrThrow<ResearchNodeProto>(Ids.Research.CopperRefinement2).GridPosition = new Vector2i(68, 21);
    }

    private void AddIronSmeltingOre(ProtoRegistrator registrator)
    {
        var proto = registrator.ResearchNodeProtoBuilder
            .Start("Iron ore smelting", ModIDs.Research.IronSmeltingOre, 2)
            .Description("Production of iron from iron ore.")
            .AddMachineToUnlock(Ids.Machines.SmeltingFurnaceT1)
            .AddRecipeToUnlock(ModIDs.Recipes.IronSmeltingT1Charcoal)
            .AddRecipeToUnlock(ModIDs.Recipes.SteelFromIronT1)
            .AddRecipeToUnlock(ModIDs.Recipes.SteelFromIronT1Coal)
            .SetGridPosition(new Vector2i(12, 10))
            .AddParents(registrator.PrototypesDb.GetOrThrow<ResearchNodeProto>(Ids.Research.VehicleAndMining))
            .BuildAndAdd();

        registrator.PrototypesDb.GetOrThrow<ResearchNodeProto>(Ids.Research.CopperRefinement).AddParent(proto);
    }

    private void AddIronDirectReduction(ProtoRegistrator registrator)
    {
        var proto = registrator.ResearchNodeProtoBuilder
            .Start("Direct iron reduction", ModIDs.Research.DirectIronReduction, 16)
            .Description("Process to reduce iron ore to iron without melting it.")
            .AddProductIcon(ModIDs.Products.SpongeIron)
            .AddRecipeToUnlock(ModIDs.Recipes.IronReductionT1)
            .AddRecipeToUnlock(ModIDs.Recipes.SteelFromSpongeT1)
            .AddRecipeToUnlock(ModIDs.Recipes.SteelFromSpongeT1Coal)
            .SetGridPosition(new Vector2i(28, 11))
            .AddParents(registrator.PrototypesDb.GetOrThrow<ResearchNodeProto>(Ids.Research.ConcreteAdvanced))
            .BuildAndAdd();
    }

    private void AddGasPoweredFurnace(ProtoRegistrator registrator)
    {
        var ohFurnace = registrator.PrototypesDb.GetOrThrow<MachineProto>(ModIDs.Machines.OpenHearthFurnace);

        var proto = registrator.ResearchNodeProtoBuilder
            .Start("Gas-heated furnace", ModIDs.Research.GasPoweredFurnace, 36)
            .Description("Gas can be used to heat our furnaces, too.")
            .AddRecipeToUnlock(ModIDs.Recipes.SteelFromScrapT1FG)
            .AddRecipeToUnlock(ModIDs.Recipes.SteelFromIronT1FG)
            .AddRecipeToUnlock(ModIDs.Recipes.SteelFromSpongeT1FG)
            .AddIcon(ohFurnace, ohFurnace.IconPath)
            .AddParents(registrator.PrototypesDb.GetOrThrow<ResearchNodeProto>(Ids.Research.GasCombustion))
            .SetGridPosition(new Vector2i(48, 33))
            .BuildAndAdd();
    }

    private void AddCokingOven(ProtoRegistrator registrator)
    {
        var cokeProto = registrator.ResearchNodeProtoBuilder
            .Start("Coke making", ModIDs.Research.CokeMaking, 48)
            .Description("Manufacturing of metallurgical coke as an alternative to charcoal from wood for blast furnaces.")
            .AddProductIcon(ModIDs.Products.Coke)
            .AddMachineToUnlock(ModIDs.Machines.CokingOven, unlockAllRecipes: true)
            .AddRecipeToUnlock(ModIDs.Recipes.IronSmeltingT1Coke)
            .AddRecipeToUnlock(ModIDs.Recipes.FlareCoalTar)
            .AddParents(registrator.PrototypesDb.GetOrThrow<ResearchNodeProto>(Ids.Research.Cp3Packing))
            .SetGridPosition(new Vector2i(56, 28))
            .BuildAndAdd();

        registrator.PrototypesDb.GetOrThrow<ResearchNodeProto>(Ids.Research.AdvancedSmelting).AddParent(cokeProto);

        var boiler = registrator.PrototypesDb.GetOrThrow<MachineProto>(Ids.Machines.BoilerCoal);
        var steamProto = registrator.ResearchNodeProtoBuilder
            .Start("Steam from coke", ModIDs.Research.SteamGenerationCoke, 42)
            .Description("With small changes to the boiler, coke can be used to generate steam, too.")
            .AddIcon(boiler, boiler.IconPath)
            .AddRecipeToUnlock(ModIDs.Recipes.SteamGenerationCoke)
            .AddParents(cokeProto, registrator.PrototypesDb.GetOrThrow<ResearchNodeProto>(Ids.Research.PowerGeneration2))
            .SetGridPosition(new Vector2i(60, 36))
            .BuildAndAdd();
    }

    private void OverrideIronSmelting(ProtosDb protosDb)
    {
        // Iron smelting (from scrap)
        var proto = protosDb.GetOrThrow<ResearchNodeProto>(Ids.Research.IronSmeltingScrap);
        proto.SetName(Ids.Research.IronSmeltingScrap + "_MP__name", "Steel smelting (from scrap)", "title of a research node in the research tree");
        proto.UnitsAsEditable()
            .AddRecipeUnlock(protosDb, ModIDs.Recipes.CharcoalFromWood)
            .RemoveRecipeUnlock(Ids.Recipes.CharcoalBurning)
            .RemoveMachineUnlock(Ids.Machines.SmeltingFurnaceT1)
            .RemoveRecipeUnlock(Ids.Recipes.IronSmeltingT1Scrap)
            .AddMachineUnlock(protosDb, ModIDs.Machines.OpenHearthFurnace, unlockAllRecipes: false)
            .AddRecipeUnlock(protosDb, ModIDs.Recipes.SteelFromScrapT1)
            .RemoveRecipeUnlock(Ids.Recipes.IronCasting)
            .AddRecipeUnlock(protosDb, ModIDs.Recipes.SteelCasting)
            .SetToResearch(proto);

        proto.IconsAsEditable()
            .RemoveIcon(Ids.Machines.SmeltingFurnaceT1)
            .AddProtoIcon(protosDb.GetOrThrow<MachineProto>(ModIDs.Machines.OpenHearthFurnace), position: 0)
            .AddProductIcon(protosDb.GetOrThrow<ProductProto>(Ids.Products.Steel), position: 3)
            .SetToIcons(proto);

        // Steel smelting
        proto = protosDb.GetOrThrow<ResearchNodeProto>(Ids.Research.SteelSmelting);
        proto.SetName(Ids.Research.SteelSmelting + "_MP__name", "Oxygen furnace", "title of a research node in the research tree");
        proto.UnitsAsEditable()
            .RemoveRecipeUnlock(Ids.Recipes.SteelSmelting)
            .RemoveRecipeUnlock(Ids.Recipes.SteelCastingCooled)
            .AddRecipeUnlock(protosDb, ModIDs.Recipes.SteelSmelting)
            .AddRecipeUnlock(protosDb, ModIDs.Recipes.SteelSmeltingDIR)
            .AddRecipeUnlock(protosDb, ModIDs.Recipes.SteelCastingCooled)
            .AddMachineUnlock(protosDb, Ids.Machines.Shredder, unlockAllRecipes: false)
            .AddRecipeUnlock(protosDb, ModIDs.Recipes.ShreddingSteel)
            .SetToResearch(proto);

        proto.IconsAsEditable()
            .RemoveIcon(Ids.Products.Steel)
            .AddProtoIcon(protosDb.GetOrThrow<MachineProto>(Ids.Machines.Shredder), position: 3)
            .SetToIcons(proto);

        // Advanced smelting
        proto = protosDb.GetOrThrow<ResearchNodeProto>(Ids.Research.AdvancedSmelting);
        proto.UnitsAsEditable()
            .RemoveRecipeUnlock(Ids.Recipes.IronSmeltingT2)
            .RemoveRecipeUnlock(Ids.Recipes.IronSmeltingT2Scrap)
            .RemoveRecipeUnlock(Ids.Recipes.SteelSmeltingT2)
            .RemoveRecipeUnlock(Ids.Recipes.SteelCastingCooledT2)
            .RemoveRecipeUnlock(Ids.Recipes.IronCastingT2)
            .AddRecipeUnlock(protosDb, ModIDs.Recipes.IronSmeltingT2, position: 0)
            .AddRecipeUnlock(protosDb, ModIDs.Recipes.SteelSmeltingT2, position: 5)
            .AddRecipeUnlock(protosDb, ModIDs.Recipes.SteelSmeltingT2DRI, position: 6)
            .AddRecipeUnlock(protosDb, ModIDs.Recipes.SteelCastingCooledT2, position: 7)
            .AddRecipeUnlock(protosDb, ModIDs.Recipes.SteelCastingT2, position: 8)
            .SetToResearch(proto);

        // Arc furnace I
        proto = protosDb.GetOrThrow<ResearchNodeProto>(Ids.Research.ArcFurnaceT1);
        proto.UnitsAsEditable()
            .RemoveRecipeUnlock(Ids.Recipes.IronSmeltingArcT1)
            .RemoveRecipeUnlock(Ids.Recipes.IronSmeltingArcScrapT1)
            .AddRecipeUnlock(protosDb, ModIDs.Recipes.SteelFromScrapT1Arc, position: 0)
            .AddRecipeUnlock(protosDb, ModIDs.Recipes.SteelFromSpongeT1Arc, position: 1)
            .SetToResearch(proto);
    }

    private void OverrideCopperRefinement(ProtosDb protosDb)
    {
        // Copper refinement
        var proto = protosDb.GetOrThrow<ResearchNodeProto>(Ids.Research.CopperRefinement);
        proto.UnitsAsEditable()
            .AddRecipeUnlock(protosDb, ModIDs.Recipes.CopperSmeltingT1ScrapCharcoal)
            .AddRecipeUnlock(protosDb, ModIDs.Recipes.CopperSmeltingT1Charcoal)
            .SetToResearch(proto);
    }

    private void OverrideVehicleAndMining(ProtosDb protosDb)
    {
        // Vehicles & mining
        var proto = protosDb.GetOrThrow<ResearchNodeProto>(Ids.Research.VehicleAndMining);
        proto.UnitsAsEditable()
            .RemoveRecipeUnlock(Ids.Recipes.IronSmeltingT1Coal)
            .AddRecipeUnlock(protosDb, ModIDs.Recipes.SteelFromScrapT1Coal)
            .SetToResearch(proto);
    }

    private void OverrideConstruction(ProtosDb protosDb)
    {
        // Construction
        var proto = protosDb.GetOrThrow<ResearchNodeProto>(Ids.Research.CpPacking);
        proto.UnitsAsEditable()
            .AddRecipeUnlock(protosDb, ModIDs.Recipes.CpAssemblySteelT1)
            .RemoveRecipeUnlock(Ids.Recipes.CpAssemblyT1)
            .AddRecipeUnlock(protosDb, ModIDs.Recipes.MechPartsAssemblyT1Steel)
            .RemoveRecipeUnlock(Ids.Recipes.MechPartsAssemblyT1)
            .SetToResearch(proto);

        // Construction II
        proto = protosDb.GetOrThrow<ResearchNodeProto>(Ids.Research.Cp2Packing);
        proto.UnitsAsEditable()
            .AddRecipeUnlock(protosDb, ModIDs.Recipes.CpAssemblySteelT2)
            .RemoveRecipeUnlock(Ids.Recipes.CpAssemblyT2)
            .AddRecipeUnlock(protosDb, ModIDs.Recipes.MechPartsAssemblyT2Steel)
            .RemoveRecipeUnlock(Ids.Recipes.MechPartsAssemblyT2)
            .SetToResearch(proto);

        // Construction III
        proto = protosDb.GetOrThrow<ResearchNodeProto>(Ids.Research.Cp3Packing);
        proto.UnitsAsEditable()
            .RemoveRecipeUnlock(Ids.Recipes.CpAssemblyT3)
            .RemoveRecipeUnlock(Ids.Recipes.MechPartsAssemblyT3Iron)
            .SetToResearch(proto);

        // Robotic assembly
        proto = protosDb.GetOrThrow<ResearchNodeProto>(Ids.Research.RoboticAssembly);
        proto.UnitsAsEditable()
            .RemoveRecipeUnlock(Ids.Recipes.CpAssemblyT4)
            .RemoveRecipeUnlock(Ids.Recipes.MechPartsAssemblyT4Iron)
            .SetToResearch(proto);

        // Robotic assembly II
        proto = protosDb.GetOrThrow<ResearchNodeProto>(Ids.Research.Assembler3);
        proto.UnitsAsEditable()
            .RemoveRecipeUnlock(Ids.Recipes.CpAssemblyT5)
            .RemoveRecipeUnlock(Ids.Recipes.MechPartsAssemblyT5Iron)
            .SetToResearch(proto);
    }

    private void AddCharcoalRecipes(ProtosDb protosDb)
    {
        // Basic concrete.
        var proto = protosDb.GetOrThrow<ResearchNodeProto>(Ids.Research.BricksProduction);
        proto.UnitsAsEditable()
            .AddRecipeUnlock(protosDb, ModIDs.Recipes.SimpleConcreteCharcoal)
            .SetToResearch(proto);

        // Basic diesel
        proto = protosDb.GetOrThrow<ResearchNodeProto>(Ids.Research.BasicDiesel);
        proto.UnitsAsEditable()
            .AddRecipeUnlock(protosDb, ModIDs.Recipes.DieselDistillationCharcoal)
            .SetToResearch(proto);

        // Basic desalination
        proto = protosDb.GetOrThrow<ResearchNodeProto>(Ids.Research.ThermalDesalinationBasic);
        proto.UnitsAsEditable()
            .AddRecipeUnlock(protosDb, ModIDs.Recipes.WaterDesalinationCharcoal)
            .SetToResearch(proto);

        // Synthetic rubber
        proto = protosDb.GetOrThrow<ResearchNodeProto>(Ids.Research.RubberProduction);
        proto.UnitsAsEditable()
            .AddRecipeUnlock(protosDb, ModIDs.Recipes.RubberProductionDieselWithCharcaol)
            .SetToResearch(proto);

        // Naphtha processing
        proto = protosDb.GetOrThrow<ResearchNodeProto>(Ids.Research.NaphthaProcessing);
        proto.UnitsAsEditable()
            .AddRecipeUnlock(protosDb, ModIDs.Recipes.RubberProductionNaphthaCharcoal)
            .SetToResearch(proto);

        // Power generation II
        proto = protosDb.GetOrThrow<ResearchNodeProto>(Ids.Research.PowerGeneration2);
        proto.UnitsAsEditable()
            .AddRecipeUnlock(protosDb, ModIDs.Recipes.SteamGenerationCharcoal, position: 1)
            .SetToResearch(proto);

        // Advanced diesel
        proto = protosDb.GetOrThrow<ResearchNodeProto>(Ids.Research.CrudeOilDistillation);
        proto.UnitsAsEditable()
            .AddRecipeUnlock(protosDb, ModIDs.Recipes.SteamGenerationCharcoal, position: 1)
            .SetToResearch(proto);
    }
}
