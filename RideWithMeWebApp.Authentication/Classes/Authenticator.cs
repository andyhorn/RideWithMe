using System;
using System.Collections;
using System.Collections.Generic;
using System.Web.Helpers;
using RideWithMeWebApp.Authentication.Interfaces;
using RideWithMeWebApp.DataProvider.Models.Interfaces;
using RideWithMeWebApp.Models.Classes;
using RideWithMeWebApp.Models.Interfaces;

namespace RideWithMeWebApp.Authentication.Classes
{
    public class Authenticator : IAuthenticator
    {
        private readonly IDataProvider _dataProvider;

        public Authenticator(IDataProvider dataProvider)
        {
            _dataProvider = dataProvider;
        }

        public bool ValidLogin(string email, string password)
        {
            try
            {
                // Will catch any errors thrown by improper SQL queries
                var userId = _dataProvider.GetUserIdByEmail(email); // Get the user's ID
                var salt = _dataProvider.GetSaltById(userId);       // Get the user's salt
                var hash = _dataProvider.GetHashById(userId);       // Get the user's hash key

                return Crypto.VerifyHashedPassword(hash, password + salt); // Verify the password is valid
            }
            catch (Exception)
            {
                // Return false on any exceptions
                return false;
            }
        }

        public IUser GetUser(string email, string password)
        {
            // Check if user exists, then check if the login is valid
            if (UserExists(email) && ValidLogin(email, password))
            {
                try
                {
                    // Return the user's data if all checks are passed
                    return _dataProvider.GetUserById(_dataProvider.GetUserIdByEmail(email));
                }
                catch (Exception)
                {
                    return null;
                }
            }
            return null;
        }

        public bool UserExists(string email)
        {
            return _dataProvider.UserExists(email);
        }

        public bool RegisterNewUser(IUser newUser, string password)
        {
            try
            {
                var salt = Crypto.GenerateSalt();
                var newPassword = password + salt;
                var hash = Crypto.HashPassword(newPassword);
                return _dataProvider.AddNewUser(newUser, salt, hash);
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static IDictionary<string, string> GetNewLogin(string password)
        {
            var salt = Crypto.GenerateSalt();
            var passwordAndSalt = password + salt;
            var hashKey = Crypto.HashPassword(passwordAndSalt);
            return new Dictionary<string, string>
            {
                {"Salt", salt},
                {"HashKey", hashKey}
            };
        }
    }
}
