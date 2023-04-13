using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using TreeCore.Data;

namespace CapaNegocio
{
    public class GlobalAntenasModelosController : GeneralBaseController<GlobalAntenasModelos, TreeCoreContext>, IBasica<GlobalAntenasModelos>
    {
        public GlobalAntenasModelosController()
            : base()
        { }

        public bool RegistroVinculado(long conflictividadID)
        {
            bool existe = true;


            return existe;
        }

        public bool RegistroDuplicado(string Modelo, long GlobalAntenaFabricanteID)
        {
            bool isExiste = false;
            List<GlobalAntenasModelos> datos = new List<GlobalAntenasModelos>();


            datos = (from c in Context.GlobalAntenasModelos where (c.Modelo == Modelo && c.GlobalAntenaFabricanteID == GlobalAntenaFabricanteID) select c).ToList<GlobalAntenasModelos>();

            if (datos.Count > 0)
            {
                isExiste = true;
            }

            return isExiste;
        }

        public bool RegistroDefecto(long GlobalAntenaModeloID)
        {
            GlobalAntenasModelos dato = new GlobalAntenasModelos();
            GlobalAntenasModelosController cController = new GlobalAntenasModelosController();
            bool defecto = false;

            dato = cController.GetItem("Defecto == true && GlobalAntenaModeloID == " + GlobalAntenaModeloID.ToString());

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

        public List<Vw_GlobalAntenasFabricantes> GetListVwOrderByGlobalAntenaFabricanteID()
        {
            List<Vw_GlobalAntenasFabricantes> globalAntenasFabricante;

            try
            {
                globalAntenasFabricante = (from c
                                           in Context.Vw_GlobalAntenasFabricantes
                                           orderby c.GlobalAntenaFabricanteID
                                           select c).ToList();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                globalAntenasFabricante = null;
            }

            return globalAntenasFabricante;
        }
    }
}