using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using TreeCore.Data;
using System.Data.SqlClient;
//using Microsoft.Ajax.Utilities;

namespace CapaNegocio
{
    public class MonedasGlobalesController : GeneralBaseController<MonedasGlobales, TreeCoreContext>
    {
        public MonedasGlobalesController()
            : base()
        { }

        public bool RegistroVinculado(long lMonedaID)
        {
            bool bExiste = true;


            return bExiste;
        }

        public bool RegistroDuplicado(long lMonedaID, long lClienteID)
        {
            bool bExiste = false;
            List<MonedasGlobales> listaDatos;

            listaDatos = (from c in Context.MonedasGlobales where (c.MonedaGlobalID == lMonedaID /*&& c.ClienteID == lClienteID*/) select c).ToList<MonedasGlobales>();

            if (listaDatos.Count > 0)
            {
                bExiste = true;
            }

            return bExiste;
        }

        public bool RegistroDefecto(long lClienteID)
        {
            MonedasGlobales oDato;
            MonedasGlobalesController cController = new MonedasGlobalesController();
            bool bDefecto = false;

            oDato = cController.GetItem("Defecto == true && ClienteID == " + lClienteID.ToString());

            if (oDato != null)
            {
                bDefecto = true;
            }
            else
            {
                bDefecto = false;
            }

            return bDefecto;
        }


        public List<Vw_MonedasGlobales> GetMonedasGlobalesVigentesByMoneda (long lMonedaID)
        {
            List<Vw_MonedasGlobales> listaDatos;

            try
            {
                listaDatos = (from c in Context.Vw_MonedasGlobales where c.FechaFin == null && c.MonedaID == lMonedaID orderby c.Moneda select c).ToList();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                listaDatos = new List<Vw_MonedasGlobales>();
            }

            return listaDatos;
        }

        public List<Vw_MonedasGlobales> GetMonedasGlobalesVigentes()
        {
            List<Vw_MonedasGlobales> listaDatos;

            try
            {

                listaDatos = (from c in Context.Vw_MonedasGlobales where c.FechaFin == null orderby c.Moneda select c).ToList();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                listaDatos = new List<Vw_MonedasGlobales>();

            }

            return listaDatos;
        }

        public List<MonedasGlobales> GetMonedasGlobalesByMoneda(long lMonedaID)
        {
            List<MonedasGlobales> listaDatos;

            try
            {
                listaDatos = (from c in Context.MonedasGlobales where c.MonedaID == lMonedaID orderby c.FechaInicio descending select c).ToList();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                listaDatos = new List<MonedasGlobales>();
            }

            return listaDatos;
        }

        public List<MonedasGlobales> GetMonedasGlobalesByMonedaFecha(long lMonedaID, DateTime dFechaLocal)
        {
            List<MonedasGlobales> listaDatos;

            try
            {
                listaDatos = (from c in Context.MonedasGlobales where c.FechaInicio > dFechaLocal && c.MonedaID == lMonedaID orderby c.FechaInicio select c).ToList();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                listaDatos = new List<MonedasGlobales>();
            }

            return listaDatos;
        }

        public List<Vw_MonedasGlobales> GetMonedasGlobalesEvolucionByMoneda(long lMonedaID)
        {
            List<Vw_MonedasGlobales> listaDatos;

            try
            {
                listaDatos = (from c in Context.Vw_MonedasGlobales where c.FechaFin != null && c.MonedaID == lMonedaID orderby c.FechaInicio descending select c).ToList();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                listaDatos = new List<Vw_MonedasGlobales>();
            }

            return listaDatos;
        }

        public MonedasGlobales GetMoneda (long lMonedaID)
        {
            MonedasGlobales oDato;

            try
            {
                oDato = (from c in Context.MonedasGlobales where c.MonedaID == lMonedaID select c).First();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                oDato = null;
            }

            return oDato;
        }
    }
}