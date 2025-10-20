using College.App.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace College.App.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        [HttpGet]
        [Route("All", Name = "GetAllStudents")]
        public ActionResult<IEnumerable<StudentDTO>> GetStudents()
        {
            //This is the endpoint to get all students

            var students = CollegeRepository.Students.Select(item => new StudentDTO()
            {
                Id = item.Id,
                StudentName = item.StudentName,
                Email = item.Email,
                Address = item.Address
            }).ToList();


            return Ok(CollegeRepository.Students);

            //https://localhost:44362/api/Students/All

        }
        [HttpGet]
        [Route("{id:int}", Name = "GetStudentById")]
        //Documentation for the possible response types
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        public ActionResult<Student> GetStudentById(int id)
        {
            if (id <= 0)
            {
                // Bad request status code 400 - client error
                return BadRequest("Invalid student id");
            }
            if (CollegeRepository.Students == null || CollegeRepository.Students.Count == 0)
            {
                return NotFound("No students found");
            }

            return Ok(CollegeRepository.Students.Where(n => n.Id == id).FirstOrDefault());
            //https://localhost:44362/api/Students/2

        }

        [HttpGet]
        [Route("{name:alpha}", Name = "GetStudentByName")]
        //Documentation for the possible response types
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<Student> GetStudentByName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return BadRequest("Invalid student name");
            }
            if (CollegeRepository.Students == null || CollegeRepository.Students.Count == 0)
            {
                return NotFound("No students found");
            }

            return Ok(CollegeRepository.Students.Where(n => n.StudentName == name).FirstOrDefault());
            //https://localhost:44362/api/Students/Anne

        }

        [HttpDelete("Delete/{id:int}", Name = "DeleteStudentById")]
        //api/students/Delete/1
        //Documentation for the possible response types
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<bool> DeleteStudentById(int id)
        {
            if (id <= 0)
            {
                // Bad request status code 400 - client error
                return BadRequest("Invalid student id");
            }

            var student = CollegeRepository.Students.Where(n => n.Id == id).FirstOrDefault();
            if (student == null)
            {
                //If you need to add {id} parameter value in the response message then use $"" string interpolation

                return NotFound($"Student with id {id} not found");
            }
            CollegeRepository.Students.Remove(student);
            return Ok(true);
            //https://localhost:44362/api/Students/1
        }
        [HttpPost]
        [Route("Create")]
        //Documentation for the possible response types
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<StudentDTO> CreateStudent(StudentDTO model)
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
            //Generate new id
            var maxId = CollegeRepository.Students.Max(s => s.Id);
            model.Id = maxId + 1;
            // Convert DTO -> Entity
            var student = new Student()
            {
                Id = model.Id,
                StudentName = model.StudentName,
                Email = model.Email,
                Address = model.Address
            };
            CollegeRepository.Students.Add(student);
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
        public ActionResult<StudentDTO> UpdateStudent(StudentDTO model)
        {
            if (model.Id <= 0 || model == null)
            {
                return BadRequest("Invalid student data");
            }
            var student = CollegeRepository.Students.Where(n => n.Id == model.Id).FirstOrDefault();
            if (student == null)
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
            //Update the student entity
            student.StudentName = model.StudentName;
            student.Email = model.Email;
            student.Address = model.Address;
            return Ok(model);
        }
        //Before work with PATCH method, we need to install the NuGet package Microsoft.AspNetCore.Mvc.NewtonsoftJson and Microsoft.AspNetCore.JsonPatch
        [HttpPatch]
        [Route("{id:int}/UpdatePartial")]
        //api/students/1/UpdatePartial
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult UpdateStudentPartial(int id, [FromBody] JsonPatchDocument<StudentDTO> patchDocument)
        {
            if (id <= 0 || patchDocument == null)
            {
                return BadRequest("Invalid student data");
            }
            var Existingstudent = CollegeRepository.Students.Where(n => n.Id == id).FirstOrDefault();
            if (Existingstudent == null)
            {
                return NotFound($"Student with id {id} not found");
            }
            // This is created for update JSON patch document
            var studentDTO = new StudentDTO()
            {
                Id = Existingstudent.Id,
                StudentName = Existingstudent.StudentName,
                Email = Existingstudent.Email,
                Address = Existingstudent.Address
            };
            //Apply the patch document to the studentDTO and if there are any errors then add to the ModelState
            patchDocument.ApplyTo(studentDTO, ModelState);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }


            //Update the student entity
            Existingstudent.StudentName = studentDTO.StudentName;
            Existingstudent.Email = studentDTO.Email;
            Existingstudent.Address = studentDTO.Address;
            return NoContent();
        }
      
        
    }
}
