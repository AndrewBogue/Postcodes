using Newtonsoft.Json;

namespace Postcodes
{
    public class ErrorResult: IPostcodeResult
    {
        [JsonProperty("status")]
        public int Status { get; set; }
        [JsonProperty("error")]
        public string Error { get; set; }
        [JsonProperty("message")]
        public string Message { get; set; }
    }
}
