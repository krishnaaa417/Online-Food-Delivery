using ePizza.UI.Helpers;
using ePizza.UI.Models.ApiRequest;
using ePizza.UI.Models.ApiResponses;
using ePizza.UI.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace ePizza.UI.Controllers
{
    [Route("Cart")]
    public class CartController : BaseController
    {
        private readonly ILogger<CartController> _logger;
        private readonly IHttpClientFactory _httpClientFactory;


        public CartController(
              ILogger<CartController> logger,
              IHttpClientFactory httpClientFactory)
        {
            _logger = logger;
            _httpClientFactory = httpClientFactory;
        }

        Guid CartId
        {
            get
            {
                Guid id;
                string CartId = Request.Cookies["CartId"];
                if (CartId == null)
                {
                    id = Guid.NewGuid();
                    Response.Cookies.Append(
                              "CartId", id.ToString(),
                                     new CookieOptions
                                     {
                                         Expires = DateTime.Now.AddDays(1)
                                     });
                }
                else
                {
                    id = Guid.Parse(CartId);
                }

                return id;
            }
        }

        [HttpGet("Index")]
        public async Task<IActionResult> Index()
        {
            using var httpClient = _httpClientFactory.CreateClient("ePizaaApiClient");

            var cartItems = await httpClient.GetFromJsonAsync<ApiResponseModel<GetCartResponseModel>>(
                $"api/Cart/get-cart-details?cartId={CartId}");

            return View(cartItems.Data);
        }


        [HttpGet("AddToCart/{itemId:int}/{unitPrice:decimal}/{quantity:int}")]
        public async Task<IActionResult> AddToCart(int itemId, decimal unitPrice, int quantity)
        {
            using var httpClient = _httpClientFactory.CreateClient("ePizaaApiClient");

            AddToCartRequest addCartRequest
                 = new AddToCartRequest()
                 {
                     ItemId = itemId,
                     Quantity = quantity,
                     UnitPrice = unitPrice,
                     CartId = CartId
                 };

            var itemAdded = await httpClient.PostAsJsonAsync("api/Cart/add-item-to-cart", addCartRequest);

            var cartItemCount = await CartCount(CartId);
            return Json(new { Count = cartItemCount });

        }


        [HttpPut("UpdateQuantity/{itemId:int}/{quantity:int}")]
        public async Task<IActionResult> UpdateQuantity(int itemId, int quantity)
        {
            using var httpClient = _httpClientFactory.CreateClient("ePizaaApiClient");

            var updateCartItems =
                new
                {
                    CartId = CartId,
                    ItemId = itemId,
                    Quantity = quantity
                };

            var itemAdded = await httpClient.PutAsJsonAsync($"api/Cart/update-item", updateCartItems);

            var cartItemCount = await CartCount(CartId);
            return Json(new { Count = cartItemCount });

        }


        [HttpDelete("DeleteItem/{itemId:int}")]
        public async Task<IActionResult> DeleteItem(int itemId, int quantity)
        {
            using var httpClient = _httpClientFactory.CreateClient("ePizaaApiClient");


            var deleteItem
                 = new
                 {
                     CartId = CartId,
                     itemId = itemId
                 };

            var deleteItemRequest = await httpClient.PutAsJsonAsync($"api/Cart/delete-item",deleteItem);

            deleteItemRequest.EnsureSuccessStatusCode();

            return Json(new { Deleted = true });

        }



        [HttpGet("Checkout")]
        public IActionResult Checkout()
        {
            return View();
        }

        [HttpPost("Checkout")]
        public async Task<IActionResult> Checkout(AddressViewModel addressViewModel)
        {
            if (ModelState.IsValid && CurrentUser is not null)
            {
                using var httpClient = _httpClientFactory.CreateClient("ePizaaApiClient");

                var cart = await httpClient.GetFromJsonAsync<ApiResponseModel<GetCartResponseModel>>(
                                                     $"api/Cart/get-cart-details?cartId={CartId}");

                if (cart.Success)
                {
                    var updateUserRequest
                        = new
                        {
                            CartId,
                            CurrentUser.UserId
                        };

                    var response = await httpClient.PutAsJsonAsync($"/api/Cart/update-cart_user", updateUserRequest);
                    response.EnsureSuccessStatusCode();

                    TempData.Set("Address", addressViewModel);
                    TempData.Set("CartDetails", cart.Data);
                }
                return RedirectToAction("Index", "Payment");
            }

            return View();
        }

        [HttpGet("GetCartCount")]
        public async Task<JsonResult> GetCartCount()
        {
            var cartItemCount = await CartCount(CartId);
            return Json(new { Count = cartItemCount });
        }

        [NonAction]
        private async Task<int> CartCount(Guid cartId)
        {
            using var httpClient = _httpClientFactory.CreateClient("ePizaaApiClient");

            var cartQuantityRequest
                = await httpClient.GetFromJsonAsync<ApiResponseModel<int>>($"/api/Cart/get-item-count?cartId={CartId}");

            return cartQuantityRequest.Data;

        }

    }
}
