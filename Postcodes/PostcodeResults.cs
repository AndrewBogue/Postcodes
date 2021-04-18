using System.Collections.Generic;

namespace Postcodes
{
    public class PostcodeResults : IPostcodeResult
    {
        public PostcodeResults()
        {
            Results = new List<PostcodeResult>();
        }
        public List<PostcodeResult> Results { get; set; }
    }
}
