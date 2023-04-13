using CapaNegocio;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using TreeAPI.API.Mobile.Crypto;
using TreeAPI.API.Mobile.General;
using TreeAPI.API.Mobile.Login.JSON;
using TreeCore.Data;

namespace TreeAPI.API.Mobile.Login
{
    // THIS MUST MATCH THE CLIENT
    public class LoginRequestSOAP
    {
        public string Platform;
        public string Locale;
        public string UserLogin;
        public string Password;
        public string UniqueDeviceID;
        public string FCMKey;
        public string UserMAC;

        public long ULLength;
        public long PLength;
    }

    public class MobileLoginHandler
    {
        // THIS MUST MATCH THE CLIENT
        public enum MobileModule
        {
            None                    = 0,
            AcquisitionsManagement  = 1,   
            Energy                  = 2,   
            Sharing                 = 3,
            Renegotiation           = 4,
            Towers                  = 5,
            Indoor                  = 6,
            AcquisitionsEndToEnd    = 7,
            Installation            = 8,
            Legal                   = 9,
            Maintenance             = 10,
            SpaceManagement         = 11,
            Uninstallation          = 12,
            Access                  = 13,
            Audit                   = 14,
            DocumentsManagement     = 15,
            Environment             = 16,
            Incidents               = 17,
            Regulations             = 18,
            Exchange                = 19,
            Invoicing               = 20,
            Inventory               = 21
        }

