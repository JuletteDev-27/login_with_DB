using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace login_with_DB
{
    public partial class Form1 : Form
    {
        private string dbconn_string = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\\Database1.mdf;Integrated Security=True";

        public Form1()
        {
            InitializeComponent();
            lbl_usernameError.ForeColor = System.Drawing.Color.Red;
        }

        private bool CheckExists(string user_accountname)
        {
            var count = 0;

            using (SqlConnection dbconn = new SqlConnection(dbconn_string))
            {
                dbconn.Open();

                using (SqlCommand checkExists = new SqlCommand("check_userexists", dbconn))
                {
                    checkExists.CommandType = CommandType.StoredProcedure;

                    checkExists.Parameters.AddWithValue("@user_accountname", user_accountname);

                    using (SqlDataReader result = checkExists.ExecuteReader())
                    {
                        while (result.Read())
                        {
                            count = Int32.Parse(result["result"].ToString());
                        }
                    }
                }

                dbconn.Close();
            }

            if (count != 1)
            {
                return false;
            }

            return true;
        }

        private bool CheckPassword(string user_accountname, string user_passwordinput)
        {
            var userpassword = "";

            using (SqlConnection dbconn = new SqlConnection(dbconn_string))
            {
                dbconn.Open();

                using (SqlCommand getPassword = new SqlCommand("get_userPassword", dbconn))
                {
                    getPassword.CommandType = CommandType.StoredProcedure;

                    getPassword.Parameters.AddWithValue("@user_accountname", user_accountname);

                    using (SqlDataReader result = getPassword.ExecuteReader())
                    {
                        while (result.Read())
                        {
                            userpassword = result["result"].ToString();
                        }
                    }
                }

                dbconn.Close();
            }

            if (userpassword != user_passwordinput)
            {
                return false;
            }

            return true;
        }


        private void cbx_showpass_CheckedChanged(object sender, EventArgs e)
        {
            if (cbx_showpass.Checked)
            {
                tbx_password.PasswordChar = '\0';
                return;
            }

            tbx_password.PasswordChar = '*';
            return;

        }

        private void btn_login_Click(object sender, EventArgs e)
        {
            var input_username = tbx_username.Text;
            var input_password = tbx_password.Text;

            if (!CheckExists(input_username))
            {
                lbl_usernameError.Text = "USER DOES NOT EXISTS!";
                lbl_passwordError.Text = "USER DOES NOT EXISTS";
                tbx_password.Text = "";
                tbx_username.Text = "";
                return;
            }

            if (!CheckPassword(input_username, input_password))
            {
                lbl_usernameError.Text = "INCORRECT USERNAME/PASSWORD!";
                lbl_passwordError.Text = "INCORRECT USERNAME/PASSWORD!";
                tbx_password.Text = "";
                tbx_username.Text = "";
                return;
            }

            loggedInWindow newWindow = new loggedInWindow(input_username);
            newWindow.Show();
            this.Hide();
        }
    }
}
