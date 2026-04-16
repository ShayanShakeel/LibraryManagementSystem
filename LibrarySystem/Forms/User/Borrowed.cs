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

namespace LibrarySystem.Forms.User
{
    public partial class Borrowed : Form
    {
        private int userID;

        public Borrowed(int userID)
        {
            InitializeComponent();
            this.userID = userID;
        }

        private void Borrowed_Load_1(object sender, EventArgs e)
        {
            // Auto size columns for better view
            borrowDataGrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;

            // Load borrowed books for the current user
            LoadBorrowedBooks();

            // Highlight overdue rows
            HighlightOverdueBooks();

            DisableColumnSorting();
            button1_Click(sender, e);
        }

        private void LoadBorrowedBooks()
        {
            try
            {
                string connectionString = "Server=localhost;Database=librarydb;User ID=root;Password=bhaibhai123;";
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    string query = @"
                        SELECT 
                            ISBN, 
                            Title, 
                            DateOfIssue, 
                            DateOfReturn 
                        FROM Requests 
                        WHERE Type = 'Borrow' AND UserID = @UserID";

                    using (MySqlCommand cmd = new MySqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@UserID", userID);

                        MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                        DataTable borrowedBooksTable = new DataTable();
                        adapter.Fill(borrowedBooksTable);

                        borrowDataGrid.DataSource = borrowedBooksTable;

                        // Set minimum column widths if needed
                        foreach (DataGridViewColumn column in borrowDataGrid.Columns)
                        {
                            column.MinimumWidth = 138;
                        }
                        DisableColumnSorting();
                    }
                }

                if (borrowDataGrid.Rows.Count == 0)
                {
                    MessageBox.Show("No borrowed books found for the current user.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while loading borrowed books: " + ex.Message);
            }
        }

        private void HighlightOverdueBooks()
        {
            try
            {
                foreach (DataGridViewRow row in borrowDataGrid.Rows)
                {
                    // Ensure DateOfReturn column exists
                    if (row.Cells["DateOfReturn"].Value != null)
                    {
                        DateTime dueDate = Convert.ToDateTime(row.Cells["DateOfReturn"].Value);
                        if (DateTime.Now > dueDate) // Compare with current date
                        {
                            row.DefaultCellStyle.BackColor = Color.Red; // Highlight row in red
                            row.DefaultCellStyle.ForeColor = Color.White; // Optional: Change text color for better visibility
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while highlighting overdue books: " + ex.Message);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();
            Main mainForm = new Main(userID); // Assuming Main form takes userID
            mainForm.Show();
        }

        private void borrowDataGrid_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Reload borrowed books for the current user
            LoadBorrowedBooks();

            // Reapply highlighting for overdue books
            HighlightOverdueBooks();
        }

        private void DisableColumnSorting()
        {
            foreach (DataGridViewColumn column in borrowDataGrid.Columns)
            {
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
            }
        }

    }
}
