using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace InventoryApplication
{
    public partial class Login : Form
    {

        private string responseMessage;

        public Login()
        {
            InitializeComponent();
        }

        // Sign-In
        private void btnSignIn_Click(object sender, EventArgs e)
        {
            if (txtLoginName.Text != "" && txtPassword.Text != "")
            {
                // Create the connection.
                using (SqlConnection connection = new SqlConnection(Properties.Settings.Default.connString))
                {
                    // Create a SqlCommand, and identify it as a stored procedure.
                    using (SqlCommand sqlCommand = new SqlCommand("dbo.uspLogin", connection))
                    {
                        sqlCommand.CommandType = CommandType.StoredProcedure;

                        // Add input parameter for the stored procedure and specify what to use as its value.
                        sqlCommand.Parameters.Add(new SqlParameter("@pLoginName", SqlDbType.NVarChar, 254));
                        sqlCommand.Parameters["@pLoginName"].Value = txtLoginName.Text;

                        // Add input parameter for the stored procedure and specify what to use as its value.
                        sqlCommand.Parameters.Add(new SqlParameter("@pPassword", SqlDbType.NVarChar, 50));
                        sqlCommand.Parameters["@pPassword"].Value = txtPassword.Text;

                        // Add the output parameter.
                        sqlCommand.Parameters.Add(new SqlParameter("@responseMessage", SqlDbType.NVarChar, 250));
                        sqlCommand.Parameters["@responseMessage"].Direction = ParameterDirection.Output;

                        try
                        {
                            connection.Open();
                            sqlCommand.ExecuteNonQuery();
                            var response = sqlCommand.Parameters["@responseMessage"].Value;
                            responseMessage = response.ToString();
                            //MessageBox.Show(responseMessage);
                        }
                        catch
                        {
                            MessageBox.Show("There was an error some where :(");
                        }
                        finally
                        {
                            connection.Close();
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Please Fill In All Fields");
            }
            validateResponseMessage();
        }

        // Validate Sign-In
        private void validateResponseMessage()
        {
            if (responseMessage == "Successful Login" || responseMessage.Equals("Successful Login"))
            {
                this.Hide();
                Dashboard dashboard = new Dashboard();
                dashboard.Closed += (s, args) => this.Close();
                dashboard.Show();
            }
            else
            {

            }
        }
    }
}
