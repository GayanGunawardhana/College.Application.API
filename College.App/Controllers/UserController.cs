using AutoMapper;
using College.App.Models;
using College.App.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace College.App.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IMapper _Mapper;
        private readonly IUserService _UserService;
        public UserController(IMapper mapper, IUserService UserService)
        {
            _Mapper = mapper;
            _UserService = UserService;
        }
        [HttpPost]
        [Route("Create")]
        //api/user/create
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> CreateUser([FromBody] UserDTO userDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _UserService.CreateUserAsync(userDTO);
            if (result)
            {
                return Ok(userDTO);
            }
            else
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error creating user");
            }
        }

        [HttpGet]
        [Route("GetAllUser", Name = "GetCurrentUser")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> GetUserAsync()
        {
            var user = await _UserService.GetUseerAsync();
            if (user == null)
            {
                return NotFound("User not found");
            }

            return Ok(user);
        }
        [HttpGet]
        [Route("GetUserById/{id}", Name = "GetUserById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> GetUserByIdAsync(int id)
        {
            var user = await _UserService.GetUserById(id);
            if (user == null)
            {
                return NotFound("User not found");
            }

            return Ok(user);

        }
        [HttpGet]
        [Route("{username}", Name = "GetUserByUsername")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> GetUserByUsernameAsync(string username)
        {
            var user = await _UserService.GetUserByUsername(username);
            if (user == null)
            {
                return NotFound($"User not found - {username}");
            }
            return Ok(user);
        }
        [HttpPut]
        [Route("Update")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> UpdateUser([FromBody] UserDTO userDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _UserService.UpdateUserAsync(userDTO);
            if (result)
            {
                return Ok(result);
            }
            else
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error updating user");
            }
        }
        [HttpDelete]
        [Route("Delete/{id}", Name ="DeleteUserById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> DeleteUser(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (id <= 0)
            {
                return BadRequest($"Invalid User Id- {id}");
            }

            var result = await _UserService.DeleteUserAsync(id);
            if (result)
            {
                return Ok(result);
            }
            else
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error deleting user");
            }
        }

    }
}
