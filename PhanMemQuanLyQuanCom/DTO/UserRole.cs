using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhanMemQuanLyQuanCom.DTO
{
    public class UserRole
    {
        public UserRole(int id, string displayName)
        {
            this.Id = id;
            this.DisplayName = displayName;
        }

        public UserRole(DataRow row)
        {
            this.Id = (int)row["id"];
            this.DisplayName = row["displayName"].ToString();
        }

        private int id;
        private string displayName;

        public int Id { get => id; set => id = value; }
        public string DisplayName { get => displayName; set => displayName = value; }
    }
}
