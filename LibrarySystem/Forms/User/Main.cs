using LibrarySystem.Forms.User;
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
    public partial class Main : Form
    {
        
        int USERID = 0;
        public Main(int userID)
        {
            USERID = userID;
            InitializeComponent();
        }

        private void Main_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
            Books open = new Books(USERID);
            open.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();
            SearchBooks open = new SearchBooks(USERID);
            open.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Hide();
            Borrowed open = new Borrowed(USERID);
            open.Show();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Hide();
            hello open = new hello();
            open.Show();
        }
    }
}
