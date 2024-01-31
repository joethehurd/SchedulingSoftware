using MySqlX.XDevAPI;
using SchedulingSoftware.Classes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using MySql.Data.MySqlClient;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace SchedulingSoftware
{
    public partial class LoginScreen : Form
    {        
        public RegionInfo region;
        public DataTable users;
        public string exception;
        public string caption;       

        public LoginScreen()
        {           
            InitializeComponent();
            CheckLanguage();
            PullFromDatabase();
        }

        public void CheckLanguage()
        {
            region = RegionInfo.CurrentRegion;           

            if (region.ToString() == "ES")
            {
                this.Text = "Iniciar Sesión";
                UserLoginLabel.Text = "Inicio de Sesión de Usuario";
                UsernameLabel.Text = "Nombre de Usuario";
                PasswordLabel.Text = "Contraseña";
                LoginButton.Text = "Iniciar Sesión";
                exception = "Credenciales de inicio de sesión no válidas.";
                caption = "Alerta";
            }
            else
            {
                this.Text = "Log-In";
                UserLoginLabel.Text = "User Log-In";
                UsernameLabel.Text = "Username";
                PasswordLabel.Text = "Password";
                LoginButton.Text = "Log-In";
                exception = "Invalid log-in credentials.";
                caption = "Alert";
            }         
        }     

        public void PullFromDatabase()
        {
            if (DAO.connection != null)
            {
                // execute sql command             
                string commandString = "SELECT user.userName, user.password FROM user";
                MySqlCommand command = new MySqlCommand(commandString, DAO.connection);
                command.ExecuteNonQuery();

                // populate data table
                MySqlDataAdapter adapter = new MySqlDataAdapter(command);
                users = new DataTable();
                adapter.Fill(users);
            }
        }

        // lamdba expression consolidates code and increases readability where called
        Action<Form, string> GrantAccess = (currentForm, user) =>
        {
            // record log-in activity to text file 
            if (!File.Exists("Activity_Log.txt"))
            {
                File.Create("Activity_Log.txt").Close();

                using (StreamWriter sw = File.AppendText("Activity_Log.txt"))
                {
                    sw.WriteLine("User <" + user + "> successfully logged in at <" + DateTime.Now.ToString("MM/dd/yyyy h:mm tt") + ">");
                }
            }
            else
            {
                using (StreamWriter sw = File.AppendText("Activity_Log.txt"))
                {
                    sw.WriteLine("User <" + user + "> successfully logged in at <" + DateTime.Now.ToString("MM/dd/yyyy h:mm tt") + ">");
                }
            }

            // open home page
            if (HomeScreen.instance == null)
            {
                // initialize the HomeScreen
                HomeScreen screen = new HomeScreen();

                // hide the current screen
                currentForm.Hide();

                // open the HomeScreen     
                screen.ShowDialog();
            }
            else
            {
                // hide the current screen
                currentForm.Hide();

                // open the HomeScreen       
                HomeScreen.instance.Show();
            }
        };
        
        // lamdba expression consolidates code and increases readability where called
        Action<TextBox, TextBox, string, string> DenyAccess = (usernameField, passwordField, user, ex) =>
        {
            // record log-in activity to text file 
            if (!File.Exists("Activity_Log.txt"))
            {
                File.Create("Activity_Log.txt").Close();

                using (StreamWriter sw = File.AppendText("Activity_Log.txt"))
                {
                    sw.WriteLine("User <" + user + "> failed to log in at <" + DateTime.Now.ToString("MM/dd/yyyy h:mm tt") + ">");
                }
            }
            else
            {
                using (StreamWriter sw = File.AppendText("Activity_Log.txt"))
                {
                    sw.WriteLine("User <" + user + "> failed to log in at <" + DateTime.Now.ToString("MM/dd/yyyy h:mm tt") + ">");
                }
            }

            // reset log-in fields
            usernameField.Text = String.Empty;
            passwordField.Text = String.Empty;

            //throw exception
            throw new InvalidLoginException(ex);
        };

        private void ValidateFields(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(EnterUsername.Text) && !String.IsNullOrEmpty(EnterPassword.Text))
            {
                LoginButton.Enabled = true;
            }
            else
            {
                LoginButton.Enabled = false;
            }
        }      

        public void ValidateLogin(string username, string password)
        {           
            for (int i = 0; i < users.Rows.Count; i++)
            {
                string tempUsername = users.Rows[i][0].ToString();
                string tempPassword = users.Rows[i][1].ToString();

                if (username == tempUsername && password == tempPassword)
                {
                    // log in successful                                
                    GrantAccess(this, username);                                     
                }
                else
                {
                    if (i == users.Rows.Count - 1)
                    {
                        // log in failed
                        DenyAccess(EnterUsername, EnterPassword, username, exception);
                    }                  
                }
            }           
        }

        public void AttemptLogin()
        {
            string username = EnterUsername.Text;
            string password = EnterPassword.Text;

            try
            {
                ValidateLogin(username, password);
            }
            catch(InvalidLoginException ex)
            {
                MessageBox.Show(ex.Message, caption);              
            }
        }

        private void LoginButton_Click(object sender, EventArgs e)
        {
            AttemptLogin();
        }
       
        private void EnterUsername_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {                
                e.Handled = true;

                if (!String.IsNullOrEmpty(EnterUsername.Text) && !String.IsNullOrEmpty(EnterPassword.Text))
                {

                    AttemptLogin();
                }
            }
        }

        private void EnterPassword_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {               
                e.Handled = true;

                if (!String.IsNullOrEmpty(EnterUsername.Text) && !String.IsNullOrEmpty(EnterPassword.Text))
                {
                    AttemptLogin();
                }
            }
        }
    }   
}
