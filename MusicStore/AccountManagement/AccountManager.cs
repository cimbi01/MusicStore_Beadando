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
        private static MusicStoreEntities storeEntities;
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
        ///Vissazaadja hogy a LogonModel Admin-e
        /// </summary>
        /// <param name="logOnModel"></param>
        /// <returns></returns>
        public static bool IsAdmin(LogOnModel logOnModel)
        {
            return storeEntities.Accounts.First(account =>
                account.UserName == logOnModel.UserName).IsAdmin;
        }
    }
}