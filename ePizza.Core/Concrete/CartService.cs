using ePizza.Core.Contracts;
using ePizza.Core.CustomExceptions;
using ePizza.Core.Mappers;
using ePizza.Domain.Models;
using ePizza.Models.Request;
using ePizza.Models.Response;
using ePizza.Repository.Contracts;

namespace ePizza.Core.Concrete
{
    public class CartService : ICartService
    {
        private readonly ICartRepository _cartRepository;

        public CartService(ICartRepository cartRepository)
        {
            _cartRepository = cartRepository;
        }

        public async Task<bool> AddItemToCartAsync(AddToCartRequest request)
        {
            var cartDetails = await _cartRepository.GetCartDetailsAsync(request.CartId);

            if (cartDetails == null)
            {
                int itemsAdded = AddNewCart(request);

                return itemsAdded > 0;
            }
            else
            {
                return AddItemsToCart(request, cartDetails);
            }
        }

        public async Task<CartResponseModel> GetCartDetailsAsync(Guid cartId)
        {
            var cartDetails = await _cartRepository.GetCartDetailsAsync(cartId);

            if (cartDetails != null)
            {
                return cartDetails.ConvertToCartResponseModel();
            }
            return new CartResponseModel();
        }

        public async Task<bool> DeleteItemFromCartAsync(Guid cartId, int itemId)
        {
            var isdeleted = await _cartRepository.DeleteItemAsync(cartId, itemId);

            if (!isdeleted)
            {
                throw new RecordNotFoundException($"Item with Item Id {itemId} doesn't exists in Cart with Id {cartId} ");
            }

            return isdeleted;    
        }

        public async Task<int> GetItemCountAsync(Guid cartId)
        {
            return await _cartRepository.GetCartItemsQuantity(cartId);
        }


        #region Private Methods
        private int AddNewCart(AddToCartRequest request)
        {
            Cart? cartDetails = new Cart
            {
                Id = request.CartId,
                UserId = request.UserId,
                CreatedDate = DateTime.UtcNow,
                IsActive = true
            };
            CartItem items = new CartItem
            {
                CartId = request.CartId,
                ItemId = request.ItemId,
                Quantity = request.Quantity,
                UnitPrice = request.UnitPrice,
            };

            cartDetails.CartItems.Add(items);
            _cartRepository.Add(cartDetails);
            return _cartRepository.CommitChanges();

        }

        public async Task<int> UpdateCartUser(Guid cartId, int userId)
        {
            var cartDetails = await _cartRepository.GetSingleItem(x => x.Id == cartId);
            
            if (cartDetails == null)
                throw new Exception("Cart doesn't exists");

            cartDetails.UserId = userId;
            return _cartRepository.CommitChanges();
        }

        private bool AddItemsToCart(
          AddToCartRequest request,
          Cart cartDetails)
        {
            CartItem cartItems = cartDetails.CartItems.Where(x => x.ItemId == request.ItemId).FirstOrDefault()!;

            if (cartItems == null)
            {
                cartItems = new CartItem
                {
                    CartId = request.CartId,
                    ItemId = request.ItemId,
                    Quantity = request.Quantity,
                    UnitPrice = request.UnitPrice,
                };

                cartDetails.CartItems.Add(cartItems);
            }
            else
            {
                cartItems.Quantity += request.Quantity;
            }

            _cartRepository.Update(cartDetails);
            int itemsAdded = _cartRepository.CommitChanges();
            return itemsAdded > 0;
        }

        public async Task<bool> UpdateItemInCartAsync(Guid cartId, int itemId, int quantity)
        {

            var cartExists = await _cartRepository.GetAllAsync(x => x.Id == cartId);

            if (!cartExists.Any())
            {
                throw new RecordNotFoundException($"Cart with Id {cartId} doesn't exists");
            }

           int recordsUpdated = await _cartRepository.UpdateItemQuantity(cartId, itemId, quantity);
           return recordsUpdated > 0;
        }
        #endregion

    }
}
