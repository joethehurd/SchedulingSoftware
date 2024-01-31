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
    public partial class ReportScreen : Form
    {
        public static ReportScreen instance;

        public ReportScreen()
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

        private void ReportOneButton_Click(object sender, EventArgs e)
        {
            if (ReportOneScreen.instance == null)
            {
                // initialize the ReportScreenOne    
                ReportOneScreen screen = new ReportOneScreen();

                // populate fields             
                screen.MonthDD.Text = "All";

                // hide the current screen
                this.Hide();

                // open the ReportScreenOne     
                screen.ShowDialog();
            }
            else
            {
                // populate fields               
                ReportOneScreen.instance.MonthDD.Text = "All";

                // hide the current screen
                this.Hide();

                // open the ReportScreenOne      
                ReportOneScreen.instance.Show();
            }
        }

        private void ReportTwoButton_Click(object sender, EventArgs e)
        {
            if (ReportTwoScreen.instance == null)
            {
                // initialize the ReportScreenTwo    
                ReportTwoScreen screen = new ReportTwoScreen();

                // hide the current screen
                this.Hide();

                // open the ReportScreenTwo     
                screen.ShowDialog();
            }
            else
            {
                // hide the current screen
                this.Hide();

                // open the ReportScreenTwo      
                ReportTwoScreen.instance.Show();
            }
        }

        private void ReportThreeButton_Click(object sender, EventArgs e)
        {
            // populate customer list
            string commandString = "SELECT customerName FROM customer;";
            MySqlCommand command = new MySqlCommand(commandString, DAO.connection);
            MySqlDataReader reader = command.ExecuteReader();
            DAO.customerList = new BindingList<string>();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    DAO.customerList.Add(reader.GetString(0));
                }
            }

            reader.Close();

            if (ReportThreeScreen.instance == null)
            {
                // initialize the ReportScreenThree    
                ReportThreeScreen screen = new ReportThreeScreen();

                // populate fields             
                screen.MonthDD.Text = "All";
                screen.CustomerDD.DataSource = DAO.customerList;

                bool isEmpty = !DAO.customerList.Any();
                if (isEmpty)
                {
                    screen.CustomerDD.Text = String.Empty;
                }

                // hide the current screen
                this.Hide();

                // open the ReportScreenThree     
                screen.ShowDialog();
            }
            else
            {
                // populate fields             
                ReportThreeScreen.instance.MonthDD.Text = "All";
                ReportThreeScreen.instance.CustomerDD.DataSource = DAO.customerList;

                bool isEmpty = !DAO.customerList.Any();
                if (isEmpty)
                {
                    ReportThreeScreen.instance.CustomerDD.Text = String.Empty;
                }

                // hide the current screen
                this.Hide();

                // open the ReportScreenThree      
                ReportThreeScreen.instance.Show();
            }
        }
    }
}
