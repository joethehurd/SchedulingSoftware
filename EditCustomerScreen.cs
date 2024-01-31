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
    public partial class EditCustomerScreen : Form
    {
        public static EditCustomerScreen instance;
         
        public string addressID;
        public string selectedName;

        public EditCustomerScreen()
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

        public void PopulateFields(string addressId, string name, string address, string phone)
        {           
            addressID = addressId;
            selectedName = name;
            EditCustomerName.Text = name;
            EditCustomerAddress.Text = address;
            EditCustomerPhone.Text = phone;
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
                if (EditCustomerName.Text.ToLower() != selectedName.ToLower())
                {
                    if (EditCustomerName.Text.ToLower() == customer.ToLower())
                    {
                        MessageBox.Show("Customer already exists.", "Alert");
                        return;
                    }
                }                               
            }

            // format text
            string tempName = EditCustomerName.Text.ToLower();
            EditCustomerName.Text = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(tempName);

            string tempAddress = EditCustomerAddress.Text.ToLower();
            EditCustomerAddress.Text = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(tempAddress);
          
            // build update statement
            string newCommand = "UPDATE customer, address" +
                "\r\nSET customer.customerName = '" + EditCustomerName.Text + "', address.address = '" + EditCustomerAddress.Text + "', address.phone = '" + EditCustomerPhone.Text + "'" +
                "\r\nWHERE customer.addressId = address.addressId\r\nAND address.addressId = '" + addressID + "';";
           
            // update customer in database  
            PushToDatabase(newCommand);

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
                EditCustomerName.Text,
                EditCustomerAddress.Text,
                EditCustomerPhone.Text
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
                    if (EditCustomerPhone.Text.Length < 8)
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

        private void EditCustomerName_KeyPress(object sender, KeyPressEventArgs e)
        {
            // check length
            if (EditCustomerName.Text.Length >= 45)
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

        private void EditCustomerAddress_KeyPress(object sender, KeyPressEventArgs e)
        {
            // check length
            if (EditCustomerAddress.Text.Length >= 20)
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

        private void EditCustomerPhone_KeyPress(object sender, KeyPressEventArgs e)
        {
            bool backspacing = false;

            // check length
            if (EditCustomerPhone.Text.Length >= 8)
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
                    if (EditCustomerPhone.Text.Length == 3)
                    {
                        if (!EditCustomerPhone.Text.Contains('-'))
                        {
                            EditCustomerPhone.Text += '-';
                            EditCustomerPhone.SelectionStart = 4;
                        }
                    }
                }
            }
        }             
    }
}
