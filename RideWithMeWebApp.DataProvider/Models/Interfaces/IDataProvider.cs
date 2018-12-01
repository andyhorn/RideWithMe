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

        void UpdateUser(long targetId, string param, string newValue);
        void UpdateVehicle(long targetId, string param, string newValue);
        void UpdateRide(long targetId, string param, string newValue);
        void UpdateLogin(long targetId, string param, string newValue);

        List<IRide> GetRidesByUserId(long userId);
        List<IRide> GetRidesById(long rideId);
        List<IRide> GetAllRides();

        bool AddNewRide(IRide ride);
    }
}