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
        IList<IUser> GetAllUsers();

        bool UpdateUser(long targetId, string param, string newValue);
        bool UpdateVehicle(long targetId, string param, string newValue);
        bool UpdateRide(long targetId, string param, string newValue);
        bool UpdateLogin(long targetId, string salt, string hash);

        //IList<IRide> GetRidesByUserId(long userId);
        IList<IRide> GetRidesByDriverId(long userId);
        IList<IRide> GetRidesByRiderId(long id);
        List<IRide> GetRidesById(long rideId);
        List<IRide> GetAllRides();

        bool AddNewRide(IRide ride);

        IList<IVehicle> GetAllVehicles();
        IList<IVehicle> GetVehiclesByUserId(int id);
    }
}