using Mafi;
using Mafi.Base;
using Mafi.Collections;
using Mafi.Core.Entities.Static.Layout;
using Mafi.Core.Factory.Machines;
using Mafi.Core.Factory.Recipes;
using Mafi.Core.Mods;
using Mafi.Core.Prototypes;
using System.Collections.Generic;

namespace CoI.MetallurgyPlus.Data;

internal class CokingOven : IModData
{
    public void RegisterData(ProtoRegistrator registrator)
    {
        RegisterCokeMaker(registrator);
        RegisterCokeMakerT2(registrator);
    }

    private void RegisterCokeMaker(ProtoRegistrator registrator)
    {
        var layoutString = new string[] {
            "   [4][6][6][6][4]>@Y",
            "A~>[4][6][6][6][4]   ",
            "   [4][6][6][6][4]>@X",
            "      [3][2][3]      ",
            "B@>[2][3][2][3][1]>@Z",
            "      [3][2][3][1]   ",
            "      [3][2][3][1]>~W",
            "      [3][2][3][1]   ",
        };
        var portHeights = new Lyst<KeyValuePair<char, int>> { Make.Kvp('B', 2) };

        // Machine: Coking oven.
        var proto = registrator.MachineProtoBuilder
            .Start("Coking oven", ModIDs.Machines.CokingOven)
            .Description("Creates metallurgical coke from coal.")
            .SetCategories([Ids.ToolbarCategories.Smelting_Iron])
            .SetCost(Costs.Build.CP3(50).Workers(4).MaintenanceT1(4))
            .SetElectricityConsumption(50.Kw())
            .SetLayout(new EntityLayoutParams(customPortHeights: portHeights), layoutString)
            .SetPrefabPath("TODO")
            .BuildAndAdd();

        registrator.RecipeProtoBuilder
            .Start("Coke making", ModIDs.Recipes.CokeFromCoal, ModIDs.Machines.CokingOven)
            .AddInput(10, Ids.Products.Coal, RecipeProtoBuilder.ANY_COMPATIBLE_PORT)
            .AddInput(4, Ids.Products.Water, RecipeProtoBuilder.ANY_COMPATIBLE_PORT)
            .SetDurationSeconds(40)
            .AddOutput(8, ModIDs.Products.Coke, RecipeProtoBuilder.ANY_COMPATIBLE_PORT)
            .AddOutput(8, ModIDs.Products.CoalTar, "X")
            .AddOutput(20, Ids.Products.Exhaust, "Y")
            .AddOutput(4, Ids.Products.WasteWater, "Z")
            .BuildAndAdd();
    }

    private void RegisterCokeMakerT2(ProtoRegistrator registrator)
    {
        var layoutString = new string[] {
            "   [4][6][6][6][4]>@Y",
            "A~>[4][6][6][6][4]   ",
            "   [4][6][6][6][4]>@X",
            "      [3][2][3]      ",
            "B@>[2][3][2][3][1]   ",
            "      [3][2][3][1]   ",
            "      [3][2][3][1]>~W",
            "      [3][2][3][1]   ",
        };
        var portHeights = new Lyst<KeyValuePair<char, int>> { Make.Kvp('B', 2) };

        // Machine: Coking oven T2.
        var proto = registrator.MachineProtoBuilder
            .Start("Coking oven II", ModIDs.Machines.CokingOvenT2)
            .Description("Creates metallurgical coke from coal.")
            .SetCategories([Ids.ToolbarCategories.Smelting_Iron])
            .SetCost(Costs.Build.CP4(100).Workers(6).MaintenanceT1(8))
            .SetElectricityConsumption(200.Kw())
            .SetLayout(new EntityLayoutParams(customPortHeights: portHeights), layoutString)
            .SetPrefabPath("TODO")
            .BuildAndAdd();

        registrator.PrototypesDb.GetOrThrow<MachineProto>(ModIDs.Machines.CokingOven).SetNextTier(proto);

        registrator.RecipeProtoBuilder
            .Start("Coke making (N2)", ModIDs.Recipes.CokeFromCoalT2, ModIDs.Machines.CokingOvenT2)
            .AddInput(10, Ids.Products.Coal, RecipeProtoBuilder.ANY_COMPATIBLE_PORT)
            .AddInput(8, Ids.Products.Nitrogen, RecipeProtoBuilder.ANY_COMPATIBLE_PORT)
            .SetDurationSeconds(20)
            .AddOutput(8, ModIDs.Products.Coke, RecipeProtoBuilder.ANY_COMPATIBLE_PORT)
            .AddOutput(6, ModIDs.Products.CoalTar, "X")
            .AddOutput(28, Ids.Products.Exhaust, "Y")
            .BuildAndAdd();
    }
}
