using webapbackend.Data;
using webapbackend.Interface;
using webapbackend.Models;

namespace webapbackend.Repository
{
    public class UserRepository : IUser
    {
        private readonly DataContext _context;

        public UserRepository(DataContext context)
        {
            _context = context;
        }

        public User GetUserByEmail(string email)
        {
            return _context.Users.FirstOrDefault(p => p.Email == email);
        }

        public bool UserExists(int id)
        {
            return _context.Users.Any(c => c.Id == id);
        }

        public bool UserExists(string email)
        {
            return _context.Users.Any(u => u.Email == email);
        }

        public bool CreateUser(User user)
        {
            _context.Add(user);
            return Save();
        }

        public bool DeleteUser(User user)
        {
            _context.Remove(user);
            return Save();
        }

        public ICollection<User> GetUsers()
        {
            return _context.Users.ToList();
        }

        public User GetUser(int id)
        {
            return _context.Users.FirstOrDefault(e => e.Id == id);
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0;
        }

        public bool UpdateUser(User user)
        {
            _context.Update(user);
            return Save();
        }

        public bool IsUserAdmin(int userId)
        {
            var user = _context.Users.FirstOrDefault(u => u.Id == userId);
            return user != null && user.isAdmin;
        }
    }
}
