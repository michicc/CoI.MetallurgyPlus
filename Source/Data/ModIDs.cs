using Mafi.Base;
using MachineID = Mafi.Core.Factory.Machines.MachineProto.ID;
using ProductID = Mafi.Core.Products.ProductProto.ID;
using RecipeID = Mafi.Core.Factory.Recipes.RecipeProto.ID;
using ResearchID = Mafi.Core.Research.ResearchNodeProto.ID;

namespace CoI.MetallurgyPlus.Data;

public static class ModIDs
{
    public static class Products
    {
        [LooseProduct(material: "Assets/CoI.Metallurgy+/Materials/Charcoal.mat", icon: "Assets/CoI.Metallurgy+/ProductIcons/Charcoal.png", useRoughPileMeshes: true, resourceColor: 4210752, pinToHomeScreen: true, doNotTrackSourceProducts: true)]
        public static readonly ProductID Charcoal = Ids.Products.CreateId("MP_Charcoal");

        [LooseProduct(material: "Assets/CoI.Metallurgy+/Materials/SpongeIron.mat", icon: "Assets/CoI.Metallurgy+/ProductIcons/SpongeIron.png", useRoughPileMeshes: false)]
        public static readonly ProductID SpongeIron = Ids.Products.CreateId("MP_SpongeIron");
    }

    public static class Machines
    {
        public static readonly MachineID OpenHearthFurnace = Ids.Machines.CreateId("MP_OpenHearthFurnace");
    }

    public static class Recipes
    {
        #region Charcoal recipes
        public static readonly RecipeID CharcoalFromWood = Ids.Recipes.CreateId("MP_CharcoalFromWood");
        public static readonly RecipeID SteamGenerationCharcoal = Ids.Recipes.CreateId("MP_SteamGenerationCharcoal");
        public static readonly RecipeID SimpleConcreteCharcoal = Ids.Recipes.CreateId("MP_SimpleConcreteCharcoal");
        public static readonly RecipeID DieselDistillationCharcoal = Ids.Recipes.CreateId("MP_DieselDistillationCharcoal");
        public static readonly RecipeID WaterDesalinationCharcoal = Ids.Recipes.CreateId("MP_WaterDesalinationCharcoal");
        public static readonly RecipeID RubberProductionDieselWithCharcaol = Ids.Recipes.CreateId("MP_RubberProductionDieselWithCharcaol");
        public static readonly RecipeID RubberProductionNaphthaCharcoal = Ids.Recipes.CreateId("MP_RubberProductionNaphthaCharcoal");

        public static readonly RecipeID CopperSmeltingT1ScrapCharcoal = Ids.Recipes.CreateId("MP_CopperSmeltingT1ScrapCharcoal");
        public static readonly RecipeID CopperSmeltingT1Charcoal = Ids.Recipes.CreateId("MP_CopperSmeltingT1Charcoal");
        #endregion

        #region Iron smelting recipes
        public static readonly RecipeID IronSmeltingT1Charcoal = Ids.Recipes.CreateId("MP_IronSmeltingT1Charcoal");
        public static readonly RecipeID SteelFromScrapT1 = Ids.Recipes.CreateId("MP_SteelFromScrapT1");
        public static readonly RecipeID SteelFromScrapT1Coal = Ids.Recipes.CreateId("MP_SteelFromScrapT1Coal");
        public static readonly RecipeID SteelFromScrapT1FG = Ids.Recipes.CreateId("MP_SteelFromScrapT1FG");
        public static readonly RecipeID SteelFromIronT1 = Ids.Recipes.CreateId("MP_SteelFromIronT1");
        public static readonly RecipeID SteelFromIronT1Coal = Ids.Recipes.CreateId("MP_SteelFromIronT1Coal");
        public static readonly RecipeID SteelFromIronT1FG = Ids.Recipes.CreateId("MP_SteelFromIronT1FG");
        public static readonly RecipeID SteelCasting = Ids.Recipes.CreateId("MP_SteelCasting");
        public static readonly RecipeID SteelCastingT2 = Ids.Recipes.CreateId("MP_SteelCastingT2");

        public static readonly RecipeID IronReductionT1 = Ids.Recipes.CreateId("MP_IronReductionT1");
        public static readonly RecipeID SteelFromSpongeT1 = Ids.Recipes.CreateId("MP_SteelFromSpongeT1");
        public static readonly RecipeID SteelFromSpongeT1Coal = Ids.Recipes.CreateId("MP_SteelFromSpongeT1Coal");
        public static readonly RecipeID SteelFromSpongeT1FG = Ids.Recipes.CreateId("MP_SteelFromSpongeT1FG");
        #endregion

        #region Steel recipes
        public static readonly RecipeID CpAssemblySteelT1 = Ids.Recipes.CreateId("MP_CpAssemblySteelT1");
        public static readonly RecipeID CpAssemblySteelT2 = Ids.Recipes.CreateId("MP_CpAssemblySteelT2");

        public static readonly RecipeID MechPartsAssemblyT1Steel = Ids.Recipes.CreateId("MP_MechPartsAssemblyT1Steel");
        public static readonly RecipeID MechPartsAssemblyT2Steel = Ids.Recipes.CreateId("MP_MechPartsAssemblyT2Steel");
        #endregion
    }

    public static class Research
    {
        public static readonly ResearchID IronSmeltingOre = Ids.Research.CreateId("MP_IronSmeltingOre");
        public static readonly ResearchID DirectIronReduction = Ids.Research.CreateId("MP_DirectIronReduction");
    }
}
