using CoI.MetallurgyPlus.Extensions;
using Mafi;
using Mafi.Base;
using Mafi.Collections;
using Mafi.Core.Entities;
using Mafi.Core.Entities.Static.Layout;
using Mafi.Core.Factory.Machines;
using Mafi.Core.Factory.Recipes;
using Mafi.Core.Mods;
using Mafi.Core.Prototypes;
using Mafi.Localization;
using System;

namespace CoI.MetallurgyPlus.Data;

internal class MachineData : IModData
{
    public void RegisterData(ProtoRegistrator registrator)
    {
        OverrideCharcoalMaker(registrator);
        OverrideRotaryKiln(registrator);
        OverrideOxygenFurnace(registrator);
        OverrideShredder(registrator);
        OverrideFlare(registrator);
    }

    private void OverrideCharcoalMaker(ProtoRegistrator registrator)
    {
        var name = Loc.Str(Ids.Machines.CharcoalMaker.Value + "_MP__name", "Charcoal maker", "name of a machine");
        var desc = Loc.Str(Ids.Machines.CharcoalMaker.Value + "_MP__desc", "Uses wood to create charcoal.", "short description of a machine");

        var proto = registrator.PrototypesDb.GetOrThrow<Proto>(Ids.Machines.CharcoalMaker);
        proto.SetProperty(nameof(Proto.Strings), new Proto.Str(name, desc));

        // Wood to charcoal.
        registrator.RecipeProtoBuilder
            .Start("Charcoal making", ModIDs.Recipes.CharcoalFromWood, Ids.Machines.CharcoalMaker)
            .AddInput(10, Ids.Products.Wood, RecipeProtoBuilder.ANY_COMPATIBLE_PORT)
            .SetDurationSeconds(30)
            .AddOutput(6, ModIDs.Products.Charcoal, RecipeProtoBuilder.ANY_COMPATIBLE_PORT)
            .AddOutput(6, Ids.Products.Exhaust, RecipeProtoBuilder.ANY_COMPATIBLE_PORT, true)
            .BuildAndAdd();

        // Remove coal to wood recipe.
        registrator.PrototypesDb.GetOrThrow<RecipeProto>(Ids.Recipes.CharcoalBurning).MarkAsObsolete();
    }

    private void OverrideRotaryKiln(ProtoRegistrator registrator)
    {
        // Change port layout of rotary kiln to match gas rotary kiln.
        var layoutString = new string[] {
            "A~>[7][7][7][6][6][2][2][2][2][2][2][2][2][2]   ",
            "   [7][7][7][6][6][2][2][2][2][2][2][2][2][2]>#X",
            "   [2][2][2][2][2][2][2][2][2][2][2][3][2][2]>~Y",
            "B~>[2][2][2][2][2][2][2][2][2][2][2][3][2][2]>@E"
        };

        var kilnProto = registrator.PrototypesDb.GetOrThrow<MachineProto>(Ids.Machines.RotaryKiln);
        kilnProto.UpdateLayout(new EntityLayoutParser(registrator.PrototypesDb).ParseLayoutOrThrow(layoutString));

        // Direct iron reduction.
        registrator.RecipeProtoBuilder
            .Start("Direct iron reduction", ModIDs.Recipes.IronReductionT1, Ids.Machines.RotaryKiln)
            .AddInput(6, Ids.Products.IronOre, "A")
            .AddInput(2, Ids.Products.Coal, "B")
            .SetDurationSeconds(15)
            .AddOutput(6, ModIDs.Products.SpongeIron, RecipeProtoBuilder.ANY_COMPATIBLE_PORT)
            .AddOutput(8, Ids.Products.Exhaust, RecipeProtoBuilder.ANY_COMPATIBLE_PORT)
            .BuildAndAdd();
    }

