namespace RideWithMeWebApp.DataProvider.Models.Interfaces
{
    public interface IUpdateHandler
    {
        bool UpdateUser(long targetId, string param, string newValue);
        bool UpdateRide(long targetId, string param, string newValue);
        bool UpdateVehicle(long targetId, string param, string newValue);
        bool UpdateLogin(long targetId, string salt, string hash);

    }
}