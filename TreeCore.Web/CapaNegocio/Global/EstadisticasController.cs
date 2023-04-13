using System;
using System.Data;
using TreeCore.Data;
using System.Collections.Generic;
using Tree.Linq.GenericExtensions;
using System.Linq;

namespace CapaNegocio
{
    public sealed class EstadisticasController : GeneralBaseController<Estadisticas, TreeCoreContext>
    {
        public EstadisticasController()
            : base()
        {

        }

        /// <summary>
        /// Registra en Global/Estadísticas las exportaciones realizadas (TipoExcel = EXCEL, GLOBAL ó IFRS16)
        /// </summary>
        public void registrarDescargaExcel(long pUsuarioID, long? pCliente, long pModulo, string pPagina, string pTipoExcel, string pPaisIdioma)
        {
            EstadisticasController cEstadisticas = new EstadisticasController();
            Estadisticas Estadistica = new Estadisticas();
            string comentarioTraducido = "";
            string nombreRecurso = "";

            try
            {
                Estadistica.UsuarioID = pUsuarioID;
                Estadistica.FechaHoraInicio = DateTime.Now;
                Estadistica.Pagina = pPagina;
                Estadistica.ProyectoTipoID = pModulo;
                Estadistica.ClienteID = pCliente;

                if (pTipoExcel == Comun.EXCEL)
                {
                    nombreRecurso = "jsExcel";
                    Estadistica.CodigoAccion = Comun.EXCEL;
                }
                if (pTipoExcel == Comun.EXCEL_CONTACTOS)
                {
                    nombreRecurso = "jsExcelContactos";
                    Estadistica.CodigoAccion = Comun.EXCEL_CONTACTOS;
                }
                else if (pTipoExcel == Comun.EXCEL_GLOBAL)
                {
                    nombreRecurso = "jsExcelGlobal";
                    Estadistica.CodigoAccion = Comun.EXCEL_GLOBAL;
                }
                else if (pTipoExcel == Comun.EXCEL_IFRS16)
                {
                    nombreRecurso = "jsExcelIfrs16";
                    Estadistica.CodigoAccion = Comun.EXCEL_IFRS16;
                }
                else if (pTipoExcel == Comun.EXCEL_ANALYTICS)
                {
                    nombreRecurso = "jsExcelAnalytics";
                    Estadistica.CodigoAccion = Comun.EXCEL_ANALYTICS;
                }
                else if (pTipoExcel == Comun.EXCEL_SIN_FORMATO)
                {
                    nombreRecurso = "jsmensajedescargaexcelSF";
                    Estadistica.CodigoAccion = Comun.EXCEL_SIN_FORMATO;
                }
                else if (pTipoExcel == Comun.EXCEL_CDM_BT)
                {
                    nombreRecurso = "jsmensajedescargaexcelCdMBT";
                    Estadistica.CodigoAccion = Comun.EXCEL_CDM_BT;
                }
                else if (pTipoExcel == Comun.EXCEL_CDM_MT)
                {
                    nombreRecurso = "jsmensajedescargaexcelCdMMT";
                    Estadistica.CodigoAccion = Comun.EXCEL_CDM_MT;
                }
                else if (pTipoExcel == Comun.EXCEL_CDM_MER)
                {
                    nombreRecurso = "jsmensajedescargaexcelCdMMer";
                    Estadistica.CodigoAccion = Comun.EXCEL_CDM_MER;
                }
                else if (pTipoExcel == Comun.EXCEL_CDM_CENA)
                {
                    nombreRecurso = "jsmensajedescargaexcelCdmCE";
                    Estadistica.CodigoAccion = Comun.EXCEL_CDM_CENA;
                }

                if (pPaisIdioma != "")
                    comentarioTraducido = Resources.Comun.ResourceManager.GetString(nombreRecurso, new System.Globalization.CultureInfo(pPaisIdioma));
                else
                    comentarioTraducido = Resources.Comun.ResourceManager.GetString(nombreRecurso, new System.Globalization.CultureInfo("es-ES"));

                Estadistica.Comentarios = comentarioTraducido;

                cEstadisticas.AddItem(Estadistica);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }

        }

