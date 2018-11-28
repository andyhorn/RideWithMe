using System;
using System.Data;
using System.Data.SqlClient;
using WebApiTest.Data.Interfaces;

namespace WebApiTest.Data.Classes
{
    public class DataProvider : IDataProvider
    {
        private readonly SqlConnection _connection;
        //private const string SqlConnectionString = "Server=.;Database=RideWithMe;Integrated Security=true;";

        private const string SqlConnectionString = @"Server=tcp:ridewithme.database.windows.net,1433;Initial Catalog=RideWithMeDb;Persist Security Info=False;User ID=andrew.horn;Password=B0r1sCat;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

        public DataProvider()
        {
            _connection = new SqlConnection();
        }

        private SqlConnection OpenConnection()
        {
            _connection.ConnectionString = SqlConnectionString;
            if (_connection.State == ConnectionState.Closed)
                _connection.Open();
            return _connection;
        }

        public string GetSaltById(long userId)
        {
            var sqlCommandString = $"SELECT Salt FROM Logins WHERE UserId = {userId};";
            var sqlCommand = new SqlCommand(sqlCommandString, _connection);

            using (OpenConnection())
            {
                var result = sqlCommand.ExecuteScalar()?.ToString();
                return result;
            }
        }

        public long GetIdByEmail(string email)
        {
            var sqlCommandString = $"SELECT UserId FROM Users WHERE Email = '{email}';";
            var sqlCommand = new SqlCommand(sqlCommandString, _connection);

            using (OpenConnection())
            {
                var result = sqlCommand.ExecuteScalar();
                if (result == null) return -1;

                return Convert.ToInt64(result.ToString());
            }
        }

        public string GetHashById(long userId)
        {
            var sqlCommandString = $"SELECT Hash FROM Logins WHERE UserId = {userId};";
            var sqlCommand = new SqlCommand(sqlCommandString, _connection);

            using (OpenConnection())
            {
                var result = sqlCommand.ExecuteScalar()?.ToString();

                return result;
            }
        }

        public bool UserExists(string email)
        {
            var sqlCommandString = $"SELECT COUNT(*) FROM Users WHERE Email = '{email}';";
            var sqlCommand = new SqlCommand(sqlCommandString, _connection);

            using (OpenConnection())
            {
                var result = sqlCommand.ExecuteScalar()?.ToString();
                return result == "1";
            }
        }

        public bool AddNewUser(string firstName, string lastName, string email, string salt, string hash)
        {
            var sqlCommandString =
                $"EXEC dbo.sp_AddNewUser '{firstName}', '{lastName}', '{email}', '{salt}', '{hash}';";
            var sqlCommand = new SqlCommand(sqlCommandString, _connection);

            using (OpenConnection())
            {
                sqlCommand.ExecuteNonQuery();
            }

            return UserExists(email);
        }

        public string GetFirstNameByEmail(string email)
        {
            var sqlCommandString = $"SELECT FirstName FROM Users WHERE Email = '{email}';";
            var sqlCommand = new SqlCommand(sqlCommandString, _connection);

            using (OpenConnection())
            {
                var result = sqlCommand.ExecuteScalar()?.ToString();
                return result;
            }
        }

        public string GetLastNameByEmail(string email)
        {
            var sqlCommandString = $"SELECT LastName FROM Users WHERE Email = '{email}';";
            var sqlCommand = new SqlCommand(sqlCommandString, _connection);

            using (OpenConnection())
            {
                var result = sqlCommand.ExecuteScalar()?.ToString();
                return result;
            }
        }
    }
}
