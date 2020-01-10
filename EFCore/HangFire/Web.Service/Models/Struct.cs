using System;
using System.Collections.Generic;
using System.Text;

namespace Web.Service.Models
{
    /// <summary>
    /// 结构
    /// </summary>
    public struct rote
    {
        public orientation direction;
        public double distance;

    }

    public enum orientation : byte
    { 
        Nornl=1,
    }
}
