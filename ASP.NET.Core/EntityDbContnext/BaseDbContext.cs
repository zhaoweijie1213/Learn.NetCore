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
    }
}
