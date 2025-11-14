using AutoMapper;
using College.App.Data;
using College.App.Data.Repository;
using College.App.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;

namespace College.App.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly IMapper _Mapper;
        private readonly ICollegeRepository<Role> _RoleRepository;

        public RoleController(IMapper mapper, ICollegeRepository<Role> RoleRepository)
        {
            _Mapper = mapper;
            _RoleRepository = RoleRepository;

        }
        [HttpPost]
        [Route("Create")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> CreateRole([FromBody] RoleDTO roleDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Role Role = _Mapper.Map<Role>(roleDTO);
            Role.IsDelete = false;
            Role.CreatedDate = DateTime.Now;
            Role.ModifiedDate = DateTime.Now;


            var createdRole = await _RoleRepository.CreateStudentAsync(Role);
            roleDTO.Id = createdRole.Id;


            // return CreatedAtAction("GetRoleById", new { id = roleDTO.Id }, createdRole);
            return Ok(roleDTO);
        }
        [HttpGet]
        [Route("All", Name = "GetAllRoles")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> GetAllRolesAsync()
        {
            var roles = await _RoleRepository.GetAllStudentsAsync();
            var roleDTOs = _Mapper.Map<List<RoleDTO>>(roles);
            return Ok(roleDTOs);
        }
        [HttpGet]
        [Route("{id:int}", Name = "GetRolesById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetRoleByIdAsync(int id)
        {
            if (id <= 0)
            {
                return BadRequest("Invalid Role Id");
            }
            var role = await _RoleRepository.GetStudentByIdAsync(r => r.Id == id, true);
            if (role == null)
            {
                return NotFound($"The Role not found with id {id}");
            }
            var roleDTO = _Mapper.Map<RoleDTO>(role);
            return Ok(roleDTO);

        }
        [HttpPut]
        [Route("Update")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> UpdateRole([FromBody] RoleDTO roleDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (roleDTO.Id <= 0)
            {
                return BadRequest("Invalid Role Id");
            }
            var existingRole = await _RoleRepository.GetStudentByIdAsync(r => r.Id == roleDTO.Id, true);
            if (existingRole == null)
            {
                return NotFound($"The Role not found with id {roleDTO.Id}");
            }

            var newRole = _Mapper.Map<Role>(roleDTO);

            var updatedRole = await _RoleRepository.UpdateStudentAsync(newRole);

            return Ok(updatedRole);


        }
        [HttpDelete]
        [Route("Delete/{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> DeleteRole(int id)
        {
            if (id <= 0)
            {
                return BadRequest("Invalid Role Id");
            }
            var existingRole = await _RoleRepository.GetStudentByIdAsync(r => r.Id == id, true);
            if (existingRole == null)
            {
                return NotFound($"The Role not found with id {id}");
            }
            var result = await _RoleRepository.DeleteStudentAsync(existingRole);
            if (result)
            {
                return Ok($"The Role with id {id} deleted successfully.");
            }
            else
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error deleting the Role.");
            }

        }
    }
}
