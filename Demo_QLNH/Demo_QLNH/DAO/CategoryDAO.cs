using Demo_QLNH.DAO;
using Demo_QLNH.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo_QLNH.DAO
{
    public class CategoryDAO
    {
        private static CategoryDAO instance;
        public static CategoryDAO Instance
        {
            get { if (instance == null) instance = new CategoryDAO(); return CategoryDAO.instance; }
            private set { CategoryDAO.instance = value; }
        }
        private CategoryDAO() { }

        public List<CategoryDTO> GetListCategory()
        {
            List<CategoryDTO> list = new List<CategoryDTO>();
            string query = "Select * From FoodCategory";
            DataTable data = DataProvider.Instance.ExecuteQuery(query);
            foreach (DataRow item in data.Rows)
            {
                CategoryDTO category = new CategoryDTO(item);
                list.Add(category);
            }
            return list;
        }
        public CategoryDTO GetCategoryByID(int id)
        {
            CategoryDTO category = null;
            string query = " Select * From FoodCategory Where id = " + id;
            DataTable data = DataProvider.Instance.ExecuteQuery(query);
            foreach (DataRow item in data.Rows)
            {
                category = new CategoryDTO(item);
                return category;
            }
            return category;

        }
        public bool DeleteCategory(int idCategory)
        {
            //BillInfoDAO.Instance.DeleteBillInfoByIDFood(idCategory);

            string query = string.Format("Delete Food where id = {0}", idCategory);
            int result = DataProvider.Instance.ExecuteNonQuery(query);

            return result > 0;
        }
        public bool UpdateCategory(int idCategory, string nameCategory)
        {
            string query = string.Format("update FoodCategory set name = N'{0}' where id = {1}", nameCategory, idCategory);
            int result = DataProvider.Instance.ExecuteNonQuery(query);
            return result > 0;

        }
        public bool InsetCategory(string nameCategory)
        {
            string query = string.Format("Insert into FoodCategory( name) values (N'{0}')", nameCategory);
            int result = DataProvider.Instance.ExecuteNonQuery(query);
            return result > 0;
        }
        public List<CategoryDTO> SearchCategoryByName(string name)
        {
            List<CategoryDTO> list = new List<CategoryDTO>();
            //string query =string.Format( "Select * From Food where name like N'%{0}%' ", name);
            string query = string.Format(" Select * From FoodCategory where [dbo].[fuConvertToUnsign1](name) like N'%' + [dbo].[fuConvertToUnsign1](N'{0}') + '%'", name);
            // string query = "Select f.id[ID],f.name[Tên Món],fc.name[Danh Mục],f.price[Gía] From Food as f join FoodCategory as fc on f.idCategory=fc.id";
            DataTable data = DataProvider.Instance.ExecuteQuery(query);
            foreach (DataRow item in data.Rows)
            {
               CategoryDTO category = new CategoryDTO(item);
                list.Add(category);
            }
            return list;
        }

    }
}
