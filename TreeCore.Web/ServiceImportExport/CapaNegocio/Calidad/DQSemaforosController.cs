using System;
using System.Data;
using TreeCore.Data;
using System.Collections.Generic;
using Tree.Linq.GenericExtensions;
using System.Linq;

namespace CapaNegocio
{
    public class DQSemaforosController : GeneralBaseController<DQSemaforos, TreeCoreContext>
    {
        public DQSemaforosController()
             : base()
        {

        }

        public List<DQSemaforos> GetAllSemaforosActivos(bool bActivo)
        {
            List<DQSemaforos> lista = null;

            try
            {
                if (bActivo)
                {
                    lista = (from c in Context.DQSemaforos where c.Activo == true select c).ToList();
                }
                else
                {
                    lista = (from c in Context.DQSemaforos select c).ToList();
                }
            }
            catch (Exception ex)
            {
                return lista;
                log.Error(ex.Message);
            }

            return lista;
        }

        public bool RegistroDuplicado(string sCalidad)
        {
            bool isExiste = false;
            List<DQSemaforos> datos = new List<DQSemaforos>();


            datos = (from c in Context.DQSemaforos where (c.DQSemaforo == sCalidad) select c).ToList<DQSemaforos>();

            if (datos.Count > 0)
            {
                isExiste = true;
            }

            return isExiste;
        }
    }
}