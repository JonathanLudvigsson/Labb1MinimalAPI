using System.Net;

namespace Book_Web.Models
{
    public class APIResponseDTO
    {
        public bool Success { get; set; }
        public Object Result { get; set; }
        public string DisplayMessage { get; set; } = "";
        public List<string> ErrorMessages { get; set; }
    }
}
