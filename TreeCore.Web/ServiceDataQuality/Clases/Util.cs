using CapaNegocio;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Diagnostics;

namespace TreeCore
{
    public class Util
    {
        // This constant is used to determine the keysize of the encryption algorithm in bits.
        // We divide this by 8 within the code below to get the equivalent number of bytes.
        private const int Keysize = 256;

        // This constant determines the number of iterations for the password bytes generation function.
        private const int DerivationIterations = 1000;


        static string key = "ABCDEFG54669525PQRSTUVWXYZabcdef852846opqrstuvwxyz";

        #region ENCRYPTACION

        public static string Encrypt(string plainText, string passPhrase)
        {
            // Salt and IV is randomly generated each time, but is preprended to encrypted cipher text
            // so that the same Salt and IV values can be used when decrypting.  
            var saltStringBytes = Generate256BitsOfRandomEntropy();
            var ivStringBytes = Generate256BitsOfRandomEntropy();
            var plainTextBytes = Encoding.UTF8.GetBytes(plainText);
            using (var password = new Rfc2898DeriveBytes(passPhrase, saltStringBytes, DerivationIterations))
            {
                var keyBytes = password.GetBytes(Keysize / 8);
                using (var symmetricKey = new RijndaelManaged())
                {
                    symmetricKey.BlockSize = 256;
                    symmetricKey.Mode = CipherMode.CBC;
                    symmetricKey.Padding = PaddingMode.PKCS7;
                    using (var encryptor = symmetricKey.CreateEncryptor(keyBytes, ivStringBytes))
                    {
                        using (var memoryStream = new MemoryStream())
                        {
                            using (var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                            {
                                cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
                                cryptoStream.FlushFinalBlock();
                                // Create the final bytes as a concatenation of the random salt bytes, the random iv bytes and the cipher bytes.
                                var cipherTextBytes = saltStringBytes;
                                cipherTextBytes = cipherTextBytes.Concat(ivStringBytes).ToArray();
                                cipherTextBytes = cipherTextBytes.Concat(memoryStream.ToArray()).ToArray();
                                memoryStream.Close();
                                cryptoStream.Close();
                                return Convert.ToBase64String(cipherTextBytes);
                            }
                        }
                    }
                }
            }
        }

        public static string Decrypt(string cipherText, string passPhrase)
        {
            // Get the complete stream of bytes that represent:
            // [32 bytes of Salt] + [32 bytes of IV] + [n bytes of CipherText]
            var cipherTextBytesWithSaltAndIv = Convert.FromBase64String(cipherText);
            // Get the saltbytes by extracting the first 32 bytes from the supplied cipherText bytes.
            var saltStringBytes = cipherTextBytesWithSaltAndIv.Take(Keysize / 8).ToArray();
            // Get the IV bytes by extracting the next 32 bytes from the supplied cipherText bytes.
            var ivStringBytes = cipherTextBytesWithSaltAndIv.Skip(Keysize / 8).Take(Keysize / 8).ToArray();
            // Get the actual cipher text bytes by removing the first 64 bytes from the cipherText string.
            var cipherTextBytes = cipherTextBytesWithSaltAndIv.Skip((Keysize / 8) * 2).Take(cipherTextBytesWithSaltAndIv.Length - ((Keysize / 8) * 2)).ToArray();

            using (var password = new Rfc2898DeriveBytes(passPhrase, saltStringBytes, DerivationIterations))
            {
                var keyBytes = password.GetBytes(Keysize / 8);
                using (var symmetricKey = new RijndaelManaged())
                {
                    symmetricKey.BlockSize = 256;
                    symmetricKey.Mode = CipherMode.CBC;
                    symmetricKey.Padding = PaddingMode.PKCS7;
                    using (var decryptor = symmetricKey.CreateDecryptor(keyBytes, ivStringBytes))
                    {
                        using (var memoryStream = new MemoryStream(cipherTextBytes))
                        {
                            using (var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
                            {
                                var plainTextBytes = new byte[cipherTextBytes.Length];
                                var decryptedByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);
                                memoryStream.Close();
                                cryptoStream.Close();
                                return Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount);
                            }
                        }
                    }
                }
            }
        }

