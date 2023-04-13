using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using TreeCore.Data;
using System.Data.SqlClient;

namespace CapaNegocio
{
    public class LicenciasController : GeneralBaseController<Licencias, TreeCoreContext>, IBasica<Licencias>
    {
        public LicenciasController()
            : base()
        { }

        public bool RegistroVinculado(long LicenciaID)
        {
            bool existe = true;
         

            return existe;
        }

        public bool RegistroDuplicado(string codigoLicencia, long clienteID)
        {
            bool isExiste = false;
            List<Licencias> datos = new List<Licencias>();


            datos = (from c in Context.Licencias where (c.CodigoLicencia == codigoLicencia && c.ClienteID == clienteID) select c).ToList<Licencias>();

            if (datos.Count > 0)
            {
                isExiste = true;
            }

            return isExiste;
        }

        public bool RegistroDefecto(long LicenciaID)
        {
            Licencias dato = new Licencias();
            LicenciasController cController = new LicenciasController();
            bool defecto = false;

            dato = cController.GetItem("Defecto == true && LicenciaID == " + LicenciaID.ToString());

            if (dato != null)
            {
                defecto = true;
            }
            else
            {
                defecto = false;
            }

            return defecto;
        }

        #region FUNCIONES GENERALES

        /// <summary>
        /// Checks if the license code is already in use for a given client
        /// </summary>
        /// <param name="codigo">Name of the code to be ckecked</param>
        /// <param name="clienteID">Client id</param>
        /// <returns>True en caso de existir un rol con el nombre indicado y false en caso contrario</returns>
        public bool ExisteCodigo(string codigo, long clienteID)
        {
            bool bRes = false;


            //It takes all the licenses with the same code for the given client
            List<Licencias> roles = (from c in Context.Licencias where c.ClienteID != clienteID && c.CodigoLicencia == codigo select c).ToList();

            if (roles.Count > 0)
            {
                bRes = true;
            }

            return bRes;
        }

