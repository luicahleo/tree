using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using TreeCore.Data;

namespace CapaNegocio
{
    public class ServiciosProyectosController : GeneralBaseController<ServiciosProyectos, TreeCoreContext>, IBasica<ServiciosProyectos>
    {
        public ServiciosProyectosController()
            : base()
        { }

        public bool RegistroDefecto(long id)
        {
            throw new NotImplementedException();
        }

        public bool RegistroDuplicado(string nombre, long clienteID)
        {
            throw new NotImplementedException();
        }

        public bool RegistroVinculado(long id)
        {
            throw new NotImplementedException();
        }
    }
}