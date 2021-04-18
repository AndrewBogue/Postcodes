using System;
using System.Collections.Generic;
using System.Linq;

namespace Postcodes
{
    /// <summary>
    /// Checks connection on startup and caches postcodes
    /// </summary>
    public class PostcodeManager
    {
        // We can decide what to do with this - shut program down/notify user not connected as appropriate
        private bool isConnected { get; set; }
        List<PostcodeResult> cachedPostcodes;
        public PostcodeManager()
        {
            // Check connection
            isConnected = CheckConnection();
            cachedPostcodes = new List<PostcodeResult>();
        }

        bool CheckConnection()
        {
            try
            {
                // Try with a postcode we know is valid
                var result = CallAPI.RunGetAsync(str: "GU1 2EA").GetAwaiter().GetResult();
                if (result == "NotFound")
                {
                    return false;
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
            
        }

        public void AddPostcodes(List<PostcodeResult> postcodes)
        {
            //if doesn't already contain postcode, add
            var newItems = postcodes.Where(x => !cachedPostcodes.Any(y => x.PostcodeString == y.PostcodeString));
            foreach (var code in newItems)
            {
                if (code.Coordinate != null)
                {
                    cachedPostcodes.Add(code);
                }
            }
        }

        /// <summary>
        /// Given a list of postcodes from the API
        /// Returns a List of postcodes that already exist
        /// </summary>
        /// <param name="postcodes"></param>
        /// <returns></returns>
        public List<PostcodeResult> PostCodesToFind(List<string> postcodes)
        {
            var val = cachedPostcodes.Where(x => postcodes.Any(y => x.PostcodeString == y));
            return val.ToList();
        }
    }
}
