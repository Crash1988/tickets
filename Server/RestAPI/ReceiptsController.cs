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
  }
}
