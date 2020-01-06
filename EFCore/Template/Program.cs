using System;
using Web.Service.Models;

namespace Template
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            int i = 1; 
            do 
            { 
                Console.WriteLine("{0}", i++); 
            }
            while (i <= 10);
            rote sw;
            sw.distance = 2.5;
            sw.direction = orientation.Nornl;
            //
            int[] intArry;
            intArry = new int[10];
            string MyString = "A string";
            char[] vs = MyString.ToCharArray();
            string[] ys = MyString.Split();
            foreach (char item in vs)
            {
                Console.WriteLine(item);
            }
            Console.WriteLine("---------------特殊参数-----------");
            int sum=Sums(1,5,2,9,8);
            Console.WriteLine($"{sum}");
            Console.WriteLine("----------------------------------");
        }
        /// <summary>
        /// 特殊参数params
        /// </summary>
        /// <param name="vals"></param>
        /// <returns></returns>
        static int Sums(params int[] vals)
        {
            int sum = 0;
            foreach (var item in vals)
            {
                sum += item;
            }
            return sum;
        }
        
    }
    public class Envent
    { 
        
    }
}
