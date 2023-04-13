/*****************************************
* Archivo generado                       *
* Usuario: Tree\build.system
* Fecha: 30/01/2019 16:56
*****************************************/
using System;
using System.Collections.Generic;
//using TreeCore.API.Mobile.Crypto;
using Newtonsoft.Json;

public static class EncryptedData
{
    public static int blockSize = 15034;
    public static string blockData = "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";
    public static string AES_KEY = "XKvCRXUx7MoUsoznYFy/p6ilCYE+XDanVGgiVCQdFnQ=";
}

public class ServerConnectionChooser
{

    public class ServerData
    {
        public ServerData()
        {
        }
        public ServerData(ServerData input)
        {
            name = input.name;
            dataSource = input.dataSource;
            initialCatalog = input.initialCatalog;
            persistSecurityInfo = input.persistSecurityInfo;
            userID = input.userID;
            password = input.password;
            extraParams = input.extraParams;
        }
        public string name { set; get; }
        public string dataSource { set; get; }
        public string initialCatalog { set; get; }
        public string persistSecurityInfo { set; get; }
        public string userID { set; get; }
        public string password { set; get; }
        public string extraParams { set; get; }
    }
    public class StringData
    {
        public StringData()
        {
        }
        public StringData(StringData input)
        {
            name = input.name;
            rawString = input.rawString;
        }
        public string name { set; get; }
        public string rawString { set; get; }
    }
    public class ServerConfiguration
    {
        public List<ServerData> configs { set; get; }
        public List<StringData> storedStrings { set; get; }
        public DateTime dateGenerated { set; get; }
        public string userGenerator { set; get; }
    }
    private static readonly bool initialised = false;
    private static readonly Dictionary<string, string> dataServerDict = new Dictionary<string, string>();
    private static readonly Dictionary<string, string> dataStringDict = new Dictionary<string, string>();
    //private static CryptoHandler cryptoHandler = new CryptoHandler();

    static ServerConnectionChooser()
    {
        if (initialised == false)
        {
            //cryptoHandler.setCryptoKey(EncryptedData.AES_KEY);
            //AESCrypto.StringDecryption strDec = cryptoHandler.DecryptString(EncryptedData.blockData, EncryptedData.blockSize);
            //if (strDec != null &&
            //    strDec.decryptedString != null)
            //{
            //    ServerConfiguration currentConfiguration = new ServerConfiguration();
            //    currentConfiguration = JsonConvert.DeserializeObject<ServerConfiguration>(strDec.decryptedString);
            //    if (currentConfiguration != null)
            //    {
            //        foreach (ServerData d in currentConfiguration.configs)
            //        {
            //            string serverString = createServerString(d);
            //            dataServerDict.Add(d.name, serverString);
            //        }
            //        foreach (StringData d in currentConfiguration.storedStrings)
            //        {
            //            dataStringDict.Add(d.name, d.rawString);
            //        }
            //        initialised = true;
            //    }
            //}
        }
    }

    public static string getServerString(string name)
    {
        if (dataServerDict.Count > 0 &&
            dataServerDict.ContainsKey(name))
            return dataServerDict[name];
        return name;
    }
    public static string getStoredString(string name)
    {
        if (dataStringDict.Count > 0 &&
            dataStringDict.ContainsKey(name))
            return dataStringDict[name];
        return name;
    }

    private static string createServerString(ServerData data)
    {
        string str = "";
        str = str + "Data Source = " + data.dataSource + ";"; // data source
        str = str + "Initial Catalog = " + data.initialCatalog + ";"; // initial catalog
        string boolStr = "True";
        if (data.persistSecurityInfo.ToUpper() != "TRUE")
            boolStr = "False";
        str = str + "Persist Security Info = " + boolStr + ";"; // persist security data
        str = str + "User ID = " + data.userID + ";"; // user ID
        str = str + "Password = " + data.password + ";"; // data source
        if (data.extraParams != "" &&
            data.extraParams != " " &&
            data.extraParams.Length != 0)
            str = str + data.extraParams; // extra params 
        return str;
    }
}
