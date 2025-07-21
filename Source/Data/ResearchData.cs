using CoI.MetallurgyPlus.Extensions;
using Mafi;
using Mafi.Base;
using Mafi.Core.Factory.Machines;
using Mafi.Core.Mods;
using Mafi.Core.Prototypes;
using Mafi.Core.Research;

namespace CoI.MetallurgyPlus.Data;

internal class ResearchData : IResearchNodesData
{
    public void RegisterData(ProtoRegistrator registrator)
    {
        AddIronSmeltingOre(registrator);
        AddIronDirectReduction(registrator);
        AddCharcoalRecipes(registrator.PrototypesDb);

        OverrideIronSmeltingScrap(registrator.PrototypesDb);
        OverrideCopperRefinement(registrator.PrototypesDb);
        OverrideVehicleAndMining(registrator.PrototypesDb);
        OverrideConstruction(registrator.PrototypesDb);
        OverrideGasCombustion(registrator.PrototypesDb);

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

    private void OverrideIronSmeltingScrap(ProtosDb protosDb)
    {
        // Iron smelting (from scrap)
        var proto = protosDb.GetOrThrow<ResearchNodeProto>(Ids.Research.IronSmeltingScrap);
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
            .SetToIcons(proto);
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

    private void OverrideGasCombustion(ProtosDb protosDb)
    {
        // Gas combustion
        var proto = protosDb.GetOrThrow<ResearchNodeProto>(Ids.Research.GasCombustion);
        proto.UnitsAsEditable()
            .AddRecipeUnlock(protosDb, ModIDs.Recipes.SteelFromScrapT1FG)
            .AddRecipeUnlock(protosDb, ModIDs.Recipes.SteelFromIronT1FG)
            .AddRecipeUnlock(protosDb, ModIDs.Recipes.SteelFromSpongeT1FG)
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
    }
}
