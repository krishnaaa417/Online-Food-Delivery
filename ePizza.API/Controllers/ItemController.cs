using ePizza.Core.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ePizza.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class ItemController : ControllerBase
    {
        private readonly IItemService _itemService;

        public ItemController(IItemService itemService)
        {
            _itemService = itemService;
        }


        [HttpGet]
        public IActionResult Get()
        {
            var items = _itemService.GetItems();
           // var items = _itemService.GetItemsUsingProcedure();

            return Ok(items);
        }
    }
}
