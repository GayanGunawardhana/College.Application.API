using AutoMapper;
using College.App.Data;
using College.App.Data.Repository;
using College.App.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Threading.Tasks;

namespace College.App.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[EnableCors(PolicyName= "AllowAll")]
    [Authorize(Roles ="Superadmin,Admin")]
    public class StudentsController : ControllerBase
    {
       
        private readonly IMapper _Mapper;   
        private readonly IStudentRepository _studentRepository;
        //Using common repository for generic operations (Step01)
        //private readonly ICollegeRepository<Student> _studentRepository;

        public StudentsController(CollegeDbContext context,IMapper mapper, IStudentRepository studentRepository)
        {
            //Using common repository for generic operations. Changing parameter type in constructor (Step02)
            //ICollegeRepository<Student> studentRepository  


            //Dependency Injection
            _Mapper = mapper;
            _studentRepository=studentRepository;
        }

        [HttpGet]
        [Route("All", Name = "GetAllStudents")]
        //Documentation for the possible response types
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status406NotAcceptable)]

        //This is to enable CORS for a specific endpoint
        //[EnableCors(PolicyName = "AllowOnlyLocalHost")]
        //This is to disable CORS for a specific endpoint
        //[DisableCors]
        public async Task<ActionResult<IEnumerable<StudentDTO>>> GetStudents()
        {
            
            //This is the endpoint to get all students
            var students = await _studentRepository.GetAllAsync();
            
            var studentDTOdata = _Mapper.Map<List<StudentDTO>>(students);
            


            return Ok(studentDTOdata);

            //https://localhost:44362/api/Students/All

        }
        [HttpGet]
        [Route("{id:int}", Name = "GetStudentById")]
        //Documentation for the possible response types
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        public async Task<ActionResult<StudentDTO>> GetStudentById(int id)
        {
            if (id <= 0)
            {
                // Bad request status code 400 - client error
                return BadRequest("Invalid student id");
            }

            //Using common repository for generic operations. Changing parameter id (Step03)
            //var student = await _studentRepository.GetByIdAsync(n => n.Id == id,false);

            var student = await _studentRepository.GetByIdAsync(id,false);

            if (student == null)
            {
                //If you need to add {id} parameter value in the response message then use $"" string interpolation
                return NotFound($"Student with id {id} not found");
            }

            var studentDTO = _Mapper.Map<StudentDTO>(student);


            return Ok(studentDTO);
            //https://localhost:44362/api/Students/2

        }

        [HttpGet]
        [Route("{name:alpha}", Name = "GetStudentByName")]
        //Documentation for the possible response types
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<StudentDTO>> GetStudentByName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return BadRequest("Invalid student name");
            }

            //Using common repository for generic operations. Changing parameter id (Step03)
            //var student = await _studentRepository.GetByIdAsync(n => n.StudentName.Contains(name),false);

            var student = await _studentRepository.GetByNameAsync(name);
            if (student == null)
            {
                //If you need to add {name} parameter value in the response message then use $"" string interpolation
                return NotFound($"Student with name {name} not found");
            }
            var studentDTO = _Mapper.Map<StudentDTO>(student);


            return Ok(studentDTO);
            //https://localhost:44362/api/Students/Anne

        }

        [HttpDelete("Delete/{id:int}", Name = "DeleteStudentById")]
        //api/students/Delete/1
        //Documentation for the possible response types
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<bool>> DeleteStudentById(int id)
        {
            if (id <= 0)
            {
                // Bad request status code 400 - client error
                return BadRequest("Invalid student id");
            }

            //Using common repository for generic operations. Changing parameter id (Step03)
            //var student = await _studentRepository.GetByIdAsync(n => n.Id == id,false,false);

            var student =await _studentRepository.GetByIdAsync(id,false);
            if (student == null)
            {
                //If you need to add {id} parameter value in the response message then use $"" string interpolation

                return NotFound($"Student with id {id} not found");
            }
            
            
            return Ok(await _studentRepository.DeleteAsync(student));
            //https://localhost:44362/api/Students/1
        }
        [HttpPost]
        [Route("Create")]
        //Documentation for the possible response types
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<StudentDTO>> CreateStudent([FromBody] StudentDTO model)
        {
            if (model == null)
            {
                return BadRequest("Student data is null");
            }
            if (string.IsNullOrWhiteSpace(model.StudentName))
            {
                ModelState.AddModelError("StudentName", "Student name is required");
            }
            if (!new EmailAddressAttribute().IsValid(model.Email))
            {
                ModelState.AddModelError("Email", "Invalid email format");
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
           
            // Convert DTO -> Entity
            
            Student student = _Mapper.Map<Student>(model);
            //Add the new student record to the database
            int id=await _studentRepository.CreateAsync(student);

            //Assign the generated id to the model
            //
            model.Id = id;


            //Return the newly created student record
            // Status - 201 Created
            return CreatedAtRoute("GetStudentById", new { id = model.Id }, model);
            //https://localhost:44362/api/Students/3
        }
        [HttpPut]
        [Route("UpdateStudent")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult> UpdateStudent([FromBody] StudentDTO model)
        {
            if (model.Id <= 0 || model == null)
            {
                return BadRequest("Invalid student data");
            }
            //Reading the data from the EF
            //AsNoTracking() is used to improve performance when you are only reading data and not updating it
            //It tells EF Core not to track changes for the retrieved entities
            //This is useful in scenarios where you want to read data without the overhead of change tracking
            //In this case, we are reading the student entity to check if it exists before updating it
            //If we don't use AsNoTracking(), EF Core will track the entity, which is unnecessary for this read-only operation

            //context.Students.AsNoTracking().Where(n => n.Id == model.Id).FirstOrDefaultAsync()
            var Existingstudent = await _studentRepository.GetByIdAsync(model.Id,true);

            if (Existingstudent == null)
            {
                return NotFound($"Student with id {model.Id} not found");
            }
            if (string.IsNullOrWhiteSpace(model.StudentName))
            {
                ModelState.AddModelError("StudentName", "Student name is required");
            }
            if (!new EmailAddressAttribute().IsValid(model.Email))
            {
                ModelState.AddModelError("Email", "Invalid email format");
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //Assign the updated values to the student entity
            var newRecord = _Mapper.Map<Student>(model);

            await _studentRepository.UpdateAsync(newRecord);

            return NoContent();
        }
        //Before work with PATCH method, we need to install the NuGet package Microsoft.AspNetCore.Mvc.NewtonsoftJson and Microsoft.AspNetCore.JsonPatch
        [HttpPatch]
        [Route("{id:int}/UpdatePartial")]
        //api/students/1/UpdatePartial
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult> UpdateStudentPartial(int id, [FromBody] JsonPatchDocument<StudentDTO> patchDocument)
        {
            if (id <= 0 || patchDocument == null)
            {
                return BadRequest("Invalid student data");
            }
            var Existingstudent = await _studentRepository.GetByIdAsync(id, true);
            if (Existingstudent == null)
            {
                return NotFound($"Student with id {id} not found");
            }
            // This is created for update JSON patch document
            var studentDTO = _Mapper.Map<StudentDTO>(Existingstudent);


            //Apply the patch document to the studentDTO and if there are any errors then add to the ModelState
            patchDocument.ApplyTo(studentDTO, ModelState);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }


            //Update the student entity
           
            Existingstudent = _Mapper.Map<Student>(studentDTO);


            await _studentRepository.UpdateAsync(Existingstudent);

            return NoContent();
        }
      
        
    }
}