        public string Process(System.Web.SessionState.HttpSessionState Session, CryptoHandler cryptoHandler, LoginRequestSOAP req)
        {
            Locale appLocale = new Locale();

            string selectedLocale = appLocale.MobileLocale(req.Platform, req.Locale);

            // set the session locale
            Session["LOCALE"] = selectedLocale;

            MobileResourceRequester ResHelper = new MobileResourceRequester(Resources.MobileResources.ResourceManager, selectedLocale);

            // try and find the user
            Usuarios us = new Usuarios();
            UsuariosController cController = new UsuariosController();

            LoginMobileSessionPackage jsonLoginPackage = new LoginMobileSessionPackage();
            jsonLoginPackage.RequestResult = false;
            jsonLoginPackage.RequestResultString = ResHelper.GetString("srv_error");
            jsonLoginPackage.BlockSize = 0;
            jsonLoginPackage.BlockData = "";

            UserMobileSession DataBlock;

            AESCrypto.StringDecryption strOut = cryptoHandler.DecryptString(req.UserLogin, req.ULLength);
            if (strOut.decryptedString != null &&
                strOut.decryptedString.Length > 0)
                req.UserLogin = strOut.decryptedString;

            strOut = cryptoHandler.DecryptString(req.Password, req.PLength);
            if (strOut.decryptedString != null &&
                strOut.decryptedString.Length > 0)
                req.Password = strOut.decryptedString;

            try
            {
                //Se añade este código para que compile pero hay qeu comprobarlo con el móvil
                //UsuariosSesiones usuariosesion = new UsuariosSesiones();
                us = cController.UsuariosLogin(req.UserLogin, req.Password);
                if (us != null &&
                    us.Activo && us.Clave.Equals(Tree.Utiles.md5.MD5String(req.Password)))
                {
                    ModulosController moduleController = new ModulosController();
                    List<Modulos> moduleList = moduleController.getModulosObj(us.UsuarioID);

                    DataBlock = new UserMobileSession();
                    DataBlock.Locale = req.Locale;
                    DataBlock.LoginTime = DateTimeOffset.Now.ToString("o");
                    DataBlock.UserName = us.Nombre + " " + us.Apellidos;
                    DataBlock.UserID = us.UsuarioID;

                    DataBlock.UserMobileAccess = new List<MobileModuleAccess>();

                    foreach (Modulos module in moduleList)
                    {
                        if (module.ModuloID == (long)Comun.ModFun.ACCESO_TOTAL_SHARING_EMPLAZAMIENTOS ||
                            module.ModuloID == (long)Comun.ModFun.ACCESO_USUARIO_A_SHARING_EMPLAZAMIENTOS ||
                            module.ModuloID == (long)Comun.ModFun.ACCESO_CLIENTE_A_SHARING_EMPLAZAMIENTOS ||
                            module.ModuloID == (long)Comun.ModFun.ACCESO_SOLO_LECTURA_A_SHARING)
                        {
                            
                            bool addToList = true;
                            foreach (MobileModuleAccess val in DataBlock.UserMobileAccess)
                            {
                                if (val.ModuleID == (long)MobileModule.Sharing)
                                {
                                    addToList = false;
                                }
                            }

                            if (addToList)
                            {
                                MobileModuleAccess mod = new MobileModuleAccess();
                                mod.ModuleID = (long)MobileModule.Sharing;
                                DataBlock.UserMobileAccess.Add(mod);
                            }
                        }

                        if (module.ModuloID == (long)Comun.ModFun.ACCESO_TOTAL_ACCESS_EMPLAZAMIENTOS ||
                            module.ModuloID == (long)Comun.ModFun.ACCESO_USUARIO_A_ACCESS_EMPLAZAMIENTOS ||
                            module.ModuloID == (long)Comun.ModFun.ACCESO_CLIENTE_A_ACCESS_EMPLAZAMIENTOS ||
                            module.ModuloID == (long)Comun.ModFun.ACCESO_SOLO_LECTURA_A_ACCESS)
                        {
                            bool addToList = true;
                            foreach (MobileModuleAccess val in DataBlock.UserMobileAccess)
                            { 
                                if (val.ModuleID == (long)MobileModule.Access)
                                {
                                    addToList = false;
                                }
                            }

                            if (addToList)
                            {
                                MobileModuleAccess mod = new MobileModuleAccess();
                                mod.ModuleID = (long)MobileModule.Access;
                                DataBlock.UserMobileAccess.Add(mod);
                            }
                        }

                        if (module.ModuloID == (long)Comun.ModFun.ACCESO_TOTAL_ADQUISICIONES_EMPLAZAMIENTOS ||
                            module.ModuloID == (long)Comun.ModFun.ACCESO_TOTAL_A_ADQUISICIONES_DBI ||
                            module.ModuloID == (long)Comun.ModFun.ACCESO_A_ADQUISICIONES_MODULO ||
                            module.ModuloID == (long)Comun.ModFun.ACCESO_CLIENTE_A_ADQUISICIONES_EMPLAZAMIENTOS ||
                            module.ModuloID == (long)Comun.ModFun.ACCESO_USUARIO_A_ADQUISICIONES_EMPLAZAMIENTOS ||
                            module.ModuloID == (long)Comun.ModFun.ACCESO_SOLO_LECTURA_A_ADQUISICIONES)
                        {
                            bool addToList = true;
                            foreach (MobileModuleAccess val in DataBlock.UserMobileAccess)
                            {
                                if (val.ModuleID == (long)MobileModule.AcquisitionsManagement)
                                {
                                    addToList = false;
                                }
                            }

                            if (addToList)
                            {
                                MobileModuleAccess mod = new MobileModuleAccess();
                                mod.ModuleID = (long)MobileModule.AcquisitionsManagement;
                                DataBlock.UserMobileAccess.Add(mod);
                            }
                        }

                        if (module.ModuloID == (long)Comun.ModFun.ACCESO_TOTAL_AUDIT_EMPLAZAMIENTOS ||
                            module.ModuloID == (long)Comun.ModFun.ACCESO_USUARIO_A_AUDIT_EMPLAZAMIENTOS ||
                            module.ModuloID == (long)Comun.ModFun.ACCESO_SOLO_LECTURA_A_AUDIT)
                        {
                            bool addToList = true;
                            foreach (MobileModuleAccess val in DataBlock.UserMobileAccess)
                            {
                                if (val.ModuleID == (long)MobileModule.Audit)
                                {
                                    addToList = false;
                                }
                            }

                            if (addToList)
                            {
                                MobileModuleAccess mod = new MobileModuleAccess();
                                mod.ModuleID = (long)MobileModule.Audit;
                                DataBlock.UserMobileAccess.Add(mod);
                            }
                        }

                        if (module.ModuloID == (long)Comun.ModFun.ACCESO_TOTAL_ENERGY_EMPLAZAMIENTOS ||
                            module.ModuloID == (long)Comun.ModFun.ACCESO_USUARIO_A_ENERGY_EMPLAZAMIENTOS ||
                            module.ModuloID == (long)Comun.ModFun.ACCESO_CLIENTE_A_ENERGY_EMPLAZAMIENTOS ||
                            module.ModuloID == (long)Comun.ModFun.ACCESO_SOLO_LECTURA_A_ENERGY)
                        {
                            bool addToList = true;
                            foreach (MobileModuleAccess val in DataBlock.UserMobileAccess)
                            {
                                if (val.ModuleID == (long)MobileModule.Energy)
                                {
                                    addToList = false;
                                }
                            }

                            if (addToList)
                            {
                                MobileModuleAccess mod = new MobileModuleAccess();
                                mod.ModuleID = (long)MobileModule.Energy;
                                DataBlock.UserMobileAccess.Add(mod);
                            }
                        }

                        if (module.ModuloID == (long)Comun.ModFun.ACCESO_TOTAL_INSTALL_TECNICA_EMPLAZAMIENTOS ||
                            module.ModuloID == (long)Comun.ModFun.ACCESO_USUARIO_INSTALL_TECNICA_EMPLAZAMIENTOS ||
                            module.ModuloID == (long)Comun.ModFun.ACCESO_CLIENTE_INSTALL_TECNICA_EMPLAZAMIENTOS ||
                            module.ModuloID == (long)Comun.ModFun.ACCESO_SOLO_LECTURA_A_INSTALL_TECNICA ||
                            module.ModuloID == (long)Comun.ModFun.ACCESO_TOTAL_A_INSTALL_OBRA_CIVIL_EMPLAZAMIENTOS ||
                            module.ModuloID == (long)Comun.ModFun.ACCESO_RESTRINGIDA_A_INSTALL_OBRA_CIVIL_EMPLAZAMIENTOS ||
                            module.ModuloID == (long)Comun.ModFun.ACCESO_USUARIO_A_INSTALL_OBRA_CIVIL_EMPLAZAMIENTOS ||
                            module.ModuloID == (long)Comun.ModFun.ACCESO_CLIENTE_A_INSTALL_OBRA_CIVIL_EMPLAZAMIENTOS ||
                            module.ModuloID == (long)Comun.ModFun.ACCESO_SOLO_LECTURA_A_INSTALL_OBRA_CIVIL)
                        {
                            bool addToList = true;
                            foreach (MobileModuleAccess val in DataBlock.UserMobileAccess)
                            {
                                if (val.ModuleID == (long)MobileModule.Installation)
                                {
                                    addToList = false;
                                }
                            }

                            if (addToList)
                            {
                                MobileModuleAccess mod = new MobileModuleAccess();
                                mod.ModuleID = (long)MobileModule.Installation;
                                DataBlock.UserMobileAccess.Add(mod);
                            }
                        }

                        if (module.ModuloID == (long)Comun.ModFun.ACCESO_TOTAL_MANTENIMIENTO_EMPLAZAMIENTOS ||
                            module.ModuloID == (long)Comun.ModFun.ACCESO_CLIENTE_A_MANTENIMIENTO_EMPLAZAMIENTOS ||
                            module.ModuloID == (long)Comun.ModFun.ACCESO_SOLO_LECTURA_A_MANTENIMIENTO)
                        {
                            bool addToList = true;
                            foreach (MobileModuleAccess val in DataBlock.UserMobileAccess)
                            {
                                if (val.ModuleID == (long)MobileModule.Maintenance)
                                {
                                    addToList = false;
                                }
                            }

                            if (addToList)
                            {
                                MobileModuleAccess mod = new MobileModuleAccess();
                                mod.ModuleID = (long)MobileModule.Maintenance;
                                DataBlock.UserMobileAccess.Add(mod);
                            }
                        }

                        if (module.ModuloID == (long)Comun.ModFun.ACCESO_CLIENTE_A_MANTENIMIENTO_EMPLAZAMIENTOS)
                        {
                            bool addToList = true;
                            foreach (MobileModuleAccess val in DataBlock.UserMobileAccess)
                            {
                                if (val.ModuleID == (long)MobileModule.Maintenance)
                                {
                                    addToList = false;
                                }
                            }

                            if (addToList)
                            {
                                MobileModuleAccess mod = new MobileModuleAccess();
                                mod.ModuleID = (long)MobileModule.Maintenance;
                                DataBlock.UserMobileAccess.Add(mod);
                            }
                        }

                        if (module.ModuloID == (long)Comun.ModFun.ACCESO_TOTAL_SAVING_EMPLAZAMIENTOS ||
                            module.ModuloID == (long)Comun.ModFun.ACCESO_USUARIO_A_SAVING_EMPLAZAMIENTOS ||
                            module.ModuloID == (long)Comun.ModFun.ACCESO_CLIENTE_A_SAVING_EMPLAZAMIENTOS ||
                            module.ModuloID == (long)Comun.ModFun.ACCESO_USUARIO_A_SAVING_EMPLAZAMIENTOS ||
                            module.ModuloID == (long)Comun.ModFun.ACCESO_SOLO_LECTURA_A_SAVING)
                        {
                            bool addToList = true;
                            foreach (MobileModuleAccess val in DataBlock.UserMobileAccess)
                            {
                                addToList = false;
                            }

                        }

                        if (module.ModuloID == (long)Comun.ModFun.ACCESO_TOTAL_UNINSTALL_ADMIN_EMPLAZAMIENTOS ||
                            module.ModuloID == (long)Comun.ModFun.ACCESO_USUARIO_A_UNINSTALL_ADMIN_EMPLAZAMIENTOS ||
                            module.ModuloID == (long)Comun.ModFun.ACCESO_CLIENTE_A_UNINSTALL_ADMIN_EMPLAZAMIENTOS ||
                            module.ModuloID == (long)Comun.ModFun.ACCESO_SOLO_LECTURA_UNINSTALL_ADMIN)
                        {

                            bool addToList = true;
                            foreach (MobileModuleAccess val in DataBlock.UserMobileAccess)
                            {
                                if (val.ModuleID == (long)MobileModule.Uninstallation)
                                {
                                    addToList = false;
                                }
                            }

                            if (addToList)
                            {
                                MobileModuleAccess mod = new MobileModuleAccess();
                                mod.ModuleID = (long)MobileModule.Uninstallation;
                                DataBlock.UserMobileAccess.Add(mod);
                            }
                        }

                        if (module.ModuloID == (long)Comun.ModFun.ACCESO_TOTAL_DOCUMENTAL_EMPLAZAMIENTOS ||
                            module.ModuloID == (long)Comun.ModFun.ACCESO_USUARIO_A_DOCUMENTAL_EMPLAZAMIENTOS ||
                            module.ModuloID == (long)Comun.ModFun.ACCESO_SOLO_LECTURA_A_DOCUMENTAL)
                        {
                            bool addToList = true;
                            foreach (MobileModuleAccess val in DataBlock.UserMobileAccess)
                            {
                                if (val.ModuleID == (long)MobileModule.DocumentsManagement)
                                {
                                    addToList = false;
                                }
                            }

                            if (addToList)
                            {
                                MobileModuleAccess mod = new MobileModuleAccess();
                                mod.ModuleID = (long)MobileModule.DocumentsManagement;
                                DataBlock.UserMobileAccess.Add(mod);
                            }
                        }

                        if (module.ModuloID == (long)Comun.ModFun.ACCESO_TOTAL_LEGALIZACIONES_EMPLAZAMIENTOS ||
                            module.ModuloID == (long)Comun.ModFun.ACCESO_USUARIO_A_LEGALIZACIONES_EMPLAZAMIENTOS ||
                            module.ModuloID == (long)Comun.ModFun.ACCESO_CLIENTE_A_LEGALIZACIONES_EMPLAZAMIENTOS ||
                            module.ModuloID == (long)Comun.ModFun.ACCESO_SOLO_LECTURA_A_LEGALIZACIONES ||
                            module.ModuloID == (long)Comun.ModFun.ACCESO_CLIENTE_A_LEGALIZACIONES_EMPLAZAMIENTOS)
                        {
                            bool addToList = true;
                            foreach (MobileModuleAccess val in DataBlock.UserMobileAccess)
                            {
                                if (val.ModuleID == (long)MobileModule.Legal)
                                {
                                    addToList = false;
                                }
                            }

                            if (addToList)
                            {
                                MobileModuleAccess mod = new MobileModuleAccess();
                                mod.ModuleID = (long)MobileModule.Legal;
                                DataBlock.UserMobileAccess.Add(mod);
                            }

                        }

                        if (module.ModuloID == (long)Comun.ModFun.ACCESO_TOTAL_PROPERTY_EMPLAZAMIENTOS ||
                            module.ModuloID == (long)Comun.ModFun.ACCESO_TOTAL_PROPERTY_EMPLAZAMIENTOS ||
                            module.ModuloID == (long)Comun.ModFun.ACCESO_SOLO_LECTURA_PROPERTY_INCIDENCIAS)
                        {
                            bool addToList = true;
                            foreach (MobileModuleAccess val in DataBlock.UserMobileAccess)
                            {
                                addToList = false;
                            }
                        }

                        if (module.ModuloID == (long)Comun.ModFun.ACCESO_TOTAL_SPACE_EMPLAZAMIENTOS ||
                            module.ModuloID == (long)Comun.ModFun.ACCESO_USUARIO_A_SPACE_EMPLAZAMIENTOS ||
                            module.ModuloID == (long)Comun.ModFun.ACCESO_SOLO_LECTURA_A_SPACE_EMPLAZAMIENTOS)
                        {
                            bool addToList = true;
                            foreach (MobileModuleAccess val in DataBlock.UserMobileAccess)
                            {
                                if (val.ModuleID == (long)MobileModule.SpaceManagement)
                                {
                                    addToList = false;
                                }
                            }

                            if (addToList)
                            {
                                MobileModuleAccess mod = new MobileModuleAccess();
                                mod.ModuleID = (long)MobileModule.SpaceManagement;
                                DataBlock.UserMobileAccess.Add(mod);
                            }
                        }

                        if (module.ModuloID == (long)Comun.ModFun.ACCESO_TOTAL_BILLING_EMPLAZAMIENTOS ||
                            module.ModuloID == (long)Comun.ModFun.ACCESO_USUARIO_A_BILLING_EMPLAZAMIENTOS)
                        {
                            bool addToList = true;
                            foreach (MobileModuleAccess val in DataBlock.UserMobileAccess)
                            {
                                if (val.ModuleID == (long)MobileModule.Invoicing)
                                {
                                    addToList = false;
                                }
                            }

                            if (addToList)
                            {
                                MobileModuleAccess mod = new MobileModuleAccess();
                                mod.ModuleID = (long)MobileModule.Invoicing;
                                DataBlock.UserMobileAccess.Add(mod);
                            }
                        }

                        if (module.ModuloID == (long)Comun.ModFun.ACCESO_TOTAL_TOWER_EMPLAZAMIENTOS ||
                            module.ModuloID == (long)Comun.ModFun.ACCESO_USUARIO_A_TOWER_EMPLAZAMIENTOS ||
                            module.ModuloID == (long)Comun.ModFun.ACCESO_SOLO_LECTURA_A_TOWER_EMPLAZAMIENTOS)
                        {
                            bool addToList = true;
                            foreach (MobileModuleAccess val in DataBlock.UserMobileAccess)
                            {
                                if (val.ModuleID == (long)MobileModule.Towers)
                                {
                                    addToList = false;
                                }
                            }

                            if (addToList)
                            {
                                MobileModuleAccess mod = new MobileModuleAccess();
                                mod.ModuleID = (long)MobileModule.Towers;
                                DataBlock.UserMobileAccess.Add(mod);
                            }
                        }

                        if (module.ModuloID == (long)Comun.ModFun.ACCESO_TOTAL_INDOOR_EMPLAZAMIENTOS ||
                            module.ModuloID == (long)Comun.ModFun.ACCESO_USUARIO_A_INDOOR_EMPLAZAMIENTOS ||
                            module.ModuloID == (long)Comun.ModFun.ACCESO_SOLO_LECTURA_A_INDOOR_EMPLAZAMIENTOS)
                        {
                            bool addToList = true;
                            foreach (MobileModuleAccess val in DataBlock.UserMobileAccess)
                            {
                                if (val.ModuleID == (long)MobileModule.Indoor)
                                {
                                    addToList = false;
                                }
                            }
                        }                      

                        if (module.ModuloID == (long)Comun.ModFun.ACCESO_SOLO_LECTURA_A_INDOOR_EMPLAZAMIENTOS)
                        {                            

                            bool addToList = true;
                            foreach (MobileModuleAccess val in DataBlock.UserMobileAccess)
                            {
                                if (val.ModuleID == (long)MobileModule.Inventory)
                                {
                                    addToList = false;
                                }
                            }

                            if (addToList)
                            {
                                MobileModuleAccess mod = new MobileModuleAccess();
                                mod.ModuleID = (long)MobileModule.Inventory;
                                DataBlock.UserMobileAccess.Add(mod);
                            }
                        }
                    }

                    jsonLoginPackage.RequestResult = true;
                    jsonLoginPackage.RequestResultString = ResHelper.GetString("srv_ok"); 

                    // now encrypt the data block
                    string rawData = JsonConvert.SerializeObject(DataBlock, Formatting.Indented);
                    if( rawData != null &
                        rawData.Length > 0 ) {
                        AESCrypto.StringEncryption strEnc = cryptoHandler.EncryptString(rawData);

                        if (strEnc.encryptedString != null &&
                            strEnc.encryptedString.Length > 0)
                        {
                            jsonLoginPackage.BlockSize = strEnc.originalByteLength;
                            jsonLoginPackage.BlockData = strEnc.encryptedString;
                        }
                        else
                        {
                            jsonLoginPackage.RequestResult = false;
                            jsonLoginPackage.RequestResultString = ResHelper.GetString("srv_auth_error");
                        }
                    }
                }
                else
                {
                    jsonLoginPackage.RequestResult = false;
                    jsonLoginPackage.RequestResultString = ResHelper.GetString("srv_user_invalid_inactive");
                }
            }
            catch (Exception ex)
            {
                System.Console.Write(ex);

                jsonLoginPackage.RequestResult = false;
                jsonLoginPackage.RequestResultString = ResHelper.GetString("srv_exception");
            }

            string jsonOut = JsonConvert.SerializeObject(jsonLoginPackage, Formatting.Indented);

            return jsonOut;
        }
    }
}
