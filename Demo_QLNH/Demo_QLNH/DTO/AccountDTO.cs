using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo_QLNH.DTO
{
   public class AccountDTO
    {   
        public AccountDTO(string userName, string displayName,int type, string passWord = null)//Hàm Dựng
        {
            this.userName = userName;
            this.displayName = displayName;
            this.passWord = passWord;
            this.type = type;
        }
        public AccountDTO(DataRow row)
        {
            this.userName = row["username"].ToString();
            this.displayName = row["displayName"].ToString();
            this.passWord = row["passWord"].ToString();
            this.type =(int) row["type"];
        }

        private string userName;
        private string displayName;
        private string passWord;
        private int type;

        public string UserName { get => userName; set => userName = value; }
        public string DisplayName { get => displayName; set => displayName = value; }
        public string PassWord { get => passWord; set => passWord = value; }
        public int Type { get => type; set => type = value; }
    }
}
