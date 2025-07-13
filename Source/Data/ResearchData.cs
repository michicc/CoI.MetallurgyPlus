using CoI.MetallurgyPlus.Extensions;
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
        OverrideIronSmeltingScrap(registrator.PrototypesDb);
        OverrideVehicleAndMining(registrator.PrototypesDb);
        OverrideConstruction(registrator.PrototypesDb);
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
            .AddMachineUnlock(protosDb, Ids.Machines.SmeltingFurnaceT1)
            .SetToResearch(proto);

        proto.IconsAsEditable()
            .AddProtoIcon(protosDb.GetOrThrow<MachineProto>(Ids.Machines.SmeltingFurnaceT1))
            .SetToIcons(proto);
    }

    private void OverrideConstruction(ProtosDb protosDb)
    {
        // Construction
        var proto = protosDb.GetOrThrow<ResearchNodeProto>(Ids.Research.CpPacking);
        proto.UnitsAsEditable()
            .AddRecipeUnlock(protosDb, ModIDs.Recipes.CpAssemblySteelT1)
            .RemoveRecipeUnlock(Ids.Recipes.CpAssemblyT1)
            .SetToResearch(proto);

        // Construction II
        proto = protosDb.GetOrThrow<ResearchNodeProto>(Ids.Research.Cp2Packing);
        proto.UnitsAsEditable()
            .AddRecipeUnlock(protosDb, ModIDs.Recipes.CpAssemblySteelT2)
            .RemoveRecipeUnlock(Ids.Recipes.CpAssemblyT2)
            .SetToResearch(proto);

        // Construction III
        proto = protosDb.GetOrThrow<ResearchNodeProto>(Ids.Research.Cp3Packing);
        proto.UnitsAsEditable()
            .RemoveRecipeUnlock(Ids.Recipes.CpAssemblyT3)
            .SetToResearch(proto);

        // Robotic assembly
        proto = protosDb.GetOrThrow<ResearchNodeProto>(Ids.Research.RoboticAssembly);
        proto.UnitsAsEditable()
            .RemoveRecipeUnlock(Ids.Recipes.CpAssemblyT4)
            .SetToResearch(proto);

        // Robotic assembly II
        proto = protosDb.GetOrThrow<ResearchNodeProto>(Ids.Research.Assembler3);
        proto.UnitsAsEditable()
            .RemoveRecipeUnlock(Ids.Recipes.CpAssemblyT5)
            .SetToResearch(proto);
    }
}
