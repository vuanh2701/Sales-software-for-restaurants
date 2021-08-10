using PhanMemQuanLyQuanCom.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace PhanMemQuanLyQuanCom.DAO
{
    public class AccountDAO
    {
        private static AccountDAO instance;

        public static AccountDAO Instance 
        { 
            get
            {
                if(instance == null)
                {
                    instance = new AccountDAO();
                }
                return instance;
            }
            private set => instance = value; 
        }

        private AccountDAO() { }

        public bool Login(string userName, string password)
        {
            // byte array representation of that string
            byte[] encodedPassword = new UTF8Encoding().GetBytes(password);

            // need MD5 to calculate the hash
            byte[] hash = ((HashAlgorithm)CryptoConfig.CreateFromName("MD5")).ComputeHash(encodedPassword);

            // string representation (similar to UNIX format)
            string encoded = BitConverter.ToString(hash);

            string query = "USP_Login @userName , @password";
            DataTable result = DataProvider.Instance.ExecuteQuery(query, new object[] {userName, encoded});

            
            return result.Rows.Count > 0;
        }

        public Account GetAccountByUserName(string userName)
        {
            string query = "SELEct * FROM Account WHere UserName = '" + userName + "'";
            DataTable data = DataProvider.Instance.ExecuteQuery(query);

            foreach  (DataRow item in data.Rows)
            {
                return new Account(item);
            }

            return null;
        }

        public List<Account> GetListAccount()
        {
            List<Account> list  = new List<Account>();
            DataTable data = DataProvider.Instance.ExecuteQuery("SELECT * FROM Account");
            
            foreach (DataRow item in data.Rows)
            {
                Account acc = new Account(item);
                list.Add(acc);
            }
            
            return list;
            
        }

        public DataTable GetListAccountOnDataTable()
        {
            string query = "SELECT Account.id, Account.UserName AS[Tên đăng nhập], Account.DisplayName AS[Tên hiển thị], UserRole.DisplayName AS[Loại tài khoản] FROm Account join UserRole ON Account.idUserrole = UserRole.id";
            return DataProvider.Instance.ExecuteQuery(query);

        }

        public DataTable SearchAccountByName(string name)
        {
            //List<Account> list = new List<Account>();
            string query = string.Format("SELECT Account.id, Account.UserName AS[Tên đăng nhập], Account.DisplayName AS[Tên hiển thị], UserRole.DisplayName AS[Loại tài khoản] FROm Account join UserRole ON Account.idUserrole = UserRole.id WHERE dbo.fuConvertToUnsign1(Account.DisplayName) like N'%' + dbo.fuConvertToUnsign1(N'{0}') + '%'", name); 
            //DataTable data = DataProvider.Instance.ExecuteQuery(query);
            return DataProvider.Instance.ExecuteQuery(query);

            //foreach (DataRow item in data.Rows)
            //{
            //    Account acc = new Account(item);
            //    list.Add(acc);
            //}
            //return list;
        }

        public bool UpdateAccount(string userName, string displayName, string password, string newPassword)
        {

            // byte array representation of that string
            byte[] encodedPassword = new UTF8Encoding().GetBytes(newPassword);

            // need MD5 to calculate the hash
            byte[] hash = ((HashAlgorithm)CryptoConfig.CreateFromName("MD5")).ComputeHash(encodedPassword);

            // string representation (similar to UNIX format)
            string encoded = BitConverter.ToString(hash);

            int result = DataProvider.Instance.ExcuteNonQuery("EXEC USP_UpdateAccount @userNAme , @displayName , @password , @newPassword", new object[] {userName, displayName, password, encoded});

            return result > 0;
        }

        public int CheckAccount (string userName)
        {
            string query = string.Format("SELECT * FROm Account where UserName = N'{0}'", userName);

            var result =  DataProvider.Instance.ExcuteScalar(query) == null ? 0 : 1;
            return result;

        }

        public bool InsertAccount(string userName, string displayname, int userRole )
        {
           
            int result = DataProvider.Instance.ExcuteNonQuery("EXEC USP_InsertAccounts1 @userName , @displayName , @userRole", new object[] { userName, displayname, userRole }); 
            return result > 0;
        }

        public bool UpdateAccount(string userName, string displayName, int idUserRole)
        {
            string query = string.Format("Update Account SET DisplayName = N'{0}', IdUserRole = {1} where userName = N'{2}'",  displayName, idUserRole, userName);
            int result = DataProvider.Instance.ExcuteNonQuery(query);
            return result > 0;
        }

        public bool DeleteAccount(string userName)
        {
            string query = string.Format("Delete Account where userName = N'{0}'", userName);
            int result = DataProvider.Instance.ExcuteNonQuery(query);
            return result > 0;
        }

        public bool ResetPassword(string userName)
        {
            string password = "0";
            // byte array representation of that string
            byte[] encodedPassword = new UTF8Encoding().GetBytes(password);

            // need MD5 to calculate the hash
            byte[] hash = ((HashAlgorithm)CryptoConfig.CreateFromName("MD5")).ComputeHash(encodedPassword);

            // string representation (similar to UNIX format)
            string encoded = BitConverter.ToString(hash);

            string query = string.Format("Update Account SET Password = N'" + encoded + "' where userName = N'{0}'", userName);
            int result = DataProvider.Instance.ExcuteNonQuery(query);
            return result > 0;
        }
    }
}
