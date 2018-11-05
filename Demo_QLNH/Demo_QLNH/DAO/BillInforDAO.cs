﻿using Demo_QLNH.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo_QLNH.DAO
{
    public class BillInfoDAO
    {
        private static BillInfoDAO instance;

        public static BillInfoDAO Instance
        {
            get { if (instance == null) instance = new BillInfoDAO(); return BillInfoDAO.instance; }
            private set { BillInfoDAO.instance = value; }
        }

        private BillInfoDAO() { }

        public List<BillInfoDTO> GetListBillInfo(int id)
        {
            List<BillInfoDTO> listBillInfo = new List<BillInfoDTO>();
            DataTable data = DataProvider.Instance.ExecuteQuery("Select * From BillInfo where idBill = " + id);
            foreach (DataRow item in data.Rows)
            {
                BillInfoDTO info = new BillInfoDTO(item);
                listBillInfo.Add(info);
            }
            return listBillInfo;

        }
        public void InsertBillInfo(int idBill, int idFood, int count)// Hàm thêm thông tin Bill Info
        {
            DataProvider.Instance.ExecuteNonQuery("exec USP_InsertBillInfo @idBill , @idFood , @count", new object[] { idBill, idFood, count });
        }
        public void DeleteBillInfoByIDFood(int idFood)// Hàm xóa bill info do trong bill info có cả food.
        {
            DataProvider.Instance.ExecuteQuery("Delete BillInfo where idFood = " + idFood);
        }
    }


}
