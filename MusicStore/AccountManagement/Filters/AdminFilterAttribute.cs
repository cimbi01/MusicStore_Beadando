using MusicStore.Models;
using Mvc3ToolsUpdateWeb_Default.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MusicStore.AccountManagement.Filters
{
    public class AdminFilterAttribute : AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            Account user = (Account)httpContext.Session[AccountController.LOGGEDINUSER_SESSION_KEY];
            // ha nincs bejelentkezett user
            if (user == null)
            {
                return false;
            }
            // ha van és  admin
            else if (user.IsAdmin)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}