using MusicStore.Models;
using Mvc3ToolsUpdateWeb_Default.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Filters;

namespace MusicStore.AccountManagement.Filters
{
    public class LoggedinFilterAttribute : AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            Account user = (Account)httpContext.Session[AccountController.LOGGEDINUSER_SESSION_KEY];
            // ha nincs bejelentkezett user
            if (user == null)
            {
                return false;
            }
            // ha van
            return true;
        }
    }
}