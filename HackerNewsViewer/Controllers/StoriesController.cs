using HackerNewsViewer.Models;
using HackerNewsViewer.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HackerNewsViewer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StoriesController : ControllerBase
    {
        private readonly IStoryService _storyService;

        public StoriesController(IStoryService storyService)
        {
            _storyService = storyService;
        }
        [HttpGet("newest")]
        public async Task<ActionResult<IEnumerable<StoryDto>>> GetNewestStories(
       [FromQuery] string? searchTerm,
       [FromQuery] int pageNumber = 1,
       [FromQuery] int pageSize = 10) // Default page size
        {
            if (pageNumber < 1) pageNumber = 1;
            if (pageSize < 1 || pageSize > 50) pageSize = 10; // Limit page size to prevent abuse

            if (searchTerm != null)
            {
                pageSize = 200;
            }

            var stories = await _storyService.GetNewestStoriesAsync(searchTerm, pageNumber, pageSize);
            return Ok(stories);
        }
    }
}
