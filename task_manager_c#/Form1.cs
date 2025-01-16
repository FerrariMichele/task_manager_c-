using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace task_manager_c_
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void buttonRegister_Click(object sender, EventArgs e)
        {
            register myForm = new register();
            this.Hide();
            myForm.ShowDialog();
            this.Close();
        }

        private void buttonLogin_Click(object sender, EventArgs e)
        {
            string username = textBoxLoginUsername.Text;
            string password = textBoxLoginPassword.Text;

            //log in the user SignInUser(username, password); if response == true open index else show messagebox
        }
    }
}
