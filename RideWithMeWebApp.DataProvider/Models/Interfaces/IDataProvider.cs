using System.Collections.Generic;
using RideWithMeWebApp.Models.Interfaces;

namespace RideWithMeWebApp.DataProvider.Models.Interfaces
{
    public interface IDataProvider
    {
        string GetSaltById(long userId);
        long GetUserIdByEmail(string email);
        string GetHashById(long userId);

        bool UserExists(string email);
        bool AddNewUser(IUser newUser, string salt, string hash);
        IUser GetUserById(long id);
        string GetFirstNameByEmail(string email);
        string GetLastNameByEmail(string email);

        bool UpdateUser(long targetId, string param, string newValue);
        bool UpdateVehicle(long targetId, string param, string newValue);
        bool UpdateRide(long targetId, string param, string newValue);
        bool UpdateLogin(long targetId, string salt, string hash);

        List<IRide> GetRidesByUserId(long userId);
        List<IRide> GetRidesById(long rideId);
        List<IRide> GetAllRides();

        bool AddNewRide(IRide ride);
    }
}