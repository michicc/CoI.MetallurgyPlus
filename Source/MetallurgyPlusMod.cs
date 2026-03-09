using CoI.MetallurgyPlus.Data;
using Mafi;
using Mafi.Base;
using Mafi.Collections;
using Mafi.Core;
using Mafi.Core.Game;
using Mafi.Core.Mods;
using Mafi.Core.Prototypes;

namespace CoI.MetallurgyPlus;

public sealed class MetallurgyPlusMod : IMod
{
    public bool IsUiOnly => false;

    public Option<IConfig> ModConfig => default;

    public ModManifest Manifest { get; private set; }

    public ModJsonConfig JsonConfig { get; }

    public MetallurgyPlusMod(ModManifest manifest)
    {
        Manifest = manifest;
        JsonConfig = new(this);

        Log.Info($"{manifest.DisplayName} v{manifest.Version}");
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

    public void MigrateJsonConfig(VersionSlim savedVersion, Dict<string, object> savedValues) { }

    public void Dispose() { }
}
