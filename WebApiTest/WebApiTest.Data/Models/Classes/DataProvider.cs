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

        private string RunScalar(string connectionString)
        {
            var sqlCommand = new SqlCommand(connectionString, _connection);

            using (OpenConnection())
            {
                var result = sqlCommand.ExecuteScalar()?.ToString();
                return result;
            }
        }

        private DataTableReader RunReader(string connectionString)
        {
            var sqlCommand = new SqlCommand(connectionString, _connection);
            var adapter = new SqlDataAdapter(sqlCommand);
            var dataSet = new DataSet();

            using (OpenConnection())
            {
                //var reader = sqlCommand.ExecuteReader();
                adapter.Fill(dataSet);
                return dataSet.CreateDataReader();
            }
        }

        private bool RunNonQuery(string connectionString)
        {
            var sqlCommand = new SqlCommand(connectionString, _connection);

            try
            {
                using (OpenConnection())
                {
                    sqlCommand.ExecuteNonQuery();
                }

                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public string GetSaltById(long userId)
        {
            var sqlCommandString = $"SELECT Salt FROM Logins WHERE UserId = {userId};";
            return RunScalar(sqlCommandString);

            /*
            var sqlCommand = new SqlCommand(sqlCommandString, _connection);

            using (OpenConnection())
            {
                var result = sqlCommand.ExecuteScalar()?.ToString();
                return result;
            }
            */
        }

        public long GetUserIdByEmail(string email)
        {
            var sqlCommandString = $"SELECT UserId FROM Users WHERE Email = '{email}';";
            return RunScalar(sqlCommandString) != null 
                ? Convert.ToInt64(RunScalar(sqlCommandString)) 
                : -1;

            /*
            var sqlCommand = new SqlCommand(sqlCommandString, _connection);

            using (OpenConnection())
            {
                var result = sqlCommand.ExecuteScalar();
                if (result == null) return -1;

                return Convert.ToInt64(result.ToString());
            }
            */
        }

        public string GetHashById(long userId)
        {
            var sqlCommandString = $"SELECT Hash FROM Logins WHERE UserId = {userId};";

            return RunScalar(sqlCommandString);

            /*
            var sqlCommand = new SqlCommand(sqlCommandString, _connection);

            using (OpenConnection())
            {
                var result = sqlCommand.ExecuteScalar()?.ToString();

                return result;
            }
            */
        }

        private IUser GetUserById(long userId)
        {
            var sqlCommandString = $"SELECT * FROM Users WHERE UserId = {userId};";

            //var results = RunReader(sqlCommandString);

            var sqlCommand = new SqlCommand(sqlCommandString, _connection);

            using (OpenConnection())
            {
                var results = sqlCommand.ExecuteReader();

                return results.Read()
                    ? new User
                    {
                        Id = results.GetInt32(0),
                        FirstName = results.GetString(1),
                        LastName = results.GetString(2),
                        Email = results.GetString(3),
                        UserType = results.GetInt32(4)
                    }
                    : null;
            }
        }

        private IVehicle GetVehicleById(long vehicleId)
        {
            var sqlCommandString = $"SELECT * FROM Vehicles WHERE VehicleId = {vehicleId};";

            var results = RunReader(sqlCommandString);
           //var table = results.Tables[0];

            return results.Read()
                ? new Vehicle
                {
                    Id = results.GetInt32(0),
                    Driver = GetUserById(results.GetInt64(1)),
                    Year = results.GetInt32(2),
                    Make = results.GetString(3),
                    Model = results.GetString(4),
                    Color = results.GetString(5),
                    License = results.GetString(6)
                }
                : null;

            /*
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
            */
        }

        public bool UserExists(string email)
        {
            var sqlCommandString = $"SELECT COUNT(*) FROM Users WHERE Email = '{email}';";

            return RunScalar(sqlCommandString).ToString() == "1";

            /*
            var sqlCommand = new SqlCommand(sqlCommandString, _connection);

            using (OpenConnection())
            {
                var result = sqlCommand.ExecuteScalar()?.ToString();
                return result == "1";
            }
            */
        }

        public bool AddNewUser(string firstName, string lastName, string email, string salt, string hash)
        {
            var sqlCommandString =
                $"EXEC dbo.sp_AddNewUser '{firstName}', '{lastName}', '{email}', '{salt}', '{hash}';";

            RunNonQuery(sqlCommandString);

            /*
            var sqlCommand = new SqlCommand(sqlCommandString, _connection);

            using (OpenConnection())
            {
                sqlCommand.ExecuteNonQuery();
            }
            */
            return UserExists(email);
        }

        public bool AddNewUser(IUser newUser, string salt, string hash)
        {
            var sqlCommandString =
                $"EXEC dbo.sp_AddNewUser '{newUser.FirstName}', '{newUser.LastName}', "
                + $"'{newUser.Email}', '{salt}', '{hash}';";

            RunNonQuery(sqlCommandString);

            /*
            var sqlCommand = new SqlCommand(sqlCommandString, _connection);

            using (OpenConnection())
            {
                sqlCommand.ExecuteNonQuery();
            }
            */
            return UserExists(newUser.Email);
        }

        public string GetFirstNameByEmail(string email)
        {
            var sqlCommandString = $"SELECT FirstName FROM Users WHERE Email = '{email}';";

            return RunScalar(sqlCommandString);
            /*
            var sqlCommand = new SqlCommand(sqlCommandString, _connection);

            using (OpenConnection())
            {
                var result = sqlCommand.ExecuteScalar()?.ToString();
                return result;
            }
            */
        }

        public string GetLastNameByEmail(string email)
        {
            var sqlCommandString = $"SELECT LastName FROM Users WHERE Email = '{email}';";

            return RunScalar(sqlCommandString);

            /*
            var sqlCommand = new SqlCommand(sqlCommandString, _connection);

            using (OpenConnection())
            {
                var result = sqlCommand.ExecuteScalar()?.ToString();
                return result;
            }
            */
        }

        public List<IRide> GetRidesByUserId(long userId)
        {
            var sqlCommandString = $"SELECT * FROM Rides r WHERE r.UserId = {userId};";
            var results = RunReader(sqlCommandString);


            if (results.HasRows)
            {
                var rideHistory = new List<IRide>();

                while (results.Read())
                {
                    rideHistory.Add(new Ride()
                    {
                        Id = results.GetInt32(0),
                        Rider = GetUserById(results.GetInt32(1)),
                        Driver = GetUserById(results.GetInt32(2)),
                        Vehicle = GetVehicleById(results.GetInt32(3)),
                        Destination = results.GetString(4),
                        PickupLocation = results.GetString(5),
                        Distance = results.GetDouble(6),
                        RequestTime = results.GetDateTime(7),
                        StartTime = results.GetDateTime(8),
                        EndTime = results.GetDateTime(9)
                    });
                }

                return rideHistory;
            }

            return null;

            /*
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
            */
        }

        public List<IRide> GetRidesById(long rideId)
        {
            var sqlCommandString = $"SELECT * FROM Rides WHERE RideId = {rideId};";

            var results = RunReader(sqlCommandString);

            if (results.HasRows)
            {
                var rideHistory = new List<IRide>();
                while (results.Read())
                {
                    rideHistory.Add(new Ride
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
                    });
                }

                return rideHistory;
            }

            return null;

            /*
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
            */
        }

        public List<IRide> GetAllRides()
        {
            var sqlCommandString = $"SELECT * FROM Rides;";

            var results = RunReader(sqlCommandString);

            if (results.HasRows)
            {
                var rideHistory = new List<IRide>();
                while (results.Read())
                {
                    rideHistory.Add(new Ride
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
                    });
                }

                return rideHistory;
            }

            return null;

            /*
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
            */
        }

        public void AddNewRide(IRide ride)
        {
            var sqlCommandString =
                "INSERT INTO Rides (DriverId, RiderId, VehicleId, PickupLocation, "
                + "Destination, RequestTime, StartTime, EndTime, Distance) "
                + $"VALUES (NULL, {ride.Rider.Id}, NULL,"
                + $" '{ride.PickupLocation}', '{ride.Destination}', '{ride.RequestTime}',"
                + $" NULL, NULL, NULL);";

            RunNonQuery(sqlCommandString);
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
