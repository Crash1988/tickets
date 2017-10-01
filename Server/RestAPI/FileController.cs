using AspCoreServer.Data;
using AspCoreServer.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AspCoreServer.Controllers
{
  [Route("api/[controller]")]
  public class FileController : Controller
  {
    private readonly SpaDbContext _context;

    public FileController(SpaDbContext context)
    {
      _context = context;
    }
    
    [HttpPost]
    public async Task<IActionResult> Post()
    {
      var files = Request.Form.Files;
     // var files = fi;
      if (files == null)
        return BadRequest();

      /*foreach (var file in files)
      {
        // to do save
      }*/

      //
      var file = files[0];

      if (file == null) throw new Exception("File is null");
      if (file.Length == 0) throw new Exception("File is empty");

      using (Stream stream = file.OpenReadStream())
      {
        using (var binaryReader = new BinaryReader(stream))
        {
          var fileContent = binaryReader.ReadBytes((int)file.Length);
          // file.OpenReadStream();
          
          //await _uploadService.AddFile(fileContent, file.FileName, file.ContentType);//just save the file
        }
      }
      return Ok();

    }
  }
}
