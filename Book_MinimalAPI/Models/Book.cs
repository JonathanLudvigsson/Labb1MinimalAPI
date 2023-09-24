using System.ComponentModel.DataAnnotations;

namespace Book_MinimalAPI.Models
{
    public class Book
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Author { get; set; }
        [Required]
        public bool Available { get; set; }
        public DateTime? ReleaseDate { get; set; }
    }
}
