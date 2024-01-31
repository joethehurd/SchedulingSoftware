using MySql.Data.MySqlClient;
using MySqlX.XDevAPI.Relational;
using SchedulingSoftware.Classes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Management.Instrumentation;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SchedulingSoftware
{
    public partial class AddAppointmentScreen : Form
    {
        public static AddAppointmentScreen instance;

        public AddAppointmentScreen()
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
        
        public void PushToDatabase(string newCommand)
        {
            if (DAO.connection != null)
            {
                // execute sql command             
                string commandString = newCommand;
                MySqlCommand command = new MySqlCommand(commandString, DAO.connection);
                command.ExecuteNonQuery();
            }
        }

        private void SaveCustomerButton_Click(object sender, EventArgs e)
        {        
            // check that a customer has been selected
            if (String.IsNullOrEmpty(CustomerNamesDD.Text))
            {
                MessageBox.Show("A Customer must be selected.", "Alert");
                return;
            }

            // check that start time is before end time
            if (StartTimePicker.Value >= EndTimePicker.Value)
            {
                MessageBox.Show("End Time must be after Start Time.", "Alert");
                return;
            }          

            // get date times
            DateTime tempStartDate = DatePicker.Value + StartTimePicker.Value.TimeOfDay;
            DateTime tempEndDate = DatePicker.Value + EndTimePicker.Value.TimeOfDay;
           
            // check that scheduled time is within business hours
            TimeSpan openingTime = new TimeSpan(06, 00, 00);
            DateTime openingHours = DatePicker.Value + openingTime;
            TimeSpan closingTime = new TimeSpan(22, 00, 00);
            DateTime closingHours = DatePicker.Value + closingTime;           

            if (tempStartDate < openingHours || tempStartDate > closingHours)
            {
                MessageBox.Show("Appointment must be within business hours: 6:00 AM - 10:00 PM", "Alert");
                return;
            }
            if (tempEndDate < openingHours || tempEndDate > closingHours)
            {
                MessageBox.Show("Appointment must be within business hours: 6:00 AM - 10:00 PM", "Alert");
                return;
            }

            // check that appointment is not in the past
            DateTime selectedTime = new DateTime(tempStartDate.Year, tempStartDate.Month, tempStartDate.Day, tempStartDate.Hour, tempStartDate.Minute, 0, tempStartDate.Kind);
            DateTime tempTime = DateTime.Now;
            DateTime currentTime = new DateTime(tempTime.Year, tempTime.Month, tempTime.Day, tempTime.Hour, tempTime.Minute, 0, tempTime.Kind);          
            
            if (selectedTime < currentTime)
            {                
                MessageBox.Show("Appointments cannot be scheduled in the past.", "Alert");
                return;
            }

            // check that appointments do not overlap
            DataTable tempTable = new DataTable();
            
            if (AppointmentScreen.instance == null)
            {
                // initialize the AppointmentScreen
                AppointmentScreen screen = new AppointmentScreen();

                tempTable = screen.appointmentsTable;
            }
            else
            {
                tempTable = AppointmentScreen.instance.appointmentsTable;               
            }

            for (int i = 0; i < tempTable.Rows.Count; i++)
            {
                DateTime existingStartTime = (DateTime)tempTable.Rows[i]["start"];            
                DateTime existingEndTime = (DateTime)tempTable.Rows[i]["end"];
                existingStartTime = existingStartTime.ToLocalTime();
                existingEndTime = existingEndTime.ToLocalTime();
               
                if (tempStartDate <= existingStartTime)
                {
                    if (tempStartDate == existingStartTime)
                    {
                        MessageBox.Show("Date and Time must not overlap with existing appointments.", "Alert");
                        return;
                    }
                    else if (tempEndDate > existingStartTime)
                    {
                        MessageBox.Show("Date and Time must not overlap with existing appointments.", "Alert");
                        return;
                    }
                }
                else if (tempStartDate >= existingStartTime)
                {
                    if (tempStartDate == existingStartTime)
                    {
                        MessageBox.Show("Date and Time must not overlap with existing appointments.", "Alert");
                        return;
                    }
                    else if (tempStartDate < existingEndTime)
                    {
                        MessageBox.Show("Date and Time must not overlap with existing appointments.", "Alert");
                        return;
                    }
                }
            }
           
            // set date times
            var startDate = tempStartDate.ToUniversalTime().ToString("yyyy-MM-dd HH:mm:ss"); 
            var endDate = tempEndDate.ToUniversalTime().ToString("yyyy-MM-dd HH:mm:ss"); 

            // set appointment type
            string type = String.Empty;
            if (RadioButtonPresentation.Checked == true)
            {
                type = "Presentation";                
            }
            else
            {
                type = "Scrum";               
            }

            // build insert statement
            string newCommand = string.Empty;

            if (AppointmentScreen.instance == null)
            {
                AppointmentScreen screen = new AppointmentScreen();

                if (screen.AppointmentsGrid.Rows.Count == 0)
                {
                    // build insert statement (first entry)     
                    newCommand = "SET FOREIGN_KEY_CHECKS = 0;\r\nDROP TABLE appointment;\r\nSET FOREIGN_KEY_CHECKS = 1;\r\n\r\nCREATE TABLE appointment\r\n(\r\nappointmentId INT(10),\r\ncustomerId INT(10),\r\nuserId INT,\r\ntitle VARCHAR (255),\r\ndescription TEXT,\r\nlocation TEXT,\r\ncontact TEXT,\r\ntype TEXT,\r\nurl VARCHAR(255),\r\nstart DATETIME,\r\nend DATETIME, \r\ncreateDate DATETIME,\r\ncreatedBy VARCHAR(40),\r\nlastUpdate TIMESTAMP,\r\nlastUpdateBy VARCHAR(40),\r\nPRIMARY KEY (appointmentID),\r\nFOREIGN KEY (customerID) REFERENCES customer(customerID),\r\nFOREIGN KEY (userID) REFERENCES user(userID)\r\n);" +
                         "\r\n\r\nINSERT INTO appointment (appointmentId, customerId, userId, title, description, location, contact, type, url, start, end, createDate, createdBy, lastUpdate, lastUpdateBy)" +
                             "\r\nVALUES \r\n(1, (SELECT ( customerId ) FROM customer c WHERE c.customerName = '" + CustomerNamesDD.Text + "'), 1, 'not needed', 'not needed', 'not needed', 'not needed', '" + type + "', 'not needed', '" + startDate + "', '" + endDate + "', 0, 'not needed', 0, 'not needed');\r\n";
                }
                else
                {
                    // build insert statement (auto-increment)      
                    newCommand = "INSERT INTO appointment (appointmentId, customerId, userId, title, description, location, contact, type, url, start, end, createDate, createdBy, lastUpdate, lastUpdateBy)" +
                             "\r\nVALUES \r\n((SELECT MAX( appointmentId )+1 FROM appointment a), (SELECT ( customerId ) FROM customer c WHERE c.customerName =  '" + CustomerNamesDD.Text + "'), 1, 'not needed', 'not needed', 'not needed', 'not needed',  '" + type + "', 'not needed',  '" + startDate + "',  '" + endDate + "', 0, 'not needed', 0, 'not needed');";
                }
            }
            else
            {
                if (AppointmentScreen.instance.AppointmentsGrid.Rows.Count == 0)
                {
                    // build insert statement (first entry)     
                    newCommand = "SET FOREIGN_KEY_CHECKS = 0;\r\nDROP TABLE appointment;\r\nSET FOREIGN_KEY_CHECKS = 1;\r\n\r\nCREATE TABLE appointment\r\n(\r\nappointmentId INT(10),\r\ncustomerId INT(10),\r\nuserId INT,\r\ntitle VARCHAR (255),\r\ndescription TEXT,\r\nlocation TEXT,\r\ncontact TEXT,\r\ntype TEXT,\r\nurl VARCHAR(255),\r\nstart DATETIME,\r\nend DATETIME, \r\ncreateDate DATETIME,\r\ncreatedBy VARCHAR(40),\r\nlastUpdate TIMESTAMP,\r\nlastUpdateBy VARCHAR(40),\r\nPRIMARY KEY (appointmentID),\r\nFOREIGN KEY (customerID) REFERENCES customer(customerID),\r\nFOREIGN KEY (userID) REFERENCES user(userID)\r\n);" +
                         "\r\n\r\nINSERT INTO appointment (appointmentId, customerId, userId, title, description, location, contact, type, url, start, end, createDate, createdBy, lastUpdate, lastUpdateBy)" +
                             "\r\nVALUES \r\n(1, (SELECT ( customerId ) FROM customer c WHERE c.customerName = '" + CustomerNamesDD.Text + "'), 1, 'not needed', 'not needed', 'not needed', 'not needed', '" + type + "', 'not needed', '" + startDate + "', '" + endDate + "', 0, 'not needed', 0, 'not needed');\r\n";
                }
                else
                {
                    // build insert statement (auto-increment)       
                    newCommand = "INSERT INTO appointment (appointmentId, customerId, userId, title, description, location, contact, type, url, start, end, createDate, createdBy, lastUpdate, lastUpdateBy)" +
                             "\r\nVALUES \r\n((SELECT MAX( appointmentId )+1 FROM appointment a), (SELECT ( customerId ) FROM customer c WHERE c.customerName =  '" + CustomerNamesDD.Text + "'), 1, 'not needed', 'not needed', 'not needed', 'not needed',  '" + type + "', 'not needed',  '" + startDate + "',  '" + endDate + "', 0, 'not needed', 0, 'not needed');";
                }
            }

            // add new customer to database
            if (!String.IsNullOrEmpty(newCommand))
            {
                PushToDatabase(newCommand);
            }
                        
            // return to AppointmentScreen
            if (AppointmentScreen.instance == null)
            {
                // initialize the AppointmentScreen
                AppointmentScreen screen = new AppointmentScreen();

                // hide the current screen
                this.Hide();

                // open the AppointmentScreen           
                screen.ShowDialog();
            }
            else
            {
                // hide the current screen
                this.Hide();

                // open the AppointmentScreen     
                AppointmentScreen.instance.Show();
            }            
        }

        private void CancelCustomerButton_Click(object sender, EventArgs e)
        {
            // return to AppointmentScreen
            if (AppointmentScreen.instance == null)
            {
                // initialize the AppointmentScreen
                AppointmentScreen screen = new AppointmentScreen();

                // hide the current screen
                this.Hide();

                // open the AppointmentScreen           
                screen.ShowDialog();
            }
            else
            {
                // hide the current screen
                this.Hide();

                // open the AppointmentScreen     
                AppointmentScreen.instance.Show();
            }
        }

     

      
    }
}
