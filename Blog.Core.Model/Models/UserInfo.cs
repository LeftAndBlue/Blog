using System.ComponentModel.DataAnnotations;

namespace Blog.core.Models
{
    public class UserInfo
    {
        /// <summary>
        /// 其中 [Required] 表示非空判断,其他自己研究百度
        /// </summary>
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string PhoneNumber { get; set; }

}
}
