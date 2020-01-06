using System;
using System.Collections.Generic;
using System.Text;

namespace Migrations.Models
{
    public class Member
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public DateTime CreateTime { get; set; }
    }
}
