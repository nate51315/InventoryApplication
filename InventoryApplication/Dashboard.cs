using System;
using System.Windows.Forms;

namespace InventoryApplication
{
    public partial class Dashboard : Form
    {
        public Dashboard()
        {
            InitializeComponent();
        }

        private void menuItemCustomers_Click(object sender, EventArgs e)
        {
            Customers customersForm = new Customers();
            customersForm.Show();
        }

        private void menuItemInventory_Click(object sender, EventArgs e)
        {
            Inventory inventoryForm = new Inventory();
            inventoryForm.Show();
        }

        private void menuItemCategories_Click(object sender, EventArgs e)
        {
            Categories categoriesForm = new Categories();
            categoriesForm.Show();
        }

        private void menuItemCreateOrder_Click(object sender, EventArgs e)
        {
            CreateOrders ordersForm = new CreateOrders();
            ordersForm.Show();
        }

        private void menuItemUser_Click(object sender, EventArgs e)
        {
            Users usersForm = new Users();
            usersForm.Show();
        }

        private void menuItemManageOrders_Click(object sender, EventArgs e)
        {
            ManageOrders manageOrders = new ManageOrders();
            manageOrders.Show();
        }
    }
}
