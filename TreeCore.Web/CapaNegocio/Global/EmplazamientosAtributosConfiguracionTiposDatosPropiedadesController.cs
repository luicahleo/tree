using System;
using System.Collections.Generic;
using System.Linq;
using Tree.Linq.GenericExtensions;
using TreeCore.Data;

namespace CapaNegocio
{
    public class EmplazamientosAtributosConfiguracionTiposDatosPropiedadesController : GeneralBaseController<EmplazamientosAtributosTiposDatosPropiedades, TreeCoreContext>
    {
        public EmplazamientosAtributosConfiguracionTiposDatosPropiedadesController()
            : base()
        { }

        public List<Vw_EmplazamientosAtributosTiposDatosPropiedades> GetPropiedadesFromAtributo(long atributoID)
        {
            List<Vw_EmplazamientosAtributosTiposDatosPropiedades> listadatos;
            try
            {
                listadatos = (from c in Context.Vw_EmplazamientosAtributosTiposDatosPropiedades where c.EmplazamientoAtributoConfiguracionID == atributoID select c).ToList();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                listadatos = null;
            }
            return listadatos;
        }

        public List<TiposDatosPropiedades> GetPropiedadesLibresEmplazamientpAtributo(long AtributoID, long TipoDatoID)
        {
            List<TiposDatosPropiedades> listaDatos;
            try
            {
                List<long> asignados = (from c in Context.EmplazamientosAtributosTiposDatosPropiedades where c.EmplazamientoAtributoConfiguracionID == AtributoID select c.TipoDatoPropiedadID).ToList();
                listaDatos = (from c in Context.TiposDatosPropiedades where c.TipoDatoID == TipoDatoID && !(asignados.Contains(c.TipoDatoPropiedadID)) select c).ToList();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                listaDatos = null;
            }
            return listaDatos;
        }
        public bool ComprobarPropiedad(TiposDatosPropiedades oPropiedad, EmplazamientosAtributosTiposDatosPropiedades oDato)
        {
            bool valido = true;
            List<EmplazamientosAtributosTiposDatosPropiedades> listaDatos;
            try
            {
                switch (oPropiedad.Codigo)
                {
                    case "MinValue":
                        listaDatos = (from c in Context.EmplazamientosAtributosTiposDatosPropiedades
                                      join prop in Context.TiposDatosPropiedades on c.TipoDatoPropiedadID equals prop.TipoDatoPropiedadID
                                      where c.EmplazamientoAtributoConfiguracionID == oDato.EmplazamientoAtributoConfiguracionID && prop.Codigo == "MaxValue"
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
                        listaDatos = (from c in Context.EmplazamientosAtributosTiposDatosPropiedades
                                      join prop in Context.TiposDatosPropiedades on c.TipoDatoPropiedadID equals prop.TipoDatoPropiedadID
                                      where c.EmplazamientoAtributoConfiguracionID == oDato.EmplazamientoAtributoConfiguracionID && prop.Codigo == "MinValue"
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


        public List<Vw_EmplazamientosAtributosTiposDatosPropiedades> getAtributosByNombre(string sNombre)
        {
            List<Vw_EmplazamientosAtributosTiposDatosPropiedades> listadatos;
            try
            {
                listadatos = (from c in Context.Vw_EmplazamientosAtributosTiposDatosPropiedades where c.Nombre == sNombre select c).ToList();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                listadatos = null;
            }
            return listadatos;
        }
    }
}