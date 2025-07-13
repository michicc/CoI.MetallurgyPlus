using Mafi.Base;
using MachineID = Mafi.Core.Factory.Machines.MachineProto.ID;
using ProductID = Mafi.Core.Products.ProductProto.ID;
using RecipeID = Mafi.Core.Factory.Recipes.RecipeProto.ID;

namespace CoI.MetallurgyPlus.Data;

public static class ModIDs
{
    public static class Products
    {
        [LooseProduct(material: "Assets/CoI.Metallurgy+/Materials/Charcoal.mat", icon: "Assets/CoI.Metallurgy+/ProductIcons/Charcoal.png", useRoughPileMeshes: true, resourceColor: 4210752, pinToHomeScreen: true, doNotTrackSourceProducts: true)]
        public static readonly ProductID Charcoal = Ids.Products.CreateId("MP_Charcoal");
    }

    public static class Machines
    {
        public static readonly MachineID OpenHearthFurnace = Ids.Machines.CreateId("MP_OpenHearthFurnace");
    }

    public static class Recipes
    {
        public static readonly RecipeID CharcoalFromWood = Ids.Recipes.CreateId("MP_CharcoalFromWood");
        public static readonly RecipeID SteelFromScrapT1 = Ids.Recipes.CreateId("MP_SteelFromScrapT1");
    }
}
