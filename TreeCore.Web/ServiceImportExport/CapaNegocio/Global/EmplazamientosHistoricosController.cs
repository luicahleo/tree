using System;
using System.Data;
using TreeCore.Data;
using System.Collections.Generic;
using Tree.Linq.GenericExtensions;
using System.Linq;

namespace CapaNegocio
{
    public class EmplazamientosHistoricosController : GeneralBaseController<EmplazamientosHistoricos, TreeCoreContext>
    {
        public EmplazamientosHistoricosController()
            : base()
        {

        }


        #region GESTION BASE

        public List<Vw_EmplazamientosHistoricos> GetHistoricoByID(long emplazamientoID)
        {

            // Local variables
            List<Vw_EmplazamientosHistoricos> lista = null;

            try
            {
                lista = (from c in Context.Vw_EmplazamientosHistoricos where c.EmplazamientoID == emplazamientoID select c).ToList();

            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                lista = null;
            }


            return lista;


        }

        public EmplazamientosHistoricos GetHistoricoAnterior(long emplazamientoID, long historicoID)
        {

            // Local variables
            List<EmplazamientosHistoricos> lista = null;
            EmplazamientosHistoricos dato = null;

            try
            {
                lista = (from c in Context.EmplazamientosHistoricos where c.EmplazamientoHistoricoID < historicoID && c.EmplazamientoID == emplazamientoID select c).ToList();
                if (lista != null && lista.Count > 0)
                {
                    dato = lista.ElementAt(0);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                dato = null;
            }


            return dato;


        }


        public EmplazamientosHistoricos GetUltimoHistorico(long emplazamientoID)
        {

            // Local variables            
            EmplazamientosHistoricos dato = null;
            List<EmplazamientosHistoricos> lista = null;

            try
            {
                lista = (from c in Context.EmplazamientosHistoricos where c.EmplazamientoID == emplazamientoID orderby c.EmplazamientoHistoricoID descending select c).ToList();

                if (lista != null && lista.Count > 0)
                {
                    dato = lista.ElementAt(0);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                dato = null;
            }


            return dato;


        }

        #endregion

        #region GESTION GLOBAL
        /// <summary>
        /// Returns the list of historical information between a date range (Both included)
        /// </summary>
        /// <param name="fechaInicio">Initial date</param>
        /// <param name="fechaFin">Final Date</param>
        /// <returns></returns>
        public List<Vw_EmplazamientosHistoricos> GetHistoricosByFechas(DateTime fechaInicio, DateTime fechaFin)
        {

            // Local variables
            List<Vw_EmplazamientosHistoricos> lista = null;

            try
            {
                lista = (from c in Context.Vw_EmplazamientosHistoricos where c.FechaModificacion >= fechaInicio && c.FechaModificacion < fechaFin select c).ToList();

            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                lista = null;
            }


            return lista;


        }

        #endregion

        #region HISTORICOS
        public bool ActualizarHistoricoIntegracion(Emplazamientos emplazamientoAnterior, Usuarios Usuario, DateTime fechaActualizacion)
        {
            // Local variables
            bool bRes = false;

            try
            {
                // Initial data
                EmplazamientosHistoricosController cHistorico = new EmplazamientosHistoricosController();
                EmplazamientosHistoricos historico = new EmplazamientosHistoricos();

                // Updates the historical data
                historico.Adquirido = emplazamientoAnterior.Adquirido;
                historico.AltitudSitio = emplazamientoAnterior.AltitudSitio;
                historico.Barrio = emplazamientoAnterior.Barrio;
                historico.CampoGenerico1 = emplazamientoAnterior.CampoGenerico1;
                historico.CampoGenerico2 = emplazamientoAnterior.CampoGenerico2;
                historico.CampoGenerico3 = emplazamientoAnterior.CampoGenerico3;
                historico.CampoGenerico4 = emplazamientoAnterior.CampoGenerico4;
                historico.CampoGenerico5 = emplazamientoAnterior.CampoGenerico5;
                historico.CampoGenerico6 = emplazamientoAnterior.CampoGenerico6;
                historico.CampoGenerico7 = emplazamientoAnterior.CampoGenerico7;
                historico.CampoGenerico8 = emplazamientoAnterior.CampoGenerico8;
                historico.CampoGenerico9 = emplazamientoAnterior.CampoGenerico9;
                historico.CampoGenerico10 = emplazamientoAnterior.CampoGenerico10;
                historico.CampoGenerico11 = emplazamientoAnterior.CampoGenerico11;
                historico.CampoGenerico12 = emplazamientoAnterior.CampoGenerico12;
                historico.CampoGenerico13 = emplazamientoAnterior.CampoGenerico13;
                historico.CampoGenerico14 = emplazamientoAnterior.CampoGenerico14;
                historico.CampoGenerico15 = emplazamientoAnterior.CampoGenerico15;
                historico.CampoGenerico16 = emplazamientoAnterior.CampoGenerico16;
                historico.CampoGenerico17 = emplazamientoAnterior.CampoGenerico17;
                historico.CampoGenerico18 = emplazamientoAnterior.CampoGenerico18;
                historico.CampoGenerico19 = emplazamientoAnterior.CampoGenerico19;
                historico.CampoGenerico20 = emplazamientoAnterior.CampoGenerico20;
                historico.CampoGenerico21 = emplazamientoAnterior.CampoGenerico21;
                historico.CampoGenerico22 = emplazamientoAnterior.CampoGenerico22;
                historico.CampoGenerico23 = emplazamientoAnterior.CampoGenerico23;
                historico.CampoGenerico24 = emplazamientoAnterior.CampoGenerico24;
                historico.CampoGenerico25 = emplazamientoAnterior.CampoGenerico25;
                if (emplazamientoAnterior.CategoriaEmplazamientoID != null)
                {
                    EmplazamientosCategoriasSitiosController cCategoria = new EmplazamientosCategoriasSitiosController();
                    EmplazamientosCategoriasSitios categoria = cCategoria.GetItem((long)emplazamientoAnterior.CategoriaEmplazamientoID);
                    historico.CategoriaEmplazamiento = categoria.CategoriaSitio;
                }
                historico.ClaveCatastral = emplazamientoAnterior.ClaveCatastral;
                if (emplazamientoAnterior.ClienteID > 0)
                {
                    ClientesController cCliente = new ClientesController();
                    Clientes clienteEmp = cCliente.GetItem(emplazamientoAnterior.ClienteID);
                    historico.Cliente = clienteEmp.Cliente;
                }
                historico.Codigo = emplazamientoAnterior.Codigo;
                historico.CodigoNC = emplazamientoAnterior.CodigoNC;
                historico.CodigoPostal = emplazamientoAnterior.CodigoPostal;
                historico.CodigoSAP = emplazamientoAnterior.CodigoSAP;
                historico.CodigoTelco = emplazamientoAnterior.CodigoTelco;
                historico.CodigoTorrero = emplazamientoAnterior.CodigoTorrero;
                historico.ComentarioEdificio = emplazamientoAnterior.ComentarioEdificio;
                historico.ComentariosGenerales = emplazamientoAnterior.ComentariosGenerales;
                historico.CometariosEquipos = emplazamientoAnterior.CometariosEquipos;
                historico.Compartido = emplazamientoAnterior.Compartido;
                historico.Conflicto = emplazamientoAnterior.Conflicto;
                historico.CosteElectrico = emplazamientoAnterior.CosteElectrico;
                historico.Direccion = emplazamientoAnterior.Direccion;
                historico.EmplazamientoID = emplazamientoAnterior.EmplazamientoID;

                if (emplazamientoAnterior.EmplazamientoOrigenID != null)
                {
                    EmplazamientosCategoriasSitiosController cCat = new EmplazamientosCategoriasSitiosController();
                    EmplazamientosCategoriasSitios cat = cCat.GetItem((long)emplazamientoAnterior.EmplazamientoOrigenID);
                    historico.EmplazamientoOrigen = cat.CategoriaSitio;
                }

                if (emplazamientoAnterior.EmplazamientoRiesgoID != null)
                {
                    EmplazamientosRiesgosController cRiesgo = new EmplazamientosRiesgosController();
                    EmplazamientosRiesgos riesgo = cRiesgo.GetItem((long)emplazamientoAnterior.EmplazamientoRiesgoID);
                    historico.EmplazamientoRiesgo = riesgo.EmplazamientoRiesgo;
                }

                if (emplazamientoAnterior.EmplazamientoTamanoID != null)
                {
                    EmplazamientosTamanosController cTamano = new EmplazamientosTamanosController();
                    EmplazamientosTamanos tamano = cTamano.GetItem((long)emplazamientoAnterior.EmplazamientoTamanoID);
                    historico.EmplazamientoTamano = tamano.Tamano;
                }

                if (emplazamientoAnterior.EmplazamientoTipoID != null)
                {
                    EmplazamientosTiposController cTipo = new EmplazamientosTiposController();
                    EmplazamientosTipos tipo = cTipo.GetItem((long)emplazamientoAnterior.EmplazamientoTipoID);
                    historico.EmplazamientoTipo = tipo.Tipo;
                }

                historico.EmpresaCompradora = emplazamientoAnterior.EmpresaCompradora;
                historico.Equipo2G = emplazamientoAnterior.Equipo2G;
                historico.Equipo3G = emplazamientoAnterior.Equipo3G;
                historico.Equipo4G = emplazamientoAnterior.Equipo4G;
                historico.Equipo5G = emplazamientoAnterior.Equipo5G;
                historico.Erlang = emplazamientoAnterior.Erlang;
                if (emplazamientoAnterior.EstadoGlobalID != null)
                {
                    EstadosGlobalesController cEstado = new EstadosGlobalesController();
                    EstadosGlobales estado = cEstado.GetItem((long)emplazamientoAnterior.EstadoGlobalID);
                    historico.EstadoGlobal = estado.EstadoGlobal;
                }
                historico.FechaActivacion = emplazamientoAnterior.FechaActivacion;
                historico.FechaDesactivacion = emplazamientoAnterior.FechaDesactivacion;
                historico.FechaModificacion = DateTime.Now;
                historico.FechaActualizacionIntegracion = fechaActualizacion;

                if (emplazamientoAnterior.GlobalLocalidadID != null)
                {
                    GlobalLocalidadesController cLocalidades = new GlobalLocalidadesController();
                    GlobalLocalidades localidad = cLocalidades.GetItem((long)emplazamientoAnterior.GlobalLocalidadID);
                    historico.GlobalLocalidad = localidad.Localidad;
                    cLocalidades = null;
                }

                if (emplazamientoAnterior.GlobalMunicipalidadID != null)
                {
                    GlobalMunicipalidadesController cMunicipalidad = new GlobalMunicipalidadesController();
                    GlobalMunicipalidades municipalidad = cMunicipalidad.GetItem((long)emplazamientoAnterior.GlobalMunicipalidadID);
                    historico.GlobalMunicipalidad = municipalidad.Municipalidad;
                    cMunicipalidad = null;
                }

                if (emplazamientoAnterior.GlobalPartidoID != null)
                {
                    GlobalPartidosController cPartidos = new GlobalPartidosController();
                    GlobalPartidos partido = cPartidos.GetItem((long)emplazamientoAnterior.GlobalPartidoID);
                    historico.GlobalPartido = partido.Partido;
                    cPartidos = null;
                }

                historico.IdentificadorUnico = emplazamientoAnterior.IdentificadorUnico;
                historico.IGBR = emplazamientoAnterior.IGBR;
                historico.ImporteAlquilerBase = emplazamientoAnterior.ImporteAlquilerBase;
                historico.ImporteFianza = emplazamientoAnterior.ImporteFianza;
                historico.ImporteOtrosGastos = emplazamientoAnterior.ImporteOtrosGastos;
                historico.ImportePagoInicial = emplazamientoAnterior.ImportePagoInicial;
                historico.ImportePlusEstrategico = emplazamientoAnterior.ImportePlusEstrategico;
                historico.ImporteSustInfraest = emplazamientoAnterior.ImporteSustInfraest;
                historico.ImposibleLicenciar = emplazamientoAnterior.ImposibleLicenciar;
                historico.Inbound = emplazamientoAnterior.Inbound;
                historico.IntegracionSequencial = emplazamientoAnterior.IntegracionSequencial;
                historico.Latitud = emplazamientoAnterior.Latitud;
                historico.Legalizado = emplazamientoAnterior.Legalizado;
                historico.Longitud = emplazamientoAnterior.Longitud;
                if (emplazamientoAnterior.MonedaID != null)
                {
                    MonedasController cMoneda = new MonedasController();
                    Monedas moneda = cMoneda.GetItem((long)emplazamientoAnterior.MonedaID);
                    historico.Moneda = moneda.Moneda;
                }
                historico.MotivoConflicto = emplazamientoAnterior.MotivoConflicto;
                historico.Municipio = emplazamientoAnterior.Municipio;
                historico.NombreSitio = emplazamientoAnterior.NombreSitio;
                historico.NumContratoSitio = emplazamientoAnterior.NumContratoSitio;
                historico.NumSitiosDependientes = emplazamientoAnterior.NumSitiosDependientes;
                if (emplazamientoAnterior.OperadorID > 0)
                {
                    OperadoresController cOperadores = new OperadoresController();
                    Operadores operador = cOperadores.GetItem((long)emplazamientoAnterior.OperadorID);
                    historico.Operador = operador.Operador;
                }
                if (emplazamientoAnterior.OperadorPropietarioEstructuraID > 0)
                {
                    OperadoresController cOperadoresES = new OperadoresController();
                    Operadores operadorES = cOperadoresES.GetItem((long)emplazamientoAnterior.OperadorPropietarioEstructuraID);
                    historico.OperadorPropietarioEstructura = operadorES.Operador;
                }
                historico.Outbound = emplazamientoAnterior.Outbound;
                if (emplazamientoAnterior.PaisID > 0)
                {
                    PaisesController cPaises = new PaisesController();
                    Paises pais = cPaises.GetItem((long)emplazamientoAnterior.PaisID);
                    historico.Pais = pais.Pais;
                }
                historico.PotencialSitio = emplazamientoAnterior.PotencialSitio;
                historico.Propietario = emplazamientoAnterior.Propietario;
                historico.Provincia = emplazamientoAnterior.Provincia;
                historico.Region = emplazamientoAnterior.Region;
                historico.RegionPais = emplazamientoAnterior.RegionPais;
                historico.RentaInicialImporteAnual = emplazamientoAnterior.RentaInicialImporteAnual;
                historico.SituacionIngenieria = emplazamientoAnterior.SituacionIngenieria;
                historico.SuperficieSitio = emplazamientoAnterior.SuperficieSitio;
                historico.SuperficieVertical = emplazamientoAnterior.SuperficieVertical;
                historico.Supervisor = emplazamientoAnterior.Supervisor;
                if (emplazamientoAnterior.TipoEdificacionID != null)
                {
                    EmplazamientosTiposEdificiosController cEdificio = new EmplazamientosTiposEdificiosController();
                    EmplazamientosTiposEdificios edificio = cEdificio.GetItem((long)emplazamientoAnterior.TipoEdificacionID);
                    historico.TipoEdificacion = edificio.TipoEdificio;
                }
                historico.TotalAreaRenegociable = emplazamientoAnterior.TotalAreaRenegociable;
                historico.TotalCargoAdicional = emplazamientoAnterior.TotalCargoAdicional;
                historico.TotalCargoPuntual = emplazamientoAnterior.TotalCargoPuntual;
                historico.TotalDeuda = emplazamientoAnterior.TotalDeuda;
                historico.TotalDeudaAnual = emplazamientoAnterior.TotalDeudaAnual;
                historico.TotalDeudaEfectiva = emplazamientoAnterior.TotalDeudaEfectiva;
                historico.TotalEmplazamiento = emplazamientoAnterior.TotalEmplazamiento;
                historico.TotalEmplazamientoEfectivo = emplazamientoAnterior.TotalEmplazamientoEfectivo;
                historico.TotalRenegociable = emplazamientoAnterior.TotalRenegociable;
                historico.TotalValorAreaRenegociable = emplazamientoAnterior.TotalValorAreaRenegociable;
                historico.UsoSitio = emplazamientoAnterior.UsoSitio;
                historico.UsuarioID = Usuario.UsuarioID;
                historico.VarRentaRentaMediaZona = emplazamientoAnterior.VarRentaRentaMediaZona;
                historico.VentaSitioATercero = emplazamientoAnterior.VentaSitioATercero;

                if (cHistorico.AddItem(historico) != null)
                {
                    bRes = true;
                }




                return bRes;
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }

            return bRes;
        }
        #endregion

        #region CALIDAD

        public int GetCalidad(string sFiltro)
        {
            // Local variables
            int iResultado = 0;
            List<EmplazamientosHistoricos> lista = null;

            try
            {
                lista = GetItemsList(sFiltro);
                if (lista != null)
                {
                    iResultado = lista.Count;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }

            // Returns the result
            return iResultado;
        }

        #endregion

        public List<long> GetLastModified(int days, long clienteID)
        {
            List<long> IDs;
            DateTime fecha = DateTime.Today;

            fecha = fecha.AddDays(-days);

            try
            {
                IDs = (from c in Context.EmplazamientosHistoricos
                       where c.FechaModificacion > fecha
                       select c.EmplazamientoID).ToList();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                IDs = new List<long>();
            }

            return IDs;
        }

        public Vw_EmplazamientosHistoricos GetVwEmplzamientoHistoricoByID(long historicoID)
        {
            Vw_EmplazamientosHistoricos res;

            EmplazamientosHistoricosController cHistoricos = new EmplazamientosHistoricosController();
            try
            {
                using (cHistoricos.Context)
                {
                    res = (from c in cHistoricos.Context.Vw_EmplazamientosHistoricos
                           where c.EmplazamientoHistoricoID == historicoID
                           select c).First();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                res = null;
            }

            return res;
        }
    }
}
