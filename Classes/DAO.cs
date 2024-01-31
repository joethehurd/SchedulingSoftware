using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace SchedulingSoftware.Classes
{
    public class DAO
    {
        public static BindingList<string> customerList = new BindingList<string>();      
        public static MySqlConnection connection { get; set; }

        public static void OpenConnection()
        {
            // establish database connection  
            string connectionString = ConfigurationManager.ConnectionStrings["virtualdb"].ConnectionString;
                    
            try
            {
                connection = new MySqlConnection(connectionString);
                connection.Open();                
            }
            catch (MySqlException error)
            {
                MessageBox.Show(error.Message);
            }
        }

        public static void CloseConnection()
        {
            try
            {
                if (connection != null)
                {
                    connection.Close();
                }
            }
            catch (MySqlException error)
            {
                MessageBox.Show(error.Message);
            }
        }
    }
}
