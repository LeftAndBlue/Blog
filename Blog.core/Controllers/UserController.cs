using Blog.core.Models;
using Blog.Core.Auth;
using Blog.Core.IServices;
using Blog.Core.Model.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Blog.Core.Services;
using Microsoft.AspNetCore.Cors;
namespace Blog.core.Controllers
{

    [Route("[controller]/[action]")]
    [ApiController]
    //[Authorize]
    [EnableCors("CorsPolicy")] //允许跨域
    public class UserController : ControllerBase
    {

        private readonly IUserService _userService ;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        /// <summary>
        /// 增加
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPost]
        public int AddUser(User user)
        {
            // User user = new User() { Id = 2024325, Name = "Czm", Age = 20 };

            return _userService.Add(user);
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        public int DeleteUser(int id)
        {
            return _userService.Delete(id);
        }
        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPut]
        public int UpdateUsre(User user)
        {
            return _userService.Update(user);
        }
        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public List<User> GetUser(int id)
        {
            return _userService.GetUsers(id);
        }
    }
}
