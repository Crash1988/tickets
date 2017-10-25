using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Asp2017.Server.ViewModels
{
    public class LoginViewModel
    {

      #region Constructor
      public LoginViewModel()
      {

      }
    #endregion Constructor

    #region Properties
    [Required]
    [EmailAddress]
    public string Username { get; set; }

    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; }
    #endregion Properties
  }
}
