using Demo_QLNH.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo_QLNH.DAO
{
    public class TableDAO
    {
        private static TableDAO instance;
        public static TableDAO Instance
        {
            get { if (instance == null) instance = new TableDAO(); return TableDAO.instance; }
            set { TableDAO.instance = value; }
        }
        public static int TableWidth = 82;
        public static int TableHeight = 82;

        private TableDAO() { }

        public void SwitchTable(int id1, int id2)// Hàm gọi truy vấn chuyển bàn theo id table.
        {
            DataProvider.Instance.ExecuteQuery("USP_SwitchTabel @idTable1 , @idTable2", new object[] { id1, id2 });
        }
        public List<TableDTO> LoadTableList()// Hàm khơi tạo list table
        {
            List<TableDTO> tablelist = new List<TableDTO>();
            DataTable data = DataProvider.Instance.ExecuteQuery("USP_GetTableList");
            foreach (DataRow item in data.Rows)
            {
                TableDTO table = new TableDTO(item);
                tablelist.Add(table);
            }
            return tablelist;

        }
    }
}
