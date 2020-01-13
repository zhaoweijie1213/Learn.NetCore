using Aliyun.Acs.alimt.Model.V20181012;
using Aliyun.Acs.Core;
using Aliyun.Acs.Core.Exceptions;
using Aliyun.Acs.Core.Http;
using Aliyun.Acs.Core.Profile;
using PublicMethod;
using System;
using System.Text;
using System.Web;

namespace Service.AliyunTranslate
{
    public partial class TranslateHelper
    {

        public string HttpAliyunTranslate(string Content, Language SourceLanguage, Language TargetLanguage, Scene scene, string FormatType = "text")
        {
            string res;
            // 构建一个 Client，用于发起请求
            IClientProfile profile = DefaultProfile.GetProfile(
                "cn-hangzhou",
                "LTAI4FjFq4RozfDdzMQACVvY",
                "WBIDojBKJA0SSqVB5sYjx8VHw9KtoJ");
            var targetLanguage = TargetLanguage.ToString();
            if ((int)TargetLanguage == 2)
            {
                targetLanguage = "zh-tw";
            }
            DefaultAcsClient client = new DefaultAcsClient(profile);
            try
            {
                // 构造请求
                TranslateGeneralRequest request = new TranslateGeneralRequest
                {
                    Method = MethodType.POST, //设置请求
                    FormatType = FormatType, //翻译文本的格式  
                    Scene = scene.ToString(),
                    SourceLanguage = SourceLanguage.ToString(), //源语言  
                    SourceText = Content, //原文  
                    TargetLanguage = targetLanguage //目标语言  
                };
                // 发起请求，并得到 Response
                TranslateGeneralResponse response = client.GetAcsResponse(request);
                if (response.Code == 200)
                {
                    res = response.Data.Translated.ToString();
                }
                else
                {
                    res = ((ResponseCode)response.Code).ToDescription();
                }
            }
            catch (ServerException ex)
            {
                res=ex.ToString();
            }
            return res;
        }
        /// <summary>
        /// 电商版本
        /// </summary>
        /// <returns></returns>
        public string HttpAliyunECommerceTranslate(string Content, Language SourceLanguage, Language TargetLanguage, Scene scene,string FormatType ="text")
        {
            string res;
            // 构建一个 Client，用于发起请求
            IClientProfile profile = DefaultProfile.GetProfile(
                "cn-hangzhou",
                "LTAI4FjFq4RozfDdzMQACVvY",
                "WBIDojBKJA0SSqVB5sYjx8VHw9KtoJ");
            var targetLanguage = TargetLanguage.ToString();
            if ((int)TargetLanguage == 2)
            {
                targetLanguage = "zh-tw";
            }
            DefaultAcsClient client = new DefaultAcsClient(profile);
            try
            {
                TranslateECommerceRequest eCommerceRequest = new TranslateECommerceRequest
                {
                    Scene = scene.ToString(),
                    Method = MethodType.POST,// 设置请求方式，POST
                    FormatType = FormatType,  //翻译文本的格式
                    SourceLanguage = SourceLanguage.ToString(),  //源语言
                    SourceText = HttpUtility.UrlEncode(Content, Encoding.GetEncoding("utf-8")), //原文
                    TargetLanguage = targetLanguage  //目标语言
                };
                TranslateECommerceResponse response = client.GetAcsResponse(eCommerceRequest);
                if (response.Code==200)
                {
                    res = response.Data.Translated.ToString();
                }
                else
                {
                    res = ((ResponseCode)response.Code).ToDescription();
                }
            }
            catch (Exception ex)
            {

                res=ex.Message.ToString();
            }
            return res;
        }
    }
}
