using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BitlockerCore
{
    public class RecoveryFileStream
    {
        public bool Write(string path, byte[] content)
        {
            try
            {
                using (var fileStream = new FileStream(path, FileMode.Create))
                {
                    using (var bWriter = new BinaryWriter(fileStream))
                    {
                        bWriter.Write(content);
                        bWriter.Close();
                    }
                    fileStream.Close();
                }
            }
            catch (IOException e)
            {
                return false;
            }
            return true;
        }
        public bool Write(string pathOnly,string fileName, byte[] content)
        {
            try
            {
                if (!Directory.Exists(pathOnly))
                {
                    Directory.CreateDirectory(pathOnly);
                }
                return Write(pathOnly + fileName, content);
            }
            catch (IOException e)
            {
                return false;
            }
            return true;
        }
        public byte[] Read(string path)
        {
            try
            {
                using (var fileStream = new FileStream(path, FileMode.Open))
                {
                    using (var bReader = new BinaryReader(fileStream))
                    {
                        var bytes = new byte[fileStream.Length];
                        //return Convert.ToBase64String(bReader.ReadBytes((int)fileStream.Length));
                        for(int i=0;i< fileStream.Length; i++)
                        {
                            bytes[i] = bReader.ReadByte();
                        }
                        return bytes;
                    }
                }
            }
            catch (IOException e)
            {
                return new byte[0];
            }
        }
    }
}
