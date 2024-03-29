using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Core.Model
{
    internal class MessageModel<T>
    {
        /// <summary>
        /// 操作是否成功
        /// </summary>
        public bool Result { get; set; }
        /// <summary>
        /// 返回信息
        /// </summary>
        public string Msg { get; set; }
        /// <summary>
        /// 返回数据集合
        /// </summary>
        public List<T> Data { get; set; }
    }
}
