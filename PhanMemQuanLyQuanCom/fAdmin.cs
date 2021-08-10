using PhanMemQuanLyQuanCom.DAO;
using PhanMemQuanLyQuanCom.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PhanMemQuanLyQuanCom
{
    public partial class fAdmin : Form
    {

        BindingSource foodList = new BindingSource();
        BindingSource categoryList = new BindingSource();
        BindingSource tableList = new BindingSource();
        BindingSource accountList = new BindingSource();

        public Account loginAccount;

        public fAdmin()
        {
            InitializeComponent();
            Load();
        }



        #region methods
        
        new void Load()
        {                    
            LoadListTableByDate(dtpkFromDate.Value, dtpkTodate.Value);
            LoadDateTimePickerBill();

            dtgvFood.DataSource = foodList;
            LoadListFood();
            AddFoodBinding();

            dtgvCategory.DataSource = categoryList;
            LoadCategoryFood();
            AddCategoryBinding();

            LoadCategoryIntoCombobox(cbFoodcategory);

            dtgvTable.DataSource = tableList;
            LoadTableList();
            AddTableBinding();

            dtgvAccount.DataSource = accountList;
            AddAccountBinding();
            LoadAccount();
            LoadTypeAccountIntoCombobox(cbAccountType);
        }

        void LoadDateTimePickerBill()
        {
            DateTime today = DateTime.Now;
            dtpkFromDate.Value = new DateTime(today.Year, today.Month, 1);
            dtpkTodate.Value = dtpkFromDate.Value.AddMonths(1).AddDays(-1);
        }
        void LoadListTableByDate(DateTime checkIn, DateTime checkOut)
        {
            dtgvBill.DataSource = BillDAO.Instance.GetBillByDateAndPage(checkIn, checkOut, Convert.ToInt32(txbPageBill.Text));
        }



        void LoadListFood()
        {
            foodList.DataSource = FoodDAO.Instance.GetListFoodOnDataTable();
        }

        void AddFoodBinding()
        {
            txbFoodName.DataBindings.Add(new Binding("Text", dtgvFood.DataSource, "Tên món", true, DataSourceUpdateMode.Never));
            txbFoodID.DataBindings.Add(new Binding("Text", dtgvFood.DataSource, "ID", true, DataSourceUpdateMode.Never));
            nmFoodPrice.DataBindings.Add(new Binding("Value", dtgvFood.DataSource, "Giá", true, DataSourceUpdateMode.Never));
            
        }

        List<Food> SearchFoodByName(string foodName)
        {
            List<Food> list = FoodDAO.Instance.SearchFoodByName(foodName);
            return list;
        }

        void LoadCategoryIntoCombobox(ComboBox cb)
        {
            cb.DataSource = CategoryDAO.Instance.GetListCategory();
            cb.DisplayMember = "DisplayName";
        }
        void LoadCategoryFood()
        {
            categoryList.DataSource = CategoryDAO.Instance.GetListCategoryOnDataTable();
        }
        void AddCategoryBinding()
        {
            txbCAtegoryID.DataBindings.Add(new Binding("Text", dtgvCategory.DataSource, "ID", true, DataSourceUpdateMode.Never));
            txbCategoryName.DataBindings.Add(new Binding("Text", dtgvCategory.DataSource, "Tên danh mục", true, DataSourceUpdateMode.Never));
        }
        List<Category> SearchCategoryByName(string categoryName)
        {
            List<Category> list = CategoryDAO.Instance.SearchCategoryByName(categoryName);
            return list;
        }



        void LoadTableList()
        {
            tableList.DataSource = TableDAO.Instance.LoadTableListOnDataTable();
        }
        void AddTableBinding()
        {
            txbTableID.DataBindings.Add(new Binding("Text", dtgvTable.DataSource, "ID", true, DataSourceUpdateMode.Never));
            txbTableName.DataBindings.Add(new Binding("Text", dtgvTable.DataSource, "Tên bàn", true, DataSourceUpdateMode.Never));
            cbTableStatus.DataBindings.Add(new Binding("Text", dtgvTable.DataSource, "Trạng thái", true, DataSourceUpdateMode.Never));
        }

        DataTable SearchTableByName(string tableName)
        {
            DataTable list = TableDAO.Instance.SearchTableByName(tableName);
            return list;
        }


        void LoadAccount()
        {
            accountList.DataSource  = AccountDAO.Instance.GetListAccountOnDataTable();    
        }       
        void AddAccountBinding()
        {
            txbUserName.DataBindings.Add (new Binding("Text", dtgvAccount.DataSource, "Tên đăng nhập", true, DataSourceUpdateMode.Never));
            txbDisplayName.DataBindings.Add(new Binding("Text", dtgvAccount.DataSource, "Tên hiển thị", true, DataSourceUpdateMode.Never));
            cbAccountType.DataBindings.Add(new Binding("Text", dtgvAccount.DataSource, "Loại tài khoản", true, DataSourceUpdateMode.Never));

        }
        void LoadTypeAccountIntoCombobox(ComboBox cb)
        {
            cb.DataSource = UserRoleDAO.Instance.GetListUserRole();
            cb.DisplayMember = "DisplayName";
        }
        //List<Account> SearchAccountByName(string accountName)
        //{
        //    List<Account> list = AccountDAO.Instance.SearchAccountByName(accountName);
        //    return list;
        //}

        DataTable SearchAccountByName(string accountName)
        {
            DataTable list = AccountDAO.Instance.SearchAccountByName(accountName);
            return list;
        }

        #endregion





        #region events


        private void btnViewBill_Click(object sender, EventArgs e)
        {
            LoadListTableByDate(dtpkFromDate.Value, dtpkTodate.Value);
        }

        private void btnFirstBillPage_Click(object sender, EventArgs e)
        {
            txbPageBill.Text = "1";
        }

        private void btnLastBillPage_Click(object sender, EventArgs e)
        {
            int sumRecord = BillDAO.Instance.GetNumBill(dtpkFromDate.Value, dtpkTodate.Value);
            int lastPage = sumRecord / 10;

            if (sumRecord % 10 != 0)
            {
                lastPage++;
            }

            txbPageBill.Text = lastPage.ToString();
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            int page = Convert.ToInt32(txbPageBill.Text);
            int sumRecord = BillDAO.Instance.GetNumBill(dtpkFromDate.Value, dtpkTodate.Value);

            if (page <= sumRecord / 10)
            {
                page++;
            }

            txbPageBill.Text = page.ToString();
        }

        private void btnPrevious_Click(object sender, EventArgs e)
        {
            int page = Convert.ToInt32(txbPageBill.Text);
            if (page > 1)
                page--;
            txbPageBill.Text = page.ToString();
        }

        private void txbPageBill_TextChanged(object sender, EventArgs e)
        {
            dtgvBill.DataSource = BillDAO.Instance.GetBillByDateAndPage(dtpkFromDate.Value, dtpkTodate.Value, Convert.ToInt32(txbPageBill.Text));
        }
    
    

        private void txbFoodID_TextChanged(object sender, EventArgs e)
        {           
            try
            {
                if (dtgvFood.SelectedCells.Count > 0)
                {
                    //string nameCategory = dtgvFood.SelectedCells[0].OwningRow.Cells["Tên danh mục"].Value.ToString() != null ? dtgvFood.SelectedCells[0].OwningRow.Cells["Tên danh mục"].Value.ToString() : "Không tồn tại" ;
                    string nameCategory = dtgvFood.SelectedCells[0].OwningRow.Cells["Tên danh mục"].Value == null ? "Không tồn tại" : dtgvFood.SelectedCells[0].OwningRow.Cells["Tên danh mục"].Value.ToString();

                    Category category = CategoryDAO.Instance.GetCategoryByID(nameCategory);
                    if(category != null)
                    {
                        cbFoodcategory.SelectedItem = category;

                        int index = -1;
                        int i = 0;
                        foreach (Category item in cbFoodcategory.Items)
                        {
                            if (item.ID == category.ID)
                            {
                                index = i;
                                break;
                            }
                            i++;
                        }

                        cbFoodcategory.SelectedIndex = index;
                    }
                    else
                    {
                        cbFoodcategory.SelectedValue = null;

                        MessageBox.Show("Không tìm thấy món");
                    }
                }
            }
            catch(Exception p)
            {
                string mess = p.Message;
            }
            
        }



        private void btnViewFood_Click(object sender, EventArgs e)
        {
            LoadListFood();
        }

        private void btnAddFood_Click(object sender, EventArgs e)
        {
            string displayName = txbFoodName.Text;
            int idCategory = (cbFoodcategory.SelectedItem as Category).ID;
            float price = (float)nmFoodPrice.Value;

            if(FoodDAO.Instance.CheckFood(displayName) == 0)
            {
                if (FoodDAO.Instance.InsertFood(displayName, idCategory, price))
                {
                    MessageBox.Show("Thêm món thành công");
                    LoadListFood();

                    if (insertFood != null)
                    {
                        insertFood(this, new EventArgs());
                    }
                }
                else
                {
                    MessageBox.Show("Có lỗi khi thêm món");
                }
            }
            else
            {
                MessageBox.Show("Món ăn đã tồn tại");
            }
        }

        private void btnEditFood_Click(object sender, EventArgs e)
        {
            string displayName = txbFoodName.Text;
            int idCategory = (cbFoodcategory.SelectedItem as Category).ID;
            float price = (float)nmFoodPrice.Value;
            int idFood = Convert.ToInt32(txbFoodID.Text);


            if (FoodDAO.Instance.UpdateFood(displayName, idCategory, price, idFood))
            {
                MessageBox.Show("Sửa món thành công");
                LoadListFood();

                if (updateFood != null)
                {
                    updateFood(this, new EventArgs());
                }
            }
            else
            {
                MessageBox.Show("Có lỗi khi thêm món");
            }
        }

        private void btnDeleteFood_Click(object sender, EventArgs e)
        {
            int idFood = Convert.ToInt32(txbFoodID.Text);
            if (FoodDAO.Instance.DeleteFood(idFood))
            {
                MessageBox.Show("Xóa món thành công");
                LoadListFood();

                if(deleteFood != null)
                {
                    deleteFood(this, new EventArgs());
                }
            }
            else
            {
                MessageBox.Show("Có lỗi khi xóa món");
            }
        }

        private void btnSearchFood_Click(object sender, EventArgs e)
        {
            foodList.DataSource = SearchFoodByName(txbSearchFood.Text);
        }



        private event EventHandler insertFood;
        public event EventHandler InsertFood
        {
            add
            {
                insertFood += value;
            }
            remove
            {
                insertFood -= value;
            }
        }

        private event EventHandler deleteFood;
        public event EventHandler DeleteFood
        {
            add
            {
                deleteFood += value;
            }
            remove
            {
                deleteFood -= value;
            }
        }

        private event EventHandler updateFood;
        public event EventHandler UpdateFood
        {
            add
            {
                updateFood += value;
            }
            remove
            {
                updateFood -= value;
            }
        }



        private void btnCategoryView_Click(object sender, EventArgs e)
        {
            LoadCategoryFood();
        }

        private void btnAddCategory_Click(object sender, EventArgs e)
        {
            string displayName = txbCategoryName.Text;
            if(CategoryDAO.Instance.CheckCategory(displayName) == 0)
            {
                if (CategoryDAO.Instance.InsertCategory(displayName))
                {
                    MessageBox.Show("Thêm danh mục thành công");
                    LoadCategoryFood();
                    LoadCategoryIntoCombobox(cbFoodcategory);
                    if (insertCategory != null)
                    {
                        insertCategory(this, new EventArgs());
                    }
                }
                else
                {
                    MessageBox.Show("Có lỗi khi thêm danh mục");
                }
            }
            else
            {
                MessageBox.Show("Danh mục đã tồn tại");
            }
        }

        private void btnEditCategory_Click(object sender, EventArgs e)
        {
            string displayName = txbCategoryName.Text;
            int id = Convert.ToInt32(txbCAtegoryID.Text);

            if (CategoryDAO.Instance.UpdateCategory(displayName, id))
            {
                MessageBox.Show("Cập nhật thành công danh mục");
                LoadCategoryFood();
                LoadCategoryIntoCombobox(cbFoodcategory);
                if(updateCategory != null)
                {
                    updateCategory(this, new EventArgs());
                }
            }
            else
            {
                MessageBox.Show("Có lỗi khi cập nhật danh mục");
            }
        }

        private void btnCategoryDelete_Click(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(txbCAtegoryID.Text);
            if (id == 8)
            {
                MessageBox.Show("Không thể xóa danh mục chờ");
            }
            else
            {
                if (CategoryDAO.Instance.DeleteCategory(id))
                {
                    MessageBox.Show("Xóa danh mục thành công");
                    LoadCategoryFood();
                    LoadCategoryIntoCombobox(cbFoodcategory);
                    LoadListFood();
                    if(deleteCategory != null)
                            {
                        deleteCategory(this, new EventArgs()); 
                    }
                }
                else
                {
                    MessageBox.Show("Có lỗi khi xóa");
                }
            }

            
        }

        private void btnSearchCategory_Click(object sender, EventArgs e)
        {
            categoryList.DataSource = SearchCategoryByName(txbSearchCategory.Text);
        }

        private event EventHandler insertCategory;
        public event EventHandler InsertCategory
        {
            add
            {
                insertCategory += value;
            }
            remove
            {
                insertCategory -= value;
            }
        }

        private event EventHandler updateCategory;
        public event EventHandler UpdateCategory
        {
            add
            {
                updateCategory += value;
            }
            remove
            {
                updateCategory -= value;
            }
        }

        private event EventHandler deleteCategory;
        public event EventHandler DeleteCategory
        {
            add
            {
                deleteCategory += value;
            }
            remove
            {
                deleteCategory -= value;
            }
        }






        private void cbFoodcategory_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void btnTableView_Click(object sender, EventArgs e)
        {
            LoadTableList();
        }

        private void btnTableAdd_Click(object sender, EventArgs e)
        {
            string displayName = txbTableName.Text;
            if(TableDAO.Instance.CheckTable(displayName) == 0)
            {
                if (TableDAO.Instance.InsertTable(displayName))
                {
                    MessageBox.Show("Thêm bàn thành công");
                    LoadTableList();
                    if (insertTable != null)
                    {
                        insertTable(this, new EventArgs());
                    }
                }
                else
                {
                    MessageBox.Show("Có lỗi khi thêm bàn");
                }
            }
            else
            {
                MessageBox.Show("Bàn đã tồn tại");
            }

        }

        private void btnSearchTable_Click(object sender, EventArgs e)
        {
            tableList.DataSource = SearchTableByName(txbSearchTable.Text);
        }

        private void btnEditTable_Click(object sender, EventArgs e)
        {
            string displayName = txbTableName.Text;
            int id = Convert.ToInt32(txbTableID.Text);

            if (TableDAO.Instance.UpdateTableName(displayName, id))
            {
                MessageBox.Show("Sửa thành công tên bàn");
                LoadTableList();
                if(updateTable != null)
                {
                    updateTable(this, new EventArgs());
                }
            }
            else
            {
                MessageBox.Show("Có lỗi khi sửa");
            }
        }

        private void btnDeleteTable_Click(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(txbTableID.Text);
            string status = cbTableStatus.Text;
            if (status == "Có người")
            {
                MessageBox.Show("Không thể xóa bàn đang có người");

            }
            else
            {
                if (TableDAO.Instance.DeleteTable(id))
                {
                    MessageBox.Show("Xóa bàn thành công");
                    LoadTableList();
                    if(deleteTable != null)
                    {
                        deleteTable(this, new EventArgs());
                    }
                }
                else
                {
                    MessageBox.Show("Có lỗi khi xóa bàn");
                }
            }
        }

        private event EventHandler insertTable;
        public event EventHandler InsertTable
        {
            add
            {
                insertTable += value;
            }
            remove
            {
                insertTable -= value;
            }
        }

        private event EventHandler updateTable;
        public event EventHandler UpdateTable
        {
            add
            {
                updateTable += value;
            }
            remove
            {
                updateTable -= value;
            }
        }

        private event EventHandler deleteTable;
        public event EventHandler DeleteTable
        {
            add
            {
                deleteTable += value;
            }
            remove
            {
                deleteTable -= value;
            }
        }




        private void btnAccountView_Click(object sender, EventArgs e)
        {
             LoadAccount();
        }

        private void btnSearchAccount_Click(object sender, EventArgs e)
        {
            accountList.DataSource = SearchAccountByName(txbSearchAccount.Text);
        }

        private void btnAccountAdd_Click(object sender, EventArgs e)
        {
            string userName = txbUserName.Text;
            string displayName = txbDisplayName.Text;
            int UserRole = cbAccountType.Text == "Staff" ? 2 : 1;

            if (AccountDAO.Instance.CheckAccount(userName) == 0)
            {
                if (AccountDAO.Instance.InsertAccount(userName, displayName, UserRole))
                {
                    MessageBox.Show("Thêm tài khoản thành công");
                    LoadAccount();
                    if(insertAccount != null)
                    {
                        insertAccount(this, new EventArgs());
                    }
                }
                else
                {
                    MessageBox.Show("Có lỗi khi thêm");
                }
            }
            else
            {
                MessageBox.Show("Tên đăng nhập đã tồn tại");
            }

        }

        private void btnAccountEdit_Click(object sender, EventArgs e)
        {
            string userName = txbUserName.Text;
            string displayName = txbDisplayName.Text;
            int idUserRole = cbAccountType.Text == "Staff" ? 2 : 1;

            if (AccountDAO.Instance.UpdateAccount(userName, displayName, idUserRole))
            {
                MessageBox.Show("Sửa tài khoản thành công");
                LoadAccount();
                if(updateAccount != null)
                {
                    updateAccount(this, new EventArgs());
                }
            }
            else
            {
                MessageBox.Show("Có lỗi khi sửa");
            }
        }

        private void btnAccountDelete_Click(object sender, EventArgs e)
        {
            string userName = txbUserName.Text;
            if (loginAccount.UserName.Equals(userName))
            {
                MessageBox.Show("Không thể xóa tài khoản đang đăng nhập!");
                return;
            }
            if (AccountDAO.Instance.DeleteAccount(userName))
            {
                MessageBox.Show("Xóa tài khoản thành công");
                LoadAccount();
                if(deleteAccount != null)
                {
                    deleteAccount(this, new EventArgs());
                }
            }
            else
            {
                MessageBox.Show("Có lỗi khi xóa tài khoản");
            }
        }

        private void btnResetPassword_Click(object sender, EventArgs e)
        {
            string userName = txbUserName.Text;

            if (AccountDAO.Instance.ResetPassword(userName))
            {
                MessageBox.Show("Đạt lại mật khẩu thành công");
            }
            else
            {
                MessageBox.Show("Có lỗi khi đặt lại mật khẩu");
            }
        }

        private event EventHandler insertAccount;
        public event EventHandler InsertAccount
        {
            add
            {
                insertAccount += value;
            }
            remove
            {
                insertAccount -= value;
            }
        }

        private event EventHandler updateAccount;
        public event EventHandler UpdateAccount
        {
            add
            {
                updateAccount += value;
            }
            remove
            {
                updateAccount -= value;
            }
        }

        private event EventHandler deleteAccount;
        public event EventHandler DeleteAccount
        {
            add
            {
                deleteAccount += value;
            }
            remove
            {
                deleteAccount -= value;
            }
        }

        #endregion


    }
}
