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
using LibrarySystem.Forms;

namespace LibrarySystem
{
    public partial class hello : Form
    {
        public hello()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void hello_Load(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                // Define the connection string (update with your database details)
                string connectionString = "Server=localhost;Database=librarydb;User ID=root;Password=bhaibhai123;";

                // Create a connection
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    // Query to check user credentials and fetch UserID
                    string query = "SELECT ID, AdminPerm , IsBlocked FROM Users WHERE FirstName = @FirstName AND Password = @Password";

                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        // Add parameters to prevent SQL injection
                        command.Parameters.AddWithValue("@FirstName", textBox1.Text);
                        command.Parameters.AddWithValue("@Password", textBox2.Text);
                       

                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                // Store the UserID in the global variable
                                int UserID = Convert.ToInt32(reader["ID"]);

                                if (reader["IsBlocked"].ToString() == "false")
                                {
                                    // Check if the user is an admin
                                    if (reader["AdminPerm"].ToString() == "true")
                                    {
                                        this.Hide();
                                        AdminPage open = new AdminPage();
                                        open.Show();
                                    }
                                    else
                                    {
                                        this.Hide();
                                        Main open = new Main(UserID);
                                        open.Show();
                                    }
                                }
                                else
                                {
                                    textBox1.Text = "";
                                    textBox2.Text = "";
                                    MessageBox.Show("You are banned from the application.\nContact the administrator for further information regarding this ban.");
                                }
                            }
                            else
                            {
                                // User not found
                                MessageBox.Show("Invalid username or password.");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message);
            }
        }


        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
