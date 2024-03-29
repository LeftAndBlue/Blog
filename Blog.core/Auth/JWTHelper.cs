using Microsoft.Extensions.Primitives;
using Microsoft.IdentityModel.Tokens;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Blog.Core.Auth
{
    /// <summary>
    /// 授权JWT类
    /// </summary>
    public class JwtHelper
    {
        private readonly IConfiguration _configuration;

        /// <summary>
        /// Token配置
        /// </summary>
        /// <param name="configuration"></param>
        public JwtHelper(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        /// <summary>
        /// 创建Token 这里面可以保存自己想要的信息
        /// </summary>
        /// <param name="username"></param>
        /// <param name="mobile"></param>
        /// <returns></returns>
        public string CreateToken(string username, string mobile)
        {
            try
            {
                // 1. 定义需要使用到的Claims
                var claims = new[]
                {
                    new Claim("username", username),
                    new Claim("mobile", mobile),
                    /* 可以保存自己想要信息，传参进来即可
                    new Claim("sex", "sex"),
                    new Claim("limit", "limit"),
                    new Claim("head_url", "xxxxx")
                    */
                };

                // 2. 从 appsettings.json 中读取SecretKey
                var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:SecKey"]));

                // 3. 选择加密算法
                var algorithm = SecurityAlgorithms.HmacSha256;

                // 4. 生成Credentials
                var signingCredentials = new SigningCredentials(secretKey, algorithm);

                // 5. 根据以上，生成token
                var jwtSecurityToken = new JwtSecurityToken(
                    _configuration["Jwt:Issuer"],    //Issuer
                    _configuration["Jwt:ExpireSeconds"],  //ExpireSeconds
                    claims,                          //Claims,
                    DateTime.Now,                    //notBefore
                    DateTime.Now.AddSeconds(30),     //expires
                    signingCredentials               //Credentials
                );

                // 6. 将token变为string
                var token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);

                return token;
            }
            catch (Exception)
            {

                throw;
            }

        }
        /// <summary>
        /// 获取信息
        /// </summary>
        /// <param name="jwt"></param>
        /// <returns></returns>
        public static string ReaderToken(string jwt)
        {
            var str = string.Empty;
            try
            {
                //获取Token的三种方式
                //第一种直接用JwtSecurityTokenHandler提供的read方法
                var jwtHander = new JwtSecurityTokenHandler();
                JwtSecurityToken jwtSecurityToken = jwtHander.ReadJwtToken(jwt);
                str = jwtSecurityToken.ToString();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            return str;
        }

        /// <summary>
        /// 解密jwt
        /// </summary>
        /// <param name="jwt"></param>
        /// <returns></returns>
        public string JwtDecrypt(string jwt)
        {

            StringBuilder sb = new StringBuilder();
            try
            {
                JwtSecurityTokenHandler tokenHandler = new();
                TokenValidationParameters valParam = new();
                var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:SecKey"]));
                valParam.IssuerSigningKey = securityKey;
                valParam.ValidateIssuer = false;
                valParam.ValidateAudience = false;
                //解密
                ClaimsPrincipal claimsPrincipal = tokenHandler.ValidateToken(jwt,
                        valParam, out SecurityToken secToken);
                claimsPrincipal.Claims.ToList().ForEach(r => sb.Append($"{r.Type}={r.Value}"));
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            return sb.ToString();
        }
    }

}
