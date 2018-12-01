using RideWithMeWebApp.Models.Interfaces;

namespace RideWithMeWebApp.Authentication.Interfaces
{
    public interface IAuthenticator
    {
        bool ValidLogin(string email, string password);
        IUser GetUser(string email, string password);
        bool UserExists(string email);
        bool RegisterNewUser(IUser newUser, string password);
    }
}