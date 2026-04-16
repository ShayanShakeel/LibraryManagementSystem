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

namespace LibrarySystem.Forms.Admin.user_settings
{
    public partial class requestSettings : Form
    {
        public requestSettings()
        {
            InitializeComponent();
        }

        private void requestSettings_Load(object sender, EventArgs e)
        {
            LoadRequestsData();
            SetDataGridViewColumnProperties(borrowDataGrid);
            SetDataGridViewColumnProperties(returnedDataGrid);
        }

        private void borrowDataGrid_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                // Get the RequestID of the selected row
                DataGridViewRow selectedRow = borrowDataGrid.Rows[e.RowIndex];
                string requestId = selectedRow.Cells["RequestID"].Value?.ToString();

                // Display the RequestID in the textbox
                textBox1.Text = requestId;
            }
        }

        private void LoadRequestsData()
        {
            try
            {
                string connectionString = "Server=localhost;Database=librarydb;User ID=root;Password=bhaibhai123;";

                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    // Query for borrowDataGrid
                    string borrowQuery = @"
                SELECT 
                    r.RequestID,
                    r.UserID,
                    CONCAT(u.FirstName, ' ', u.LastName) AS UserName,
                    r.BookID,
                    r.ISBN,
                    r.Title,
                    r.DateOfIssue,
                    r.DateOfReturn,
                    r.Type
                FROM requests AS r
                JOIN users AS u ON r.UserID = u.ID
                WHERE r.Type = 'Borrow';";

                    MySqlDataAdapter borrowAdapter = new MySqlDataAdapter(borrowQuery, connection);
                    DataTable borrowTable = new DataTable();
                    borrowAdapter.Fill(borrowTable);
                    borrowDataGrid.DataSource = borrowTable;

                    // Query for returnedDataGrid
                    string returnQuery = @"
                SELECT 
                    r.RequestID,
                    r.UserID,
                    CONCAT(u.FirstName, ' ', u.LastName) AS UserName,
                    r.BookID,
                    r.ISBN,
                    r.Title,
                    r.DateOfIssue,
                    r.DateOfReturn,
                    r.Type
                FROM requests AS r
                JOIN users AS u ON r.UserID = u.ID
                WHERE r.Type IN ('Returned', 'Buy');";

                    MySqlDataAdapter returnAdapter = new MySqlDataAdapter(returnQuery, connection);
                    DataTable returnTable = new DataTable();
                    returnAdapter.Fill(returnTable);
                    returnedDataGrid.DataSource = returnTable;

                    // Set minimum width for all columns
                    SetDataGridViewColumnProperties(borrowDataGrid);
                    SetDataGridViewColumnProperties(returnedDataGrid);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while loading requests data: " + ex.Message);
            }
        }

        private void SetDataGridViewColumnProperties(DataGridView dataGridView)
        {
            foreach (DataGridViewColumn column in dataGridView.Columns)
            {
                column.MinimumWidth = 100; // Set the minimum width for all columns
                column.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells; // Auto-size columns
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            string requestId = textBox1.Text.Trim();

            if (string.IsNullOrWhiteSpace(requestId))
            {
                MessageBox.Show("Please select a valid Request ID.");
                return;
            }

            try
            {
                string connectionString = "Server=localhost;Database=librarydb;User ID=root;Password=bhaibhai123;";
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    // Get BookID from the selected RequestID
                    string getBookIdQuery = "SELECT BookID FROM requests WHERE RequestID = @requestId";
                    int bookId = 0;
                    using (MySqlCommand getBookIdCmd = new MySqlCommand(getBookIdQuery, connection))
                    {
                        getBookIdCmd.Parameters.AddWithValue("@requestId", requestId);
                        object result = getBookIdCmd.ExecuteScalar();
                        if (result != null)
                        {
                            bookId = Convert.ToInt32(result);
                        }
                        else
                        {
                            MessageBox.Show("Invalid Request ID. Book not found.");
                            return;
                        }
                    }

                    // Update the Type to "Returned" for the selected RequestID
                    string updateQuery = "UPDATE requests SET Type = 'Returned' WHERE RequestID = @requestId";
                    using (MySqlCommand cmd = new MySqlCommand(updateQuery, connection))
                    {
                        cmd.Parameters.AddWithValue("@requestId", requestId);
                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            // Increment the NoOfCopies in the Books table
                            string incrementCopiesQuery = "UPDATE Books SET NoOfCopies = NoOfCopies + 1 WHERE BookID = @bookId";
                            using (MySqlCommand incrementCmd = new MySqlCommand(incrementCopiesQuery, connection))
                            {
                                incrementCmd.Parameters.AddWithValue("@bookId", bookId);
                                incrementCmd.ExecuteNonQuery();
                            }

                            MessageBox.Show("Request successfully updated to 'Returned' and NoOfCopies increased.");
                            // Reload the DataGridViews
                            LoadRequestsData();
                            textBox1.Text = "";
                        }
                        else
                        {
                            MessageBox.Show("No matching Request ID found. Update failed.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while updating the request: " + ex.Message);
            }
        }


        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
            AdminPage open = new AdminPage();
            open.Show();
        }

       
        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        // History Search Button
        private void button3_Click(object sender, EventArgs e)
        {
            string searchValue = textBox2.Text.Trim();

            if (string.IsNullOrWhiteSpace(searchValue))
            {
                MessageBox.Show("Please enter a value to search in History.");
                return;
            }

            try
            {
                string connectionString = "Server=localhost;Database=librarydb;User ID=root;Password=bhaibhai123;";
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    string query = @"
                SELECT 
                    r.RequestID,
                    r.UserID,
                    CONCAT(u.FirstName, ' ', u.LastName) AS UserName,
                    r.BookID,
                    r.ISBN,
                    r.Title,
                    r.DateOfIssue,
                    r.DateOfReturn,
                    r.Type
                FROM requests AS r
                JOIN users AS u ON r.UserID = u.ID
                WHERE r.Type IN ('Returned', 'Buy') 
                AND (r.RequestID LIKE @search OR
                     r.UserID LIKE @search OR
                     u.FirstName LIKE @search OR
                     u.LastName LIKE @search OR
                     r.BookID LIKE @search OR
                     r.ISBN LIKE @search OR
                     r.Title LIKE @search OR
                     r.DateOfIssue LIKE @search OR
                     r.DateOfReturn LIKE @search OR
                     r.Type LIKE @search);";

                    using (MySqlCommand cmd = new MySqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@search", $"%{searchValue}%");

                        MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                        DataTable historyTable = new DataTable();
                        adapter.Fill(historyTable);
                        returnedDataGrid.DataSource = historyTable;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while searching in History: " + ex.Message);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            string searchValue = textBox3.Text.Trim();

            if (string.IsNullOrWhiteSpace(searchValue))
            {
                MessageBox.Show("Please enter a value to search in Pending.");
                return;
            }

            try
            {
                string connectionString = "Server=localhost;Database=librarydb;User ID=root;Password=bhaibhai123;";
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    string query = @"
                SELECT 
                    r.RequestID,
                    r.UserID,
                    CONCAT(u.FirstName, ' ', u.LastName) AS UserName,
                    r.BookID,
                    r.ISBN,
                    r.Title,
                    r.DateOfIssue,
                    r.DateOfReturn,
                    r.Type
                FROM requests AS r
                JOIN users AS u ON r.UserID = u.ID
                WHERE r.Type = 'Borrow'
                AND (r.RequestID LIKE @search OR
                     r.UserID LIKE @search OR
                     u.FirstName LIKE @search OR
                     u.LastName LIKE @search OR
                     r.BookID LIKE @search OR
                     r.ISBN LIKE @search OR
                     r.Title LIKE @search OR
                     r.DateOfIssue LIKE @search OR
                     r.DateOfReturn LIKE @search OR
                     r.Type LIKE @search);";

                    using (MySqlCommand cmd = new MySqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@search", $"%{searchValue}%");

                        MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                        DataTable pendingTable = new DataTable();
                        adapter.Fill(pendingTable);
                        borrowDataGrid.DataSource = pendingTable;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while searching in Pending: " + ex.Message);
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            LoadRequestsData();
        }

        private void returnedDataGrid_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

       

        private void label7_Click(object sender, EventArgs e)
        {

        }
    }
}
