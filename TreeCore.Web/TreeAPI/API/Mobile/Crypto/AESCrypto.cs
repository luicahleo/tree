using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Paddings;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Utilities.Encoders;

namespace TreeAPI.API.Mobile.Crypto
{
    public class AESCrypto
    {
        private IBlockCipher AESCipher = new Org.BouncyCastle.Crypto.Engines.AesEngine();

        private PaddedBufferedBlockCipher pbbc;
        private KeyParameter key;
        private System.Text.UTF8Encoding UTF8EncDec;

        public class StringEncryption
        {
            public StringEncryption(string input)
            {
                originalString = input;
            }

            // inputs
            public string originalString;
            public long originalByteLength;

            // outputs
            public byte[] encryptedBytes;
            public string encryptedString;
        }

        public class StringDecryption
        {
            public StringDecryption(string input, long srcByteLength)
            {
                encryptedString = input;
                originalByteLength = srcByteLength;
            }

            // inputs
            public string encryptedString;
            public long originalByteLength;

            // outputs
            public byte[] decryptedBytes;
            public string decryptedString;
        }

        public AESCrypto()
        {
            UTF8EncDec = new System.Text.UTF8Encoding();
        }

        public void setPadding(IBlockCipherPadding bcp)
        {
            this.pbbc = new PaddedBufferedBlockCipher(AESCipher, bcp);
        }

        public void setKey(byte[] key)
        {
            this.key = new Org.BouncyCastle.Crypto.Parameters.KeyParameter(key);
        }

        public byte[] encrypt(byte[] input)
        {
            return processing(input, true);
        }

        public byte[] decrypt(byte[] input)
        {
            return processing(input, false);
        }

        public long EncryptString(StringEncryption data)
        {
            byte[] ba = null;
            try
            {
                ba = UTF8EncDec.GetBytes(data.originalString);
            }
            catch (System.ArgumentNullException ex)
            {
                System.Console.Write(ex);
            }
            catch (System.Text.EncoderFallbackException ex)
            {
                System.Console.Write(ex);
            }

            try
            {
                data.encryptedBytes = encrypt(ba);
            }
            catch (InvalidCipherTextException ex)
            {
                System.Console.Write(ex);

                return 0;
            }

            data.encryptedString = UTF8EncDec.GetString(Base64.Encode(data.encryptedBytes));
            data.originalByteLength = ba.Length;

            return data.originalByteLength;
        }

        public long DecryptString(StringDecryption data)
        {
            byte[] src = null;
            try
            {
                src = Base64.Decode(UTF8EncDec.GetBytes(data.encryptedString));
            }
            catch (System.ArgumentNullException ex)
            {
                System.Console.Write(ex);
            }
            catch (System.Text.EncoderFallbackException ex)
            {
                System.Console.Write(ex);
            }


            byte[] retr = null;
            try
            {
                retr = decrypt(src);
            }
            catch (InvalidCipherTextException ex)
            {
                System.Console.Write(ex);
            }

            if (data.originalByteLength > 0)
            {
                data.decryptedBytes = new byte[(int)data.originalByteLength];

                if (retr != null)
                {
                    if (retr.Length == data.originalByteLength)
                    {
                        data.decryptedBytes = retr;
                    }
                    else
                    {
                        System.Array.Copy(retr, 0, data.decryptedBytes, 0, (int)data.originalByteLength);
                    }
                }

                try
                {
                    data.decryptedString = UTF8EncDec.GetString(data.decryptedBytes);
                }
                catch (System.ArgumentNullException ex)
                {
                    System.Console.Write(ex);
                }
                catch (System.Text.EncoderFallbackException ex)
                {
                    System.Console.Write(ex);
                }
            }

            return data.originalByteLength;
        }

        private byte[] processing(byte[] input, bool encrypt)
        {
            pbbc.Init(encrypt, key);

            byte[] output = new byte[pbbc.GetOutputSize(input.Length)];

            int bytesWrittenOut = pbbc.ProcessBytes( input, 0, input.Length, output, 0);

            pbbc.DoFinal(output, bytesWrittenOut);

            return output;
        }
    }
}
