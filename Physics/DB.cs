using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Physics
{
    public class DB
    {
        MySqlConnection connection = new MySqlConnection("server=localhost;port=3306;username=root;password=root;database=2048");

        public void openConnection()
        {
            if (connection.State == System.Data.ConnectionState.Closed);
            connection.Open();
        }
        public void closeConnection()
        {
            if (connection.State == System.Data.ConnectionState.Open) ;
            connection.Close();
        }

        public MySqlConnection getConnection()
        {
            return connection;
        }




        public bool IsConnectionOpen()
        {
            return (connection.State == ConnectionState.Open);
        }

        public bool Login(string username, string password)
        {
            try
            {
                openConnection();

                // Предположим, что у вас есть таблица "users" с колонками "username" и "password"
                string query = "SELECT COUNT(*) FROM users WHERE username=@username AND password=@password";
                MySqlCommand cmd = new MySqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@username", username);
                cmd.Parameters.AddWithValue("@password", password);

                int count = Convert.ToInt32(cmd.ExecuteScalar());

                return count > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
            finally
            {
                closeConnection();
            }
        }
    }
}
