using Blog.Core.Model.Models;

namespace Blog.Core.IServices
{
    public interface IUserService
    {
        int Add(User user);
        int Delete(string Id);
        int Update(User user);
        List<User> GetUsers(string Id);
        bool loginCheck(string EMAIL, string PASSWORD);
        User GetUser(string EMAIL, string PASSWORD);
    }
}
