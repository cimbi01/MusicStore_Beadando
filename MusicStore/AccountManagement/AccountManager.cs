using MusicStore.EntityContext;
using MusicStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MusicStore.AccountManagement
{
    public static class AccountManager
    {
        private static MovieStoreEntities storeEntities = new MovieStoreEntities();
        /// <summary>
        /// Validálja, hogy létezik-e ilyen User ezzel a jelszóval az adatbázisban
        /// </summary>
        /// <param name="username">Felhasználónév</param>
        /// <param name="password">Jelszó</param>
        /// <returns></returns>
        public static bool ValidateAccount(string username, string password)
        {
            return storeEntities.Accounts.Any(account =>
                account.UserName == username &&
                account.Password == password);
        }
        /// <summary>
        /// Visszaadja az adatbázisban tárolt Account-t
        /// </summary>
        /// <param name="logOnModel">LogonModel amit a logonModel ad</param>
        /// <returns></returns>
        public static Account GetAccount(LogOnModel logOnModel)
        {
            return storeEntities.Accounts.First(account =>
                account.UserName == logOnModel.UserName);
        }
        /// <summary>
        /// Visszaadja az adatbázisban tárolt Account-t
        /// </summary>
        /// <param name="registerModel">RegisterModel amit a registerModel ad</param>
        /// <returns></returns>
        public static Account GetAccount(RegisterModel registerModel)
        {
            return storeEntities.Accounts.First(account =>
                account.UserName == registerModel.UserName);
        }
        /// <summary>
        /// User hozzáadása az Adabzásihoz
        /// Ellenőrzi hogy vannak-e ütközések, vagy hibák
        /// </summary>
        /// <param name="registerModel"></param>
        /// <returns>AccountCreateStatus enum hozzáadás eredménye szerint</returns>
        public static AccountCreateStatus CreateUser(RegisterModel registerModel)
        {
            // Van egyező username
            if (storeEntities.Accounts.Any(account =>
                 account.UserName == registerModel.UserName))
            {
                return AccountCreateStatus.DuplicateUserName;
            }
            // Van egyező email
            else if (storeEntities.Accounts.Any(account =>
                 account.Email == registerModel.Email))
            {
                return AccountCreateStatus.DuplicateEmail;
            }
            // invalid password
            if((registerModel.Password.Length < 6 ||
                registerModel.Password.Length > 100) ||
                (registerModel.Password != registerModel.ConfirmPassword))
            {
                return AccountCreateStatus.InvalidPassword;
            }
            // invalid username
            if (registerModel.UserName == "")
            {
                return AccountCreateStatus.InvalidUserName;
            }
            // invalid email: üres, vagy nincs benne kukac
            //forma: valami@valami.valami
            if (registerModel.Email == "" ||
                registerModel.Email.Split('@').Length != 2)
            {
                return AccountCreateStatus.InvalidEmail;
            }

            //invalid email: van benne kukac, de a kukac utáni részben nem csak egy pont van
            //forma: valami@valami.valami
            if (registerModel.Email.Split('@')[1]
                .Split('.').Length != 2)
            {
                return AccountCreateStatus.InvalidEmail;
            }
            // nem dobott hibát azaz beletette az entitibe
            storeEntities.Accounts.Add(
                new Account()
                {
                    UserName = registerModel.UserName,
                    Email = registerModel.Email,
                    Password = registerModel.Password
                });
            storeEntities.SaveChanges();
            return AccountCreateStatus.Success;
        }
        public static bool ChangePassword(ChangePasswordModel changePasswordModel, Account current_user)
        {
            // invalid newpassword
            if ((changePasswordModel.NewPassword.Length < 6 ||
                changePasswordModel.NewPassword.Length > 100) ||
                (changePasswordModel.NewPassword != changePasswordModel.ConfirmPassword) ||
                (changePasswordModel.OldPassword != current_user.Password))
            {
                return false;
            }
            //valid new password
            storeEntities.Accounts.First(account =>
                account.Password == changePasswordModel.OldPassword).Password = changePasswordModel.NewPassword;
            storeEntities.SaveChanges();
            current_user.Password = changePasswordModel.NewPassword;
            return true;
        }
    }
}