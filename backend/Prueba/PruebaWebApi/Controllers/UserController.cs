using Microsoft.AspNetCore.Mvc;
using Prueba.Application.Interfaces.Servicie;
using Prueba.Application.Models.Request.Users;

namespace PruebaWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUsuarioService userService;

        public UserController(IUsuarioService userService)
        {
            this.userService = userService;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateUsersRequest model)
        {
            var srv = await userService.Create(model);
            return Ok(srv);
        }

        [HttpGet]
        public IActionResult GetAll([FromQuery] FilterUserRequest model)
        {
            var srv = userService.Get(model);
            return Ok(srv);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var srv = await userService.Get(id);
            return Ok(srv);
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update([FromBody] UpdateUserRequest model, Guid id)
        {
            var srv = await userService.Update(id, model);
            return Ok(srv);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var srv = await userService.Delete(id);
            return Ok(srv);
        }
    }
}
