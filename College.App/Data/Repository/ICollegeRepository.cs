using System.Linq.Expressions;

namespace College.App.Data.Repository
{
    public interface ICollegeRepository<T> where T : class
    {
        Task<List<T>> GetAllStudentsAsync();
        Task<T> GetStudentByIdAsync(Expression<Func<T, bool>> filter, bool useNoTracking = false);
        Task<T> GetStudentByNameAsync(Expression<Func<T, bool>> filter);
        Task<T> CreateStudentAsync(T GenericClass);
        Task<T> UpdateStudentAsync(T GenericClass);
        Task<bool> DeleteStudentAsync(T GenericClass);

    }
}
