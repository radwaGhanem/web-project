using WebApplication1.Data;
using WebApplication1.DTOs;

namespace WebApplication1.services
{
    public interface IUserService
    {
        Task<UserReadDto> CreateUserAsync(UserCreateDto dto);
        Task<IEnumerable<UserReadDto>> GetAllUsersAsync();
        Task<UserReadDto?> GetUserByIdAsync(int id);
        Task<bool> UpdateUserAsync(int id, UserUpdateDto dto);
        Task<bool> DeleteUserAsync(int id);
        Task<AppUser?> FindByEmailAsync(string email);
    }
}
