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

namespace LibrarySystem
{
    public partial class AddNewBook : Form
    {
        public AddNewBook()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                // Define the connection string
                string connectionString = "Server=localhost;Database=librarydb;User ID=root;Password=bhaibhai123;";

                using (MySqlConnection connect = new MySqlConnection(connectionString))
                {
                    // Check for empty fields
                    if (!string.IsNullOrWhiteSpace(ISBNtxt.Text) &&
                        !string.IsNullOrWhiteSpace(titletxt.Text) &&
                        !string.IsNullOrWhiteSpace(authortxt.Text) &&
                        !string.IsNullOrWhiteSpace(locationtxt.Text) &&
                        !string.IsNullOrWhiteSpace(pricetxt.Text) &&
                        !string.IsNullOrWhiteSpace(genretxt.Text) &&
                        !string.IsNullOrWhiteSpace(noctxt.Text))
                    {
                        // Open the connection
                        connect.Open();

                        // Define the insert query with parameters
                        string query = "INSERT INTO Books (ISBN, Title, Author, Location, NoOfCopies, Price, Genre) " +
                                       "VALUES (@ISBN, @Title, @Author, @Location, @NoOfCopies, @Price, @Genre)";

                        using (MySqlCommand cmd = new MySqlCommand(query, connect))
                        {
                            // Add parameters to the command
                            cmd.Parameters.AddWithValue("@ISBN", ISBNtxt.Text);
                            cmd.Parameters.AddWithValue("@Title", titletxt.Text);
                            cmd.Parameters.AddWithValue("@Author", authortxt.Text);
                            cmd.Parameters.AddWithValue("@Location", locationtxt.Text);
                            cmd.Parameters.AddWithValue("@NoOfCopies", int.Parse(noctxt.Text)); // Ensure numeric value
                            cmd.Parameters.AddWithValue("@Price", decimal.Parse(pricetxt.Text)); // Ensure decimal value
                            cmd.Parameters.AddWithValue("@Genre", genretxt.Text);

                            // Execute the query
                            cmd.ExecuteNonQuery();
                            MessageBox.Show("Data has been successfully added.");

                           ClearTextBoxes();
                        }
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

        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();
            BooksSettings open = new BooksSettings();
            open.Show();
        }

        private void AddNewBook_Load(object sender, EventArgs e)
        {

        }

        public void ClearTextBoxes()
        {
            ISBNtxt.Text = "";
            titletxt.Text = "";
            authortxt.Text = "";
            locationtxt.Text = "";
            pricetxt.Text = "";
            noctxt.Text = "";
            genretxt.Text = "";
        }
    }
}
