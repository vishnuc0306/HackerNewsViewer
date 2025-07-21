using HackerNewsViewer.Models;

namespace HackerNewsViewer.Clients
{
    public interface IHackerNewsApiClient
    {
        Task<List<int>> GetNewStoryIdsAsync();
        Task<HackernewsStory> GetStoryDetailsAsync(int id);
    }
}
