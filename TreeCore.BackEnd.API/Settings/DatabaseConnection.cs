using Microsoft.Extensions.Configuration;
using System.Text;
using System;

using System.Security.Cryptography;


namespace TreeCore.BackEnd.API.Settings
{
    public class DatabaseConnection
    {
        static string key = "ABCDEFG54669525PQRSTUVWXYZabcdef852846opqrstuvwxyz";
        public static string BuildConnectionString(IConfiguration config, string sEnviroment)
        {
            DatabaseSettings dbSettings = new DatabaseSettings();
            //string encript = EncryptKey("Atrebo.2021");
            config.Bind("database", dbSettings);
            if (!sEnviroment.Equals("Development"))
            {
                dbSettings.Password = DecryptKey(dbSettings.Password); 
            }
            StringBuilder sb = new StringBuilder();
            sb.Append($"data source={dbSettings.Server};");
            sb.Append($"initial catalog={dbSettings.DatabaseName};");
            sb.Append($"user id={dbSettings.User};");
            sb.Append($"password={dbSettings.Password};");
            sb.Append($"persist security info={dbSettings.SecurityInfo}");

            return sb.ToString();
        }

        public static string DecryptKey(string clave)
        {
            byte[] keyArray;
            //convierte el texto en una secuencia de bytes
            byte[] Array_a_Descifrar =
            Convert.FromBase64String(clave);

            //se llama a las clases que tienen los algoritmos
            //de encriptación se le aplica hashing
            //algoritmo MD5
            MD5CryptoServiceProvider hashmd5 =
            new MD5CryptoServiceProvider();

            keyArray = hashmd5.ComputeHash(
            UTF8Encoding.UTF8.GetBytes(key));

            hashmd5.Clear();

            TripleDESCryptoServiceProvider tdes =
            new TripleDESCryptoServiceProvider();

            tdes.Key = keyArray;
            tdes.Mode = CipherMode.ECB;
            tdes.Padding = PaddingMode.PKCS7;

            ICryptoTransform cTransform =
             tdes.CreateDecryptor();
            byte[] resultArray;
            try
            {
                resultArray = cTransform.TransformFinalBlock(Array_a_Descifrar, 0, Array_a_Descifrar.Length);
            }
            catch (Exception ex)
            {
                string x = ex.Message;
                resultArray = null;
            }
            tdes.Clear();
            //se regresa en forma de cadena
            return UTF8Encoding.UTF8.GetString(resultArray);
        }

        public static string EncryptKey(string cadena)
        {
            //arreglo de bytes donde guardaremos la llave
            byte[] keyArray;
            //arreglo de bytes donde guardaremos el texto
            //que vamos a encriptar
            byte[] Arreglo_a_Cifrar =
            UTF8Encoding.UTF8.GetBytes(cadena);

            //se utilizan las clases de encriptación
            //provistas por el Framework
            //Algoritmo MD5
            MD5CryptoServiceProvider hashmd5 =
            new MD5CryptoServiceProvider();
            //se guarda la llave para que se le realice
            //hashing
            keyArray = hashmd5.ComputeHash(
            UTF8Encoding.UTF8.GetBytes(key));

            hashmd5.Clear();

            //Algoritmo 3DAS
            TripleDESCryptoServiceProvider tdes =
            new TripleDESCryptoServiceProvider();

            tdes.Key = keyArray;
            tdes.Mode = CipherMode.ECB;
            tdes.Padding = PaddingMode.PKCS7;

            //se empieza con la transformación de la cadena
            ICryptoTransform cTransform =
            tdes.CreateEncryptor();

            //arreglo de bytes donde se guarda la
            //cadena cifrada
            byte[] ArrayResultado =
            cTransform.TransformFinalBlock(Arreglo_a_Cifrar,
            0, Arreglo_a_Cifrar.Length);

            tdes.Clear();

            //se regresa el resultado en forma de una cadena
            return Convert.ToBase64String(ArrayResultado,
                   0, ArrayResultado.Length);

        }



        public class DatabaseSettings
        {
            public string Server { get; set; }
            public int Port { get; set; }
            public string DatabaseName { get; set; }
            public string User { get; set; }
            public string Password { get; set; }
            public bool SecurityInfo { get; set; }
        }
    }
}
