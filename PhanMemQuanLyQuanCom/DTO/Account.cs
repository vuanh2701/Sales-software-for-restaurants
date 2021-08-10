using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhanMemQuanLyQuanCom.DTO
{
    public class Account
    {
        public Account(int id, string userName, string displayName, int idUserRole )
        {
            this.Id = id;
            this.UserName = userName;
            this.DisplayName = displayName;
            //this.Password = password;
            this.IdUserRole = idUserRole;
        }

        public Account(DataRow row)
        {
            this.Id = (int)row["id"];
            this.UserName = row["userName"].ToString();
            this.DisplayName = row["displayName"].ToString();
            //this.Password = row["password"].ToString();
            this.IdUserRole = (int)row["idUserRole"];
        }
            
        

        private int id;
        private string userName;
        private string displayName;
        //private string password;
        private int idUserRole;

        public int Id { get => id; set => id = value; }
        public string UserName { get => userName; set => userName = value; }
        public string DisplayName { get => displayName; set => displayName = value; }
        //public string Password { get => password; set => password = value; }
        public int IdUserRole { get => idUserRole; set => idUserRole = value; }
    }
}