        /// <summary>
        /// Obtains all the licenses for a given client
        /// </summary>
        /// <param name="clientID"></param>
        /// <returns></returns>
        public List<Vw_Licencias> GetLicenciasByClient(long clientID)
        {
            List<Vw_Licencias> lista = new List<Vw_Licencias>();
            lista = (from c in Context.Vw_Licencias where (c.ClienteID == clientID) select c).ToList<Vw_Licencias>();
            return lista;
        }
        /// <summary>
        /// Creates a license object from a given code
        /// </summary>
        /// <param name="codigo">The license code</param>
        /// <returns></returns>
        public Licencias ExtractLicenciaFromCodigo(string codigo)
        {
            // Local variables
            Licencias licencia = null;
            string sCodigo = null;
            List<string> lista = new List<string>();
            int i = 0;
            string sAux = null;
            string sControl = null;
            int iParcial = 0;
            string sDiaActivacion = null;
            string sMesActivacion = null;
            string sYearActivacion = null;
            string sDiaCancel = null;
            string sMesCancel = null;
            string sYearCancel = null;
            string sNumeroLicencias = null;
            string sCodigoCliente = null;
            string sTipoLicencia = null;
            string sCRC = null;

            try
            {
                // Process the code
                sCodigo = codigo.Replace(Comun.KEY_SEPARATOR, "");
                // Obtains the CRC Code
                sCRC = sCodigo.Substring(sCodigo.Length - 3, 3);
                sCodigo = sCodigo.Substring(0, sCodigo.Length - 3);
                // Validates the CRC
                char[] chars = sCodigo.ToCharArray();
                int l = 0;
                long lCodigo = 0;
                for (l = 0; l < chars.Length; l = l + 1)
                {
                    lCodigo = lCodigo + chars[l];
                }
                lCodigo = lCodigo % Comun.KEY_LICENSE_CRC_MOD;
                string sCRCCalculado = lCodigo.ToString();
                if (sCRCCalculado.Length > 3)
                {
                    sCRCCalculado = sCRCCalculado.Substring(0, 3);
                }
                else
                {
                    l = sCRCCalculado.Length;
                    while (l < 3)
                    {
                        sCRCCalculado = sCRCCalculado + Comun.KEY_LICENSE_CRC_SUPPLEMENT;
                        l = l + 1;
                    }
                }

                if (!sCRCCalculado.Equals(sCRC))
                {
                    return licencia;
                }

                // Creates the license object
                while (i < sCodigo.Length)
                {
                    sAux = sCodigo.Substring(i, 4);
                    lista.Add(sAux);
                    i = i + 4;
                }

                foreach (string sCampo in lista)
                {
                    // Validates the code
                    switch (sCampo.Substring(0, 2))
                    {
                        case Comun.KEY_ACTIVATION_DAY_CODE:
                            sControl = sCampo.Substring(2, 2);
                            iParcial = int.Parse(sControl);
                            sAux = lista.ElementAt(iParcial);
                            sAux = sAux.Substring(2, 2);
                            iParcial = int.Parse(sAux);
                            iParcial = iParcial - Comun.KEY_ACTIVATION_DATE_DELAY;
                            sDiaActivacion = iParcial.ToString();

                            break;
                        case Comun.KEY_ACTIVATION_MONTH_CODE:
                            sControl = sCampo.Substring(2, 2);
                            iParcial = int.Parse(sControl);
                            sAux = lista.ElementAt(iParcial);
                            sAux = sAux.Substring(2, 2);
                            iParcial = int.Parse(sAux);
                            iParcial = iParcial - Comun.KEY_ACTIVATION_DATE_DELAY;
                            sMesActivacion = iParcial.ToString();
                            break;
                        case Comun.KEY_ACTIVATION_YEAR_CODE:
                            sControl = sCampo.Substring(2, 2);
                            iParcial = int.Parse(sControl);
                            sAux = lista.ElementAt(iParcial);
                            iParcial = int.Parse(sAux);
                            iParcial = iParcial - Comun.KEY_ACTIVATION_DATE_DELAY;
                            sYearActivacion = iParcial.ToString();
                            break;
                        case Comun.KEY_CANCEL_DAY_CODE:
                            sControl = sCampo.Substring(2, 2);
                            iParcial = int.Parse(sControl);
                            sAux = lista.ElementAt(iParcial);
                            sAux = sAux.Substring(2, 2);
                            iParcial = int.Parse(sAux);
                            iParcial = iParcial - Comun.KEY_CANCEL_DATE_DELAY;
                            sDiaCancel = iParcial.ToString();
                            break;
                        case Comun.KEY_CANCEL_MONTH_CODE:
                            sControl = sCampo.Substring(2, 2);
                            iParcial = int.Parse(sControl);
                            sAux = lista.ElementAt(iParcial);
                            sAux = sAux.Substring(2, 2);
                            iParcial = int.Parse(sAux);
                            iParcial = iParcial - Comun.KEY_CANCEL_DATE_DELAY;
                            sMesCancel = iParcial.ToString();
                            break;
                        case Comun.KEY_CANCEL_YEAR_CODE:
                            sControl = sCampo.Substring(2, 2);
                            iParcial = int.Parse(sControl);
                            sAux = lista.ElementAt(iParcial);
                            iParcial = int.Parse(sAux);
                            iParcial = iParcial - Comun.KEY_CANCEL_DATE_DELAY;
                            sYearCancel = iParcial.ToString();
                            break;
                        case Comun.KEY_CLIENT_CODE:
                            sControl = sCampo.Substring(2, 2);
                            iParcial = int.Parse(sControl);
                            sAux = lista.ElementAt(iParcial);
                            iParcial = int.Parse(sAux);
                            iParcial = iParcial - Comun.KEY_CLIENT_CODE_DELAY;
                            sCodigoCliente = iParcial.ToString();
                            break;
                        case Comun.KEY_LICENSES_NUMBER_CODE:
                            sControl = sCampo.Substring(2, 2);
                            iParcial = int.Parse(sControl);
                            sAux = lista.ElementAt(iParcial);
                            iParcial = int.Parse(sAux);
                            iParcial = iParcial - Comun.KEY_LICENSES_NUMBER_DELAY;
                            sNumeroLicencias = iParcial.ToString();
                            break;
                        case Comun.KEY_LICENSE_TYPE_CODE:
                            sControl = sCampo.Substring(2, 2);
                            iParcial = int.Parse(sControl);
                            sAux = lista.ElementAt(iParcial);
                            iParcial = int.Parse(sAux);
                            iParcial = iParcial - Comun.KEY_LICENSE_TYPE_DELAY;
                            sTipoLicencia = iParcial.ToString();
                            break;
                    }
                }

                // Creates the license object
                licencia = new Licencias();
                licencia.CodigoLicencia = codigo;
                licencia.FechaActivacion = new DateTime(int.Parse(sYearActivacion), int.Parse(sMesActivacion), int.Parse(sDiaActivacion));
                licencia.FechaCaducidad = new DateTime(int.Parse(sYearCancel), int.Parse(sMesCancel), int.Parse(sDiaCancel));
                licencia.NumeroLicencias = int.Parse(sNumeroLicencias);
                licencia.ClienteID = long.Parse(sCodigoCliente);
                licencia.LicenciaTipoID = long.Parse(sTipoLicencia);
                if (licencia.FechaCaducidad.CompareTo(DateTime.Now) > 0)
                {
                    licencia.Activa = true;
                }
                else
                {
                    licencia.Activa = false;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                licencia = null;
            }

            // Returns the desencrypted license
            return licencia;
        }
        /// <summary>
        /// Creates the code from a given license
        /// </summary>
        /// <param name="licencia">The license from which the code is generated</param>
        /// <returns></returns>
        public string CreateCodeFromLicencia(Licencias licencia)
        {
            // Local variables
            string sRes = null;
            string sCodigo = null;
            List<string> lista = new List<string>();
            List<string> listaCodigo = new List<string>();
            //int i = 0;
            string sAux = null;
            //string sControl = null;
            int iParcial = 0;
            string sDiaActivacion = null;
            string sMesActivacion = null;
            string sYearActivacion = null;
            string sDiaCancel = null;
            string sMesCancel = null;
            string sYearCancel = null;
            string sNumeroLicencias = null;
            string sCodigoCliente = null;
            string sTipoLicencia = null;
            Random aleatorio = null;
            int iAleatorio = 0;
            string alfabeto = "ABCDEFGHIJKLMNÑOPQRSTUWXYZ0123456789";
            List<int> orden = null;
            List<int> ordenFinal = null;
            int iPosicionClave = 0;
            int iPosicionValor = 0;

            // Initializes variables
            aleatorio = new Random();

            // Fill the order list
            orden = new List<int>();
            ordenFinal = new List<int>();
            for (int k = 0; k < 18; k = k + 1)
            {
                if (k < 9)
                {
                    orden.Add(k);
                }
                ordenFinal.Add(k);
                listaCodigo.Add("");
            }


            // Creates the elements
            iParcial = licencia.FechaActivacion.Day;
            iParcial = iParcial + Comun.KEY_ACTIVATION_DATE_DELAY;
            sAux = iParcial.ToString();
            if (sAux.Length < 2)
            {
                sAux = "0" + sAux;
            }
            iAleatorio = aleatorio.Next(alfabeto.Length);
            sAux = alfabeto.Substring(iAleatorio, 1) + sAux;
            iAleatorio = aleatorio.Next(alfabeto.Length);
            sAux = alfabeto.Substring(iAleatorio, 1) + sAux;
            sDiaActivacion = sAux;
            lista.Add(sDiaActivacion);

            iParcial = licencia.FechaActivacion.Month;
            iParcial = iParcial + Comun.KEY_ACTIVATION_DATE_DELAY;
            sAux = iParcial.ToString();
            if (sAux.Length < 2)
            {
                sAux = "0" + sAux;
            }
            iAleatorio = aleatorio.Next(alfabeto.Length);
            sAux = alfabeto.Substring(iAleatorio, 1) + sAux;
            iAleatorio = aleatorio.Next(alfabeto.Length);
            sAux = alfabeto.Substring(iAleatorio, 1) + sAux;
            sMesActivacion = sAux;
            lista.Add(sMesActivacion);

            iParcial = licencia.FechaActivacion.Year;
            iParcial = iParcial + Comun.KEY_ACTIVATION_DATE_DELAY;
            sAux = iParcial.ToString();
            sYearActivacion = sAux;
            lista.Add(sYearActivacion);

            iParcial = licencia.FechaCaducidad.Day;
            iParcial = iParcial + Comun.KEY_CANCEL_DATE_DELAY;
            sAux = iParcial.ToString();
            if (sAux.Length < 2)
            {
                sAux = "0" + sAux;
            }
            iAleatorio = aleatorio.Next(alfabeto.Length);
            sAux = alfabeto.Substring(iAleatorio, 1) + sAux;
            iAleatorio = aleatorio.Next(alfabeto.Length);
            sAux = alfabeto.Substring(iAleatorio, 1) + sAux;
            sDiaCancel = sAux;
            lista.Add(sDiaCancel);

            iParcial = licencia.FechaCaducidad.Month;
            iParcial = iParcial + Comun.KEY_CANCEL_DATE_DELAY;
            sAux = iParcial.ToString();
            if (sAux.Length < 2)
            {
                sAux = "0" + sAux;
            }
            iAleatorio = aleatorio.Next(alfabeto.Length);
            sAux = alfabeto.Substring(iAleatorio, 1) + sAux;
            iAleatorio = aleatorio.Next(alfabeto.Length);
            sAux = alfabeto.Substring(iAleatorio, 1) + sAux;
            sMesCancel = sAux;
            lista.Add(sMesCancel);

            iParcial = licencia.FechaCaducidad.Year;
            iParcial = iParcial + Comun.KEY_CANCEL_DATE_DELAY;
            sAux = iParcial.ToString();
            sYearCancel = sAux;
            lista.Add(sYearCancel);

            // License number
            iParcial = licencia.NumeroLicencias + Comun.KEY_LICENSES_NUMBER_DELAY;
            sAux = iParcial.ToString();
            while (sAux.Length < 4)
            {
                sAux = "0" + sAux;
            }
            sNumeroLicencias = sAux;
            lista.Add(sNumeroLicencias);

            iParcial = (int)licencia.ClienteID + Comun.KEY_CLIENT_CODE_DELAY;
            sAux = iParcial.ToString();
            while (sAux.Length < 4)
            {
                sAux = "0" + sAux;
            }
            sCodigoCliente = sAux;
            lista.Add(sCodigoCliente);

            iParcial = (int)licencia.LicenciaTipoID + Comun.KEY_LICENSE_TYPE_DELAY;
            sAux = iParcial.ToString();
            while (sAux.Length < 4)
            {
                sAux = "0" + sAux;
            }
            sTipoLicencia = sAux;
            lista.Add(sTipoLicencia);

            while (orden.Count > 0)
            {
                iParcial = aleatorio.Next(orden.Count - 1);
                iParcial = orden.ElementAt(iParcial);
                orden.Remove(iParcial);
                iPosicionClave = aleatorio.Next(ordenFinal.Count - 1);
                iPosicionClave = ordenFinal.ElementAt(iPosicionClave);
                ordenFinal.Remove(iPosicionClave);
                iPosicionValor = aleatorio.Next(ordenFinal.Count - 1);
                iPosicionValor = ordenFinal.ElementAt(iPosicionValor);
                ordenFinal.Remove(iPosicionValor);

                switch (iParcial)
                {
                    // Activation day
                    case 0:
                        sAux = iPosicionValor.ToString();
                        if (sAux.Length < 2)
                        {
                            sAux = "0" + sAux;
                        }
                        sAux = Comun.KEY_ACTIVATION_DAY_CODE + sAux;
                        listaCodigo[iPosicionClave] = sAux;
                        listaCodigo[iPosicionValor] = sDiaActivacion;
                        break;
                    // Activation Month
                    case 1:
                        sAux = iPosicionValor.ToString();
                        if (sAux.Length < 2)
                        {
                            sAux = "0" + sAux;
                        }
                        sAux = Comun.KEY_ACTIVATION_MONTH_CODE + sAux;
                        listaCodigo[iPosicionClave] = sAux;
                        listaCodigo[iPosicionValor] = sMesActivacion;
                        break;
                    // Activation Year
                    case 2:
                        sAux = iPosicionValor.ToString();
                        if (sAux.Length < 2)
                        {
                            sAux = "0" + sAux;
                        }
                        sAux = Comun.KEY_ACTIVATION_YEAR_CODE + sAux;
                        listaCodigo[iPosicionClave] = sAux;
                        listaCodigo[iPosicionValor] = sYearActivacion;
                        break;
                    // Cancel day
                    case 3:
                        sAux = iPosicionValor.ToString();
                        if (sAux.Length < 2)
                        {
                            sAux = "0" + sAux;
                        }
                        sAux = Comun.KEY_CANCEL_DAY_CODE + sAux;
                        listaCodigo[iPosicionClave] = sAux;
                        listaCodigo[iPosicionValor] = sDiaCancel;
                        break;
                    // Cancel Month
                    case 4:
                        sAux = iPosicionValor.ToString();
                        if (sAux.Length < 2)
                        {
                            sAux = "0" + sAux;
                        }
                        sAux = Comun.KEY_CANCEL_MONTH_CODE + sAux;
                        listaCodigo[iPosicionClave] = sAux;
                        listaCodigo[iPosicionValor] = sMesCancel;
                        break;
                    // Cancel Year
                    case 5:
                        sAux = iPosicionValor.ToString();
                        if (sAux.Length < 2)
                        {
                            sAux = "0" + sAux;
                        }
                        sAux = Comun.KEY_CANCEL_YEAR_CODE + sAux;
                        listaCodigo[iPosicionClave] = sAux;
                        listaCodigo[iPosicionValor] = sYearCancel;
                        break;
                    // License number
                    case 6:
                        sAux = iPosicionValor.ToString();
                        if (sAux.Length < 2)
                        {
                            sAux = "0" + sAux;
                        }
                        sAux = Comun.KEY_LICENSES_NUMBER_CODE + sAux;
                        listaCodigo[iPosicionClave] = sAux;
                        listaCodigo[iPosicionValor] = sNumeroLicencias;
                        break;
                    // Client code
                    case 7:
                        sAux = iPosicionValor.ToString();
                        if (sAux.Length < 2)
                        {
                            sAux = "0" + sAux;
                        }
                        sAux = Comun.KEY_CLIENT_CODE + sAux;
                        listaCodigo[iPosicionClave] = sAux;
                        listaCodigo[iPosicionValor] = sCodigoCliente;
                        break;

                    // License type
                    case 8:
                        sAux = iPosicionValor.ToString();
                        if (sAux.Length < 2)
                        {
                            sAux = "0" + sAux;
                        }
                        sAux = Comun.KEY_LICENSE_TYPE_CODE + sAux;
                        listaCodigo[iPosicionClave] = sAux;
                        listaCodigo[iPosicionValor] = sTipoLicencia;
                        break;
                }

            }

            // Creates the code
            sCodigo = "";
            foreach (string valor in listaCodigo)
            {
                sCodigo = sCodigo + valor;
                //sCodigo = sCodigo + Comun.KEY_SEPARATOR;
            }
            // Calculate the CRC (3 characters)
            char[] chars = sCodigo.ToCharArray();
            int l = 0;
            long lCodigo = 0;
            for (l = 0; l < chars.Length; l = l + 1)
            {
                lCodigo = lCodigo + chars[l];
            }
            lCodigo = lCodigo % Comun.KEY_LICENSE_CRC_MOD;
            string sCRC = lCodigo.ToString();
            if (sCRC.Length > 3)
            {
                sCodigo = sCodigo + sCRC.Substring(0, 3);
            }
            else
            {
                l = sCRC.Length;
                while (l < 3)
                {
                    sCRC = sCRC + Comun.KEY_LICENSE_CRC_SUPPLEMENT;
                    l = l + 1;
                }
                sCodigo = sCodigo + sCRC;
            }
            int iSepara = 0;
            sRes = "";
            while (iSepara < sCodigo.Length)
            {
                sRes = sRes + sCodigo.Substring(iSepara, 15);
                sRes = sRes + Comun.KEY_SEPARATOR;
                iSepara = iSepara + 15;
            }
            sRes = sRes.Substring(0, sRes.Length - 1);

            // Returns the code
            return sRes;
        }

        public List<long> GetUsuriosPerfilDestinatarios(long perfilID)
        {
            List<long> Usuarios = new List<long>();
            Usuarios = (from c in Context.UsuariosPerfiles where (perfilID == 0) || (c.PerfilID == perfilID) select c.UsuarioID).Distinct<long>().ToList<long>();
            return Usuarios;
        }

        public List<long> GetUsuriosDestinatariosByPerfilesList(List<long> perfilesIDs)
        {
            List<long> Usuarios = new List<long>();
            Usuarios = (from c in Context.UsuariosPerfiles where perfilesIDs.Contains(c.PerfilID) select c.UsuarioID).Distinct<long>().ToList<long>();
            return Usuarios;
        }

        // Verifies if it is a license active for the given environment
        public bool hasLicenciaActiva(long clienteLocalID, long sLicenciaTipo)
        {
            // Local variables
            bool bRes = false;
            List<Licencias> lista = new List<Licencias>();
            Licencias licenciaLocal = null;
            Licencias licenciaCalculada = null;

            // Takes the current license
            lista = (from c in Context.Licencias where (c.ClienteID == clienteLocalID && c.Activa && c.LicenciaTipoID == sLicenciaTipo) select c).ToList<Licencias>();
            if (lista != null && lista.Count > 0)
            {
                licenciaLocal = lista.ElementAt(0);
                licenciaCalculada = ExtractLicenciaFromCodigo(licenciaLocal.CodigoLicencia);
                if (licenciaCalculada.FechaCaducidad >= DateTime.Now)
                {
                    bRes = true;
                }
                else
                {
                    licenciaLocal.Activa = false;
                    UpdateItem(licenciaLocal);
                }
            }

            // Returns the result
            return bRes;
        }

        #endregion

        public Licencias GetDefault(long clienteID)
        {
            Licencias oLicencia;
            try
            {
                oLicencia = (from c in Context.Licencias where c.Defecto && c.ClienteID == clienteID select c).First();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                oLicencia = null;
            }

            return oLicencia;
        }


    }
}