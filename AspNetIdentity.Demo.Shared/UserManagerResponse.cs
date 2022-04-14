using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AspNetIdentity.Demo.Shared
{
    public class UserManagerResponse
    {
        public string Message { get; set; }
        public bool  IsSuccess { get; set; }
        public IEnumerable<string>  Errors { get; set; }
        public DateTime? ExpireData {get; set; } 
    }
}
