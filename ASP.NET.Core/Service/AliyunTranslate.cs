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
        public void HttpAliyunTranslate()
        {
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
                request.SourceText = "你好"; //原文  
                request.TargetLanguage = "en"; //目标语言  
                                               // 发起请求，并得到 Response
                TranslateGeneralResponse response = client.GetAcsResponse(request);
                System.Console.WriteLine(response.Data);
            }
            catch (ServerException ex)
            {
                System.Console.WriteLine(ex.ToString());
            }
            catch (ClientException ex)
            {
                System.Console.WriteLine(ex.ToString());
            }
        }
    }
}
