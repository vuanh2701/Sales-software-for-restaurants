using PhanMemQuanLyQuanCom.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PhanMemQuanLyQuanCom.DAO
{
    public  class MenuDAO
    {
        private static MenuDAO instance;

        public static MenuDAO Instance 
        { 
            get
            {
                if (instance == null)
                    instance = new MenuDAO();
                return MenuDAO.instance;
            }
            set => instance = value; 
        }

        private MenuDAO() { }


        public List<MenuOverview> GetListMenuByTable(int id)
        {
            List<MenuOverview> listMenu = new List<MenuOverview>();

            string query = "SELECT f.DisplayName, bi.count, f.Price, f.Price* bi.count AS[Total Price] FROM BillInfo as bi, Bill as b, Food as f WHERE bi.idBill = b.id AND bi.idFood = f.id AND b.status = 0 AND b.idTable = " + id;

            DataTable data = DataProvider.Instance.ExecuteQuery(query);

            foreach (DataRow item in data.Rows)
            {
                MenuOverview menu = new MenuOverview(item);
                listMenu.Add(menu);
            }

            return listMenu;
        }

        
    }
}
