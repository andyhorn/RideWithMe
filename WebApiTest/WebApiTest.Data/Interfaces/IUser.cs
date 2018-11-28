namespace WebApiTest.Data.Interfaces
{
    public interface IUser
    {

    }

    public class User : IUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
    }
}