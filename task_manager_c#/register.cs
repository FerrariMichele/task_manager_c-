using System;
using System.CodeDom;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

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
            DateTime dateBirth = dateTimePickerDOB.Value.ToUniversalTime();
            string name = textBoxRegisterName.Text;
            string surname = textBoxRegisterSurname.Text;
            string profilePicture = "nopfp.png";

            if (password != passwordConf)
            {
                MessageBox.Show("Passwords do not match");
                return;
            }

            string hashedPassword = HashPassword(password);

            using (MySqlConnection conn = Conn2Database())
            {
                if (conn == null)
                {
                    MessageBox.Show("Failed to connect to the database.");
                    return;
                }

                int response = RegisterUser(conn, username, email, hashedPassword, dateBirth, name, surname, profilePicture);

                switch (response)
                {
                    case 1:
                        MessageBox.Show("Registration successful");
                        Form1 myForm = new Form1();
                        this.Hide();
                        myForm.ShowDialog();
                        this.Close();
                        break;
                    case 2:
                        MessageBox.Show("Username is already in use");
                        break;
                    case 3:
                        MessageBox.Show("Email is already in use");
                        break;
                    default:
                        MessageBox.Show("An error occurred during registration");
                        break;
                }
            }
        }

        public MySqlConnection Conn2Database()
        {
            string connString = "SERVER=localhost;PORT=3306;DATABASE=my_micheleferrari;UID=root;PASSWORD=;";
            try
            {
                MySqlConnection conn = new MySqlConnection(connString);
                conn.Open();
                return conn;
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(ex.Message);
                return null;
            }
        }

        public int RegisterUser(MySqlConnection conn, string username, string email, string password, DateTime dateBirth, string name, string surname, string pfp)
        {
            try
            {
                // Check if username already exists
                string queryCheckUsername = "SELECT COUNT(*) FROM tm1_users WHERE username = @username";
                using (MySqlCommand cmd = new MySqlCommand(queryCheckUsername, conn))
                {
                    cmd.Parameters.AddWithValue("@username", username);
                    int count = Convert.ToInt32(cmd.ExecuteScalar());
                    if (count > 0)
                        return 2; // Username in use
                }

                // Check if email already exists
                string queryCheckEmail = "SELECT COUNT(*) FROM tm1_users WHERE email = @Email";
                using (MySqlCommand cmd = new MySqlCommand(queryCheckEmail, conn))
                {
                    cmd.Parameters.AddWithValue("@Email", email);
                    int count = Convert.ToInt32(cmd.ExecuteScalar());
                    if (count > 0)
                        return 3; // Email in use
                }

                // Insert new user
                string queryInsert = "INSERT INTO tm1_users (username, email, password, date_birth, name, surname, pfp_image_url) VALUES (@Username, @Email, @Password, @DateOfBirth, @Name, @Surname, @pfp)";
                using (MySqlCommand cmd = new MySqlCommand(queryInsert, conn))
                {
                    cmd.Parameters.AddWithValue("@Username", username);
                    cmd.Parameters.AddWithValue("@Email", email);
                    cmd.Parameters.AddWithValue("@Password", password);
                    cmd.Parameters.AddWithValue("@DateOfBirth", dateBirth);
                    cmd.Parameters.AddWithValue("@Name", name);
                    cmd.Parameters.AddWithValue("@Surname", surname);
                    cmd.Parameters.AddWithValue("@pfp", pfp);

                    cmd.ExecuteNonQuery();
                }

                return 1; // Success
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return 0; // General error
            }
        }

        private string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] hashedPasswordBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return BitConverter.ToString(hashedPasswordBytes).Replace("-", "").ToLower();
            }
        }
    }
}