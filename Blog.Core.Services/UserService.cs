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
        private  IUserRepository _userService ;

        public UserService(IUserRepository userService)
        {
            _userService = userService;
        }
        public int Add(User user)
        {
            try
            {
                return _userService.Add(user);
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
                return _userService.Delete(Id);
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
                return _userService.GetUsers(Id);
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
                return _userService.Update(user);
            }
            catch (Exception)
            {

                throw new NotImplementedException();
            }
        }
    }
}
