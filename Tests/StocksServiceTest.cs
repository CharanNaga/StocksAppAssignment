﻿using ServiceContracts;
using ServiceContracts.DTO;
using Services;
using Xunit.Abstractions;
using Microsoft.EntityFrameworkCore;
using Entities;
using RepositoryContracts;
using Moq;
using AutoFixture;
using FluentAssertions;

namespace Tests
{
    public class StocksServiceTest
    {
        private readonly IStocksService _stocksService;
        private readonly IStocksRepository _stocksRepository;
        private readonly Mock<IStocksRepository> _stocksRepositoryMock;
        private readonly IFixture _fixture;
        private readonly ITestOutputHelper _testOutputHelper;

        public StocksServiceTest(ITestOutputHelper testOutputHelper)
        {
            _fixture = new Fixture();
            _stocksRepositoryMock = new Mock<IStocksRepository>();
            _stocksRepository = _stocksRepositoryMock.Object;
            _stocksService = new StocksService(_stocksRepository);
            _testOutputHelper = testOutputHelper;
        }

        #region CreateBuyOrder
        //1. When you supply BuyOrderRequest as null, it should throw ArgumentNullException.
        [Fact]
        public async Task CreateBuyOrder_NullBuyOrder_ToBeArgumentNullException()
        {
            //Arrange
            BuyOrderRequest? request = null;
            BuyOrder buyOrderFixture = _fixture.Create<BuyOrder>();

            //mocking
            _stocksRepositoryMock.Setup(
                temp=>temp.CreateBuyOrder(It.IsAny<BuyOrder>()))
                .ReturnsAsync(buyOrderFixture);

            //Assert
            Func<Task> action = async () =>
            {
                await _stocksService.CreateBuyOrder(request);
            };

            //Assert
            await action.Should().ThrowAsync<ArgumentNullException>();
        }

        //2. When you supply buyOrderQuantity as 0 (as per the specification, minimum is 1), it should throw ArgumentException.
        [Fact]
        public async Task CreateBuyOrder_MinimumBuyOrderQuantity_ToBeArgumentException()
        {
            uint quantity = 0;
            //Arrange
            BuyOrderRequest buyOrderRequest = _fixture.Build<BuyOrderRequest>()
                .With(temp=>temp.Quantity,quantity)
                .Create();
            BuyOrder buyOrder = buyOrderRequest.ToBuyOrder();

            //mocking 
            _stocksRepositoryMock.Setup(
                temp=>temp.CreateBuyOrder(It.IsAny<BuyOrder>()))
                .ReturnsAsync(buyOrder);

            //Act
            Func<Task> action = async () =>
            {
                await _stocksService.CreateBuyOrder(buyOrderRequest);
            };

            //Assert
            await action.Should().ThrowAsync<ArgumentException>();
        }

        //3. When you supply buyOrderQuantity as 100001 (as per the specification, maximum is 100000), it should throw ArgumentException.
        [Fact]
        public async Task CreateBuyOrder_MaximumBuyOrderQuantity_ToBeArgumentException()
        {
            uint quantity = 100001;
            //Arrange
            BuyOrderRequest buyOrderRequest = _fixture.Build<BuyOrderRequest>()
                .With(temp => temp.Quantity, quantity)
                .Create();
            BuyOrder buyOrder = buyOrderRequest.ToBuyOrder();

            //mocking 
            _stocksRepositoryMock.Setup(
                temp => temp.CreateBuyOrder(It.IsAny<BuyOrder>()))
                .ReturnsAsync(buyOrder);

            //Act
            Func<Task> action = async () =>
            {
                await _stocksService.CreateBuyOrder(buyOrderRequest);
            };

            //Assert
            await action.Should().ThrowAsync<ArgumentException>();
        }

        //4. When you supply buyOrderPrice as 0 (as per the specification, minimum is 1), it should throw ArgumentException.
        [Fact]
        public async Task CreateBuyOrder_MinimumBuyOrderPrice_ToBeArgumentException()
        {
            double price = 0;
            //Arrange
            BuyOrderRequest buyOrderRequest = _fixture.Build<BuyOrderRequest>()
                .With(temp => temp.Price, price)
                .Create();
            BuyOrder buyOrder = buyOrderRequest.ToBuyOrder();

            //mocking 
            _stocksRepositoryMock.Setup(
                temp => temp.CreateBuyOrder(It.IsAny<BuyOrder>()))
                .ReturnsAsync(buyOrder);

            //Act
            Func<Task> action = async () =>
            {
                await _stocksService.CreateBuyOrder(buyOrderRequest);
            };

            //Assert
            await action.Should().ThrowAsync<ArgumentException>();
        }

