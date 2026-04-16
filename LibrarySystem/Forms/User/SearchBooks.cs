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
 
    public partial class SearchBooks : Form
    {

        int userID = 0;
        public SearchBooks(int userId)
        {
            InitializeComponent();
           this.userID = userId;

        }

        private void SearchBooks_Load(object sender, EventArgs e)
        {

            booksDataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;

        


            // Populate the search filter dropdown
            searchByComboBox.Items.AddRange(new string[] { "Title", "ISBN", "Author", "Genre", "Location", "Price" });
            searchByComboBox.SelectedIndex = 0; // Set default selection

            // Load all books data initially
            LoadBooksData();
            button1_Click(sender, e);
            
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();
            Main mainForm = new Main(userID);
            mainForm.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string searchValue = searchTextBox.Text.Trim();
            string selectedColumn = searchByComboBox.SelectedItem?.ToString();

            if (string.IsNullOrWhiteSpace(selectedColumn))
            {
                MessageBox.Show("Please select a column to search by.");
                return;
            }

            try
            {
                string connectionString = "Server=localhost;Database=librarydb;User ID=root;Password=bhaibhai123;";
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    string query = $"SELECT BookID, ISBN, Title, Author, Location, Price, NoOfCopies, Genre FROM Books WHERE {selectedColumn} LIKE @searchValue";
                    using (MySqlCommand cmd = new MySqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@searchValue", $"%{searchValue}%");

                        MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                        DataTable booksTable = new DataTable();
                        adapter.Fill(booksTable);

                        booksDataGridView.DataSource = booksTable;
                    }
                }

                if (booksDataGridView.Rows.Count == 0)
                {
                    MessageBox.Show("No books found matching the search criteria.");
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message);
            }
        }

        private void LoadBooksData()
        {
            try
            {
                string connectionString = "Server=localhost;Database=librarydb;User ID=root;Password=bhaibhai123;";
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    string query = "SELECT BookID, ISBN, Title, Author, Location, Price, NoOfCopies, Genre FROM Books";
                    using (MySqlCommand cmd = new MySqlCommand(query, connection))
                    {
                        MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                        DataTable booksTable = new DataTable();
                        adapter.Fill(booksTable);

                        booksDataGridView.DataSource = booksTable;

                        // Set minimum width for all columns after data binding
                        foreach (DataGridViewColumn column in booksDataGridView.Columns)
                        {
                            column.MinimumWidth = 100; // Set the minimum width for all columns
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while loading data: " + ex.Message);
            }
        }

        private void booksDataGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void searchTextBox_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
