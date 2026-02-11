using CoI.MetallurgyPlus.Data;
using Mafi;
using Mafi.Base;
using Mafi.Core;
using Mafi.Core.Game;
using Mafi.Core.Mods;
using Mafi.Core.Prototypes;

namespace CoI.MetallurgyPlus;

public sealed class MetallurgyPlusMod : IMod
{
    public string Name => "Metallurgy+ Mod";

    public int Version => 0;

    public bool IsUiOnly => false;

    public Option<IConfig> ModConfig => default;

    public MetallurgyPlusMod(CoreMod coreMod, BaseMod baseMod)
    {
        Log.Debug("[Metallurgy+] Instance constructed");
    }

    public void RegisterPrototypes(ProtoRegistrator registrator)
    {
        Log.Info("[Metallurgy+] Registering prototypes");

        registrator.RegisterAllProducts();
        registrator.RegisterData<ProductData>();
        registrator.RegisterData<OpenHearthFurnaceData>();
        registrator.RegisterData<CokingOven>();
        registrator.RegisterData<MachineData>();
        registrator.RegisterData<RecipesData>();
        registrator.RegisterDataWithInterface<IResearchNodesData>();
        registrator.RegisterData<TradeData>();
        registrator.RegisterData<GoalsData>();
    }

    public void RegisterDependencies(DependencyResolverBuilder depBuilder, ProtosDb protosDb, bool gameWasLoaded) { }

    public void EarlyInit(DependencyResolver resolver) { }

    public void Initialize(DependencyResolver resolver, bool gameWasLoaded)
    {
        ProductData.OnInitialize(resolver, gameWasLoaded);
    }
}
