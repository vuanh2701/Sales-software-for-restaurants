using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhanMemQuanLyQuanCom.DTO
{
    public class Food
    {
        public Food(int id, string displayName, int idCategory, float price)
        {
            this.Id = id;
            this.DisplayName = displayName;
            this.IdCategory = idCategory;
            this.Price = price;
        }


        public Food(DataRow row)
        {
            this.Id = (int)row["id"];
            this.DisplayName = row["displayName"].ToString();
            this.IdCategory = (int)row["idCategory"];
            this.Price = (float)Convert.ToDouble(row["price"].ToString());
        }

        private int id;
        private string displayName;
        private int idCategory;
        private float price;

        public int Id { get => id; set => id = value; }
        public string DisplayName { get => displayName; set => displayName = value; }
        public int IdCategory { get => idCategory; set => idCategory = value; }
        public float Price { get => price; set => price = value; }
    }
}
