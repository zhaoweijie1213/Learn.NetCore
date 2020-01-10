using System;
using Service;

namespace Csharp
{
    class Program
    {
        static void Main(string[] args)
        {
            AliyunTranslate aliyunTranslate = new AliyunTranslate();
            var s= Console.ReadLine();
            var res=aliyunTranslate.HttpAliyunTranslate(s);
            Console.WriteLine(res);
            Console.ReadKey();
        }
    }
}