    private void OverrideOxygenFurnace(ProtoRegistrator registrator)
    {
        // Add more ports to the oxygen furnaces.
        var layoutString = new string[] {
            "                     ^@S   ",
            "   [6][6][6][6][6][6][1]   ",
            "   [6][6][6][6][6][6][2]>~Y",
            "A'>[6][6][6][6][6][6][2]>'X",
            "C~>[6][6][6][6][6][6][2]   ",
            "   [6][6][6][6][6][6][2]   ",
            "                     B@^   "
        };
        var parsed = new EntityLayoutParser(registrator.PrototypesDb).ParseLayoutOrThrow(layoutString);

        var oxyT1 = registrator.PrototypesDb.GetOrThrow<MachineProto>(Ids.Machines.OxygenFurnace);
        oxyT1.UpdateLayout(parsed);
        oxyT1.Graphics.ReplacePrefabWith("Assets/CoI.Metallurgy+/OxygenFurnace.prefab");
        oxyT1.Graphics.SetField("m_instancedRenderingAnimationMaterialSwap", new Dict<string, string>());
        oxyT1.Graphics.SetField("m_instancedRenderingAnimationProtoSwap", null);
        oxyT1.Graphics.SetField("UseSemiInstancedRendering", false);

        var oxyT2 = registrator.PrototypesDb.GetOrThrow<MachineProto>(Ids.Machines.OxygenFurnaceT2);
        oxyT2.UpdateLayout(parsed);
        oxyT2.Graphics.ReplacePrefabWith("Assets/CoI.Metallurgy+/OxygenFurnaceT2.prefab");
        oxyT2.Graphics.SetField("m_instancedRenderingAnimationMaterialSwap", new Dict<string, string>());
        oxyT2.Graphics.SetField("m_instancedRenderingAnimationProtoSwap", null);
        oxyT2.Graphics.SetField("UseSemiInstancedRendering", false);
    }

    private void OverrideShredder(ProtoRegistrator registrator)
    {
        // Add 'iron smelting' category to shredder.
        var proto = registrator.PrototypesDb.GetOrThrow<MachineProto>(Ids.Machines.Shredder);
        var categories = proto.Graphics.Categories.ToLyst();
        categories.Add(new ToolbarEntryData(registrator.PrototypesDb.GetOrThrow<ToolbarCategoryProto>(Ids.ToolbarCategories.Smelting_Iron), true));
        proto.Graphics.SetProperty(nameof(MachineProto.Gfx.Categories), categories.ToImmutableArray());

        EntityCostsTpl newCosts = Costs.Build.CP2(50).Workers(1).MaintenanceT1(1);
        proto.SetProperty(nameof(MachineProto.Costs), newCosts.MapToEntityCosts(registrator));

        // Steel shredding.
        registrator.RecipeProtoBuilder
            .Start("Shredding steel", ModIDs.Recipes.ShreddingSteel, Ids.Machines.Shredder)
            .AddInput(2, Ids.Products.Steel)
            .SetDurationSeconds(10)
            .AddOutput(2, Ids.Products.IronScrap, RecipeProtoBuilder.ANY_COMPATIBLE_PORT)
            .BuildAndAdd();
    }

    private void OverrideFlare(ProtoRegistrator registrator)
    {
        // Update Flare particles for burning coal tar.
        var proto = registrator.PrototypesDb.GetOrThrow<MachineProto>(Ids.Machines.Flare);

        var particles = proto.Graphics.ParticlesParams.ToLyst();
        var clean = particles.Find(p => p.SystemId == "FireClean");
        if (clean is not null) {
            var lambda = clean.SupportedRecipesSelector;
            if (lambda.HasValue) {
                clean.SetField(nameof(ParticlesParams.SupportedRecipesSelector), Option<Func<RecipeProto, bool>>.Create(r => lambda.Value(r) && !IsCoalTarRecipe(r)));
            } else {
                clean.SetField(nameof(ParticlesParams.SupportedRecipesSelector), Option<Func<RecipeProto, bool>>.Create(IsCoalTarRecipe));
            }
        }
        particles.Add(ParticlesParams.Loop("FireSmokeDark", false, IsCoalTarRecipe, null));

        proto.Graphics.SetField(nameof(MachineProto.Gfx.ParticlesParams), particles.ToImmutableArray());

        static bool IsCoalTarRecipe(RecipeProto r) => r.Id == ModIDs.Recipes.FlareCoalTar;
    }
}
