using Demo_QLNH.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
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
    }
}
