using System.Data.SqlClient;
using RideWithMeWebApp.DataProvider.Models.Interfaces;

namespace RideWithMeWebApp.DataProvider.Models.Classes
{
    public class UpdateHandler : IUpdateHandler
    {
        private readonly SqlConnection _connection;
        private readonly string _sqlConnectionString;
        //private readonly IAuthenticator _authenticator;

        public UpdateHandler(SqlConnection con, string sqlConnectionString)
        {
            _connection = con;
            _sqlConnectionString = sqlConnectionString;
        }

        public bool UpdateUser(long targetId, string param, string newValue)
        {
            string sqlCommandString;
            if (param == "UserType")
            {
                sqlCommandString = $"UPDATE Users SET UserType = {newValue} WHERE UserId = {targetId};";
            }
            else
            {
                sqlCommandString = $"UPDATE Users SET {param} = '{newValue}' WHERE UserId = {targetId};";
            }

            var sqlCommand = new SqlCommand(sqlCommandString, _connection);

            _connection.ConnectionString = _sqlConnectionString;
            _connection.Open();
            var success = sqlCommand.ExecuteNonQuery() > 0;
            _connection.Close();
            return success;
        }

        public bool UpdateRide(long targetId, string param, string newValue)
        {
            var sqlCommandString = $"UPDATE Rides SET {param}  = ";
            if (param == "DriverId" || param == "RiderId" || param == "VehicleId")
            {
                sqlCommandString += $"{newValue}";
            }
            else if (param == "RequestTime" || param == "StartTime" || param == "EndTime")
            {
                // SQL Compliant Date Format:
                // YYYY-MM-DDThh:mm:ss[.mmm]
                // 2005-05-23T14:25:10

                sqlCommandString += $"{newValue}'";
            }
            else
            {
                sqlCommandString += $"'{newValue}";
            }

            sqlCommandString += $" WHERE RideId = {targetId};";

            var sqlCommand = new SqlCommand(sqlCommandString, _connection);

            _connection.ConnectionString = _sqlConnectionString;
            _connection.Open();

            var success = sqlCommand.ExecuteNonQuery();
            _connection.Close();
            return success > 0;
        }

        public bool UpdateVehicle(long targetId, string param, string newValue)
        {
            var sqlCommandString = $"UPDATE Vehicles SET {param} = ";

            if (param == "DriverId" || param == "ProdYear")
            {
                sqlCommandString += $"{newValue}";
            }
            else
            {
                sqlCommandString += $"'{newValue}'";
            }

            sqlCommandString += $" WHERE VehicleId = {targetId};";

            var sqlCommand = new SqlCommand(sqlCommandString, _connection);

            _connection.ConnectionString = _sqlConnectionString;
            _connection.Open();

            var success = sqlCommand.ExecuteNonQuery();
            _connection.Close();
            return success > 0;
        }

        public bool UpdateLogin(long targetId, string salt, string hash)
        {
            var sqlSaltCommandString = $"UPDATE Logins SET Salt = '{salt}' WHERE UserId = {targetId};";
            var sqlHashCommandString = $"UPDATE Logins SET Hash = '{hash}' WHERE UserId = {targetId};";
            
            var sqlSaltCommand = new SqlCommand(sqlSaltCommandString, _connection);
            var sqlHashCommand = new SqlCommand(sqlHashCommandString, _connection);

            _connection.ConnectionString = _sqlConnectionString;
            _connection.Open();

            var saltSuccess = sqlSaltCommand.ExecuteNonQuery();
            var hashSuccess = sqlHashCommand.ExecuteNonQuery();
            _connection.Close();

            return saltSuccess > 0 && hashSuccess > 0;
        }
    }
}