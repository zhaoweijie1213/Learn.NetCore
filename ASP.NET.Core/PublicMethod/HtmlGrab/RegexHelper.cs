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
    }
}
