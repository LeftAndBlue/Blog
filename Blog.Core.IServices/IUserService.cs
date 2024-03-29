using Blog.Core.Model.Models;

namespace Blog.Core.IServices
{
    public interface IUserService
    {
        int Add(User user);
        int Delete(int Id);
        int Update(User user);
        List<User> GetUsers(int Id);
    }
}
