using CoI.MetallurgyPlus.Extensions;
using Mafi;
using Mafi.Base;
using Mafi.Collections.ImmutableCollections;
using Mafi.Core.Entities.Static;
using Mafi.Core.Messages.Goals;
using Mafi.Core.Mods;
using Mafi.Core.Products;
using Mafi.Core.Prototypes;
using Mafi.Core.Research;
using Mafi.Localization;
using System.Collections.Generic;

namespace CoI.MetallurgyPlus.Data;

internal class GoalsData : IModData
{
    public void RegisterData(ProtoRegistrator registrator)
    {
        OverrideIronProductionGoal(registrator.PrototypesDb);
        OverrideCpIProduction(registrator.PrototypesDb);
        OverrideSetupTradings(registrator.PrototypesDb);
        OverrideMaintenance(registrator.PrototypesDb);
        OverrideProcessIronOre(registrator.PrototypesDb);
    }

    private void OverrideIronProductionGoal(ProtosDb protosDb)
    {
        var goalProduceCoal = protosDb.GetOrThrow<GoalToReachProductStatsValue.Proto>(MakeGoalID("ProduceCoal"));
        goalProduceCoal.SetField(nameof(GoalToReachProductStatsValue.Proto.ProtoToTrack), protosDb.GetOrThrow<ProductProto>(ModIDs.Products.Charcoal));

        var goalBuildFurnace = protosDb.GetOrThrow<GoalToConstructStaticEntity.Proto>(MakeGoalID("BuildFurnace"));
        KeyValuePair<StaticEntityProto, int>[] toBuild = [
            Make.Kvp(protosDb.GetOrThrow<StaticEntityProto>(ModIDs.Machines.OpenHearthFurnace), 1),
            Make.Kvp(protosDb.GetOrThrow<StaticEntityProto>(Ids.Machines.Caster), 2),
            Make.Kvp(protosDb.GetOrThrow<StaticEntityProto>(Ids.Machines.SmokeStack), 1)
        ];
        goalBuildFurnace.SetField(nameof(GoalToConstructStaticEntity.Proto.ProtosToBuild), toBuild.ToImmutableArray());

        var goalProduceIron = protosDb.GetOrThrow<GoalToReachProductStatsValue.Proto>(MakeGoalID("ProduceIron"));
        goalProduceIron.SetField(nameof(GoalToReachProductStatsValue.Proto.ProtoToTrack), protosDb.GetOrThrow<ProductProto>(Ids.Products.Steel));
        goalProduceIron.SetField(nameof(GoalToReachProductStatsValue.Proto.MinQuantityRequired), 12.Quantity());
    }

    private void OverrideCpIProduction(ProtosDb protosDb)
    {
        var goalSelectCpRecipe = protosDb.GetOrThrow<GoalToActivateRecipe.Proto>(MakeGoalID("SelectCpRecipe"));
        goalSelectCpRecipe.MachineRecipeToActivate = ImmutableArray.Create(Make.Kvp(Ids.Machines.AssemblyManual, ModIDs.Recipes.CpAssemblySteelT1));
    }

    private void OverrideSetupTradings(ProtosDb protosDb)
    {
        var protoConcrete = protosDb.GetOrThrow<ProductProto>(Ids.Products.ConcreteSlab);
        var protoSteel = protosDb.GetOrThrow<ProductProto>(Ids.Products.Steel);

        LocStrFormatted tradeConcreteTitle = LocStrHelper.GetExistingLocalizedString2Arg("Goal__Trade", "Purchase {0} for {1} from the village on the world map", "goal text, {0} - bricks, {1} - iron").Format($"<bc>{protoConcrete.Strings.Name}</bc>", $"<bc>{protoSteel.Strings.Name}</bc>");

        var goalTradeForBricks = protosDb.GetOrThrow<GoalToReachProductStatsValue.Proto>(MakeGoalID("TradeForBricks"));
        goalTradeForBricks.SetField("m_formatFunc", (string _) => tradeConcreteTitle);
    }

    private void OverrideMaintenance(ProtosDb protosDb)
    {
        var goalMaintenanceAssembly = protosDb.GetOrThrow<GoalToActivateRecipe.Proto>(MakeGoalID("MaintenanceAssembly"));
        goalMaintenanceAssembly.MachineRecipeToActivate = ImmutableArray.Create(Make.Kvp(Ids.Machines.AssemblyManual, ModIDs.Recipes.MechPartsAssemblyT1Steel), Make.Kvp(Ids.Machines.AssemblyElectrified, ModIDs.Recipes.MechPartsAssemblyT2Steel));
    }

    private void OverrideProcessIronOre(ProtosDb protosDb)
    {
        var buildBlastStr = LocStrHelper.GetExistingLocalizedString3Arg("Goal__BuildAndConnect3", "Build {0} and connect it to {1} and {2}", "text for a goal, {0}, {1}, {2} - machine / building names");
        LocStrFormatted ironSteelRecipeStr = Loc.Str2("Goal__MP_IronOpenHearth", "Enable <bc>steel recipe</bc> that uses <bc>{0}</bc> in <bc>{1}</bc>", "goal text").Format(protosDb.GetOrThrow<Proto>(Ids.Products.MoltenIron).Strings.Name, protosDb.GetOrThrow<Proto>(ModIDs.Machines.OpenHearthFurnace).Strings.Name);

        GoalProto[] goals = [
            new GoalToResearchNode.Proto("MP_ResearchIronSmeltingOre", protosDb.GetOrThrow<ResearchNodeProto>(ModIDs.Research.IronSmeltingOre)),
            new GoalToConstructStaticEntity.Proto("MP_BuildBlastFurnace", Make.Kvp(protosDb.GetOrThrow<StaticEntityProto>(Ids.Machines.SmeltingFurnaceT1), 1), Make.Kvp(protosDb.GetOrThrow<StaticEntityProto>(Ids.Machines.SmokeStack), 1), Make.Kvp(protosDb.GetOrThrow<StaticEntityProto>(ModIDs.Machines.OpenHearthFurnace), 1), buildBlastStr, lockedByIndex: 0),
            new GoalToActivateRecipe.Proto("MP_ActiveIronOpenHearthRecipe", ironSteelRecipeStr, ModIDs.Machines.OpenHearthFurnace, ModIDs.Recipes.SteelFromIronT1, tutorial:Ids.Messages.TutorialOnIronOreSmelting, lockedByIndex: 1),
            protosDb.GetOrThrow<GoalProto>(MakeGoalID("ProcessIronOre")),
            protosDb.GetOrThrow<GoalProto>(MakeGoalID("SetupSlagDumpDesignations")),
            protosDb.GetOrThrow<GoalProto>(MakeGoalID("DumpSlag"))
        ];

        // Replace goal proto.
        var goalList = protosDb.GetOrThrow<GoalListProto>(new("Goal__ProcessIronOre"));
        goalList.SetField(nameof(GoalListProto.Goals), CreateGoals(protosDb, goals));
    }

    private static Proto.ID MakeGoalID(string goalName) => new("Goal_" + goalName);

    internal static ImmutableArray<GoalProto> CreateGoals(ProtosDb db, params GoalProto[] protos)
    {
        foreach (GoalProto goalProto in protos) {
            if (db.Get<Proto>(goalProto.Id).IsNone) {
                db.Add<Proto>(goalProto, false);
            }
        }
        return ImmutableArray.Create(protos);
    }
}
