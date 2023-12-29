using Exceptions;
using Microsoft.Extensions.Logging;
using RepositoryContracts;
using ServiceContracts;

namespace Services
{
    public class FinnhubCompanyProfileService:IFinnhubCompanyProfileService
    {
        private readonly IFinnhubRepository _finnhubRepository;
        private readonly ILogger<FinnhubCompanyProfileService> _logger;
        public FinnhubCompanyProfileService(IFinnhubRepository finnhubRepository,ILogger<FinnhubCompanyProfileService> logger)
        {
           _finnhubRepository = finnhubRepository;
           _logger = logger;
        }
        public async Task<Dictionary<string, object>?> GetCompanyProfile(string stockSymbol)
        {
            //writing log message
            _logger.LogInformation($"In {nameof(FinnhubCompanyProfileService)}.{nameof(GetCompanyProfile)} Service method");
            Dictionary<string, object>? responseDictionary = await _finnhubRepository.GetCompanyProfile(stockSymbol);
            return responseDictionary;

            //try
            //{
            //    //writing log message
            //    _logger.LogInformation($"In {nameof(FinnhubService)}.{nameof(GetCompanyProfile)} Service method");
            //    Dictionary<string, object>? responseDictionary = await _finnhubRepository.GetCompanyProfile(stockSymbol);
            //    return responseDictionary;
            //}
            //catch (Exception ex)
            //{
            //    throw new FinnhubException("Unable to connect to finnhub ",ex);
            //}
        }
    }
}