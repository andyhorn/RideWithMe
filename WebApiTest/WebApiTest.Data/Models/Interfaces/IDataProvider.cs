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
    }
}