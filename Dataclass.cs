using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.RightsManagement;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Project
{
    public class Dataclass
    {
        public Dataclass()
        {

        }

        public bool ValidateEmail(string email)
        {
            string cond = @"^(?!\.)(""([^""\r\\]|\\[""\r\\])*""|"
                        + @"([-a-z0-9!#$%&'*+/=?^_`{|}~]|(?<!\.)\.)*)(?<!\.)"
                        + @"@[a-z0-9][\w\.-]*[a-z0-9]\.[a-z][a-z\.]*[a-z]$";
            Regex regex = new Regex(cond);
            return regex.IsMatch(email);
        }

        public bool ValidatePassword(string password, string againPassword)
        {
            if (password.Length < 6)
            {
                return false;
            }

            if (!password.Any(c => char.IsUpper(c)))
            {
                return false;
            }

            if (!password.Any(c => char.IsDigit(c)))
            {
                return false;
            }

            if (!password.Any(c => "!@#$%^".Contains(c)))
            {
                return false;
            }

            if (password != againPassword)
            {
                return false;
            }

            return true;
        }

        public bool ValidateIncome(string income)
        {
            if (double.TryParse(income, out double value))
            {
                // Check if the parsed value is greater than zero
                if (value > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        public bool ValidateName(string name)
        {
            if (name.Length > 20)
            {
                return false;
            }
            return true;
        }

        public bool ValidateFields(string f1, string f2, string f3, string f4, string f5)
        {
            if (string.IsNullOrWhiteSpace(f1) || string.IsNullOrWhiteSpace(f2) || string.IsNullOrWhiteSpace(f3) || string.IsNullOrWhiteSpace(f4) || string.IsNullOrWhiteSpace(f5))
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public List<User> LoadUsers(List<User> users, string file)
        {
            FileStream fs1 = new FileStream(file, FileMode.Open);
            {
                users = (List<User>)JsonSerializer.Deserialize(fs1, typeof(List<User>));
                fs1.Close();
            }
            return users;
        }

        public void RegistrateUser(string email, string password, string name, double income, string file)
        {
            bool inUser = false;
            List<User> userss = new List<User>();
            if (!System.IO.File.Exists(file))
            {
                FileStream fs1 = new FileStream(file, FileMode.Create);
                {
                    string uuid = Guid.NewGuid().ToString();
                    userss.Add(new User(uuid, income, name, email, password));
                    JsonSerializer.Serialize(fs1, userss);
                    MessageBox.Show("Файл создан и пользователь добавлен!");
                    fs1.Close();
                }
            }
            else
            {
                userss = LoadUsers(userss, file);

                foreach (User user in userss)
                {
                    if (user.Email == email)
                    {
                        MessageBox.Show("Пользователь с таким Email'ом уже существует");
                        inUser = true;
                        break;
                    }
                }
                if (!inUser)
                {
                    FileStream fs2 = new FileStream(file, FileMode.Truncate);
                    string uuid = Guid.NewGuid().ToString();
                    userss.Add(new User(uuid, income, name, email, password));
                    JsonSerializer.Serialize(fs2, userss);
                    MessageBox.Show("Пользователь добавлен!");
                    fs2.Close();
                }
            }
        }
    }
}
