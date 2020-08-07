using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace HZZG.Common.Tolls
{
    public static class ZipUtil
    {
        //gzip压缩  String压缩=>byte[]
        public static byte[] Compress( string content )
        {
            MemoryStream ms = new MemoryStream();
            GZipStream gzipStream = new GZipStream(ms, CompressionMode.Compress, true);
            var bytes = System.Text.ASCIIEncoding.Default.GetBytes(content);
            gzipStream.Write(bytes , 0 , bytes.Length);
            gzipStream.Close( );
            ms.Seek(0 , SeekOrigin.Begin);
            byte[] buffer = ReadBytes(ms);
            return buffer;
        }

        //byte[]解压=>string
        public static string Decompress( byte[] bytes )
        {
            MemoryStream ms = new MemoryStream();
            ms.Write(bytes , 0 , bytes.Length);
            ms.Seek(0 , SeekOrigin.Begin);
            GZipStream gzipStream = new GZipStream(ms, CompressionMode.Decompress);
            var zipBytes = ReadBytes(gzipStream);
            string stringContent = System.Text.ASCIIEncoding.Default.GetString(zipBytes);
            return stringContent;
        }

        //内容数组压缩成byte[]数组
        public static byte[] Compress( byte[] content )
        {
            MemoryStream ms = new MemoryStream();
            GZipStream gzipStream = new GZipStream(ms, CompressionMode.Compress, true);
            gzipStream.Write(content , 0 , content.Length);
            gzipStream.Close( );
            ms.Seek(0 , SeekOrigin.Begin);
            byte[] buffer = ReadBytes(ms);
            return buffer;
        }

        //解压成内容数组
        public static byte[] DecompressBin( byte[] bytes )
        {
            MemoryStream ms = new MemoryStream();
            ms.Write(bytes , 0 , bytes.Length);
            ms.Seek(0 , SeekOrigin.Begin);
            GZipStream gzipStream = new GZipStream(ms, CompressionMode.Decompress);
            var zipBytes = ReadBytes(gzipStream);
            return zipBytes;
        }

        private static byte[] ReadBytes( Stream ms )
        {
            int bufferSize = 100;
            byte[] buffer = new byte[bufferSize];
            List<byte> totalBytes = new List<byte>();
            while ( true )
            {
                int bytesRead = ms.Read(buffer, 0, bufferSize);
                if ( bytesRead == 0 )
                {
                    break;
                }

                totalBytes.AddRange(buffer.Take(bytesRead));
            }

            return totalBytes.ToArray( );
        }

        public static byte[] Struct2Bytes(object o)
        {
            // create a new byte buffer the size of your struct
            byte[] buffer = new byte[Marshal.SizeOf(o)];
            // pin the buffer so we can copy data into it w/o GC collecting it
            GCHandle bufferHandle = GCHandle.Alloc(buffer, GCHandleType.Pinned);
            // copy the struct data into the buffer
            Marshal.StructureToPtr(o, bufferHandle.AddrOfPinnedObject(), false);
            // free the GC handle
            bufferHandle.Free();

            return buffer;
        }

        public static T ToStruct<T>(byte[] by) where T : struct
        {
            int objectSize = Marshal.SizeOf(typeof(T));
            if (objectSize > by.Length) return default(T);

            // Allocate some unmanaged memory.
            IntPtr buffer = Marshal.AllocHGlobal(objectSize);

            // Copy the read byte array (byte[]) into the unmanaged memory block.
            Marshal.Copy(by, 0, buffer, objectSize);

            // Push the memory into a new struct of type (T).
            T returnStruct = (T)Marshal.PtrToStructure(buffer, typeof(T));

            // Free the unmanaged memory block.
            Marshal.FreeHGlobal(buffer);

            return returnStruct;
        }
    }
}