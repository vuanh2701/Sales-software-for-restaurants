using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhanMemQuanLyQuanCom.DTO
{
    public class Table
    {
        public Table(int id, string displayName, string status)
        {
            this.ID = id;
            this.DisplayName = displayName;
            this.Status = status;
        }


        public Table(DataRow row)
        {
            this.ID = (int)row["id"];
            this.DisplayName = row["displayName"].ToString();
            this.Status = row["status"].ToString();
        }

        private int iD;
        private string displayName;
        private string status;

        public int ID { get => iD; set => iD = value; }
        public string DisplayName { get => displayName; set => displayName = value; }
        public string Status { get => status; set => status = value; }
    }
}
