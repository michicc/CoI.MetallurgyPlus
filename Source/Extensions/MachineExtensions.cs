using Mafi.Collections;
using Mafi.Core.Factory.Machines;
using Mafi.Core.Factory.Recipes;
using Mafi.Core.Prototypes;

namespace CoI.MetallurgyPlus.Extensions;

internal static class MachineExtensions
{
    public static Lyst<RecipeProto> RecipesAsEditable(this MachineProto proto) => proto.GetField<Lyst<RecipeProto>, MachineProto>("m_recipes");

    public static void RemoveRecipeFromMachine(this ProtosDb protosDb, MachineProto.ID machine, RecipeProto.ID recipe)
    {
        protosDb.GetOrThrow<MachineProto>(machine).RecipesAsEditable().RemoveFirst(x => x.Id == recipe);
    }
}
