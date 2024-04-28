using Blog.core.IRepository;
using Blog.Core.Model.Models;
using static Blog.Core.Common.DbContext;
namespace Blog.Core.Repository
{
    public class UserRepository : IUserRepository
    {
        public  int Add(User user)
        {
            var line = db.Insertable(user).ExecuteCommand();
            return line;
        }

        public int Delete(string UserId)
        {
            var line = db.Deleteable<User>(new User
            {
                ID = UserId
            }).ExecuteCommand();
            return line;
        }

        public List<User> GetUsers(string Id)
        {
            List<User> users;
            if (Id is not null)
            {
                users = db.Queryable<User>().Where(it => it.ID == Id).ToList();
            }
            else
            {
                users = db.Queryable<User>().ToList();
            }
            return users;
        }

        public int Update(User user)
        {
            var res = db.Updateable<User>(user).ExecuteCommand();
            return res;
        }

        public User GetUserInfo(string Email,string Password)
        {
            User user = db.Queryable<User>().Where(r => r.EMAIL == Email && r.PASSWORD == Password).First();
          
            return (User)user;
        }
    }
}
