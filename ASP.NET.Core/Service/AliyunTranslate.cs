using Aliyun.Acs.Core;
using Aliyun.Acs.Core.Profile;
using Aliyun.Acs.Core.Exceptions;
using Aliyun.Acs.alimt.Model.V20181012;
using System;
using System.Collections.Generic;
using System.Text;
using Aliyun.Acs.Core.Http;

namespace Service
{
    public class AliyunTranslate
    {
        public string HttpAliyunTranslate(string Content)
        {
            string res = "";
            // 构建一个 Client，用于发起请求
            IClientProfile profile = DefaultProfile.GetProfile(
                "cn-hangzhou",
                "LTAI4FjFq4RozfDdzMQACVvY",
                "WBIDojBKJA0SSqVB5sYjx8VHw9KtoJ");
            DefaultAcsClient client = new DefaultAcsClient(profile);
            try
            {
                // 构造请求
                TranslateGeneralRequest request = new TranslateGeneralRequest();
                request.Method = MethodType.POST; //设置请求
                request.FormatType = "text"; //翻译文本的格式  
                request.SourceLanguage = "zh"; //源语言  
                request.SourceText = Content; //原文  
                request.TargetLanguage = "en"; //目标语言  
                                               // 发起请求，并得到 Response
                TranslateGeneralResponse response = client.GetAcsResponse(request);
                res = response.Data.Translated.ToString(); 
            }
            catch (ServerException ex)
            {
                System.Console.WriteLine(ex.ToString());
            }
            return res;
        }
    }
    enum Language : byte
    {
        //en 英语
        en=0,
        //zh 简体中文
        zh=1,
        //zh-tw 繁体中文
        zh_tw=2,
        //es 西班牙语
        //ru 俄语
        //fr 法语
        //pt 葡萄牙语
        //de 德语
        //pt 葡萄牙语
        //pl 波兰语
        //ar 阿拉伯语
        //tr 土耳其语
        //th 泰语
        //vi 越南语
        //id 印尼语
        //ms 马来
        //ja 日语
        //ko 韩语
    }
}