        private static byte[] Generate256BitsOfRandomEntropy()
        {
            var randomBytes = new byte[32]; // 32 Bytes will give us 256 bits.
            using (var rngCsp = new RNGCryptoServiceProvider())
            {
                // Fill the array with cryptographically secure random bytes.
                rngCsp.GetBytes(randomBytes);
            }
            return randomBytes;
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

            catch (Exception)
            {
                resultArray = null;
            }
            tdes.Clear();
            //se regresa en forma de cadena
            return UTF8Encoding.UTF8.GetString(resultArray);
        }

        #endregion

        #region CLAVES

        public static string EncolarClaveAnterior(string anteriores, string nueva)
        {
            string aux = "";
            int pos;
            if (anteriores == null)
            {
                aux = nueva;
            }
            else
            {
                if (anteriores.Split(Convert.ToChar(",")).Length >= 5)
                {
                    pos = anteriores.IndexOf(",", 0);
                    aux = anteriores.Substring(pos + 1) + "," + nueva;
                }
                else
                {
                    aux = anteriores + "," + nueva;
                }

            }
            return aux;
        }

        #endregion

        #region EXCEPCIONTES


        public static string ExceptionHandler(System.Exception ex)
        {
            string codTit = "";

            if (ex.GetType().Name.Equals("SystemException"))
            {
                //Excepción del sistema
                codTit = Comun.cod110;
            }
            else if (ex.GetType().Name.Equals("IndexOutOfRangeException"))
            {
                //Indexación fuera del intervalo válido
                codTit = Comun.cod111;
            }
            else if (ex.GetType().Name.Equals("NullReferenceException"))
            {
                // Referencia a objeto nulo
                codTit = Comun.cod112;
            }
            else if (ex.GetType().Name.Equals("AccessViolationException"))
            {
                //Acceso a memoria no permitida
                codTit = Comun.cod113;
            }
            else if (ex.GetType().Name.Equals("InvalidOperationException"))
            {
                //Llamada al método no es válida para el estado actual del objeto
                codTit = Comun.cod114;
            }

            else if (ex.GetType().Name.Equals("ArgumentException"))
            {
                //Excepción de argumento
                codTit = Comun.cod120;
            }

            else if (ex.GetType().Name.Equals("ArgumentNullException"))
            {
                //Argumento nulo
                codTit = Comun.cod121;

            }
            else if (ex.GetType().Name.Equals("ArgumentOutOfRangeException"))
            {
                //Argumentos fuera del intervalo determinado
                codTit = Comun.cod122;
            }


            else if (ex.GetType().Name.Equals("ExternalException"))
            {
                //Excepción de interoperabilidad COM o SEH (structured exception handling, control estructurado de excepciones).
                codTit = Comun.cod130;
            }


            else if (ex.GetType().Name.Equals("COMException"))
            {
                //Resultado desconocido del método COM
                codTit = Comun.cod131;
            }
            else if (ex.GetType().Name.Equals("SEHException"))
            {
                //Tree.Web.MiniExt.MensajeBox("Código de Excepción 132 ", Comun.ERRORAJAXGENERICO, MessageBox.Icon.WARNING, null);

                //Error de Control de excepciones estructurado
                codTit = Comun.cod132;

            }
            else if (ex.GetType().Name.Equals("FormatException"))
            {

                codTit = Comun.cod140;

            }
            else
            {

                //Error genérico (insert en conflicto con foreign key,... etc)
                codTit = Comun.cod100;

            }


            return codTit;
        }

        #endregion

        #region GESTION PROCESOS

        public static List<int> SnapshotProcessesWithName(string name)
        {
            List<int> oldProcessIDs = new List<int>();

            Process[] runningProcesses = Process.GetProcessesByName(name);

            foreach (Process pro in runningProcesses)
            {
                oldProcessIDs.Add(pro.Id);
            }

            return oldProcessIDs;
        }

