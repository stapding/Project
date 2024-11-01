using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using WPFModernVerticalMenu.Classes;

namespace WPFModernVerticalMenuTests
{
    [TestClass]
    public class UnitTest1
    {
        private const string TestFile = "test_users.json";

        [TestInitialize]
        public void Setup()
        {
            if (File.Exists(TestFile))
            {
                File.Delete(TestFile);
            }

        }

        [TestCleanup]
        public void Cleanup()
        {
            if (File.Exists(TestFile))
            {
                File.Delete(TestFile);
            }
        }

        [TestMethod]
        public void RegistrateUser_NewFile_CreatesFileAndAddsUser()
        {
            // Arrange
            var target = new Dataclass();
            string email = "test@example.com";
            string password = "password";
            string name = "Test User";
            double income = 50000;

            // Act
            target.RegistrateUser(email, password, name, income, TestFile);

            // Assert
            Assert.IsTrue(File.Exists(TestFile));
            var users = target.LoadUsers(TestFile);
            Assert.AreEqual(1, users.Count);
            Assert.AreEqual(email, users[0].Email);
            Assert.AreEqual(password, users[0].Password);
            Assert.AreEqual(name, users[0].Name);
            Assert.AreEqual(income, users[0].Income);
        }

        [TestMethod]
        public void RegistrateUser_ExistingFile_AddsUserIfNotExists()
        {
            // Arrange
            var target = new Dataclass();
            string email1 = "test1@example.com";
            string password1 = "password1";
            string name1 = "Test User 1";
            double income1 = 50000;

            string email2 = "test2@example.com";
            string password2 = "password2";
            string name2 = "Test User 2";
            double income2 = 60000;

            // Act
            target.RegistrateUser(email1, password1, name1, income1, TestFile);
            target.RegistrateUser(email2, password2, name2, income2, TestFile);

            // Assert
            var users = target.LoadUsers(TestFile);
            Assert.AreEqual(2, users.Count);
            Assert.AreEqual(email1, users[0].Email);
            Assert.AreEqual(password1, users[0].Password);
            Assert.AreEqual(name1, users[0].Name);
            Assert.AreEqual(income1, users[0].Income);

            Assert.AreEqual(email2, users[1].Email);
            Assert.AreEqual(password2, users[1].Password);
            Assert.AreEqual(name2, users[1].Name);
            Assert.AreEqual(income2, users[1].Income);
        }

        [TestMethod]
        public void RegistrateUser_ExistingFile_DoesNotAddDuplicateUser()
        {
            // Arrange
            var target = new Dataclass();
            string email = "test@example.com";
            string password = "password";
            string name = "Test User";
            double income = 50000;

            // Act
            target.RegistrateUser(email, password, name, income, TestFile);
            target.RegistrateUser(email, password, name, income, TestFile);

            // Assert
            var users = target.LoadUsers(TestFile);
            Assert.AreEqual(1, users.Count);
            Assert.AreEqual(email, users[0].Email);
            Assert.AreEqual(password, users[0].Password);
            Assert.AreEqual(name, users[0].Name);
            Assert.AreEqual(income, users[0].Income);
        }

        [TestMethod]
        public void GetUserByEmail_ExistingEmail_ReturnsUser()
        {
            // Arrange
            var target = new Dataclass();
            string email = "test1@example.com";

            // Создаем тестовый файл с пользователями
            var users = new List<User>
            {
                new User("1", 50000, "Test User 1", email, "password1"),
                new User("2", 60000, "Test User 2", "test2@example.com", "password2")
            };

            using (FileStream fs = new FileStream(TestFile, FileMode.Create))
            {
                using (StreamWriter writer = new StreamWriter(fs))
                {
                    writer.Write(JsonConvert.SerializeObject(users));
                }
            }

            // Act
            var user = target.GetUserByEmail(email, TestFile);

            // Assert
            Assert.IsNotNull(user);
            Assert.AreEqual(email, user.Email);
            Assert.AreEqual("Test User 1", user.Name);
            Assert.AreEqual(50000, user.Income);
            Assert.AreEqual("password1", user.Password);
        }

        [TestMethod]
        public void GetUserByEmail_NonExistingEmail_ReturnsNull()
        {
            // Arrange
            var target = new Dataclass();
            string email = "nonexistent@example.com";

            // Создаем тестовый файл с пользователями
            var users = new List<User>
            {
                new User("1", 50000, "Test User 1", "test1@example.com", "password1"),
                new User("2", 60000, "Test User 2", "test2@example.com", "password2")
            };

            using (FileStream fs = new FileStream(TestFile, FileMode.Create))
            {
                using (StreamWriter writer = new StreamWriter(fs))
                {
                    writer.Write(JsonConvert.SerializeObject(users));
                }
            }

            // Act
            var user = target.GetUserByEmail(email, TestFile);

            // Assert
            Assert.IsNull(user);
        }

        [TestMethod]
        public void LoadUsers_ExistingFile_ReturnsUsers()
        {
            // Arrange
            var target = new Dataclass();

            // Создаем тестовый файл с пользователями
            var users = new List<User>
            {
                new User("1", 50000, "Test User 1", "test1@example.com", "password1"),
                new User("2", 60000, "Test User 2", "test2@example.com", "password2")
            };

            using (FileStream fs = new FileStream(TestFile, FileMode.Create))
            {
                using (StreamWriter writer = new StreamWriter(fs))
                {
                    writer.Write(JsonConvert.SerializeObject(users));
                }
            }

            // Act
            var loadedUsers = target.LoadUsers(TestFile);

            // Assert
            Assert.AreEqual(2, loadedUsers.Count);
            Assert.AreEqual("test1@example.com", loadedUsers[0].Email);
            Assert.AreEqual("password1", loadedUsers[0].Password);
            Assert.AreEqual("Test User 1", loadedUsers[0].Name);
            Assert.AreEqual(50000, loadedUsers[0].Income);

            Assert.AreEqual("test2@example.com", loadedUsers[1].Email);
            Assert.AreEqual("password2", loadedUsers[1].Password);
            Assert.AreEqual("Test User 2", loadedUsers[1].Name);
            Assert.AreEqual(60000, loadedUsers[1].Income);
        }

        [TestMethod]
        [ExpectedException(typeof(FileNotFoundException))]
        public void LoadUsers_NonExistingFile_ThrowsException()
        {
            // Arrange
            var target = new Dataclass();
            string nonExistingFile = "non_existing_users.json";

            // Act
            target.LoadUsers(nonExistingFile);

            // Assert
            // ExpectedException attribute will handle the exception
        }


    }
}
