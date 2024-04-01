using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project
{
    public class User
    {

        public string ID { get; set; }
        public double Income { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public Family Family { get; set; }


        public User(string id, double income, string name, string email, string password)
        {
            this.ID = id;
            this.Income = income;
            this.Name = name;
            this.Email = email;
            this.Password = password;
            this.Family = null;
        }
    }
}
