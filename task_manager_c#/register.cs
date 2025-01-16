using System;
using System.CodeDom;
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
    public partial class register : Form
    {
        public register()
        {
            InitializeComponent();
        }

        private void buttonLogin_Click(object sender, EventArgs e)
        {
            Form1 myForm = new Form1();
            this.Hide();
            myForm.ShowDialog();
            this.Close();
        }

        private void buttonRegister_Click(object sender, EventArgs e)
        {
            string username = textBoxRegisterUsername.Text;
            string email = textBoxRegisterEmail.Text;
            string password = textBoxRegisterPassword.Text;
            string passwordConf = textBoxRegisterPasswordConf.Text;
            DateTime dateBirh = dateTimePickerDOB.Value.ToUniversalTime();
            string name = textBoxRegisterName.Text;
            string surname = textBoxRegisterSurname.Text;

            //validate email -> if true continue, if false messagebox
            if (password != passwordConf)
            {
                MessageBox.Show("Passords do not match");
                return;
            }

            //register the user RegisterUser(username, email, password, passwordConf, dateBirth, name, surname); if response == 1 ok, == 2 username in use, == 3 email in use, else general error

            //index myForm = index Form1();
            //this.Hide();
            //myForm.ShowDialog();
            //this.Close();
        }
    }
}
