namespace ServiceContracts
{
    /// <summary>
    /// Represents a service that makes HTTP requests to finnhub.io for retrieving matching stocks
    /// </summary>
    public interface IFinnhubSearchStocksService
    {
        /// <summary>
        /// Returns list of matching stocks based on the given stock symbol
        /// </summary>
        /// <param name="stockSymbolToSearch">Stock symbol to search</param>
        /// <returns>List of matching stocks</returns>
        Task<Dictionary<string, object>?> SearchStocks(string stockSymbolToSearch);
    }
}