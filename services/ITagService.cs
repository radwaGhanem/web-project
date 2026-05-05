using WebApplication1.Data;
using WebApplication1.DTOs;

namespace WebApplication1.services
{
    public interface ITagService
    {
        Task<TagReadDto> CreateTagAsync(TagCreateDto dto);
        Task<IEnumerable<TagReadDto>> GetAllTagsAsync();
        Task<Tag?> FindByNameAsync(string name);
    }
}
