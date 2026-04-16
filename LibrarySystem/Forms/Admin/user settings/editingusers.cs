using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using MySql.Data.MySqlClient;

namespace LibrarySystem.Forms.Admin.user_settings
{
    public partial class editingusers : Form
    {
        public editingusers()
        {
            InitializeComponent();
        }

        private int selectedUserId; // Private variable to store the ID
        string connectionString = "Server=localhost;Database=librarydb;User ID=root;Password=bhaibhai123;";
        private void usersBindingNavigatorSaveItem_Click(object sender, EventArgs e)
        {
            this.Validate();
            this.usersBindingSource.EndEdit();
            this.tableAdapterManager.UpdateAll(this.librarySystemDBDataSet);

        }

        private void editingusers_Load(object sender, EventArgs e)
        {
            LoadUserData();

        }

        private void LoadUserData()
        {
            try
            {
                using (MySqlConnection connect = new MySqlConnection(connectionString))
                {
                    string query = "SELECT ID, `FirstName`, `LastName`, Password, AdminPerm, IsBlocked,Phone,Address, Email FROM Users";
                    MySqlDataAdapter adapter = new MySqlDataAdapter(query, connect);
                    System.Data.DataTable table = new System.Data.DataTable();
                    adapter.Fill(table);

                    UserDataGrid.DataSource = table; // Assuming a DataGridView for user display
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading data: " + ex.Message);
            }
        }

    

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
            LibrarySystem.Forms.Admin.UserSettings.UserEdittingOptions open = new UserSettings.UserEdittingOptions();
            open.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                if (selectedUserId > 0)
                {
                    using (MySqlConnection connect = new MySqlConnection(connectionString))
                    {
                        string query = "DELETE FROM Users WHERE ID = @ID";

                        using (MySqlCommand cmd = new MySqlCommand(query, connect))
                        {
                            cmd.Parameters.AddWithValue("@ID", selectedUserId);

                            connect.Open();
                            cmd.ExecuteNonQuery();
                            MessageBox.Show("User has been successfully deleted.");

                            ClearTextBoxes();
                            LoadUserData(); // Refresh the data
                        }
                    }
                }
                else
                {
                    MessageBox.Show("No user selected for deletion.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message);
            }
        }

        private void UserDataGrid_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                // Get the selected row
                DataGridViewRow row = UserDataGrid.Rows[e.RowIndex];

                // Store ID in the private variable (hidden from user)
                selectedUserId = Convert.ToInt32(row.Cells["ID"].Value);

                // Populate textboxes with the selected user's details
                first_NameTextBox.Text = row.Cells["FirstName"].Value.ToString();
                last_NameTextBox.Text = row.Cells["LastName"].Value.ToString();
                passwordTextBox.Text = row.Cells["Password"].Value.ToString();
                phoneTextBox.Text = row.Cells["Phone"].Value.ToString();
                addressTextBox.Text = row.Cells["Address"].Value.ToString();
                emailTextBox.Text = row.Cells["Email"].Value.ToString();

                // Update the state of checkboxes based on the database values
                adminCheckBox.Checked = row.Cells["AdminPerm"].Value.ToString().ToLower() == "true";
                isBlockedCheckBox.Checked = row.Cells["IsBlocked"].Value.ToString().ToLower() == "true";
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                if (IsValidInput())
                {
                    using (MySqlConnection connect = new MySqlConnection(connectionString))
                    {
                        string query = @"UPDATE Users 
                                 SET `FirstName` = @FirstName, 
                                     `LastName` = @LastName, 
                                     Password = @Password, 
                                     Phone = @Phone, 
                                     Address = @Address, 
                                     Email = @Email,
                                     AdminPerm = @AdminPerm,
                                     IsBlocked = @IsBlocked
                                 WHERE ID = @ID";

                        using (MySqlCommand cmd = new MySqlCommand(query, connect))
                        {
                            cmd.Parameters.AddWithValue("@ID", selectedUserId);
                            cmd.Parameters.AddWithValue("@FirstName", first_NameTextBox.Text);
                            cmd.Parameters.AddWithValue("@LastName", last_NameTextBox.Text);
                            cmd.Parameters.AddWithValue("@Password", passwordTextBox.Text);
                            cmd.Parameters.AddWithValue("@Phone", phoneTextBox.Text);
                            cmd.Parameters.AddWithValue("@Address", addressTextBox.Text);
                            cmd.Parameters.AddWithValue("@Email", emailTextBox.Text);

                            // Convert checkbox values to "true" or "false"
                            cmd.Parameters.AddWithValue("@AdminPerm", adminCheckBox.Checked ? "true" : "false");
                            cmd.Parameters.AddWithValue("@IsBlocked", isBlockedCheckBox.Checked ? "true" : "false");

                            connect.Open();
                            cmd.ExecuteNonQuery();
                            MessageBox.Show("User has been successfully updated.");

                            ClearTextBoxes();
                            LoadUserData(); // Refresh the data
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Empty fields are not allowed! Please fill all fields.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message);
            }
        }

        private bool IsValidInput()
        {
            return !string.IsNullOrWhiteSpace(first_NameTextBox.Text) &&
                   !string.IsNullOrWhiteSpace(last_NameTextBox.Text) &&
                   !string.IsNullOrWhiteSpace(passwordTextBox.Text) &&
                   !string.IsNullOrWhiteSpace(phoneTextBox.Text) &&
                   !string.IsNullOrWhiteSpace(addressTextBox.Text) &&
                   !string.IsNullOrWhiteSpace(emailTextBox.Text);
        }

        private void ClearTextBoxes()
        {
            first_NameTextBox.Clear();
            last_NameTextBox.Clear();
            passwordTextBox.Clear();
            phoneTextBox.Clear();
            addressTextBox.Clear();
            emailTextBox.Clear();
            adminCheckBox.Checked = false;
            isBlockedCheckBox.Checked = false;
            selectedUserId = 0; // Reset selected ID
        }

        private void last_NameTextBox_TextChanged(object sender, EventArgs e)
        {

        }

        private void phoneTextBox_TextChanged(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
           
                string searchValue = searchTextBox.Text.Trim();

                try
                {
                string connectionString = "Server=localhost;Database=librarydb;User ID=root;Password=bhaibhai123;";
                using (MySqlConnection connect = new MySqlConnection(connectionString))
                    {
                        // Query to search across multiple columns
                        string query = @"
                SELECT ID, FirstName, LastName, Password, AdminPerm, IsBlocked, Phone, Address, Email 
                FROM Users
                WHERE 
                    FirstName LIKE @search OR 
                    LastName LIKE @search OR 
                    Email LIKE @search OR 
                    Phone LIKE @search OR 
                    Address LIKE @search";

                        using (MySqlCommand cmd = new MySqlCommand(query, connect))
                        {
                            // Parameterized query to prevent SQL injection
                            cmd.Parameters.AddWithValue("@search", $"%{searchValue}%");

                            MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                            System.Data.DataTable table = new System.Data.DataTable();
                            adapter.Fill(table);

                            // Bind the filtered data to the DataGridView
                            UserDataGrid.DataSource = table;

                            // Update the selected user ID (optional: use the first match as the selected user)
                            if (table.Rows.Count > 0)
                            {
                                selectedUserId = Convert.ToInt32(table.Rows[0]["ID"]);
                            }
                            else
                            {
                                selectedUserId = 0; // Reset if no results found
                            }
                        }
                    }

                    // Automatically select and scroll to the first matching row
                    if (UserDataGrid.Rows.Count > 0)
                    {
                        UserDataGrid.Rows[0].Selected = true;
                        UserDataGrid.FirstDisplayedScrollingRowIndex = 0;
                    }
                    searchTextBox.Clear();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error during search: " + ex.Message);
                }
            

        }

        private void UserDataGrid_SelectionChanged(object sender, EventArgs e)
        {
            if (UserDataGrid.SelectedRows.Count > 0)
            {
                DataGridViewRow selectedRow = UserDataGrid.SelectedRows[0];
                selectedUserId = Convert.ToInt32(selectedRow.Cells["ID"].Value); // Store the selected user's ID
                // Populate textboxes
                first_NameTextBox.Text = selectedRow.Cells["FirstName"].Value.ToString();
                last_NameTextBox.Text = selectedRow.Cells["LastName"].Value.ToString();
                passwordTextBox.Text = selectedRow.Cells["Password"].Value.ToString();
                phoneTextBox.Text = selectedRow.Cells["Phone"].Value.ToString();
                addressTextBox.Text = selectedRow.Cells["Address"].Value.ToString();
                emailTextBox.Text = selectedRow.Cells["Email"].Value.ToString();

                // Set checkbox states
                adminCheckBox.Checked = selectedRow.Cells["AdminPerm"].Value.ToString().ToLower() == "true";
                isBlockedCheckBox.Checked = selectedRow.Cells["IsBlocked"].Value.ToString().ToLower() == "true";
            }
        }
    }
}
