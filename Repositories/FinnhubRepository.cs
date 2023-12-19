using Microsoft.Extensions.Configuration;
using RepositoryContracts;
using System.Text.Json;

namespace Repositories
{
    public class FinnhubRepository : IFinnhubRepository
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;

        public FinnhubRepository(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
        }

        public async Task<Dictionary<string, object>?> GetCompanyProfile(string stockSymbol)
        {
            //create http client
            using (HttpClient httpClient = _httpClientFactory.CreateClient())
            {
                //create http request
                HttpRequestMessage httpRequestMessage = new HttpRequestMessage()
                {
                    Method = HttpMethod.Get,
                    RequestUri = new Uri($"https://finnhub.io/api/v1/stock/profile2?symbol={stockSymbol}&token={_configuration["FinnhubToken"]}") //URI includes the secret token
                };

                //send request
                HttpResponseMessage httpResponseMessage = await httpClient.SendAsync(httpRequestMessage);

                //read response body
                string responseBody = new StreamReader(httpResponseMessage.Content.ReadAsStream()).ReadToEnd();

                //convert response body (from JSON into Dictionary)
                Dictionary<string, object>? responseDictionary = JsonSerializer.Deserialize<Dictionary<string, object>>(responseBody);

                if (responseDictionary == null)
                    throw new InvalidOperationException("No response from server");

                if (responseDictionary.ContainsKey("error"))
                    throw new InvalidOperationException(Convert.ToString(responseDictionary["error"]));

                //return response dictionary back to the caller
                return responseDictionary;
            }
        }

        public async Task<Dictionary<string, object>?> GetStockPriceQuote(string stockSymbol)
        {
            using (HttpClient httpClient = _httpClientFactory.CreateClient()) //creates and returns object of HttpClient class and place it in using block such that it disposes object at the end.
            {
                HttpRequestMessage httpRequestMessage = new HttpRequestMessage() //represent actual http request.
                {
                    RequestUri = new Uri($"https://finnhub.io/api/v1/quote?symbol={stockSymbol}&token={_configuration["FinnhubToken"]}"), //pasting the api key for stocks price-> quotes curl from finnhub.io
                    Method = HttpMethod.Get
                };
                HttpResponseMessage httpResponseMessage = await httpClient.SendAsync(httpRequestMessage); //our server acts as a client to external source, so need to receive response. Sends request to finnhub.io and validates token provided. If valid, provides the response.

                //read responsebody
                string responseBody = await httpResponseMessage.Content.ReadAsStringAsync();

                //Converting Json into a dictionary object.
                Dictionary<string, object>? responseDictionary = JsonSerializer.Deserialize<Dictionary<string, object>>(responseBody);

                if (responseDictionary == null)
                    throw new InvalidOperationException("No response from finnhub server");

                if (responseDictionary.ContainsKey("error"))
                    throw new InvalidOperationException(Convert.ToString(responseDictionary["error"]));
                return responseDictionary;
            }
        }

        public async Task<List<Dictionary<string, string>>?> GetStocks()
        {
            //create http client
            using (HttpClient httpClient = _httpClientFactory.CreateClient())
            {
                //create http request
                HttpRequestMessage httpRequestMessage = new HttpRequestMessage()
                {
                    Method = HttpMethod.Get,
                    RequestUri = new Uri($"https://finnhub.io/api/v1/stock/symbol?exchange=US&token={_configuration["FinnhubToken"]}") //URI includes the secret token
                };

                //send request
                HttpResponseMessage httpResponseMessage = await httpClient.SendAsync(httpRequestMessage);

                //read response body
                string responseBody = await httpResponseMessage.Content.ReadAsStringAsync();

                //convert response body (from JSON into Dictionary)
                List<Dictionary<string,string>>? responseDictionary = JsonSerializer.Deserialize<List<Dictionary<string, string>>>(responseBody);

                if (responseDictionary == null)
                    throw new InvalidOperationException("No response from server");

                //return response dictionary back to the caller
                return responseDictionary;
            }
        }

        public Task<Dictionary<string, object>?> SearchStocks(string stockSymbolToSearch)
        {
            throw new NotImplementedException();
        }
    }
}
