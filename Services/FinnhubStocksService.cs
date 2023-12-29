using Exceptions;
using Microsoft.Extensions.Logging;
using RepositoryContracts;
using ServiceContracts;

namespace Services
{
    public class FinnhubStocksService:IFinnhubStocksService
    {
        private readonly IFinnhubRepository _finnhubRepository;
        private readonly ILogger<FinnhubStocksService> _logger;
        public FinnhubStocksService(IFinnhubRepository finnhubRepository,ILogger<FinnhubStocksService> logger)
        {
           _finnhubRepository = finnhubRepository;
           _logger = logger;
        }
        public async Task<List<Dictionary<string, string>>?> GetStocks()
        {
            List<Dictionary<string, string>>? responseDictionaryList = await _finnhubRepository.GetStocks();
            return responseDictionaryList;

            //try
            //{
            //    List<Dictionary<string, string>>? responseDictionaryList = await _finnhubRepository.GetStocks();
            //    return responseDictionaryList;
            //}
            //catch(Exception ex)
            //{
            //    throw new FinnhubException("Unable to connect to finnhub", ex);
            //}
        }
    }
}