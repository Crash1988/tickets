using AspCoreServer.Data;
using AspCoreServer.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace AspCoreServer.Controllers
{
  [Route("api/[controller]")]
  public class ReceiptsController : Controller
  {
    private readonly ApplicationDbContext _context;

    public ReceiptsController(ApplicationDbContext context)
    {
      _context = context;
    }
    /*
    [HttpGet]
    public async Task<IActionResult> Get(int currentPageNo = 1, int pageSize = 20)
    {
      var users = await _context.User
          .OrderByDescending(u => u.EntryTime)
          .Skip((currentPageNo - 1) * pageSize)
          .Take(pageSize)
          .ToArrayAsync();

      if (!users.Any())
      {
        return NotFound("Users not Found");
      }
      else
      {
        return Ok(users);
      }
    }*/
    /*

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
      var user = await _context.User
          .Where(u => u.ID == id)
          .AsNoTracking()
          .SingleOrDefaultAsync(m => m.ID == id);

      if (user == null)
      {
        return NotFound("User not Found");
      }
      else
      {
        return Ok(user);
      }
    }
    */
    [HttpPost]
    public async Task<IActionResult> Post([FromBody]Receipt receipt)
    {
     
      if (!string.IsNullOrEmpty(receipt.Name))
      {
        _context.Add(receipt);
        await _context.SaveChangesAsync();
        return CreatedAtAction("Post", receipt);
      }
      else
      {
        return BadRequest("Receipt's name was not given");
      }
    }
    /*
    [HttpPut("{id}")]
    public async Task<IActionResult> Put(int id, [FromBody]User userUpdateValue)
    {
      try
      {
        userUpdateValue.EntryTime = DateTime.Now;

        var userToEdit = await _context.User
          .AsNoTracking()
          .SingleOrDefaultAsync(m => m.ID == id);

        if (userToEdit == null)
        {
          return NotFound("Could not update user as it was not Found");
        }
        else
        {
          _context.Update(userUpdateValue);
          await _context.SaveChangesAsync();
          return Ok("Updated user - " + userUpdateValue.Name);
        }
      }
      catch (DbUpdateException)
      {
        //Log the error (uncomment ex variable name and write a log.)
        ModelState.AddModelError("", "Unable to save changes. " +
            "Try again, and if the problem persists, " +
            "see your system administrator.");
        return NotFound("User not Found");
      }
    }*/
    /*
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
      var userToRemove = await _context.User
          .AsNoTracking()
      .SingleOrDefaultAsync(m => m.ID == id);
      if (userToRemove == null)
      {
        return NotFound("Could not delete user as it was not Found");
      }
      else
      {
        _context.User.Remove(userToRemove);
        await _context.SaveChangesAsync();
        return Ok("Deleted user - " + userToRemove.Name);
      }
    }*/
  }
}
