using PhanMemQuanLyQuanCom.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhanMemQuanLyQuanCom.DAO
{
    public class BillDAO
    {
        private static BillDAO instance;

        public static BillDAO Instance 
        {
            get
            {
                if (instance == null)
                    instance = new BillDAO();
                return BillDAO.instance;
            }
            set => instance = value;
        }

        private BillDAO() { }

        public int GetUncheckBillIDByTableID(int id)
        {
            /// Thành công : bill ID
            /// Thất bại : -1

            DataTable data = DataProvider.Instance.ExecuteQuery(" SELECT * FROm Bill WHERE idTable = " + id + " AND status = 0");

            if(data.Rows.Count > 0)
            {
                Bill bill = new Bill(data.Rows[0]);
                return bill.ID;
            }
            return -1;
        }


        public void InsertBill(int id)
        {
            DataProvider.Instance.ExcuteNonQuery("EXEC USP_InsertBill @idTable", new object[] { id});
        }

        public void CheckOut(int id, int discount, float totalprice)
        {
            string query = "UPDATE Bill SET dateCheckOut = GetDate(), status = 1, " +  " discount = " + discount + ", totalPrice = "+ totalprice +" where id = " + id;
            DataProvider.Instance.ExcuteNonQuery(query);
        }

        public DataTable GetBillByDate(DateTime checkIn, DateTime checkOut)
        {
            return DataProvider.Instance.ExecuteQuery("EXEC USP_GetListBillByDate @checkIn , @checkOut", new object[] { checkIn, checkOut });
        }


        public DataTable GetBillByDateAndPage(DateTime checkIn, DateTime checkOut, int pageNum)
        {
            return DataProvider.Instance.ExecuteQuery("EXEC USP_GetListBillByDateAndPage @checkIn , @checkOut , @page", new object[] { checkIn, checkOut , pageNum });
        }


        public int GetNumBill(DateTime checkIn, DateTime checkOut)
        {
            return (int)DataProvider.Instance.ExcuteScalar("EXEC USP_GetNumBillByDate @checkIn , @checkOut", new object[] { checkIn, checkOut });
        }


        public int GetMaxBill()
        {
            try
            {
                 return (int)DataProvider.Instance.ExcuteScalar("SELECT MAX(id) FROM Bill");
            }
            catch
            {
                return 1;
            }
        }
    }
}
