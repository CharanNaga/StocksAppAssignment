using ServiceContracts;
using ServiceContracts.DTO;
using Services;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Runtime.ConstrainedExecution;
using System;

namespace Tests
{
    public class StocksServiceTest
    {
        private readonly IStocksService _stocksService;

        public StocksServiceTest()
        {
            _stocksService = new StocksService();
        }

        #region CreateBuyOrder
        //1. When you supply BuyOrderRequest as null, it should throw ArgumentNullException.
        [Fact]
        public void CreateBuyOrder_NullBuyOrder()
        {
            //Arrange
            BuyOrderRequest? request = null;

            //Assert
            Assert.ThrowsAsync<ArgumentNullException>(async () => 
            {
                //Act
                await _stocksService.CreateBuyOrder(request);
            });
        }

        //2. When you supply buyOrderQuantity as 0 (as per the specification, minimum is 1), it should throw ArgumentException.
        [Fact]
        public void CreateBuyOrder_MinimumBuyOrderQuantity()
        {
            //Arrange
            BuyOrderRequest buyOrderRequest = new BuyOrderRequest() { Quantity = 0 };
            //Assert
            Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                //Act
                await _stocksService.CreateBuyOrder(buyOrderRequest);
            });
        }

        //3. When you supply buyOrderQuantity as 100001 (as per the specification, maximum is 100000), it should throw ArgumentException.
        [Fact]
        public void CreateBuyOrder_MaximumBuyOrderQuantity()
        {
            //Arrange
            BuyOrderRequest buyOrderRequest = new BuyOrderRequest() { Quantity = 100001 };
            //Assert
            Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                //Act
                await _stocksService.CreateBuyOrder(buyOrderRequest);
            });
        }

        //4. When you supply buyOrderPrice as 0 (as per the specification, minimum is 1), it should throw ArgumentException.
        [Fact]
        public void CreateBuyOrder_MinimumBuyOrderPrice()
        {
            //Arrange
            BuyOrderRequest buyOrderRequest = new BuyOrderRequest() { Price = 0 };
            //Assert
            Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                //Act
                await _stocksService.CreateBuyOrder(buyOrderRequest);
            });
        }

        //5. When you supply buyOrderPrice as 10001 (as per the specification, maximum is 10000), it should throw ArgumentException.
        [Fact]
        public void CreateBuyOrder_MaximumBuyOrderPrice()
        {
            //Arrange
            BuyOrderRequest buyOrderRequest = new BuyOrderRequest() { Price = 10001 };
            //Assert
            Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                //Act
                await _stocksService.CreateBuyOrder(buyOrderRequest);
            });
        }

        //6. When you supply stock symbol=null (as per the specification, stock symbol can't be null), it should throw ArgumentException.
        [Fact]
        public void CreateBuyOrder_StockSymbolIsNull()
        {
            //Arrange
            BuyOrderRequest buyOrderRequest = new BuyOrderRequest() { StockSymbol = null };
            //Assert
            Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                //Act
                await _stocksService.CreateBuyOrder(buyOrderRequest);
            });
        }

        //7. When you supply dateAndTimeOfOrder as "1999-12-31" (YYYY-MM-DD) - (as per the specification, it should be equal or newer date than 2000-01-01), it should throw ArgumentException.
        [Fact]
        public void CreateBuyOrder_OlderDateAndTimeBuyOrder()
        {
            //Arrange
            BuyOrderRequest buyOrderRequest = new BuyOrderRequest() 
            { 
                DateAndTimeOfOrder =  DateTime.Parse("1999-12-31")
            };
            //Assert
            Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                //Act
                await _stocksService.CreateBuyOrder(buyOrderRequest);
            });
        }

        //8. If you supply all valid values, it should be successful and return an object of BuyOrderResponse type with auto-generated BuyOrderID(guid).
        [Fact]
        public async void CreateBuyOrder_ValidBuyOrderDetails()
        {
            //Arrange
            BuyOrderRequest buyOrderRequest = new BuyOrderRequest()
            {
                StockSymbol = "sample symbol",
                StockName = "Test",
                DateAndTimeOfOrder = DateTime.Parse("2001-04-01"),
                Quantity = 5,
                Price = 50
            };
            BuyOrderResponse buyOrderResponse = await _stocksService.CreateBuyOrder(buyOrderRequest);

            Assert.True(buyOrderResponse.BuyOrderID!= Guid.Empty);
        }
        #endregion


    }
}
