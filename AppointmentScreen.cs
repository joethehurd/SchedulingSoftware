using MySql.Data.MySqlClient;
using MySqlX.XDevAPI.Relational;
using SchedulingSoftware.Classes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Drawing;
using System.Linq;
using System.Management.Instrumentation;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SchedulingSoftware
{
    public partial class AppointmentScreen : Form
    {
        public static AppointmentScreen instance;
        
        public DataTable appointmentsTable;

        public AppointmentScreen()
        {
            // check if instance already exists before initializing            
            if (instance == null)
            {
                instance = this;
                InitializeComponent();
                PullFromDatabase();
            }
            else
            {
                this.Dispose();
            }
        }

        private void AppointmentScreen_VisibleChanged(object sender, EventArgs e)
        {
            if (this.Visible == true)
            {
                AppointmentsGrid.ClearSelection();
                EditAppointmentButton.Enabled = false;
                DeleteAppointmentButton.Enabled = false;

                PullFromDatabase();
            }
        }

        public void PullFromDatabase()
        {
            if (DAO.connection != null)
            {
                // make sql query
                string commandString = String.Empty;

                if (AppointmentFilter.Text == "All")
                {
                    commandString = "SELECT customer.customerName, appointment.type, appointment.start, appointment.start, appointment.end, appointment.appointmentId, user.userName\r\nFROM customer\r\nINNER JOIN appointment\r\nON customer.customerId=appointment.customerId\r\nINNER JOIN user\r\nON appointment.userId=user.userId;";
                }
                else if (AppointmentFilter.Text == "Today")
                {
                    commandString = "SELECT customer.customerName, appointment.type, appointment.start, appointment.start, appointment.end, appointment.appointmentId, user.userName\r\nFROM customer\r\nINNER JOIN appointment\r\nON customer.customerId=appointment.customerId\r\nINNER JOIN user\r\nON appointment.userId=user.userId\r\nWHERE DATE(appointment.start)=CURDATE();";
                }
                else if (AppointmentFilter.Text == "Current Week")
                {
                    commandString = "SELECT customer.customerName, appointment.type, appointment.start, appointment.start, appointment.end, appointment.appointmentId, user.userName\r\nFROM customer\r\nINNER JOIN appointment\r\nON customer.customerId=appointment.customerId\r\nINNER JOIN user\r\nON appointment.userId=user.userId\r\nWHERE YEARWEEK(appointment.start)=YEARWEEK(NOW());";
                }
                else if (AppointmentFilter.Text == "Current Month")
                {
                    commandString = "SELECT customer.customerName, appointment.type, appointment.start, appointment.start, appointment.end, appointment.appointmentId, user.userName\r\nFROM customer\r\nINNER JOIN appointment\r\nON customer.customerId=appointment.customerId\r\nINNER JOIN user\r\nON appointment.userId=user.userId\r\nWHERE MONTH(appointment.start)=MONTH(NOW())\r\nAND YEAR(appointment.start)=YEAR(NOW());";
                }
                else
                {
                    AppointmentFilter.Text = "All";
                    commandString = "SELECT customer.customerName, appointment.type, appointment.start, appointment.start, appointment.end, appointment.appointmentId, user.userName\r\nFROM customer\r\nINNER JOIN appointment\r\nON customer.customerId=appointment.customerId\r\nINNER JOIN user\r\nON appointment.userId=user.userId;";
                }

                MySqlCommand command = new MySqlCommand(commandString, DAO.connection);

                // using a temp data table
                MySqlDataAdapter adapter = new MySqlDataAdapter(command);
                appointmentsTable = new DataTable();
                adapter.Fill(appointmentsTable);
                AppointmentsGrid.DataSource = appointmentsTable;              
                AppointmentsGrid.Columns[0].HeaderText = "Customer";
                AppointmentsGrid.Columns[1].HeaderText = "Type";
                AppointmentsGrid.Columns[2].HeaderText = "Date";
                AppointmentsGrid.Columns[3].HeaderText = "Start Time"; 
                AppointmentsGrid.Columns[4].HeaderText = "End Time";
                AppointmentsGrid.Columns[5].HeaderText = "ID";
                AppointmentsGrid.Columns[6].HeaderText = "User";               

                // reformat data table data types
                DataTable newTable = new DataTable();              
                newTable.Columns.Add("Customer", typeof(string));
                newTable.Columns.Add("Type", typeof(string));
                newTable.Columns.Add("Date", typeof(string));
                newTable.Columns.Add("Start Time", typeof(string));
                newTable.Columns.Add("End Time", typeof(string));
                newTable.Columns.Add("ID", typeof(string));
                newTable.Columns.Add("User", typeof(string));

                for (int i = 0; i < AppointmentsGrid.Rows.Count; i++)
                {
                    newTable.Rows.Add();

                    for (int y = 0; y < AppointmentsGrid.Columns.Count; y++)
                    {
                        if (AppointmentsGrid.Columns[y].HeaderText != "Date" && AppointmentsGrid.Columns[y].HeaderText != "Start Time" && AppointmentsGrid.Columns[y].HeaderText != "End Time")
                        {
                            newTable.Rows[i][y] = appointmentsTable.Rows[i][y];
                        }
                        else if (AppointmentsGrid.Columns[y].HeaderText == "Date")
                        {                           
                            string tempCell = appointmentsTable.Rows[i][y].ToString();                           
                            DateTime tempDate = Convert.ToDateTime(tempCell).ToLocalTime();
                            string newCell = tempDate.ToShortDateString();
                            newTable.Rows[i][y] = newCell;                           
                        }
                        else if (AppointmentsGrid.Columns[y].HeaderText == "Start Time")
                        {                            
                            string tempCell = appointmentsTable.Rows[i][y].ToString();
                            DateTime tempDate = Convert.ToDateTime(tempCell).ToLocalTime();
                            string [] tempString = tempDate.ToString("yyyy-MM-dd h:mm tt").Split(' ');
                            string newCell = tempString[1] + ' ' + tempString[2];
                            newTable.Rows[i][y] = newCell;                
                        }
                        else if (AppointmentsGrid.Columns[y].HeaderText == "End Time")
                        {
                            string tempCell = appointmentsTable.Rows[i][y].ToString();                            
                            DateTime tempDate = Convert.ToDateTime(tempCell).ToLocalTime();
                            string[] tempString = tempDate.ToString("yyyy-MM-dd h:mm tt").Split(' ');
                            string newCell = tempString[1] + ' ' + tempString[2];
                            newTable.Rows[i][y] = newCell;
                        }
                    }
                }

                newTable.DefaultView.Sort = "ID asc";
                AppointmentsGrid.DataSource = newTable;
                //AppointmentsGrid.Columns["ID"].Visible = false;
                //AppointmentsGrid.Columns["User"].Visible = false;

                // populate customer list for add appointment combo box
                string secondCommandString = "SELECT customerName FROM customer;";
                MySqlCommand secondCommand = new MySqlCommand(secondCommandString, DAO.connection);
                MySqlDataReader reader = secondCommand.ExecuteReader();                         
                DAO.customerList = new BindingList<string>();
              
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        DAO.customerList.Add(reader.GetString(0));
                    }
                }

                reader.Close();              
            }
        }

        public void RemoveFromDatabase(string newCommand)
        {
            if (DAO.connection != null)
            {
                // execute sql command             
                string commandString = newCommand;
                MySqlCommand command = new MySqlCommand(commandString, DAO.connection);
                command.ExecuteNonQuery();
            }
        }

        private void BindingIsComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            AppointmentsGrid.ClearSelection();
        }

        private void MenuButton_Click(object sender, EventArgs e)
        {
            if (HomeScreen.instance == null)
            {
                // initialize the HomeScreen
                HomeScreen screen = new HomeScreen();

                // hide the current screen
                this.Hide();

                // open the HomeScreen     
                screen.ShowDialog();
            }
            else
            {
                // hide the current screen
                this.Hide();

                // open the HomeScreen       
                HomeScreen.instance.Show();
            }
        }

        private void AddAppointmentButton_Click(object sender, EventArgs e)
        {
            if (AddAppointmentScreen.instance == null)
            {
                // initialize the AddAppointmentScreen    
                AddAppointmentScreen screen = new AddAppointmentScreen();
               
                // populate combo box
                screen.CustomerNamesDD.DataSource = DAO.customerList;

                // format radio buttons
                screen.RadioButtonPresentation.Checked = true;

                // format date pickers                
                var currentHour = DateTime.Now.Hour;

                if (currentHour >= 22)
                {
                    screen.DatePicker.Value = DateTime.Today.AddDays(1);

                    TimeSpan openingTime = new TimeSpan(06, 00, 00);              
                    screen.StartTimePicker.Value = screen.DatePicker.Value + openingTime;
                }
                else
                {
                    screen.DatePicker.Value = DateTime.Today;

                    screen.StartTimePicker.Value = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd h:mm tt"));
                }                

                TimeSpan closingTime = new TimeSpan(22, 00, 00);
                screen.EndTimePicker.Value = screen.DatePicker.Value + closingTime;                                

                // hide the current screen
                this.Hide();

                // open the AddAppointmentScreen           
                screen.ShowDialog();
            }
            else
            {              
                // populate combo box
                AddAppointmentScreen.instance.CustomerNamesDD.DataSource = DAO.customerList;

                // format radio buttons
                AddAppointmentScreen.instance.RadioButtonPresentation.Checked = true;

                // format date pickers               
                var currentHour = DateTime.Now.Hour;

                if (currentHour >= 22)
                {
                    AddAppointmentScreen.instance.DatePicker.Value = DateTime.Today.AddDays(1);

                    TimeSpan openingTime = new TimeSpan(06, 00, 00);                             
                    AddAppointmentScreen.instance.StartTimePicker.Value = AddAppointmentScreen.instance.DatePicker.Value + openingTime;
                }
                else
                {
                    AddAppointmentScreen.instance.DatePicker.Value = DateTime.Today;
                    AddAppointmentScreen.instance.StartTimePicker.Value = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd h:mm tt"));
                }                               

                TimeSpan closingTime = new TimeSpan(22, 00, 00);
                AddAppointmentScreen.instance.EndTimePicker.Value = AddAppointmentScreen.instance.DatePicker.Value + closingTime;                

                // hide the current screen
                this.Hide();

                // open the AddAppointmentScreen     
                AddAppointmentScreen.instance.Show();
            }
        }

        private void EditAppointmentButton_Click(object sender, EventArgs e)
        {
            
            if (EditAppointmentScreen.instance == null)
            {
                // initialize the EditAppointmentScreen    
                EditAppointmentScreen screen = new EditAppointmentScreen();

                // populate combo box
                screen.CustomerNamesDD.DataSource = DAO.customerList;

                // send data to EditAppointmentScreen
                string appointmentId = AppointmentsGrid.CurrentRow.Cells["ID"].Value.ToString();
                string name = AppointmentsGrid.CurrentRow.Cells["Customer"].Value.ToString();
                string type = AppointmentsGrid.CurrentRow.Cells["Type"].Value.ToString();
                string date = AppointmentsGrid.CurrentRow.Cells["Date"].Value.ToString();
                string start = AppointmentsGrid.CurrentRow.Cells["Start Time"].Value.ToString();
                string end = AppointmentsGrid.CurrentRow.Cells["End Time"].Value.ToString();

                screen.PopulateFields(appointmentId, name, type, date, start, end);              

                // hide the current screen
                this.Hide();

                // open the EditAppointmentScreen           
                screen.ShowDialog();
            }
            else
            {
                // populate combo box
                EditAppointmentScreen.instance.CustomerNamesDD.DataSource = DAO.customerList;

                // send data to EditAppointmentScreen
                string appointmentId = AppointmentsGrid.CurrentRow.Cells["ID"].Value.ToString();
                string name = AppointmentsGrid.CurrentRow.Cells["Customer"].Value.ToString();
                string type = AppointmentsGrid.CurrentRow.Cells["Type"].Value.ToString();
                string date = AppointmentsGrid.CurrentRow.Cells["Date"].Value.ToString();
                string start = AppointmentsGrid.CurrentRow.Cells["Start Time"].Value.ToString();
                string end = AppointmentsGrid.CurrentRow.Cells["End Time"].Value.ToString();

                EditAppointmentScreen.instance.PopulateFields(appointmentId, name, type, date, start, end);               

                // hide the current screen
                this.Hide();

                // open the EditAppointmentScreen     
                EditAppointmentScreen.instance.Show();
            }            
        }

        private void DeleteAppointmentButton_Click(object sender, EventArgs e)
        {
            
            // confirm deletion
            var confirmDeletion = MessageBox.Show("Are you sure you want to delete this appointment?",
                                                  "Confirm", MessageBoxButtons.YesNo);
            if (confirmDeletion == DialogResult.Yes)
            {
                // get selected row
                string appointmentId = AppointmentsGrid.CurrentRow.Cells["ID"].Value.ToString();

                // build delete statement    
                string newCommand = "SET FOREIGN_KEY_CHECKS = 0;" +
                                    "DELETE appointment" +
                                "\r\nFROM appointment" +                              
                                "\r\nWHERE appointmentId = '" + appointmentId + "';" +
                                    "SET FOREIGN_KEY_CHECKS = 1;";

                // delete Appointment from database  
                RemoveFromDatabase(newCommand);

                // refresh data grid view
                PullFromDatabase();

                // reset buttons
                EditAppointmentButton.Enabled = false;
                DeleteAppointmentButton.Enabled = false;
            }            
        }

        private void AppointmentsGrid_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (AppointmentsGrid.CurrentRow.Selected == true)
            {
                EditAppointmentButton.Enabled = true;
                DeleteAppointmentButton.Enabled = true;
            }
        }

        // for navigating the DataGridView with a keyboard
        private void AppointmentsGrid_KeyDown(object sender, KeyEventArgs e)
        {
            if (AppointmentsGrid.Rows.Count > 0)
            {
                if (AppointmentsGrid.CurrentCell == null || AppointmentsGrid.Rows.Count == 1)
                {
                    AppointmentsGrid.Rows[0].Selected = true;
                    AppointmentsGrid.CurrentCell = AppointmentsGrid.Rows[0].Cells[0];

                    EditAppointmentButton.Enabled = true;
                    DeleteAppointmentButton.Enabled = true;
                }

                if (e.KeyCode == Keys.Enter)
                {
                    if (AppointmentsGrid.CurrentCell.RowIndex != AppointmentsGrid.Rows.Count - 1)
                    {
                        int rowIndex = AppointmentsGrid.CurrentRow.Index;
                        int columnIndex = AppointmentsGrid.CurrentCell.ColumnIndex;
                        AppointmentsGrid.Rows[rowIndex].Selected = true;
                        AppointmentsGrid.CurrentCell = AppointmentsGrid.Rows[rowIndex].Cells[columnIndex];

                        EditAppointmentButton.Enabled = true;
                        DeleteAppointmentButton.Enabled = true;

                        e.Handled = true;
                    }
                }
                else if (e.KeyCode == Keys.Up)
                {
                    if (AppointmentsGrid.CurrentCell.RowIndex > 0)
                    {
                        int rowIndex = AppointmentsGrid.CurrentRow.Index;
                        int columnIndex = AppointmentsGrid.CurrentCell.ColumnIndex;
                        AppointmentsGrid.Rows[rowIndex - 1].Selected = true;
                        AppointmentsGrid.CurrentCell = AppointmentsGrid.Rows[rowIndex - 1].Cells[columnIndex];

                        EditAppointmentButton.Enabled = true;
                        DeleteAppointmentButton.Enabled = true;

                        e.Handled = true;
                    }
                }
                else if (e.KeyCode == Keys.Down)
                {
                    if (AppointmentsGrid.CurrentCell.RowIndex < AppointmentsGrid.Rows.Count - 1)
                    {
                        int rowIndex = AppointmentsGrid.CurrentRow.Index;
                        int columnIndex = AppointmentsGrid.CurrentCell.ColumnIndex;
                        AppointmentsGrid.Rows[rowIndex + 1].Selected = true;
                        AppointmentsGrid.CurrentCell = AppointmentsGrid.Rows[rowIndex + 1].Cells[columnIndex];

                        EditAppointmentButton.Enabled = true;
                        DeleteAppointmentButton.Enabled = true;
                        e.Handled = true;
                    }
                }
                else if (e.KeyCode == Keys.Tab)
                {                    
                    e.Handled = false;
                }
            }
        }   

        private void AppointmentFilter_SelectedValueChanged(object sender, EventArgs e)
        {
            PullFromDatabase();
        }
    }
}
