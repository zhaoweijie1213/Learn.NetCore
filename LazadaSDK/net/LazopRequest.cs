using System;
using System.Collections.Generic;
using Lazop.Api.Util;

namespace Lazop.Api
{
    /// <summary>
    /// Lazada Open Platform basic request.
    /// </summary>
    public class LazopRequest
    {
        /// <summary>
        /// API name
        /// </summary>
        private string apiName;

        /// <summary>
        /// API parameters
        /// </summary>
        private LazopDictionary apiParams;

        /// <summary>
        /// File parameters
        /// </summary>
        private IDictionary<string, FileItem> fileParams;
        /// <summary>
        /// HTTP header parameters
        /// </summary>
        private LazopDictionary headerParams;

        /// <summary>
        /// HTTP method: GET or POST, default is POST
        /// </summary>
        private string httpMethod = Constants.METHOD_POST;

        public LazopRequest()
        {
               
        }

        public LazopRequest(string apiName)
        {
            this.apiName = apiName;
        }

        public void AddApiParameter(string key, string value)
        {
            if (this.apiParams == null)
            {
                this.apiParams = new LazopDictionary();
            }
            this.apiParams.Add(key, value);
        }

        public void AddFileParameter(string key, FileItem file)
        {
            if (this.fileParams == null)
            {
                this.fileParams = new Dictionary<string, FileItem>();
            }
            this.fileParams.Add(key, file);
        }

        public void AddHeaderParameter(string key, string value)
        {
            if (this.headerParams == null)
            {
                this.headerParams = new LazopDictionary();
            }
            this.headerParams.Add(key, value);
        }

        public string GetApiName()
        {
            return this.apiName;
        }

        public void SetApiName(string apiName)
        {
            this.apiName = apiName;
        }

        public string GetHttpMethod()
        {
            return this.httpMethod;
        }

        public void SetHttpMethod(string httpMethod)
        {
            this.httpMethod = httpMethod;
        }

        public IDictionary<string, string> GetParameters()
        {
            if (this.apiParams == null)
            {
                this.apiParams = new LazopDictionary();
            }
            return this.apiParams;
        }

        public IDictionary<string, FileItem> GetFileParameters()
        {
            return this.fileParams;
        }

        public IDictionary<string, string> GetHeaderParameters()
        {
            return this.headerParams;
        }
    }
}
