using RepositoryContracts;

namespace Repositories
{
    public class FinnhubRepository : IFinnhubRepository
    {
        public Task<Dictionary<string, object>?> GetCompanyProfile(string stockSymbol)
        {
            throw new NotImplementedException();
        }

        public Task<Dictionary<string, object>?> GetStockPriceQuote(string stockSymbol)
        {
            throw new NotImplementedException();
        }

        public Task<List<Dictionary<string, string>>?> GetStocks()
        {
            throw new NotImplementedException();
        }

        public Task<Dictionary<string, object>?> SearchStocks(string stockSymbolToSearch)
        {
            throw new NotImplementedException();
        }
    }
}