        //5. When you supply buyOrderPrice as 10001 (as per the specification, maximum is 10000), it should throw ArgumentException.
        [Fact]
        public async Task CreateBuyOrder_MaximumBuyOrderPrice_ToBeArgumentException()
        {
            double price = 10001;
            //Arrange
            BuyOrderRequest buyOrderRequest = _fixture.Build<BuyOrderRequest>()
                .With(temp => temp.Price, price)
                .Create();
            BuyOrder buyOrder = buyOrderRequest.ToBuyOrder();

            //mocking 
            _stocksRepositoryMock.Setup(
                temp => temp.CreateBuyOrder(It.IsAny<BuyOrder>()))
                .ReturnsAsync(buyOrder);

            //Act
            Func<Task> action = async () =>
            {
                await _stocksService.CreateBuyOrder(buyOrderRequest);
            };

            //Assert
            await action.Should().ThrowAsync<ArgumentException>();
        }

        //6. When you supply stock symbol=null (as per the specification, stock symbol can't be null), it should throw ArgumentException.
        [Fact]
        public async Task CreateBuyOrder_StockSymbolIsNull_ToBeArgumentException()
        {
            //Arrange
            BuyOrderRequest buyOrderRequest = _fixture.Build<BuyOrderRequest>()
                .With(temp => temp.StockSymbol, null as string)
                .Create();
            BuyOrder buyOrder = buyOrderRequest.ToBuyOrder();

            //mocking 
            _stocksRepositoryMock.Setup(
                temp => temp.CreateBuyOrder(It.IsAny<BuyOrder>()))
                .ReturnsAsync(buyOrder);

            //Act
            Func<Task> action = async () =>
            {
                await _stocksService.CreateBuyOrder(buyOrderRequest);
            };

            //Assert
            await action.Should().ThrowAsync<ArgumentException>();
        }

        //7. When you supply dateAndTimeOfOrder as "1999-12-31" (YYYY-MM-DD) - (as per the specification, it should be equal or newer date than 2000-01-01), it should throw ArgumentException.
        [Fact]
        public async Task CreateBuyOrder_OlderDateAndTimeBuyOrder_ToBeArgumentException()
        {
            DateTime dateTime = DateTime.Parse("1999-12-31");
            //Arrange
            BuyOrderRequest buyOrderRequest = _fixture.Build<BuyOrderRequest>()
                .With(temp => temp.DateAndTimeOfOrder, dateTime)
                .Create();
            BuyOrder buyOrder = buyOrderRequest.ToBuyOrder();

            //mocking 
            _stocksRepositoryMock.Setup(
                temp => temp.CreateBuyOrder(It.IsAny<BuyOrder>()))
                .ReturnsAsync(buyOrder);

            //Act
            Func<Task> action = async () =>
            {
                await _stocksService.CreateBuyOrder(buyOrderRequest);
            };

            //Assert
            await action.Should().ThrowAsync<ArgumentException>();
        }

        //8. If you supply all valid values, it should be successful and return an object of BuyOrderResponse type with auto-generated BuyOrderID(guid).
        [Fact]
        public async Task CreateBuyOrder_ValidBuyOrderDetails_ToBeSuccessful()
        {
            //Arrange
            BuyOrderRequest buyOrderRequest = _fixture.Create<BuyOrderRequest>();
            BuyOrder buyOrder = buyOrderRequest.ToBuyOrder();

            //mocking
            _stocksRepositoryMock.Setup(
                temp=>temp.CreateBuyOrder(It.IsAny<BuyOrder>()))
                .ReturnsAsync(buyOrder);

            //Act
            BuyOrderResponse buyOrderResponseFromCreateActual = await _stocksService.CreateBuyOrder(buyOrderRequest);
            buyOrder.BuyOrderID = buyOrderResponseFromCreateActual.BuyOrderID;
            BuyOrderResponse buyOrderResponseExpected = buyOrder.ToBuyOrderResponse();

            //Assert
            buyOrderResponseFromCreateActual.BuyOrderID.Should().NotBe(Guid.Empty);
            buyOrderResponseFromCreateActual.Should().Be(buyOrderResponseExpected);
        }
        #endregion

