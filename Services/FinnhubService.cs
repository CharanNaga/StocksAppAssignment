using ServiceContracts;

namespace Services
{
    public class FinnhubService:IFinnhubService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        public FinnhubService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public Dictionary<string, object>? GetCompanyProfile(string stockSymbol)
        {
            throw new NotImplementedException();
        }

        public Dictionary<string, object>? GetStockPriceQuote(string stockSymbol)
        {
            throw new NotImplementedException();
        }
    }
}