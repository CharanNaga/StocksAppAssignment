using ServiceContracts.DTO;

namespace StocksAppAssignment.Models
{
    /// <summary>
    /// Represents model class to supply list of buy orders and sell orders to the Trade/Orders view
    /// </summary>
    public class Orders
    {
        public List<BuyOrderResponse> BuyOrders { get; set; } = new List<BuyOrderResponse>();

        public List<SellOrderResponse> SellOrders { get; set; } = new List<SellOrderResponse>();
    }
}
