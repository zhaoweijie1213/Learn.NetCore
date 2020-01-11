using System;
using System.Collections.Generic;
using System.Text;

namespace HttpOperation.QueryParams
{
    public class AliyunTranslateParams
    {
        /// <summary>
        /// 引擎
        /// </summary>
        public int TranslationEngine { get; set; }
        /// <summary>
        /// 需要翻译内容
        /// </summary>
        public string Content { get; set; }
        /// <summary>
        /// 源语言
        /// </summary>
        public int SourceLanguage { get; set; }
        /// <summary>
        /// 目标语言
        /// </summary>
        public int TargetLanguage { get; set; }
        /// <summary>
        /// 场景
        /// </summary>
        public int Scene { get; set; }
        /// <summary>
        /// 类型，text或者html
        /// </summary>
        public string FormatType { get; set; }
    }
}
