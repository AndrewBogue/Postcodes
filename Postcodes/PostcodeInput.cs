using Newtonsoft.Json;
using System.Collections.Generic;

namespace Postcodes
{
    public class PostcodeInput
    {
        [JsonProperty("postcodes")]
        public List<string> Postcodes { get; set; }
    }
}
