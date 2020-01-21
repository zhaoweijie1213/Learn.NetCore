using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace PublicMethod.HtmlGrab
{
    public class RegexHelper
    {
        /// <summary>
        /// 验证手机
        /// </summary>
        /// <param name="phone"></param>
        /// <returns></returns>
        public static bool IsPhoneNum(string phone)
        {
            bool status = Regex.IsMatch(phone, @"^[1-8][3-8]\d{9}$");
            return status;
        }
        /// <summary>
        /// 验证邮箱
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public static bool IsEmail(string email)
        {
            bool status = Regex.IsMatch(email, @"^[\w\d]{5,16}\@[\w\d]{2,6}\.com$");
            return status;
        }
        /// <summary>
        /// 身份证验证
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static bool IsIDCard(string IDcard)
        {
            bool status = Regex.IsMatch(IDcard, @"^[\w\d]{5,16}\@[\w\d]{2,6}\.com$");
            return status;
        }
        /// <summary>
        /// 身份证上数字对应地址
        /// </summary>
        enum IDAddress
        {
            北京 = 11, 天津 = 12, 河北 = 13, 山西 = 14, 内蒙古 = 15, 辽宁 = 21, 吉林 = 22, 黑龙江 = 23, 上海 = 31, 江苏 = 32, 浙江 = 33,
            安徽 = 34, 福建 = 35, 江西 = 36, 山东 = 37, 河南 = 41, 湖北 = 42, 湖南 = 43, 广东 = 44, 广西 = 45, 海南 = 46, 重庆 = 50, 四川 = 51,
            贵州 = 52, 云南 = 53, 西藏 = 54, 陕西 = 61, 甘肃 = 62, 青海 = 63, 宁夏 = 64, 新疆 = 65, 台湾 = 71, 香港 = 81, 澳门 = 82, 国外 = 91
        }
    }
}
