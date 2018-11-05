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
    public partial class frmTableManager : Form
    {
        private AccountDTO loginAccount;// Hàm Dựng account

        public AccountDTO LoginAccount
        {
            get { return loginAccount; }
            set { loginAccount = value; ChangeAccount(loginAccount.Type); }
        }

        

        public frmTableManager(AccountDTO acc)
        {
          
            InitializeComponent();
            this.LoginAccount = acc;// truyền tài khoản đăng nhập vào formd dể kiểm bước tiếp theo kiểm tra xem có phải tài khaorn admin không.
            LoadTable();// Load danh sách bàn khi load from
            LoadCategory();// Load danh sách Category khi load From
            LoadComboboxTable(cbSwithTable);// Load Danh sách bàn lên CBox để chọn chuyển bàn
            
        }
        #region Method
        private void ChangeAccount(int type)//Hàm kiểm tra loại tài khoản để bật menu Admin(Chi admin mới được truy cập menu này).bằng cách truyền vào type của tài khoản.

        {
            adminToolStripMenuItem.Enabled = type == 1;// bật nếu là admin và tắt nếu là tài khoản nhân viên.
            mnThongtintaikhoan.Text += " (" + LoginAccount.DisplayName + ")";
        }
        void LoadCategory()// Hàm Load ra danh sách các Category
        {
            List<CategoryDTO> listCategory = CategoryDAO.Instance.GetListCategory();
            cbCategory.DataSource = listCategory;
            cbCategory.DisplayMember = "Name"; // Định dạng hiển thị trển combox là phần tên
        }
        void LoadFoodListByCategoryID(int id) // Hàm lấy ra danh sách đồ ăn theo ID Category
        {
            List<FoodDTO> listFood = FoodDAO.Instance.GetFoodByCategoryID(id);
            cbFood.DataSource = listFood;
            cbFood.DisplayMember = "Name";// Định dạng hiển thị trển combox là phần tên
        }
        void LoadComboboxTable(ComboBox cb)// Hàm Load Combox danh sách các bàn để chọn chuyển bàn.
        {
            cb.DataSource = TableDAO.Instance.LoadTableList();
            cb.DisplayMember = "Name";
        }
        void LoadTable()// Hàm Load table lên flowpanelLayout
        {
            flpTable.Controls.Clear();// Clear dữ liệu cũ mỗi khi load lại table
            List<TableDTO> tableList = TableDAO.Instance.LoadTableList();
            foreach (TableDTO item in tableList)
            {
                Button btn = new Button() { Width = TableDAO.TableWidth, Height = TableDAO.TableHeight };
                btn.Text = item.Name + Environment.NewLine + item.Status;
                btn.Click += btn_Click;
                btn.Tag = item;
                switch (item.Status)
                {
                    case "Trống":
                        btn.BackColor = Color.Green;
                        break;
                    default:
                        btn.BackColor = Color.Red;
                        break;
                }
                flpTable.Controls.Add(btn);
            }
        }
        void ShowBill(int id)// Hàm hiển thị các Bill lên listview
        {
            lsvBill.Items.Clear();
            List<MenuDTO> listBillInfo = MenuDAO.Instance.GetListMenuByTable(id);
            float totalPrice = 0;
            foreach (MenuDTO item in listBillInfo)
            {

                ListViewItem lsvItem = new ListViewItem(item.FoodName.ToString());
                lsvItem.SubItems.Add(item.Count.ToString());
                lsvItem.SubItems.Add(item.Price.ToString());
                lsvItem.SubItems.Add(item.Totalprice.ToString());
                totalPrice += item.Totalprice;

                lsvBill.Items.Add(lsvItem);

            }
            /* CultureInfo culture = new CultureInfo("en-VN");
             Thread.CurrentThread.CurrentCulture = culture;
             txbTotalPrice.Text = totalPrice.ToString("c");*/
            txtTotalPrice.Text = totalPrice.ToString();

        }


        #endregion

        #region Events
        private void btn_Click(object sender, EventArgs e)// Sự kiên click vào button table để hiển thị Bill
        {
            int tableID = ((sender as Button).Tag as TableDTO).ID;
            lsvBill.Tag = (sender as Button).Tag;// Khi click vào bàn thì lấy ra tag của bàn đó để xác định khi thêm món
            ShowBill(tableID);//show bill trên view
        }
        private void thôngTinCáNhânToolStripMenuItem_Click(object sender, EventArgs e)//Sự kiên click vào thông tin cá nhân trong menu
        {
            frmAccountProfile f = new frmAccountProfile(LoginAccount);
            f.UpdateAccout += f_UpdateAccount;//Update from cha
            f.ShowDialog();
        }

        private void f_UpdateAccount(object sender, AccountEvent e)//hàm thực hiện update
        {
            mnThongtintaikhoan.Text = "Thông tin tài khoản (" + e.Acc.DisplayName + ")";
        }

        private void đăngXuấtToolStripMenuItem_Click(object sender, EventArgs e)// Sự kiện Click vào Đăng xuất
        {
            this.Close();
        }

        private void adminToolStripMenuItem_Click(object sender, EventArgs e)// Sự kiện click vào Admin
        {
            frmAdmin f = new frmAdmin();
            //this.Hide();
            f.InsertFood += F_InsertFood;
            f.DeleteFood += F_DeleteFood;
            f.UpdateFood += F_UpdateFood;
            f.ShowDialog();
            this.Show();
        }

        private void F_UpdateFood(object sender, EventArgs e)
        {
            LoadFoodListByCategoryID((cbCategory.SelectedItem as CategoryDTO).ID);
            if (lsvBill.Tag!=null)
            {
                ShowBill((lsvBill.Tag as TableDTO).ID);
            }
           
        }

        private void F_DeleteFood(object sender, EventArgs e)
        {
            LoadFoodListByCategoryID((cbCategory.SelectedItem as CategoryDTO).ID);
            if (lsvBill.Tag != null)
            {
                ShowBill((lsvBill.Tag as TableDTO).ID);
            }
            LoadTable();
        }

        private void F_InsertFood(object sender, EventArgs e)
        {
            LoadFoodListByCategoryID((cbCategory.SelectedItem as CategoryDTO).ID);
            if (lsvBill.Tag != null)
            {
                ShowBill((lsvBill.Tag as TableDTO).ID);
            }
        }

        private void cbCategory_SelectedIndexChanged(object sender, EventArgs e)// Bắt sự kiện chọn Category để load Food theo trên cbox
        {
            int id = 0;
            ComboBox cb = sender as ComboBox;
            if (cb.SelectedItem==null) // neu ma combo category chua co data
            {
                return;
            }
            CategoryDTO selected = cb.SelectedItem as CategoryDTO;
            id = selected.ID;
            LoadFoodListByCategoryID(id);// Gọi Hàm Load Food bên trên.
        }
        private void btnAddFood_Click(object sender, EventArgs e)// Sự kiện add Food
        {

            TableDTO table = lsvBill.Tag as TableDTO;// lấy ra  table hiện tại đang chọn
            if (table==null)
            {
                MessageBox.Show(" Chưa chọn bàn !");
                return;
            }
            int idBill = BillDAO.Instance.GetUncheckBillIDByTableID(table.ID);// Lấy ra id Bill hiện tại bằng cách truyền idTable
            int foodID = (cbFood.SelectedItem as FoodDTO).ID;// Lấy ra id Food từ combox
            int count = (int)nmFoodCount.Value;//Số lượng fodd add vào lấy từ numberMenu
            if (idBill == -1)// Khi chưa có bill
            {
                BillDAO.Instance.InsertBill(table.ID);//tạo Bill mới
                BillInfoDAO.Instance.InsertBillInfo(BillDAO.Instance.GetMaxIDBill(), foodID, count);//Thêm Bill Info
                //Do thuật toán ID tự tăng lên id bill sẽ bằng GetMaxIDBill
            }
            else// Bill đã tồn tại
            {
                BillInfoDAO.Instance.InsertBillInfo(idBill, foodID, count);
            }
            ShowBill(table.ID);
            LoadTable();
        }
        private void btnCheckOut_Click(object sender, EventArgs e)// Sự kiện nhấn thanh toán
        {
            TableDTO table = lsvBill.Tag as TableDTO;// Lấy id table theo tag của listview.
            int idBill = BillDAO.Instance.GetUncheckBillIDByTableID(table.ID);//lấy ra Id bill tồn tại theo bàn
            int discount = (int)nmDisCount.Value;
           // float toalPrice = float.Parse(txtTotalPrice.Text);
            double totalPrice = Convert.ToDouble(txtTotalPrice.Text);// Conver dữ liệu từ textBox tổng tiền ra kiểu double.
            double finalTotalPrice = totalPrice - (totalPrice / 100) * discount;// Tính tổng tiền cần thanh toán sau khi đã giảm giá
            if (idBill != -1)
            {
                if (MessageBox.Show(string.Format("Bạn muốn thanh toán cho bàn {0}\n Tổng tiền -(Tổng Tiền /100)xGiảm Giá = {1} - ({1} / 100 ) x {2} = {3} ", table.Name, totalPrice, discount, finalTotalPrice), "Thông Báo !", MessageBoxButtons.OKCancel) == System.Windows.Forms.DialogResult.OK)
                {
                    BillDAO.Instance.CheckOut(idBill, discount, (float)finalTotalPrice);// Thực hiên checkout
                    ShowBill(table.ID);//load lại bill theo id table
                }
            }
            LoadTable();//load lại table khi có thao tác trên giao diện để cập nhật thông tin bàn ăn
        }

        private void btnSwitchTable_Click(object sender, EventArgs e)// Sự kiện chuyển bàn
        {
            int id1 = (lsvBill.Tag as TableDTO).ID;//Lấy ra id của table đang chọn trên listview
            int id2 = (cbSwithTable.SelectedItem as TableDTO).ID;// Lấy ra id của table muốn chuyển trên cbox danh sách bàn
            if (MessageBox.Show(string.Format("Bạn có muốn chuyển bàn {0} qua bàn {1}", (lsvBill.Tag as TableDTO).Name, (cbSwithTable.SelectedItem as TableDTO).Name), "Thông Báo", MessageBoxButtons.OKCancel) == System.Windows.Forms.DialogResult.OK)
            {
                TableDAO.Instance.SwitchTable(id1, id2);
                LoadTable();
            }
        }






        #endregion


    }
}
