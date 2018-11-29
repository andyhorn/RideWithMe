using System.Collections.Generic;

namespace WebApiTest.Data.Models.Interfaces
{
    public interface IDataProvider
    {
        string GetSaltById(long userId);
        long GetIdByEmail(string email);
        string GetHashById(long userId);
        bool UserExists(string email);
        bool AddNewUser(string firstName, string lastName, string email, string salt, string hash);
        string GetFirstNameByEmail(string email);
        string GetLastNameByEmail(string email);

        void UpdateUser(long targetId, string param, string newValue);
        void UpdateVehicle(long targetId, string param, string newValue);
        void UpdateRide(long targetId, string param, string newValue);
        void UpdateLogin(long targetId, string param, string newValue);

        List<IRide> GetRidesByUserId(long userId);
        List<IRide> GetRidesById(long rideId);
        List<IRide> GetAllRides();

        void AddNewRide(IRide ride);
    }
}