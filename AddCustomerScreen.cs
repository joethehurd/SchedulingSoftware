using MySql.Data.MySqlClient;
using SchedulingSoftware.Classes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SchedulingSoftware
{
    public partial class AddCustomerScreen : Form
    {
        public static AddCustomerScreen instance;
     
        public AddCustomerScreen()
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
       
        private void AddCustomerScreen_VisibleChanged(object sender, EventArgs e)
        {
            if (this.Visible == true)
            {
                // reset fields when form is shown
                AddCustomerName.Text = String.Empty;
                AddCustomerAddress.Text = String.Empty;
                AddCustomerPhone.Text = String.Empty;                
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
            // check that customer does not already exist
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

            foreach (string customer in DAO.customerList)
            {
                if (AddCustomerName.Text.ToLower() == customer.ToLower())
                {
                    MessageBox.Show("Customer already exists.", "Alert");
                    return;
                }
            }

            // format text
            string tempName = AddCustomerName.Text.ToLower();
            AddCustomerName.Text = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(tempName);

            string tempAddress = AddCustomerAddress.Text.ToLower();
            AddCustomerAddress.Text = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(tempAddress);

            // build insert statement
            string newCommand = string.Empty;
            if (CustomerScreen.instance == null)
            {                
                CustomerScreen screen = new CustomerScreen();

                if (screen.CustomersGrid.Rows.Count == 0)
                {
                    // build insert statement (first entry)     
                    newCommand = "SET FOREIGN_KEY_CHECKS = 0;\r\nDROP TABLE address;\r\nDROP TABLE customer;\r\nSET FOREIGN_KEY_CHECKS = 1;\r\n\r\nCREATE TABLE address\r\n (\r\n addressId INT(10),\r\n address VARCHAR(50),\r\n address2 VARCHAR(50),\r\n cityId INT(10),\r\n postalCode VARCHAR(10),\r\n phone VARCHAR(20),\r\n createDate DATETIME,\r\n createdBy VARCHAR(40),\r\n lastUpdate TIMESTAMP,\r\n lastUpdateBy VARCHAR(40),\r\n PRIMARY KEY (addressID),\r\n FOREIGN KEY (cityID) REFERENCES city(cityID)\r\n );\r\n\r\n CREATE TABLE customer\r\n (\r\n customerId INT(10),\r\n customerName VARCHAR(45),\r\n addressId INT(10),\r\n active INT(1),\r\n createDate DATETIME,\r\n createdBy VARCHAR(40),\r\n lastUpdate TIMESTAMP,\r\n lastUpdateBy VARCHAR(40),\r\n PRIMARY KEY (customerID),\r\n FOREIGN KEY (addressID) REFERENCES address(addressID)\r\n );" +
                                 "INSERT INTO address (addressID, address, address2, cityId, postalCode, phone, createDate, createdBy, lastUpdate, lastUpdateBy)" +
                             "\r\nVALUES \r\n(1, '" + AddCustomerAddress.Text + "', 'not needed', 1, 0, '" + AddCustomerPhone.Text + "', 0, 'not needed', 0, 'not needed');" +                                 
                             "\r\nINSERT INTO customer (customerID, customerName, addressID, active, createDate, createdBy, lastUpdate, lastUpdateBy)" +
                             "\r\nVALUES \r\n(1, '" + AddCustomerName.Text + "', 1, 0, 0, 'not needed', 0, 'not needed');";
                }
                else
                {
                    // build insert statement (auto-increment)      
                    newCommand = "INSERT INTO address (addressID, address, address2, cityId, postalCode, phone, createDate, createdBy, lastUpdate, lastUpdateBy)" +
                                    "\r\nVALUES \r\n((SELECT MAX( addressID )+1 FROM address a), '" + AddCustomerAddress.Text + "', 'not needed', 1, 0, '" + AddCustomerPhone.Text + "', 0, 'not needed', 0, 'not needed');" +
                                    "\r\nSELECT @AddressID := MAX(addressID) FROM address;" +
                                    "\r\nINSERT INTO customer (customerID, customerName, addressID, active, createDate, createdBy, lastUpdate, lastUpdateBy)" +
                                    "\r\nVALUES \r\n((SELECT MAX( customerID )+1 FROM customer c), '" + AddCustomerName.Text + "', @AddressID, 0, 0, 'not needed', 0, 'not needed');";
                }
            }
            else
            {
                if (CustomerScreen.instance.CustomersGrid.Rows.Count == 0)
                {
                    // build insert statement (first entry)     
                    newCommand = "SET FOREIGN_KEY_CHECKS = 0;\r\nDROP TABLE address;\r\nDROP TABLE customer;\r\nSET FOREIGN_KEY_CHECKS = 1;\r\n\r\nCREATE TABLE address\r\n (\r\n addressId INT(10),\r\n address VARCHAR(50),\r\n address2 VARCHAR(50),\r\n cityId INT(10),\r\n postalCode VARCHAR(10),\r\n phone VARCHAR(20),\r\n createDate DATETIME,\r\n createdBy VARCHAR(40),\r\n lastUpdate TIMESTAMP,\r\n lastUpdateBy VARCHAR(40),\r\n PRIMARY KEY (addressID),\r\n FOREIGN KEY (cityID) REFERENCES city(cityID)\r\n );\r\n\r\n CREATE TABLE customer\r\n (\r\n customerId INT(10),\r\n customerName VARCHAR(45),\r\n addressId INT(10),\r\n active INT(1),\r\n createDate DATETIME,\r\n createdBy VARCHAR(40),\r\n lastUpdate TIMESTAMP,\r\n lastUpdateBy VARCHAR(40),\r\n PRIMARY KEY (customerID),\r\n FOREIGN KEY (addressID) REFERENCES address(addressID)\r\n );" +
                                 "INSERT INTO address (addressID, address, address2, cityId, postalCode, phone, createDate, createdBy, lastUpdate, lastUpdateBy)" +
                             "\r\nVALUES \r\n(1, '" + AddCustomerAddress.Text + "', 'not needed', 1, 0, '" + AddCustomerPhone.Text + "', 0, 'not needed', 0, 'not needed');" +
                             "\r\nINSERT INTO customer (customerID, customerName, addressID, active, createDate, createdBy, lastUpdate, lastUpdateBy)" +
                             "\r\nVALUES \r\n(1, '" + AddCustomerName.Text + "', 1, 0, 0, 'not needed', 0, 'not needed');";
                }
                else
                {
                    // build insert statement (auto-increment)       
                    newCommand = "INSERT INTO address (addressID, address, address2, cityId, postalCode, phone, createDate, createdBy, lastUpdate, lastUpdateBy)" +
                                    "\r\nVALUES \r\n((SELECT MAX( addressID )+1 FROM address a), '" + AddCustomerAddress.Text + "', 'not needed', 1, 0, '" + AddCustomerPhone.Text + "', 0, 'not needed', 0, 'not needed');" +
                                    "\r\nSELECT @AddressID := MAX(addressID) FROM address;" +
                                    "\r\nINSERT INTO customer (customerID, customerName, addressID, active, createDate, createdBy, lastUpdate, lastUpdateBy)" +
                                    "\r\nVALUES \r\n((SELECT MAX( customerID )+1 FROM customer c), '" + AddCustomerName.Text + "', @AddressID, 0, 0, 'not needed', 0, 'not needed');";
                }
            }                    

            // add new customer to database
            if (!String.IsNullOrEmpty(newCommand))
            {
                PushToDatabase(newCommand);
            }
            
            // return to CustomerScreen
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

        private void CancelCustomerButton_Click(object sender, EventArgs e)
        {
            // return to CustomerScreen
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

        private void ValidateSave(object sender, EventArgs e)
        {
            List<string> tempList = new List<string>()
            {
                AddCustomerName.Text,
                AddCustomerAddress.Text,
                AddCustomerPhone.Text
            };

            foreach (string tempString in tempList)
            {
                if (string.IsNullOrEmpty(tempString))
                {
                    // a form is blank
                    SaveCustomerButton.Enabled = false;
                    break;
                }
                else
                {
                    if (AddCustomerPhone.Text.Length < 8)
                    {
                        // phone number is incomplete
                        SaveCustomerButton.Enabled = false;
                        break;
                    }
                    else
                    {
                        // form is ready to be saved
                        SaveCustomerButton.Enabled = true;
                    }
                }
            }
        }        

        private void AddCustomerName_KeyPress(object sender, KeyPressEventArgs e)
        {           
            // check length
            if (AddCustomerName.Text.Length >= 45)
            {
                // accept backspacing
                if (e.KeyChar == (char)8)
                {
                    e.Handled = false;
                }
                else
                {
                    e.Handled = true;
                }
            }
            else
            {
                // accept letters
                if (!char.IsLetter(e.KeyChar))
                {
                    e.Handled = true;
                }
                // accept backspacing
                if (e.KeyChar == (char)8)
                {
                    e.Handled = false;
                }
                // accept spacing
                if (e.KeyChar == (char)Keys.Space)
                {
                    e.Handled = false;
                }
            }           
        }

        private void AddCustomerAddress_KeyPress(object sender, KeyPressEventArgs e)
        {
            // check length
            if (AddCustomerAddress.Text.Length >= 20)
            {
                // accept backspacing
                if (e.KeyChar == (char)8)
                {
                    e.Handled = false;
                }
                else
                {
                    e.Handled = true;
                }
            }
            else
            {
                // accept letters
                if (!char.IsLetter(e.KeyChar))
                {
                    // accept numbers
                    if (char.IsDigit(e.KeyChar))
                    {
                        e.Handled = false;
                    }
                    else
                    {
                        e.Handled = true;
                    }                  
                }               
                // accept backspacing
                if (e.KeyChar == (char)8)
                {
                    e.Handled = false;
                }
                // accept spacing
                if (e.KeyChar == (char)Keys.Space)
                {
                    e.Handled = false;
                }
            }          
        }

        private void AddCustomerPhone_KeyPress(object sender, KeyPressEventArgs e)
        {
            bool backspacing = false;

            // check length
            if (AddCustomerPhone.Text.Length >= 8)
            {
                // accept backspacing
                if (e.KeyChar == (char)8)
                {
                    backspacing = true;
                    e.Handled = false;
                }
                else
                {                 
                    e.Handled = true;
                }              
            }
            else
            {             
                // accept numbers
                if (!char.IsDigit(e.KeyChar))
                {
                    e.Handled = true;
                }
                // accept backspacing
                if (e.KeyChar == (char)8)
                {
                    backspacing = true;
                    e.Handled = false;
                }
                // format text
                if (backspacing == false)
                {                   
                    if (AddCustomerPhone.Text.Length == 3)
                    {
                        if (!AddCustomerPhone.Text.Contains('-'))
                        {
                            AddCustomerPhone.Text += '-';
                            AddCustomerPhone.SelectionStart = 4;
                        }
                    }
                }
            }          
        }       
    }
}
