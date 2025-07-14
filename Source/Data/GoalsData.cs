using CoI.MetallurgyPlus.Extensions;
using Mafi;
using Mafi.Base;
using Mafi.Collections.ImmutableCollections;
using Mafi.Core.Entities.Static;
using Mafi.Core.Messages.Goals;
using Mafi.Core.Mods;
using Mafi.Core.Products;
using Mafi.Core.Prototypes;
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

    private static Proto.ID MakeGoalID(string goalName) => new("Goal_" + goalName);
}
