using MySql.Data.MySqlClient;
using SchedulingSoftware.Classes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SchedulingSoftware
{
    public partial class CustomerScreen : Form
    {
        public static CustomerScreen instance;

        public CustomerScreen()
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

        private void CustomerScreen_Activated(object sender, EventArgs e)
        {
            CustomersGrid.ClearSelection();
            EditCustomerButton.Enabled = false;
            DeleteCustomerButton.Enabled = false;
            PullFromDatabase();
        }      

        public void PullFromDatabase()
        {          
            if (DAO.connection != null)
            {
                // build select statement               
                string commandString = "SELECT customer.customerId, customer.customerName, address.address, address.phone, address.addressId" +
                                   "\r\nFROM customer" +
                                   "\r\nINNER JOIN address ON customer.addressId=address.addressId;";
                
                MySqlCommand command = new MySqlCommand(commandString, DAO.connection);

                // using a temp data table
                MySqlDataAdapter adapter = new MySqlDataAdapter(command);
                DataTable table = new DataTable();
                adapter.Fill(table);
                CustomersGrid.DataSource = table;
                CustomersGrid.Columns[0].HeaderText = "ID";               
                CustomersGrid.Columns[1].HeaderText = "Name";
                CustomersGrid.Columns[2].HeaderText = "Address";
                CustomersGrid.Columns[3].HeaderText = "Phone";
                CustomersGrid.Columns[4].HeaderText = "Address ID";
                CustomersGrid.Columns["addressId"].Visible = false;              

                #region using an object list               
                /*
                MySqlDataReader reader = command.ExecuteReader();

                // populate customerList with acquired data               
                DAO.customerList = new BindingList<Customer>();

                if (reader.HasRows)
                {                   
                    while (reader.Read())
                    {                      
                        DAO.customerList.Add(new Customer(reader.GetInt32(0),
                                                      reader.GetString(1),
                                                      reader.GetInt32(2),
                                                      reader.GetInt32(3),
                                                      reader.GetDateTime(4).ToLocalTime(),
                                                      reader.GetString(5),
                                                      reader.GetDateTime(6).ToLocalTime(),
                                                      reader.GetString(7)));
                    }
                }

                reader.Close();
                CustomersGrid.DataSource = DAO.customerList;
                */
                #endregion
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
            CustomersGrid.ClearSelection();          
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
      
        private void AddCustomerButton_Click(object sender, EventArgs e)
        {
            if (AddCustomerScreen.instance == null)
            {
                // initialize the AddCustomerScreen    
                AddCustomerScreen screen = new AddCustomerScreen();

                // hide the current screen
                this.Hide();

                // open the AddCustomerScreen           
                screen.ShowDialog();
            }
            else
            {
                // hide the current screen
                this.Hide();

                // open the AddCustomerScreen     
                AddCustomerScreen.instance.Show();
            }
        }

        private void EditCustomerButton_Click(object sender, EventArgs e)
        {
            if (EditCustomerScreen.instance == null)
            {
                // initialize the EditCustomerScreen    
                EditCustomerScreen screen = new EditCustomerScreen();

                // send data to EditCustomerScreen                
                string addressId = CustomersGrid.CurrentRow.Cells["addressId"].Value.ToString();
                string name = CustomersGrid.CurrentRow.Cells["customerName"].Value.ToString();
                string address = CustomersGrid.CurrentRow.Cells["address"].Value.ToString();
                string phone = CustomersGrid.CurrentRow.Cells["phone"].Value.ToString();               

                screen.PopulateFields(addressId, name, address, phone);

                // hide the current screen
                this.Hide();

                // open the EditCustomerScreen           
                screen.ShowDialog();
            }
            else
            {
                // send data to EditCustomerScreen            
                string addressId = CustomersGrid.CurrentRow.Cells["addressId"].Value.ToString();
                string name = CustomersGrid.CurrentRow.Cells["customerName"].Value.ToString();
                string address = CustomersGrid.CurrentRow.Cells["address"].Value.ToString();
                string phone = CustomersGrid.CurrentRow.Cells["phone"].Value.ToString();

                EditCustomerScreen.instance.PopulateFields(addressId, name, address, phone);

                // hide the current screen
                this.Hide();

                // open the EditCustomerScreen     
                EditCustomerScreen.instance.Show();
            }
        }
        
        private void DeleteCustomerButton_Click(object sender, EventArgs e)
        {
            // confirm deletion
            var confirmDeletion = MessageBox.Show("Are you sure you want to delete this customer?",
                                                  "Confirm", MessageBoxButtons.YesNo);
            if (confirmDeletion == DialogResult.Yes)
            {
                // get selected row
                string customerId = CustomersGrid.CurrentRow.Cells["customerId"].Value.ToString();
                string addressId = CustomersGrid.CurrentRow.Cells["addressId"].Value.ToString();

                // build delete statement    
                string newCommand = "SET FOREIGN_KEY_CHECKS = 0;" +
                                    "DELETE appointment" +
                                "\r\nFROM appointment" +
                                "\r\nWHERE appointment.customerId = '" + customerId + "';" +
                                    "DELETE customer, address" +
                                "\r\nFROM customer" +
                                "\r\nJOIN address" +
                                "\r\nON customer.addressId = address.addressId" +
                                "\r\nWHERE customer.addressId = '" + addressId + "';" +
                                    "SET FOREIGN_KEY_CHECKS = 1;";

                // delete customer from database  
                RemoveFromDatabase(newCommand);

                // refresh data grid view
                PullFromDatabase();

                // reset buttons
                EditCustomerButton.Enabled = false;
                DeleteCustomerButton.Enabled = false;
            }               
        }

        private void CustomersGrid_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (CustomersGrid.CurrentRow.Selected == true)
            {
                EditCustomerButton.Enabled = true;
                DeleteCustomerButton.Enabled = true;
            }
        }

        // for navigating the DataGridView with a keyboard
        private void CustomersGrid_KeyDown(object sender, KeyEventArgs e)
        {
            if (CustomersGrid.Rows.Count > 0)
            {
                if (CustomersGrid.CurrentCell == null || CustomersGrid.Rows.Count == 1)
                {
                    CustomersGrid.Rows[0].Selected = true;
                    CustomersGrid.CurrentCell = CustomersGrid.Rows[0].Cells[0];
                  
                    EditCustomerButton.Enabled = true;
                    DeleteCustomerButton.Enabled = true;                  
                }

                if (e.KeyCode == Keys.Enter)
                {
                    if (CustomersGrid.CurrentCell.RowIndex != CustomersGrid.Rows.Count - 1)
                    {
                        int rowIndex = CustomersGrid.CurrentRow.Index;
                        int columnIndex = CustomersGrid.CurrentCell.ColumnIndex;
                        CustomersGrid.Rows[rowIndex].Selected = true;
                        CustomersGrid.CurrentCell = CustomersGrid.Rows[rowIndex].Cells[columnIndex];
                                                
                        EditCustomerButton.Enabled = true;
                        DeleteCustomerButton.Enabled = true;                       

                        e.Handled = true;
                    }
                }
                else if (e.KeyCode == Keys.Up)
                {
                    if (CustomersGrid.CurrentCell.RowIndex > 0)
                    {
                        int rowIndex = CustomersGrid.CurrentRow.Index;
                        int columnIndex = CustomersGrid.CurrentCell.ColumnIndex;
                        CustomersGrid.Rows[rowIndex - 1].Selected = true;
                        CustomersGrid.CurrentCell = CustomersGrid.Rows[rowIndex - 1].Cells[columnIndex];
                                               
                        EditCustomerButton.Enabled = true;
                        DeleteCustomerButton.Enabled = true;                       

                        e.Handled = true;
                    }
                }
                else if (e.KeyCode == Keys.Down)
                {
                    if (CustomersGrid.CurrentCell.RowIndex < CustomersGrid.Rows.Count - 1)
                    {
                        int rowIndex = CustomersGrid.CurrentRow.Index;
                        int columnIndex = CustomersGrid.CurrentCell.ColumnIndex;
                        CustomersGrid.Rows[rowIndex + 1].Selected = true;
                        CustomersGrid.CurrentCell = CustomersGrid.Rows[rowIndex + 1].Cells[columnIndex];
                       
                        EditCustomerButton.Enabled = true;
                        DeleteCustomerButton.Enabled = true;                       
                        e.Handled = true;
                    }
                }
                else if (e.KeyCode == Keys.Tab)
                {
                    e.Handled = false;
                }
            }
        }       
    }
}
