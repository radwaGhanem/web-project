using WebApplication1.DTOs;

namespace WebApplication1.services
{
    public interface ITaskService
    {
        Task<TaskReadDto> CreateTaskAsync(TaskCreateDto dto);
        Task<IEnumerable<TaskReadDto>> GetAllTasksAsync();
        Task<TaskReadDto?> GetTaskByIdAsync(int id);
        Task<bool> UpdateTaskAsync(int id, TaskUpdateDto dto);
        Task<bool> AssignTaskAsync(int id, int userId);
        Task<bool> DeleteTaskAsync(int id);
        Task<bool> SetTaskStatusAsync(int id, bool isDone);
    }
}