        public List<Vw_Estadisticas> GetEstadisticasByCliente(long ClienteID)
        {
            EstadisticasController cEstadisticas = new EstadisticasController();
            List<Vw_Estadisticas> listEstCli = new List<Vw_Estadisticas>();

            listEstCli = (from c in Context.Vw_Estadisticas where (c.ClienteID == ClienteID) select c).ToList();

            return listEstCli;

        }
        public List<Vw_Estadisticas> GetEstadisticasByClienteV2(long ClienteID)
        {
            List<Vw_Estadisticas> listEstCli = null;

            if (ClienteID != 0)
            {
                var ls = (from c in Context.Vw_Estadisticas where (c.ClienteID == ClienteID) select c).OrderBy("ProyectoTipo").ToList();
                if (ls != null)
                    listEstCli = ls;
            }
            else
            {
                var ls = (from c in Context.Vw_Estadisticas where (c.ClienteID == null) select c).OrderBy("ProyectoTipo").ToList();

                if (ls != null)
                    listEstCli = ls;
            }

            return listEstCli;

        }


        public List<Vw_Estadisticas> GetEstadisticasByClienteV2Fechas(long ClienteID, DateTime fechaini, DateTime fechafin)
        {
            List<Vw_Estadisticas> listEstCli = null;

            if (ClienteID != 0)
            {
                var ls = (from c in Context.Vw_Estadisticas where (c.ClienteID == ClienteID && c.FechaHoraInicio >= fechaini && c.FechaHoraFin <= fechafin) select c).OrderBy("ProyectoTipo").ToList();
                if (ls != null)
                    listEstCli = ls;
            }
            else
            {
                var ls = (from c in Context.Vw_Estadisticas where (c.ClienteID == null && c.FechaHoraInicio >= fechaini && c.FechaHoraFin <= fechafin) select c).OrderBy("ProyectoTipo").ToList();

                if (ls != null)
                    listEstCli = ls;
            }

            return listEstCli;

        }


        public List<Vw_Estadisticas> GetEstadisticasByClienteV2Last30days(long ClienteID)
        {
            List<Vw_Estadisticas> listEstCli = null;
            DateTime rangomes = DateTime.Now.AddDays(-30);

            if (ClienteID != 0)
            {
                var ls = (from c in Context.Vw_Estadisticas where (c.ClienteID == ClienteID && c.FechaHoraInicio > rangomes) select c).OrderBy("ProyectoTipo").ToList();
                if (ls != null)
                    listEstCli = ls;
            }
            else
            {
                var ls = (from c in Context.Vw_Estadisticas where (c.ClienteID == null && c.FechaHoraInicio > rangomes) select c).OrderBy("ProyectoTipo").ToList();

                if (ls != null)
                    listEstCli = ls;
            }

            return listEstCli;

        }

        public List<Vw_Estadisticas> GetEstadisticasSuperUsuario()
        {
            EstadisticasController cEstadisticas = new EstadisticasController();
            List<Vw_Estadisticas> listEstCli = new List<Vw_Estadisticas>();

            listEstCli = (from c in Context.Vw_Estadisticas where (c.ClienteID == null) select c).ToList();

            return listEstCli;

        }


