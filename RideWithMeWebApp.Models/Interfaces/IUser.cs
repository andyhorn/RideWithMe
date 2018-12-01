namespace RideWithMeWebApp.Models.Interfaces
{
    public interface IUser
    {
        long Id { get; set; }
        string FirstName { get; set; }
        string LastName { get; set; }
        string Email { get; set; }
        int UserType { get; set; }
    }
}