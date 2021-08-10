using PhanMemQuanLyQuanCom.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhanMemQuanLyQuanCom.DAO
{
    public class CategoryDAO
    {
        private static CategoryDAO instance;

        public static CategoryDAO Instance 
        { 
            get
            {
                if (instance == null)
                    instance = new CategoryDAO();
                return CategoryDAO.instance;
            }
            private set => instance = value; 
        }

        private CategoryDAO() { }

        public List<Category> GetListCategory()
        {
            List<Category> listCategory = new List<Category>();

            string query = "SELECT * FROm FoodCategory ";
            DataTable data = DataProvider.Instance.ExecuteQuery(query);

            foreach (DataRow item in data.Rows)
            {
                Category category = new Category(item);
                listCategory.Add(category);
            }

            return listCategory;
        }


        public DataTable GetListCategoryOnDataTable()
        {
            string query = "SELECT id, DisplayName AS [Tên danh mục] FROM FoodCategory";
            return DataProvider.Instance.ExecuteQuery(query);

        }

        public int CheckCategory(string displayName)
        {
            string query = string.Format("SELECT * FROM FoodCategory where DisplayName = N'{0}'", displayName);
            var result = DataProvider.Instance.ExcuteScalar(query) == null ? 0 : 1;
            return result;
        }

        public List<Category> SearchCategoryByName(string name)
        {
            List<Category> listCategory = new List<Category>();

            string query = string.Format("SELECT * FROM FoodCategory WHERE dbo.fuConvertToUnsign1(DisplayName) like N'%' + dbo.fuConvertToUnsign1(N'{0}') + '%'", name);
            DataTable data = DataProvider.Instance.ExecuteQuery(query);

            foreach (DataRow item in data.Rows)
            {
                Category category = new Category(item);
                listCategory.Add(category);
            }

            return listCategory;
        }

        public Category GetCategoryByID(string nameCategory)
        {
            Category category = null;

            string query = string.Format("SELECT * FROm FoodCategory where DisplayName = N'{0}'", nameCategory);
            DataTable data = DataProvider.Instance.ExecuteQuery(query);

            foreach (DataRow item in data.Rows)
            {
                category = new Category(item);
                return category;
            }


            return category;
        }

        public bool InsertCategory(string displayName)
        {
            string query = string.Format("INSErt INTO FoodCategory (DisplayName) VALUES(N'{0}')", displayName);
            int result = DataProvider.Instance.ExcuteNonQuery(query);

            return result > 0;
        }

        public bool UpdateCategory(string displayName, int id)
        {
            string query = string.Format("UPDATE FoodCategory Set DisplayName = N'{0}' WHERE id = {1}", displayName, id);
            int result = DataProvider.Instance.ExcuteNonQuery(query);

            return result > 0;
        }

        public bool DeleteCategory(int id)
        {
            FoodDAO.Instance.ChangeFoodCategory(id);
            string query = string.Format("DELETE FROM FoodCategory WHERE id = {0}", id);

            int result = DataProvider.Instance.ExcuteNonQuery(query);

            return result > 0;
        }
    }
}
