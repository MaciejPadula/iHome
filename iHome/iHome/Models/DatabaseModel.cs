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
        public List<IDevice> GetDevices(int RoomId)
        {
            List<IDevice> listOfDevices = new List<IDevice>();
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
                            if (reader.GetInt32(4) == 1)
                            {
                                listOfDevices.Add(new TemperatureSensor());
                            }
                        }
                    }
                }
                connection.Close();
            }
            return listOfDevices;
        }
        public bool AddRoom(string name, string description, string image, string uuid)
        {
            using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
            {
                connection.Open();

                String sql = "INSERT INTO Rooms (RoomName, RoomDescription, RoomImage, UserId)VALUES" +
                    "('"+name+"','"+description+"','"+image+"','"+uuid+"')";

                using (SqlCommand command = new SqlCommand(sql, connection)) { }
                if((new SqlCommand(sql, connection)).ExecuteNonQuery() == 1)
                {
                    connection.Close();
                    return true;
                }
                connection.Close();
            }
            return false;
        }
        public bool RemoveRoom(int id)
        {
            using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
            {
                connection.Open();

                String sql = "DELETE FROM Rooms WHERE RoomId="+id;

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
