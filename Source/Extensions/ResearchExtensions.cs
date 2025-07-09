using Mafi;
using Mafi.Collections;
using Mafi.Collections.ImmutableCollections;
using Mafi.Core.Factory.Machines;
using Mafi.Core.Factory.Recipes;
using Mafi.Core.Prototypes;
using Mafi.Core.Research;
using Mafi.Core.UnlockingTree;
using System;
using System.Linq;

namespace CoI.MetallurgyPlus.Extensions;

internal static class ResearchExtensions
{
    public static Set<IUnlockNodeUnit> UnitsAsEditable(this ResearchNodeProto proto) => new(proto.Units);

    public static void SetToResearch(this Set<IUnlockNodeUnit> units, ResearchNodeProto proto) => proto.SetField("Units", units.ToImmutableArray());

    public static Set<IUnlockNodeUnit> AddRecipeUnlock(this Set<IUnlockNodeUnit> units, ProtosDb protosDb, RecipeProto.ID protoId, bool hideInUi = false, bool ensureMachineIsUnlocked = false)
    {
        var proto = protosDb.GetOrThrow<RecipeProto>(protoId);
        var machine = protosDb.All<MachineProto>().FirstOrDefault(x => x.Recipes.Contains(proto)) ?? throw new InvalidOperationException(string.Format("No machine that can execute {0} the given recipe was found", proto.Id));

        units.Add(new RecipeUnlock(proto, machine, hideInUi, ensureMachineIsUnlocked));

        return units;
    }

    public static Set<IUnlockNodeUnit> RemoveRecipeUnlock(this Set<IUnlockNodeUnit> units, RecipeProto.ID protoId)
    {
        var recipeUnlocks = units.OfType<RecipeUnlock>().Where(x => x.Proto.Id == protoId).ToLyst();

        units.RemoveRange(recipeUnlocks);

        return units;
    }
}
