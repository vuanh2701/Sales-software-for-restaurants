using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhanMemQuanLyQuanCom.DTO
{
    public class Category
    {
        public Category(int id, string displayName)
        {
            this.ID = id;
            this.DisplayName = displayName;
        }

        public Category(DataRow row)
        {
            this.ID = (int)row["id"];
            this.DisplayName = row["displayName"].ToString();
        }

        private int iD;
        private string displayName;

        public int ID { get => iD; set => iD = value; }
        public string DisplayName { get => displayName; set => displayName = value; }
    }
}
