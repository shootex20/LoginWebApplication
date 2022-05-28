using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Configuration;
using MySql.Data.MySqlClient;

namespace LoginWebApplication
{
    public class ValidateUser
    {


        //private string connString = ConfigurationManager.AppSettings["sqlConnection"];

        private string connString = @"server=127.0.0.1;userid=root;password=4tsb?~8K_3~zcVn9cNU]Q4.@;database=buyselldb";

        public bool ValidateUserEmail(User user)
        {
            string pattern = @"^(?!\.)(""([^""\r\\]|\\[""\r\\])*""|" + @"([-a-z0-9!#$%&'*+/=?^_`{|}~]|(?<!\.)\.)*)(?<!\.)" + @"@[a-z0-9][\w\.-]*[a-z0-9]\.[a-z][a-z\.]*[a-z]$";
            var regex = new Regex(pattern, RegexOptions.IgnoreCase);
            return regex.IsMatch(user.Email);
        }
        public bool ValidateEmailNotRegistered(User user) {
            var con = new MySqlConnection(connString);
            var cmd = new MySqlCommand("SELECT COUNT(*) FROM users WHERE email = @email", con);
            con.Open();
            cmd.Parameters.AddWithValue("@email", user.Email);
            cmd.Prepare();
            Int64 UserExist = (Int64) cmd.ExecuteScalar();
            if (UserExist > 0)
            {
                con.Close();
                return true;
            }
            else {
                con.Close();
                return false; 
            }
        }

        public bool ValidateUserIsNotEmpty(User user) {

            if (user.FirstName != null && user.LastName != null && user.Id != null && user.Email != null && user.Password != null)
            {
                return false;
            }
            else {
                return true;
            }
        }
    }
}
