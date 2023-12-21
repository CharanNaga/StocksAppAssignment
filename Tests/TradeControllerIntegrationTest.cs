using Fizzler.Systems.HtmlAgilityPack;
using FluentAssertions;
using HtmlAgilityPack;

namespace Tests
{
    //IClassFixture<> provides the object of CustomWebApplicationFactory
    public class TradeControllerIntegrationTest : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly HttpClient _httpClient;

        public TradeControllerIntegrationTest(CustomWebApplicationFactory factory)
        {
            _httpClient = factory.CreateClient();
        }

        #region Orders
        [Fact]
        public async Task Orders_ToReturnView()
        {
            //Arrange

            //Act
            //HttpResponseMessage response = await _httpClient.GetAsync("/Trade/Index/MSFT");
            HttpResponseMessage response = await _httpClient.GetAsync("/Trade/Orders");

            //Assert
            response.Should().BeSuccessful(); //2xx

            string responseBody = await response.Content.ReadAsStringAsync();

            HtmlDocument html = new HtmlDocument();
            html.LoadHtml(responseBody);
            var document = html.DocumentNode;

            document.QuerySelectorAll(".price").Should().NotBeNull();
        }

        #endregion
    }
}