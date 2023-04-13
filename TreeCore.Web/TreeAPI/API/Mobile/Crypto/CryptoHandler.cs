using Org.BouncyCastle.Crypto.Paddings;
using Org.BouncyCastle.Utilities.Encoders;

namespace TreeAPI.API.Mobile.Crypto
{
    public class CryptoHandler
    {
        // DO NOT TOUCH THIS KEY UNLESS YOU KNOWN WHAT TO DO
        // This key is base64 encoded and must match the clients
        private string AES_KEY = "hwWT0eVCdNDLgb26gWJZsqcJ9De0QM67aZdTjFXzFAg=";

        private AESCrypto cryptoMethod;
        private bool cryptoEnabled = true;
        private System.Text.UTF8Encoding utf8EncDec = null;

        public CryptoHandler()
        {
            utf8EncDec = new System.Text.UTF8Encoding();

            cryptoMethod = new AESCrypto();
            cryptoMethod.setPadding(new Pkcs7Padding());

            // decode the key before we use it
            cryptoMethod.setKey(Base64.Decode(utf8EncDec.GetBytes(AES_KEY)));
        }

        public void setCryptoKey( string newKey )
        {
            cryptoMethod.setKey(Base64.Decode(utf8EncDec.GetBytes(newKey)));
        }

        public AESCrypto.StringEncryption EncryptString(string str)
        {
            AESCrypto.StringEncryption strEnc = new AESCrypto.StringEncryption(str);

            if (!cryptoEnabled)
            {
                strEnc.encryptedString = str;
                strEnc.encryptedBytes = utf8EncDec.GetBytes(str);

                return strEnc;
            }

            if (cryptoMethod.EncryptString(strEnc) > 0)
            {
                return strEnc;
            }

            return null;
        }

        public AESCrypto.StringDecryption DecryptString(string str, long len)
        {
            AESCrypto.StringDecryption strDec = new AESCrypto.StringDecryption(str, len);
            if (!cryptoEnabled)
            {

                strDec.decryptedString = str;
                strDec.decryptedBytes = utf8EncDec.GetBytes(str);

                return strDec;
            }

           if (cryptoMethod.DecryptString(strDec) > 0)
            {
                return strDec;
            }

            return null;
        }
    }
}
