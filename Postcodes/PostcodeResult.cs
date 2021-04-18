using Newtonsoft.Json;

namespace Postcodes
{
    public class PostcodeResult: IPostcodeResult
    {
        [JsonProperty("postcode")]
        public string PostcodeString { get; set; }
        [JsonProperty("coordinate")]
        public Coordinate Coordinate { get; set; }
    }
}
