using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using AspCoreServer.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using AspCoreServer.Models;
using Microsoft.Extensions.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Asp2017.Server.ViewModels;

namespace Asp2017.Server.Models
{
  

  [Route("api/[controller]")]
  public class TokenController : Controller
  {

    protected SignInManager<ApplicationUser> _signInManager;
    protected UserManager<ApplicationUser> _userManager;
    protected RoleManager<IdentityRole> _roleManager;
    private readonly ApplicationDbContext _context;
    protected IConfiguration _config;

    //constructor
    public TokenController(ApplicationDbContext context, SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager,  IConfiguration config)
    {
      this._context = context;
      this._signInManager = signInManager;
      this._userManager = userManager;
      this._roleManager = roleManager;
      this._config = config;
    }

    [AllowAnonymous]
    [HttpPost]
    public async Task<IActionResult> Post( LoginViewModel model)
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

            return Ok(new { token = new JwtSecurityTokenHandler().WriteToken(token), username = user.UserName });
          }
        }
      }

      return BadRequest("Could not create token");
    }


    private async Task CreateUsersAsync()
    {
      // local variables
      DateTime createdDate = new DateTime(2016, 03, 01, 12, 30, 00);
      DateTime lastModifiedDate = DateTime.Now;
      string role_Administrators = "Administrators";
      string role_Registered = "Registered";

      //Create Roles (if they doesn't exist yet)
      if (!await _roleManager.RoleExistsAsync(role_Administrators)) await _roleManager.CreateAsync(new IdentityRole(role_Administrators));
      if (!await _roleManager.RoleExistsAsync(role_Registered)) await _roleManager.CreateAsync(new IdentityRole(role_Registered));

      // Create the "Admin" ApplicationUser account (if it doesn't exist already)
      var user_Admin = new ApplicationUser()
      {
        UserName = "Admin",
        Email = "admin@admin.com",
      };

      // Insert "Admin" into the Database and also assign the "Administrator" role to him.
      if (await _userManager.FindByIdAsync(user_Admin.Id) == null)
      {
        await _userManager.CreateAsync(user_Admin, "Pass4Admin");
        await _userManager.AddToRoleAsync(user_Admin, role_Administrators);
        // Remove Lockout and E-Mail confirmation.
        user_Admin.EmailConfirmed = true;
        user_Admin.LockoutEnabled = false;
      }
      
      await _context.SaveChangesAsync();
    }



  }


}
