using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication_attw114.Models
{
    public class UserForLogin
    {
        public int Id { get; set; }
        public string EmpNo { get; set; }
        public string Password { get; set; }
        public bool Remembpass { get; set; }
    }
}
