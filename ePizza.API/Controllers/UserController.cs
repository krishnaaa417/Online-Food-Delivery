using ePizza.Core.Contracts;
using ePizza.Models.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ePizza.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var userResponse = _userService.GetAllUsers();

            return Ok(userResponse);
        }


        [HttpPost]
        public IActionResult Create([FromBody]CreateUserRequest createUserRequest)
        {
            if (ModelState.IsValid)
            {
                var createUser = _userService.AddUser(createUserRequest);

                return Ok();
            }

            return BadRequest(ModelState.Select(x => x.Key));
        }
    }
}
