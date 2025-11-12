using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace College.App.Data
{
    public class Student
    {
        
        public int Id { get; set; }
        public string StudentName { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }

        public DateTime DOB { get; set; }

        //Foreign Key
        //Nullable value type
        public int? DepartmentId { get; set; }
        //Navigation Property
        //Nullable reference type
        public virtual Department? Department { get; set; }

    }
}
