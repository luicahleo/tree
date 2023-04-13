using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using TreeCore.Data;
using System.Data.SqlClient;

namespace CapaNegocio
{
    public class MonedasController : GeneralBaseController<Monedas, TreeCoreContext>
    {
        public MonedasController()
            : base()
        { }

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

        public bool RegistroDuplicado(string sMoneda, long lClienteID)
        {
            bool bExiste = false;
            List<Monedas> listaDatos;

            listaDatos = (from c in Context.Monedas where c.Moneda.Equals(sMoneda) &&
                            c.ClienteID == lClienteID select c).ToList<Monedas>();

            if (listaDatos.Count > 0)
            {
                bExiste = true;
            }

            return bExiste;
        }

        public bool RegistroDefecto(long lMonedaID)
        {
            Monedas oMoneda;
            MonedasController cController = new MonedasController();
            bool bDefecto = false;

            oMoneda = cController.GetItem("Defecto == true && MonedaID == " + lMonedaID.ToString());

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
                oMoneda = (from c in Context.Monedas where c.Defecto && c.ClienteID == lClienteID select c).First();
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