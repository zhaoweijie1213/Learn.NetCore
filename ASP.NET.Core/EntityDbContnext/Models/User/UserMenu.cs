using System;
using System.Collections.Generic;
using System.Text;

namespace EntityDbContnext.Models.Product.User
{
    public class UserMenu
    {
        public long Id { get; set; }
        /// <summary>
        /// 角色Id
        /// </summary>
        public long RoleId { get; set; }
        /// <summary>
        /// 菜单Id
        /// </summary>
        public long MenuId { get; set; }
    }
}
