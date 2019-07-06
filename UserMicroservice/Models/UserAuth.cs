using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UserMicroservice.Models
{
    public class UserAuth : User
    {
        public bool Authenticated { get; set; }
        public string Error { get; set; }
    }
}
