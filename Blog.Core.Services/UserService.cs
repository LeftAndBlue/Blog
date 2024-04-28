using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Blog.Core.IServices;
using Blog.Core.Repository;
using Blog.Core.Model.Models;
using Blog.core.IRepository;
using System.Data;
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

        public int Delete(string Id)
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

        public List<User> GetUsers(string Id)
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
        public bool loginCheck(string EMAIL, string PASSWORD)
        {
            User user = _userService.GetUserInfo(EMAIL, PASSWORD);
            if (user == null)
            {
                return false;
            }

            user.LASTLOGINTIME = DateTime.Now;
            _userService.Update(user);

            return true;

        }
        public User GetUser(string EMAIL, string PASSWORD)
        {
            return _userService.GetUserInfo(EMAIL, PASSWORD);
        }
    }
}
