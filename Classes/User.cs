using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace WPFModernVerticalMenu.Classes
{
    public class User
    {

        public string ID { get; set; }
        public double Income { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string ImageAvatar { get; set; }
        public string FamilyID { get; set; }
        public List<Report> Reports { get; set; }
        public double[] Values_category { get; set; }

        public double SumValues_category { get; set; }


        public User(string id, double Income, string name, string email, string password)
        {
            this.ID = id;
            this.Income = Income;
            this.Name = name;
            this.Email = email;
            this.Password = password;
            this.FamilyID = null;
            this.Reports = null;
            this.ImageAvatar = null;
        }
    }
}
