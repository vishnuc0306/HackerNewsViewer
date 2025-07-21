namespace HackerNewsViewer.Models
{
    public class HackernewsStory
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Url { get; set; } // Can be null for "ask" or "job" stories
        public string By { get; set; } // Author
        public long Time { get; set; } // Unix timestamp
        public string Type { get; set; } // e.g., "story", "comment", "job"
    }
    public class StoryDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Link { get; set; } // Renamed for client clarity
                                         // Add other properties if needed for display
    }
}
