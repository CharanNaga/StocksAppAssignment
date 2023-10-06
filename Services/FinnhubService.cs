using ServiceContracts;
using System.Text.Json;

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
            //create http client
            HttpClient httpClient = _httpClientFactory.CreateClient();

            //create http request
            HttpRequestMessage httpRequestMessage = new HttpRequestMessage()
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri($"https://finnhub.io/api/v1/quote?symbol=MSFT&token=ckg0eq1r01qknh1jk6c0ckg0eq1r01qknh1jk6cg") //URI includes the secret token
            };

            //send request
            HttpResponseMessage httpResponseMessage = httpClient.Send(httpRequestMessage);

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
}