using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Management.Instrumentation;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using MySqlX.XDevAPI.Relational;
using SchedulingSoftware.Classes;

namespace SchedulingSoftware
{
    public partial class HomeScreen : Form
    {
        public static HomeScreen instance;

        public static int closeCount = 0;
        public static int showCount = 0;

        public HomeScreen()
        {            
            // check if instance already exists before initializing            
            if (instance == null)
            {
                instance = this;
                InitializeComponent();                                     
            }
            else
            {               
                this.Dispose();
            }            
        }

        private void HomeScreen_Shown(object sender, EventArgs e)
        {            
            if (showCount == 0)
            {
                CheckForAppointment();
            }

            showCount++;
        }

        public void CheckForAppointment()
        {
            if (DAO.connection != null)
            {                
                // execute sql command             
                string commandString = "SELECT appointment.start, appointment.start FROM appointment";
                MySqlCommand command = new MySqlCommand(commandString, DAO.connection);
                command.ExecuteNonQuery();

                // populate data table
                MySqlDataAdapter adapter = new MySqlDataAdapter(command);
                DataTable table = new DataTable();               
                adapter.Fill(table);
              
                // sort table
                DataView view = table.DefaultView;
                view.Sort = "Start asc";
                DataTable sortedTable = view.ToTable();
               
                // check if appointments are within 15 minutes
                string tempDate = String.Empty; 
                string tempTime = String.Empty;
                string displayTime = String.Empty;
     
                for (int i = 0; i < sortedTable.Rows.Count; i++)
                {
                    for (int y = 0; y < sortedTable.Columns.Count; y++)
                    {
                        if (y == 0)
                        {
                            DateTime getDate = Convert.ToDateTime(sortedTable.Rows[i][y].ToString()).ToLocalTime();
                            tempDate = getDate.ToShortDateString();                            
                        }
                        else if (y == 1)
                        {
                            DateTime getDate = Convert.ToDateTime(sortedTable.Rows[i][y].ToString()).ToLocalTime();
                            var formatTime = getDate.ToString("yyyy-MM-dd H:mm");
                            string[] tempString = formatTime.ToString().Split(' ');
                            tempTime = tempString[1];

                            var formatDisplayTime = getDate.ToString("yyyy-MM-dd h:mm tt");
                            string[] tempDisplayString = formatDisplayTime.ToString().Split(' ');
                            displayTime = tempDisplayString[1] + " " + tempDisplayString[2];
                        }                        
                    }

                    // compare scheduled dates with current date                                 
                    if (tempDate == DateTime.Now.ToShortDateString())
                    {
                        string currentDate = DateTime.Now.ToString("yyyy-MM-dd H:mm");
                        string[] tempStringDate = currentDate.Split(' ');
                        string currentTime = tempStringDate[1];

                        double currentTimeInSeconds = TimeSpan.Parse(currentTime).TotalSeconds;
                        double tempTimeInSeconds = TimeSpan.Parse(tempTime).TotalSeconds;

                        var difference = tempTimeInSeconds - currentTimeInSeconds;

                        // compare scheduled times with current time
                        if ((difference <= 900) && (difference >= 0))
                        {                                                                                 
                            MessageBox.Show("You have an upcoming appointment at " + displayTime + ".", "Notice");                           
                            return;
                        }
                    }
                }
            }                                        
        }

        private void CustomerRecordsButton_Click(object sender, EventArgs e)
        {           
            if (CustomerScreen.instance == null)
            {
                // initialize the CustomerScreen
                CustomerScreen screen = new CustomerScreen();

                // hide the current screen
                this.Hide();

                // open the CustomerScreen           
                screen.ShowDialog();              
            }
            else
            {
                // hide the current screen
                this.Hide();

                // open the CustomerScreen     
                CustomerScreen.instance.Show();              
            }                   
        }

        private void AppointmentsButton_Click(object sender, EventArgs e)
        {
            if (AppointmentScreen.instance == null)
            {
                // initialize the AppointmentScreen    
                AppointmentScreen screen = new AppointmentScreen();

                // reset appointment filter
                screen.AppointmentFilter.Text = "All";

                // hide the current screen
                this.Hide();

                // open the AppointmentScreen     
                screen.ShowDialog();
            }
            else
            {
                // reset appointment filter
                AppointmentScreen.instance.AppointmentFilter.Text = "All";

                // hide the current screen
                this.Hide();

                // open the AppointmentScreen       
                AppointmentScreen.instance.Show();
            }            
        }

        private void ReportsButton_Click(object sender, EventArgs e)
        {
            if (ReportScreen.instance == null)
            {
                // initialize the ReportScreen    
                ReportScreen screen = new ReportScreen();

                // hide the current screen
                this.Hide();

                // open the ReportScreen     
                screen.ShowDialog();
            }
            else
            {
                // hide the current screen
                this.Hide();

                // open the ReportScreen      
                ReportScreen.instance.Show();
            }
        }

        private void HomeScreen_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.Visible == true)
            {
                if (closeCount == 0)
                {
                    var confirmClose = MessageBox.Show("Are you sure you want to close this application?",
                                                    "Confirm", MessageBoxButtons.YesNo);

                    if (confirmClose == DialogResult.No)
                    {
                        e.Cancel = true;
                    }
                    else
                    {
                        closeCount = 1;
                    }
                }

                if (closeCount == 1)
                {
                    Application.Exit();
                }
            }           
        }      
    }
}
