using iHome.Models.iHomeComponents;
using MySql.Data.MySqlClient;
using System.Data.SqlClient;

namespace iHome.Models
{
    public class DatabaseModel
    {
        MySqlConnectionStringBuilder builder;
        public DatabaseModel(string server, string login, string password, string db)
        {
            builder = new MySqlConnectionStringBuilder();
            builder.Server = server;
            builder.UserID = login;
            builder.Password = password;
            builder.Database = db;
        }
        public List<Room> GetRooms(string? uuid)
        {
            List<Room> listOfRooms = new List<Room>();
            using (MySqlConnection connection = new MySqlConnection(builder.ConnectionString))
            {
                connection.Open();

                string sql = "SELECT roomId, roomName, roomDescription, roomImage FROM Rooms WHERE uuid = '"+uuid+"'";

                using (MySqlCommand command = new MySqlCommand(sql, connection))
                {
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string description = "", image="";
                            try
                            {
                                description = reader.GetString(2);
                            }
                            catch { }
                            try
                            {
                                image = reader.GetString(3);
                            }
                            catch { }
                            listOfRooms.Add(new Room()
                            {
                                roomId = reader.GetInt32(0),
                                roomName = reader.GetString(1),
                                roomDescription = description,
                                roomImage = image,
                                devices = GetDevices(reader.GetInt32(0))
                            });
                        }
                    }
                }
                connection.Close();
            }
            return listOfRooms;
        }
        public List<Device> GetDevices(int RoomId)
        {
            List<Device> listOfDevices = new List<Device>();
            using (MySqlConnection connection = new MySqlConnection(builder.ConnectionString))
            {
                connection.Open();

                String sql = "SELECT * FROM Devices WHERE roomId='"+RoomId+"';";

                using (MySqlCommand command = new MySqlCommand(sql, connection))
                {
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            listOfDevices.Add(new Device()
                            {
                                deviceId = reader.GetString(0),
                                deviceName = reader.GetString(1),
                                deviceType = reader.GetInt32(2),
                                deviceData = reader.GetString(3),
                                roomId = reader.GetInt32(4),
                                
                            });
                        }
                    }
                }
                connection.Close();
            }
            return listOfDevices;
        }
        public bool AddRoom(string? name, string? description, string? image, string? uuid)
        {
            string sql = "INSERT INTO Rooms (roomName, roomDescription, roomImage, uuid)VALUES" +
                    "('" + name + "','" + description + "','" + image + "','" + uuid + "')";
            return ExecuteShortSql(sql);
        }
        public bool RemoveRoom(int id)
        {
            return ExecuteShortSql("DELETE FROM Rooms WHERE roomId=" + id);
        }
        public bool SetDeviceData(string? deviceId, string? data)
        {
            String sql = "UPDATE Devices SET deviceData='"+data+"' WHERE deviceId='"+deviceId+"'";
            return ExecuteShortSql(sql);
        }
        public bool ExecuteShortSql(string sql)
        {
            using (MySqlConnection connection = new MySqlConnection(builder.ConnectionString))
            {
                connection.Open();
                using (MySqlCommand command = new MySqlCommand(sql, connection)) { }
                if ((new MySqlCommand(sql, connection)).ExecuteNonQuery() == 1)
                {
                    connection.Close();
                    return true;
                }
                connection.Close();
            }
            return false;
        }
        public bool UpdateDeviceRoom(string? deviceId, int roomId)
        {
            if (ExecuteShortSql("UPDATE Devices SET roomId="+roomId+" WHERE deviceId=\""+deviceId+"\""))
            {
                return true;
            }
            return false;
        }
    }
}
