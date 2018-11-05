using Demo_QLNH.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo_QLNH.DAO
{
    public class FoodDAO
    {
        private static FoodDAO instance;
        public static FoodDAO Instance
        {
            get { if (instance == null) instance = new FoodDAO(); return FoodDAO.instance; }
            private set { FoodDAO.instance = value; }
        }
        private FoodDAO() { }

        public List<FoodDTO> GetFoodByCategoryID(int id)
        {
            List<FoodDTO> list = new List<FoodDTO>();
            string query = "Select * From Food where idCategory=" + id;
            DataTable data = DataProvider.Instance.ExecuteQuery(query);
            foreach (DataRow item in data.Rows)
            {
                FoodDTO food = new FoodDTO(item);
                list.Add(food);
            }
            return list;
        }
        public List<FoodDTO> GetListFood()
        {
            List<FoodDTO> list = new List<FoodDTO>();
            string query = "Select * From Food";
            //string query = "Select id [ID],name [Tên Món],idCategory [Mã Danh Mục],price [Giá] From Food";
            DataTable data = DataProvider.Instance.ExecuteQuery(query);
            foreach (DataRow item in data.Rows)
            {
                FoodDTO food = new FoodDTO(item);
                list.Add(food);
            }
            return list;
        }
        public bool InsertFood(string name, int idCategory, float price)// theem mon an
        {
            string query =string.Format( "Insert Food (name , idCategory , price ) values ( N' {0} ' , {1} , {2} )", name , idCategory , price );
            int result =  DataProvider.Instance.ExecuteNonQuery(query);
            return result > 0;
        }
        public bool UpdateFood(int idFood, string name, int idCategory, float price)
        {
            string query = string.Format("UPDATE dbo.Food SET name = N'{0}', idCategory = {1}, price = {2} WHERE id = {3}", name, idCategory, price, idFood);
            int result = DataProvider.Instance.ExecuteNonQuery(query);

            return result > 0;
        }
        public bool DeleteFood(int idFood)
        {
            BillInfoDAO.Instance.DeleteBillInfoByIDFood(idFood);

            string query = string.Format("Delete Food where id = {0}", idFood);
            int result = DataProvider.Instance.ExecuteNonQuery(query);

            return result > 0;
        }


    }
}
