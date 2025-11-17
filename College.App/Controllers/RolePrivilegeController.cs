using AutoMapper;
using College.App.Data;
using College.App.Data.Repository;
using College.App.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace College.App.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolePrivilegeController : ControllerBase
    {
        private readonly IMapper _Mapper;
        private readonly ICollegeRepository<RolePrivilege> _RolePrivilegeRepository;

        public RolePrivilegeController(IMapper mapper, ICollegeRepository<RolePrivilege> RolePrivilegeRepository)
        {
            _Mapper = mapper;
            _RolePrivilegeRepository = RolePrivilegeRepository;

        }
        [HttpPost]
        [Route("Create")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> CreateRolePrivilege([FromBody] RolePrivilegeDTO rolePrivilegeDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            RolePrivilege rolePrivilege = _Mapper.Map<RolePrivilege>(rolePrivilegeDTO);
            //rolePrivilege.IsDelete = false;
            rolePrivilege.CreatedDate = DateTime.Now;
            rolePrivilege.ModifiedDate = DateTime.Now;


            var createdRolePrivilege = await _RolePrivilegeRepository.CreateStudentAsync(rolePrivilege);
            rolePrivilegeDTO.Id = createdRolePrivilege.Id;


            // return CreatedAtAction("GetRoleById", new { id = roleDTO.Id }, createdRole);
            return Ok(rolePrivilegeDTO);
        }
        [HttpGet]
        [Route("All", Name = "GetAllRolesPrivilege")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> GetAllRolesPrivilegeAsync()
        {
            var rolesPrivilege = await _RolePrivilegeRepository.GetAllStudentsAsync();
            var rolePrivilegeDTOs = _Mapper.Map<List<RolePrivilegeDTO>>(rolesPrivilege);
            return Ok(rolePrivilegeDTOs);
        }
        [HttpGet]
        [Route("{id:int}", Name = "GetRolesPrivilegeById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> GetRolePrivilegeByIdAsync(int id)
        {
            if (id <= 0)
            {
                return BadRequest("Invalid Role Privilege Id");
            }
            var rolePrivilege = await _RolePrivilegeRepository.GetStudentByIdAsync(r => r.Id == id, true);
            if (rolePrivilege == null)
            {
                return NotFound($"The Role Privilege not found with id {id}");
            }
            var rolePrivilegeDTO = _Mapper.Map<RolePrivilegeDTO>(rolePrivilege);
            return Ok(rolePrivilegeDTO);

        }
        [HttpGet]
        [Route("AllRolePrivilegesById", Name = "GetAllRolesPrivilegesByRoleId")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<RolePrivilegeDTO>> GetRolePrivilegesByRoleIdAsync(int Roleid)
        {
            if (Roleid <= 0)
            {
                return BadRequest("Invalid Role Privilege Id");
            }
            var rolePrivileges = await _RolePrivilegeRepository.GetAllByFilterAsync(r => r.RoleId == Roleid, false);
            if (rolePrivileges == null)
            {
                return NotFound($"The Role Privileges not found with id {Roleid}");
            }
            var rolePrivilegeDTO = _Mapper.Map<List<RolePrivilegeDTO>>(rolePrivileges);
            return Ok(rolePrivilegeDTO);

        }
        [HttpPut]
        [Route("Update")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> UpdateRolePrivilege([FromBody] RolePrivilegeDTO rolePrivilegeDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (rolePrivilegeDTO.Id <= 0)
            {
                return BadRequest("Invalid Role Privilege Id");
            }
            var existingRole = await _RolePrivilegeRepository.GetStudentByIdAsync(r => r.Id == rolePrivilegeDTO.Id, true);
            if (existingRole == null)
            {
                return NotFound($"The Role Privilege not found with id {rolePrivilegeDTO.Id}");
            }

            var newRolePrivilege = _Mapper.Map<RolePrivilege>(rolePrivilegeDTO);

            var updatedRolePrivilege = await _RolePrivilegeRepository.UpdateStudentAsync(newRolePrivilege);

            return Ok(updatedRolePrivilege);


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
                return BadRequest("Invalid Role Privilege Id");
            }
            var existingRole = await _RolePrivilegeRepository.GetStudentByIdAsync(r => r.Id == id, true);
            if (existingRole == null)
            {
                return NotFound($"The Role Privilege not found with id {id}");
            }
            var result = await _RolePrivilegeRepository.DeleteStudentAsync(existingRole);
            if (result)
            {
                return Ok($"The Role Privilege with id {id} deleted successfully.");
            }
            else
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error deleting the Role Privilege.");
            }

        }
    }
}
