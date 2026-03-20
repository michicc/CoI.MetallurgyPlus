using CoI.MetallurgyPlus.Extensions;
using Mafi;
using Mafi.Base;
using Mafi.Core.Entities;
using Mafi.Core.Entities.Animations;
using Mafi.Core.Entities.Static.Layout;
using Mafi.Core.Factory.Machines;
using Mafi.Core.Mods;
using Mafi.Core.Prototypes;

namespace CoI.MetallurgyPlus.Data;

internal class AirSeparatorData : IModData
{
    public void RegisterData(ProtoRegistrator registrator)
    {
        CustomLayoutToken[] customTokens = [
            new CustomLayoutToken("[0!", (EntityLayoutParams p, int h) => {
                LayoutTileConstraint layoutTileConstraint = LayoutTileConstraint.None;
                Proto.ID? id = new Proto.ID?(p.HardenedFloorSurfaceId);
                return new LayoutTokenSpec(0, h + 2, layoutTileConstraint, 0, surfaceId: id);
            })];

        // Machine: Advanced air separator.
        var proto = registrator.MachineProtoBuilder
            .Start("Air separator II", ModIDs.Machines.AirSeparatorT2)
            .Description("Improved air separator that can distill argon, too.")
            .SetCategories([Ids.ToolbarCategories.Smelting_Iron])
            .SetCost(Costs.Build.CP2(100).Workers(6).MaintenanceT1(6))
            .SetElectricityConsumption(800.Kw())
            .SetLayout(new EntityLayoutParams(customTokens: customTokens), [
                "[4][4][4][4][4][4][4][5][1]   ",
                "[4][4][4][4][9][9][9][6][6]>@X",
                "[4][4][4][9][9![9![9![9![3]>@Z",
                "[4][4][4][9][9![9![9![5][3]   ",
                "[4][4][4][4][9][9][9][6][6]>@Y",
                "[4][4][4][4][4][4][4][5][1]   "
                ])
            .SetPrefabPath("Assets/CoI.Metallurgy+/AirSeparatorT2.prefab")
            .SetAnimationParams(AnimationParams.Loop(50.Percent()))
            .AddParticleParams(ParticlesParams.Loop("Steam"))
            .EnableSemiInstancedRendering()
            .SetCustomIconPath("Assets/CoI.Metallurgy+/Icons/Machine_MP_AirSeparatorT2.png")
            .BuildAndAdd();

        // Set proto as next tier for Air separator.
        registrator.PrototypesDb.GetOrThrow<MachineProto>(Ids.Machines.AirSeparator).SetNextTier(proto);

        // Recipe: Advanced air separation.
        registrator.RecipeProtoBuilder
            .Start("Air separation (T2)", ModIDs.Recipes.AirSeparationT2, proto)
            .SetDurationSeconds(20)
            .AddOutput(12, Ids.Products.Oxygen, "Y")
            .AddOutput(12, Ids.Products.Nitrogen, "X")
            .AddOutput(2, ModIDs.Products.Argon, "Z")
            .BuildAndAdd();
    }

    public static void OnInitialize(DependencyResolver resolver, bool gameWasLoaded)
    {
        var protosDb = resolver.GetResolvedDependency<ProtosDb>();
        var proto = protosDb.ValueOrThrow("Missing ProtosDb").GetOrThrow<MachineProto>(ModIDs.Machines.AirSeparatorT2);

        // Update asset path for instanced rendering.
        proto.SetAssetPathToSelf();
    }
}
