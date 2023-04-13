using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using TreeCore.Clases;
using TreeCore.Data;

namespace CapaNegocio
{
    public class MonedasController : GeneralBaseController<Monedas, TreeCoreContext>, IGestionBasica<Monedas>
    {
        public MonedasController()
            : base()
        { }

        public InfoResponse Add(Monedas oEntidad)
        {
            MonedasEvolucionesController cEvolucion = new MonedasEvolucionesController();
            cEvolucion.SetDataContext(this.Context);

            InfoResponse oResponse;

            try
            {
                if (!RegistroDuplicado(oEntidad))
                {
                    oResponse = AddEntity(oEntidad);

                    if (oResponse.Result)
                    {
                        MonedasEvoluciones evolucion = new MonedasEvoluciones()
                        {
                            Monedas = oEntidad,
                            CambioDollarUS = oEntidad.CambioDollarUS.Value,
                            CambioEuro = oEntidad.CambioEuro.Value,
                            FechaCambio = DateTime.Now.Date
                        };

                        oResponse = cEvolucion.Add(evolucion);
                    }
                }
                else
                {
                    oResponse = new InfoResponse()
                    {
                        Result = false,
                        Description = GetGlobalResource(Comun.LogRegistroExistente)
                    };
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                oResponse = new InfoResponse
                {
                    Result = false,
                    Code = "",
                    Description = Comun.strMensajeGenerico,
                    Data = null
                };
            }

            return oResponse;
        }

        public InfoResponse Update(Monedas oEntidad)
        {
            MonedasEvolucionesController cEvolucion = new MonedasEvolucionesController();
            cEvolucion.SetDataContext(this.Context);
            InfoResponse oResponse;

            try
            {
                oResponse = UpdateEntity(oEntidad);

                if (!RegistroDuplicado(oEntidad))
                {
                    oResponse = UpdateEntity(oEntidad);

                    if (oResponse.Result)
                    {
                        MonedasEvoluciones evolucion = new MonedasEvoluciones()
                        {
                            Monedas = oEntidad,
                            CambioDollarUS = oEntidad.CambioDollarUS.Value,
                            CambioEuro = oEntidad.CambioEuro.Value,
                            FechaCambio = DateTime.Now.Date
                        };

                        oResponse = cEvolucion.Add(evolucion);
                    }
                }
                else
                {
                    oResponse = new InfoResponse()
                    {
                        Result = false,
                        Description = GetGlobalResource(Comun.LogRegistroExistente)
                    };
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                oResponse = new InfoResponse
                {
                    Result = false,
                    Code = "",
                    Description = Comun.strMensajeGenerico,
                    Data = null
                };
            }

            return oResponse;
        }

        public InfoResponse Delete(Monedas oEntidad)
        {
            InfoResponse oResponse;

            try
            {
                if (RegistroDefecto(oEntidad))
                {
                    oResponse = new InfoResponse()
                    {
                        Result = false,
                        Description = GetGlobalResource(Comun.jsPorDefecto)
                    };
                }
                else
                {
                    oResponse = DeleteEntity(oEntidad);
                    if (oResponse.Result)
                    {
                        log.Info(GetGlobalResource(Comun.LogEliminacionRealizada));
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                oResponse = new InfoResponse
                {
                    Result = false,
                    Code = "",
                    Description = Comun.strMensajeGenerico,
                    Data = null
                };
            }

            return oResponse;
        }

        public InfoResponse AsignarPorDefecto(long monedaID, long clienteID)
        {
            InfoResponse infoResponse;

            // BUSCAMOS SI HAY UN ELEMENTO POR DEFECTO
            Monedas oDato = GetDefault(clienteID);

            // SI HAY Y ES DISTINTO AL SELECCIONADO
            if (oDato != null)
            {
                if (oDato.MonedaID != monedaID)
                {
                    if (oDato.Defecto)
                    {
                        oDato.Defecto = !oDato.Defecto;
                        infoResponse = Update(oDato);
                    }

                    oDato = GetItem(monedaID);
                    oDato.Defecto = true;
                    oDato.Activo = true;
                    infoResponse = Update(oDato);
                }
                else
                {
                    oDato.Defecto = !oDato.Defecto;
                    infoResponse = Update(oDato);
                }
            }
            else
            {
                oDato = GetItem(monedaID);
                oDato.Defecto = true;
                oDato.Activo = true;
                infoResponse = Update(oDato);
            }

            log.Info(GetGlobalResource(Comun.LogCambioRegistroPorDefecto));

            return infoResponse;
        }

        public bool HasActiveMoneda(string moneda, long clienteID)
        {
            bool existe = false;

            try
            {
                existe = (from c in Context.Monedas
                          where c.Activo == true &&
                                  c.Moneda == moneda &&
                                  c.ClienteID == clienteID
                          select c).Count() > 0;
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                existe = false;
            }

            return existe;
        }
        public Monedas GetActivoMoneda(string moneda, long clienteID)
        {
            Monedas result;

            try
            {
                result = (from c in Context.Monedas
                          where c.Activo == true &&
                                  c.Moneda == moneda &&
                                  c.ClienteID == clienteID
                          select c).First();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                result = null;
            }

            return result;
        }

        public bool RegistroDuplicado(Monedas Moneda)
        {
            bool bExiste = false;
            List<Monedas> listaDatos;

            listaDatos = (from c in Context.Monedas
                          where 
                                c.Moneda.Equals(Moneda.Moneda) &&
                                c.ClienteID == Moneda.ClienteID &&
                                c.MonedaID != Moneda.MonedaID
                          select c).ToList();

            if (listaDatos.Count > 0)
            {
                bExiste = true;
            }

            return bExiste;
        }

        public bool RegistroDuplicado(string sMoneda, long lClienteID)
        {
            bool bExiste = false;
            List<Monedas> listaDatos;

            listaDatos = (from c in Context.Monedas
                          where c.Moneda.Equals(sMoneda) &&
                                c.ClienteID == lClienteID
                          select c).ToList<Monedas>();

            if (listaDatos.Count > 0)
            {
                bExiste = true;
            }

            return bExiste;
        }

        public bool RegistroDefecto(Monedas moneda)
        {
            Monedas oMoneda;
            MonedasController cController = new MonedasController();
            bool bDefecto = false;

            oMoneda = (from c in Context.Monedas 
                       where 
                            c.Defecto && 
                            c.MonedaID == moneda.MonedaID 
                       select c).FirstOrDefault();

            if (oMoneda != null)
            {
                bDefecto = true;
            }
            else
            {
                bDefecto = false;
            }

            return bDefecto;
        }

        public long GetMonedaBySimbolo(string Simbolo)
        {

            long MonedaID = 0;
            try
            {

                MonedaID = (from c in Context.Monedas where c.Simbolo.Equals(Simbolo) select c.MonedaID).First();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                MonedaID = -1;

            }
            return MonedaID;
        }

        public MonedasEvoluciones getUltimoCambio(long monedaID)
        {
            try
            {
                List<MonedasEvoluciones> datos = null;
                datos = (from c in Context.MonedasEvoluciones where (c.MonedaID == monedaID) orderby c.MonedaEvolucionID descending select c).ToList();
                if (datos != null && datos.Count > 0)
                {
                    return datos[0];
                }
                else
                    return null;

            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return null;
            }
        }

        public List<Monedas> GetAllMonedas()
        {
            List<Monedas> listaComponentes;
            MonedasController cMonedas = new MonedasController();

            listaComponentes = cMonedas.GetItemsList<Monedas>("", "Moneda");

            return listaComponentes;
        }

        public List<Monedas> GetActivos(long clienteID)
        {
            List<Monedas> listadatos;
            try
            {
                listadatos = (from c
                              in Context.Monedas
                              where c.Activo == true &&
                                    c.ClienteID == clienteID
                              orderby c.Moneda
                              select c).ToList();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                listadatos = null;
            }
            return listadatos;
        }

        public List<Monedas> GetAllActivos()
        {
            List<Monedas> listadatos;
            try
            {
                listadatos = (from c
                              in Context.Monedas
                              where c.Activo == true
                              orderby c.Moneda
                              select c).ToList();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                listadatos = null;
            }
            return listadatos;
        }

        public Monedas GetDefault(long lClienteID)
        {
            Monedas oMoneda;
            try
            {
                oMoneda = (from c in Context.Monedas 
                           where c.Defecto && c.ClienteID == lClienteID 
                           select c).First();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                oMoneda = null;
            }

            return oMoneda;
        }

        public List<Monedas> GetActivosCliente(long lClienteID)
        {
            List<Monedas> listaMonedas;

            try
            {
                listaMonedas = (from c
                              in Context.Monedas
                              where c.Activo == true && c.ClienteID == lClienteID 
                              orderby c.Moneda
                              select c).ToList();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                listaMonedas = null;
            }

            return listaMonedas;
        }


        public List<string> GetMonedasNombre(long ClienteID)
        {
            List<string> listaMonedas;

            try
            {
                listaMonedas = (from c
                                in Context.Monedas
                                where c.Activo == true && c.ClienteID == ClienteID
                                orderby c.Moneda
                                select c.Moneda).ToList();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                listaMonedas = null;
            }

            return listaMonedas;
        }

        public List<string> GetMonedasCodigo(long ClienteID)
        {
            List<string> listaMonedas;

            try
            {
                listaMonedas = (from c
                                in Context.Monedas
                                where c.Activo == true && c.ClienteID == ClienteID
                                orderby c.Moneda
                                select c.Simbolo).ToList();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                listaMonedas = null;
            }

            return listaMonedas;
        }

        #region CONVERSION

        public double ConvertMoneda(double val, long srcMonedaID, long destMonedaID)
        {
            Monedas srcMoneda = GetItem("MonedaID == " + srcMonedaID.ToString());
            Monedas destMoneda = GetItem("MonedaID == " + destMonedaID.ToString());

            if (srcMoneda != null &&
                destMoneda != null)
            {
                double toDolar = val * (double)srcMoneda.CambioDollarUS;

                double outVal = toDolar / (double)destMoneda.CambioDollarUS;

                return outVal;
            }

            return val;
        }

        public double ConvertMoneda(double val, Monedas srcMoneda, Monedas destMoneda)
        {
            if (srcMoneda != null &&
                destMoneda != null)
            {
                double toDolar = val * (double)srcMoneda.CambioDollarUS;

                double outVal = toDolar / (double)destMoneda.CambioDollarUS;

                return outVal;
            }

            return val;
        }

        #endregion
    }
}