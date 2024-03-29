using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Core.Model.Models
{
    [SugarTable(tableName: "Person")]
    public class User
    {
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int Id { get; set; }
        public int Age { get; set; }
        public string? Name { get; set; }

    }
}
