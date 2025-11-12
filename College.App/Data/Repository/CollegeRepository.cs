using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace College.App.Data.Repository
{
    public class CollegeRepository<T> : ICollegeRepository<T> where T : class
    {
        private readonly CollegeDbContext _context;
        private DbSet<T> _dbSet;

        public CollegeRepository(CollegeDbContext context) 
        {
            _context = context;
            _dbSet = _context.Set<T>();

        }
        public async Task<T> CreateStudentAsync(T GenericClass)
        {
            //await _context.Set<T>().AddAsync(GenericClass);
            _dbSet.Add(GenericClass);
            await _context.SaveChangesAsync();
            return (GenericClass);
        }
        public async Task<bool> DeleteStudentAsync(T GenericClass)
        {
            _dbSet.Remove(GenericClass);
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<List<T>> GetAllStudentsAsync()
        {
            return await _dbSet.AsNoTracking().ToListAsync();
        }   
        public async Task<T> GetStudentByIdAsync(Expression<Func<T,bool>> filter, bool useNoTracking = false)
        {
            if (useNoTracking)
            {
                return await _dbSet.AsNoTracking().Where(filter).FirstOrDefaultAsync();
            }

            return await _dbSet.Where(filter).FirstOrDefaultAsync();
        }
        public async Task<T> GetStudentByNameAsync(Expression<Func<T,bool>> filter)
        {
            //return await _dbSet.FirstOrDefaultAsync(e => EF.Property<string>(e, "StudentName").Contains(name));
            return await _dbSet.Where(filter).FirstOrDefaultAsync();
        }
        public async Task<T> UpdateStudentAsync(T GenericClass)
        {
            _dbSet.Update(GenericClass);
            await _context.SaveChangesAsync();
            return GenericClass;

        }

    }
}
