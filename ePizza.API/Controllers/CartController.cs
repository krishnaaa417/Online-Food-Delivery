using ePizza.Core.Contracts;
using ePizza.Models.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ePizza.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;

        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }

        [HttpGet]
        [Route("get-item-count")]
        [AllowAnonymous]
        public async Task<IActionResult> GetItemCount(Guid cartId)
        {
            var data = await _cartService.GetItemCountAsync(cartId);
            return Ok(data);
        }


        [HttpGet]
        [Route("get-cart-details")]
        [AllowAnonymous]
        public async Task<IActionResult> GetCartDetailsAsync(Guid cartId)
        {
            var data = await _cartService.GetCartDetailsAsync(cartId);
            return Ok(data);
        }

        [HttpPost]
        [Route("add-item-to-cart")]
        [AllowAnonymous]
        public async Task<IActionResult> AddItemToCart([FromBody] AddToCartRequest addToCartRequest)
        {
            var data = await _cartService.AddItemToCartAsync(addToCartRequest);
            return Ok(data);
        }

        [HttpPut]
        [Route("delete-item")]
        [AllowAnonymous]
        public async Task<IActionResult> DeleteItem(DeleteItemFromCartRequest deleteItemFromCart)
        {
            var data = await _cartService.DeleteItemFromCartAsync(deleteItemFromCart.CartId, deleteItemFromCart.ItemId);
            return Ok(data);
        }

        [HttpPut]
        [Route("update-item")]
        [AllowAnonymous]
        public async Task<IActionResult> UpdateItem(UpdateCartItemRequest updateCartItemRequest)
        {
            var data = await _cartService.UpdateItemInCartAsync(
                     updateCartItemRequest.CartId, updateCartItemRequest.ItemId, updateCartItemRequest.Quantity);

            return Ok(data);
        }

        [HttpPost]
        [Route("place-order")]
        public async Task<IActionResult> PlaceOrder(UpdateCartItemRequest updateCartItemRequest)
        {
            var data = await _cartService.UpdateItemInCartAsync(
                     updateCartItemRequest.CartId, updateCartItemRequest.ItemId, updateCartItemRequest.Quantity);

            return Ok(data);
        }

        [HttpPut]
        [Route("update-cart_user")]
        public async Task<IActionResult> UpdateCartUser(UpdateCartUserRequest updateCartItemRequest)
        {
            var data = await _cartService.UpdateCartUser(updateCartItemRequest.CartId, updateCartItemRequest.UserId);

            return Ok(data);
        }
    }
}
