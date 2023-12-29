using Exceptions;
using Microsoft.Extensions.Logging;
using RepositoryContracts;
using ServiceContracts;

namespace Services
{
    public class FinnhubSearchStocksService:IFinnhubSearchStocksService
    {
        private readonly IFinnhubRepository _finnhubRepository;
        private readonly ILogger<FinnhubSearchStocksService> _logger;
        public FinnhubSearchStocksService(IFinnhubRepository finnhubRepository,ILogger<FinnhubSearchStocksService> logger)
        {
           _finnhubRepository = finnhubRepository;
           _logger = logger;
        }

        public async Task<Dictionary<string, object>?> SearchStocks(string stockSymbolToSearch)
        {
            Dictionary<string, object>? responseDictionary = await _finnhubRepository.SearchStocks(stockSymbolToSearch);
            return responseDictionary;

            //try
            //{
            //    Dictionary<string, object>? responseDictionary = await _finnhubRepository.SearchStocks(stockSymbolToSearch);
            //    return responseDictionary;
            //}
            //catch(Exception ex)
            //{
            //    throw new FinnhubException("Unable to connect to finnhub ", ex);
            //}
        }
    }
}