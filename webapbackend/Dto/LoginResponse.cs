using System.Runtime.InteropServices.JavaScript;
using Newtonsoft.Json;


namespace webapbackend.Dto
{
    public class LoginResponse
    {
        [JsonProperty("id")]
        public int id { get; set; }

        [JsonProperty("email")]
        public string? UserEmail { get; set; }
       
        [JsonProperty("token")]
        public  string Token { get; set; }

        [JsonProperty("isAdmin")]
        public bool IsAdmin { get; set; }
    }
}
