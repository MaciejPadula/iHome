using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iHomeAzureFunctions.Models
{
    internal class DatabaseModel
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
        public bool SetDeviceData(string deviceId, string deviceData)
        {
            String sql = "UPDATE Devices SET deviceData='"+deviceData+"' WHERE deviceId='"+deviceId+"';";
            return ExecuteShortSql(sql);
        }
        public dynamic GetDeviceData(string deviceId)
        {
            dynamic data=null;
            using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
            {
                connection.Open();

                String sql = "SELECT deviceData FROM Devices WHERE deviceId = '" + deviceId + "'";

                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            data = JsonConvert.DeserializeObject(reader.GetString(0));
                        }
                    }
                }
                connection.Close();
            }
            return data;
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
