using System;

namespace Lazop.Api
{
    [Serializable]
    public class LazopResponse
    {
        /// <summary>
        /// error type for error response.
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// error code for error response, zero means successful response.
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// error message for error response.
        /// </summary>
        public string Message { get; set; }


        /// <summary>
        /// request id for api request.
        /// </summary>
        public string RequestId { get; set; }

        /// <summary>
        /// api response
        /// </summary>
        public string Body { get; set; }

        public bool IsError()
        {
            return Code != null && !Code.Equals("0");
        }
    }
}
