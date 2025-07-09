using Mafi.Base;
using ProductID = Mafi.Core.Products.ProductProto.ID;
using RecipeID = Mafi.Core.Factory.Recipes.RecipeProto.ID;

namespace CoI.MetallurgyPlus.Data;

public static class ModIDs
{
    public static class Products
    {
        [LooseProduct(material: "Assets/CoI.Metallurgy+/Materials/Charcoal.mat", icon: "Assets/CoI.Metallurgy+/ProductIcons/Charcoal.png", useRoughPileMeshes: true, resourceColor: 4210752, pinToHomeScreen: true, doNotTrackSourceProducts: true)]
        public static readonly ProductID Charcoal = Ids.Products.CreateId("Charcoal");
    }

    public static class Recipes
    {
        public static readonly RecipeID CharcoalFromWood = Ids.Recipes.CreateId("CharcoalFromWood");
    }
}
