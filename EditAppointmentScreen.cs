using MySql.Data.MySqlClient;
using SchedulingSoftware.Classes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SchedulingSoftware
{
    public partial class EditAppointmentScreen : Form
    {
        public static EditAppointmentScreen instance;
        public string appointmentId;

        public EditAppointmentScreen()
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
      
        public void PopulateFields(string ID, string name, string type, string date, string start, string end)
        {
            appointmentId = ID;
            CustomerNamesDD.Text = name;
            if (type == "Presentation")
            {
                RadioButtonPresentation.Checked = true;
            }
            else
            {
                RadioButtonScrum.Checked = true;
            }
            DatePicker.Value = Convert.ToDateTime(date);
            StartTimePicker.Value = Convert.ToDateTime(start);
            EndTimePicker.Value = Convert.ToDateTime(end);          
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
                int tempId = (int)tempTable.Rows[i]["appointmentId"];
                DateTime existingStartTime = (DateTime)tempTable.Rows[i]["start"];
                DateTime existingEndTime = (DateTime)tempTable.Rows[i]["end"];
                existingStartTime = existingStartTime.ToLocalTime();
                existingEndTime = existingEndTime.ToLocalTime();             

                if (appointmentId != tempId.ToString())
                {
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

            // build update statement
            string newCommand = string.Empty;

            if (AppointmentScreen.instance == null)
            {
                AppointmentScreen screen = new AppointmentScreen();

                newCommand = "UPDATE appointment\r\n" +
                             "SET appointment.customerID=(SELECT customer.customerID FROM customer WHERE customer.customerName = '" + CustomerNamesDD.Text + "'), appointment.type = '" + type + "', appointment.start = '" + startDate + "', appointment.end = '" + endDate + "'" +
                         "\r\nWHERE appointment.appointmentId = " + appointmentId + ";";               
            }
            else
            {
                newCommand = "UPDATE appointment\r\n" +
                              "SET appointment.customerID=(SELECT customer.customerID FROM customer WHERE customer.customerName = '" + CustomerNamesDD.Text + "'), appointment.type = '" + type + "', appointment.start = '" + startDate + "', appointment.end = '" + endDate + "'" +
                          "\r\nWHERE appointment.appointmentId = " + appointmentId + ";";
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
