using CapaNegocio;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TreeAPI.API.Mobile.Crypto;
using TreeAPI.API.Mobile.General;
using TreeAPI.API.Mobile.Login.JSON;
using TreeCore.Data;

namespace TreeAPI.API.Mobile.Login
{

    public class LoginRequestSOAPV3
    {
        public string Platform;
        public string Locale;
        public string UserLogin;
        public string Password;
        public string UniqueDeviceID;
        public string GeneratedCodeDevice;
        public string FCMKey;
        public string UserMAC;

        public long ULLength;
        public long PLength;
    }

    public class MobileLoginV3Handler
    {

        private static Random random = new Random();

        // THIS MUST MATCH THE CLIENT
        public enum MobileModule
        {
            None = 0,
            AcquisitionsManagement = 1,
            Energy = 2,
            Sharing = 3,
            Renegotiation = 4,
            Towers = 5,
            Indoor = 6,
            AcquisitionsEndToEnd = 7,
            Installation = 8,
            Legal = 9,
            Maintenance = 10,
            SpaceManagement = 11,
            Uninstallation = 12,
            Access = 13,
            Audit = 14,
            DocumentsManagement = 15,
            Environment = 16,
            Incidents = 17,
            Regulations = 18,
            Exchange = 19,
            Invoicing = 20,
            Inventory = 21,
            //FUNCIONALIDADES
            DobleFactor = 22,
            ValidacionDocumentos = 23
        }

