using System;
using System.Data;
using TreeCore.Data;
using System.Collections.Generic;
using Tree.Linq.GenericExtensions;
using System.Linq;

namespace CapaNegocio
{
    public class CoreConfiguracionHomeModulosController : GeneralBaseController<CoreConfiguracionHomeModulos, TreeCoreContext>
    {
        public CoreConfiguracionHomeModulosController()
            : base()
        { }

        public CoreConfiguracionHomeModulos GetConfiguracionByModulo(long clienteID, string pagina)
        {
            CoreConfiguracionHomeModulos config = new CoreConfiguracionHomeModulos();

            try
            {
                config = (from c in Context.CoreConfiguracionHomeModulos
                          join m in Context.Modulos on c.ModuloID equals m.ModuloID
                          where c.ClienteID == clienteID && m.Pagina == pagina.ToLower()
                          select c).FirstOrDefault();
            }
            catch (Exception ex)
            {
                config = null;
                log.Error(ex.Message);
            }

            return config;
        }
    }

    public class ConfiguracionHomeModulo {
        public List<long> categoriasIDs { get; set; }
        public List<long> operadoresIDs { get; set; }
        public List<long> estadosOperacionalesIDs { get; set; }
    }
}