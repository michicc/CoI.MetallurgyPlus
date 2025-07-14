using CoI.MetallurgyPlus.Extensions;
using Mafi;
using Mafi.Base;
using Mafi.Core.Mods;
using Mafi.Core.Products;

namespace CoI.MetallurgyPlus.Data;

internal class ProductData : IModData
{
    public void RegisterData(ProtoRegistrator registrator)
    {
        registrator.PrototypesDb.GetOrThrow<ProductProto>(Ids.Products.Iron).SetField("PinToHomeScreenByDefault", false);
    }

    public static void OnInitialize(DependencyResolver resolver, bool gameWasLoaded)
    {
        if (gameWasLoaded) return;

        if (resolver.TryGetResolvedDependency(out StartingFactoryConfig sfc)) {
            // Replace Coal with Charcoal for initial shipyard products.
            var initialProducts = sfc.InitialProducts.ToLyst();
            for (int i = 0; i < initialProducts.Count; i++) {
                if (initialProducts[i].Key == Ids.Products.Coal) {
                    initialProducts[i] = Make.Kvp(ModIDs.Products.Charcoal, initialProducts[i].Value);
                }
            }
            sfc.SetProperty(nameof(StartingFactoryConfig.InitialProducts), initialProducts.ToImmutableArray());
        } else {
            Log.Warning("[Metallurgy+] Could not get StartingFactoryConfig");
        }
    }
}
