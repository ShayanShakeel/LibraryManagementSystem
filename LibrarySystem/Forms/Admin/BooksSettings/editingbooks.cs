using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Windows.Forms;

namespace LibrarySystem
{
    public partial class editingbooks : Form
    {
        public editingbooks()
        {
            InitializeComponent();
        }

        private void editingbooks_Load(object sender, EventArgs e)
        {
            LoadBooksData();
            // Populate the Books search combobox
            searchByComboBox.Items.AddRange(new string[] { "Title", "ISBN", "Author", "Genre", "Location", "Price" });
            searchByComboBox.SelectedIndex = 0; // Set default selection 
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Hide();
            BooksSettings open = new BooksSettings();
            open.Show();
        }

        private int currentBookID; // Holds the ID of the selected book

        private void button2_Click(object sender, EventArgs e) // Delete a book
        {
            try
            {
                string connectionString = "Server=localhost;Database=librarydb;User ID=root;Password=bhaibhai123;";
                using (MySqlConnection connect = new MySqlConnection(connectionString))
                {
                    if (!string.IsNullOrWhiteSpace(ISBNtxt.Text))
                    {
                        connect.Open();


                        using (MySqlTransaction transaction = connect.BeginTransaction())
                        {
                            try
                            {
                                // Step 1: Delete associated rows from the requests table
                                string deleteRequestsQuery = "DELETE FROM requests WHERE BookID = @BookID";
                                using (MySqlCommand deleteRequestsCmd = new MySqlCommand(deleteRequestsQuery, connect, transaction))
                                {
                                    deleteRequestsCmd.Parameters.AddWithValue("@BookID", currentBookID);
                                    deleteRequestsCmd.ExecuteNonQuery();
                                }

                                // Step 2: Delete the book from the Books table
                                string deleteBookQuery = "DELETE FROM Books WHERE BookID = @BookID";
                                using (MySqlCommand deleteBookCmd = new MySqlCommand(deleteBookQuery, connect, transaction))
                                {
                                    deleteBookCmd.Parameters.AddWithValue("@BookID", currentBookID);
                                    deleteBookCmd.ExecuteNonQuery();
                                }

                                // Commit the transaction if both deletions succeed
                                transaction.Commit();
                                MessageBox.Show("Book and its associated requests have been successfully removed.");
                                RefreshData();
                            }
                            catch (Exception ex)
                            {
                                // Rollback the transaction in case of any failure
                                transaction.Rollback();
                                MessageBox.Show("An error occurred while deleting the book: " + ex.Message);
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("Please enter the ISBN of the book to remove.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message);
            }
        }

        private void button1_Click(object sender, EventArgs e) // Update book details
        {
            try
            {
                string connectionString = "Server=localhost;Database=librarydb;User ID=root;Password=bhaibhai123;";
                using (MySqlConnection connect = new MySqlConnection(connectionString))
                {
                    if (!string.IsNullOrWhiteSpace(titletxt.Text) &&
                        !string.IsNullOrWhiteSpace(authortxt.Text) &&
                        !string.IsNullOrWhiteSpace(locationtxt.Text) &&
                        !string.IsNullOrWhiteSpace(pricetxt.Text) &&
                        !string.IsNullOrWhiteSpace(genretxt.Text) &&
                        !string.IsNullOrWhiteSpace(noctxt.Text))
                    {
                        connect.Open();

                        string query = "UPDATE Books SET ISBN = @ISBN, Title = @Title, Author = @Author, Location = @Location, " +
                                       "NoOfCopies = @NoOfCopies, Price = @Price, Genre = @Genre WHERE BookID = @ID";

                        using (MySqlCommand cmd = new MySqlCommand(query, connect))
                        {
                            // Add parameters
                            cmd.Parameters.AddWithValue("@ID", currentBookID); // Use the hidden ID
                            cmd.Parameters.AddWithValue("@ISBN", ISBNtxt.Text);
                            cmd.Parameters.AddWithValue("@Title", titletxt.Text);
                            cmd.Parameters.AddWithValue("@Author", authortxt.Text);
                            cmd.Parameters.AddWithValue("@Location", locationtxt.Text);
                            cmd.Parameters.AddWithValue("@NoOfCopies", int.Parse(noctxt.Text));
                            cmd.Parameters.AddWithValue("@Price", decimal.Parse(pricetxt.Text));
                            cmd.Parameters.AddWithValue("@Genre", genretxt.Text);

                            cmd.ExecuteNonQuery();
                            MessageBox.Show("Record updated successfully!");

                            // Refresh the DataGridView
                            RefreshData();

                            // Clear textboxes after update
                            ClearTextBoxes();
                        }
                    }
                    else
                    {
                        MessageBox.Show("All fields are required. Please fill in all details.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message);
            }
        }


        private void booksBindingNavigator_RefreshItems(object sender, EventArgs e)
        {

        }

        private void authortxt_TextChanged(object sender, EventArgs e)
        {

        }
        // code to load the selected row from the gridlist onto the textboxes
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0) // Check if a valid row is clicked
            {
                DataGridViewRow row = booksDataGridView.Rows[e.RowIndex];

                // Store the ID in the private variable
                currentBookID = Convert.ToInt32(row.Cells["BookID"].Value); // "BookId" is the column name in the grid

                // Populate textboxes with selected row data
                ISBNtxt.Text = row.Cells["ISBN"].Value.ToString();
                titletxt.Text = row.Cells["Title"].Value.ToString();
                authortxt.Text = row.Cells["Author"].Value.ToString();
                locationtxt.Text = row.Cells["Location"].Value.ToString();
                pricetxt.Text = row.Cells["Price"].Value.ToString();
                noctxt.Text = row.Cells["NoOfCopies"].Value.ToString();
                genretxt.Text = row.Cells["Genre"].Value.ToString();
            }
        }




        private void LoadBooksData()
        {
            try
            {
                // Replace with your actual connection string
                string connectionString = "Server=localhost;Database=librarydb;User ID=root;Password=bhaibhai123;";

                // Connect to the database
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    // SQL query to fetch all books
                    string query = "SELECT BookID, ISBN, Title, Author, Location, Price, NoOfCopies, Genre FROM books";

                    // Create a MySqlCommand object
                    MySqlCommand cmd = new MySqlCommand(query, connection);

                    // Fetch data using a MySqlDataAdapter
                    MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);

                    // Fill the DataTable with query results
                    DataTable booksTable = new DataTable();
                    adapter.Fill(booksTable);

                    // Bind the DataTable to the DataGridView
                    booksDataGridView.DataSource = booksTable;
                }
            }
            catch (Exception ex)
            {
                // Show an error message if something goes wrong
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        public void RefreshData()
        {
            try
            {
                // Connection string for MySQL
                MySqlConnection connect = new MySqlConnection("Server=localhost;Database=librarydb;User ID=root;Password=bhaibhai123;");
                MySqlDataAdapter adapter = new MySqlDataAdapter("SELECT * FROM books", connect);
                DataTable dt = new DataTable();
                adapter.Fill(dt);

                // Bind the refreshed data to the DataGridView
                booksDataGridView.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
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

        // the search logic coded behind the search button
        private void button4_Click(object sender, EventArgs e)
        {
            string searchValue = searchTextBox.Text.Trim();
            string selectedColumn = searchByComboBox.SelectedItem?.ToString(); // Get the selected column

            if (string.IsNullOrWhiteSpace(selectedColumn))
            {
                MessageBox.Show("Please select a column to search by.");
                return;
            }

            try
            {
                string connectionString = "Server=localhost;Database=librarydb;User ID=root;Password=bhaibhai123;";
                using (MySqlConnection connect = new MySqlConnection(connectionString))
                {
                    string query = $"SELECT BookID, ISBN , Title, Author, Location, Price, NoOfCopies, Genre " +
                                   $"FROM Books WHERE {selectedColumn} LIKE @search";

                    using (MySqlCommand cmd = new MySqlCommand(query, connect))
                    {
                        cmd.Parameters.AddWithValue("@search", $"%{searchValue}%");

                        MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                        System.Data.DataTable table = new System.Data.DataTable();
                        adapter.Fill(table);

                        booksDataGridView.DataSource = table; // Bind the filtered data to the DataGridView
                    }
                }

                // Automatically select and scroll to the first matching row
                if (booksDataGridView.Rows.Count > 0)
                {
                    // Select the first row and scroll to it
                    booksDataGridView.Rows[0].Selected = true;
                    booksDataGridView.FirstDisplayedScrollingRowIndex = 0;

                    // Update the currentBookID variable with the BookID of the first row
                    currentBookID = Convert.ToInt32(booksDataGridView.Rows[0].Cells["BookID"].Value);
                }
                else
                {
                    currentBookID = -1; // Reset the currentBookID if no results are found
                }
                searchTextBox.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error during search: " + ex.Message);
            }
        }

        private void BooksDataGrid_SelectionChanged(object sender, EventArgs e)
        {
            if (booksDataGridView.SelectedRows.Count > 0)
            {
                // Get the selected row
                DataGridViewRow selectedRow = booksDataGridView.SelectedRows[0];
                currentBookID = Convert.ToInt32(selectedRow.Cells["BookID"].Value); // Update currentBookID

                // Populate textboxes with the selected book's details
                ISBNtxt.Text = selectedRow.Cells["ISBN"].Value?.ToString() ?? string.Empty;
                titletxt.Text = selectedRow.Cells["Title"].Value?.ToString() ?? string.Empty;
                authortxt.Text = selectedRow.Cells["Author"].Value?.ToString() ?? string.Empty;
                locationtxt.Text = selectedRow.Cells["Location"].Value?.ToString() ?? string.Empty;
                pricetxt.Text = selectedRow.Cells["Price"].Value?.ToString() ?? string.Empty;
                noctxt.Text = selectedRow.Cells["NoOfCopies"].Value?.ToString() ?? string.Empty;
                genretxt.Text = selectedRow.Cells["Genre"].Value?.ToString() ?? string.Empty;
            }
        }

    }
}
