using System.IO;

namespace Lazop.Api.Util
{
    /// <summary>
    /// File Encapsulator.
    /// File Path：new FileItem("C:/temp.jpg");
    /// File Object：new FileItem(new FileInfo("C:/temp.jpg"));
    /// File Bytes：new FileItem("abc.jpg", bytes);
    /// File Stream：new FileItem("abc.jpg", stream);
    /// </summary>
    public class FileItem
    {
        private Contract contract;

        /// <summary>
        /// local file object constructor
        /// </summary>
        /// <param name="fileInfo">local file object</param>
        public FileItem(FileInfo fileInfo)
        {
            this.contract = new LocalContract(fileInfo);
        }

        /// <summary>
        /// local file path constructor
        /// </summary>
        /// <param name="filePath"> local file path</param>
        public FileItem(string filePath) : this(new FileInfo(filePath))
        {
        }

        /// <summary>
        /// memory byte array constructor
        /// </summary>
        /// <param name="fileName">file name</param>
        /// <param name="content">file content</param>
        public FileItem(string fileName, byte[] content) : this(fileName, content, null)
        {
        }

        /// <summary>
        /// memory byte array and file type constructor
        /// </summary>
        /// <param name="fileName">file name</param>
        /// <param name="content">file content</param>
        /// <param name="mimeType">file type</param>
        public FileItem(string fileName, byte[] content, string mimeType)
        {
            this.contract = new ByteArrayContract(fileName, content, mimeType);
        }

        /// <summary>
        /// file stream constructor
        /// </summary>
        /// <param name="fileName">file name</param>
        /// <param name="content">file input stream</param>
        public FileItem(string fileName, Stream stream) : this(fileName, stream, null)
        {
        }

        /// <summary>
        /// file stream and file type constructor
        /// </summary>
        /// <param name="fileName">file name</param>
        /// <param name="content">file input stream</param>
        /// <param name="mimeType">file type</param>
        public FileItem(string fileName, Stream stream, string mimeType)
        {
            this.contract = new StreamContract(fileName, stream, mimeType);
        }

        public bool IsValid()
        {
            return this.contract.IsValid();
        }

        public long GetFileLength()
        {
            return this.contract.GetFileLength();
        }

        public string GetFileName()
        {
            return this.contract.GetFileName();
        }

        public string GetMimeType()
        {
            return this.contract.GetMimeType();
        }

        public void Write(Stream output)
        {
            this.contract.Write(output);
        }
    }

    internal interface Contract
    {
        bool IsValid();
        string GetFileName();
        string GetMimeType();
        long GetFileLength();
        void Write(Stream output);
    }

    internal class LocalContract : Contract
    {
        private FileInfo fileInfo;

        public LocalContract(FileInfo fileInfo)
        {
            this.fileInfo = fileInfo;
        }

        public long GetFileLength()
        {
            return this.fileInfo.Length;
        }

        public string GetFileName()
        {
            return this.fileInfo.Name;
        }

        public string GetMimeType()
        {
            return Constants.CTYPE_DEFAULT;
        }

        public bool IsValid()
        {
            return this.fileInfo != null && this.fileInfo.Exists;
        }

        public void Write(Stream output)
        {
            using (BufferedStream bfs = new BufferedStream(this.fileInfo.OpenRead()))
            {
                int n = 0;
                byte[] buffer = new byte[Constants.READ_BUFFER_SIZE];
                while ((n = bfs.Read(buffer, 0, buffer.Length)) > 0)
                {
                    output.Write(buffer, 0, n);
                }
            }
        }
    }

    internal class ByteArrayContract : Contract
    {
        private string fileName;
        private byte[] content;
        private string mimeType;

        public ByteArrayContract(string fileName, byte[] content, string mimeType)
        {
            this.fileName = fileName;
            this.content = content;
            this.mimeType = mimeType;
        }

        public bool IsValid()
        {
            return this.content != null && this.fileName != null;
        }

        public long GetFileLength()
        {
            return this.content.Length;
        }

        public string GetFileName()
        {
            return this.fileName;
        }

        public string GetMimeType()
        {
            if (string.IsNullOrEmpty(this.mimeType))
            {
                return Constants.CTYPE_DEFAULT;
            }
            else
            {
                return this.mimeType;
            }
        }

        public void Write(Stream output)
        {
            output.Write(this.content, 0, this.content.Length);
        }
    }

    internal class StreamContract : Contract
    {
        private string fileName;
        private Stream stream;
        private string mimeType;

        public StreamContract(string fileName, Stream stream, string mimeType)
        {
            this.fileName = fileName;
            this.stream = stream;
            this.mimeType = mimeType;
        }

        public long GetFileLength()
        {
            return 0L;
        }

        public string GetFileName()
        {
            return this.fileName;
        }

        public string GetMimeType()
        {
            if (string.IsNullOrEmpty(mimeType))
            {
                return Constants.CTYPE_DEFAULT;
            }
            else
            {
                return this.mimeType;
            }
        }

        public bool IsValid()
        {
            return this.stream != null && this.fileName != null;
        }

        public void Write(Stream output)
        {
            using (this.stream)
            {
                int n = 0;
                byte[] buffer = new byte[Constants.READ_BUFFER_SIZE];
                while ((n = this.stream.Read(buffer, 0, buffer.Length)) > 0)
                {
                    output.Write(buffer, 0, n);
                }
            }
        }
    }
}
