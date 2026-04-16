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

namespace LibrarySystem.Forms.User
{
    public partial class Requests : Form
    {
        int userID;
        public Requests(string BookID , int userID)
        {
            InitializeComponent();
            this.userID = userID;
            // Populate User ID and Book ISBN fields
            idTextBox.Text = userID.ToString(); // setting the id
            isbnTextBox.Text = BookID; // Set the ID
        }
        

        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();
            Books open = new Books(userID);
            open.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                // Define the connection string
                string connectionString = "Server=localhost;Database=librarydb;User ID=root;Password=bhaibhai123;";

                // Collect the values from the form
                string requestType = borrowCheckBox.Checked ? "Borrow" : buyCheckBox.Checked ? "Buy" : null; // Handle request type
                string userId = idTextBox.Text.Trim();
                string BookID = isbnTextBox.Text.Trim();
                DateTime requestDate = requestdateTimePicker.Value;
                DateTime dueDate = duedateTimePicker.Value;

                if (string.IsNullOrEmpty(requestType))
                {
                    MessageBox.Show("Please select a request type (Borrow or Buy).");
                    return;
                }

                if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(BookID))
                {
                    MessageBox.Show("User ID and BookID are required.");
                    return;
                }

                // Retrieve the book title, check stock, and insert the data into the Requests table
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    // Retrieve the book title and check stock
                    string bookQuery = "SELECT Title, NoOfCopies, ISBN FROM Books WHERE BookID = @BookID";
                    string bookTitle = string.Empty;
                    int isbn = 0;
                    int noOfCopies = 0;

                    using (MySqlCommand getBookCommand = new MySqlCommand(bookQuery, connection))
                    {
                        getBookCommand.Parameters.AddWithValue("@BookID", BookID);

                        using (MySqlDataReader reader = getBookCommand.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                bookTitle = reader["Title"].ToString();
                                noOfCopies = Convert.ToInt32(reader["NoOfCopies"]);
                                isbn = Convert.ToInt32(reader["ISBN"]);
                            }
                            else
                            {
                                MessageBox.Show("Book not found. Please check the BookID.");
                                return;
                            }
                        }
                    }

                    // Check if the book is out of stock
                    if (noOfCopies <= 0)
                    {
                        MessageBox.Show("The book is out of stock.");
                        return;
                    }

                    // Subtract one from the NoOfCopies column
                    string updateCopiesQuery = "UPDATE Books SET NoOfCopies = NoOfCopies - 1 WHERE BookID = @BookID";
                    using (MySqlCommand updateCopiesCommand = new MySqlCommand(updateCopiesQuery, connection))
                    {
                        updateCopiesCommand.Parameters.AddWithValue("@BookID", BookID);
                        updateCopiesCommand.ExecuteNonQuery();
                    }

                    // Insert the data into the Requests table
                    string insertQuery = "INSERT INTO Requests (BookID,ISBN, Title, UserID, DateOfIssue, DateOfReturn, Type) " +
                                         "VALUES (@BookID ,@isbn, @Title, @UserID, @DateOfIssue, @DateOfReturn, @Type)";

                    using (MySqlCommand insertCommand = new MySqlCommand(insertQuery, connection))
                    {
                        // Add parameters to prevent SQL injection
                        insertCommand.Parameters.AddWithValue("@BookID", BookID);
                        insertCommand.Parameters.AddWithValue("@isbn", isbn);
                        insertCommand.Parameters.AddWithValue("@Title", bookTitle);
                        insertCommand.Parameters.AddWithValue("@UserID", userId);
                        insertCommand.Parameters.AddWithValue("@DateOfIssue", requestDate.ToString("yyyy-MM-dd"));
                        insertCommand.Parameters.AddWithValue("@DateOfReturn", dueDate.ToString("yyyy-MM-dd"));
                        insertCommand.Parameters.AddWithValue("@Type", requestType);

                        // Execute the query
                        insertCommand.ExecuteNonQuery();
                        MessageBox.Show("Request submitted successfully.");

                      
                        this.Hide();
                        Main open = new Main(userID);
                        open.Show();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message);
            }
        }

        private void Requests_Load(object sender, EventArgs e)
        {
            
        }
    }
}
