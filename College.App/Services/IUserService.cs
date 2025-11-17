using College.App.Models;

namespace College.App.Services
{
    public interface IUserService
    {
        (string PasswordHash, string Salt) CreatePasswordHash(string password);

        Task<bool> CreateUserAsync(UserDTO dto);

        Task<UserReadOnlyDTO> GetUseerAsync();

        Task<UserReadOnlyDTO> GetUserById(int Id);

        Task<UserReadOnlyDTO> GetUserByUsername(string Username);

        Task<bool> UpdateUserAsync(UserDTO DTO);

        Task<bool> DeleteUserAsync(int Id);


    }
}
