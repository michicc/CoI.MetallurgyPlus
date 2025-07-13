using CoI.MetallurgyPlus.Extensions;
using Mafi;
using Mafi.Base;
using Mafi.Collections.ImmutableCollections;
using Mafi.Core.Entities.Static;
using Mafi.Core.Messages.Goals;
using Mafi.Core.Mods;
using Mafi.Core.Products;
using Mafi.Core.Prototypes;
using System.Collections.Generic;

namespace CoI.MetallurgyPlus.Data;

internal class GoalsData : IModData
{
    public void RegisterData(ProtoRegistrator registrator)
    {
        OverrideIronProductionGoal(registrator.PrototypesDb);
    }

    private void OverrideIronProductionGoal(ProtosDb protosDb)
    {
        var goalProduceCoal = protosDb.GetOrThrow<GoalToReachProductStatsValue.Proto>(MakeGoalID("ProduceCoal"));
        goalProduceCoal.SetField("ProtoToTrack", protosDb.GetOrThrow<ProductProto>(ModIDs.Products.Charcoal));

        var goalBuildFurnace = protosDb.GetOrThrow<GoalToConstructStaticEntity.Proto>(MakeGoalID("BuildFurnace"));
        KeyValuePair<StaticEntityProto, int>[] toBuild = [
            Make.Kvp(protosDb.GetOrThrow<StaticEntityProto>(ModIDs.Machines.OpenHearthFurnace), 1),
            Make.Kvp(protosDb.GetOrThrow<StaticEntityProto>(Ids.Machines.Caster), 1),
            Make.Kvp(protosDb.GetOrThrow<StaticEntityProto>(Ids.Machines.SmokeStack), 1)
        ];
        goalBuildFurnace.SetField("ProtosToBuild", toBuild.ToImmutableArray());

        var goalProduceIron = protosDb.GetOrThrow<GoalToReachProductStatsValue.Proto>(MakeGoalID("ProduceIron"));
        goalProduceIron.SetField("ProtoToTrack", protosDb.GetOrThrow<ProductProto>(Ids.Products.Steel));
        goalProduceIron.SetField("MinQuantityRequired", 12.Quantity());
    }

    private static Proto.ID MakeGoalID(string goalName) => new("Goal_" + goalName);
}
