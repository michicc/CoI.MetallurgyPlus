using CoI.MetallurgyPlus.Extensions;
using Mafi;
using Mafi.Base;
using Mafi.Base.Prototypes.Products;
using Mafi.Core;
using Mafi.Core.Economy;
using Mafi.Core.Entities;
using Mafi.Core.Entities.Dynamic;
using Mafi.Core.Fleet;
using Mafi.Core.Mods;
using Mafi.Core.Products;
using Mafi.Core.Prototypes;
using Mafi.Core.Terrain;
using Mafi.Core.World.Entities;
using System.Linq;

namespace CoI.MetallurgyPlus.Data;

internal class ProductData : IModData
{
    public void RegisterData(ProtoRegistrator registrator)
    {
        RegisterProducts(registrator);

        // Tile surfaces.
        var surfaceProto = registrator.PrototypesDb.GetOrThrow<TerrainTileSurfaceProto>(Ids.TerrainTileSurfaces.Metal2);
        surfaceProto.SetField(nameof(TerrainTileSurfaceProto.CostPerTile), new ProductQuantity(registrator.PrototypesDb.GetOrThrow<ProductProto>(Ids.Products.Steel), new Quantity(1)));
    }

    private void RegisterProducts(ProtoRegistrator registrator)
    {
        ProductBuilder.AddLooseProduct(registrator, ModIDs.Products.Charcoal, "Charcoal", "Assets/CoI.Metallurgy+/Materials/Charcoal.mat", ProductBuilder.PileRough, icon: "Assets/CoI.Metallurgy+/Icons/Charcoal.png", resourceColor: 0x404040, pinToHomeScreen: true, doNotTrackSource: true);
        ProductBuilder.AddLooseProduct(registrator, ModIDs.Products.Coke, "Coke", "Assets/CoI.Metallurgy+/Materials/Coke.mat", ProductBuilder.PileSmooth, icon: "Assets/CoI.Metallurgy+/Icons/Coke.png", doNotTrackSource: true);
        ProductBuilder.AddLooseProduct(registrator, ModIDs.Products.SpongeIron, "Sponge iron", "Assets/CoI.Metallurgy+/Materials/SpongeIron.mat", ProductBuilder.PileSmooth, icon: "Assets/CoI.Metallurgy+/Icons/SpongeIron.png");

        ProductBuilder.AddFluidProduct(registrator, ModIDs.Products.CoalTar, "Coal tar", 0x161616, "Assets/CoI.Metallurgy+/Icons/CoalTar.png", pipeColor: 0x151515, accentColor: 0xC6A13C, canBeDiscarded: true);
        ProductBuilder.AddFluidProduct(registrator, ModIDs.Products.Argon, "Argon", 0x7B9E7D, "Assets/CoI.Metallurgy+/Icons/Argon.png", pipeColor: 0x1B7A22, accentColor: 0xBBBBBB, canBeDiscarded: true);
        ProductBuilder.AddFluidProduct(registrator, ModIDs.Products.Syngas, "Syngas", 0x659DA5, "Assets/CoI.Metallurgy+/Icons/Syngas.png", pipeColor: 0x197887, accentColor: 0x96C4CC, canBeDiscarded: true);
        ProductBuilder.AddFluidProduct(registrator, ModIDs.Products.SyngasUsed, "Syngas (used)", 0x96C4CC, "Assets/CoI.Metallurgy+/Icons/SyngasUsed.png", pipeColor: 0x96C4CC, accentColor: 0x197887, canBeDiscarded: true);
    }

    public static void SwitchIronToSteel(ProtosDb protosDb)
    {
        var steelProto = protosDb.GetOrThrow<ProductProto>(Ids.Products.Steel);
        var ironProto = protosDb.GetOrThrow<ProductProto>(Ids.Products.Iron);
        ironProto.SetField("PinToHomeScreenByDefault", false);

        // Convert costs of builds and vehicles.
        foreach (EntityProto entity in protosDb.All(typeof(EntityProto)).Cast<EntityProto>()) {
            // Any iron in the costs?
            var cost = entity.Costs;
            if (cost.BaseConstructionCost.GetQuantityOf(ironProto).IsNotZero) {
                // Set new costs.
                var newPrice = new AssetValue(cost.BaseConstructionCost.Products.Map(MapIronToSteel));
                entity.SetProperty(nameof(EntityProto.Costs), new EntityCosts(newPrice, cost.DefaultPriority, cost.Workers, cost.Maintenance));

                if (entity is DrivingEntityProto vehicle) {
                    var newBuildCost = new AssetValue(vehicle.CostToBuild.Products.Map(MapIronToSteel));
                    vehicle.SetProperty(nameof(DrivingEntityProto.CostToBuild), newBuildCost);
                }
            }
        }

        // Convert costs of battleship parts.
        foreach (FleetEntityPartProto fleetEntity in protosDb.All(typeof(FleetEntityPartProto)).Cast<FleetEntityPartProto>()) {
            if (fleetEntity.Value.GetQuantityOf(ironProto).IsNotZero) {
                var newValue = new AssetValue(fleetEntity.Value.Products.Map(MapIronToSteel));
                fleetEntity.SetField(nameof(FleetEntityPartProto.Value), newValue);
            }
        }

        // Convert costs of world map cargo ships.
        foreach (WorldMapCargoShipWreckProto wreck in protosDb.All(typeof(WorldMapCargoShipWreckProto)).Cast<WorldMapCargoShipWreckProto>()) {
            if (wreck.CostToRepair.GetQuantityOf(ironProto).IsNotZero) {
                var newCost = new AssetValue(wreck.CostToRepair.Products.Map(MapIronToSteel));
                wreck.SetField(nameof(WorldMapCargoShipWreckProto.CostToRepair), newCost);
            }
        }

        ProductQuantity MapIronToSteel(ProductQuantity quantity)
        {
            if (quantity.Product == ironProto) {
                return new ProductQuantity(steelProto, quantity.Quantity.CeilDiv(2).Max(Quantity.One)); // Half iron amount for steel.
            } else {
                return quantity;
            }
        }
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
