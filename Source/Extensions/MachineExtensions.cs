using Mafi;
using Mafi.Collections;
using Mafi.Collections.ImmutableCollections;
using Mafi.Core.Entities.Static.Layout;
using Mafi.Core.Factory.Machines;
using Mafi.Core.Factory.Recipes;
using Mafi.Core.Prototypes;
using System.Reflection;

namespace CoI.MetallurgyPlus.Extensions;

internal static class MachineExtensions
{
    public static Lyst<RecipeProto> RecipesAsEditable(this MachineProto proto) => proto.GetField<Lyst<RecipeProto>, MachineProto>("m_recipes");

    public static void RemoveRecipeFromMachine(this ProtosDb protosDb, MachineProto.ID machine, RecipeProto.ID recipe)
    {
        protosDb.GetOrThrow<MachineProto>(machine).RecipesAsEditable().RemoveFirst(x => x.Id == recipe);
    }

    public static void UpdateLayout(this LayoutEntityProto proto, EntityLayout layout)
    {
        proto.SetProperty(nameof(LayoutEntityProto.Layout), layout);

        proto.SetField(nameof(LayoutEntityProto.InputPorts), layout.Ports.Where(pt => pt.Type == IoPortType.Input).ToImmutableArray());
        proto.SetField(nameof(LayoutEntityProto.OutputPorts), layout.Ports.Where(pt => pt.Type == IoPortType.Output).ToImmutableArray());
    }

    public static void SetAssetPathToSelf(this LayoutEntityProto proto)
    {
        string? protoSwap = proto.Graphics.GetField<string, LayoutEntityProto.Gfx>("m_instancedRenderingAnimationProtoSwap");
        string path = $"Assets/{Assembly.GetCallingAssembly().GetName().Name}/Generated/Animations/{protoSwap ?? proto.Id.ToString()}";
        proto.Graphics.SetProperty(nameof(LayoutEntityProto.Gfx.AnimationDataAssetPathBase), path);
    }
}
