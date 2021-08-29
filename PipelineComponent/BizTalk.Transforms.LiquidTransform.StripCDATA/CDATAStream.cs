using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace BizTalk.Transforms.LiquidTransform
{
    public class CDATAStream : Stream
    {
        byte[] cdataStart = UTF8Encoding.UTF8.GetBytes("<![CDATA[");
        byte[] cdataEnd = UTF8Encoding.UTF8.GetBytes("]]></JSON>");
        //]]></JSON>
        Stream internalStream = null;

        public CDATAStream(Stream stream)
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
               return  internalStream.Position;
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
            int ret = 0;

            if (internalStream.Length == internalStream.Position)
                return 0;

            if (internalStream.Position == 0)
            {
                byte[] firstBuffer = new byte[count];

                ret = internalStream.Read(firstBuffer, offset, count);

                int start = IndexOf(firstBuffer, cdataStart, 0);

                Array.Copy(firstBuffer, start + cdataStart.Length, buffer, 0, ret - (start + cdataStart.Length));

                ret = ret - (start + cdataStart.Length);

                ret = CheckCDATA(buffer, ret);

                

            }
            else
            {
                ret = internalStream.Read(buffer, offset, count);

                ret = CheckCDATA(buffer,ret);

            }

            return ret;
            


        }

        private int CheckCDATA(byte[] buffer, int ret)
        {
            
            if (internalStream.Length == internalStream.Position)
            {
                ret = IndexOf(buffer, cdataEnd, 0);


            }
            else if ((internalStream.Position + cdataEnd.Length) > internalStream.Length)
            {
                long c = internalStream.Length - internalStream.Position;

                ret = ret - (int)(cdataEnd.Length - c);

                internalStream.Position = internalStream.Length;

           

            }

            return ret;
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            return internalStream.Seek(offset,  origin);
        }

        public override void SetLength(long value)
        {
            internalStream.SetLength(value);
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            internalStream.Write(buffer, offset, count);
        }

        private static int IndexOf(byte[] array, byte[] pattern, int offset)
        {
            int success = 0;
            for (int i = offset; i < array.Length; i++)
            {
                if (array[i] == pattern[success])
                {
                    success++;
                }
                else
                {
                    success = 0;
                }

                if (pattern.Length == success)
                {
                    return i - pattern.Length + 1;
                }
            }
            return -1;
        }
    }
}
