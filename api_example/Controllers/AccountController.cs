using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api_example.DTOs.Account;
using api_example.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace api_example.Controllers
{
    [Route("api_example/account")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        public AccountController(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }
        [HttpPost("register")]
        #pragma warning disable CS8604 // Possible null reference argument.

        public async Task<IActionResult>  Register([FromBody] RegisterDto registerDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var appUser = new AppUser
                {
                    UserName = registerDto.Username,
                    Email = registerDto.Email
                };
                var createdUser = await _userManager.CreateAsync(appUser,registerDto.Password);

                if (createdUser.Succeeded)
                {
                    var roleResult = await _userManager.AddToRoleAsync(appUser,"User");
                    if (roleResult.Succeeded)
                    {
                        return Ok("User Created");
                    }
                    else
                    {
                        return StatusCode(500,createdUser.Errors);
                    }
                }
                else
                {
                         return StatusCode(500,createdUser.Errors);

                }

            }catch(Exception e)
            {
                return StatusCode(500,e);
            }
            #pragma warning restore CS8604 // Possible null reference argument.

        }
    }
}