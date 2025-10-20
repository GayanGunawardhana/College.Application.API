namespace College.App.Models
{
    public static class CollegeRepository
    {
        public static List<Student> Students { get; set; } = new List<Student>()
        {
                new Student(){Id=1, StudentName="John Doe", Email="john@gmail.com", Address="123 Main St, NK"},
                new Student(){Id=2, StudentName="Anne", Email="Anne@gmail.com", Address="88 Main St, SW"},

        };


    }
}
