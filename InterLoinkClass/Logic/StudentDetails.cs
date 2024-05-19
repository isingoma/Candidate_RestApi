using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterLoinkClass.Logic
{
    public class StudentDetails
    {
        public string RequestType { get; set; }
        public string CandidateId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string CallTimeInterval { get; set; }
        public string LinkedInUrl { get; set; }
        public string GitHubUrl { get; set; }
        public string Comment { get; set; }
    }
    public class StudentDetailsResponse
    {
        public string CandidateId { get; set; }
        public string fullname { get; set; }    
        public string Email { get; set; }
        public string LinkedInUrl { get; set; }
        public string GitHubUrl { get; set; }
        public string statuscode { get; set; }
        public string statusdescription { get; set; }
    }
}
