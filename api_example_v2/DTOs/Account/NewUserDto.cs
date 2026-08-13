using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api_example.DTOs.Account
{
    public class NewUserDto
    {
        public string UserName { get; set; } = String.Empty;

        public string Email { get; set; } = String.Empty;

        public string Token { get; set; }   = String.Empty;
    }
}