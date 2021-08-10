using PhanMemQuanLyQuanCom.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhanMemQuanLyQuanCom.DAO
{
    public class BillInfoDAO
    {
        private static BillInfoDAO instance;

        public static BillInfoDAO Instance 
        {
            get
            {
                if (instance == null)
                    instance = new BillInfoDAO();
                return BillInfoDAO.instance;
            } 
            set => instance = value; 
        }

        private BillInfoDAO() { }

        
        public List<Menu> GetListBillInfo (int id)
        {
            List<Menu> listBillInfo = new List<Menu>();

            DataTable data = DataProvider.Instance.ExecuteQuery("SELECT * FROM BillInfo WHERE idBill = " + id);

            foreach (DataRow item in data.Rows)
            {
                Menu info = new Menu(item);
                listBillInfo.Add(info);
            }

            return listBillInfo;
        }

        public void InsertBillInfo(int idBill, int idFood, int count)
        {
            DataProvider.Instance.ExcuteNonQuery("USP_InsertBillInfo @idBill , @idFood , @count ", new object[] { idBill, idFood, count });
        }

        public void DeleteBillInfoByIdFood(int idFood)
        {
            string query = string.Format("DELETE BillInfo WHere idFood = {0}", idFood);
            DataProvider.Instance.ExcuteNonQuery(query);
        }

    }
}
