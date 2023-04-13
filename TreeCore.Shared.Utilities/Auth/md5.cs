using System;
using System.Security.Cryptography;
using System.Text;

namespace TreeCore.Shared.Utilities.Auth
{
    public class md5
    {
        public static string MD5String(string SourceText)
        {
            //Create an encoding object to ensure the encoding standard for the source text
            UnicodeEncoding Ue = new UnicodeEncoding();
            //Retrieve a byte array based on the source text
            byte[] ByteSourceText = Ue.GetBytes(SourceText);
            //Instantiate an MD5 Provider object
            MD5CryptoServiceProvider Md5 = new MD5CryptoServiceProvider();
            //Compute the hash value from the source
            byte[] ByteHash = Md5.ComputeHash(ByteSourceText);
            //And convert it to String format for return
            return Convert.ToBase64String(ByteHash);
        }
    }
}
