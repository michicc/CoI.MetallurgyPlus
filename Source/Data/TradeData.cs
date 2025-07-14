using CoI.MetallurgyPlus.Extensions;
using Mafi;
using Mafi.Base;
using Mafi.Core.Mods;
using Mafi.Core.Products;
using Mafi.Core.Prototypes;
using Mafi.Core.World.QuickTrade;

namespace CoI.MetallurgyPlus.Data;

internal class TradeData : IModData
{
    public void RegisterData(ProtoRegistrator registrator)
    {
        OverrideQuickTrades(registrator.PrototypesDb);
    }

    private void OverrideQuickTrades(ProtosDb protosDb)
    {
        // Iron -> Concrete
        var proto = protosDb.GetOrThrow<QuickTradePairProto>(new Proto.ID($"Trade_{Ids.Products.ConcreteSlab.Value}_For_{Ids.Products.Iron.Value}"));
        proto.SetField("ProductToPayWith", protosDb.GetOrThrow<ProductProto>(Ids.Products.Steel).WithQuantity(10));
    }
}
