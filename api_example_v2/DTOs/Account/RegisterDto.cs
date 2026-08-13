using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace api_example.DTOs.Account
{
    public class RegisterDto
    {
    [Required]
    public string? Username { get; set; }

    [Required]
    [EmailAddress]
    public string? Email { get; set; }
    [Required]
    [MinLength(8,ErrorMessage ="Password must be longer than 8 characters")]
    public string? Password { get; set; }
    }
}