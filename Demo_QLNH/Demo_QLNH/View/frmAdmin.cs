﻿using Demo_QLNH.DAO;
using Demo_QLNH.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Demo_QLNH.From
{
    public partial class frmAdmin : Form
    {
        BindingSource foodList = new BindingSource();// Xử lí việc soure bị thay đổi do dùng binding. Bug khi bấm xem không load được index
        BindingSource accountList = new BindingSource();
        BindingSource categoryList = new BindingSource();
        BindingSource tableList = new BindingSource();
        public AccountDTO loginAccount;
        public frmAdmin()
        {
            InitializeComponent();
            Load();
        }
        #region methods

        void Load()
        {
            dtgvAccount.DataSource = accountList;
            dtgvFood.DataSource = foodList;
            dtgvCategory.DataSource = categoryList;
            LoadListBillByDate(dtpkDateStart.Value, dtpkDateEnd.Value);// Load Bill theo mốc ngày
            LoadDateTimePickerBill();// Load ngày 
            LoadListFood();
            LoadListCategory();
            loadListAccount();
                            loadlisttable();
            AddFoodBinding();
            AddAccountBinDing();
            AddCategoryBinDing();
                           AddTableBinding();         
            LoadCategoryIntoCombobox(cbFoodCategory);
        }
        void LoadDateTimePickerBill()// Load time
        {
            DateTime today = DateTime.Now;
            dtpkDateStart.Value = new DateTime(today.Year, today.Month, 1);
            dtpkDateEnd.Value = dtpkDateStart.Value.AddMonths(1).AddDays(-1);
        }
        void LoadListBillByDate(DateTime checkIn, DateTime checkOut)// Hàm load thông tin bill lên datagridview
        {
            dtgvBill.DataSource = BillDAO.Instance.GetBillListByDate(checkIn, checkOut);//gán data soure cho datagridView
        }
        void LoadListFood()//Load Danh sách thức ăn
        {
            foodList.DataSource = FoodDAO.Instance.GetListFood();
        }
        void LoadListCategory()
        {
            categoryList.DataSource = CategoryDAO.Instance.GetListCategory();
        }
        void loadlisttable()
         {
            tableList.DataSource = TableDAO.Instance.GetListTable();
         }
        void loadListAccount()
        {
            accountList.DataSource = AccountDAO.Instance.GetListAccount();
        }
        void LoadCategoryIntoCombobox(ComboBox cb)// Load danh sách các Danh muc lên combobox
        {
            cb.DataSource = CategoryDAO.Instance.GetListCategory();
            cb.DisplayMember = "Name";
        }
        void AddFoodBinding()// Binding dữ liệu lên view
        {
            txtNameFood.DataBindings.Add(new Binding("Text", dtgvFood.DataSource, "Name",true, DataSourceUpdateMode.Never)); // true , ... never để không truyền ngược dữ liệu từ textbox về view khi chưa goi event
            txtIdFood.DataBindings.Add(new Binding("Text", dtgvFood.DataSource, "ID",true,DataSourceUpdateMode.Never));
            txtPriceFood.DataBindings.Add(new Binding("Text", dtgvFood.DataSource, "Price",true,DataSourceUpdateMode.Never));
         
        }
        void AddAccountBinDing()
        {
            txtNameAccount.DataBindings.Add(new Binding("Text", dtgvAccount.DataSource, "UserName", true, DataSourceUpdateMode.Never));
            txtNameShowAccount.DataBindings.Add(new Binding("Text", dtgvAccount.DataSource, "DisplayName", true, DataSourceUpdateMode.Never));
            nbType.DataBindings.Add(new Binding("Value", dtgvAccount.DataSource, "Type", true, DataSourceUpdateMode.Never));


        }
        void AddCategoryBinDing()
        {
            txtIdCategory.DataBindings.Add(new Binding("Text", dtgvCategory.DataSource, "id", true, DataSourceUpdateMode.Never));
            txtCategory.DataBindings.Add(new Binding("Text", dtgvCategory.DataSource, "name", true, DataSourceUpdateMode.Never));
        }
        void AddTableBinding()
        {
            txtIdTable.DataBindings.Add(new Binding("Text", dtgvTable.DataSource, "id", true, DataSourceUpdateMode.Never));
            txtNameTable.DataBindings.Add(new Binding("Text", dtgvTable.DataSource, "name", true, DataSourceUpdateMode.Never));
            txtSatusTable.DataBindings.Add(new Binding("Text", dtgvTable.DataSource, "status", true, DataSourceUpdateMode.Never));
        }


        void AddAccount (string userName , string disPlayName, int type)
        {
            try
            {
                if (AccountDAO.Instance.InsertAccount(userName, disPlayName, type))
                {
                    MessageBox.Show(" Thêm Tài Khoản Thành Công", "Thông Báo");
                }
                else
                {
                    MessageBox.Show(" Thêm Thất Bại", "Thông Báo");
                }
                loadListAccount();
            }
            catch (Exception)
            {

                MessageBox.Show("Bạn chưa điền đủ thông tin !","Thông Báo");
            }
            
          
        }
        void EditAccount(string userName, string disPlayName, int type)
        {
            try
            {
                if (AccountDAO.Instance.UpdateAccount(userName, disPlayName, type))
                {
                    MessageBox.Show(" Sửa Tài Khoản Thành Công", "Thông Báo");
                }
                else
                {
                    MessageBox.Show(" Sửa Thất Bại", "Thông Báo");
                }
                loadListAccount();
            }
            catch (Exception)
            {

                MessageBox.Show("Bạn chưa điền đủ thông tin !", "Thông Báo");
            }
            

        }
        void DeleteAccount(string userName)
        {
            
            if (loginAccount.UserName.Equals(userName))
            {
                MessageBox.Show("Không thể xóa tài khoản đang đăng nhập !","Thông Báo");
                return;
            }
            if (AccountDAO.Instance.DeleteAccount(userName))
            {
                MessageBox.Show("Xóa tài khoản thành công","Thông Báo");
            }
            else
            {
                MessageBox.Show("Xóa tài khoản thất bại","Thông Báo");
            }

            loadListAccount();
        }

        void ResetPass(string userName)
        {
            if (AccountDAO.Instance.ResetPassword(userName))
            {
                MessageBox.Show("Đặt lại mật khẩu thành công","Thông Báo");
            }
            else
            {
                MessageBox.Show("Đặt lại mật khẩu thất bại","Thông Báo");
            }
        }
        List<FoodDTO> SearchFoodByName (string nameFood)
        {
            List<FoodDTO> listFood = FoodDAO.Instance.SearchFoodByName(nameFood);

            return listFood;
        }
        List<CategoryDTO> SearchFoodCategoryByName(string nameCategory)
        {
            List<CategoryDTO> listCategory = CategoryDAO.Instance.SearchCategoryByName(nameCategory);

            return listCategory;
        }

        #endregion


        #region even
        private void btnViewBill_Click(object sender, EventArgs e)
        {
            LoadListBillByDate(dtpkDateStart.Value, dtpkDateEnd.Value);
        }
        private void btnShowFood_Click(object sender, EventArgs e)
        {
            LoadListFood();
        }
        private void txtIdFood_TextChanged(object sender, EventArgs e)//Lấy data từ gridview 
            
        {

            if (dtgvFood.SelectedCells.Count >0)
            {
                try
                {
                    int id = (int)dtgvFood.SelectedCells[0].OwningRow.Cells["CategoryID"].Value;// laays ra idCategory từ datagridView
                                                                                                // Khi chọn vào 1 dòng trong view sẽ lấy ra các ô và lấy ra ô có tên như yêu cầu
                    CategoryDTO category = CategoryDAO.Instance.GetCategoryByID(id); // khởi tạo danh sách category
                                                                                     // cbFoodCategory.SelectedItem = category;
                    int index = -1;
                    int i = 0;
                    foreach (CategoryDTO item in cbFoodCategory.Items)
                    {
                        if (item.ID == category.ID)
                        {
                            index = i;
                            break;
                        }
                        i++;
                    }
                    cbFoodCategory.SelectedIndex = index; // lấy ra địa chỉ index

                }
                catch 
                {
                  
                }
                  

            }
            
        }
        private void btnAddFood_Click(object sender, EventArgs e)
        {
            try
            {
                string name = txtNameFood.Text;
                int idCategory = (cbFoodCategory.SelectedItem as CategoryDTO).ID;
                float price = float.Parse(txtPriceFood.Text);
                if (FoodDAO.Instance.InsertFood(name, idCategory, price))
                {
                    MessageBox.Show(" Thêm món thành công !","Thông Báo");
                    LoadListFood();
                    if (insertFood != null)
                    {
                        insertFood(this, new EventArgs());
                    }
                }
                else
                {
                    MessageBox.Show(" Thêm món thất bại !","Thông Báo");
                }
            }
            catch (Exception)
            {

                MessageBox.Show("Bạn chưa nhập đủ thông tin !", "Thông Báo");
            }
           
        }
        private void btnEditFood_Click(object sender, EventArgs e)
        {

            try
            {
                int idFood = int.Parse(txtIdFood.Text);
                string name = txtNameFood.Text;
                int idCategory = (cbFoodCategory.SelectedItem as CategoryDTO).ID;
                float price = float.Parse(txtPriceFood.Text);
                if (FoodDAO.Instance.UpdateFood(idFood, name, idCategory, price))
                {
                    MessageBox.Show(" Sửa món thành công !","Thông Báo");
                    LoadListFood();
                    if (updateFood != null)
                    {
                        updateFood(this, new EventArgs());
                    }
                }
                else
                {
                    MessageBox.Show(" Sửa món thất bại !","Thông Báo");
                }
            }
            catch (Exception)
            {

                MessageBox.Show("Bạn chưa nhập đủ thông tin !", "Thông Báo");
            }
                
        
        }
        private void btnDeleteFood_Click(object sender, EventArgs e)
        {
            int idFood = int.Parse(txtIdFood.Text);
            if (FoodDAO.Instance.DeleteFood(idFood))
            {
                MessageBox.Show(" Xóa thành công !","Thông Báo");
                LoadListFood();
                if (deleteFood !=null)
                {
                    deleteFood(this, new EventArgs());
                }
            }
            else
            {
                MessageBox.Show(" Xóa thất bại !","Thông Báo");
            }
         
        }
        private void btnAddCategory_Click(object sender, EventArgs e)
        {
            try
            {
                string nameCategory = txtCategory.Text;
                if (CategoryDAO.Instance.InsetCategory(nameCategory))
                {
                    MessageBox.Show("Thêm danh mục thành công !", "Thông báo !");
                    LoadListCategory();
                    if (insertCategory != null)
                    {
                        insertCategory(this, new EventArgs());                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        private void btnEditCategory_Click(object sender, EventArgs e)
        {
            try
            {
                int idCategory = int.Parse(txtIdCategory.Text);
                string nameCategory = txtCategory.Text;
                if (CategoryDAO.Instance.UpdateCategory(idCategory,nameCategory))
                {
                    MessageBox.Show(" Sửa danh mục thành công !", "Thông Báo");
                    LoadListCategory();
                    if (updateCategory != null)
                    {
                       updateCategory(this, new EventArgs());
                    }
                }
            }
            catch (Exception)
            {

                MessageBox.Show(" Sửa danh mục thất bại !", "Thông Báo");
            }
            
        }
        private void btnSearchCategory_Click(object sender, EventArgs e)
        {
            categoryList.DataSource = SearchFoodCategoryByName(txtSearchCategory.Text);
        }
        private void btnDeleteCategory_Click(object sender, EventArgs e)
        {
            
        }
        

        private void btnSearchFood_Click(object sender, EventArgs e)
        {
           foodList.DataSource = SearchFoodByName(txtSearchFood.Text);
        }

        private void btnShowAccount_Click(object sender, EventArgs e)
        {
            loadListAccount();
        }
        private void btnShowCategory_Click(object sender, EventArgs e)
        {
           
            LoadListCategory();
        }

        private void btnAddAccount_Click(object sender, EventArgs e)
        {
            string userName = txtNameAccount.Text;
            string disPlayName = txtNameShowAccount.Text;
            int type = (int)nbType.Value;
            AddAccount(userName, disPlayName, type);
        }
        private void btnDeleteAccount_Click(object sender, EventArgs e)
        {
            string userName = txtNameAccount.Text;
            DeleteAccount(userName);
        }

        private void btnEditAccount_Click(object sender, EventArgs e)
        {
            string userName = txtNameAccount.Text;
            string disPlayName = txtNameShowAccount.Text;
            int type = (int)nbType.Value;
            EditAccount(userName , disPlayName, type);
        }
        private void btnResetPassWord_Click(object sender, EventArgs e)
        {
            string userName = txtNameAccount.Text;
            ResetPass(userName);
        }

        // Cac even cap 2
        // Tạo các even insert, update, delete Food
        private event EventHandler insertFood;
        public event EventHandler InsertFood
        {
            add { insertFood += value; }
            remove { insertFood -= value; }
        }

        private event EventHandler deleteFood;
        public event EventHandler DeleteFood
        {
            add { deleteFood += value; }
            remove { deleteFood -= value; }
        }

        private event EventHandler updateFood;
        public event EventHandler UpdateFood
        {
            add { updateFood += value; }
            remove { updateFood -= value; }
        }
        // Tạo các even insert, update, delete Table
        private event EventHandler insertTable;
        public event EventHandler InsertTable
        {
            add { insertTable += value; }
            remove { insertTable -= value; }
        }

        private event EventHandler deleteTable;
        public event EventHandler DeleteTable
        {
            add { deleteTable += value; }
            remove { deleteTable -= value; }
        }

        private event EventHandler updateTable;
        public event EventHandler UpdateTable
        {
            add { updateTable += value; }
            remove { updateTable -= value; }
        }
        // Tạo các even insert, update, delete Category
        private event EventHandler insertCategory;
        public event EventHandler InsertCategory
        {
            add { insertCategory += value; }
            remove { insertCategory -= value; }
        }

        private event EventHandler deleteCategory;
        public event EventHandler DeleteCategory
        {
            add { deleteCategory += value; }
            remove { deleteCategory -= value; }
        }

        private event EventHandler updateCategory;
        public event EventHandler UpdateCategory
        {
            add { updateCategory += value; }
            remove { updateCategory -= value; }
        }













        #endregion


    }
}
