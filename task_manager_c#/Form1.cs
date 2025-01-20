using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.Security.Cryptography;
using Mysqlx.Expr;

namespace task_manager_c_
{
    public partial class Form1 : Form
    {
        MySqlConnection conn;

        public Form1()
        {
            InitializeComponent();
            Conn2Database();
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
            int resultSignIn = SignInUser(username, password);
            switch (resultSignIn)
            {
                case 0:
                    //redirect
                    break;
                case 1:
                    MessageBox.Show("Username not found");
                    break;
                case 2:
                    MessageBox.Show("Incorrect password");
                    break;
                case 3:
                    MessageBox.Show("Error accessing the database");
                    break;
                default:
                    break;
            }
        }

        public int SignInUser(string usernameT, string passwordT)
        {
            try
            {
                string usernameQuery = "SELECT password FROM tm1_users WHERE username = @username";
                using (MySqlCommand cmd = new MySqlCommand(usernameQuery, conn))
                {
                    cmd.Parameters.AddWithValue("@username", usernameT);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (!reader.HasRows)
                        {
                            // Username not found
                            return 1;
                        }

                        reader.Read();
                        string storedHashedPassword = reader.GetString("password");

                        // Step 2: Verify the hashed password
                        if (VerifyPassword(passwordT, storedHashedPassword))
                        {
                            // Successful sign-in
                            return 0;
                        }
                        else
                        {
                            // Incorrect password
                            return 2;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // General error in accessing the database
                return 3;
            }
        }

        public void Conn2Database()
        {
            int result;
            string connString = "SERVER=localhost;PORT=3306;DATABASE=my_micheleferrari;UID=root;PASSWORD=;";
            try
            {
                conn = new MySqlConnection();
                conn.ConnectionString = connString;
                conn.Open();
            } catch (MySqlException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private bool VerifyPassword(string password, string storedHash)
        {
            // Hash the input password and compare with the stored hash
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] hashedPasswordBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                string hashedPassword = BitConverter.ToString(hashedPasswordBytes).Replace("-", "").ToLower();

                return hashedPassword == storedHash;
            }
        }
    }
}
