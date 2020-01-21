using System;

namespace Lazop.Api
{
    /// <summary>
    /// Lazada Open Platform API Client Interface.
    /// </summary>
    public interface ILazopClient
    {
        /// <summary>
        /// Execute api request without access token.
        /// </summary>
        /// <param name="request">common api requst</param>
        /// <returns>common response</returns>
        LazopResponse Execute(LazopRequest request);

        /// <summary>
        /// Execute api request with access token.
        /// </summary>
        /// <param name="request">common api requst</param>
        /// <param name="session">user access token</param>
        /// <returns>common respons</returns>
        LazopResponse Execute(LazopRequest request, string accessToken);

        /// <summary>
        /// execute api with accessToken and timestamp
        /// </summary>
        /// <param name="request">common api requst</param>
        /// <param name="session">user access token</param>
        /// <param name="timestamp">request timestamp</param>
        /// <returns>common respons</returns>
        LazopResponse Execute(LazopRequest request, string accessToken, DateTime timestamp);
    }
}
