using webapbackend.Models;

namespace webapbackend.Interface
{
    public interface IUser
    {
        ICollection<User> GetUsers();
        User GetUser(int id);
        User GetUserByEmail (string email);
        bool UserExists(int id);
        bool UserExists(string email);
        bool CreateUser(User user);
        bool UpdateUser(User user);
        bool DeleteUser(User user);
        bool Save();
        bool IsUserAdmin(int userId);
    }
}
