using System;
using System.Collections.Generic;
using System.Linq;
using Tree.Linq.GenericExtensions;
using TreeCore.Data;

namespace CapaNegocio
{
    public class CoreAtributosConfiguracionTiposDatosPropiedadesController : GeneralBaseController<CoreAtributosConfiguracionTiposDatosPropiedades, TreeCoreContext>
    {
        public CoreAtributosConfiguracionTiposDatosPropiedadesController()
            : base()
        { }

        public List<Vw_CoreAtributosConfiguracionTiposDatosPropiedades> GetVwPropiedadesFromAtributo(long lAtributoID)
        {
            List<Vw_CoreAtributosConfiguracionTiposDatosPropiedades> listaDatos;
            try
            {
                listaDatos = (from c in Context.Vw_CoreAtributosConfiguracionTiposDatosPropiedades where c.CoreAtributoConfiguracionID == lAtributoID select c).ToList();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                listaDatos = null;
            }
            return listaDatos;
        }

        public List<TiposDatosPropiedades> GetPropiedadesLibresAtributo(long AtributoID, long TipoDatoID)
        {
            List<TiposDatosPropiedades> listaDatos;
            try
            {
                List<long> asignados = (from c in Context.CoreAtributosConfiguracionTiposDatosPropiedades where c.CoreAtributoConfiguracionID == AtributoID select c.TipoDatoPropiedadID).ToList();
                listaDatos = (from c in Context.TiposDatosPropiedades where c.TipoDatoID == TipoDatoID && !(asignados.Contains(c.TipoDatoPropiedadID)) select c).ToList();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                listaDatos = null;
            }
            return listaDatos;
        }

        public string GetMaxValue(long lAtributoID)
        {
            string Dato = null;
            try
            {
                Dato = (from c in Context.Vw_CoreAtributosConfiguracionTiposDatosPropiedades
                        where c.CoreAtributoConfiguracionID == lAtributoID && c.CodigoTipoDatoPropiedad == "MaxValue"
                        select c.Valor).First();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                Dato = null;
            }
            return Dato;
        }

        public string GetMinValue(long lAtributoID)
        {
            string Dato = null;
            try
            {
                Dato = (from c in Context.Vw_CoreAtributosConfiguracionTiposDatosPropiedades
                        where c.CoreAtributoConfiguracionID == lAtributoID && c.CodigoTipoDatoPropiedad == "MinValue"
                        select c.Valor).First();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                Dato = null;
            }
            return Dato;
        }

        public bool ComprobarPropiedad (TiposDatosPropiedades oPropiedad, CoreAtributosConfiguracionTiposDatosPropiedades oDato)
        {
            bool valido = true;
            List<CoreAtributosConfiguracionTiposDatosPropiedades> listaDatos;
            try
            {
                switch (oPropiedad.Codigo)
                {
                    case "MinValue":
                        listaDatos = (from c in Context.CoreAtributosConfiguracionTiposDatosPropiedades
                                      join prop in Context.TiposDatosPropiedades on c.TipoDatoPropiedadID equals prop.TipoDatoPropiedadID
                                      where c.CoreAtributoConfiguracionID == oDato.CoreAtributoConfiguracionID && prop.Codigo == "MaxValue"
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
                        listaDatos = (from c in Context.CoreAtributosConfiguracionTiposDatosPropiedades
                                      join prop in Context.TiposDatosPropiedades on c.TipoDatoPropiedadID equals prop.TipoDatoPropiedadID
                                      where c.CoreAtributoConfiguracionID == oDato.CoreAtributoConfiguracionID && prop.Codigo == "MinValue"
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