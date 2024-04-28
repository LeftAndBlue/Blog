using SqlSugar;
namespace Blog.Core.Model.Models
{
    /// <summary>
    /// 用户信息
    /// </summary>
    [SugarTable(tableName: "Person")]
    public class User
    {
        [SugarColumn(IsPrimaryKey = true)]
        public string ID { get; set; }
        /// <summary>
        /// 年龄
        /// </summary>
        public int AGE { get; set; }
        /// <summary>
        /// 姓名
        /// </summary>
        public string? NAME { get; set; }
        /// <summary>
        /// 邮箱
        /// </summary>
        public string EMAIL { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        public string PASSWORD { get; set; }
        /// <summary>
        /// 性别
        /// </summary>
        public enum GENDER;
        /// <summary>
        /// 注册时间
        /// </summary>
        public DateTime REGISTRATIONTIME {  get; set; }
        /// <summary>
        /// 上次登陆时间
        /// </summary>
        public DateTime LASTLOGINTIME {  get; set; }

    }
}
