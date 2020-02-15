using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace InventoryApplication
{
    public partial class CreateOrders : Form
    {
        public CreateOrders()
        {
            InitializeComponent();
        }

        DataTable customersDT = new DataTable("Customers");
        DataTable productsDT = new DataTable("Products");
        public int newID;

        // Load Customer Data
        private void loadCustomerDataFromDB()
        {
            using (SqlConnection connection = new SqlConnection(Properties.Settings.Default.connString))
            {
                String query = "SELECT * FROM customers";
                using (SqlCommand sqlCommand = new SqlCommand(query, connection))
                {
                    try
                    {
                        connection.Open();
                        SqlDataAdapter adapter = new SqlDataAdapter(sqlCommand);
                        adapter.Fill(customersDT);
                        dataGridCustomers.DataSource = customersDT;
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
                        adapter.Fill(productsDT);
                        dataGridProducts.DataSource = productsDT;
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

        // Form Load Event
        private void Orders_Form_Load(object sender, EventArgs e)
        {
            loadCustomerDataFromDB();
            loadProductsFromDB();
        }

        // Selects entire row - Customers Table
        private void dataGridCustomers_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridCustomers.SelectedRows.Count == 1)
            {
                txtCustomerID.Text = dataGridCustomers.SelectedCells[0].Value.ToString();
            }
            else
            {
                MessageBox.Show("Please select a row");
            }
        }

        // Selectes entire row - Products Table
        private void dataGridProducts_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridProducts.SelectedRows.Count == 1)
            {
                txtProductID.Text = dataGridProducts.SelectedCells[0].Value.ToString();
                txtProductName.Text = dataGridProducts.SelectedCells[1].Value.ToString();
                txtProductPrice.Text = dataGridProducts.SelectedCells[2].Value.ToString();
                //txtProductQuantity.Text = dataGridProducts.SelectedCells[3].Value.ToString();
                //comboCategories.Text = dataGridProducts.SelectedCells[4].Value.ToString();
            }
            else
            {
                MessageBox.Show("Please select a row");
            }
        }

        // Calculates Total Amount in current order
        private void calculateTotalAmount()
        {
            double price;
            int quantity;
            double totalAmount = 0;
            foreach (DataGridViewRow row in dataGridOrder.Rows)
            {
                price = double.Parse(row.Cells[2].Value.ToString());
                quantity = int.Parse(row.Cells[3].Value.ToString());
                totalAmount += price * (double)quantity;
            }
            txtTotalAmount.Text = totalAmount.ToString();
        }

        // Button - Deletes Product from Order
        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dataGridOrder.SelectedRows.Count == 1)
            {
                updateAfterProductDelete(dataGridOrder.SelectedCells[0].Value.ToString(), 
                                         dataGridOrder.SelectedCells[3].Value.ToString());
                dataGridOrder.Rows.RemoveAt(dataGridOrder.CurrentCell.RowIndex);
                calculateTotalAmount();
            }
            else
            {
                MessageBox.Show("Please select a row to delete");
            }
        }

        // Updates product grid after product removed from order
        private void updateAfterProductDelete(string id, string qty)
        {
            int productID = int.Parse(id);
            int productQuantity = int.Parse(qty);
            foreach (DataGridViewRow row in dataGridProducts.Rows)
            {
                if (row.Cells[0].Value.ToString().Equals(id))
                {
                    row.Cells[3].Value = int.Parse(row.Cells[3].Value.ToString()) + productQuantity;
                }
            }
        }

        // Button - Adds Product to Order
        private void btnAdd_Click(object sender, EventArgs e)
        {

            if (validateProductQuantity())
            {
                int result = validateOrder(txtProductID.Text);
                if (result == -1)
                {
                    dataGridOrder.Rows.Add(txtProductID.Text, txtProductName.Text, txtProductPrice.Text, txtOrderQuantity.Text);
                    updateDateProductsGrid(int.Parse(txtOrderQuantity.Text));
                    calculateTotalAmount();
                }
                else
                {
                    string temp = dataGridOrder.Rows[result].Cells[3].Value.ToString();
                    int value = (int.Parse(temp) + int.Parse(txtOrderQuantity.Text));
                    if (validateProductQuantity2(int.Parse(txtOrderQuantity.Text)))
                    {
                        updateDateProductsGrid(int.Parse(txtOrderQuantity.Text));
                        int gridValue = int.Parse(dataGridOrder.Rows[result].Cells[3].Value.ToString());
                        dataGridOrder.Rows[result].Cells[3].Value = gridValue + int.Parse(txtOrderQuantity.Text);
                        calculateTotalAmount();                      
                    }
                    else
                    {
                        MessageBox.Show("Error Invalid Quantity");
                    }
                }
            }
            else
            {
                MessageBox.Show("Error Invalid Quantity");
            }
        }

        // Updates products grid when items are added to order
        private void updateDateProductsGrid(int value)
        {
            int initialValue = int.Parse(dataGridProducts.SelectedCells[3].Value.ToString());
            dataGridProducts.SelectedCells[3].Value = (initialValue - value);
        }

        // Validates a duplicate product entry and the added quantities
        private Boolean validateProductQuantity2(int qtyAdd)
        {
            //if (value <= int.Parse(dataGridProducts.SelectedCells[3].Value.ToString()))
            if (qtyAdd <= int.Parse(dataGridProducts.SelectedCells[3].Value.ToString()))
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        // Validates quantity added is less than quantity in inventory
        private Boolean validateProductQuantity()
        {
            int.TryParse(txtOrderQuantity.Text, out int qty);
            if (qty > int.Parse(dataGridProducts.SelectedCells[3].Value.ToString()) || qty <= 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        // Validates Order Grid for Duplicate Entries
        private int validateOrder(string id)
        {
            int pos = -1;
            for (int i = 0; i < dataGridOrder.Rows.Count; i++)
            {
                //string value = dataGridOrder.Rows[i].Cells[0].Value.ToString();
                if (id == dataGridOrder.Rows[i].Cells[0].Value.ToString())
                {
                    pos = i;
                }
            }
            return pos;
        }

        // Search Filter - Customers
        private void txtCustomerSearch_TextChanged(object sender, EventArgs e)
        {
            customersDT.DefaultView.RowFilter = string.Format("FirstName LIKE '%{0}%'", txtCustomerSearch.Text);
        }

        // Search Filter - Products
        private void txtProductSeatch_TextChanged(object sender, EventArgs e)
        {
            productsDT.DefaultView.RowFilter = string.Format("ProductName LIKE '%{0}%'", txtProductSearch.Text);
        }

        // Button - Inserts into Orders table
        private void btnCreateOrder_Click(object sender, EventArgs e)
        {
           
            if (validOrder())
            { 
                using (SqlConnection connection = new SqlConnection(Properties.Settings.Default.connString))
                {
                    string query = "INSERT INTO orders(CustomerID, OrderDate, TotalAmount) VALUES(@CustomerID, @OrderDate, @TotalAmount); SELECT CAST(scope_identity() AS int)";
                    using (SqlCommand sqlCommand = new SqlCommand(query, connection))
                    {
                        try
                        {
                            sqlCommand.CommandType = CommandType.Text;
                            sqlCommand.Parameters.AddWithValue("@CustomerID", int.Parse(txtCustomerID.Text));
                            sqlCommand.Parameters.AddWithValue("@OrderDate", datePicker.Value.Date);
                            sqlCommand.Parameters.AddWithValue("@TotalAmount", double.Parse(txtTotalAmount.Text));
                            connection.Open();
                            newID = (int)sqlCommand.ExecuteScalar();
                            //MessageBox.Show("Successfully Created New Order");
                        }
                        catch
                        {
                            MessageBox.Show("ERROR while creating new Order");
                        }
                        finally
                        {
                            connection.Close();
                        }
                    }
                    
                }
                insertIntoSalesTable();
                updateProductsAfterOrderCreation();
            }
            else
            {
                MessageBox.Show("Please select a CustomerID or add Products to order");
            }
            
        }

        // Grabs OrderID - Inserts into Sales Table
        private void insertIntoSalesTable()
        {
            using (SqlConnection connection = new SqlConnection(Properties.Settings.Default.connString))
            {
                string query = "INSERT INTO sales(OrderID, FirstName, ProductName, Quantity, TotalPrice) VALUES(@OrderID, @FirstName, @ProductName, @Quantity, @TotalPrice)";
                for (int i=0; i < dataGridOrder.Rows.Count; i++)
                {
                    SqlCommand sqlCommand = new SqlCommand(query, connection);
                    var p = dataGridOrder.Rows[i].Cells[2].Value;
                    double price = Convert.ToDouble(p);
                    var q = dataGridOrder.Rows[i].Cells[3].Value;
                    int qty = Convert.ToInt32(q);
                    sqlCommand.Parameters.AddWithValue("@OrderID", newID);
                    sqlCommand.Parameters.AddWithValue("@FirstName", dataGridCustomers.SelectedCells[1].Value.ToString());
                    sqlCommand.Parameters.AddWithValue("@ProductName", dataGridOrder.Rows[i].Cells[1].Value);
                    sqlCommand.Parameters.AddWithValue("@Quantity", qty);
                    sqlCommand.Parameters.AddWithValue("@TotalPrice", (double)qty * price);
                    connection.Open();
                    sqlCommand.ExecuteNonQuery();
                    connection.Close();
                }
                
            }
        }

        // Updates Product Table when Order is created
        private void updateProductsAfterOrderCreation()
        {

            List<int> productID = new List<int>();
            int ID;
            for (int i=0; i<dataGridOrder.Rows.Count; i++)
            {
                ID = int.Parse(dataGridOrder.Rows[i].Cells[0].Value.ToString());
                productID.Add(ID);
            }

            List<int> productQty = new List<int>();
            int qty;
            for (int i=0; i<productID.Count; i++)
            {
                foreach (DataGridViewRow row in dataGridProducts.Rows)
                {
                    if (row.Cells[0].Value.ToString().Equals(productID[i].ToString()))
                    {
                        qty = int.Parse(row.Cells[3].Value.ToString());
                        productQty.Add(qty);
                        break;
                    }

                }
            }
            
            using (SqlConnection connection = new SqlConnection(Properties.Settings.Default.connString))
            {
                string query = "UPDATE products SET ProductQuantity=@ProductQuantity WHERE ProductID=@ProductID";
                for (int i=0; i<productID.Count; i++)
                {
                    SqlCommand sqlCommand = new SqlCommand(query, connection);                 
                    sqlCommand.CommandType = CommandType.Text;
                    sqlCommand.Parameters.AddWithValue("@ProductQuantity", productQty[i]);
                    sqlCommand.Parameters.AddWithValue("@ProductID", productID[i]);
                    connection.Open();
                    sqlCommand.ExecuteNonQuery();
                    connection.Close();

                }
                MessageBox.Show("Successfully Created New Order");
            }
            dataGridOrder.Rows.Clear();
        }

        // Validation of fields
        private Boolean validOrder()
        {
            if (txtCustomerID.Text != "" && dataGridOrder.Rows.Count != 0 && txtTotalAmount.Text != "")
            {
                return true;
            }
            else
            {
                return false;
            }
        }

    }
}
