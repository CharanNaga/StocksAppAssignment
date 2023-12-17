using Entities;

namespace ServiceContracts.DTO
{
    public class SellOrderResponse:IOrderResponse
    {
        public Guid SellOrderID { get; set; }

        public string? StockSymbol { get; set; }

        public string? StockName { get; set; }

        public DateTime DateAndTimeOfOrder { get; set; }

        public uint Quantity { get; set; }

        public double Price { get; set; }
        public OrderType TypeOfOrder => OrderType.SellOrder;

        public double TradeAmount { get; set; }

        public override bool Equals(object? obj)
        {
            if (obj == null)
                return false;

            if (obj.GetType() != typeof(SellOrderResponse))
                return false;

            SellOrderResponse other = (SellOrderResponse)obj;
            return SellOrderID == other.SellOrderID
                && StockSymbol == other.StockSymbol
                && StockName == other.StockName
                && DateAndTimeOfOrder == other.DateAndTimeOfOrder
                && Quantity == other.Quantity
                && Price == other.Price;
        }

        public override string ToString()
        {
            return $"Sell Order ID: {SellOrderID}, Stock Symbol: {StockSymbol}, Stock Name: {StockName}, Date and Time of Sell Order: {DateAndTimeOfOrder.ToString("dd MMM yyyy hh:mm ss tt")}, Quantity: {Quantity}, Sell Price: {Price}, Trade Amount: {TradeAmount}";
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
    public static class SellOrderExtensions
    {
        public static SellOrderResponse ToSellOrderResponse(this SellOrder sellOrder)
        {
            return new SellOrderResponse()
            {
                SellOrderID = sellOrder.SellOrderID,
                StockSymbol = sellOrder.StockSymbol,
                StockName = sellOrder.StockName,
                DateAndTimeOfOrder = sellOrder.DateAndTimeOfOrder,
                Quantity = sellOrder.Quantity,
                Price = sellOrder.Price,
                TradeAmount = sellOrder.Quantity * sellOrder.Price
            };
        }
    }
}
