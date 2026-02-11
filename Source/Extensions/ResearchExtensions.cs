using Mafi;
using Mafi.Collections;
using Mafi.Core.Entities.Static.Layout;
using Mafi.Core.Factory.Machines;
using Mafi.Core.Factory.Recipes;
using Mafi.Core.Products;
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
    public static Lyst<IUnlockNodeUnit> UnitsAsEditable(this ResearchNodeProto proto) => [.. proto.Units.AsEnumerable()];

    public static void SetToResearch(this Lyst<IUnlockNodeUnit> units, ResearchNodeProto proto) => proto.SetField("Units", units.ToImmutableArray());

    public static Lyst<IUnlockNodeUnit> AddRecipeUnlock(this Lyst<IUnlockNodeUnit> units, ProtosDb protosDb, RecipeProto.ID protoId, bool hideInUi = false, bool ensureMachineIsUnlocked = false, int position =  -1)
    {
        var proto = protosDb.GetOrThrow<RecipeProto>(protoId);
        var machine = protosDb.All<MachineProto>().FirstOrDefault(x => x.Recipes.Contains(proto)) ?? throw new InvalidOperationException(string.Format("No machine that can execute {0} the given recipe was found", proto.Id));

        var unlock = new RecipeUnlock(proto, machine, hideInUi, ensureMachineIsUnlocked);
        if (position >= 0) {
            units.Insert(position, unlock);
        } else {
            units.Add(unlock);
        }

        return units;
    }

    public static Lyst<IUnlockNodeUnit> RemoveRecipeUnlock(this Lyst<IUnlockNodeUnit> units, RecipeProto.ID protoId)
    {
        units.RemoveAll(x => x is RecipeUnlock u && u.Proto.Id == protoId);

        return units;
    }

    public static Lyst<IUnlockNodeUnit> AddMachineUnlock(this Lyst<IUnlockNodeUnit> units, ProtosDb protosDb, MachineProto.ID protoId, bool unlockAllRecipes = false, bool hideAllRecipeInUi = false)
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

    public static Lyst<IUnlockNodeUnit> RemoveMachineUnlock(this Lyst<IUnlockNodeUnit> units, MachineProto.ID protoId)
    {
        units.RemoveAll(x => x is ProtoWithIconUnlock u && u.Proto.Id == protoId);

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

    public static Lyst<KeyValuePair<Option<Proto>, string>> AddProtoIcon(this Lyst<KeyValuePair<Option<Proto>, string>> icons, LayoutEntityProto entity, int position = -1) => AddIcon(icons, entity, entity.IconPath, position);
    public static Lyst<KeyValuePair<Option<Proto>, string>> AddProductIcon(this Lyst<KeyValuePair<Option<Proto>, string>> icons, ProductProto entity, int position = -1) => AddIcon(icons, entity, entity.IconPath, position);

    private static Lyst<KeyValuePair<Option<Proto>, string>> AddIcon(Lyst<KeyValuePair<Option<Proto>, string>> icons, Proto proto, string iconPath, int position = -1)
    {
        // Don't add duplicate icons.
        if (icons.Contains(x => x.Value == iconPath)) return icons;

        var icon = new KeyValuePair<Option<Proto>, string>(proto.SomeOption(), iconPath);
        if (position >= 0) {
            icons.Insert(position, icon);
        } else {
            icons.Add(icon);
        }

        return icons;
    }
    #endregion
}
