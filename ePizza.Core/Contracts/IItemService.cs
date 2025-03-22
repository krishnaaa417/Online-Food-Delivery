using ePizza.Models.Response;

namespace ePizza.Core.Contracts
{
    public interface IItemService
    {
        IEnumerable<ItemResponseModel> GetItems();

        IEnumerable<ItemResponseModel> GetItemsUsingAdo();

        IEnumerable<ItemResponseModel> GetItemsUsingProcedure();

    }
}
