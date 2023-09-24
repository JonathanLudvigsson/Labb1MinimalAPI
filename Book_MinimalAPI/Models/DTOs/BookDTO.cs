namespace Book_MinimalAPI.Models.DTOs
{
    public class BookDTO
    {
        public string Name { get; set; }
        public string Author { get; set; }
        public bool Available { get; set; }
        public DateTime ReleaseDate { get; set; }
    }
}
