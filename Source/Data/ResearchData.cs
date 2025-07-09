using CoI.MetallurgyPlus.Extensions;
using Mafi.Base;
using Mafi.Core.Mods;
using Mafi.Core.Prototypes;
using Mafi.Core.Research;

namespace CoI.MetallurgyPlus.Data;

internal class ResearchData : IResearchNodesData
{
    public void RegisterData(ProtoRegistrator registrator)
    {
        ApplyOverrides(registrator.PrototypesDb);
    }

    private void ApplyOverrides(ProtosDb protosDb)
    {
        // Iron smelting (from scrap)
        var scrapSmelting = protosDb.GetOrThrow<ResearchNodeProto>(Ids.Research.IronSmeltingScrap);
        scrapSmelting.UnitsAsEditable()
            .AddRecipeUnlock(protosDb, ModIDs.Recipes.CharcoalFromWood)
            .RemoveRecipeUnlock(Ids.Recipes.CharcoalBurning)
            .SetToResearch(scrapSmelting);
    }
}
