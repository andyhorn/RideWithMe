namespace RideWithMeWebApp.Models.Interfaces
{
    public interface IVehicle
    {
        int Id { get; set; }
        IUser Driver { get; set; }
        string Make { get; set; }
        string Model { get; set; }
        int Year { get; set; }
        string Color { get; set; }
        string License { get; set; }
    }
}