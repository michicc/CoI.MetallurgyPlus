using CoI.MetallurgyPlus.Extensions;
using Mafi;
using Mafi.Base;
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
        SwitchIronToSteel(registrator.PrototypesDb);

        // Tile surfaces.
        var surfaceProto = registrator.PrototypesDb.GetOrThrow<TerrainTileSurfaceProto>(Ids.TerrainTileSurfaces.Metal2);
        surfaceProto.SetField(nameof(TerrainTileSurfaceProto.CostPerTile), new ProductQuantity(registrator.PrototypesDb.GetOrThrow<ProductProto>(Ids.Products.Steel), new Quantity(1)));
    }

    public void SwitchIronToSteel(ProtosDb protosDb)
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
                return new ProductQuantity(steelProto, (quantity.Quantity / 2).Max(Quantity.One)); // Half iron amount for steel.
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
