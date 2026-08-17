using CoI.MetallurgyPlus.Data;
using Mafi;
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

    private bool ConfigKeepIronPlates => JsonConfig.GetBool("keep_iron_plates");
    private bool ConfigReplaceIronInRecipes => JsonConfig.GetBool("replace_iron_in_recipes");

    public MetallurgyPlusMod(ModManifest manifest)
    {
        Manifest = manifest;
        JsonConfig = new(this);

        Log.Info($"{manifest.DisplayName} v{manifest.Version}");
    }

    public void RegisterPrototypes(ProtoRegistrator registrator)
    {
        Log.Info("[Metallurgy+] Registering prototypes");
        if (ConfigKeepIronPlates) Log.Info("[Metallurgy+] Compatibility option for keeping iron plates enabled.");

        registrator.RegisterAllProducts();
        registrator.RegisterData<ProductData>();
        registrator.RegisterData<OpenHearthFurnaceData>();
        registrator.RegisterData<CokingOven>();
        registrator.RegisterData<AirSeparatorData>();
        registrator.RegisterData<MachineData>();
        registrator.RegisterData<RecipesData>(x => x.KeepIronCasting = ConfigKeepIronPlates);
        registrator.RegisterData<ResearchData>(x => x.KeepIronCasting = ConfigKeepIronPlates);
        registrator.RegisterData<TradeData>();
        registrator.RegisterData<GoalsData>();

        registrator.PrototypesDb.AddOnLockCheck(ReplaceIronWithSteel);
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

    private void ReplaceIronWithSteel(ProtosDb protosDb)
    {
        ProductData.SwitchIronToSteel(protosDb);

        if (ConfigReplaceIronInRecipes) {
            Log.Info("[Metallurgy+] Trying to replace all uses of iron plates with steel.");
            RecipesData.SwithIronToSteel(protosDb, ConfigKeepIronPlates);
        }
    }
}
