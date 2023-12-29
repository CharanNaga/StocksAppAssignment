namespace ServiceContracts
{
    /// <summary>
    /// Represents a service that makes HTTP requests to finnhub.io for retrieving stock price details
    /// </summary>
    public interface IFinnhubStockPriceQuoteService
    {
        /// <summary>
        /// Returns stock price details such as current price, change in price, percentage change, high price of the day, low price of the day, open price of the day, previous close price
        /// </summary>
        /// <param name="stockSymbol">Stock symbol to search</param>
        /// <returns>Returns a dictionary that contains details such as current price, change in price, percentage change, high price of the day, low price of the day, open price of the day, previous close price</returns>
        Task<Dictionary<string, object>?> GetStockPriceQuote(string stockSymbol);
    }
}