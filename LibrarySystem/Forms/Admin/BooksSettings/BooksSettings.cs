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

namespace LibrarySystem
{
    public partial class BooksSettings : Form
    {

        public BooksSettings()
        {
            InitializeComponent();
        }

        private void BooksSettings_Load(object sender, EventArgs e)
        {
   
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
            AdminPage close = new AdminPage();
            close.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();
            AddNewBook open = new AddNewBook();
            open.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Hide();
            editingbooks open = new editingbooks();
            open.Show();
        }
    }
}
