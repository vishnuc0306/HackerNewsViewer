using HackerNewsViewer.Models;

namespace HackerNewsViewer.Services
{
    public interface IStoryService
    {
        Task<IEnumerable<StoryDto>> GetNewestStoriesAsync(string? searchTerm, int pageNumber, int pageSize);
    }
}
