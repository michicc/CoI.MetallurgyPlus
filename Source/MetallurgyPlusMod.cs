using CoI.MetallurgyPlus.Data;
using Mafi;
using Mafi.Base;
using Mafi.Collections;
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
        bool keepIron = JsonConfig.GetBool("keep_iron_plates");

        Log.Info("[Metallurgy+] Registering prototypes");
        if (keepIron) Log.Info("[Metallurgy+] Compatibility option for keeping iron plates enabled.");

        registrator.RegisterAllProducts();
        registrator.RegisterData<ProductData>();
        registrator.RegisterData<OpenHearthFurnaceData>();
        registrator.RegisterData<CokingOven>();
        registrator.RegisterData<AirSeparatorData>();
        registrator.RegisterData<MachineData>();
        registrator.RegisterData<RecipesData>(x => x.KeepIronCasting = keepIron);
        registrator.RegisterData<ResearchData>(x => x.KeepIronCasting = keepIron);
        registrator.RegisterData<TradeData>();
        registrator.RegisterData<GoalsData>();
    }

    public void RegisterDependencies(DependencyResolverBuilder depBuilder, ProtosDb protosDb, bool gameWasLoaded) { }

    public void EarlyInit(DependencyResolver resolver) { }

    public void Initialize(DependencyResolver resolver, bool gameWasLoaded)
    {
        ProductData.OnInitialize(resolver, gameWasLoaded);
        MachineData.OnInitialize(resolver, gameWasLoaded);
        AirSeparatorData.OnInitialize(resolver, gameWasLoaded);
    }

    public void MigrateJsonConfig(VersionSlim savedVersion, Dict<string, object> savedValues) { }

    public void Dispose() { }
}
