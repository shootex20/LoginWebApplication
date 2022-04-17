using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Configuration;
using MySql.Data.MySqlClient;

namespace LoginWebApplication.Operations
{
    public class UserOperations
    {

       //private string connString = ConfigurationManager.AppSettings["sqlConnection"];

        private string connString = @"server=127.0.0.1;userid=root;password=password;database=buyselldb";

        public bool CreateUser(Person person) {
            var con = new MySqlConnection(connString);
            var cmd = new MySqlCommand("INSERT INTO users (firstName,lastName,id,password,email) VALUES (@firstName,@lastName,@id,@password,@email)", con);
            con.Open();
            cmd.Parameters.AddWithValue("@firstName", person.FirstName);
            cmd.Parameters.AddWithValue("@lastName", person.LastName);
            cmd.Parameters.AddWithValue("@id", person.Id);
            cmd.Parameters.AddWithValue("@password", person.Password);
            cmd.Parameters.AddWithValue("@email", person.Email);
            cmd.Prepare();
            var result = cmd.ExecuteNonQuery();
            if (result > 0)
            {
                con.Close();
                return true;
            }
            else
            {
                con.Close();
                return false;
            }
        } 
    }
}
