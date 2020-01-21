using Lazop.Api.Util;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Threading;

namespace Lazop.Api
{

    /// <summary>
    ///  IOP default logger 
    ///  DefaultLazopLogger.Instance.LogDirectory=@"./"; default is application rumtime directory
    ///  DefaultLazopLogger.Instance.LogWrite("aa");
    ///  DefaultLazopLogger.Instance.LogWrite("aa", MsgLevel.Debug);
    /// </summary>
    public class LazopLogger : IDisposable, ILazopLogger
    {
        private static LazopLogger _instance = null;
        private static readonly object _synObject = new object();

        public static LazopLogger Instance
        {
            get
            {
                if (null == _instance)
                {
                    lock (_synObject)
                    {
                        if (null == _instance)
                        {
                            _instance = new LazopLogger();
                        }
                    }
                }
                return _instance;
            }
        }
        /// <summary>
        /// log queue
        /// </summary>
        private static Queue<Msg> _msgs;
        private bool _state;
        //private string _logDirectory = AppDomain.CurrentDomain.BaseDirectory;
        private string _logDirectory = System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "/logs/";

        private bool isDebugEnabled = false;
        /// <summary>
        /// log save directory
        /// </summary>
        public string LogDirectory
        {
            get { return _logDirectory; }
            set { _logDirectory = value; }
        }
        private LogFileSplit _logFileSplit = LogFileSplit.Sizely;

        public LogFileSplit logFileSplit
        {
            get { return _logFileSplit; }
            set { _logFileSplit = value; }
        }
        private MsgLevel _currentLogLevel = MsgLevel.Infor;
        /// <summary>
        /// log level
        /// </summary>
        public MsgLevel CurrentMsgType
        {
            get { return _currentLogLevel; }
            set { _currentLogLevel = value; }
        }
        /// <summary>
        /// current log postfix name
        /// </summary>
        private string _currentFileName = "1.log";

        private string _fileNamePrefix = "lazopsdk_";
        /// <summary>
        /// log prefix name
        /// </summary>
        public string FileNamePrefix
        {
            get { return _fileNamePrefix; }
            set { _fileNamePrefix = value; }
        }

        private DateTime _CurrentFileTimeSign = new DateTime();
        private int _maxFileSize = 1024;
        /// <summary>
        /// default log file size.(MB)
        /// </summary>
        public int MaxFileSize
        {
            get { return _maxFileSize; }
            set { _maxFileSize = value; }
        }

        private int _fileSymbol = 0;

        /// <summary>
        /// currnet log size
        /// </summary>
        private long _fileSize = 0;

        /// <summary>
        /// log file write stream
        /// </summary>
        private StreamWriter _writer;

        private LazopLogger()
        {
            if (_msgs == null)
            {
                GetCurrentFilename();
                _state = true;
                _msgs = new Queue<Msg>();

                if(!Directory.Exists(_logDirectory)) {
                    Directory.CreateDirectory(_logDirectory);
                }

                Thread thread = new Thread(work);
                thread.Start();
            }
        }

        private void work()
        {
            while (true)
            {
                if (_msgs.Count > 0)
                {
                    Msg msg = null;
                    lock (_msgs)
                    {
                        msg = _msgs.Dequeue();
                        if (msg != null)
                        {
                            FileWrite(msg);
                        }
                    }
                }
                else
                {
                    if (_state)
                    {
                        Thread.Sleep(1);
                    }
                    else
                    {
                        FileClose();
                    }
                }
            }
        }

        private
            void GetCurrentFilename()
        {
            DateTime now = DateTime.Now;
            string format = "";
            switch (_logFileSplit)
            {
                case LogFileSplit.Daily:
                    _CurrentFileTimeSign = new DateTime(now.Year, now.Month, now.Day);
                    _CurrentFileTimeSign = _CurrentFileTimeSign.AddDays(1);
                    format = now.ToString("yyyyMMdd'.log'");
                    break;
                case LogFileSplit.Weekly:
                    _CurrentFileTimeSign = new DateTime(now.Year, now.Month, now.Day);
                    _CurrentFileTimeSign = _CurrentFileTimeSign.AddDays(7);
                    format = now.ToString("yyyyMMdd'.log'");
                    break;
                case LogFileSplit.Monthly:
                    _CurrentFileTimeSign = new DateTime(now.Year, now.Month, 1);
                    _CurrentFileTimeSign = _CurrentFileTimeSign.AddMonths(1);
                    format = now.ToString("yyyyMM'.log'");
                    break;
                case LogFileSplit.Annually:
                    _CurrentFileTimeSign = new DateTime(now.Year, 1, 1);
                    _CurrentFileTimeSign = _CurrentFileTimeSign.AddYears(1);
                    format = now.ToString("yyyy'.log'");
                    break;
                default:
                    _fileSymbol++;
                    format = _fileSymbol.ToString() + ".log";
                    break;
            }
            if (File.Exists(Path.Combine(LogDirectory, _currentFileName)))
            {
                _fileSize = new FileInfo(Path.Combine(LogDirectory, _currentFileName)).Length;
            }
            else
            {
                _fileSize = 0;
            }
            _currentFileName = _fileNamePrefix + format.Trim();
        }

