using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Reflection.Emit;
using System.Security.Cryptography;
using System.Text;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Homework_30._08._2024_Fluent_API_Annotations
{
    internal class Program
    {
        static void Main(string[] args)
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                //db.Database.EnsureDeleted();
                //db.Database.EnsureCreated();

                Console.WriteLine("Make a choice:\n\t\t 1 - Registration\n\t\t 2 - Authorization");
                string? choice = Console.ReadLine();
                Console.Clear();

                if (choice == "1")
                {
                    Register();
                }
                else if (choice == "2")
                {
                    Login();
                }
            }
        }

        static void Register()
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                Console.Write("Enter login: ");
                string? login = Console.ReadLine();
                Console.Clear();

                Console.Write("Enter password: ");
                string? password = Console.ReadLine();
                Console.Clear();

                string hashPass = ComputerHash(password);

                var user = new User { Login = login, Password = hashPass };
                db.Users.Add(user);
                db.SaveChanges();

                Console.WriteLine("Registration complete!");
                Thread.Sleep(2000);
                Console.Clear();
                Login();
            }
        }

        static void Login()
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                Console.Write("Enter login: ");
                string? login = Console.ReadLine();
                Console.Clear();

                Console.Write("Enter password: ");
                string? password = Console.ReadLine();
                Console.Clear();

                string hashPass = ComputerHash(password);

                var user = db.Users.FirstOrDefault(u => u.Login == login && u.Password == hashPass);

                if (user != null)
                {
                    Console.WriteLine("Complete!");
                    Thread.Sleep(2000);
                    Console.Clear();
                    ShowMainMenu();
                }
                else
                {
                    Console.WriteLine("Invalid login or password!");

                }
            }
        }

        static void ShowMainMenu()
        {
            Console.WriteLine("\t\tMAIN MENU");
        }

        private static string ComputerHash(string? input)
        {
            using (MD5 md5 = MD5.Create())
            {
                byte[] inputBytes = Encoding.ASCII.GetBytes(input);
                byte[] hashBytes = md5.ComputeHash(inputBytes);
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("X2"));
                }
                return sb.ToString();
            }
        }
    }


    public class User
    {
        public int Id { get; set; }
        public string? Login { get; set; }
        public string? Password { get; set; }  
    }


    public class ApplicationContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=DESKTOP-M496S5I;Database=USERS;Trusted_Connection=True; TrustServerCertificate=True");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
