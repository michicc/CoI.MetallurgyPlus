using CoI.MetallurgyPlus.Data;
using Mafi;
using Mafi.Base;
using Mafi.Core;
using Mafi.Core.Mods;

namespace CoI.MetallurgyPlus;

public sealed class MetallurgyPlusMod : DataOnlyMod
{
    public override string Name => "Metallurgy+ Mod";

    public override int Version => 0;

    public MetallurgyPlusMod(CoreMod coreMod, BaseMod baseMod)
    {
        Log.Debug("[Metallurgy+] Instance constructed");
    }

    public override void RegisterPrototypes(ProtoRegistrator registrator)
    {
        Log.Info("[Metallurgy+] Registering prototypes");

        registrator.RegisterAllProducts();
        registrator.RegisterData<OpenHearthFurnaceData>();
        registrator.RegisterData<RecipesData>();
        registrator.RegisterDataWithInterface<IResearchNodesData>();
        registrator.RegisterData<GoalsData>();
    }
}
