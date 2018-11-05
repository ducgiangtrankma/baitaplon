using Demo_QLNH.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo_QLNH.DAO
{
    public class BillDAO
    {
        private static BillDAO instance;
        public static BillDAO Instance
        {
            get { if (instance == null) instance = new BillDAO(); return BillDAO.instance; }
            private set { BillDAO.instance = value; }
        }
        // Thanh cong : bill ID
        // That bai : -1
        private BillDAO() { }
        public int GetUncheckBillIDByTableID(int id)
        {
            DataTable data = DataProvider.Instance.ExecuteQuery("Select * From Bill where idTable = " + id + " and status =0");
            if (data.Rows.Count > 0)
            {
                BillDTO bill = new BillDTO(data.Rows[0]);
                return bill.ID;
            }
            return -1;
        }
        public void CheckOut(int id, int discount, float totalPrice)// Hàm để gán time CheckOut cho bill truyền vào giảm giá và tiền
        {
            string query = "Update Bill set dateCheckOut = GETDATE(), status=1, " + "discount = " + discount + ", totalPrice = " + totalPrice + " where id=" + id;
            DataProvider.Instance.ExecuteNonQuery(query);
        }
        public void InsertBill(int id)// Hàm Inser thông tin bill mới theo id Table
        {
            DataProvider.Instance.ExecuteNonQuery("exec USP_InsertBill @idTable", new object[] { id });
        }
        public DataTable GetBillListByDate(DateTime checkIn, DateTime checkOut)// Lấy ra list bill đã thanh toán theo khoảng time trả về 1 table để hiển thị
        {
            return DataProvider.Instance.ExecuteQuery("exec USP_GetListBillByDate @checkIn , @checkOut", new object[] { checkIn, checkOut });
        }
        public int GetMaxIDBill()// Lấy ra IDBill Max
        {
            try
            {
                return (int)DataProvider.Instance.ExecuteScalar(" Select MAX(id) From Bill");// Dung Scalar để đếm.

            }
            catch
            {
                return 1;// Nếu chưa có bill nào thì cho ID bằng 1.
            }
            // Dùng try catch để bắt lỗi nếu chưa có bill nào thì không lấy được Id
        }
    }
}
