using CoI.MetallurgyPlus.Extensions;
using Mafi;
using Mafi.Base;
using Mafi.Core.Factory.Machines;
using Mafi.Core.Mods;
using Mafi.Core.PropertiesDb;
using Mafi.Core.Prototypes;
using Mafi.Core.Research;

namespace CoI.MetallurgyPlus.Data;

internal class ResearchData : IResearchNodesData
{
    public void RegisterData(ProtoRegistrator registrator)
    {
        AddIronSmeltingOre(registrator);
        AddCharcoalRecipes(registrator.PrototypesDb);

        OverrideIronSmeltingScrap(registrator.PrototypesDb);
        OverrideVehicleAndMining(registrator.PrototypesDb);
        OverrideConstruction(registrator.PrototypesDb);

        // Adjust positions of existing research items.
        registrator.PrototypesDb.GetOrThrow<ResearchNodeProto>(Ids.Research.Beacon).GridPosition = new Vector2i(12, 5);
        registrator.PrototypesDb.GetOrThrow<ResearchNodeProto>(Ids.Research.RepairDock).GridPosition = new Vector2i(16, 5);
    }

    private void AddIronSmeltingOre(ProtoRegistrator registrator)
    {
        var proto = registrator.ResearchNodeProtoBuilder
            .Start("Iron ore smelting", ModIDs.Research.IronSmeltingOre, 2)
            .Description("Production of iron from iron ore.")
            .AddMachineToUnlock(Ids.Machines.SmeltingFurnaceT1)
            .AddRecipeToUnlock(ModIDs.Recipes.IronSmeltingT1Charcoal)
            .AddRecipeToUnlock(ModIDs.Recipes.SteelFromIronT1)
            .SetGridPosition(new Vector2i(12, 10))
            .AddParents(registrator.PrototypesDb.GetOrThrow<ResearchNodeProto>(Ids.Research.VehicleAndMining))
            .BuildAndAdd();

        registrator.PrototypesDb.GetOrThrow<ResearchNodeProto>(Ids.Research.CopperRefinement).AddParent(proto);
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

    private void OverrideVehicleAndMining(ProtosDb protosDb)
    {
        // Vehicles & mining
        var proto = protosDb.GetOrThrow<ResearchNodeProto>(Ids.Research.VehicleAndMining);
        proto.UnitsAsEditable()
            .RemoveRecipeUnlock(Ids.Recipes.IronSmeltingT1Coal)
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
    }
}
