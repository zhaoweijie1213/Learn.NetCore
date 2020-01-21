using Lazop.Api.Util;
using System;
using System.Collections.Generic;

namespace Lazop.Api
{
    /// <summary>
    /// Lazada Open Platform client implemetation.
    /// </summary>
    public class LazopClient : ILazopClient
    {
        internal string serverUrl;
        internal string appKey;
        internal string appSecret;
        internal string signMethod = Constants.SIGN_METHOD_SHA256;
        internal string sdkVersion = "lazop-sdk-net-20180508";
        internal string logLevel = Constants.LOG_LEVEL_ERROR;

        internal DateTime dt1970 = new DateTime(1970, 1, 1, 0, 0, 0, 0);

        internal WebUtils webUtils;
        internal ILazopLogger topLogger;
        internal bool disableTrace = false; // disable log api error
        internal IDictionary<string, string> customrParameters; // set client custom params

        #region LazopClient Constructors

        public LazopClient(string serverUrl, string appKey, string appSecret)
        {
            this.appKey = appKey;
            this.appSecret = appSecret;
            this.serverUrl = serverUrl;
            this.webUtils = new WebUtils();
            this.topLogger = Lazop.Api.LazopLogger.Instance;
        }

        #endregion

        public void SetTimeout(int timeout)
        {
            this.webUtils.Timeout = timeout;
        }

        public void SetSignMethod(string signMethod)
        {
            if (signMethod.Equals(Constants.SIGN_METHOD_HMAC) || signMethod.Equals(Constants.SIGN_METHOD_SHA256))
            {
                this.signMethod = signMethod;
            }
        }

        public void SetReadWriteTimeout(int readWriteTimeout)
        {
            this.webUtils.ReadWriteTimeout = readWriteTimeout;
        }

        public void SetDisableTrace(bool disableTrace)
        {
            this.disableTrace = disableTrace;
        }

        public void SetIgnoreSSLCheck(bool ignore)
        {
            this.webUtils.IgnoreSSLCheck = ignore;
        }

        /// <summary>
        /// disable local proxy
        /// </summary>
        public void SetDisableWebProxy(bool disable)
        {
            this.webUtils.DisableWebProxy = disable;
        }

        public void SetMaxConnectionLimit(int limit)
        {
            System.Net.ServicePointManager.DefaultConnectionLimit = limit;
        }

        public void SetCustomParameters(IDictionary<string, string> customParameters)
        {
            this.customrParameters = customParameters;
        }

        #region ILazopClient Members

        public virtual LazopResponse Execute(LazopRequest request)
        {
            return DoExecute(request, null, DateTime.UtcNow);
        }

        public virtual LazopResponse Execute(LazopRequest request, string accessToken)
        {
            return DoExecute(request, accessToken, DateTime.UtcNow);
        }

        public virtual LazopResponse Execute(LazopRequest request, string accessToken, DateTime timestamp)
        {
            return DoExecute(request, accessToken, timestamp);
        }

        #endregion

