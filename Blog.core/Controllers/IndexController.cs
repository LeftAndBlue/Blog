using Blog.Core.IServices;
using Blog.Core.Model.Models;
using Blog.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace Blog.Core.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
[Authorize]
[EnableCors("CorsPolicy")] //允许跨域]
public class IndexController : ControllerBase
{
    private readonly IUserService _userService;
    public IndexController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet]
    public ActionResult<ResInfo> UserInfo(string ID)
    {

        User user = _userService.GetUsers(ID).FirstOrDefault();

        if (user is not null)
        {
            return new ResInfo(user, "用户信息", "", 200);
        }

        else
            return BadRequest(new ResInfo("", "无此用户信息", "", 500));
    }
}

