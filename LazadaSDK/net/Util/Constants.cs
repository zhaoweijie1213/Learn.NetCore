
namespace Lazop.Api.Util
{
    public sealed class Constants
    {
        public const string CHARSET_UTF8 = "utf-8";

        public const string DATE_TIME_FORMAT = "yyyy-MM-dd HH:mm:ss";
        public const string DATE_TIME_MS_FORMAT = "yyyy-MM-dd HH:mm:ss.fff";

        public const string SIGN_METHOD_HMAC = "hmac";
        public const string SIGN_METHOD_SHA256 = "sha256";

        public const string PARTNER_ID = "partner_id";

        public const string LOG_SPLIT = "^_^";
        public const string LOG_FILE_NAME = "lazopsdk.log";

        public const string ACCEPT_ENCODING = "Accept-Encoding";
        public const string CONTENT_ENCODING = "Content-Encoding";
        public const string CONTENT_ENCODING_GZIP = "gzip";

        public const string SDK_VERSION = "lazop-sdk-net";

        public const string APP_KEY = "app_key";
        public const string FORMAT = "format";
        public const string TIMESTAMP = "timestamp";
        public const string SIGN = "sign";
        public const string SIGN_METHOD = "sign_method";
        public const string ACCESS_TOKEN = "access_token";
        public const string FORMAT_XML = "xml";
        public const string FORMAT_JSON = "json";
        public const string DEBUG = "debug";

        public const string METHOD_POST = "POST";
        public const string METHOD_GET = "GET";

        public const string RSP_TYPE = "type";
        public const string RSP_CODE = "code";
        public const string RSP_MSG = "message";
        public const string RSP_REQUEST_ID = "request_id";

        public const string CTYPE_DEFAULT = "application/octet-stream";
        public const int READ_BUFFER_SIZE = 1024 * 4;

        public const string LOG_LEVEL_DEBUG = "DEBUG";
        public const string LOG_LEVEL_INFO = "INFO";
        public const string LOG_LEVEL_ERROR = "ERROR";
    }
}
