using WebApiTest.Data.Models.Interfaces;

namespace WebApiTest.Authentication.Interfaces
{
    public interface IAuthenticator
    {
        IUser ValidateUser(string email, string password);
        bool UserExists(string email);
        bool RegisterNewUser(string email, string password, string firstName, string lastName);
        bool RegisterNewUser(IUser newUser, string password);
    }
}