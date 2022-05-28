using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Configuration;
using MySql.Data.MySqlClient;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace LoginWebApplication.Operations
{
    public class UserOperations
    {

       //private string connString = ConfigurationManager.AppSettings["sqlConnection"];

        private string connString = @"server=127.0.0.1;userid=root;password=4tsb?~8K_3~zcVn9cNU]Q4.@;database=buyselldb";

        public bool CreateUser(User user) {
            var con = new MySqlConnection(connString);
            var cmd = new MySqlCommand("INSERT INTO users (firstName,lastName,id,password,email) VALUES (@firstName,@lastName,@id,@password,@email)", con);
            con.Open();
            cmd.Parameters.AddWithValue("@firstName", user.FirstName);
            cmd.Parameters.AddWithValue("@lastName", user.LastName);
            cmd.Parameters.AddWithValue("@id", user.Id);
            cmd.Parameters.AddWithValue("@password", user.Password);
            cmd.Parameters.AddWithValue("@email", user.Email);
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

        public User SaltAndHash(User user)
        {
            byte[] salt = new byte[128 / 8];
            using (var rngCsp = new RNGCryptoServiceProvider())
            {
                rngCsp.GetNonZeroBytes(salt);
            }

            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: user.Password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA512,
                iterationCount: 100000,
                numBytesRequested: 256 / 8));

            user.Password = hashed;

            return user;
        }
    }
}
