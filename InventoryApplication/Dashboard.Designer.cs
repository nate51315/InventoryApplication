namespace InventoryApplication
{
    partial class Dashboard
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.menuItemUser = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemInventory = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemCustomers = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemCategories = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemOrders = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemCreateOrder = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemManageOrders = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItemUser,
            this.menuItemInventory,
            this.menuItemCustomers,
            this.menuItemCategories,
            this.menuItemOrders});
            this.menuStrip1.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.HorizontalStackWithOverflow;
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Padding = new System.Windows.Forms.Padding(6, 4, 0, 4);
            this.menuStrip1.Size = new System.Drawing.Size(1160, 40);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // menuItemUser
            // 
            this.menuItemUser.Name = "menuItemUser";
            this.menuItemUser.Size = new System.Drawing.Size(71, 32);
            this.menuItemUser.Text = "Users";
            this.menuItemUser.Click += new System.EventHandler(this.menuItemUser_Click);
            // 
            // menuItemInventory
            // 
            this.menuItemInventory.Name = "menuItemInventory";
            this.menuItemInventory.Size = new System.Drawing.Size(107, 32);
            this.menuItemInventory.Text = "Inventory";
            this.menuItemInventory.Click += new System.EventHandler(this.menuItemInventory_Click);
            // 
            // menuItemCustomers
            // 
            this.menuItemCustomers.Name = "menuItemCustomers";
            this.menuItemCustomers.Size = new System.Drawing.Size(116, 32);
            this.menuItemCustomers.Text = "Customers";
            this.menuItemCustomers.Click += new System.EventHandler(this.menuItemCustomers_Click);
            // 
            // menuItemCategories
            // 
            this.menuItemCategories.Name = "menuItemCategories";
            this.menuItemCategories.Size = new System.Drawing.Size(117, 32);
            this.menuItemCategories.Text = "Categories";
            this.menuItemCategories.Click += new System.EventHandler(this.menuItemCategories_Click);
            // 
            // menuItemOrders
            // 
            this.menuItemOrders.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItemCreateOrder,
            this.menuItemManageOrders});
            this.menuItemOrders.Name = "menuItemOrders";
            this.menuItemOrders.Size = new System.Drawing.Size(83, 32);
            this.menuItemOrders.Text = "Orders";
            // 
            // menuItemCreateOrder
            // 
            this.menuItemCreateOrder.Name = "menuItemCreateOrder";
            this.menuItemCreateOrder.Size = new System.Drawing.Size(225, 32);
            this.menuItemCreateOrder.Text = "Create Orders";
            this.menuItemCreateOrder.Click += new System.EventHandler(this.menuItemCreateOrder_Click);
            // 
            // menuItemManageOrders
            // 
            this.menuItemManageOrders.Name = "menuItemManageOrders";
            this.menuItemManageOrders.Size = new System.Drawing.Size(225, 32);
            this.menuItemManageOrders.Text = "Manage Orders";
            this.menuItemManageOrders.Click += new System.EventHandler(this.menuItemManageOrders_Click);
            // 
            // Dashboard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1160, 615);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Dashboard";
            this.Text = "Dashboard";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem menuItemInventory;
        private System.Windows.Forms.ToolStripMenuItem menuItemCustomers;
        private System.Windows.Forms.ToolStripMenuItem menuItemCategories;
        private System.Windows.Forms.ToolStripMenuItem menuItemOrders;
        private System.Windows.Forms.ToolStripMenuItem menuItemCreateOrder;
        private System.Windows.Forms.ToolStripMenuItem menuItemManageOrders;
        private System.Windows.Forms.ToolStripMenuItem menuItemUser;
    }
}