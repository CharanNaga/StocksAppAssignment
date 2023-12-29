namespace ServiceContracts
{
    /// <summary>
    /// Represents a service that makes HTTP requests to finnhub.io for retrieving all stocks supported by an exchange
    /// </summary>
    public interface IFinnhubStocksService
    {
        /// <summary>
        /// Returns list of all stocks supported by an exchange (default: US)
        /// </summary>
        /// <returns>List of stocks</returns>
        Task<List<Dictionary<string, string>>?> GetStocks();
    }
}