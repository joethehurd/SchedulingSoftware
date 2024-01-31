using MySql.Data.MySqlClient;
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
    public partial class ReportOneScreen : Form
    {
        public static ReportOneScreen instance;

        public ReportOneScreen()
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

        private void ReportOneScreen_Activated(object sender, EventArgs e)
        {
            ReportOneGrid.ClearSelection();
            PullFromDatabase();
        }      

        public void PullFromDatabase()
        {
            if (DAO.connection != null)
            {
                // make sql query
                string commandString = String.Empty;              
                int month = 0;

                switch (MonthDD.Text)
                {
                    case "All":
                        month = 0;
                        break;
                    case "January":
                        month = 1;
                        break;
                    case "February":
                        month = 2;
                        break;
                    case "March":
                        month = 3;
                        break;
                    case "April":
                        month = 4;
                        break;
                    case "May":
                        month = 5;
                        break;
                    case "June":
                        month = 6;
                        break;
                    case "July":
                        month = 7;
                        break;
                    case "August":
                        month = 8;
                        break;
                    case "September":
                        month = 9;
                        break;
                    case "October":
                        month = 10;
                        break;
                    case "November":
                        month = 11;
                        break;
                    case "December":
                        month = 12;
                        break;
                    default:
                        month = 0;
                        break;
                }

                if (month != 0)
                {                    
                    commandString = "SELECT appointment.type FROM appointment" +
                                "\r\nWHERE MONTH(appointment.start) = '" + month + "';";
                }
                else
                {                    
                    commandString = "SELECT appointment.type FROM appointment;";
                }

                MySqlCommand command = new MySqlCommand(commandString, DAO.connection);

                // using a temp data table
                MySqlDataAdapter adapter = new MySqlDataAdapter(command);
                DataTable table = new DataTable();
                adapter.Fill(table);

                // reformat data table data types
                DataTable newTable = new DataTable();
                newTable.Columns.Add("Month", typeof(string));
                newTable.Columns.Add("Presentation", typeof(int));
                newTable.Columns.Add("Scrum", typeof(int));
                newTable.Rows.Add();
                
                int presentationCount = 0;
                int scrumCount = 0;

                for (int i = 0; i < table.Rows.Count; i++)
                {
                    if (table.Rows[i][0].ToString() == "Presentation")
                    {
                        presentationCount++;
                    }
                    else if (table.Rows[i][0].ToString() == "Scrum")
                    {
                        scrumCount++;
                    }
                }

                newTable.Rows[0][0] = MonthDD.Text;
                newTable.Rows[0][1] = presentationCount;
                newTable.Rows[0][2] = scrumCount;

                ReportOneGrid.DataSource = newTable;

                // resize columns
                for (int i = 0; i < ReportOneGrid.Columns.Count; i++)
                {
                    DataGridViewColumn column = ReportOneGrid.Columns[i];
                    column.Width = ReportOneGrid.Width / ReportOneGrid.Columns.Count;
                }             
            }
        }

        private void ReportOneGrid_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            ReportOneGrid.ClearSelection();
        }

        private void MonthDD_SelectedValueChanged(object sender, EventArgs e)
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
