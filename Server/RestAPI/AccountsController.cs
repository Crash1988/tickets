using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using AspCoreServer.Models;
using Asp2017.Server.ViewModels;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.Extensions.Configuration;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Asp2017.Server.RestAPI
{
  [Route("api/[controller]")]
  public class AccountsController : Controller
  {
    protected SignInManager<ApplicationUser> _signInManager;
    protected UserManager<ApplicationUser> _userManager;
    protected IConfiguration _config;

    // Login api/accounts/login
    [HttpGet]
    public void LogIn()
    {
    }

    public AccountsController( SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager, IConfiguration config)
    {
      this._signInManager = signInManager;
      this._userManager = userManager;
      this._config = config;

    }
    [AllowAnonymous]
    [Route("~/api/accounts/createtoken")]
    [HttpPost]
    public async Task<IActionResult> CreateToken(LoginViewModel model)
    {

      //sawait CreateUsersAsync();

      if (ModelState.IsValid)
      {
        var user = await _userManager.FindByEmailAsync(model.Username);

        if (user != null)
        {
          var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, false);
          if (result.Succeeded)
          {

            var claims = new[]
            {
            new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Tokens:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(_config["Tokens:Issuer"],
            _config["Tokens:Issuer"],
            claims,
            expires: DateTime.Now.AddMinutes(30),
            signingCredentials: creds);
            
            return Ok(new { access_token = new JwtSecurityTokenHandler().WriteToken(token), expiration = (int)TimeSpan.FromMinutes(10).TotalSeconds });
          }
        }
      }

      return BadRequest("Could not create token");
    }


  }
}
