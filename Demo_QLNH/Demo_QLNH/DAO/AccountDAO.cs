using Demo_QLNH.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography;// THU VIEN MD5 dùng để mã hóa
using System.Text;
using System.Threading.Tasks;

namespace Demo_QLNH.DAO
{
    public class AccountDAO
    {
        private static AccountDAO instance;
        public static AccountDAO Instance
        {
            get { if (instance == null) instance = new AccountDAO(); return instance; }
            set { instance = value; }
        }
        public bool Login(string userName, string passWord)// Hàm kiểm tra login
        {
            //byte[] temp = ASCIIEncoding.ASCII.GetBytes(passWord);//ma hoa pass thanh byte
            //byte[] hasData = new MD5CryptoServiceProvider().ComputeHash(temp);// giai pass ra de check
            //string hasPass = "";
            //foreach (byte item in hasData)
            //{
             //   hasPass += item;
            //}
            //var list = hasData.ToString();// chuyen pass ve string
            //list.Reverse();// dao nguoc pass de tranh check pass (lam cho vui)
        
            string query = "USP_Login @userName , @passWord";
            DataTable result = DataProvider.Instance.ExecuteQuery(query, new object[] { userName, passWord });
            return result.Rows.Count > 0; // trả về giá trị > 0 nếu trong dánh data có tài khoản và mkhau đúng
        }
        public bool UpdateAccount(string userName, string displayName, string pass, string newPass)// hàm upadte acc mới vào database
        {
            int result = DataProvider.Instance.ExecuteNonQuery("exec USP_UpdateAccount @username , @displayName , @passWord , @newPassword ", new object[] { userName, displayName, pass, newPass });
            return result > 0;
        }
        public AccountDTO GetAccountByUserName(string userName)// Lấy thông tin acc theo Username
        {
            DataTable data = DataProvider.Instance.ExecuteQuery("Select * From account where userName = '" + userName + "'");// chú ý đặt ngoặc đơn ở userName để sql nhận dạng kiểu chuỗi/
            foreach (DataRow item in data.Rows)
            {
                return new AccountDTO(item);
            }
            return null;
        }
        public DataTable GetListAccount()// Lấy ra list accout
        {
            return DataProvider.Instance.ExecuteQuery("select a.UserName, a.DisplayName , a.Type From Account as a");
        }
        public bool InsertAccount(string name, string displayName, int type)// Theem account
        {
            string query = string.Format("INSERT dbo.Account ( UserName, DisplayName, Type )VALUES  ( N'{0}', N'{1}', {2})", name, displayName, type);
            int result = DataProvider.Instance.ExecuteNonQuery(query);

            return result > 0;
        }

        public bool UpdateAccount(string name, string displayName, int type) // sua account
        {
            string query = string.Format("UPDATE dbo.Account SET DisplayName = N'{1}', Type = {2} WHERE UserName = N'{0}'", name, displayName, type);
            int result = DataProvider.Instance.ExecuteNonQuery(query);

            return result > 0;
        }

        public bool DeleteAccount(string name)// xoa account
        {
            string query = string.Format("Delete Account where UserName = N'{0}'", name);
            int result = DataProvider.Instance.ExecuteNonQuery(query);

            return result > 0;
        }

        public bool ResetPassword(string name)// Reset pass
        {
            string query = string.Format("update account set password = N'0' where UserName = N'{0}'", name);
            int result = DataProvider.Instance.ExecuteNonQuery(query);

            return result > 0;
        }
    }
}