        public string Process(System.Web.SessionState.HttpSessionState Session, CryptoHandler cryptoHandler, LoginRequestSOAPV3 req)
        {
            //const int SUPER_USER_CLIENT_ID = 14;
            Locale appLocale = new Locale();

            string selectedLocale = appLocale.MobileLocale(req.Platform, req.Locale);

            // set the session locale
            Session["LOCALE"] = selectedLocale;

            MobileResourceRequester ResHelper = new MobileResourceRequester(Resources.MobileResources_GIS.ResourceManager, selectedLocale);

            // try and find the user
            Vw_UsuariosAPI us = new Vw_UsuariosAPI();
            UsuariosController cController = new UsuariosController();
            ParametrosController cParametros = new ParametrosController();

            LoginMobileSessionPackageV3 jsonLoginPackage = new LoginMobileSessionPackageV3();
            jsonLoginPackage.RequestResult = false;
            jsonLoginPackage.RequestResultString = ResHelper.GetString("srv_error");
            jsonLoginPackage.BlockSize = 0;
            jsonLoginPackage.BlockData = "";

            UserMobileSession DataBlock = new UserMobileSession();

            AESCrypto.StringDecryption strOut = cryptoHandler.DecryptString(req.UserLogin, req.ULLength);
            if (strOut.decryptedString != null &&
                strOut.decryptedString.Length > 0)
                req.UserLogin = strOut.decryptedString;

            strOut = cryptoHandler.DecryptString(req.Password, req.PLength);
            if (strOut.decryptedString != null &&
                strOut.decryptedString.Length > 0)
                req.Password = strOut.decryptedString;

            //bool bIntegracion = false;
            try
            {
                ToolIntegracionesServiciosMetodosController cIntegra = new ToolIntegracionesServiciosMetodosController();
                Vw_ToolServicios dato = null;
                string sDominioABuscar = "";
                // Searches for an integration for the login service
                dato = cIntegra.GetIntegracionActivaByNombreServicio(Comun.INTEGRACION_SERVICIO_IDENTIFICACION);


                //INTEGRACIÓN LDAP ACTIVA Y EL USUARIO NO ES EL SUPER
                if (dato != null && !req.UserLogin.Equals(Comun.TREE_SUPER_USER))
                {

                    if (req.UserLogin.Contains("\\"))
                    {
                        sDominioABuscar = req.UserLogin.Substring(0, req.UserLogin.LastIndexOf("\\"));

                    }


                    string sIntegracion = dato.Integracion;
                    string sNombreClase = dato.NombreClase;
                    Type type = Type.GetType(sNombreClase);
                    //var myObj = Activator.CreateInstance(type, sDominioABuscar);

                    if (type != null)
                    {
                        MethodInfo methodInfo = type.GetMethod(dato.Metodo);

                        if (methodInfo != null)
                        {
                            object result = null;
                            ParameterInfo[] parameters = methodInfo.GetParameters();
                            object classInstance = Activator.CreateInstance(type, sDominioABuscar);

                            if (parameters.Length == 0)
                            {
                                // This works fine
                                result = methodInfo.Invoke(classInstance, null);
                            }
                            else
                            {
                                object[] parametersArray = new object[] { req.UserLogin, req.Password };

                                // The invoke does NOT work;
                                // it throws "Object does not match target type"             
                                result = methodInfo.Invoke(classInstance, parametersArray);
                                if (result != null)
                                {
                                    Usuarios user = (Usuarios)result;
                                    us = cController.getUsuarioApi(user.UsuarioID);

                                    if (us.EMail != Comun.TREE_SUPER_USER)
                                    {
                                        if (string.IsNullOrEmpty(req.GeneratedCodeDevice))
                                        {
                                            req.GeneratedCodeDevice = GenerarGeneratedCodeDevice(req.UniqueDeviceID);
                                        }
                                        
                                        us = cController.ModificarMAC(us.UsuarioID, req.GeneratedCodeDevice);
                                        if (us != null)
                                        {
                                            jsonLoginPackage.GeneratedCodeDevice = us.MacDispositivo;
                                        }
                                    }

                                }
                                else
                                {
                                    us = null;
                                }
                            }
                        }
                    }

                }
                else
                {
                    //Integración login no activa
                    if (string.IsNullOrEmpty(req.GeneratedCodeDevice))
                    {
                        req.GeneratedCodeDevice = GenerarGeneratedCodeDevice(req.UniqueDeviceID);
                    }

                    us = cController.UsuariosLoginAPIV3(req.UserLogin, req.Password, req.GeneratedCodeDevice);
                    if (us != null)
                    {
                        jsonLoginPackage.GeneratedCodeDevice = us.MacDispositivo;
                    }
                }


                if (us != null)
                {
                    if (us.Activo)
                    {

                        ModulosController moduleController = new ModulosController();
                        List<Modulos> moduleList = moduleController.getModulosObj(us.UsuarioID);
                        FuncionalidadesController cFuncionalidades = new FuncionalidadesController();
                        List<long> Funcionalidades = cFuncionalidades.getFuncionalidades(us.UsuarioID);

                        DataBlock = new UserMobileSession();
                        DataBlock.Locale = req.Locale;
                        DataBlock.LoginTime = DateTimeOffset.Now.ToString("o");
                        DataBlock.UserName = us.Nombre + " " + us.Apellidos;
                        DataBlock.UserID = us.UsuarioID;
                        DataBlock.Email = us.EMail;

                        DataBlock.UserMobileAccess = new List<MobileModuleAccess>();

                        foreach (Modulos module in moduleList)
                        {
                            if (Funcionalidades.Contains((long)Comun.ModFun.ACCESO_TOTAL_SHARING_EMPLAZAMIENTOS) ||
                                Funcionalidades.Contains((long)Comun.ModFun.ACCESO_USUARIO_A_SHARING_EMPLAZAMIENTOS) ||
                                Funcionalidades.Contains((long)Comun.ModFun.ACCESO_CLIENTE_A_SHARING_EMPLAZAMIENTOS) ||
                                Funcionalidades.Contains((long)Comun.ModFun.ACCESO_SOLO_LECTURA_A_SHARING))
                            {
                                //btnSharing.Disabled = false;
                                //btnSharing.Hidden = false;
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

                            if (Funcionalidades.Contains((long)Comun.ModFun.ACCESO_TOTAL_ACCESS_EMPLAZAMIENTOS) ||
                                Funcionalidades.Contains((long)Comun.ModFun.ACCESO_USUARIO_A_ACCESS_EMPLAZAMIENTOS) ||
                                Funcionalidades.Contains((long)Comun.ModFun.ACCESO_CLIENTE_A_ACCESS_EMPLAZAMIENTOS) ||
                                Funcionalidades.Contains((long)Comun.ModFun.ACCESO_SOLO_LECTURA_A_ACCESS))
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

                            if (Funcionalidades.Contains((long)Comun.ModFun.ACCESO_TOTAL_ADQUISICIONES_EMPLAZAMIENTOS) ||
                                Funcionalidades.Contains((long)Comun.ModFun.ACCESO_TOTAL_A_ADQUISICIONES_DBI) ||
                                Funcionalidades.Contains((long)Comun.ModFun.ACCESO_A_ADQUISICIONES_MODULO) ||
                                Funcionalidades.Contains((long)Comun.ModFun.ACCESO_CLIENTE_A_ADQUISICIONES_EMPLAZAMIENTOS) ||
                                Funcionalidades.Contains((long)Comun.ModFun.ACCESO_USUARIO_A_ADQUISICIONES_EMPLAZAMIENTOS) ||
                                Funcionalidades.Contains((long)Comun.ModFun.ACCESO_SOLO_LECTURA_A_ADQUISICIONES))
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

                            if (Funcionalidades.Contains((long)Comun.ModFun.ACCESO_TOTAL_AUDIT_EMPLAZAMIENTOS) ||
                                Funcionalidades.Contains((long)Comun.ModFun.ACCESO_USUARIO_A_AUDIT_EMPLAZAMIENTOS) ||
                                Funcionalidades.Contains((long)Comun.ModFun.ACCESO_SOLO_LECTURA_A_AUDIT))
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

                            if (Funcionalidades.Contains((long)Comun.ModFun.ACCESO_TOTAL_ENERGY_EMPLAZAMIENTOS) ||
                                Funcionalidades.Contains((long)Comun.ModFun.ACCESO_USUARIO_A_ENERGY_EMPLAZAMIENTOS) ||
                                Funcionalidades.Contains((long)Comun.ModFun.ACCESO_CLIENTE_A_ENERGY_EMPLAZAMIENTOS) ||
                                Funcionalidades.Contains((long)Comun.ModFun.ACCESO_SOLO_LECTURA_A_ENERGY))
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

                            if (Funcionalidades.Contains((long)Comun.ModFun.ACCESO_TOTAL_INSTALL_TECNICA_EMPLAZAMIENTOS) ||
                                Funcionalidades.Contains((long)Comun.ModFun.ACCESO_USUARIO_INSTALL_TECNICA_EMPLAZAMIENTOS) ||
                                Funcionalidades.Contains((long)Comun.ModFun.ACCESO_CLIENTE_INSTALL_TECNICA_EMPLAZAMIENTOS) ||
                                Funcionalidades.Contains((long)Comun.ModFun.ACCESO_SOLO_LECTURA_A_INSTALL_TECNICA) ||
                                Funcionalidades.Contains((long)Comun.ModFun.ACCESO_TOTAL_A_INSTALL_OBRA_CIVIL_EMPLAZAMIENTOS) ||
                                Funcionalidades.Contains((long)Comun.ModFun.ACCESO_RESTRINGIDA_A_INSTALL_OBRA_CIVIL_EMPLAZAMIENTOS) ||
                                Funcionalidades.Contains((long)Comun.ModFun.ACCESO_USUARIO_A_INSTALL_OBRA_CIVIL_EMPLAZAMIENTOS) ||
                                Funcionalidades.Contains((long)Comun.ModFun.ACCESO_CLIENTE_A_INSTALL_OBRA_CIVIL_EMPLAZAMIENTOS) ||
                                Funcionalidades.Contains((long)Comun.ModFun.ACCESO_SOLO_LECTURA_A_INSTALL_OBRA_CIVIL))
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

                            if (Funcionalidades.Contains((long)Comun.ModFun.ACCESO_TOTAL_MANTENIMIENTO_EMPLAZAMIENTOS) ||
                                Funcionalidades.Contains((long)Comun.ModFun.ACCESO_USUARIO_MANTENIMIENTO_EMPLAZAMIENTOS) ||
                                Funcionalidades.Contains((long)Comun.ModFun.ACCESO_CLIENTE_A_MANTENIMIENTO_EMPLAZAMIENTOS) ||
                                Funcionalidades.Contains((long)Comun.ModFun.ACCESO_SOLO_LECTURA_A_MANTENIMIENTO))
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

                            if (Funcionalidades.Contains((long)Comun.ModFun.ACCESO_TOTAL_A_MANTENIMIENTO_EMPLAZAMIENTOS_CORRECTIVOS) ||
                                Funcionalidades.Contains((long)Comun.ModFun.ACCESO_USUARIO_MANTENIMIENTO_EMPLAZAMIENTOS_CORRECTIVOS) ||
                                Funcionalidades.Contains((long)Comun.ModFun.ACCESO_CLIENTE_A_MANTENIMIENTO_EMPLAZAMIENTOS) ||
                                Funcionalidades.Contains((long)Comun.ModFun.ACCESO_CLIENTE_A_MANTENIMIENTO_EMPLAZAMIENTOS_CORRECTIVOS))
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

                            if (Funcionalidades.Contains((long)Comun.ModFun.ACCESO_TOTAL_SAVING_EMPLAZAMIENTOS) ||
                                Funcionalidades.Contains((long)Comun.ModFun.ACCESO_USUARIO_A_SAVING_EMPLAZAMIENTOS) ||
                                Funcionalidades.Contains((long)Comun.ModFun.ACCESO_CLIENTE_A_SAVING_EMPLAZAMIENTOS) ||
                                Funcionalidades.Contains((long)Comun.ModFun.ACCESO_USUARIO_A_SAVING_EMPLAZAMIENTOS) ||
                                Funcionalidades.Contains((long)Comun.ModFun.ACCESO_SOLO_LECTURA_A_SAVING))
                            {


                                bool addToList = true;
                                foreach (MobileModuleAccess val in DataBlock.UserMobileAccess)
                                {
                                    //if (val.ModuleID == (long)MobileModule.Saving)
                                    {
                                        addToList = false;
                                    }
                                }
                            }

                            if (Funcionalidades.Contains((long)Comun.ModFun.ACCESO_TOTAL_UNINSTALL_ADMIN_EMPLAZAMIENTOS) ||
                                Funcionalidades.Contains((long)Comun.ModFun.ACCESO_USUARIO_A_UNINSTALL_ADMIN_EMPLAZAMIENTOS) ||
                                Funcionalidades.Contains((long)Comun.ModFun.ACCESO_CLIENTE_A_UNINSTALL_ADMIN_EMPLAZAMIENTOS) ||
                                Funcionalidades.Contains((long)Comun.ModFun.ACCESO_SOLO_LECTURA_UNINSTALL_ADMIN))
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

                            if (Funcionalidades.Contains((long)Comun.ModFun.ACCESO_TOTAL_DOCUMENTAL_EMPLAZAMIENTOS) ||
                                Funcionalidades.Contains((long)Comun.ModFun.ACCESO_USUARIO_A_DOCUMENTAL_EMPLAZAMIENTOS) ||
                                Funcionalidades.Contains((long)Comun.ModFun.ACCESO_SOLO_LECTURA_A_DOCUMENTAL))
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

                            if (Funcionalidades.Contains((long)Comun.ModFun.ACCESO_TOTAL_LEGALIZACIONES_EMPLAZAMIENTOS) ||
                                Funcionalidades.Contains((long)Comun.ModFun.ACCESO_USUARIO_A_LEGALIZACIONES_EMPLAZAMIENTOS) ||
                                Funcionalidades.Contains((long)Comun.ModFun.ACCESO_CLIENTE_A_LEGALIZACIONES_EMPLAZAMIENTOS) ||
                                Funcionalidades.Contains((long)Comun.ModFun.ACCESO_SOLO_LECTURA_A_LEGALIZACIONES) ||
                                Funcionalidades.Contains((long)Comun.ModFun.ACCESO_CLIENTE_A_LEGALIZACIONES_EMPLAZAMIENTOS))
                            {
                                //btnLegalizaciones.Disabled = false;
                                //btnLegalizaciones.Hidden = false;

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

                            if (Funcionalidades.Contains((long)Comun.ModFun.ACCESO_TOTAL_PROPERTY_EMPLAZAMIENTOS) ||
                                Funcionalidades.Contains((long)Comun.ModFun.ACCESO_TOTAL_PROPERTY_EMPLAZAMIENTOS) ||
                                Funcionalidades.Contains((long)Comun.ModFun.ACCESO_SOLO_LECTURA_PROPERTY_INCIDENCIAS))
                            {
                                //btnPropietarios.Disabled = false;
                                //btnPropietarios.Hidden = false;

                                bool addToList = true;
                                foreach (MobileModuleAccess val in DataBlock.UserMobileAccess)
                                {
                                    //if (val.ModuleID == (long)MobileModule.Property)
                                    {
                                        addToList = false;
                                    }
                                }

                                if (addToList)
                                {
                                    //MobileModuleAccess mod = new MobileModuleAccess();
                                    //mod.ModuleID = (long)MobileModule.Property;
                                    //jsonLoginPackage.UserMobileSession.UserMobileAccess.Add(mod);
                                }
                            }

                            if (Funcionalidades.Contains((long)Comun.ModFun.ACCESO_TOTAL_SPACE_EMPLAZAMIENTOS) ||
                                Funcionalidades.Contains((long)Comun.ModFun.ACCESO_USUARIO_A_SPACE_EMPLAZAMIENTOS) ||
                                Funcionalidades.Contains((long)Comun.ModFun.ACCESO_SOLO_LECTURA_A_SPACE_EMPLAZAMIENTOS))
                            {
                                //btnSpaces.Disabled = false;
                                //btnSpaces.Hidden = false;

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

                            if (Funcionalidades.Contains((long)Comun.ModFun.ACCESO_TOTAL_BILLING_EMPLAZAMIENTOS) ||
                                Funcionalidades.Contains((long)Comun.ModFun.ACCESO_USUARIO_A_BILLING_EMPLAZAMIENTOS) ||
                                Funcionalidades.Contains((long)Comun.ModFun.ACCESO_SOLO_LECTURA_A_BILLING))
                            {
                                //btnFacturacion.Disabled = false;
                                //btnFacturacion.Hidden = false;

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

                            if (Funcionalidades.Contains((long)Comun.ModFun.ACCESO_TOTAL_TOWER_EMPLAZAMIENTOS) ||
                                Funcionalidades.Contains((long)Comun.ModFun.ACCESO_USUARIO_A_TOWER_EMPLAZAMIENTOS) ||
                                Funcionalidades.Contains((long)Comun.ModFun.ACCESO_SOLO_LECTURA_A_TOWER_EMPLAZAMIENTOS))
                            {
                                //btnTorreros.Disabled = false;
                                //btnTorreros.Hidden = false;

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

                            if (Funcionalidades.Contains((long)Comun.ModFun.ACCESO_TOTAL_INDOOR_EMPLAZAMIENTOS) ||
                                Funcionalidades.Contains((long)Comun.ModFun.ACCESO_USUARIO_A_INDOOR_EMPLAZAMIENTOS) ||
                                Funcionalidades.Contains((long)Comun.ModFun.ACCESO_SOLO_LECTURA_A_INDOOR_EMPLAZAMIENTOS))
                            {
                                //btnIndoor.Disabled = false;
                                //btnIndoor.Hidden = false;

                                bool addToList = true;
                                foreach (MobileModuleAccess val in DataBlock.UserMobileAccess)
                                {
                                    if (val.ModuleID == (long)MobileModule.Indoor)
                                    {
                                        addToList = false;
                                    }
                                }

                                if (addToList)
                                {
                                    //MobileModuleAccess mod = new MobileModuleAccess();
                                    //mod.ModuleID = (long)MobileModule.Indoor;
                                    //jsonLoginPackage.UserMobileSession.UserMobileAccess.Add(mod);
                                }
                            }

                            if (Funcionalidades.Contains((long)Comun.ModFun.ACCESO_TOTAL_INVENTARIO_EMPLAZAMIENTOS) ||
                                Funcionalidades.Contains((long)Comun.ModFun.ACCESO_USUARIO_A_INVENTARIO_EMPLAZAMIENTOS) ||
                                Funcionalidades.Contains((long)Comun.ModFun.ACCESO_SOLO_LECTURA_A_INDOOR_EMPLAZAMIENTOS) ||
                                Funcionalidades.Contains((long)Comun.ModFun.ACCESO_CLIENTE_A_INVENTARIO_EMPLAZAMIENTOS))
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

                        //FUNCIONALIDADES
                        Parametros paramDobleFactor = new Parametros();
                        paramDobleFactor = cParametros.GetItemByName("DOBLE_FACTOR");

                        if (paramDobleFactor != null)
                        {
                            if (paramDobleFactor.Valor.ToUpper().Equals("SI") || paramDobleFactor.Valor.ToUpper().Equals("YES"))
                            {
                                MobileModuleAccess mod = new MobileModuleAccess();
                                mod.ModuleID = (long)MobileModule.DobleFactor;
                                DataBlock.UserMobileAccess.Add(mod);
                            }
                        }

                        if (Funcionalidades.Contains((long)Comun.ModFun.APP_ACCESO_VALIDACIONES))
                        {
                            MobileModuleAccess mod = new MobileModuleAccess();
                            mod.ModuleID = (long)MobileModule.ValidacionDocumentos;
                            DataBlock.UserMobileAccess.Add(mod);
                        }


                        jsonLoginPackage.RequestResult = true;

                        if (us.MacDispositivo == null)
                        {
                            jsonLoginPackage.RequestResult = false;
                            jsonLoginPackage.RequestResultString = ResHelper.GetString("srv_auth_error_mac");
                        }
                        else
                        {
                            jsonLoginPackage.RequestResultString = ResHelper.GetString("srv_ok");
                        }

                        // now encrypt the data block
                        string rawData = JsonConvert.SerializeObject(DataBlock, Formatting.Indented);
                        if (rawData != null &
                            rawData.Length > 0)
                        {
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

        private string GenerarGeneratedCodeDevice(string uniqueDeviceID)
        {
            string codeDevice;
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            codeDevice = new string(Enumerable.Repeat(chars, 20)
              .Select(s => s[random.Next(s.Length)]).ToArray());

            codeDevice += uniqueDeviceID;

            return codeDevice;
        }
    }
}
