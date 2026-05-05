using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.DTOs;

namespace WebApplication1.services
{
    public class TagService : ITagService
    {
        private readonly ApplicationDB _dbContext;

        public TagService(ApplicationDB dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<TagReadDto> CreateTagAsync(TagCreateDto dto)
        {
            var tag = new Tag
            {
                Name = dto.Name.Trim()
            };

            await _dbContext.tags.AddAsync(tag);
            await _dbContext.SaveChangesAsync();

            return new TagReadDto
            {
                Id = tag.Id,
                Name = tag.Name
            };
        }

        public async Task<Tag?> FindByNameAsync(string name)
        {
            return await _dbContext.tags
                .AsNoTracking()
                .FirstOrDefaultAsync(t => t.Name.ToLower() == name.Trim().ToLower());
        }

        public async Task<IEnumerable<TagReadDto>> GetAllTagsAsync()
        {
            return await _dbContext.tags
                .AsNoTracking()
                .Select(t => new TagReadDto
                {
                    Id = t.Id,
                    Name = t.Name
                })
                .ToListAsync();
        }
    }
}
