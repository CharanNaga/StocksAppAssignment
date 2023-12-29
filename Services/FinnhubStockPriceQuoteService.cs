using Exceptions;
using Microsoft.Extensions.Logging;
using RepositoryContracts;
using ServiceContracts;

namespace Services
{
    public class FinnhubStockPriceQuoteService:IFinnhubStockPriceQuoteService
    {
        private readonly IFinnhubRepository _finnhubRepository;
        private readonly ILogger<FinnhubStockPriceQuoteService> _logger;
        public FinnhubStockPriceQuoteService(IFinnhubRepository finnhubRepository,ILogger<FinnhubStockPriceQuoteService> logger)
        {
           _finnhubRepository = finnhubRepository;
           _logger = logger;
        }

        public async Task<Dictionary<string, object>?> GetStockPriceQuote(string stockSymbol)
        {
            //writing log message
            _logger.LogInformation($"In {nameof(FinnhubStockPriceQuoteService)}.{nameof(GetStockPriceQuote)} Service method");
            Dictionary<string, object>? responseDictionary = await _finnhubRepository.GetStockPriceQuote(stockSymbol);
            return responseDictionary;

            //try
            //{
            //    //writing log message
            //    _logger.LogInformation($"In {nameof(FinnhubService)}.{nameof(GetStockPriceQuote)} Service method");
            //    Dictionary<string, object>? responseDictionary = await _finnhubRepository.GetStockPriceQuote(stockSymbol);
            //    return responseDictionary;
            //}
            //catch(Exception ex)
            //{
            //    throw new FinnhubException("Unable to connect to finnhub ", ex);
            //}
        }
    }
}