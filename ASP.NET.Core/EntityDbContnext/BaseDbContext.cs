using EntityDbContnext.Models;
using EntityDbContnext.Models.Product;
using EntityDbContnext.Models.Product.User;
using EntityDbContnext.Models.User;
using Microsoft.EntityFrameworkCore;
using System;

namespace EntityDbContnext
{
    /// <summary>
    /// 数据库实体
    /// </summary>
    public class BaseDbContext : DbContext
    {
        public BaseDbContext(DbContextOptions<BaseDbContext> options)
            :base(options)
        { 
            
        }
        public DbSet<AliyunTranslateLanguage> AliyunTranslateLanguage { get; set; }
        public DbSet<Ali1688ProductLink> Ali1688ProductLink { get; set; }
        public DbSet<Ali1688ProductLinkImg> Ali1688ProductLinkImg { get; set; }
        public DbSet<UserInfo> UserInfo { get; set; }
        public DbSet<UserMenu> UserMenu { get; set; }
        public DbSet<UserRole> UserRole { get; set; }
    }
}
