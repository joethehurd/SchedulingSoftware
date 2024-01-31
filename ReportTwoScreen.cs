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
    public partial class ReportTwoScreen : Form
    {
        public static ReportTwoScreen instance;

        public DataTable appointmentsTable;

        public ReportTwoScreen()
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

        private void ReportTwoScreen_Activated(object sender, EventArgs e)
        {
            UserDD.Text = "test";
            ReportTwoGrid.ClearSelection();
            PullFromDatabase();
        }      

        public void PullFromDatabase()
        {
            if (DAO.connection != null)
            {
                // make sql query
                string commandString = String.Empty;                

                if (AppointmentFilter.Text == "All")
                {
                    commandString = "SELECT user.userName, appointment.appointmentId, customer.customerName, appointment.type, appointment.start, appointment.start, appointment.end\r\nFROM customer\r\nINNER JOIN appointment\r\nON customer.customerId=appointment.customerId\r\nINNER JOIN user\r\nON appointment.userId=user.userId;";
                }
                else if (AppointmentFilter.Text == "Today")
                {
                    commandString = "SELECT user.userName, appointment.appointmentId, customer.customerName, appointment.type, appointment.start, appointment.start, appointment.end\r\nFROM customer\r\nINNER JOIN appointment\r\nON customer.customerId=appointment.customerId\r\nINNER JOIN user\r\nON appointment.userId=user.userId\r\nWHERE DATE(appointment.start)=CURDATE();";
                }
                else if (AppointmentFilter.Text == "Current Week")
                {
                    commandString = "SELECT user.userName, appointment.appointmentId, customer.customerName, appointment.type, appointment.start, appointment.start, appointment.end\r\nFROM customer\r\nINNER JOIN appointment\r\nON customer.customerId=appointment.customerId\r\nINNER JOIN user\r\nON appointment.userId=user.userId\r\nWHERE YEARWEEK(appointment.start)=YEARWEEK(NOW());";
                }
                else if (AppointmentFilter.Text == "Current Month")
                {
                    commandString = "SELECT user.userName, appointment.appointmentId, customer.customerName, appointment.type, appointment.start, appointment.start, appointment.end\r\nFROM customer\r\nINNER JOIN appointment\r\nON customer.customerId=appointment.customerId\r\nINNER JOIN user\r\nON appointment.userId=user.userId\r\nWHERE MONTH(appointment.start)=MONTH(NOW())\r\nAND YEAR(appointment.start)=YEAR(NOW());";
                }
                else
                {
                    AppointmentFilter.Text = "All";
                    commandString = "SELECT user.userName, appointment.appointmentId, customer.customerName, appointment.type, appointment.start, appointment.start, appointment.end\r\nFROM customer\r\nINNER JOIN appointment\r\nON customer.customerId=appointment.customerId\r\nINNER JOIN user\r\nON appointment.userId=user.userId;";
                }

                MySqlCommand command = new MySqlCommand(commandString, DAO.connection);

                // using a temp data table
                MySqlDataAdapter adapter = new MySqlDataAdapter(command);
                appointmentsTable = new DataTable();
                adapter.Fill(appointmentsTable);
                ReportTwoGrid.DataSource = appointmentsTable;
                ReportTwoGrid.Columns[0].HeaderText = "User";
                ReportTwoGrid.Columns[1].HeaderText = "ID";
                ReportTwoGrid.Columns[2].HeaderText = "Customer";
                ReportTwoGrid.Columns[3].HeaderText = "Type";
                ReportTwoGrid.Columns[4].HeaderText = "Date";
                ReportTwoGrid.Columns[5].HeaderText = "Start Time";
                ReportTwoGrid.Columns[6].HeaderText = "End Time";
                
                // reformat data table data types
                DataTable newTable = new DataTable();
                newTable.Columns.Add("User", typeof(string));
                newTable.Columns.Add("ID", typeof(string));
                newTable.Columns.Add("Customer", typeof(string));
                newTable.Columns.Add("Type", typeof(string));
                newTable.Columns.Add("Date", typeof(string));
                newTable.Columns.Add("Start Time", typeof(string));
                newTable.Columns.Add("End Time", typeof(string));
                
                for (int i = 0; i < ReportTwoGrid.Rows.Count; i++)
                {
                    newTable.Rows.Add();

                    for (int y = 0; y < ReportTwoGrid.Columns.Count; y++)
                    {
                        if (ReportTwoGrid.Columns[y].HeaderText != "Date" && ReportTwoGrid.Columns[y].HeaderText != "Start Time" && ReportTwoGrid.Columns[y].HeaderText != "End Time")
                        {
                            newTable.Rows[i][y] = appointmentsTable.Rows[i][y];
                        }
                        else if (ReportTwoGrid.Columns[y].HeaderText == "Date")
                        {
                            string tempCell = appointmentsTable.Rows[i][y].ToString();
                            DateTime tempDate = Convert.ToDateTime(tempCell).ToLocalTime();
                            string newCell = tempDate.ToShortDateString();
                            newTable.Rows[i][y] = newCell;
                        }
                        else if (ReportTwoGrid.Columns[y].HeaderText == "Start Time")
                        {
                            string tempCell = appointmentsTable.Rows[i][y].ToString();
                            DateTime tempDate = Convert.ToDateTime(tempCell).ToLocalTime();
                            string[] tempString = tempDate.ToString("yyyy-MM-dd h:mm tt").Split(' ');
                            string newCell = tempString[1] + ' ' + tempString[2];
                            newTable.Rows[i][y] = newCell;
                        }
                        else if (ReportTwoGrid.Columns[y].HeaderText == "End Time")
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
                ReportTwoGrid.DataSource = newTable;
                ReportTwoGrid.Columns["ID"].Visible = false;               

                // resize columns
                for (int i = 0; i < ReportTwoGrid.Columns.Count; i++)
                {
                    DataGridViewColumn column = ReportTwoGrid.Columns[i];
                    column.Width = ReportTwoGrid.Width / ReportTwoGrid.Columns.Count;
                }

                ReportTwoGrid.Columns[ReportTwoGrid.Columns.Count - 1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            }
        }

        private void ReportTwoGrid_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            ReportTwoGrid.ClearSelection();
        }

        private void UserDD_SelectedValueChanged(object sender, EventArgs e)
        {
            PullFromDatabase();
        }

        private void AppointmentFilter_SelectedValueChanged(object sender, EventArgs e)
        {
            PullFromDatabase();
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
    }
}
