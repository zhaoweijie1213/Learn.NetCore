using System;
using System.Collections.Generic;
using System.Text;

namespace EntityDbContnext.Models.Product
{
    public class Ali1688ProductLink
    {
        public long Id { get; set; }
        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }

        //public ICollection  Ali1688ProductLinkImg { get; set; }
        public virtual ICollection<Ali1688ProductLinkImg> Ali1688ProductLinkImg { get; set; }
        /// <summary>
        /// 属性
        /// </summary>
        public string Type { get; set; }
        /// <summary>
        /// 采集链接
        /// </summary>
        public string Url { get; set; }
        /// <summary>
        /// 价格
        /// </summary>
        public decimal Price { get; set; }
        /// <summary>
        /// 销量
        /// </summary>
        public long Sales { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTimeOffset CreateTime { get; set; }
    }
}
