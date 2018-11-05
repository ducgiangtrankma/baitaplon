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
    public partial class frmAccountProfile : Form
    {
        private AccountDTO loginAccount;

        public AccountDTO LoginAccount
        {
            get { return loginAccount; }
            set { loginAccount = value; ChangeAccount(loginAccount); }
        }
        public frmAccountProfile(AccountDTO acc)
        {
            InitializeComponent();
            LoginAccount = acc;
        }
        void ChangeAccount(AccountDTO acc)// Hàm để show tên và display name lên from Accprofile.
        {
            txtUser.Text = LoginAccount.UserName;
            txtShowName.Text = LoginAccount.DisplayName;

        }
        void UpdateAccountInfo()// Hàm update thông tin acc
        {
            string usernam = txtUser.Text;
            string displayName = txtShowName.Text;
            string passWord = txtPassWord.Text;
            string newpass = txtNewPassWord.Text;
            string reenterpass = txtTypeNewPassWord.Text;

            if (newpass != reenterpass)// Nếu pass nhập lại không khớp new pass
            {
                MessageBox.Show("Nhập lại Password trùng với New PassWord !");
            }
            else
            {
                if (AccountDAO.Instance.UpdateAccount(usernam, displayName, passWord, newpass))// gọi update acc bên DAO
                {
                    MessageBox.Show(" Update thành công !");
                    if (updataAccount!=null)
                    {
                        updataAccount(this,new AccountEvent( AccountDAO.Instance.GetAccountByUserName(usernam)));
                    }

                }
                else
                {
                    MessageBox.Show(" Sai mật khẩu !");
                }
            }
        } 

        // Tao even truyền sự kiện từ from con lên cha để update lại thông tin của from cha( thoogn tin cá nhân) khi thực hiện update display name trong from accprofile,
        private event EventHandler<AccountEvent> updataAccount;
        public event EventHandler<AccountEvent> UpdateAccout
        {
            add { updataAccount += value; }
            remove { updataAccount -= value; }
        }
        //Hết even

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            UpdateAccountInfo();
        }

        private void btnExit_Click_1(object sender, EventArgs e)
        {
            this.Close();
        }
    }


    public class AccountEvent : EventArgs // Even level2 - Có tham khảo 
    {
        private AccountDTO acc;

        public AccountDTO Acc { get => acc; set => acc = value; }
        public AccountEvent(AccountDTO acc)
        {
            this.Acc = acc;
        }
    }

}