        public List<List<string>> GetEstadisticasGrafica(long? ModuloID, long? UsuarioID, DateTime FechaInicial, DateTime FechaFinal, long? clienteID)
        {
            // local variables
            List<List<string>> sRes = null;
            List<string> lFila = null;
            List<string> lFilaCabecera = null;

            sRes = new List<List<string>>();


            EstadisticasController cEstadisticas = new EstadisticasController();
            List<Estadisticas> listEstadistica = new List<Estadisticas>();

            // Extraemos los elementos en las fechas y el modulo seleccionados.

            if (UsuarioID == 0 && ModuloID != null)
            {
                listEstadistica = (from c in Context.Estadisticas where (c.FechaHoraInicio.Date >= FechaInicial.Date && c.FechaHoraInicio.Date <= FechaFinal) && (c.ProyectoTipoID == ModuloID) && (c.ClienteID == clienteID) select c).ToList();
            }
            else if (UsuarioID == 0 && ModuloID == null)
            {
                listEstadistica = (from c in Context.Estadisticas where (c.FechaHoraInicio.Date >= FechaInicial.Date && c.FechaHoraInicio.Date <= FechaFinal) && (c.ClienteID == clienteID) select c).ToList();

            }
            else if (UsuarioID != 0 && ModuloID == null)
            {
                listEstadistica = (from c in Context.Estadisticas where (c.FechaHoraInicio.Date >= FechaInicial.Date && c.FechaHoraInicio.Date <= FechaFinal) && (c.UsuarioID == UsuarioID) select c).ToList();
            }


            if (listEstadistica == null || listEstadistica.Count == 0)
            {
                sRes = null;

            }
            else
            {

                // La fila de cabecera estara formada por los nombres
                // de las paginas del modulo seleccionado.

                lFilaCabecera = new List<string>();
                IQueryable<string> listCabeceraAux = null;

                if (ModuloID != null)
                {
                    listCabeceraAux = from c in Context.Estadisticas where (c.ProyectoTipoID == ModuloID) select c.Pagina;
                }
                else
                {

                    listCabeceraAux = from c in Context.Estadisticas select c.Pagina;


                }

                IQueryable<string> listCabecera = listCabeceraAux.Distinct();

                string paginacorregida = "";

                foreach (string pagina in listCabecera)
                {
                    paginacorregida = "";
                    paginacorregida = pagina.Remove(pagina.Length - 5);

                    lFilaCabecera.Add(paginacorregida);

                }
                sRes.Add(lFilaCabecera);

                // Ya tenemos la fila completa con cada pagina que se ha registrado
                // en las estadisticas. Ahora debemos ver el numero de registros que 
                // hay en la tabla listEstadisticas para cada una de las paginas.

                List<Estadisticas> listEstadisticaAux = null;
                int NumeroVisitas = 0;

                lFila = new List<string>();

                foreach (string pagina in listCabecera)
                {
                    NumeroVisitas = 0;

                    listEstadisticaAux = new List<Estadisticas>();
                    listEstadisticaAux = (from c in listEstadistica where (c.Pagina == pagina) select c).ToList();

                    NumeroVisitas = listEstadisticaAux.Count();

                    if (NumeroVisitas > 0)
                    {
                        lFila.Add(NumeroVisitas.ToString());
                    }
                    else
                    {

                        string cadena = "";
                        if (pagina.Contains("."))
                        {
                            cadena = pagina.Substring(0, pagina.IndexOf(".")).ToLower();
                        }
                        else
                        {
                            cadena = pagina.ToLower();
                        }


                        for (int i = 0; i < lFilaCabecera.Count; i++)
                        {
                            if (lFilaCabecera[i].ToLower().Equals(cadena))
                            {
                                lFilaCabecera.RemoveAt(i);
                                i--;
                            }
                        }




                    }
                }

                sRes.Add(lFila);
            }

            // Returns the result
            return sRes;
        }


        /// <summary>
        /// Funcion que escribe un registro cada vez que se cargue una pagina
        /// siempre y cuando este activa la variable que habilita la estadistica.
        /// </summary>
        /// <param name="UsuarioID"></param>
        /// <param name="ProyectoTipoID"></param>
        /// <param name="pagina"></param>
        /// <param name="Activo"> Indica si esta activa el registro de estadisticas</param>
        /// <param name="Comentario"> Especifica la accion realizada en casos que una misma pagina genere varios formularios</param>        
        /// <param name="CodigoAccion"> Especifica la accion realizada por el usuario</param>
        public void EscribeEstadisticaAccion(long lUsuarioID, long? lClienteID, string sPagina, bool bActivo, string sComentario, string sCodigoAccion)
        {

            try
            {

                if (bActivo == true)
                {
                    EstadisticasController cEstadisticas = new EstadisticasController();
                    ModulosController cModulos = new ModulosController();
                    ProyectosTiposController cProyectosTipos = new ProyectosTiposController();

                    Modulos oModulo;
                    Estadisticas registro = new Estadisticas();

                    oModulo = cModulos.getModuloByPagina(sPagina);

                    if (oModulo != null)
                    {
                        registro.UsuarioID = lUsuarioID;
                        registro.FechaHoraInicio = DateTime.Now;
                        registro.Pagina = sPagina;

                        registro.Comentarios = sComentario;
                        registro.ModuloID = oModulo.ModuloID;
                        registro.ClienteID = lClienteID;
                        registro.CodigoAccion = sCodigoAccion;

                        if (oModulo.ProyectoTipoID != null)
                        {
                            registro.ProyectoTipoID = (long)oModulo.ProyectoTipoID;
                        }
                        else
                        {
                            long lProyectoTipoID = cProyectosTipos.getidProyectoTipo("GLOBAL");
                            registro.ProyectoTipoID = lProyectoTipoID;
                        }

                        if (sComentario == "LOGOUT")
                        {
                            registro.FechaHoraFin = DateTime.Now;
                        }

                        cEstadisticas.AddItem(registro);
                    }
                }

            }

            catch (Exception ex)
            {
                log.Error(ex.Message);
            }

        }
    }
}

