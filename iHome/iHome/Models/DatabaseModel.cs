using iHome.Models.iHomeComponents;
using System.Data.SqlClient;

namespace iHome.Models
{
    public class DatabaseModel
    {
        SqlConnectionStringBuilder builder;
        public DatabaseModel(string server, string login, string password, string db)
        {
            builder = new SqlConnectionStringBuilder();
            builder.DataSource = server;
            builder.UserID = login;
            builder.Password = password;
            builder.InitialCatalog = db;
        }
        public List<Room> GetRooms(string uuid)
        {
            List<Room> listOfRooms = new List<Room>();
            using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
            {
                connection.Open();

                String sql = "SELECT RoomId, RoomName, RoomDescription, RoomImage FROM Rooms WHERE UserId = '"+uuid+"'";

                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
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
                            listOfRooms.Add(new Room(
                                reader.GetInt32(0),
                                reader.GetString(1), 
                                description,
                                image,
                                GetDevices(reader.GetInt32(0))
                            ));
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
            using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
            {
                connection.Open();

                String sql = "SELECT * FROM [dbo].[Devices] WHERE RoomId='"+RoomId+"';";

                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            listOfDevices.Add(new Device()
                            {
                                deviceId = reader.GetString(0),
                                name = reader.GetString(1),
                                data = reader.GetString(2),
                                roomId = reader.GetInt32(3),
                                type = reader.GetInt32(5),
                            });
                        }
                    }
                }
                connection.Close();
            }
            return listOfDevices;
        }
        public bool AddRoom(string name, string description, string image, string uuid)
        {
            String sql = "INSERT INTO Rooms (RoomName, RoomDescription, RoomImage, UserId)VALUES" +
                    "('" + name + "','" + description + "','" + image + "','" + uuid + "')";
            return ExecuteShortSql(sql);
        }
        public bool RemoveRoom(int id)
        {
            return ExecuteShortSql("DELETE FROM Rooms WHERE RoomId=" + id);
        }

        public bool ExecuteShortSql(string sql)
        {
            using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(sql, connection)) { }
                if ((new SqlCommand(sql, connection)).ExecuteNonQuery() == 1)
                {
                    connection.Close();
                    return true;
                }
                connection.Close();
            }
            return false;
        }
    }
}
