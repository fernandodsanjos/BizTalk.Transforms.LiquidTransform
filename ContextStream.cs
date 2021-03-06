using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace BizTalk.Transforms.LiquidTransform
{
    //BizTalk adds the Stream to  Microsoft.BizTalk.Message.Interop.StreamViewOfIStream stream so the values becomes inaccessible
    public class ContextStream : Stream
    {
        Stream internalStream = null;

        public Dictionary<string, string> ContextValues { get; } = new Dictionary<string, string>();
        public ContextStream(Stream stream)
        {
            internalStream = stream;
        }
        public override bool CanRead
        {
            get
            {
                return internalStream.CanRead;
            }
        }

        public override bool CanSeek
        {
            get
            {
                return internalStream.CanSeek;
            }
        }

        public override bool CanWrite
        {
            get
            {
                return internalStream.CanWrite;
            }
        }

        public override long Length
        {
            get
            {
                return internalStream.Length;
            }
        }

        public override long Position
        {
            get
            {
                return internalStream.Position;
            }

            set
            {
                internalStream.Position = value;
            }
        }

        public override void Flush()
        {
            internalStream.Flush();
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            return internalStream.Read(buffer, offset, count);
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            return internalStream.Seek(offset, origin);
        }

        public override void SetLength(long value)
        {
            internalStream.SetLength(value);
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            internalStream.Write(buffer, offset, count);
        }
    }
}
