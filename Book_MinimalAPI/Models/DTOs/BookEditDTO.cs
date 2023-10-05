namespace Book_MinimalAPI.Models.DTOs
{
    public class BookEditDTO
    {
        public string Name { get; set; }
        public string Author { get; set; }
        public bool Available { get; set; }
        public DateTime ReleaseDate { get; set; }
    }
}