        private LazopResponse DoExecute(LazopRequest request, string accessToken, DateTime timestamp)
        {

            long start = DateTime.Now.Ticks;

            // add common params
            LazopDictionary txtParams = new LazopDictionary(request.GetParameters());

            txtParams.Add(Constants.APP_KEY, appKey);
            txtParams.Add(Constants.TIMESTAMP, GetTimestamp(timestamp));
            txtParams.Add(Constants.ACCESS_TOKEN, accessToken);
            txtParams.Add(Constants.PARTNER_ID, sdkVersion);
            txtParams.AddAll(this.customrParameters);
            txtParams.Add(Constants.SIGN_METHOD, this.signMethod);
            if(IsDebugEnabled()) {
                txtParams.Add(Constants.DEBUG, true);
            }

            // compute and add sign
            txtParams.Add(Constants.SIGN, LazopUtils.SignRequest(request.GetApiName(), txtParams, appSecret, this.signMethod));

            string realServerUrl = GetServerUrl(this.serverUrl, request.GetApiName(), accessToken);
            string reqUrl = WebUtils.BuildRequestUrl(realServerUrl, txtParams);

            try
            {
                string body;
                if (request.GetFileParameters() != null) // if file params is set
                {
                    body = webUtils.DoPost(realServerUrl, txtParams, request.GetFileParameters(), request.GetHeaderParameters());
                }
                else
                {
                    if (request.GetHttpMethod().Equals(Constants.METHOD_POST))
                    {
                        body = webUtils.DoPost(realServerUrl, txtParams, request.GetHeaderParameters());
                    }
                    else
                    {
                        body = webUtils.DoGet(realServerUrl, txtParams, request.GetHeaderParameters());
                    }
                }

                LazopResponse response = ParseResponse(body);

                // log error response
                if (response.IsError())
                {
                    TimeSpan latency = new TimeSpan(DateTime.Now.Ticks - start);
                    LogApiError(appKey, sdkVersion, request.GetApiName(), serverUrl, txtParams, latency.TotalMilliseconds, response.Body);
                } else
                {
                    if (IsDebugEnabled() || IsInfoEnabled())
                    {
                        TimeSpan latency = new TimeSpan(DateTime.Now.Ticks - start);
                        LogApiError(appKey, sdkVersion, request.GetApiName(), serverUrl, txtParams, latency.TotalMilliseconds, response.Body);
                    }
                }

                return response;
            }
            catch (Exception e)
            {
                TimeSpan latency = new TimeSpan(DateTime.Now.Ticks - start);
                LogApiError(appKey, sdkVersion, request.GetApiName(), serverUrl, txtParams, latency.TotalMilliseconds, e.GetType() + ": " + e.Message);
                throw e;
            }
        }

        private LazopResponse ParseResponse(string jsonRsp)
        {
            IDictionary<string, object> root =  FastJSON.JSON.Parse(jsonRsp) as IDictionary<string, object>;
            LazopResponse lazopRsp = new LazopResponse();
            lazopRsp.Type = GetStringValue(root,Constants.RSP_TYPE);
            lazopRsp.Code = GetStringValue(root, Constants.RSP_CODE);
            lazopRsp.Message = GetStringValue(root, Constants.RSP_MSG);
            lazopRsp.RequestId = GetStringValue(root, Constants.RSP_REQUEST_ID);
            lazopRsp.Body = jsonRsp;
            return lazopRsp;
        }

        private String GetStringValue(IDictionary<string, object> raw, String key)
        {
            if (raw.ContainsKey(key))
            {
                object value = raw[key];
                if (value != null)
                {
                    return (String)value;
                }
            }
            return null;
        }

        private long GetTimestamp(DateTime dateTime)
        {
            return (dateTime.Ticks - dt1970.Ticks) / 10000;
        }

        private bool IsDebugEnabled() {
            return logLevel == Constants.LOG_LEVEL_DEBUG;
        }

        private bool IsInfoEnabled() {
            return logLevel == Constants.LOG_LEVEL_INFO;
        }

        private bool IsErrorEnabled() {
            return logLevel == Constants.LOG_LEVEL_ERROR;
        }

        public void SetLogLevel(String logLevel)
        {
            this.logLevel = logLevel;
        }

        internal virtual string GetServerUrl(string serverUrl, string apiName, string session)
        {
            if (apiName == null || apiName.Length == 0)
            {
                return serverUrl;
            }
            bool hasPrepend = serverUrl.EndsWith("/");
            if (hasPrepend)
            {
                return serverUrl + apiName.Substring(1);
            }
            else
            {
                return serverUrl + apiName;
            }
        }

        internal void LogApiError(string appKey, String sdkVersion, string apiName, string url, Dictionary<string, string> parameters, double latency, string errorMessage)
        {
            if (!disableTrace)
            {
                this.topLogger.TraceApiError(appKey, sdkVersion, apiName, url, parameters, latency, errorMessage);
            }
        }
    }
}
