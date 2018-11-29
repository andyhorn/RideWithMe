using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using WebApiTest.Data.Models.Interfaces;

namespace WebApiTest.Data.Models.Classes
{
    public class DataProvider : IDataProvider
    {
        private readonly SqlConnection _connection;

        private readonly IUpdateHandler _updateHandler;
        //private const string SqlConnectionString = "Server=.;Database=RideWithMe;Integrated Security=true;";
        private const string SqlConnectionString = @"Server=tcp:ridewithme.database.windows.net,1433;Initial Catalog=RideWithMeDb;Persist Security Info=False;User ID=andrew.horn;Password=B0r1sCat;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

        public DataProvider()
        {
            _connection = new SqlConnection();
            _updateHandler = new UpdateHandler(_connection, SqlConnectionString);
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

        private IUser GetUserById(long userId)
        {
            var sqlCommandString = $"SELECT * FROM Users WHERE UserId = {userId};";
            var sqlCommand = new SqlCommand(sqlCommandString, _connection);
            var newUser = new User();

            using (OpenConnection())
            {
                var results = sqlCommand.ExecuteReader();

                while (results.Read())
                {
                    newUser.Email = results.GetString(3);
                    newUser.FirstName = results.GetString(1);
                    newUser.LastName = results.GetString(2);
                    newUser.UserType = results.GetInt32(4);
                }
            }

            return newUser;
        }

        private IVehicle GetVehicleById(long vehicleId)
        {
            var sqlCommandString = $"SELECT * FROM Vehicles WHERE VehicleId = {vehicleId};";
            var sqlCommand = new SqlCommand(sqlCommandString, _connection);
            var vehicle = new Vehicle();

            using (OpenConnection())
            {
                var results = sqlCommand.ExecuteReader();

                while (results.Read())
                {
                    vehicle.Driver = GetUserById(results.GetInt64(1));
                    vehicle.Year = results.GetInt32(2);
                    vehicle.Make = results.GetString(3);
                    vehicle.Model = results.GetString(4);
                    vehicle.Color = results.GetString(5);
                    vehicle.License = results.GetString(6);
                }
            }

            return vehicle;
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

        public List<IRide> GetRidesByUserId(long userId)
        {
            var sqlCommandString = $"SELECT * FROM Rides r WHERE r.UserId = {userId};";
            var sqlCommand = new SqlCommand(sqlCommandString, _connection);
            var rideHistory = new List<IRide>();
            SqlDataReader results;

            using (OpenConnection())
            {
                results = sqlCommand.ExecuteReader();
            }

            if (results.HasRows)
            {
                while (results.Read())
                {
                    var ride = new Ride()
                    {
                        Driver = GetUserById(1),
                        Rider = GetUserById(2),
                        Vehicle = GetVehicleById(3),
                        PickupLocation = results.GetString(4),
                        Destination = results.GetString(5),
                        RequestTime = results.GetDateTime(6),
                        StartTime = results.GetDateTime(7),
                        EndTime = results.GetDateTime(8),
                        Distance = results.GetDouble(9)  
                    };

                    rideHistory.Add(ride);
                }
            }

            return rideHistory;
        }

        public List<IRide> GetRidesById(long rideId)
        {
            var sqlCommandString = $"SELECT * FROM Rides WHERE RideId = {rideId};";
            var sqlCommand = new SqlCommand(sqlCommandString, _connection);
            SqlDataReader results;
            List<IRide> rideHistory = new List<IRide>();

            using (OpenConnection())
                results = sqlCommand.ExecuteReader();

            while (results.Read())
            {
                rideHistory.Add(new Ride
                {
                    // index zero is a throwaway for now
                    Driver = GetUserById(results.GetInt64(1)),
                    Rider = GetUserById(results.GetInt64(2)),
                    Vehicle = GetVehicleById(results.GetInt64(3)),
                    PickupLocation = results.GetString(4),
                    Destination = results.GetString(5),
                    RequestTime = results.GetDateTime(6),
                    StartTime = results.GetDateTime(7),
                    EndTime = results.GetDateTime(8),
                    Distance = results.GetDouble(9)
                });
            }

            return rideHistory;
        }

        public List<IRide> GetAllRides()
        {
            var sqlCommandString = $"SELECT * FROM Rides;";
            var sqlCommand = new SqlCommand(sqlCommandString, _connection);
            SqlDataReader results;
            List<IRide> rideHistory = new List<IRide>();

            using (OpenConnection())
                results = sqlCommand.ExecuteReader();

            while (results.Read())
            {
                rideHistory.Add(new Ride
                {
                    // index zero is a throwaway for now
                    Driver = GetUserById(results.GetInt64(1)),
                    Rider = GetUserById(results.GetInt64(2)),
                    Vehicle = GetVehicleById(results.GetInt64(3)),
                    PickupLocation = results.GetString(4),
                    Destination = results.GetString(5),
                    RequestTime = results.GetDateTime(6),
                    StartTime = results.GetDateTime(7),
                    EndTime = results.GetDateTime(8),
                    Distance = results.GetDouble(9)
                });
            }

            return rideHistory;
        }

        public void AddNewRide(IRide ride)
        {
            // TODO: Implement AddNewRide
        }

        public void UpdateUser(long targetId, string param, string newValue)
        {
            _updateHandler.UpdateUser(targetId, param, newValue);
        }

        public void UpdateVehicle(long targetId, string param, string newValue)
        {
            _updateHandler.UpdateVehicle(targetId, param, newValue);
        }

        public void UpdateRide(long targetId, string param, string newValue)
        {
            _updateHandler.UpdateRide(targetId, param, newValue);
        }

        public void UpdateLogin(long targetId, string param, string newValue)
        {
            _updateHandler.UpdateLogin(targetId, param, newValue);
        }
    }
}
