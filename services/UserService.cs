using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.DTOs;

namespace WebApplication1.services
{
    public class UserService : IUserService
    {
        private readonly ApplicationDB _dbContext;

        public UserService(ApplicationDB dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<UserReadDto> CreateUserAsync(UserCreateDto dto)
        {
            var user = new AppUser
            {
                Name = dto.Name,
                Email = dto.Email.Trim(),
                Role = dto.Role,
                PasswordHash = AuthService.HashPassword(dto.Password),
                Profile = new UserProfile
                {
                    Bio = string.Empty,
                    PhoneNumber = string.Empty
                }
            };

            await _dbContext.users.AddAsync(user);
            await _dbContext.SaveChangesAsync();

            return new UserReadDto
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                Role = user.Role,
                Bio = user.Profile.Bio,
                PhoneNumber = user.Profile.PhoneNumber
            };
        }

        public async Task<bool> DeleteUserAsync(int id)
        {
            var existing = await _dbContext.users.FindAsync(id);
            if (existing == null)
            {
                return false;
            }

            _dbContext.users.Remove(existing);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<UserReadDto>> GetAllUsersAsync()
        {
            return await _dbContext.users
                .AsNoTracking()
                .Include(u => u.Profile)
                .Select(u => new UserReadDto
                {
                    Id = u.Id,
                    Name = u.Name,
                    Email = u.Email,
                    Role = u.Role,
                    Bio = u.Profile.Bio,
                    PhoneNumber = u.Profile.PhoneNumber
                })
                .ToListAsync();
        }

        public async Task<UserReadDto?> GetUserByIdAsync(int id)
        {
            var user = await _dbContext.users
                .AsNoTracking()
                .Include(u => u.Profile)
                .FirstOrDefaultAsync(u => u.Id == id);

            if (user == null)
            {
                return null;
            }

            return new UserReadDto
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                Role = user.Role,
                Bio = user.Profile.Bio,
                PhoneNumber = user.Profile.PhoneNumber
            };
        }

        public async Task<AppUser?> FindByEmailAsync(string email)
        {
            return await _dbContext.users
                .AsNoTracking()
                .Include(u => u.Profile)
                .FirstOrDefaultAsync(u => u.Email.ToLower() == email.Trim().ToLower());
        }

        public async Task<bool> UpdateUserAsync(int id, UserUpdateDto dto)
        {
            var existing = await _dbContext.users.Include(u => u.Profile).FirstOrDefaultAsync(u => u.Id == id);
            if (existing == null)
            {
                return false;
            }

            existing.Name = dto.Name;
            existing.Email = dto.Email.Trim();
            existing.Role = dto.Role;
            existing.Profile.Bio = dto.Bio;
            existing.Profile.PhoneNumber = dto.PhoneNumber;

            await _dbContext.SaveChangesAsync();
            return true;
        }
    }
}
