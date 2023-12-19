using RepositoryContracts;
using ServiceContracts;

namespace Services
{
    public class FinnhubService:IFinnhubService
    {
        private readonly IFinnhubRepository _finnhubRepository;
        public FinnhubService(IFinnhubRepository finnhubRepository)
        {
           _finnhubRepository = finnhubRepository;
        }

        public async Task<Dictionary<string, object>?> GetCompanyProfile(string stockSymbol)
        {
            var responseDictionary = await _finnhubRepository.GetCompanyProfile(stockSymbol);
            return responseDictionary;
        }

        public async Task<Dictionary<string, object>?> GetStockPriceQuote(string stockSymbol)
        {
            var responseDictionary = await _finnhubRepository.GetStockPriceQuote(stockSymbol);
            return responseDictionary;
        }

        public async Task<List<Dictionary<string, string>>?> GetStocks()
        {
            var responseDictionaryList  = await _finnhubRepository.GetStocks();
            return responseDictionaryList;
        }

        public async Task<Dictionary<string, object>?> SearchStocks(string stockSymbolToSearch)
        {
            var responseDictionary = await _finnhubRepository.SearchStocks(stockSymbolToSearch);
            return responseDictionary;
        }
    }
}