using ePizza.API.Controllers;
using ePizza.Core.Contracts;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace ePizzaAPI.UnitTest
{
    public class CartControllerTest
    {
        private readonly Mock<ICartService> _mockCartService;

        private readonly CartController _controller;

        public CartControllerTest()
        {
            _mockCartService = new Mock<ICartService>();
            _controller = new CartController(_mockCartService.Object);
        }


        [Fact]
        public void Get_ReturnOkResult_When_GetItemCountInvoked()
        {
            Guid cartId = Guid.NewGuid();

            int itemCount = 1;

            // arrange
            _mockCartService.Setup(service => service.GetItemCountAsync(cartId)).ReturnsAsync(itemCount);

            //act
            var result = _controller.GetItemCount(cartId).Result;

            // assert
            var objectResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, objectResult.StatusCode);
        }
    }
}