using PhanMemQuanLyQuanCom.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhanMemQuanLyQuanCom.DAO
{
    public class UserRoleDAO
    {
        private static UserRoleDAO instance;

        public static UserRoleDAO Instance 
        { 
            get
            {
                if(instance == null)
                {
                    instance = new UserRoleDAO();
                }
                return instance;
            }
            private  set => instance = value; 
        }

        private UserRoleDAO() { }

        public List<UserRole> GetListUserRole()
        {
            List<UserRole> list = new List<UserRole>();

            string query = "SELECT * FROM UserRole";
            DataTable data = DataProvider.Instance.ExecuteQuery(query);

            foreach(DataRow item in data.Rows)
            {
                UserRole role = new UserRole(item);
                list.Add(role);
            }
            return list;
        }
    }
}
