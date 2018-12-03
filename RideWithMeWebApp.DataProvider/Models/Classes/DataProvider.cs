using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using RideWithMeWebApp.DataProvider.Models.Interfaces;
using RideWithMeWebApp.Models.Classes;
using RideWithMeWebApp.Models.Interfaces;

namespace RideWithMeWebApp.DataProvider.Models.Classes
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
            catch (Exception)
            {
                return false;
            }
        }

        public string GetSaltById(long userId)
        {
            var sqlCommandString = $"SELECT Salt FROM Logins WHERE UserId = {userId};";
            return RunScalar(sqlCommandString);
        }

        public long GetUserIdByEmail(string email)
        {
            var sqlCommandString = $"SELECT UserId FROM Users WHERE Email = '{email}';";
            return RunScalar(sqlCommandString) != null 
                ? Convert.ToInt64(RunScalar(sqlCommandString)) 
                : -1;
        }

        public string GetHashById(long userId)
        {
            var sqlCommandString = $"SELECT Hash FROM Logins WHERE UserId = {userId};";

            return RunScalar(sqlCommandString);
        }

        public IUser GetUserById(long userId)
        {
            var sqlCommandString = $"SELECT * FROM Users WHERE UserId = {userId};";

            var results = RunReader(sqlCommandString);

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

        public IList<IUser> GetAllUsers()
        {
            var collection = new List<IUser>();

            var sqlCommandString = "SELECT * FROM Users ORDER BY FirstName, LastName;";
            var results = RunReader(sqlCommandString);

            while (results.Read())
            {
                collection.Add(new User()
                {
                    Id = results.GetInt32(0),
                    FirstName = results.GetString(1),
                    LastName = results.GetString(2),
                    Email = results.GetString(3),
                    UserType = results.GetInt32(4)
                });
            }

            return collection;
        }

        private IVehicle GetVehicleById(long vehicleId)
        {
            var sqlCommandString = $"SELECT * FROM Vehicles WHERE VehicleId = {vehicleId};";

            var results = RunReader(sqlCommandString);

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
        }

        public bool UserExists(string email)
        {
            var sqlCommandString = $"SELECT COUNT(*) FROM Users WHERE Email = '{email}';";

            return RunScalar(sqlCommandString) == "1";
        }

        public bool AddNewUser(IUser newUser, string salt, string hash)
        {
            var sqlCommandString =
                $"EXEC dbo.sp_AddNewUser '{newUser.FirstName}', '{newUser.LastName}', "
                + $"'{newUser.Email}', '{salt}', '{hash}', {newUser.UserType};";

            RunNonQuery(sqlCommandString);

            return UserExists(newUser.Email);
        }

        public string GetFirstNameByEmail(string email)
        {
            var sqlCommandString = $"SELECT FirstName FROM Users WHERE Email = '{email}';";

            return RunScalar(sqlCommandString);
        }

        public string GetLastNameByEmail(string email)
        {
            var sqlCommandString = $"SELECT LastName FROM Users WHERE Email = '{email}';";

            return RunScalar(sqlCommandString);
        }

        public IList<IRide> GetRidesByRiderId(long id)
        {
            var sqlCommandString = $"SELECT * FROM Rides r WHERE r.RiderId = {id};";
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
        }

        public IList<IRide> GetRidesByDriverId(long id)
        {
            var sqlCommandString = $"SELECT * FROM Rides r WHERE r.DriverId = {id};";
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
                        Id = results.GetInt32(0),
                        Driver = (results.GetValue(1) != DBNull.Value) ? GetUserById(results.GetInt32(1)) : null,       // Driver can be null
                        Rider = GetUserById(results.GetInt32(2)),                                                       // Rider cannot be null
                        Vehicle = (results.GetValue(3) != DBNull.Value) ? GetVehicleById(results.GetInt32(3)) : null,   // Vehicle can be null
                        PickupLocation = results.GetString(4),                                                          // Can be null string
                        Destination = results.GetString(5),                                                             // Can, but won't be, null
                        RequestTime = results.GetDateTime(6),                                                           // Cannot be null
                        StartTime = (results.GetValue(7) != DBNull.Value) ? results.GetDateTime(7) : new DateTime?(),   // Can be null
                        EndTime = (results.GetValue(8) != DBNull.Value) ? results.GetDateTime(8) : new DateTime?(),     // Can be null
                        Distance = (results.GetValue(9) != DBNull.Value) ? results.GetDouble(9) : 0,                    // Can be null/zero
                        Status = results.GetString(10)
                    });
                }

                return rideHistory;
            }

            return null;
        }

        public bool AddNewRide(IRide ride)
        {
            var sqlCommandString =
                "INSERT INTO Rides (DriverId, RiderId, VehicleId, PickupLocation, "
                + "Destination, RequestTime, StartTime, EndTime, Distance, StatusId) "
                + $"VALUES (NULL, {ride.Rider.Id}, NULL,"
                + $" '{ride.PickupLocation}', '{ride.Destination}', '{ride.RequestTime}',"
                + $" NULL, NULL, NULL, 0);";

            return RunNonQuery(sqlCommandString);
        }

        public IList<IVehicle> GetAllVehicles()
        {
            var sqlCommandString = "SELECT * FROM Vehicles";
            var results = RunReader(sqlCommandString);

            if (results.HasRows)
            {
                var collection = new List<IVehicle>();
                while (results.Read())
                {
                    collection.Add(new Vehicle()
                    {
                        Id = results.GetInt32(0),
                        Driver = GetUserById(results.GetInt32(1)),
                        Year = results.GetInt32(2),
                        Make = results.GetString(3),
                        Model = results.GetString(4),
                        Color = results.GetString(5),
                        License = results.GetString(6)
                    });
                }

                return collection;
            }

            return null;
        }

        public IList<IVehicle> GetVehiclesByUserId(int id)
        {
            var sqlCommandString = $"SELECT * FROM Vehicles WHERE DriverId = {id};";
            var results = RunReader(sqlCommandString);

            if (results.HasRows)
            {
                var collection = new List<IVehicle>();
                while (results.Read())
                {
                    collection.Add(new Vehicle
                    {
                        Id = results.GetInt32(0),
                        Driver = GetUserById(results.GetInt32(1)),
                        Year = results.GetInt32(2),
                        Make = results.GetString(3),
                        Model = results.GetString(4),
                        Color = results.GetString(5),
                        License = results.GetString(6)
                    });
                }

                return collection;
            }

            return null;
        }

        public bool UpdateLogin(long targetId, string salt, string hash)
        {
            return _updateHandler.UpdateLogin(targetId, salt, hash);
        }

        public bool UpdateUser(long targetId, string param, string newValue)
        {
            return _updateHandler.UpdateUser(targetId, param, newValue);
        }

        public bool UpdateVehicle(long targetId, string param, string newValue)
        {
            return _updateHandler.UpdateVehicle(targetId, param, newValue);
        }

        public bool UpdateRide(long targetId, string param, string newValue)
        {
            return _updateHandler.UpdateRide(targetId, param, newValue);
        }
    }
}
