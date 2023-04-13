using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using TreeCore.Data;
using System.Data.SqlClient;

namespace CapaNegocio
{
    public class InventarioAtributosTiposDatosPropiedadesController : GeneralBaseController<InventarioAtributosTiposDatosPropiedades, TreeCoreContext>
    {
        public InventarioAtributosTiposDatosPropiedadesController()
            : base()
        { }

        public List<Vw_InventarioAtributosTiposDatosPropiedades> GetPropiedadesFromAtributo(long atributoID)
        {
            List<Vw_InventarioAtributosTiposDatosPropiedades> listadatos;
            try
            {
                listadatos = (from c in Context.Vw_InventarioAtributosTiposDatosPropiedades where c.InventarioAtributoID == atributoID && (bool)c.Activo select c).ToList();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                listadatos = null;
            }
            return listadatos;
        }


        public List<Vw_ConfCoreAtributosConfiguracionTiposDatosPropiedades> GetPropiedadesFromAtributoCore(long atributoID)
        {
            List<long> listaIDs;
            List<Vw_ConfCoreAtributosConfiguracionTiposDatosPropiedades> listadatos = new List<Vw_ConfCoreAtributosConfiguracionTiposDatosPropiedades>();
            Vw_ConfCoreAtributosConfiguracionTiposDatosPropiedades oDato;

            try
            {
                listaIDs = (from c in Context.CoreAtributosConfiguracionTiposDatosPropiedades where c.CoreAtributoConfiguracionID == atributoID select c.CoreAtributoConfiguracionTipoDatoPropiedadID).ToList();

                for (int i = 0; i < listaIDs.Count; i++)
                {
                    oDato = (from c in Context.Vw_ConfCoreAtributosConfiguracionTiposDatosPropiedades where c.CoreAtributoConfiguracionTipoDatoPropiedadID == listaIDs[i] select c).First();

                    if (oDato != null)
                    {
                        listadatos.Add(oDato);
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                listadatos = null;
            }
            return listadatos;
        }

        public List<TiposDatosPropiedades> GetPropiedadesLibresInventarioAtributo(long AtributoID, long TipoDatoID)
        {
            List<TiposDatosPropiedades> listaDatos;
            try
            {
                List<long> asignados = (from c in Context.InventarioAtributosTiposDatosPropiedades where c.InventarioAtributoID == AtributoID select c.TipoDatoPropiedadID).ToList();
                listaDatos = (from c in Context.TiposDatosPropiedades where c.TipoDatoID == TipoDatoID && !(asignados.Contains(c.TipoDatoPropiedadID)) select c).ToList();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                listaDatos = null;
            }
            return listaDatos;
        }

        public bool ComprobarPropiedad(TiposDatosPropiedades oPropiedad, InventarioAtributosTiposDatosPropiedades oDato)
        {
            bool valido = true;
            List<InventarioAtributosTiposDatosPropiedades> listaDatos;
            try
            {
                switch (oPropiedad.Codigo)
                {
                    case "MinValue":
                        listaDatos = (from c in Context.InventarioAtributosTiposDatosPropiedades
                                      join prop in Context.TiposDatosPropiedades on c.TipoDatoPropiedadID equals prop.TipoDatoPropiedadID
                                      where c.InventarioAtributoID == oDato.InventarioAtributoID && prop.Codigo == "MaxValue"
                                      select c).ToList();
                        if (listaDatos.Count > 0)
                        {
                            if (double.Parse(listaDatos.First().Valor) < double.Parse(oDato.Valor))
                            {
                                valido = false;
                            }
                        }
                        break;
                    case "MaxValue":
                        listaDatos = (from c in Context.InventarioAtributosTiposDatosPropiedades
                                      join prop in Context.TiposDatosPropiedades on c.TipoDatoPropiedadID equals prop.TipoDatoPropiedadID
                                      where c.InventarioAtributoID == oDato.InventarioAtributoID && prop.Codigo == "MinValue"
                                      select c).ToList();
                        if (listaDatos.Count > 0)
                        {
                            if (double.Parse(listaDatos.First().Valor) > double.Parse(oDato.Valor))
                            {
                                valido = false;
                            }
                        }
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                valido = false;
            }
            return valido;
        }
    }
}