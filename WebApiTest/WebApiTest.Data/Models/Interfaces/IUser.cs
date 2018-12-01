namespace WebApiTest.Data.Models.Interfaces
{
    public interface IUser
    {
        int Id { get; set; }
        string FirstName { get; set; }
        string LastName { get; set; }
        string Email { get; set; }
        int UserType { get; set; }
    }
}