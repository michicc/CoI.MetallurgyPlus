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

    public static void SetToResearch(this Lyst<IUnlockNodeUnit> units, ResearchNodeProto proto) => proto.SetProperty(nameof(ResearchNodeProto.Units), units.ToImmutableArray());

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
    public class ResearchNodeIcons
    {
        private Lyst<string> _icons;
        private Lyst<IProtoWithIcon> _protos;

        public ResearchNodeIcons(ResearchNodeProto proto)
        {
            _icons = proto.Graphics.Icons.ToLyst();
            _protos = proto.Graphics.IconsProtos.ToLyst();
        }

        public ResearchNodeIcons RemoveIcon(Proto.ID toRemove)
        {
            _protos.RemoveWhere(toRemove, static (x, id) => x.Id == id);
            return this;
        }

        public ResearchNodeIcons AddIcon(IProtoWithIcon entity, int position = -1)
        {
            // Don't add duplicate icons.
            if (_protos.Contains(entity)) return this;

            if (position >= 0) {
                _protos.Insert(position, entity);
            } else {
                _protos.Add(entity);
            }

            return this;
        }

        public ResearchNodeIcons AddIcon(string icon, int position = -1)
        {
            // Don't add duplicate icons.
            if (_icons.Contains(icon)) return this;

            if (position >= 0) {
                _icons.Insert(position, icon);
            } else {
                _icons.Add(icon);
            }

            return this;
        }

        public void ApplyTo(ResearchNodeProto proto) => proto.SetField("Graphics", new ResearchNodeProto.Gfx(_icons.ToImmutableArray(), _protos.ToImmutableArray()));
    }

    public static ResearchNodeIcons IconsAsEditable(this ResearchNodeProto proto) => new(proto);
    #endregion
}
