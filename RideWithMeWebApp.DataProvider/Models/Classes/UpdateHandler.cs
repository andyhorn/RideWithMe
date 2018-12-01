using System.Data.SqlClient;
using RideWithMeWebApp.DataProvider.Models.Interfaces;

namespace RideWithMeWebApp.DataProvider.Models.Classes
{
    public class UpdateHandler : IUpdateHandler
    {
        private readonly SqlConnection _connection;
        private readonly string _sqlConnectionString;

        public UpdateHandler(SqlConnection con, string sqlConnectionString)
        {
            _connection = con;
            _sqlConnectionString = sqlConnectionString;
        }

        public void UpdateUser(long targetId, string param, string newValue)
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

            sqlCommand.ExecuteNonQuery();

            _connection.Close();
        }

        public void UpdateRide(long targetId, string param, string newValue)
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

            sqlCommand.ExecuteNonQuery();

            _connection.Close();
        }

        public void UpdateVehicle(long targetId, string param, string newValue)
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

            sqlCommand.ExecuteNonQuery();

            _connection.Close();
        }

        public void UpdateLogin(long targetId, string param, string newValue)
        {
            var sqlCommandString = $"UPDATE Logins SET Hash = '{newValue}; WHERE LoginId = {targetId};";
            
            var sqlCommand = new SqlCommand(sqlCommandString, _connection);

            _connection.ConnectionString = _sqlConnectionString;
            _connection.Open();

            sqlCommand.ExecuteNonQuery();

            _connection.Close();
        }
    }
}