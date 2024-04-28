using Blog.Core.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.core.IRepository
{
    public interface IUserRepository
    {
        int Add(User user);
        int Delete(string Id);
        int Update(User user);
        List<User> GetUsers(string Id);
        User GetUserInfo(string Email, string Password);
    }
}
