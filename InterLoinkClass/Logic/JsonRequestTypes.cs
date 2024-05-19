using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterLoinkClass.Logic
{
    public class JsonRequestTypes
    {
        public string CreateCanditate = "{\r\n  \"RequestType\": \"CreateCandidate\",\r\n  \"CandidateId\": \"12345\",\r\n  \"FirstName\": \"John\",\r\n  \"LastName\": \"Doe\",\r\n  \"PhoneNumber\": \"123-456-7890\",\r\n  \"Email\": \"johndoe@example.com\",\r\n  \"CallTimeInterval\": \"10:00 AM - 12:00 PM\",\r\n  \"LinkedInUrl\": \"https://www.linkedin.com/in/johndoe\",\r\n  \"GitHubUrl\": \"https://github.com/johndoe\",\r\n  \"Comment\": \"Lorem ipsum dolor sit amet.\"\r\n}";

        public string QueryCandidate = "{\r\n  \"RequestType\": \"QueryCandidate\",\r\n  \"Email\": \"johndoe@example.com\"\r\n}";

        public string UpdateCandidate = "{\r\n  \"RequestType\": \"UpdateCandidate\",\r\n  \"CandidateId\": \"12345\",\r\n  \"CallTimeInterval\": \"10:00 AM - 12:00 PM\",\r\n  \"LinkedInUrl\": \"https://www.linkedin.com/in/newlinkedinurl\",\r\n  \"GitHubUrl\": \"https://github.com/newgithuburl\",\r\n  \"Email\": \"johndoe@example.com\"\r\n}";
    }
}
