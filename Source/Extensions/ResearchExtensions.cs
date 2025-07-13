using Mafi;
using Mafi.Collections;
using Mafi.Collections.ImmutableCollections;
using Mafi.Core.Entities.Static.Layout;
using Mafi.Core.Factory.Machines;
using Mafi.Core.Factory.Recipes;
using Mafi.Core.Prototypes;
using Mafi.Core.Research;
using Mafi.Core.UnlockingTree;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CoI.MetallurgyPlus.Extensions;

internal static class ResearchExtensions
{
    #region Unlock units
    public static Set<IUnlockNodeUnit> UnitsAsEditable(this ResearchNodeProto proto) => new(proto.Units);

    public static void SetToResearch(this Set<IUnlockNodeUnit> units, ResearchNodeProto proto) => proto.SetField("Units", units.ToImmutableArray());

    public static Set<IUnlockNodeUnit> AddRecipeUnlock(this Set<IUnlockNodeUnit> units, ProtosDb protosDb, RecipeProto.ID protoId, bool hideInUi = false, bool ensureMachineIsUnlocked = false)
    {
        var proto = protosDb.GetOrThrow<RecipeProto>(protoId);
        var machine = protosDb.All<MachineProto>().FirstOrDefault(x => x.Recipes.Contains(proto)) ?? throw new InvalidOperationException(string.Format("No machine that can execute {0} the given recipe was found", proto.Id));

        units.Add(new RecipeUnlock(proto, machine, hideInUi, ensureMachineIsUnlocked));

        return units;
    }

    public static Set<IUnlockNodeUnit> RemoveRecipeUnlock(this Set<IUnlockNodeUnit> units, RecipeProto.ID protoId)
    {
        var recipeUnlocks = units.OfType<RecipeUnlock>().Where(x => x.Proto.Id == protoId).ToLyst();

        units.RemoveRange(recipeUnlocks);

        return units;
    }

    public static Set<IUnlockNodeUnit> AddMachineUnlock(this Set<IUnlockNodeUnit> units, ProtosDb protosDb, MachineProto.ID protoId, bool unlockAllRecipes = false, bool hideAllRecipeInUi = false)
    {
        var proto = protosDb.GetOrThrow<MachineProto>(protoId);
        if (unlockAllRecipes) {
            foreach (var recipe in proto.Recipes) {
                if (!recipe.IsObsolete) units.Add(new RecipeUnlock(recipe, proto, hideAllRecipeInUi, false));
            }
        }

        units.Add(new ProtoWithIconUnlock(proto, false));

        return units;
    }

    public static Set<IUnlockNodeUnit> RemoveMachineUnlock(this Set<IUnlockNodeUnit> units, MachineProto.ID protoId)
    {
        var machineUnlocks = units.OfType<ProtoWithIconUnlock>().Where(x => x.Proto.Id == protoId).ToLyst();

        units.RemoveRange(machineUnlocks);

        return units;
    }
    #endregion

    #region Research icons
    public static Lyst<KeyValuePair<Option<Proto>, string>> IconsAsEditable(this ResearchNodeProto proto) => proto.Graphics.Icons.ToLyst();

    public static void SetToIcons(this Lyst<KeyValuePair<Option<Proto>, string>> icons, ResearchNodeProto proto) => proto.SetField("Graphics", new ResearchNodeProto.Gfx(icons.ToImmutableArray()));

    public static Lyst<KeyValuePair<Option<Proto>, string>> RemoveIcon(this Lyst<KeyValuePair<Option<Proto>, string>> icons, Proto.ID toRemove)
    {
        icons.RemoveWhere(toRemove, static (x, id) => x.Key.HasValue && x.Key.Value.Id == id);
        return icons;
    }

    public static Lyst<KeyValuePair<Option<Proto>, string>> AddProtoIcon(this Lyst<KeyValuePair<Option<Proto>, string>> icons, LayoutEntityProto entity, int position = -1)
    {
        // Don't add duplicate icons.
        if (icons.Contains(x => x.Value == entity.IconPath)) return icons;

        var icon = new KeyValuePair<Option<Proto>, string>(entity.SomeOption().As<Proto>(), entity.IconPath);
        if (position >= 0) {
            icons.Insert(position, icon);
        } else {
            icons.Add(icon);
        }

        return icons;
    }
    #endregion
}
