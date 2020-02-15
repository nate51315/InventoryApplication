using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace InventoryApplication
{
    public partial class Inventory : Form
    {
        public Inventory()
        {
            InitializeComponent();
        }

        DataTable productsDataTable = new DataTable("Products");

        // Load Products
        private void loadProductsFromDB()
        {
            using (SqlConnection connection = new SqlConnection(Properties.Settings.Default.connString))
            {
                String query = "SELECT * FROM products";
                using (SqlCommand sqlCommand = new SqlCommand(query, connection))
                {
                    try
                    {
                        connection.Open();
                        SqlDataAdapter adapter = new SqlDataAdapter(sqlCommand);
                        adapter.Fill(productsDataTable);
                        dataGridProducts.DataSource = productsDataTable;
                    }
                    catch
                    {
                        MessageBox.Show("ERROR while loading users from database");
                    }
                    finally
                    {
                        connection.Close();
                    }

                }
            }
        }

        // Load Categories
        private void loadCategoriesFromDB()
        {
            using (SqlConnection connection = new SqlConnection(Properties.Settings.Default.connString))
            {
                String query = "SELECT * FROM categories";
                using (SqlCommand sqlCommand = new SqlCommand(query, connection))
                {
                    try
                    {
                        connection.Open();
                        SqlDataReader dataReader;
                        dataReader = sqlCommand.ExecuteReader();
                        while (dataReader.Read())
                        {
                            comboCategories.Items.Add(dataReader.GetString(1));
                        }
                    }
                    catch
                    {
                        MessageBox.Show("ERROR while loading categories to combobox");
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }
        }

        // Form Loading Event
        private void Inventory_Form_Load(object sender, EventArgs e)
        {
            loadProductsFromDB();
            loadCategoriesFromDB();
        }

        // Refresh
        private void btnRefreshProducts_Click(object sender, EventArgs e)
        {
            productsDataTable.Clear();
            loadProductsFromDB();
            loadCategoriesFromDB();
        }

        // Clear Fields
        private void btnClearFields_Click(object sender, EventArgs e)
        {
            txtProductID.Text = "";
            txtProductName.Text = "";
            txtProductPrice.Text = "";
            txtProductQuantity.Text = "";
            comboCategories.Text = "";
            txtProductSearch.Text = "";
        }

        // Selects All Row Values - Fills Fields
        private void dataGridProducts_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridProducts.SelectedRows.Count == 1)
            {
                txtProductID.Text = dataGridProducts.SelectedCells[0].Value.ToString();
                txtProductName.Text = dataGridProducts.SelectedCells[1].Value.ToString();
                txtProductPrice.Text = dataGridProducts.SelectedCells[2].Value.ToString();
                txtProductQuantity.Text = dataGridProducts.SelectedCells[3].Value.ToString();
                comboCategories.Text = dataGridProducts.SelectedCells[4].Value.ToString();
            }
            else
            {
                MessageBox.Show("Please select a row");
            }
        }

        // Validate Product
        private Boolean validateProduct()
        {
            if (txtProductName.Text != "" && comboCategories.Text != "" && double.TryParse(txtProductPrice.Text, out double price) && int.TryParse(txtProductQuantity.Text, out int qty))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        // CREATE
        private void btnCreateNewProduct_Click(object sender, EventArgs e)
        {
            if (validateProduct())
            {
                using (SqlConnection connection = new SqlConnection(Properties.Settings.Default.connString))
                {
                    String query = "INSERT INTO products(ProductName, ProductPrice, ProductQuantity, CategoryName) VALUES(@ProductName, @ProductPrice, @ProductQuantity, @CategoryName)";
                    using (SqlCommand sqlCommand = new SqlCommand(query, connection))
                    {
                        try
                        {
                            connection.Open();
                            sqlCommand.CommandType = CommandType.Text;
                            sqlCommand.Parameters.AddWithValue("@ProductName", txtProductName.Text);
                            sqlCommand.Parameters.AddWithValue("@ProductPrice", double.Parse(txtProductPrice.Text));
                            sqlCommand.Parameters.AddWithValue("@ProductQuantity", int.Parse(txtProductQuantity.Text));
                            sqlCommand.Parameters.AddWithValue("@CategoryName", comboCategories.Text);
                            sqlCommand.ExecuteNonQuery();
                            productsDataTable.Clear();
                            loadProductsFromDB();
                            MessageBox.Show("Successfully Added New Product");
                        }
                        catch
                        {
                            MessageBox.Show("ERROR while loading users from database");
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
                MessageBox.Show("Invalid fields");
            }
        }

        // UPDATE
        private void btnUpdateProduct_Click(object sender, EventArgs e)
        {
            if (validateProduct())
            {
                using (SqlConnection connection = new SqlConnection(Properties.Settings.Default.connString))
                {
                    String query = "UPDATE products SET ProductName=@ProductName, ProductPrice=@ProductPrice, ProductQuantity=@ProductQuantity, CategoryName=@CategoryName WHERE ProductID=@ProductID";
                    using (SqlCommand sqlCommand = new SqlCommand(query, connection))
                    {
                        try
                        {
                            connection.Open();
                            sqlCommand.CommandType = CommandType.Text;
                            sqlCommand.Parameters.AddWithValue("@ProductName", txtProductName.Text);
                            sqlCommand.Parameters.AddWithValue("@ProductPrice", double.Parse(txtProductPrice.Text));
                            sqlCommand.Parameters.AddWithValue("@ProductQuantity", int.Parse(txtProductQuantity.Text));
                            sqlCommand.Parameters.AddWithValue("@CategoryName", comboCategories.Text);
                            sqlCommand.Parameters.AddWithValue("@ProductID", int.Parse(txtProductID.Text));
                            sqlCommand.ExecuteNonQuery();
                            productsDataTable.Clear();
                            loadProductsFromDB();
                            MessageBox.Show("Successfully Updated Product");
                        }
                        catch
                        {
                            MessageBox.Show("ERROR while updating");
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
                MessageBox.Show("Invalid fields");
            }
        }

        // DELETE
        private void btnDeleteProduct_Click(object sender, EventArgs e)
        {
            if (validateProduct())
            {
                DialogResult dialogResult = MessageBox.Show("Are you sure you want to delete this product?", "Warning", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    using (SqlConnection connection = new SqlConnection(Properties.Settings.Default.connString))
                    {
                        String query = "DELETE FROM products WHERE ProductID=@ProductID";
                        using (SqlCommand sqlCommand = new SqlCommand(query, connection))
                        {
                            try
                            {
                                connection.Open();
                                sqlCommand.CommandType = CommandType.Text;
                                sqlCommand.Parameters.AddWithValue("@ProductID", int.Parse(txtProductID.Text));
                                sqlCommand.ExecuteNonQuery();
                                productsDataTable.Clear();
                                loadProductsFromDB();
                                MessageBox.Show("Successfully Deleted Product");
                            }
                            catch
                            {
                                MessageBox.Show("ERROR while deleting");
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
                MessageBox.Show("Invalid fields");
            }
        }

        // Search Filter
        private void txtProductSearch_TextChanged(object sender, EventArgs e)
        {
            productsDataTable.DefaultView.RowFilter = string.Format("ProductName LIKE '%{0}%'", txtProductSearch.Text);
        }

    }
}
