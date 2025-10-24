
using Microsoft.EntityFrameworkCore;

namespace College.App.Data.Repository
{
    public class StudentRepository : IStudentRepository
    {
        private readonly CollegeDbContext _context;
        public StudentRepository(CollegeDbContext context)
        {
            _context = context;
        }
        public async Task<int> CreateAsync(Student student)
        {
            await _context.Students.AddAsync(student);
            await _context.SaveChangesAsync();
            return student.Id;
        }

        public async Task<bool> DeleteAsync(Student student)
        {
           
            _context.Students.Remove(student);
            await _context.SaveChangesAsync();
            return true;

        }

        public async Task<List<Student>> GetAllAsync()
        {
            return await _context.Students.AsNoTracking().ToListAsync();
        }

        public async Task<Student> GetByIdAsync(int id, bool useNoTracking=false)
        {
            if (useNoTracking)
            {
                return await _context.Students.AsNoTracking().Where(n => n.Id == id).FirstOrDefaultAsync();
            }

            return await _context.Students.Where(n => n.Id == id).FirstOrDefaultAsync();
        }

        public async Task<Student> GetByNameAsync(string name)
        {
           return await _context.Students.Where(n => n.StudentName.Contains(name)).FirstOrDefaultAsync();
        }

        public async Task<int> UpdateAsync(Student student)
        {
            _context.Students.Update(student);
            await _context.SaveChangesAsync();
            return student.Id;

        }
    }
}
