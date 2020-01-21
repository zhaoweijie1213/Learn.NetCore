using Lazop.Api;
using Lazop.Api.Util;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace HttpOperation.Lazada.api
{
   
    public class LazadaHelper
    {
        public string accessToken { get; set; }
        public string Request(string url, string appKey= "116053", string appSecret= "ars0XBi2JNSlJWleflrPN2VXAFvpeGBT")
        {
            ILazopClient client = new LazopClient(url, appKey, appSecret);
            LazopRequest request = new LazopRequest();
            request.SetApiName("/order/get");
            request.SetHttpMethod("GET");
            request.AddApiParameter("order_id", "16090");
            LazopResponse response = client.Execute(request, accessToken);
            return response.Code.Trim();
        }
        /// <summary>
        ///  签名获取方法
        /// </summary>
        /// <param name="parameters"></param>
        /// <param name="body"></param>
        /// <param name="appSecret"></param>
        /// <param name="signMethod"></param>
        /// <param name="apiName"></param>
        /// <returns></returns>
        public static string SignRequest(IDictionary<string, string> parameters, string body, string appSecret, string signMethod, string apiName)
        {
            // first : sort all key with asci order
            IDictionary<string, string> sortedParams = new SortedDictionary<string, string>(parameters, StringComparer.Ordinal);

            // second : contact all params with key order
            StringBuilder query = new StringBuilder();
            query.Append(apiName);
            foreach (KeyValuePair<string, string> kv in sortedParams)
            {
                if (!string.IsNullOrEmpty(kv.Key) && !string.IsNullOrEmpty(kv.Value))
                {
                    query.Append(kv.Key).Append(kv.Value);
                }
            }

            // third : add body to last
            if (!string.IsNullOrEmpty(body))
            {
                query.Append(body);
            }

            // next : sign the string
            byte[] bytes = null;
            if (signMethod.Equals(Constants.SIGN_METHOD_SHA256))
            {
                HMACSHA256 sha256 = new HMACSHA256(Encoding.UTF8.GetBytes(appSecret));
                bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(query.ToString()));
            }

            // finally : transfer binary byte to hex string
            StringBuilder result = new StringBuilder();
            for (int i = 0; i < bytes.Length; i++)
            {
                result.Append(bytes[i].ToString("X2"));
            }

            return result.ToString();
        }
    }
}