        public static void KillNewProcessesWithName(string name, List<int> oldProcessIDs)
        {
            Process[] runningProcesses = Process.GetProcessesByName(name);

            foreach (Process proc in runningProcesses)
            {
                if (oldProcessIDs != null &&
                    oldProcessIDs.Count > 0 &&
                    !oldProcessIDs.Contains(proc.Id))
                {
                    try
                    {
                        proc.Kill();
                    }
                    catch (System.Exception)
                    {

                    }
                }
            }
        }

        #endregion

        #region CONVERTIDORES

        public static string GetDateToLocalizedShortString(DateTime? dt, string locale)
        {
            // early exit if date is null
            if (dt == null)
                return "";

            try
            {
                // try the culture info
                System.Globalization.CultureInfo ci = new System.Globalization.CultureInfo(locale);
                if (ci != null)
                {
                    System.Globalization.DateTimeFormatInfo dtfi = ci.DateTimeFormat;

                    DateTime convert = (DateTime)dt;

                    // return the short pattern
                    return String.Format("{0}", convert.ToString(dtfi.ShortDatePattern));
                }
            }
            catch (System.Exception)
            {
                // problem converting the locale?
            }

            // got here there is a problem, return an empty string
            return "";
        }

        public static string GetDoubleToStringConComa(Double? dou, string locale)
        {
            // early exit if date is null
            if (dou == null)
                return ";";

            try
            {
                string dato = dou.ToString();
                dato = dato.Replace(".", ",");

                return String.Format("{0};", dato);
            }
            catch (System.Exception)
            {
                // problem converting the locale?
            }

            // got here there is a problem, return an empty string
            return ";";
        }

        public static class NumberConvert
        {
            static double StringNumberToDouble(string val, System.Globalization.CultureInfo ci)
            {
                double outVal = 0.0;
                System.Globalization.NumberStyles style = System.Globalization.NumberStyles.Any;

                try
                {
                    if (double.TryParse(val, style, ci, out outVal))
                    {
                        return outVal;
                    }
                    else
                    {

                    }
                }
                catch (System.ArgumentException ex)
                {
                    System.Console.Write(ex);
                }

                return outVal;
            }

        }




        #endregion

        #region ESTADISITICAS
        /// <summary>
        /// Funcion que escribe un registro cada vez que se cargue una pagina
        /// siempre y cuando este activa la variable que habilita la estadistica.
        /// </summary>
        /// <param name="UsuarioID"></param>
        /// <param name="ProyectoTipoID"></param>
        /// <param name="pagina"></param>
        /// <param name="Activo"> Indica si esta activa el registro de estadisticas</param>
        /// <param name="Comentario"> Especifica la accion realizada en casos que una misma pagina genere varios formularios</param>
        public static void EscribeEstadistica(long UsuarioID, long ProyectoTipoID, string pagina, bool Activo, string Comentario)
        {

            try
            {

                if (Activo == true)
                {
                    EstadisticasController cEstadisticas = new EstadisticasController();
                    Data.Estadisticas registro = new Data.Estadisticas();

                    registro.UsuarioID = UsuarioID;
                    registro.FechaHoraInicio = DateTime.Now;
                    registro.Pagina = pagina;
                    registro.ProyectoTipoID = ProyectoTipoID;
                    registro.Comentarios = Comentario;

                    UsuariosController cUsuario = new UsuariosController();
                    Data.Usuarios usuario = new Data.Usuarios();
                    usuario = cUsuario.GetItem(UsuarioID);

                    registro.ClienteID = usuario.ClienteID;


                    if (Comentario == "LOGOUT")
                    {
                        registro.FechaHoraFin = DateTime.Now;
                    }

                    cEstadisticas.AddItem(registro);
                }

            }

            catch (Exception exception)
            {
                throw new Exception(" Se ha producido un error al guardar el registro.", exception);
            }

        }
        #endregion

    }

}
