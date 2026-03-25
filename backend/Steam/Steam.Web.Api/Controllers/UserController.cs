using Microsoft.AspNetCore.Mvc;
using SteamApplication.Interfaces;
using SteamApplication.Models.Request.Users;
using SteamShared.Constants;

namespace Steam.Web.Api.Controllers
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

        // GET: api/user
        [HttpGet]
        public IActionResult GetAll()
        {
            var users = _userService.GetAll();
            return Ok(users);
        }

        // GET: api/user/1
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var user = _userService.GetById(id);

            if (user == null)
                return NotFound(ValidationConstants.USER_NOT_FOUND);

            return Ok(user);
        }

        // POST: api/user
        [HttpPost]
        public IActionResult Create([FromBody] CreateUsersRequest user)
        {
            if (user == null)
                return BadRequest(ValidationConstants.INVALID_DATA);

            var createdUser = _userService.Create(user);
            return Ok(createdUser);
        }

        // PUT: api/user/1
        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] CreateUsersRequest user)
        {
            var updated = _userService.Update(id, user);

            if (!updated)
                return NotFound(ValidationConstants.USER_NOT_FOUND);

            return Ok("Usuario actualizado");
        }

        // DELETE: api/user/1
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var deleted = _userService.Delete(id);

            if (!deleted)
                return NotFound(ValidationConstants.USER_NOT_FOUND);

            return Ok("Usuario eliminado");
        }

    }
}
