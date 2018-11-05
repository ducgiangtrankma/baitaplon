using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo_QLNH.DTO
{
   public class BillInfoDTO
    {
        public BillInfoDTO(int id, int idBill, int idFood, int count)
        {
            this.ID = id;
            this.IDBill = idBill;
            this.IDFood = idFood;
            this.Count = count;
        }
        public BillInfoDTO(DataRow row)
        {
            this.ID = (int)row["id"];
            this.IDBill = (int)row["idBill"];
            this.IDFood = (int)row["idFood"];
            this.Count = (int)row["count"];

        }
        private int iD;
        private int iDBill;
        private int iDFood;
        private int count;

        public int ID { get => iD; set => iD = value; }
        public int IDBill { get => iDBill; set => iDBill = value; }
        public int Count { get => count; set => count = value; }
        public int IDFood { get => iDFood; set => iDFood = value; }
    }
}
