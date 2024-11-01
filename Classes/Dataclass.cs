using System;
using System.Collections.Generic;
using System.Configuration;
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
using Newtonsoft.Json;

namespace WPFModernVerticalMenu.Classes
{
    public class Dataclass
    {
        public Dataclass()
        {

        }

        public bool ValidateEmail(string email)
        {
            string cond = "^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\\.[a-zA-Z]{2,}$";
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

        public List<User> LoadUsers(string file)
        {
            using (FileStream fs = new FileStream(file, FileMode.Open))
            {
                using (StreamReader reader = new StreamReader(fs))
                {
                    string json = reader.ReadToEnd();
                    return JsonConvert.DeserializeObject<List<User>>(json);
                }
            }
        }

        public List<Family> LoadFamilies(List<Family> families, string file)
        {
            FileStream fs1 = new FileStream(file, FileMode.Open);
            {
                families = (List<Family>)System.Text.Json.JsonSerializer.Deserialize(fs1, typeof(List<Family>));
                fs1.Close();
            }
            return families;
        }

        public User GetUserByEmail(string findemail, string file)
        {
            if (!System.IO.File.Exists(file))
            {
                return null;
            }

            List<User> users = LoadUsers(file);
            foreach (User user in users)
            {
                if (user.Email == findemail)
                {
                    return user;
                }
            }
            return null;
        }

        public void RegistrateUser(string email, string password, string name, double income, string file)
        {
            bool inUser = false;
            List<User> userss = new List<User>();
            if (!System.IO.File.Exists(file))
            {
                using (FileStream fs1 = new FileStream(file, FileMode.Create))
                {
                    string uuid = Guid.NewGuid().ToString();
                    userss.Add(new User(uuid, income, name, email, password));
                    using (StreamWriter writer = new StreamWriter(fs1))
                    {
                        writer.Write(JsonConvert.SerializeObject(userss));
                    }
                    //MessageBox.Show("Файл создан и пользователь добавлен!");
                }
            }
            else
            {
                userss = LoadUsers(file);

                foreach (User user in userss)
                {
                    if (user.Email == email)
                    {
                        //MessageBox.Show("Пользователь с таким Email'ом уже существует");
                        inUser = true;
                        break;
                    }
                }
                if (!inUser)
                {
                    using (FileStream fs2 = new FileStream(file, FileMode.Truncate))
                    {
                        string uuid = Guid.NewGuid().ToString();
                        userss.Add(new User(uuid, income, name, email, password));
                        using (StreamWriter writer = new StreamWriter(fs2))
                        {
                            writer.Write(JsonConvert.SerializeObject(userss));
                        }
                        //MessageBox.Show("Пользователь добавлен!");
                    }
                }
            }
        }

        public void DeleteReport(User user_del, Report report_del)
        {
            List<User> users = new List<User>();
            users = this.LoadUsers("users.json");
            foreach (User user in users)
            {
                if (user.Email == user_del.Email)
                {
                    foreach (Report report in user.Reports)
                    {
                        if (report.Current_month == report_del.Current_month)
                        {
                            user.Reports.Remove(report);
                            break;
                        }
                    }
                    break;
                }
            }
            FileStream fs1 = new FileStream("users.json", FileMode.Truncate);
            System.Text.Json.JsonSerializer.Serialize(fs1, users);
            fs1.Close();
        }

        public void CreateFamily(string file, User user)
        {
            List<Family> families = new List<Family>();
            List<User> users = new List<User>();
            users = LoadUsers("users.json");
            if (!System.IO.File.Exists(file))
            {
                FileStream fs1 = new FileStream(file, FileMode.Create);
                Family family = new Family();
                family.MembersID.Add(user.ID);
                families.Add(family);
                System.Text.Json.JsonSerializer.Serialize(fs1, families);
                fs1.Close();
                foreach(User user1 in users)
                {
                    if (user1.ID == user.ID)
                    {
                        user1.FamilyID = family.ID_family;
                        break;
                    }
                }
                FileStream fs2 = new FileStream("users.json", FileMode.Truncate);
                System.Text.Json.JsonSerializer.Serialize(fs2, users);
                fs2.Close();
                MessageBox.Show("Файл и семья созданы!");
            }
            else
            {
                families = LoadFamilies(families, file);
                FileStream fs2 = new FileStream(file, FileMode.Truncate);
                Family family = new Family();
                family.MembersID.Add(user.ID);
                families.Add(family);
                System.Text.Json.JsonSerializer.Serialize(fs2, families);
                fs2.Close();
                foreach (User user1 in users)
                {
                    if (user1.ID == user.ID)
                    {
                        user1.FamilyID = family.ID_family;
                        break;
                    }
                }
                FileStream fs1 = new FileStream("users.json", FileMode.Truncate);
                System.Text.Json.JsonSerializer.Serialize(fs1, users);
                fs1.Close();
                MessageBox.Show("Семья добавлена!");
            }
        }

        public void AddToFamily(string addUserID, string addFamilyID)
        {
            bool userExist = false;
            List<User> users = new List<User>();
            users = LoadUsers("users.json");
            foreach (User user in users)
            {
                if (addUserID == user.ID)
                {
                    userExist = true;
                    break;
                }
            }
            if (userExist)
            {
                List<Family> families = new List<Family>();
                families = LoadFamilies(families, "families.json");
                foreach (Family family in families)
                {
                    foreach (string userID in family.MembersID)
                    {
                        if (userID == addUserID)
                        {
                            MessageBox.Show("Этот пользователь уже есть в семье");
                            return;
                        }
                    }
                }
                foreach (Family family in families)
                {
                    if (family.ID_family == addFamilyID)
                    {
                        family.MembersID.Add(addUserID);
                        foreach (User user in users)
                        {
                            if (user.ID == addUserID)
                            {
                                user.FamilyID = addFamilyID;
                                break;
                            }
                        }
                        FileStream fs2 = new FileStream("users.json", FileMode.Truncate);
                        System.Text.Json.JsonSerializer.Serialize(fs2, users);
                        fs2.Close();
                        FileStream fs1 = new FileStream("families.json", FileMode.Truncate);
                        System.Text.Json.JsonSerializer.Serialize(fs1, families);
                        fs1.Close();
                        MessageBox.Show("Пользователь добавлен");
                        return;
                    }
                }
            }
            else
            {
                MessageBox.Show("Пользователь не найден");
                return;
            }
        }

        public void DeleteFromFamily(string deleteUserID, string deleteFamilyID)
        {
            bool userExist = false;
            List<User> users = new List<User>();
            List<Family> families = new List<Family>();
            families = LoadFamilies(families, "families.json");
            users = LoadUsers("users.json");
            foreach (User user in users)
            {
                if (deleteUserID == user.ID && deleteFamilyID == user.FamilyID)
                {
                    userExist = true;
                    break;
                }
            }
            if (userExist)
            {
                families = LoadFamilies(families, "families.json");
                foreach (Family family in families)
                {
                    if (family.ID_family == deleteFamilyID)
                    {
                        foreach (User user in users)
                        {
                            if (user.ID == deleteUserID)
                            {
                                if (user.ID == family.MembersID[0])
                                {
                                    MessageBox.Show("Вы не можете удалить самого себя");
                                    return;
                                }
                                else
                                {
                                    family.MembersID.Remove(deleteUserID);
                                    user.FamilyID = null;
                                    break;
                                }
                            }
                        }
                        FileStream fs2 = new FileStream("users.json", FileMode.Truncate);
                        System.Text.Json.JsonSerializer.Serialize(fs2, users);
                        fs2.Close();
                        FileStream fs1 = new FileStream("families.json", FileMode.Truncate);
                        System.Text.Json.JsonSerializer.Serialize(fs1, families);
                        fs1.Close();
                        MessageBox.Show("Пользователь удалён");
                        return;
                    }
                }
            }
            else
            {
                MessageBox.Show("Пользователь не найден");
                return;
            }
        }
    }
}
