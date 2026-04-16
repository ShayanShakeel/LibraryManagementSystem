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

namespace LibrarySystem.Forms.Admin.UserSettings
{
    public partial class AddNewUser : Form
    {
        public AddNewUser()
        {
            InitializeComponent();
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            this.Hide();
            UserEdittingOptions open = new UserEdittingOptions();
            open.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                // Define the connection string
                string connectionString = "Server=localhost;Database=librarydb;User ID=root;Password=bhaibhai123;";
                using (MySqlConnection connect = new MySqlConnection(connectionString))
                {
                    if (
                        !string.IsNullOrWhiteSpace(first_nameTextBox.Text) &&
                        !string.IsNullOrWhiteSpace(last_nameTextBox.Text) &&
                        !string.IsNullOrWhiteSpace(passwordTextBox.Text) &&
                        !string.IsNullOrWhiteSpace(phoneTextBox.Text) &&
                        !string.IsNullOrWhiteSpace(addressTextBox.Text) &&
                        !string.IsNullOrWhiteSpace(emailTextBox.Text))
                    {
                        connect.Open();

                        string query = "INSERT INTO Users (FirstName, LastName, Password, AdminPerm, IsBlocked, Phone, Address, Email) " +
                            "VALUES (@FirstName, @LastName, @Password, @AdminPerm, @IsBlocked, @Phone, @Address, @Email)";

                        using (MySqlCommand cmd = new MySqlCommand(query, connect))
                        {
                            // Add parameters to prevent SQL injection
                            cmd.Parameters.AddWithValue("@FirstName", first_nameTextBox.Text);
                            cmd.Parameters.AddWithValue("@LastName", last_nameTextBox.Text);
                            cmd.Parameters.AddWithValue("@Password", passwordTextBox.Text); 
                            cmd.Parameters.AddWithValue("@AdminPerm", typeCheckBox.Checked ? "true" : "false");
                            cmd.Parameters.AddWithValue("@IsBlocked", blockCheckBox.Checked ? "true" : "false");
                            cmd.Parameters.AddWithValue("@Phone", phoneTextBox.Text);
                            cmd.Parameters.AddWithValue("@Address", addressTextBox.Text);
                            cmd.Parameters.AddWithValue("@Email", emailTextBox.Text);
                            cmd.ExecuteNonQuery();
                        }

                        this.Hide();
                        this.Show();
                        MessageBox.Show("User has been successfully added to the library.");
                        button2_Click_1(sender,e);
                    }
                    else
                    {
                        MessageBox.Show("Empty fields are not allowed! Please make sure every data has been added.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message);
            }
        }
    }
}
