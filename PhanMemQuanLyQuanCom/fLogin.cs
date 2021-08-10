using PhanMemQuanLyQuanCom.DAO;
using PhanMemQuanLyQuanCom.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PhanMemQuanLyQuanCom
{
    public partial class fLogin : Form
    {
        public fLogin()
        {
            InitializeComponent();
        }


        bool Login(string userName, string password)
        {
            return AccountDAO.Instance.Login(userName, password);
        }


        private void btnLogin_Click(object sender, EventArgs e)
        {
            string userName = tbxUserName.Text;
            string password = tbxPassword.Text;
            if (Login(userName, password))
            {
                Account loginAccount = AccountDAO.Instance.GetAccountByUserName(userName);
                fTableManage f = new fTableManage(loginAccount);
                this.Hide();
                f.ShowDialog();
                this.Show();
            }
            else
                MessageBox.Show("Sai tên đăng nhập hoặc mật khẩu!");
        }



        private void tbxExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void fLogin_FormClosing(object sender, FormClosingEventArgs e)
        {
            if(MessageBox.Show("Bạn có thật sự muốn thoát chương trình?","Thông báo",MessageBoxButtons.OKCancel) != DialogResult.OK)
            {
                e.Cancel = true;
            }
        }
    }
}
