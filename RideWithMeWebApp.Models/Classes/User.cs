using RideWithMeWebApp.Models.Interfaces;

namespace RideWithMeWebApp.Models.Classes
{
    public class User : IUser
    {
        public long Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public int UserType { get; set; }
    }
}