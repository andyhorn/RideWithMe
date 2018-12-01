namespace RideWithMeWebApp.DataProvider.Models.Interfaces
{
    public interface IUpdateHandler
    {
        void UpdateUser(long targetId, string param, string newValue);
        void UpdateRide(long targetId, string param, string newValue);
        void UpdateVehicle(long targetId, string param, string newValue);
        void UpdateLogin(long targetId, string param, string newValue);

    }
}