        //write log to file
        private void FileWrite(Msg msg)
        {
            try
            {
                if (_writer == null)
                {
                    FileOpen();
                }
                if (_writer != null)
                {
                    if ((_logFileSplit != LogFileSplit.Sizely && DateTime.Now >= _CurrentFileTimeSign) ||
                        (_logFileSplit == LogFileSplit.Sizely && ((double)_fileSize / 1048576) > _maxFileSize))
                    {
                        GetCurrentFilename();
                        FileClose();
                        FileOpen();
                    }
                    _writer.Write(msg.datetime);
                    _writer.Write('\t');
                    _writer.Write(msg.type);
                    _writer.Write('\t');
                    _writer.WriteLine(msg.text);
                    _fileSize += System.Text.Encoding.UTF8.GetBytes(msg.ToString()).Length;
                    _writer.Flush();
                }
            }
            catch (Exception e)
            {
                Console.Out.Write(e);
            }
        }

        private void FileOpen()
        {
            _writer = new StreamWriter(LogDirectory + _currentFileName, true, Encoding.UTF8);
        }

        private void FileClose()
        {
            if (_writer != null)
            {
                _writer.Flush();
                _writer.Close();
                _writer.Dispose();
                _writer = null;
            }
        }
        /// <summary>
        /// add log to queue
        /// </summary>
        private void LogWrite(Msg msg)
        {
            if (msg.type < CurrentMsgType)
                return;
            if (_msgs != null)
            {
                lock (_msgs)
                {
                    _msgs.Enqueue(msg);
                }
            }
        }
        /// <summary>
        /// add new log to queue
        /// </summary>
        /// <param name="text">log content</param>
        /// <param name="type">log level</param>
        public void LogWrite(string text, MsgLevel type)
        {
            LogWrite(new Msg(text, type));
        }
        /// <summary>
        /// add new log to queue
        /// </summary>
        /// <param name="text">log conten</param>
        public void LogWrite(string text)
        {
            LogWrite(text, MsgLevel.Debug);
        }
        public void LogWrite(DateTime dt, string text, MsgLevel type)
        {
            LogWrite(new Msg(dt, text, type));
        }

        public void LogWrite(Exception e)
        {
            LogWrite(new Msg(e.Message, MsgLevel.Error));
        }

        public void Dispose()
        {
            _state = false;
        }

        public void TraceApiError(string appKey, String sdkVersion, string apiName, string url, System.Collections.Generic.Dictionary<string, string> parameters, double latency, string errorMessage)
        {
            StringBuilder info = new StringBuilder();
            info.Append(appKey);
            info.Append(Constants.LOG_SPLIT);
            info.Append(sdkVersion);
            info.Append(Constants.LOG_SPLIT);
            info.Append(apiName);
            info.Append(Constants.LOG_SPLIT);
            info.Append(LazopUtils.GetIntranetIp());
            info.Append(Constants.LOG_SPLIT);
            info.Append(System.Environment.OSVersion.VersionString);
            info.Append(Constants.LOG_SPLIT);
            info.Append(latency);
            info.Append(Constants.LOG_SPLIT);
            info.Append(url);
            info.Append(Constants.LOG_SPLIT);
            info.Append(WebUtils.BuildQuery(parameters));
            info.Append(Constants.LOG_SPLIT);
            info.Append(errorMessage);
            this.Error(info.ToString());
        }

        public void Error(string message)
        {
            this.LogWrite(message,MsgLevel.Error);
        }

        public void Error(string format, params object[] args)
        {
            this.LogWrite(string.Format(format, args), MsgLevel.Error);
        }

        public void Warn(string message)
        {
            this.LogWrite(message, MsgLevel.Warn);
        }

        public void Warn(string format, params object[] args)
        {
            this.LogWrite(string.Format(format, args), MsgLevel.Warn);
        }

        public void Info(string message)
        {
            this.LogWrite(message, MsgLevel.Infor);
        }

        public void Info(string format, params object[] args)
        {
            this.LogWrite(string.Format(format, args), MsgLevel.Infor);
        }

        public bool IsDebugEnabled()
        {
            return this.isDebugEnabled;
        }

        public void Debug(string message)
        {
            if (isDebugEnabled)
            {
                this.LogWrite(message, MsgLevel.Debug);
            }
        }

        public void Debug(string format, params object[] args)
        {
            if (isDebugEnabled)
            {
                this.LogWrite(string.Format(format, args), MsgLevel.Debug);
            }
        }
    }

    /// <summary>
    /// log struct
    /// </summary>
    public class Msg
    {
        public Msg()
            : this("", MsgLevel.Debug)
        {
        }

        public Msg(string t, MsgLevel p)
            : this(DateTime.Now, t, p)
        {
        }

        public Msg(DateTime dt, string t, MsgLevel p)
        {
            datetime = dt;
            type = p;
            text = t;
        }

        public DateTime datetime { get; set; }

        public string text { get; set; }

        public MsgLevel type { get; set; }

        public new string ToString()
        {
            return datetime.ToString(CultureInfo.InvariantCulture) + "\t" + text + "\n";
        }
    }

    public enum LogFileSplit
    {
        Daily,
        Weekly,
        Monthly,
        Annually,
        Sizely
    }

    public enum MsgLevel
    {
        Debug = 0,
        Infor,
        Warn,
        Error
    }
}
