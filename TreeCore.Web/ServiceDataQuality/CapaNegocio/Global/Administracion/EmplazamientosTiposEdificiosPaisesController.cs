using CapaNegocio;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using TreeCore.Data;

namespace TreeCore.CapaNegocio.Global.Administracion
{
    public sealed class EmplazamientosTiposEdificiosPaisesController : GeneralBaseController<EmplazamientosTiposEdificiosPaises, TreeCoreContext>
    {
        public EmplazamientosTiposEdificiosPaisesController()
            : base()
        {

        }

        public bool CompruebaDuplicadoCrea(long TipoID, string Pais, string Anio, double Coste, double Descuento, double inflacion, String Moneda)
        {
            //long categoriaID = 0;
            EmplazamientosTiposEdificiosPaisesController cCategoriasTecnica = new EmplazamientosTiposEdificiosPaisesController();
            List<EmplazamientosTiposEdificiosPaises> lista = new List<EmplazamientosTiposEdificiosPaises>();
            EmplazamientosTiposEdificiosPaises dato = new EmplazamientosTiposEdificiosPaises();

            MonedasController CMonedas = new MonedasController();
            Monedas moneda = new Monedas();
            PaisesController cPaises = new PaisesController();
            Paises pais = new Paises();
            bool correcta = false;


            try
            {


                if (TipoID != -1)
                {
                    long PaisID = cPaises.GetPaisByNombre(Pais);
                    if (PaisID != -1)
                    {
                        long MonedaID = CMonedas.GetMonedaBySimbolo(Moneda);
                        if (MonedaID != -1)
                        {
                            lista = (from c in Context.EmplazamientosTiposEdificiosPaises where c.EmplazamientoTipoEdificioID == TipoID && c.PaisID == PaisID && c.MonedaID == MonedaID && c.Anyo == Convert.ToInt16(Anio) && c.CostoPorDesmantelamiento == Coste select c).ToList();

                            if (lista != null && lista.Count != 0)
                            {
                                correcta = false;
                            }
                            else
                            {
                                dato = new EmplazamientosTiposEdificiosPaises();
                                dato.EmplazamientoTipoEdificioID = TipoID;
                                dato.PaisID = PaisID;
                                dato.MonedaID = MonedaID;
                                dato.CostoPorDesmantelamiento = Coste;
                                dato.Anyo = Convert.ToInt16(Anio);
                                dato.TasaDescuento = Descuento;
                                dato.TasaInflacion = inflacion;
                                dato.Activo = true;
                                cCategoriasTecnica.AddItem(dato);
                                correcta = false;
                            }
                        }
                    }
                }


            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return false;
            }

            return correcta;
        }
        public bool RegistroDefecto(long EmplazamientoTipoEdificioPaisID)
        {
            EmplazamientosTiposEdificiosPaises dato = new EmplazamientosTiposEdificiosPaises();
            EmplazamientosTiposEdificiosPaisesController cController = new EmplazamientosTiposEdificiosPaisesController();
            bool defecto = false;

            dato = cController.GetItem("Defecto == true && EmplazamientoTipoEdificioPaisID == " + EmplazamientoTipoEdificioPaisID.ToString());

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

        public List<Vw_EmplazamientosTiposEdificiosPaises> GetTipoEdificioID(long lTipo)
        {
            List<Vw_EmplazamientosTiposEdificiosPaises> listaDatos;

            listaDatos = (from c in Context.Vw_EmplazamientosTiposEdificiosPaises where c.EmplazamientoTipoEdificioID == lTipo orderby c.Pais select c).ToList();

            return listaDatos;
        }

        public EmplazamientosTiposEdificiosPaises GetDefault(long lClienteID)
        {
            EmplazamientosTiposEdificiosPaises oTipo;

            try
            {
                oTipo = (from c in Context.EmplazamientosTiposEdificiosPaises where c.Defecto && (c.ClienteID == lClienteID || c.ClienteID == null) select c).First();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                oTipo = null;
            }

            return oTipo;
        }

    }
}