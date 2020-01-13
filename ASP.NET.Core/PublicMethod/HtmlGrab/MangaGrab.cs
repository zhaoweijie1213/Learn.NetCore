using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace PublicMethod.HtmlGrab
{
    public class MangaGrab
    {
        /// <summary>
        /// 获取内容
        /// </summary>
        /// <param name="Url"></param>
        /// <returns></returns>
        public async Task<string> GetContent(string Url)
        {
            HtmlWeb web = new HtmlWeb();
            await web.LoadFromWebAsync(Url);
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(Url);
            return doc.DocumentNode.InnerHtml;
        }   
    }
}
