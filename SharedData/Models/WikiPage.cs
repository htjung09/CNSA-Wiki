namespace CNSAWiki.Models
{
    public class WikiPage
    {
        public long PageId { get; set; }
        public string Title { get; set; } = null!;
        public string Content { get; set; } = null!;

        public long? AuthorId { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}