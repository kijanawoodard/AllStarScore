using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace AllStarScore.Admin.Models
{
    public class User
    {
        public string Id { get; set; }
		public string UserName { get; set; }
		public string Email { get; set; }
		public bool Enabled { get; set; }
        
        const string ConstantSalt = "H+iur}H+|tw)/59";
		protected string HashedPassword { get; private set; }	
        private string PasswordSalt { get; set; }

        public User()
        {
            PasswordSalt = Guid.NewGuid().ToString("N"); //factory value; thrown away when loaded from db
        }

        public User SetPassword(string pwd)
		{
			HashedPassword = GetHashedPassword(pwd);
			return this;
		}

		public bool ValidatePassword(string maybePwd)
		{
			return HashedPassword == GetHashedPassword(maybePwd);
		}

        private string GetHashedPassword(string pwd)
        {
            using (var sha = SHA256.Create())
            {
                var computedHash = sha.ComputeHash(Encoding.Unicode.GetBytes(PasswordSalt + pwd + ConstantSalt));
                return Convert.ToBase64String(computedHash);
            }
        }
    }
}