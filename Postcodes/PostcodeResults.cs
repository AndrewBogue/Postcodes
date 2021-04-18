using System.Collections.Generic;

namespace Postcodes
{
    public class PostcodeResults : IPostcodeResult
    {
        public PostcodeResults()
        {
            results = new List<PostcodeResult>();
        }
        public List<PostcodeResult> results { get; set; }
    }
}
