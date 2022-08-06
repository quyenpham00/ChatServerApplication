using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ChatServerApplication.Uilities
{
    public class PasswordEncoder
    {
        public string HashingPassword(string password)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(password));
                StringBuilder passwordHash = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    passwordHash.Append(bytes[i].ToString("x2"));
                }
                return passwordHash.ToString();
            }
        }
        //public bool CheckPasswordValid(string password)
        //{
        //    try
        //    {
        //        string pattern = "^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,}$";
        //        Match match = Regex.Match(password, pattern);
        //        return match.Success;
        //    }
        //    catch (Exception exception)
        //    {
        //        Console.WriteLine(exception.Message);
        //        return false;
        //    }
        //}
    }
}
