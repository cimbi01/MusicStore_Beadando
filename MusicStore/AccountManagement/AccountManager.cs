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
        public static bool ValidateUser(string username, string password)
        {
            foreach (Account account in storeEntities.Accounts)
            {
                if(account.UserName == username &&
                    account.Password == password)
                {
                    return true;
                }
            }
            return false;
        }
    }
}