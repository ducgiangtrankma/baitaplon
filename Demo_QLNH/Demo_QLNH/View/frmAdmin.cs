using Demo_QLNH.DAO;
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
        public frmAdmin()
        {
            InitializeComponent();
            Load();
        }
        #region methods
        void Load()
        {
            dtgvFood.DataSource = foodList;
            LoadListBillByDate(dtpkDateStart.Value, dtpkDateEnd.Value);// Load Bill theo mốc ngày
            LoadDateTimePickerBill();// Load ngày 
            LoadListFood();
            AddFoodBinding();
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
        void LoadCategoryIntoCombobox(ComboBox cb)// Load danh sách các Danh muc lên combobox
        {
            cb.DataSource = CategoryDAO.Instance.GetListCategory();
            cb.DisplayMember = "Name";
        }
        void AddFoodBinding()// Binding dữ liệu lên view
        {
            txtNameFood.DataBindings.Add(new Binding("Text", dtgvFood.DataSource, "Name",true, DataSourceUpdateMode.Never));
            txtIdFood.DataBindings.Add(new Binding("Text", dtgvFood.DataSource, "ID",true,DataSourceUpdateMode.Never));
            txtPriceFood.DataBindings.Add(new Binding("Text", dtgvFood.DataSource, "Price",true,DataSourceUpdateMode.Never));
         


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
                  int id = (int)dtgvFood.SelectedCells[0].OwningRow.Cells["CategoryID"].Value;// laays ra idCategory từ datagridView
                // Khi chọn vào 1 dòng trong view sẽ lấy ra các ô và lấy ra ô có tên như yêu cầu
                  CategoryDTO category = CategoryDAO.Instance.GetCategoryByID(id); // khởi tạo danh sách category
                 // cbFoodCategory.SelectedItem = category;
                int index = -1;
                int i = 0;
                foreach (CategoryDTO item in cbFoodCategory.Items)
                {
                    if (item.ID==category.ID)
                    {
                        index = i;
                        break;
                    }
                    i++;
                }
                cbFoodCategory.SelectedIndex = index; // lấy ra địa chỉ index
                

            }
            
        }
        private void btnAddFood_Click(object sender, EventArgs e)
        {
            string name = txtNameFood.Text;
            int idCategory = (cbFoodCategory.SelectedItem as CategoryDTO).ID;
            float price = float.Parse(txtPriceFood.Text);
            if (FoodDAO.Instance.InsertFood(name,idCategory,price))
            {
                MessageBox.Show(" Thêm thành công !");
                LoadListFood();
                if (insertFood !=null)
                {
                    insertFood(this, new EventArgs());
                }
            }
            else
            {
                MessageBox.Show(" Thêm thất bại !");
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
                    MessageBox.Show(" Sửa thành công !");
                    LoadListFood();
                    if (updateFood != null)
                    {
                        updateFood(this, new EventArgs());
                    }
                }
                else
                {
                    MessageBox.Show(" Sửa thất bại !");
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
                MessageBox.Show(" Xóa thành công !");
                LoadListFood();
                if (deleteFood !=null)
                {
                    deleteFood(this, new EventArgs());
                }
            }
            else
            {
                MessageBox.Show(" Xóa thất bại !");
            }
        }
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





        #endregion


    }
}
