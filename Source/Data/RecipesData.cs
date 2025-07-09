using Mafi;
using Mafi.Base;
using Mafi.Core.Factory.Recipes;
using Mafi.Core.Mods;
using Mafi.Core.Prototypes;

namespace CoI.MetallurgyPlus.Data;

internal class RecipesData : IModData
{
    public void RegisterData(ProtoRegistrator registrator)
    {
        // Wood to charcoal.
        registrator.RecipeProtoBuilder
            .Start("Charcoal making", ModIDs.Recipes.CharcoalFromWood, Ids.Machines.CharcoalMaker)
            .AddInput(10, Ids.Products.Wood, RecipeProtoBuilder.ANY_COMPATIBLE_PORT)
            .SetDurationSeconds(30)
            .AddOutput(6, ModIDs.Products.Charcoal, RecipeProtoBuilder.ANY_COMPATIBLE_PORT)
            .AddOutput(3, Ids.Products.Exhaust, RecipeProtoBuilder.ANY_COMPATIBLE_PORT, true)
            .BuildAndAdd();
    }
}
