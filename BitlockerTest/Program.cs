using BitlockerCore;
using BitlockerCore.Encryptions;
using System;
using System.Text;

namespace BitlockerTest
{
    class Program
    {
        static string RecoveryPassword = "556699-633963-469744-326909-325369-041349-671308-046717";
        static string key = "12345678876543211234567887654abc";
        static void Main(string[] args)
        {
            // var bl = new BitLockerExecute("L");
            // bl.Unlock(RecoveryPassword);
            //// bl.Lock();
            // Console.ReadLine();
            //string str1 = Encoding.UTF8.GetString(bytes);

            //ae();
            de();

            Console.ReadLine();
        }
        public static void ae()
        {
            var bytes = Encoding.UTF8.GetBytes(RecoveryPassword);
            string str2 = Encoding.UTF8.GetString(bytes);

            var aes = new AESUtil();
            var afterAESStr = aes.AesEncrypt(RecoveryPassword, key);
            var recoveryFileStream = new RecoveryFileStream();
            recoveryFileStream.Write(@"J:\bitlockerauto.rp", afterAESStr);
        }
        public static void de()
        {
            var recoveryFileStream = new RecoveryFileStream();
            var byteFile = recoveryFileStream.Read(@"J:\bitlockerauto.rp");
            //var bytes = Encoding.UTF8.GetBytes(byteFile);
            //var str2 = Encoding.UTF8.GetString(bytes);
            var aes = new AESUtil();
            var afterAESStr = aes.AesDecrypt(byteFile, key);

            Console.WriteLine("解密：" + afterAESStr);

        }
    }
}
