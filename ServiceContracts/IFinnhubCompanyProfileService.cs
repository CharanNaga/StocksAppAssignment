namespace ServiceContracts
{
    /// <summary>
    /// Represents a service that makes HTTP requests to finnhub.io for retrieving company details
    /// </summary>
    public interface IFinnhubCompanyProfileService
    {
        /// <summary>
        /// Returns company details such as company country, currency, exchange, IPO date, logo image, market capitalization, name of the company, phone number etc.
        /// </summary>
        /// <param name="stockSymbol">Stock symbol to search</param>
        /// <returns>Returns a dictionary that contains details such as company country, currency, exchange, IPO date, logo image, market capitalization, name of the company, phone number etc.</returns>
        Task<Dictionary<string, object>?> GetCompanyProfile(string stockSymbol);
    }
}