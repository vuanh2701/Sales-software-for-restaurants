using PhanMemQuanLyQuanCom.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhanMemQuanLyQuanCom.DAO
{
    public class TableDAO
    {
        private static TableDAO instance;
        public static TableDAO Instance 
        {
            get
            {
                if (instance == null)
                    instance = new TableDAO();
                return TableDAO.instance;
            }
            private set => instance = value; 
        }

        public static int TableWidth = 130;
        public static int TableHeight = 100;


        private TableDAO() { }

        public List<Table> LoadTableList()
        {
            List<Table> tableList = new List<Table>();

            DataTable data = DataProvider.Instance.ExecuteQuery("USP_GetTableList");

            foreach (DataRow item in data.Rows)
            {
                Table table = new Table(item);
                tableList.Add(table);
            }

            return tableList;
        }

        public DataTable LoadTableListOnDataTable()
        { 
            return DataProvider.Instance.ExecuteQuery("SELECT id, DisplayName AS [Tên bàn], Status AS [Trạng thái] FROM TableFood");

        }

        //public List<Table> SearchTableByName(string name)
        //{
        //    List<Table> tableList = new List<Table>();
        //    string query = string.Format("SELECT * FROM TableFood WHERE dbo.fuConvertToUnsign1(DisplayName) like N'%' + dbo.fuConvertToUnsign1(N'{0}') + '%'", name);
        //    DataTable data = DataProvider.Instance.ExecuteQuery(query);

        //    foreach (DataRow item in data.Rows)
        //    {
        //        Table table = new Table(item);
        //        tableList.Add(table);
        //    }

        //    return tableList;
        //}

        public DataTable SearchTableByName(string name)
        {         
            string query = string.Format("SELECT id, DisplayName AS [Tên bàn], Status AS [Trạng thái] FROM TableFood WHERE dbo.fuConvertToUnsign1(DisplayName) like N'%' + dbo.fuConvertToUnsign1(N'{0}') + '%'", name);
            return DataProvider.Instance.ExecuteQuery(query);

        }

        public void SwitchTable (int id1, int id2)
        {
            DataProvider.Instance.ExecuteQuery("USP_SwitchTable @idTable1 , @idTable2", new object[]{id1, id2});
        }


        public int CheckTable(string userName)
        {
            string query = string.Format("SELECT * FROM TableFood where DisplayName = N'{0}'", userName);

            var result = DataProvider.Instance.ExcuteScalar(query) == null ? 0 : 1;
            return result;

        }

        public bool InsertTable(string displayName)
        {
            string query = string.Format("INSERT TableFood (DisplayName , Status) VALUES (N'{0}', N'Trống')", displayName);
            int result = DataProvider.Instance.ExcuteNonQuery(query);

            return result > 0;
        }

        public bool UpdateTableName (string displayName, int id)
        {
            string query = string.Format("Update TableFood SET DisplayName = N'{0}' where id = {1}", displayName, id);
            int result = DataProvider.Instance.ExcuteNonQuery(query);

            return result > 0;
        }
            
        public bool DeleteTable(int id)
        {
            string query = string.Format("DELETE TableFood WHERE id = {0}", id);
            int result = DataProvider.Instance.ExcuteNonQuery(query);

            return result > 0;
        }
    }
}
