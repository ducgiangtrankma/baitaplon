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
    public partial class frmLogin : Form
    {
        public frmLogin()
        {
            InitializeComponent();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void frmLogin_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (MessageBox.Show("Bạn có muốn thoát chương trình ?","Thông Báo !",MessageBoxButtons.OKCancel)!=System.Windows.Forms.DialogResult.OK)

            {
                e.Cancel = true;
            }
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string userName = txtUserName.Text;
            string passWord = txtPassWord.Text;
            if (Login(userName, passWord))
            {
                AccountDTO loginAccount = AccountDAO.Instance.GetAccountByUserName(userName);// gọi và khởi tạo acc theo ÚserName
                frmTableManager f = new frmTableManager(loginAccount);// khởi tạo from quan ly
                this.Hide();// tắt from login
                f.ShowDialog();
                this.Show();// bật from login
            }
            else
            {
                MessageBox.Show("Tài khoản hoặc mật khẩu không đúng !","Thông Báo");
            }
        }
        bool Login( string userName, string passWord)
        {
            return AccountDAO.Instance.Login(userName, passWord);
        }

        private void frmLogin_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode ==Keys.Enter)
            {
                btnLogin.PerformClick();
            }
            if (e.KeyCode == Keys.Exsel)
            {
                btnExit.PerformClick();
            }
        }
    }
}
