using LibrarySystem.Forms.User;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LibrarySystem
{
    public partial class Books : Form
    {

        // Replace with your database connection string
        private string connectionString = "Server=localhost;Database=librarydb;User ID=root;Password=bhaibhai123;";

        int userID;
        public Books(int userId)
        {
            InitializeComponent();
            userID = userId;
            LoadBookNames();
        }

        private void LoadBookNames()
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = "SELECT BookID, Title FROM books";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    booksComboBox.DisplayMember = "Title";
                    booksComboBox.ValueMember = "BookID";
                    booksComboBox.DataSource = dt;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }
        private void LoadBookDetails(int bookID)
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = "SELECT Title, Author, Genre , NoOfCopies FROM books WHERE BookID = @BookID";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@BookID", bookID);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            string bookTitle = reader["Title"].ToString();
                            string author = reader["Author"].ToString();
                            string genre = reader["Genre"].ToString();
                            string NoOfCopies = reader["NoOfCopies"].ToString();
                            // Clear the RichTextBox
                            BooksDetails.Clear();

                            // Add Title
                            BooksDetails.SelectionFont = new Font("Segoe UI", 12, FontStyle.Bold); // Bold font for heading
                            BooksDetails.AppendText("Title: ");
                            BooksDetails.SelectionFont = new Font("Segoe UI", 12, FontStyle.Regular); // Regular font for value
                            BooksDetails.AppendText($"{bookTitle}\n");

                            // Add spacing
                            BooksDetails.AppendText("\n");

                            // Add Author
                            BooksDetails.SelectionFont = new Font("Segoe UI", 12, FontStyle.Bold);
                            BooksDetails.AppendText("Author: ");
                            BooksDetails.SelectionFont = new Font("Segoe UI", 12, FontStyle.Regular);
                            BooksDetails.AppendText($"{author}\n");

                            // Add spacing
                            BooksDetails.AppendText("\n");

                            // Add Genre
                            BooksDetails.SelectionFont = new Font("Segoe UI", 12, FontStyle.Bold );
                            BooksDetails.AppendText("Genre: ");
                            BooksDetails.SelectionFont = new Font("Segoe UI", 12, FontStyle.Regular);
                            BooksDetails.AppendText($"{genre}\n");

                            // Add spacing
                            BooksDetails.AppendText("\n");

                            // Add Copies Available
                            BooksDetails.SelectionFont = new Font("Segoe UI", 12, FontStyle.Bold);
                            BooksDetails.AppendText("Copies Available: ");
                            BooksDetails.SelectionFont = new Font("Segoe UI", 12, FontStyle.Regular);
                            BooksDetails.AppendText($"{NoOfCopies}\n");

                            // Ensure the text box is scrolled to the top
                            BooksDetails.SelectionStart = 0;
                            BooksDetails.ScrollToCaret();
                        }
                        else
                        {
                            BooksDetails.Text = "No details available.";
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }

        private void Books_Load(object sender, EventArgs e)
        {
            
        }

        private void booksDataGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();
            Main open = new Main(userID);
            open.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Get the selected BookID from the combo box
            string selectedBookID = (booksComboBox.SelectedValue).ToString();

            if (!string.IsNullOrEmpty(selectedBookID))
            {
                // Open the Requests form with the ISBN
                Requests requestForm = new Requests(selectedBookID , userID);
                requestForm.Show();
                this.Hide(); // Optionally hide the current form
            }
            else
            {
                MessageBox.Show("Unable to fetch ID for the selected book.");
            }
        }

        // below method is for the case when ISBN is used for requests 
        // 

        //private string GetISBNFromBookID(int bookID)
        //{
        //    string isbn = string.Empty;
        //    using (MySqlConnection conn = new MySqlConnection(connectionString))
        //    {
        //        try
        //        {
        //            conn.Open();
        //            string query = "SELECT ISBN FROM books WHERE BookID = @BookID";
        //            MySqlCommand cmd = new MySqlCommand(query, conn);
        //            cmd.Parameters.AddWithValue("@BookID", bookID);

        //            object result = cmd.ExecuteScalar(); // Fetch a single value (ISBN)
        //            if (result != null)
        //            {
        //                isbn = result.ToString();
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            MessageBox.Show("Error fetching ISBN: " + ex.Message);
        //        }
        //    }
        //    return isbn;
        //}


        private void booksComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (booksComboBox.SelectedValue != null)
            {
                int selectedBookID = Convert.ToInt32(booksComboBox.SelectedValue);
                LoadBookDetails(selectedBookID);
            }
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }
    }
}
