using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace HTTLVN.QLTLH.Code.ChiHoi
{
    public class TransferFile
    {
        public TransferFile() { }

        public bool WriteBinarFile(byte[] fs, string path, string fileName)
        {
            try
            {
                MemoryStream memoryStream = new MemoryStream(fs);
                FileStream fileStream = new FileStream(path + fileName, FileMode.Create);
                memoryStream.WriteTo(fileStream);
                memoryStream.Close();
                fileStream.Close();
                fileStream = null;
                memoryStream = null;
                return true;
            }
            catch
            {
            }

            return false;
        }

        /// <summary>
        /// getBinaryFile：Return array of byte which you specified。
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public byte[] ReadBinaryFile(string path, string fileName)
        {
            if (File.Exists(path + fileName))
            {
                try
                {
                    ///Open and read a file。
                    FileStream fileStream = File.OpenRead(path + fileName);
                    byte[] arrReturn = ConvertStreamToByteBuffer(fileStream);
                    fileStream.Close();
                    return arrReturn;
                }
                catch
                {
                    return new byte[0];
                }
            }
            else
            {
                return new byte[0];
            }
        }

        /// <summary>
        /// ConvertStreamToByteBuffer：Convert Stream To ByteBuffer。
        /// </summary>
        /// <param name="theStream"></param>
        /// <returns></returns>
        public byte[] ConvertStreamToByteBuffer(System.IO.Stream theStream)
        {
            int b1;
            System.IO.MemoryStream tempStream = new System.IO.MemoryStream();
            while ((b1 = theStream.ReadByte()) != -1)
            {
                tempStream.WriteByte(((byte)b1));
            }
            return tempStream.ToArray();
        }
    }
}