using Blog.Core.Auth;
using Blog.Core.Controllers;
using Blog.Core.IServices;
using Blog.Core.Model.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]/[action]")]
[ApiController]
//[Authorize]
[EnableCors("CorsPolicy")] //允许跨域
public class LoginController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly JwtHelper _jwt;

    public LoginController(IUserService userService, JwtHelper jwtHelper)
    {
        _userService = userService;
        _jwt = jwtHelper;
    }
    [HttpPost]
    public ActionResult<ResInfo> login(LoginRequest info)
    {

       
        if (_userService.loginCheck(info.EMAIL, PASSWORD: info.PASSWORD))
        {
           User user= _userService.GetUser(info.EMAIL, info.PASSWORD);
            var token = _jwt.CreateToken(user.NAME, user.ID);

            return new ResInfo(new userInfo(user.ID,user.NAME,token, user.EMAIL), "登录成功", "", 200);

        }

        else
            return BadRequest(new ResInfo(info, "无此用户信息", "", 500));
    }
    public record LoginRequest(string EMAIL, string PASSWORD);
    public record userInfo(string Id,string Name, string Token,string Email);
}

