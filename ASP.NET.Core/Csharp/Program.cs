using System;
using PublicMethod.HtmlGrab;
using Service.AliyunTranslate;
using static Service.AliyunTranslate.TranslateHelper;

namespace Csharp
{
    class Program
    {
        static void Main(string[] args)
        {
            do
            {
                //Console.WriteLine("通用版或者电商版？Y/N");
                //string s = Console.ReadLine();
                //if (s == "Y" || s == "y")
                //{
                //    TranslateContent();
                //}
                //else
                //{
                //    ECommerceTranslate();
                //}
                Console.WriteLine("-----手机号码-----");
                var phone = Console.ReadLine();
                var status = RegexHelper.IsPhoneNum(phone);
                Console.WriteLine($"-----{status}-----");


                Console.WriteLine("-----邮箱号-----");
                var email = Console.ReadLine();
                status = RegexHelper.IsEmail(email);
                Console.WriteLine($"-----{status}-----");
            } while (true);

        }
        public static void TranslateContent()
        {
            //TranslateHelper aliyunTranslate = new TranslateHelper();
            //Console.WriteLine("-----输入要翻译的文本------");
            //var s = Console.ReadLine();
            //Console.WriteLine("-----输入源语言------");
            //var SourceLanguage = Console.ReadLine();
            //Console.WriteLine("-----输入目标语言------");
            //var TargetLanguage = Console.ReadLine().ToString();
            //var res = aliyunTranslate.HttpAliyunTranslate(s, (Language)Enum.Parse(typeof(Language), SourceLanguage), (Language)Enum.Parse(typeof(Language), TargetLanguage));
            //Console.WriteLine(res);
        }
        public static void ECommerceTranslate()
        {
            TranslateHelper aliyunTranslate = new TranslateHelper();
            Console.WriteLine("-----输入要翻译的文本------");
            var content = Console.ReadLine();
            Console.WriteLine("-----输入源语言------");
            var SourceLanguage = Console.ReadLine();
            Console.WriteLine("-----输入目标语言------");
            var TargetLanguage = Console.ReadLine().ToString();        
            Console.WriteLine("-----输入场景------\n-----商品标题:title，商品描述:description，商品沟通:communication-----");
            var scene = Console.ReadLine().ToString();
            Console.WriteLine("text or html ?");
            string FormatType = Console.ReadLine();
            var res = aliyunTranslate.HttpAliyunECommerceTranslate(content, (Language)Enum.Parse(typeof(Language), SourceLanguage), (Language)Enum.Parse(typeof(Language), TargetLanguage), (Scene)Enum.Parse(typeof(Scene), scene), FormatType);
            Console.WriteLine(res);
        }
    }
}
