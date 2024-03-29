using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Blog.Core.IServices;
using Blog.Core.Repository;
using Blog.Core.Model.Models;
using Blog.core.IRepository;
namespace Blog.Core.Services
{
    public class UserService : IUserService
    {
        UserRepository userRepository = new UserRepository();
        public int Add(User user)
        {
            try
            {
              return  userRepository.Add(user);
            }
            catch (Exception)
            {

                throw new NotImplementedException();
            }
          
        }

        public int Delete(int Id)
        {
            try
            {
                return userRepository.Delete(Id);
            }
            catch (Exception)
            {

                throw new NotImplementedException();
            }
        }

        public List<User> GetUsers(int Id)
        {
            try
            {
                return userRepository.GetUsers(Id);
            }
            catch (Exception)
            {

                throw new NotImplementedException();
            }
        }

        public int Update(User user)
        {
            try
            {
                return userRepository.Update(user);
            }
            catch (Exception)
            {

                throw new NotImplementedException();
            }
        }
    }
}
