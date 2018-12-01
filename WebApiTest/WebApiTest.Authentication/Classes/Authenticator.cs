using System;
using System.Web.Helpers;
using WebApiTest.Authentication.Interfaces;
using WebApiTest.Data.Models.Classes;
using WebApiTest.Data.Models.Interfaces;

namespace WebApiTest.Authentication.Classes
{
    public class Authenticator : IAuthenticator
    {
        private readonly IDataProvider _dataProvider;

        public Authenticator(IDataProvider dataProvider)
        {
            _dataProvider = dataProvider;
        }

        public IUser ValidateUser(string email, string password)
        {
            try
            {
                var userId = _dataProvider.GetUserIdByEmail(email);
                var salt = _dataProvider.GetSaltById(userId);
                var hash = _dataProvider.GetHashById(userId);

                if (Crypto.VerifyHashedPassword(hash, password + salt))
                {
                    return new User()
                    {
                        Email = email,
                        FirstName = _dataProvider.GetFirstNameByEmail(email),
                        LastName = _dataProvider.GetLastNameByEmail(email)
                    };
                }

                return null;
            }
            catch (Exception)
            {
                return null;
            }

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
                return _dataProvider.AddNewUser(firstName, lastName, email, salt, hash);
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
