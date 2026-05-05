using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.DTOs;

namespace WebApplication1.services
{
    public class TaskService : ITaskService
    {
        private readonly ApplicationDB _dbContext;

        public TaskService(ApplicationDB dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<TaskReadDto> CreateTaskAsync(TaskCreateDto dto)
        {
            var task = new TaskItem
            {
                Title = dto.Title,
                Description = dto.Description,
                DueDate = dto.DueDate,
                IsDone = false,
                AssignedUserId = dto.AssignedUserId
            };

            if (dto.TagIds.Any())
            {
                var tags = await _dbContext.tags.Where(t => dto.TagIds.Contains(t.Id)).ToListAsync();
                foreach (var tag in tags)
                {
                    task.TaskTags.Add(new TaskTag { TagId = tag.Id, Tag = tag, TaskItem = task });
                }
            }

            await _dbContext.tasks.AddAsync(task);
            await _dbContext.SaveChangesAsync();

            return await GetTaskByIdAsync(task.Id) ?? throw new InvalidOperationException("Task was created but could not be loaded.");
        }

        public async Task<bool> DeleteTaskAsync(int id)
        {
            var existing = await _dbContext.tasks.FindAsync(id);
            if (existing == null)
            {
                return false;
            }

            _dbContext.tasks.Remove(existing);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<TaskReadDto>> GetAllTasksAsync()
        {
            return await _dbContext.tasks
                .AsNoTracking()
                .Include(t => t.AssignedUser)
                .Include(t => t.TaskTags)
                    .ThenInclude(tt => tt.Tag)
                .Select(t => new TaskReadDto
                {
                    Id = t.Id,
                    Title = t.Title,
                    Description = t.Description,
                    DueDate = t.DueDate,
                    IsDone = t.IsDone,
                    AssignedUserId = t.AssignedUserId,
                    AssignedUserName = t.AssignedUser != null ? t.AssignedUser.Name : string.Empty,
                    Tags = t.TaskTags.Select(tt => tt.Tag.Name).ToList()
                })
                .ToListAsync();
        }

        public async Task<TaskReadDto?> GetTaskByIdAsync(int id)
        {
            var task = await _dbContext.tasks
                .AsNoTracking()
                .Include(t => t.AssignedUser)
                .Include(t => t.TaskTags)
                    .ThenInclude(tt => tt.Tag)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (task == null)
            {
                return null;
            }

            return new TaskReadDto
            {
                Id = task.Id,
                Title = task.Title,
                Description = task.Description,
                DueDate = task.DueDate,
                IsDone = task.IsDone,
                AssignedUserId = task.AssignedUserId,
                AssignedUserName = task.AssignedUser?.Name ?? string.Empty,
                Tags = task.TaskTags.Select(tt => tt.Tag.Name).ToList()
            };
        }

        public async Task<bool> SetTaskStatusAsync(int id, bool isDone)
        {
            var existing = await _dbContext.tasks.FindAsync(id);
            if (existing == null)
            {
                return false;
            }

            existing.IsDone = isDone;
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> AssignTaskAsync(int id, int userId)
        {
            var existing = await _dbContext.tasks.FindAsync(id);
            if (existing == null)
            {
                return false;
            }

            var user = await _dbContext.users.FindAsync(userId);
            if (user == null)
            {
                return false;
            }

            existing.AssignedUserId = userId;
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateTaskAsync(int id, TaskUpdateDto dto)
        {
            var existing = await _dbContext.tasks
                .Include(t => t.TaskTags)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (existing == null)
            {
                return false;
            }

            existing.Title = dto.Title;
            existing.Description = dto.Description;
            existing.DueDate = dto.DueDate;
            existing.IsDone = dto.IsDone;
            existing.AssignedUserId = dto.AssignedUserId;

            existing.TaskTags.Clear();
            if (dto.TagIds.Any())
            {
                var tags = await _dbContext.tags.Where(t => dto.TagIds.Contains(t.Id)).ToListAsync();
                foreach (var tag in tags)
                {
                    existing.TaskTags.Add(new TaskTag { TaskItemId = existing.Id, TagId = tag.Id, Tag = tag, TaskItem = existing });
                }
            }

            await _dbContext.SaveChangesAsync();
            return true;
        }
    }
}
