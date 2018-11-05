using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo_QLNH.DTO
{
   public class MenuDTO
    {
        public MenuDTO(string foodName, int count, float price, float totalPrice = 0)
        {
            this.FoodName = foodName;
            this.Count = count;
            this.Price = price;
            this.Totalprice = totalPrice;
        }
        public MenuDTO(DataRow row)
        {
            this.FoodName = row["Name"].ToString();
            this.Count = (int)row["count"];
            this.Price = (float)Convert.ToDouble(row["price"].ToString());
            this.Totalprice = (float)Convert.ToDouble(row["totalprice"].ToString());
        }
        private string foodName;
        public string FoodName
        {
            get { return foodName; }
            set { foodName = value; }
        }
        private int count;
        public int Count { get => count; set => count = value; }
        private float price;
        public float Price { get => price; set => price = value; }
        private float totalPrice;
        public float Totalprice { get => totalPrice; set => totalPrice = value; }
    }
}
