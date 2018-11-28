using WebApiTest.Data.Interfaces;

namespace WebApiTest.Authentication.Interfaces
{
    public interface IAuthenticator
    {
        IUser ValidateUser(string email, string password);
        bool UserExists(string email);
        bool RegisterNewUser(string email, string password, string firstName, string lastName);
    }
}