        #region CreateSellOrder
        //1. When you supply SellOrderRequest as null, it should throw ArgumentNullException.
        [Fact]
        public async Task CreateSellOrder_NullSellOrder()
        {
            //Arrange
            SellOrderRequest? request = null;

            //Assert
            await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                //Act
                await _stocksService.CreateSellOrder(request);
            });
        }

        //2. When you supply sellOrderQuantity as 0 (as per the specification, minimum is 1), it should throw ArgumentException.
        [Fact]
        public async Task CreateSellOrder_MinimumSellOrderQuantity()
        {
            //Arrange
            SellOrderRequest sellOrderRequest = new SellOrderRequest() { Quantity = 0 };
            //Assert
            await Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                //Act
                await _stocksService.CreateSellOrder(sellOrderRequest);
            });
        }

        //3. When you supply sellOrderQuantity as 100001 (as per the specification, maximum is 100000), it should throw ArgumentException.
        [Fact]
        public async Task CreateSellOrder_MaximumSellOrderQuantity()
        {
            //Arrange
            SellOrderRequest sellOrderRequest = new SellOrderRequest() { Quantity = 100001 };
            //Assert
            await Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                //Act
                await _stocksService.CreateSellOrder(sellOrderRequest);
            });
        }

        //4. When you supply sellOrderPrice as 0 (as per the specification, minimum is 1), it should throw ArgumentException.
        [Fact]
        public async Task CreateSellOrder_MinimumSellOrderPrice()
        {
            //Arrange
            SellOrderRequest sellOrderRequest = new SellOrderRequest() { Price = 0 };
            //Assert
            await Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                //Act
                await _stocksService.CreateSellOrder(sellOrderRequest);
            });
        }

        //5. When you supply sellOrderPrice as 10001 (as per the specification, maximum is 10000), it should throw ArgumentException.
        [Fact]
        public async Task CreateSellOrder_MaximumSellOrderPrice()
        {
            //Arrange
            SellOrderRequest sellOrderRequest = new SellOrderRequest() { Price = 10001 };
            //Assert
            await Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                //Act
                await _stocksService.CreateSellOrder(sellOrderRequest);
            });
        }

        //6. When you supply stock symbol=null (as per the specification, stock symbol can't be null), it should throw ArgumentException.
        [Fact]
        public async Task CreateSellOrder_StockSymbolIsNull()
        {
            //Arrange
            SellOrderRequest sellOrderRequest = new SellOrderRequest() { StockSymbol = null };
            //Assert
            await Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                //Act
                await _stocksService.CreateSellOrder(sellOrderRequest);
            });
        }

        //7. When you supply dateAndTimeOfOrder as "1999-12-31" (YYYY-MM-DD) - (as per the specification, it should be equal or newer date than 2000-01-01), it should throw ArgumentException.
        [Fact]
        public async Task CreateSellOrder_OlderDateAndTimeSellOrder()
        {
            //Arrange
            SellOrderRequest sellOrderRequest = new SellOrderRequest()
            {
                DateAndTimeOfOrder = DateTime.Parse("1999-12-31")
            };
            //Assert
            await Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                //Act
                await _stocksService.CreateSellOrder(sellOrderRequest);
            });
        }

        //8. If you supply all valid values, it should be successful and return an object of SellOrderResponse type with auto-generated SellOrderID(guid).
        [Fact]
        public async Task CreateSellOrder_ValidSellOrderDetails()
        {
            //Arrange
            SellOrderRequest sellOrderRequest = new SellOrderRequest()
            {
                StockSymbol = "sample symbol",
                StockName = "Test",
                DateAndTimeOfOrder = DateTime.Parse("2001-04-01"),
                Quantity = 5,
                Price = 50
            };
            //Act
            SellOrderResponse sellOrderResponse = await _stocksService.CreateSellOrder(sellOrderRequest);
            List<SellOrderResponse> sellOrderResponseFromGet = await _stocksService.GetSellOrders();

            //Assert
            Assert.True(sellOrderResponse.SellOrderID != Guid.Empty);
            Assert.Contains(sellOrderResponse, sellOrderResponseFromGet);
        }
        #endregion

        #region GetBuyOrders
        //1. When you invoke this method, by default, the returned list should be empty.
        [Fact]
        public async Task GetBuyOrders_DefaultList_ToBeEmpty()
        {
            //Act 
            List<BuyOrder> buyOrders = new List<BuyOrder>(); 

            //mocking 
            _stocksRepositoryMock.Setup(
                temp => temp.GetBuyOrders())
                .ReturnsAsync(buyOrders);

            //Act
            List<BuyOrderResponse> buyOrderResponseFromGet = await _stocksService.GetBuyOrders();

            //Assert
            buyOrderResponseFromGet.Should().BeEmpty();
        }

        //2. When you first add few buy orders using CreateBuyOrder() method; and then invoke GetBuyOrders() method; the returned list should contain all the same buy orders.
        [Fact]
        public async Task GetBuyOrders_WithFewBuyOrders_ToBeSuccessful()
        {
            //Arrange
            BuyOrder buyOrder1 = _fixture.Create<BuyOrder>();
            BuyOrder buyOrder2 = _fixture.Create<BuyOrder>();
            List<BuyOrder> buyOrderRequests = new List<BuyOrder> { buyOrder1, buyOrder2 };

            //Act
            List<BuyOrderResponse> buyOrderResponseExpected = buyOrderRequests.Select(temp => temp.ToBuyOrderResponse()).ToList();
            List<BuyOrderResponse> buyOrderResponseFromAdd = new List<BuyOrderResponse>();
            
            //print expected list
            _testOutputHelper.WriteLine("Expected: ");
            foreach(BuyOrderResponse response in buyOrderResponseExpected)
            {
                _testOutputHelper.WriteLine($"BuyOrderResponse: {response}");
            }

            //mocking
            _stocksRepositoryMock.Setup(temp => temp.GetBuyOrders())
                .ReturnsAsync(buyOrderRequests);

            List<BuyOrderResponse> buyOrderResponseFromGet = await _stocksService.GetBuyOrders();

            //print actual list
            _testOutputHelper.WriteLine("Actual: ");
            foreach (BuyOrderResponse response in buyOrderResponseFromGet)
            {
                _testOutputHelper.WriteLine($"BuyOrderResponse: {response}");
            }

            //Assert
            buyOrderResponseFromGet.Should().BeEquivalentTo(buyOrderResponseExpected);
        }
        #endregion

        #region GetSellOrders
        //1. When you invoke this method, by default, the returned list should be empty.
        [Fact]
        public async Task GetSellOrders_DefaultList_ToBeEmpty()
        {
            //Act 
            List<SellOrder> sellOrders = new List<SellOrder>();

            //mocking 
            _stocksRepositoryMock.Setup(
                temp => temp.GetSellOrders())
                .ReturnsAsync(sellOrders);

            //Act
            List<SellOrderResponse> sellOrderResponseFromGet = await _stocksService.GetSellOrders();

            //Assert
            sellOrderResponseFromGet.Should().BeEmpty();
        }

        //2. When you first add few sell orders using CreateSellOrder() method; and then invoke GetSellOrders() method; the returned list should contain all the same sell orders.
        [Fact]
        public async Task GetSellOrders_WithFewSellOrders_ToBeSuccessful()
        {
            //Arrange
            List<SellOrder> sellOrderRequests = new List<SellOrder>()
            {
               _fixture.Create<SellOrder>(),
               _fixture.Create<SellOrder>()
            };

            //Act
            List<SellOrderResponse> sellOrderResponseExpected = sellOrderRequests.Select(temp => temp.ToSellOrderResponse()).ToList();

            //print expected list
            _testOutputHelper.WriteLine("Expected: ");
            foreach (SellOrderResponse response in sellOrderResponseExpected)
            {
                _testOutputHelper.WriteLine($"SellOrderResponse: {response}");
            }

            //mocking
            _stocksRepositoryMock.Setup(temp => temp.GetSellOrders())
                .ReturnsAsync(sellOrderRequests);

            List<SellOrderResponse> sellOrderResponseFromGet = await _stocksService.GetSellOrders();

            //print actual list
            _testOutputHelper.WriteLine("Actual: ");
            foreach (SellOrderResponse response in sellOrderResponseFromGet)
            {
                _testOutputHelper.WriteLine($"SellOrderResponse: {response}");
            }

            //Assert
            sellOrderResponseFromGet.Should().BeEquivalentTo(sellOrderResponseExpected);
        }
        #endregion
    }
}