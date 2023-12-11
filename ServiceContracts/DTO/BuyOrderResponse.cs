using Entities;

namespace ServiceContracts.DTO
{
    public class BuyOrderResponse
    {
        public Guid BuyOrderID { get; set; }

        public string? StockSymbol { get; set; }

        public string? StockName { get; set; }

        public DateTime DateAndTimeOfOrder { get; set; }

        public uint Quantity { get; set; }

        public double Price { get; set; }

        public double TradeAmount { get; set; }

        public override bool Equals(object? obj)
        {
            if(obj == null)
                return false;

            if (obj.GetType() != typeof(BuyOrderResponse))
                return false;

            BuyOrderResponse buyOrderResponse = (BuyOrderResponse)obj;

            return BuyOrderID == buyOrderResponse.BuyOrderID
                && StockSymbol == buyOrderResponse.StockSymbol
                && StockName == buyOrderResponse.StockName
                && DateAndTimeOfOrder == buyOrderResponse.DateAndTimeOfOrder
                && Quantity == buyOrderResponse.Quantity
                && Price == buyOrderResponse.Price;
        }

        public override string ToString()
        {
            return $"Buy Order ID: {BuyOrderID}, Stock Symbol: {StockSymbol}, Stock Name: {StockName}, Date and Time of Buy Order: {DateAndTimeOfOrder.ToString("dd MMM yyyy hh:mm ss tt")}, Quantity: {Quantity}, Buy Price: {Price}, Trade Amount: {TradeAmount}";
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
    public static class BuyOrderExtensions
    {
        public static BuyOrderResponse ToBuyOrderResponse(this BuyOrderResponse buyOrderResponse)
        {
            return new BuyOrderResponse()
            {
                BuyOrderID = buyOrderResponse.BuyOrderID,
                StockSymbol = buyOrderResponse.StockSymbol,
                StockName = buyOrderResponse.StockName,
                DateAndTimeOfOrder = buyOrderResponse.DateAndTimeOfOrder,
                Quantity = buyOrderResponse.Quantity,
                Price = buyOrderResponse.Price,
                TradeAmount = buyOrderResponse.Price * buyOrderResponse.Quantity
            };
        }
    }
}
