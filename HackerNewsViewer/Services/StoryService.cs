using HackerNewsViewer.Clients;
using HackerNewsViewer.Models;
using Microsoft.Extensions.Caching.Memory;

namespace HackerNewsViewer.Services
{
    public class StoryService : IStoryService
    {
        private readonly IHackerNewsApiClient _hackerNewsApiClient;
        private readonly IMemoryCache _cache;
        private const string NewStoryIdsCacheKey = "NewStoryIds";
        private readonly TimeSpan NewStoryIdsCacheDuration = TimeSpan.FromMinutes(5); // Cache for 5 minutes
        public StoryService(IHackerNewsApiClient hackerNewsApiClient, IMemoryCache cache)
        {
            _hackerNewsApiClient = hackerNewsApiClient;
            _cache = cache;
        }
        public async Task<IEnumerable<StoryDto>> GetNewestStoriesAsync(string? searchTerm, int pageNumber, int pageSize)
        {
            // 1. Get New Story IDs (from cache or API)
            List<int> storyIds;
            if (!_cache.TryGetValue(NewStoryIdsCacheKey, out storyIds))
            {
                storyIds = await _hackerNewsApiClient.GetNewStoryIdsAsync();
                _cache.Set(NewStoryIdsCacheKey, storyIds, NewStoryIdsCacheDuration);
            }

            // 2. Fetch Story Details for the required range
            var startIndex = (pageNumber - 1) * pageSize;
            if (startIndex >= storyIds.Count)
            {
                return Enumerable.Empty<StoryDto>(); // No more stories
            }

            var idsToFetch = storyIds.Skip(startIndex).Take(pageSize).ToList();
            var stories = new List<HackernewsStory>();
            foreach (var id in idsToFetch)
            {
                // You might consider caching individual stories as well, but for simplicity
                // and to ensure fresh data, we'll refetch each time or use a short cache.
                // For a more robust solution, batch item fetching or parallelize these calls.
                var story = await _hackerNewsApiClient.GetStoryDetailsAsync(id);
                if (story != null && story.Type == "story") // Only show actual stories
                {
                    stories.Add(story);
                }
            }

            // 3. Apply Search Filter
            IEnumerable<HackernewsStory> filteredStories = stories;
            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                filteredStories = filteredStories.Where(s =>
                    s.Title != null && s.Title.Contains(searchTerm, StringComparison.OrdinalIgnoreCase));
            }

            // 4. Map to DTO and return
            return filteredStories.Select(s => new StoryDto
            {
                Id = s.Id,
                Title = s.Title,
                Link = s.Url // Null if not present, handled by front-end
            });
        }
    }
}
