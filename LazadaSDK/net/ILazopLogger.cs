using System.Collections.Generic;

namespace Lazop.Api
{
    /// <summary>
    /// Lazada Open Platform DefaultLazopLogger Interface.
    /// </summary>
    public interface ILazopLogger
    {
        bool IsDebugEnabled();

        void TraceApiError(string appKey, string sdkVersion, string apiName, string url, Dictionary<string, string> parameters, double latency, string errorMessage);

        void Error(string message);
        void Error(string format, params object[] args);

        void Warn(string message);
        void Warn(string format, params object[] args);

        void Info(string message);
        void Info(string format, params object[] args);

        void Debug(string message);
        void Debug(string format, params object[] args);
    }
}
