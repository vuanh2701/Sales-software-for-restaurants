using PhanMemQuanLyQuanCom.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhanMemQuanLyQuanCom.DAO
{
    public class FoodDAO
    {
        private static FoodDAO instance;

        public static FoodDAO Instance 
        {
            get 
            {
                if (instance == null)
                    instance = new FoodDAO();
                return FoodDAO.instance;
            }
            set => instance = value; 
        }

        private FoodDAO() { }

        public List<Food> GetFoodByIdCategory(int id)
        {
            List<Food> listFood = new List<Food>();

            string query = "SELECT * FROm Food WHERE idCategory = " + id;
            DataTable data = DataProvider.Instance.ExecuteQuery(query);

            foreach (DataRow item in data.Rows)
            {
                Food food = new Food(item);
                listFood.Add(food);
            }

            return listFood;
        }



        public List<Food> SearchFoodByName(string foodName)
        {
            List<Food> listFood = new List<Food>();

            string query =  string.Format("SELECT * FROM Food WHERE dbo.fuConvertToUnsign1(DisplayName) like N'%' + dbo.fuConvertToUnsign1(N'{0}') + '%'", foodName);
            DataTable data = DataProvider.Instance.ExecuteQuery(query);

            foreach (DataRow item in data.Rows)
            {
                Food food = new Food(item);
                listFood.Add(food);
            }

            return listFood;
        }



        public List<Food> GetListFood()
        {
            List<Food> list = new List<Food>();
            string query = "SELECT * FROM Food";
            DataTable data =  DataProvider.Instance.ExecuteQuery(query);
            foreach  (DataRow item in data.Rows)
            {
                Food food = new Food(item);
                list.Add(food);
            }
            return list;

        }

        public DataTable GetListFoodOnDataTable()
        {
            return DataProvider.Instance.ExecuteQuery("SELECt Food.Id, Food.DisplayName AS [Tên món], FoodCategory.DisplayNAme AS [Tên danh mục], Food.Price AS [Giá] FROm Food join FoodCategory ON Food.idCategory = FoodCategory.id");
        }


        public int CheckFood(string displayName)
        {
            string query = string.Format("SELECT * Food WHERE DisplayName = N'{0}'", displayName);
            var result = DataProvider.Instance.ExcuteScalar(query) == null ? 0 : 1;
            return result;
        }
        public DataTable ListFood()
        {
            string query = "EXEC USP_GetListFood";
            return DataProvider.Instance.ExecuteQuery(query);
        }

        public bool InsertFood(string displayName, int idCategory, float price)
        {
            string query = string.Format("INSERT Food (DisplayName, idCategory, Price) Values ( N'{0}', {1}, {2})", displayName, idCategory, price);
            int result = DataProvider.Instance.ExcuteNonQuery(query);
            return result > 0;
        }

        public bool UpdateFood(string displayName, int idCategory, float price, int idFood)
        {
            string query = string.Format("Update Food SET DisplayName = N'{0}', idCategory = {1}, Price = {2} where id = {3}", displayName, idCategory, price, idFood);
            int result = DataProvider.Instance.ExcuteNonQuery(query);
            return result > 0;
        }

        public bool DeleteFood(int idFood)
        {
            BillInfoDAO.Instance.DeleteBillInfoByIdFood(idFood);
            
            string query = string.Format("Delete Food where id = {0}", idFood);
            int result = DataProvider.Instance.ExcuteNonQuery(query);
            return result > 0;
        }

        public bool ChangeFoodCategory(int idCategory)
        {
            string query = string.Format("Update Food SET idCategory = 8 WHERe idCategory = {0}", idCategory);
            int result = DataProvider.Instance.ExcuteNonQuery(query);

            return result > 0;
        }
    }
}
