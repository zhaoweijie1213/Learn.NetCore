using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;

namespace Lazop.Api.Util
{
    public abstract class LazopUtils
    {
        private static string intranetIp;

        static LazopUtils()
        {
        }

        /// <summary>
        /// Sign API Request.
        /// </summary>
        /// <param name="parameters">all api params</param>
        /// <param name="appSecret">app secret</param>
        /// <param name="signMethod">sign method : sha256, hmac</param>
        /// <returns>sign</returns>
        public static string SignRequest(string apiName,IDictionary<string, string> parameters, string appSecret, string signMethod)
        {
            return SignRequest(apiName, parameters, null, appSecret, signMethod);
        }

        /// <summary>
        /// Sign API Request with body.
        /// </summary>
        /// <param name="parameters">all api params</param>
        /// <param name="body">body</param>
        /// <param name="appSecret">app secret</param>
        /// <param name="signMethod">sign method : sha256, hmac</param>
        /// <returns>sign</returns>
        public static string SignRequest(string apiName,IDictionary<string, string> parameters, string body, string appSecret, string signMethod)
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
            else
            {
                throw new Exception("Invalid Sign Method");
            }

            // finally : transfer binary byte to hex string
            StringBuilder result = new StringBuilder();
            for (int i = 0; i < bytes.Length; i++)
            {
                result.Append(bytes[i].ToString("X2"));
            }

            return result.ToString();
        }

        /// <summary>
        /// get local ip
        /// </summary>
        public static string GetIntranetIp()
        {
            if (intranetIp == null)
            {
                NetworkInterface[] nis = NetworkInterface.GetAllNetworkInterfaces();
                foreach (NetworkInterface ni in nis)
                {
                    if (OperationalStatus.Up == ni.OperationalStatus && (NetworkInterfaceType.Ethernet == ni.NetworkInterfaceType || NetworkInterfaceType.Wireless80211 == ni.NetworkInterfaceType))
                    {
                        foreach (UnicastIPAddressInformation info in ni.GetIPProperties().UnicastAddresses)
                        {
                            if (AddressFamily.InterNetwork == info.Address.AddressFamily)
                            {
                                intranetIp = info.Address.ToString();
                                break;
                            }
                        }
                        if (intranetIp != null) break;
                    }
                }
            }
            if (intranetIp == null)
            {
                intranetIp = "127.0.0.1";
            }
            return intranetIp;
        }
    }
}
