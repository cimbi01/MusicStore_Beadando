using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using Mvc3ToolsUpdateWeb_Default.Models;
using MusicStore.Models;
using MusicStore.AccountManagement;

namespace Mvc3ToolsUpdateWeb_Default.Controllers
{
    public class AccountController : Controller
    {

        //
        // GET: /Account/LogOn

        public ActionResult LogOn()
        {
            return View();
        }

        //
        // POST: /Account/LogOn

        [HttpPost]
        public ActionResult LogOn(LogOnModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                //ASP Web管理器暂时无法使用 所以进行硬编码
                //角色也无法使用 ....
                //账号：Admin
                //密码：admin
                #region 暂时无法使用

                #endregion

                if (AccountManager.ValidateAccount(model.UserName, model.Password))
                {
                    Session["LoggedInUser"] = AccountManager.GetAccount(model);
                    MigrateShoppingCart(model.UserName);
                    FormsAuthentication.SetAuthCookie(model.UserName, model.RememberMe);
                    if (Url.IsLocalUrl(returnUrl) && returnUrl.Length > 1 && returnUrl.StartsWith("/")
                        && !returnUrl.StartsWith("//") && !returnUrl.StartsWith("/\\"))
                    {
                        return Redirect(returnUrl);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "The user name or password provided is incorrect.");
                }
            }
            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/LogOff

        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();
            Session["LoggedInUser"] = null;
            return RedirectToAction("Index", "Home");
        }

        //
        // GET: /Account/Register

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                AccountCreateStatus createStatus = AccountManager.CreateUser(model);
                if (createStatus == AccountCreateStatus.Success)
                {
                    MigrateShoppingCart(model.UserName);
                    //belépés
                    FormsAuthentication.SetAuthCookie(model.UserName, false /* createPersistentCookie */);
                    Session["LoggedInUser"] = AccountManager.GetAccount(model);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", ErrorCodeToString(createStatus));
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/ChangePassword

        [Authorize]
        public ActionResult ChangePassword()
        {
            return View();
        }

        //
        // POST: /Account/ChangePassword

        [Authorize]
        [HttpPost]
        public ActionResult ChangePassword(ChangePasswordModel model)
        {
            if (ModelState.IsValid)
            {
                Account currentUser = ((Account)Session["LoggedInUser"]);
                bool changePasswordSucceeded = AccountManager.ChangePassword(model, currentUser);
                if (changePasswordSucceeded)
                {
                    return RedirectToAction("ChangePasswordSuccess");
                }
                else
                {
                    ModelState.AddModelError("", "The current password is incorrect or the new password is invalid.");
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/ChangePasswordSuccess

        public ActionResult ChangePasswordSuccess()
        {
            return View();
        }
        /// <summary>
        /// 迁移购物车
        /// </summary>
        /// <param name="UserName"></param>
        private void MigrateShoppingCart(string UserName)
        {
            // Associate shopping cart items with logged-in user
            //visszaad egy üres shoppingcartot
            // és beállítja a felhasznalo id-javal a cartid-t
            var cart = ShoppingCart.GetCart(this.HttpContext);
            cart.MigrateCart(UserName);
            Session[ShoppingCart.CartSessionKey] = UserName;
        }
        #region Status Codes
        private static string ErrorCodeToString(AccountCreateStatus createStatus)
        {
            // See http://go.microsoft.com/fwlink/?LinkID=177550 for
            // a full list of status codes.
            switch (createStatus)
            {
                case AccountCreateStatus.DuplicateUserName:
                    return "User name already exists. Please enter a different user name.";

                case AccountCreateStatus.DuplicateEmail:
                    return "A user name for that e-mail address already exists. Please enter a different e-mail address.";

                case AccountCreateStatus.InvalidPassword:
                    return "The password provided is invalid. Please enter a valid password value.";

                case AccountCreateStatus.InvalidEmail:
                    return "The e-mail address provided is invalid. Please check the value and try again.";

                case AccountCreateStatus.InvalidAnswer:
                    return "The password retrieval answer provided is invalid. Please check the value and try again.";

                case AccountCreateStatus.InvalidQuestion:
                    return "The password retrieval question provided is invalid. Please check the value and try again.";

                case AccountCreateStatus.InvalidUserName:
                    return "The user name provided is invalid. Please check the value and try again.";

                case AccountCreateStatus.ProviderError:
                    return "The authentication provider returned an error. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                case AccountCreateStatus.UserRejected:
                    return "The user creation request has been canceled. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                default:
                    return "An unknown error occurred. Please verify your entry and try again. If the problem persists, please contact your system administrator.";
            }
        }
        #endregion
    }
}
