using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Service.AliyunTranslate
{
    public partial class TranslateHelper
    {
       
        public enum Language : byte
        {
            //en 英语
            [Description("英语")]
            en = 0,
            //zh 简体中文
            [Description("简体中文")]
            zh = 1,
            [Description("繁体中文")]
            //zh-tw 繁体中文
            zh_tw = 2,
            [Description("西班牙语")]
            //es 西班牙语
            es = 3,
            [Description("俄语")]
            ru = 4,
            //fr 法语
            [Description("法语")]
            fr = 5,
            //pt 葡萄牙语
            [Description("葡萄牙语")]
            pt = 6,
            //de 德语
            [Description("德语")]
            de = 7,
            ////pt 葡萄牙语
            //pt=8,
            //pl 波兰语
            [Description("波兰语")]
            pl = 8,
            //ar 阿拉伯语
            [Description("阿拉伯语")]
            ar = 9,
            //tr 土耳其语
            [Description("土耳其语")]
            土耳其语 = 10,
            //th 泰语
            [Description("泰语")]
            th = 11,
            //vi 越南语
            [Description("越南语")]
            vi = 12,
            //id 印尼语
            [Description("印尼语")]
            id = 13,
            //ms 日语
            [Description("马来语")]
            ms = 14,
            //ja 日语
            [Description("日语")]
            ja = 15,
            //ko 韩语
            [Description("韩语")]
            ko = 16
        }
        public enum Scene : byte
        {
            //商品标题:title，商品描述:description，买卖家沟通:communication
            [Description("商品标题")]
            title = 0,
            [Description("商品描述")]
            description = 1,
            [Description("商品沟通")]
            communication = 2,
            [Description("通用")]
            general = 3,
            [Description("医疗")]
            medical = 4
        }
        public enum ResponseCode : int
        {
            [Description("请求超时,可重试!")]
            RequestTimeout = 10001,
            [Description("系统错误，可重试!")]
            SystemError = 10002,
            [Description("译文URL decode失败，确认是否是UTF-8编码，并且URL encode正确!")]
            TranslationURLDecodeFailed = 10003,
            [Description("参数缺失!")]
            ParameterIsMissing = 1004,
            [Description("译文翻译语言方向不支持,检查译文是否在支持的语言对里面!")]
            LanguageDoesNotSupport = 10005,
            [Description("语种识别失败,确认传入的待识别文本是否正确!")]
            LanguageIdentificationFailure = 10006,
            [Description("翻译失败,确认是否是正确的文字!")]
            TranslationFailures = 10007,
            [Description("字符长度过长,确认翻译原文字符长度，可以分多次调用，长度限制5000字符!")]
            CharacterLengthIsTooLong = 10008,
            [Description("子账号没有权限,请让主账号给子账号授权!")]
            NoPermissions = 10009,
            [Description("账号没有开通服务,请开通机器翻译产品后在使用!")]
            No_AccessService = 10010,
            [Description("子账号服务失败,联系客服解决!")]
            ServiceFailure = 10011,
            [Description("翻译服务调用失败,联系客服解决!")]
            CallFailed = 10012,
            [Description("账号服务没有开通或者欠费,请开通服务或者交清欠费!")]
            NotConnectedOrArrears = 10013,
            [Description("未知错误,联系客服解决!")]
            UnknownError = 19999,
        }
    }
}
