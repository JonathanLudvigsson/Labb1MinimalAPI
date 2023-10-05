using System.Security.AccessControl;
using static Book_Web.StaticDetails;

namespace Book_Web.Models
{
    public class APIRequest
    {
        public ApiType ApiType { get; set; }

        public string Url { get; set; }

        public object Data { get; set; }

        public string AccessToken { get; set; }
    }
}
