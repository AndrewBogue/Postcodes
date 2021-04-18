using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Postcodes.Controllers
{
    [ApiController]
    public class PostcodeController : ControllerBase
    {
        private readonly PostcodeManager postcodeManager;
        public PostcodeController(PostcodeManager postcodeManager)
        {
            this.postcodeManager = postcodeManager;
        }

        [Route("api/[controller]/{code?}")]
        [HttpGet]
        public IPostcodeResult Get(string code)
        {
            if (string.IsNullOrEmpty(code))
            {
                return new ErrorResult
                {
                    Status = 422,
                    Error = "UnprocessableEntity",
                    Message = "The postcode is mandatory"
                };
            }
            var exists = postcodeManager.PostCodesToFind(new List<string> { code });
            if (exists.Count == 1)
            {
                return exists[0];
            }
            var result = CallAPI.RunGetAsync(str: code).GetAwaiter().GetResult();
            if (result == "NotFound")
            {
                return new ErrorResult
                {
                    Status = 404,
                    Error = "NotFound",
                    Message = "You've entered an invalid postcode"
                };
            }
            dynamic jsonResponse = JsonConvert.DeserializeObject(result);
            Coordinate coord = new Coordinate
            {
                Latitude = jsonResponse.result.latitude,
                Longitude = jsonResponse.result.longitude
            };
            var postcode =  new PostcodeResult { 
                PostcodeString = jsonResponse.result.postcode,
                Coordinate = coord
            };
            postcodeManager.AddPostcodes(new List<PostcodeResult> { postcode });
            return postcode;
        }

        [Route("api/[controller]")]
        [HttpPost]
        public IPostcodeResult Post([FromBody] PostcodeInput postcodes)
        {
            if (postcodes.Postcodes.Count == 0)
            {
                return new ErrorResult
                {
                    Status = 422,
                    Error = "UnprocessableEntity",
                    Message = "A list with at least one postcode is mandatory"
                };
            }
            var newPostCodes = new PostcodeResults();
            var exists = postcodeManager.PostCodesToFind(postcodes.Postcodes);
            {
                //all already exist - return them
                if (exists.Count == postcodes.Postcodes.Count)
                {
                    newPostCodes.results = exists;
                    return newPostCodes;
                }
                // We need to retrieve at least some postcodes from the API
                // store the ones we have cached and retrieve the rest
                foreach (var postCode in exists)
                {
                    newPostCodes.results.Add(postCode);
                    postcodes.Postcodes.Remove(postCode.PostcodeString);
                }
            }

            var result = CallAPI.RunPostAsync(postcodes: postcodes).GetAwaiter().GetResult();
            if (result == "NotFound")
            {
                return new ErrorResult
                {
                    Status = 404,
                    Error = "NotFound",
                    Message = "You've entered an invalid postcode"
                };
            }
            dynamic jsonResponse = JsonConvert.DeserializeObject(result);
           
            foreach (var resultVal in jsonResponse.result)
            {
                var postcode = new PostcodeResult();
                if (resultVal.result == null)
                {
                    postcode.PostcodeString = resultVal.query + " invalid";
                }
                else
                {
                    Coordinate coord = new Coordinate
                    {
                        Latitude = resultVal.result.latitude,
                        Longitude = resultVal.result.longitude
                    };
                    postcode.PostcodeString = resultVal.result.postcode;
                    postcode.Coordinate = coord;
                }
                
                newPostCodes.results.Add(postcode);
            }
            postcodeManager.AddPostcodes(newPostCodes.results);
            return newPostCodes;
            
        }

    }
}