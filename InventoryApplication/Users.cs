using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace InventoryApplication
{
    public partial class Users : Form
    {
        public Users()
        {
            InitializeComponent();
        }

        private DataTable userDataTable = new DataTable("User");

        // Clear All Fields
        private void clearFields()
        {
            txtFirstName.Text = "";
            txtLastName.Text = "";
            txtLoginName.Text = "";
            txtPassword.Text = "";
            txtUserID.Text = "";
            txtUserSearch.Text = "";
        }

        // Load User Data From Database
        private void loadUserDataFromDB()
        {
            using (SqlConnection connection = new SqlConnection(Properties.Settings.Default.connString))
            {
                string query = "SELECT DISTINCT UserID, LoginName, FirstName, LastName FROM [dbo].[Users]";
                using (SqlCommand sqlCommand = new SqlCommand(query, connection))
                {
                    try
                    {
                        connection.Open();
                        SqlDataAdapter adapter = new SqlDataAdapter(sqlCommand);
                        adapter.Fill(userDataTable);
                        dataGridUser.DataSource = userDataTable;
                    }
                    catch
                    {
                        MessageBox.Show("ERROR while loading User data from Database");
                    }
                    finally
                    {
                        connection.Close();
                    }

                }
            }
        }

        // Form Loading Event
        private void User_Load(object sender, EventArgs e)
        {
            loadUserDataFromDB();
        }

        // Refresh
        private void btnRefreshUsers_Click(object sender, EventArgs e)
        {
            userDataTable.Clear();
            loadUserDataFromDB();
        }

        // Clear All Fields
        private void btnClearFields_Click(object sender, EventArgs e)
        {
            clearFields();
        }

        // Selects All Row Values - Fills Fields
        private void dataGridUser_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridUser.SelectedRows.Count == 1)
            {
                txtUserID.Text = dataGridUser.SelectedCells[0].Value.ToString();
                txtLoginName.Text = dataGridUser.SelectedCells[1].Value.ToString();
                txtFirstName.Text = dataGridUser.SelectedCells[2].Value.ToString();
                txtLastName.Text = dataGridUser.SelectedCells[3].Value.ToString();
                //txtPassword.Text = dataGridUser.SelectedCells[4].Value.ToString();
            }
            else
            {
                MessageBox.Show("Please select a row");
            }
        }

        // Validate User
        private Boolean validateUser()
        {
            if (txtUserID.Text != "" && txtFirstName.Text != "" && txtLastName.Text != "" && txtLoginName.Text != "")
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        // CREATE
        private void btnCreateNewUser_Click(object sender, EventArgs e)
        {
            if (validateUser() && txtPassword.Text != "")
            {
                using (SqlConnection connection = new SqlConnection(Properties.Settings.Default.connString))
                {
                    // Create a SqlCommand, and identify it as a stored procedure.
                    using (SqlCommand sqlCommand = new SqlCommand("dbo.uspAddUser", connection))
                    {
                        sqlCommand.CommandType = CommandType.StoredProcedure;

                        // LoginName
                        sqlCommand.Parameters.Add(new SqlParameter("@pLogin", SqlDbType.NVarChar, 50));
                        sqlCommand.Parameters["@pLogin"].Value = txtLoginName.Text;

                        // Password
                        sqlCommand.Parameters.Add(new SqlParameter("@pPassword", SqlDbType.NVarChar, 50));
                        sqlCommand.Parameters["@pPassword"].Value = txtPassword.Text;

                        // First Name
                        sqlCommand.Parameters.Add(new SqlParameter("@pFirstName", SqlDbType.NVarChar, 40));
                        sqlCommand.Parameters["@pFirstName"].Value = txtFirstName.Text;

                        // Last Name
                        sqlCommand.Parameters.Add(new SqlParameter("@pLastName", SqlDbType.NVarChar, 40));
                        sqlCommand.Parameters["@pLastName"].Value = txtLastName.Text;

                        // Add the OUTPUT parameter.
                        sqlCommand.Parameters.Add(new SqlParameter("@responseMessage", SqlDbType.NVarChar, 250));
                        sqlCommand.Parameters["@responseMessage"].Direction = ParameterDirection.Output;

                        try
                        {
                            connection.Open();
                            sqlCommand.ExecuteNonQuery();
                            var response = sqlCommand.Parameters["@responseMessage"].Value;
                            string responseMessage = response.ToString();
                            clearFields();
                            userDataTable.Clear();
                            loadUserDataFromDB();
                            MessageBox.Show(responseMessage);
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
        }

        // UPDATE - USER INFO
        private void btnUpdateUserInfo_Click(object sender, EventArgs e)
        {
            if (validateUser() && txtPassword.Text == "")
            {
                using (SqlConnection connection = new SqlConnection(Properties.Settings.Default.connString))
                {
                    string query = "UPDATE [dbo].[Users] SET LoginName=@LoginName, FirstName=@FirstName, LastName=@LastName WHERE UserID=@UserID";
                    using (SqlCommand sqlCommand = new SqlCommand(query, connection))
                    {
                        try
                        {
                            connection.Open();
                            sqlCommand.CommandType = CommandType.Text;
                            sqlCommand.Parameters.AddWithValue("@LoginName", txtLoginName.Text);
                            sqlCommand.Parameters.AddWithValue("@FirstName", txtFirstName.Text);
                            sqlCommand.Parameters.AddWithValue("@LastName", txtLastName.Text);
                            sqlCommand.Parameters.AddWithValue("@UserID", int.Parse(txtUserID.Text));
                            sqlCommand.ExecuteNonQuery();
                            userDataTable.Clear();
                            loadUserDataFromDB();
                            MessageBox.Show("Successfully Updated User Info");
                        }
                        catch
                        {
                            MessageBox.Show("ERROR");
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
                MessageBox.Show("Please fill all fields EXCEPT Password");
            }
        }

        // UPDATE - USER PASSWORD
        private void btnUpdateUserPassword_Click(object sender, EventArgs e)
        {
            if (validateUser() && txtPassword.Text != "")
            {
                using (SqlConnection connection = new SqlConnection(Properties.Settings.Default.connString))
                {
                    // Create a SqlCommand, and identify it as a stored procedure.
                    using (SqlCommand sqlCommand = new SqlCommand("dbo.uspUpdateUser", connection))
                    {
                        sqlCommand.CommandType = CommandType.StoredProcedure;

                        // User ID
                        sqlCommand.Parameters.Add(new SqlParameter("@pUserID", SqlDbType.Int));
                        sqlCommand.Parameters["@pUserID"].Value = int.Parse(txtUserID.Text);

                        // Password
                        sqlCommand.Parameters.Add(new SqlParameter("@pPassword", SqlDbType.NVarChar, 50));
                        sqlCommand.Parameters["@pPassword"].Value = txtPassword.Text;

                        // Add the OUTPUT parameter.
                        sqlCommand.Parameters.Add(new SqlParameter("@responseMessage", SqlDbType.NVarChar, 250));
                        sqlCommand.Parameters["@responseMessage"].Direction = ParameterDirection.Output;

                        try
                        {
                            connection.Open();
                            // Run the stored procedure.
                            sqlCommand.ExecuteNonQuery();
                            var response = sqlCommand.Parameters["@responseMessage"].Value;
                            string responseMessage = response.ToString();
                            clearFields();
                            MessageBox.Show(responseMessage);
                        }
                        catch
                        {
                            MessageBox.Show("There was an error updating user password");
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
        }

        // DELETE
        private void btnDeleteUser_Click(object sender, EventArgs e)
        {
            if (validateUser())
            {
                DialogResult dialogResult = MessageBox.Show("Are you sure you want to delete this User?", "Warning", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    using (SqlConnection connection = new SqlConnection(Properties.Settings.Default.connString))
                    {
                        string query = "DELETE FROM [dbo].[Users] WHERE UserID=@UserID";
                        using (SqlCommand sqlCommand = new SqlCommand(query, connection))
                        {
                            try
                            {
                                connection.Open();
                                sqlCommand.CommandType = CommandType.Text;
                                sqlCommand.Parameters.AddWithValue("@UserID", int.Parse(txtUserID.Text));
                                sqlCommand.ExecuteNonQuery();
                                userDataTable.Clear();
                                loadUserDataFromDB();
                                MessageBox.Show("Successfully Deleted User");
                            }
                            catch
                            {
                                MessageBox.Show("ERROR while deleting from User table");
                            }
                            finally
                            {
                                connection.Close();
                            }

                        }
                    }
                }
                else if (dialogResult == DialogResult.No)
                {

                }

            }
            else
            {
                MessageBox.Show("Please Fill In All Fields");
            }
        }

        // Search Filter
        private void txtUserSearch_TextChanged(object sender, EventArgs e)
        {
            userDataTable.DefaultView.RowFilter = string.Format("LoginName LIKE '{0}%'", txtUserSearch.Text);
        }

    }
}
