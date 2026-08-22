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
        public static readonly ProductID Charcoal = Ids.Products.CreateId("MP_Charcoal");
        public static readonly ProductID Coke = Ids.Products.CreateId("MP_Coke");
        public static readonly ProductID SpongeIron = Ids.Products.CreateId("MP_SpongeIron");

        public static readonly ProductID CoalTar = Ids.Products.CreateId("MP_CoalTar");
        public static readonly ProductID Argon = Ids.Products.CreateId("MP_Argon");
        public static readonly ProductID Syngas = Ids.Products.CreateId("MP_Syngas");
        public static readonly ProductID SyngasUsed = Ids.Products.CreateId("MP_SyngasUsed");
    }

    public static class Machines
    {
        public static readonly MachineID OpenHearthFurnace = Ids.Machines.CreateId("MP_OpenHearthFurnace");
        public static readonly MachineID CokingOven = Ids.Machines.CreateId("MP_CokingOven");
        public static readonly MachineID CokingOvenT2 = Ids.Machines.CreateId("MP_CokingOvenT2");
        public static readonly MachineID AirSeparatorT2 = Ids.Machines.CreateId("MP_AirSeparatorT2");
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

        #region Coke recipes
        public static readonly RecipeID CokeFromCoal = Ids.Recipes.CreateId("MP_CokeFromCoal");
        public static readonly RecipeID CokeFromCoalT2 = Ids.Recipes.CreateId("MP_CokeFromCoalT2");

        public static readonly RecipeID SteamGenerationCoke = Ids.Recipes.CreateId("MP_SteamGenerationCoke");
        public static readonly RecipeID SuperSteamGenerationCoke = Ids.Recipes.CreateId("MP_SuperSteamGenerationCoke");
        public static readonly RecipeID FlareCoalTar = Ids.Recipes.CreateId("MP_FlareCoalTar");
        public static readonly RecipeID CoalTarProcessing = Ids.Recipes.CreateId("MP_CoalTarProcessing");
        public static readonly RecipeID SyntheticMorphine = Ids.Recipes.CreateId("MP_SyntheticMorphine");
        #endregion

        #region Iron smelting recipes
        public static readonly RecipeID IronSmeltingT1Charcoal = Ids.Recipes.CreateId("MP_IronSmeltingT1Charcoal");
        public static readonly RecipeID IronSmeltingT1Coke = Ids.Recipes.CreateId("MP_IronSmeltingT1Coke");
        public static readonly RecipeID IronSmeltingT2 = Ids.Recipes.CreateId("MP_IronSmeltingT2");
        public static readonly RecipeID IronSmeltingArc = Ids.Recipes.CreateId("MP_IronSmeltingArc");
        public static readonly RecipeID SteelFromScrapT1 = Ids.Recipes.CreateId("MP_SteelFromScrapT1");
        public static readonly RecipeID SteelFromScrapT1Coal = Ids.Recipes.CreateId("MP_SteelFromScrapT1Coal");
        public static readonly RecipeID SteelFromScrapT1FG = Ids.Recipes.CreateId("MP_SteelFromScrapT1FG");
        public static readonly RecipeID SteelFromScrapT1Arc = Ids.Recipes.CreateId("MP_SteelFromScrapT1Arc");
        public static readonly RecipeID SteelFromScrapT2Arc = Ids.Recipes.CreateId("MP_SteelFromScrapT2Arc");
        public static readonly RecipeID SteelFromIronT1 = Ids.Recipes.CreateId("MP_SteelFromIronT1");
        public static readonly RecipeID SteelFromIronT1Coal = Ids.Recipes.CreateId("MP_SteelFromIronT1Coal");
        public static readonly RecipeID SteelFromIronT1FG = Ids.Recipes.CreateId("MP_SteelFromIronT1FG");

        public static readonly RecipeID IronReductionT1 = Ids.Recipes.CreateId("MP_IronReductionT1");
        public static readonly RecipeID IronReductionT2 = Ids.Recipes.CreateId("MP_IronReductionT2");
        public static readonly RecipeID SteelFromSpongeT1 = Ids.Recipes.CreateId("MP_SteelFromSpongeT1");
        public static readonly RecipeID SteelFromSpongeT1Coal = Ids.Recipes.CreateId("MP_SteelFromSpongeT1Coal");
        public static readonly RecipeID SteelFromSpongeT1FG = Ids.Recipes.CreateId("MP_SteelFromSpongeT1FG");
        public static readonly RecipeID SteelFromSpongeT1Arc = Ids.Recipes.CreateId("MP_SteelFromSpongeT1Arc");
        public static readonly RecipeID SteelFromSpongeT2Arc = Ids.Recipes.CreateId("MP_SteelFromSpongeT2Arc");

        public static readonly RecipeID SteelSmelting = Ids.Recipes.CreateId("MP_SteelSmelting");
        public static readonly RecipeID SteelSmeltingDIR = Ids.Recipes.CreateId("MP_SteelSmeltingDRI");
        public static readonly RecipeID SteelSmeltingT2 = Ids.Recipes.CreateId("MP_SteelSmeltingT2");
        public static readonly RecipeID SteelSmeltingT2DRI = Ids.Recipes.CreateId("MP_SteelSmeltingT2DRI");

        public static readonly RecipeID SteelCasting = Ids.Recipes.CreateId("MP_SteelCasting");
        public static readonly RecipeID SteelCastingCooled = Ids.Recipes.CreateId("MP_SteelCastingCooled");
        #endregion

        #region Steel recipes
        public static readonly RecipeID MechPartsAssemblySteel = Ids.Recipes.CreateId("MP_MechPartsAssemblySteel");

        public static readonly RecipeID ShreddingSteel = Ids.Recipes.CreateId("MP_ShreddingSteel");
        #endregion

        #region Misc recipes
        public static readonly RecipeID SyngasProduction = Ids.Recipes.CreateId("MP_SyngasProduction");
        public static readonly RecipeID SyngasProductionInitial = Ids.Recipes.CreateId("MP_SyngasProductionInitial");
        public static readonly RecipeID SyngasSmokeStack = Ids.Recipes.CreateId("MP_SyngasSmokeStack");

        public static readonly RecipeID AirSeparationT2 = Ids.Recipes.CreateId("MP_AirSeparationT2");
        public static readonly RecipeID ArgonSmokeStack = Ids.Recipes.CreateId("MP_ArgonSmokeStack");
        #endregion
    }

    public static class Research
    {
        public static readonly ResearchID IronSmeltingOre = Ids.Research.CreateId("MP_IronSmeltingOre");
        public static readonly ResearchID DirectIronReduction = Ids.Research.CreateId("MP_DirectIronReduction");
        public static readonly ResearchID GasPoweredFurnace = Ids.Research.CreateId("MP_GasPoweredFurnace");
        public static readonly ResearchID CokeMaking = Ids.Research.CreateId("MP_CokeMaking");
        public static readonly ResearchID CokeMakingT2 = Ids.Research.CreateId("MP_CokeMakingT2");
        public static readonly ResearchID SteamGenerationCoke = Ids.Research.CreateId("MP_SteamGenerationCoke");
        public static readonly ResearchID CoalTarProcessing = Ids.Research.CreateId("MP_CoalTarProcessing");
        public static readonly ResearchID DirectIronReductionFG = Ids.Research.CreateId("MP_DirectIronReductionFG");
        public static readonly ResearchID HydrogenSteelSmelting = Ids.Research.CreateId("MP_HydrogenSteelSmelting");
        public static readonly ResearchID SuperSteamFromCoke = Ids.Research.CreateId("MP_SuperSteamFromCoke");
        public static readonly ResearchID SyntheticMorphine = Ids.Research.CreateId("MP_SyntheticMorphine");
    }
}
