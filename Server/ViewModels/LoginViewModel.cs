using System;
using System.Collections.Generic;
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
      public string UserName { get; set; }
      public string Password { get; set; }
      public string PasswordNew { get; set; }
      public string Email { get; set; }
      public string DisplayName { get; set; }
      #endregion Properties
  }
}
