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

        public int Delete(int UserId)
        {
            var line = db.Deleteable<User>(new User
            {
                Id = UserId
            }).ExecuteCommand();
            return line;
        }

        public List<User> GetUsers(int Id)
        {
            List<User> users;
            if (Id is not 0)
            {
                users = db.Queryable<User>().Where(it => it.Id == Id).ToList();
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
    }
}
