using Aliyun.Acs.Core;
using Aliyun.Acs.Core.Profile;
using Aliyun.Acs.Core.Exceptions;
using Aliyun.Acs.alimt.Model.V20181012;
using System;
using System.Collections.Generic;
using System.Text;
using Aliyun.Acs.Core.Http;
using System.ComponentModel;

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
        [Description("西班牙语")]
        //es 西班牙语
        es =3,
        [Description("俄语")]
        ru=4,
        //fr 法语
        [Description("法语")]
        fr =5,
        //pt 葡萄牙语
        [Description("葡萄牙语")]
        pt = 6,
        //de 德语
        [Description("德语")]
        de =7,
        //pt 葡萄牙语
        //pt=8,
        //pl 波兰语
        [Description("波兰语")]
        pl =9,
        //ar 阿拉伯语
        [Description("阿拉伯语")]
        ar =10,
        //tr 土耳其语
        [Description("土耳其语")]
        tr =11,
        //th 泰语
        [Description("泰语")]
        th =12,
        //vi 越南语
        [Description("越南语")]
        vi =13,
        //id 印尼语
        [Description("印尼语")]
        id =14,
        //ms 日语
        [Description("日语")]
        ms =15,
        //ja 日语
        [Description("日语")]
        ja =16,
        //ko 韩语
        [Description("韩语")]
        ko =17
    }
}
