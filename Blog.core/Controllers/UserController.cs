﻿using Blog.core.Models;
using Blog.Core.Auth;
using Blog.Core.IServices;
using Blog.Core.Model.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Blog.Core.Services;
namespace Blog.core.Controllers
{

    [Route("[controller]/[action]")]
        [ApiController]
        public class UserController : ControllerBase
        {
            private readonly JwtHelper _jwt;
            private readonly IUserService _userService;
            /// <summary>
            /// 初始化
            /// </summary>
            /// <param name="jwtHelper"></param>
            public UserController(JwtHelper jwtHelper, IUserService userService)
                {
                    _jwt = jwtHelper;
                    _userService = userService;
                }
                /// <summary>
                /// 获取Token
                /// </summary>
                /// <returns></returns>
                [HttpPost]
                public IActionResult GetToken(UserInfo user)
                {
                    //参数验证等等....
                    if (string.IsNullOrEmpty(user.UserName))
                    {
                        return Ok("参数异常！");
                    }

                    //这里可以连接mysql数据库做账号密码验证
                    if (string.IsNullOrEmpty(user.Password)) { throw new Exception("密码为空"); }
                  //  if (user.Password.Length != 数据库中的密码) {... }

                    //这里可以做Redis缓存验证等等


                    //这里获取Token，当然，这里也可以选择传结构体过去
                    var token = _jwt.CreateToken(user.UserName, user.PhoneNumber);
                    //解密后的Token
                    var PWToken = _jwt.JwtDecrypt( token);
                    return Ok(token+"解密后："+PWToken);
                }

                /// <summary>
                /// 获取自己的详细信息，其中 [Authorize] 就表示要带Token才行
                /// </summary>
                /// <returns></returns>
                [HttpPost]
                [Authorize]
                public IActionResult GetSelfInfo()
                {
                    //执行到这里，就表示已经验证授权通过了
                    /*
                     * 这里返回个人信息有两种方式
                     * 第一种：从Header中的Token信息反向解析出用户账号，再从数据库中查找返回
                     * 第二种：从Header中的Token信息反向解析出用户账号信息直接返回，当然，在前面创建        Token时，要保存进使用到的Claims中。
                    */
                    return Ok("授权通过了！");
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
