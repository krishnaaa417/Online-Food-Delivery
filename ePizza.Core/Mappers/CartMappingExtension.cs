using ePizza.Domain.Models;
using ePizza.Models.Response;

namespace ePizza.Core.Mappers
{
    public static class CartMappingExtension
    {
        public static CartResponseModel ConvertToCartResponseModel(
               this Cart cartDetails)
        {
            CartResponseModel cartData = new CartResponseModel();  

            cartData.Id = cartDetails.Id;
            cartData.UserId = cartDetails.UserId;
            cartData.CreatedDate = cartDetails.CreatedDate;

            cartData.Items = cartDetails
                                    .CartItems
                                        .Select(
                                              x => new CartItemResponse
                                              {
                                                  Id = x.Id,
                                                  ItemId = x.ItemId,
                                                  Quantity = x.Quantity,
                                                  UnitPrice = x.UnitPrice,
                                                  ImageUrl = x.Item.ImageUrl,
                                                  ItemName = x.Item.Name
                                              })
                                        .ToList();


            cartData.Total = cartData.Items.Sum(x => x.Quantity * x.UnitPrice);
            cartData.Tax = Math.Round(cartData.Total * 0.05m, 2);
            cartData.GrantTotal = cartData.Total + cartData.Tax;

            return cartData;
        }
    }
}
