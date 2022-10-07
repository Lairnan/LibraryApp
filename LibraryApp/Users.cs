using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryApp
{
    public class Users
    {
        public int Id { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public Roles Role { get; set; }

        private static List<Users> _users = new();

        public static ICollection<Users> GetUsers()
        {
            return _users;
        }

        public static void Add(Users user)
        {
            if(!Roles.GetRoles().Contains(user.Role))
                Roles.Add(user.Role);
            _users.Add(user);
        }

        public static void AddRange(ICollection<Users> users)
        {
            foreach (var user in users)
            {
                if (!Roles.GetRoles().Contains(user.Role))
                    Roles.Add(user.Role);
                _users.Add(user);
            }
        }

        public static async void AddAsync(Users user)
        {
            await Task.Run(() =>
            {
                if (!Roles.GetRoles().Contains(user.Role))
                    Roles.AddAsync(user.Role);
                _users.Add(user);
            });
        }

        public static async void AddRangeAsync(ICollection<Users> users)
        {
            foreach (var user in users)
            {
                await Task.Run(() =>
                {
                    if (!Roles.GetRoles().Contains(user.Role))
                        Roles.AddAsync(user.Role);
                    _users.Add(user);
                });
            }
        }
    }

    public class Roles
    {
        public int Id { get; set; }
        public string Name { get; set; }

        private static List<Roles> _roles = new();

        public static ICollection<Roles> GetRoles()
        {
            return _roles;
        }

        public static void Add(Roles role)
        {
            _roles.Add(role);
        }

        public static void AddRange(ICollection<Roles> roles)
        {
            foreach (var role in roles)
            {
                _roles.Add(role);
            }
        }

        public static async void AddAsync(Roles role)
        {
            await Task.Run(() =>
            {
                _roles.Add(role);
            });
        }

        public static async void AddRangeAsync(ICollection<Roles> roles)
        {
                foreach (var role in roles)
                {
                    await Task.Run(() =>
                    {
                        _roles.Add(role);
                    });
                }
        }
    }

    public class TestUsers
    {
        public static ICollection<Users> GetUsersCollection()
        {

            Users.AddRange(new List<Users>
            {
                new() {Id = 1, Login = "Test", Password = "Pass", Role = new Roles() {Id = 1, Name = "test"}},
                new() {Id = 2, Login = "Test2", Password = "Pass2", Role = new Roles() {Id = 2, Name = "test2"}},
                new() {Id = 3, Login = "Test3", Password = "Pass3", Role = new Roles() {Id = 3, Name = "test3"}}
            });
            return Users.GetUsers();
        }
    }
}
