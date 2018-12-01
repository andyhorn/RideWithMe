using System;
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
                var userId = _dataProvider.GetUserIdByEmail(email);
                var salt = _dataProvider.GetSaltById(userId);
                var hash = _dataProvider.GetHashById(userId);

                return Crypto.VerifyHashedPassword(hash, password + salt);
            }
            catch (Exception)
            {
                return false;
            }

            //try
            //{
            //    var userId = _dataProvider.GetUserIdByEmail(email);
            //    var salt = _dataProvider.GetSaltById(userId);
            //    var hash = _dataProvider.GetHashById(userId);

            //    if (Crypto.VerifyHashedPassword(hash, password + salt))
            //    {
            //        // TODO: Return the actual user object from GetUserByEmail method
            //        return _dataProvider.GetUserById(_dataProvider.GetUserIdByEmail(email));
            //    }

            //    return null;
            //}
            //catch (Exception)
            //{
            //    return null;
            //}

        }

        public IUser GetUser(string email, string password)
        {
            if (UserExists(email) && ValidLogin(email, password))
            {
                try
                {
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

        public bool RegisterNewUser(string email, string password, string firstName, string lastName)
        {
            try
            {
                var salt = Crypto.GenerateSalt();
                var newPassword = password + salt;
                var hash = Crypto.HashPassword(newPassword);
                return _dataProvider.AddNewUser(new User
                {
                    FirstName = firstName,
                    LastName = lastName,
                    Email = email
                }, salt, hash);

                // TODO: 
            }
            catch (Exception)
            {
                return false;
            }
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
    }
}
