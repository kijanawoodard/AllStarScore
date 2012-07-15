using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using AllStarScore.Models;
using AllStarScore.Models.Commands;

namespace AllStarScore.Admin.Models
{
    public class User : ICanBeUpdatedByCommand, IBelongToCompany, IGenerateMyId
    {
        public string Id { get; set; }

		public string Name { get; set; }
		public string Email { get; set; }
		public bool Enabled { get; set; }
        
        const string ConstantSalt = "H+iur}H+|tw)/59";
		protected string HashedPassword { get; private set; }	
        private string PasswordSalt { get; set; }

        public string CompanyId { get; set; }
        public string LastCommand { get; set; }
        public string LastCommandBy { get; set; }
        public DateTime LastCommandDate { get; set; }

        public User()
        {
            PasswordSalt = Guid.NewGuid().ToString("N"); //factory value; thrown away when loaded from db
        }

        public void Update(UserCreateCommand command)
        {
            Email = command.Email;
            Name = command.UserName;
            SetPassword(command.Password);
            Enabled = true;

            this.RegisterCommand(command);
        }

        public bool ValidatePassword(string maybePwd)
		{
			return HashedPassword == GetHashedPassword(maybePwd);
		}

        private void SetPassword(string pwd)
        {
            HashedPassword = GetHashedPassword(pwd);
        }

        private string GetHashedPassword(string pwd)
        {
            using (var sha = SHA256.Create())
            {
                var computedHash = sha.ComputeHash(Encoding.Unicode.GetBytes(PasswordSalt + pwd + ConstantSalt));
                return Convert.ToBase64String(computedHash);
            }
        }

        public string GenerateId()
        {
            return CompanyId + "/user/";
        }

        public override string ToString()
        {
            return string.Format("Id: {0}, UserName: {1}, Email: {2}", Id, Name, Email);
        }

        public bool Equals(User other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(other.Id, Id);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof (User)) return false;
            return Equals((User) obj);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}