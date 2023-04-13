using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using TreeCore.Data;
using System.Data.SqlClient;
using Tree.Linq.GenericExtensions;

namespace CapaNegocio
{
    public class GruposAccesosWebController : GeneralBaseController<GruposAccesosWeb, TreeCoreContext>, IBasica<GruposAccesosWeb>
    {
        public GruposAccesosWebController()
            : base()
        { }

        public bool RegistroVinculado(long GrupoAccesoWebID)
        {
            bool existe = true;
         

            return existe;
        }

        public bool RegistroDuplicado(string sURL, long clienteID)
        {
            bool isExiste = false;
            List<GruposAccesosWeb> datos = new List<GruposAccesosWeb>();


            datos = (from c in Context.GruposAccesosWeb where (c.URL == sURL && c.ClienteID == clienteID) select c).ToList<GruposAccesosWeb>();

            if (datos.Count > 0)
            {
                isExiste = true;
            }

            return isExiste;
        }

        public bool RegistroDuplicadoLogin(string sURL, string sGrupo, long clienteID)
        {
            bool isExiste = false;
            List<GruposAccesosWeb> datos = new List<GruposAccesosWeb>();


            datos = (from c in Context.GruposAccesosWeb where (c.URL == sURL || c.GrupoAcceso == sGrupo) && c.ClienteID == clienteID select c).ToList<GruposAccesosWeb>();

            if (datos.Count > 0)
            {
                isExiste = true;
            }

            return isExiste;
        }

        public bool RegistroDefecto(long id)
        {
            throw new NotImplementedException();
        }

        public GruposAccesosWeb GetDefault(long clienteID) {
            GruposAccesosWeb oGruposAccesosWeb;
            try
            {
                oGruposAccesosWeb = (from c in Context.GruposAccesosWeb where c.Defecto && c.ClienteID == clienteID select c).First();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                oGruposAccesosWeb = null;
            }
            return oGruposAccesosWeb;
        }

        public List<GruposAccesosWeb> getListaByIDs(List<long> lIDs, long? lClienteID)
        {
            List<GruposAccesosWeb> listaDatos;

            try
            {
                listaDatos = (from c in Context.GruposAccesosWeb where lIDs.Contains(c.GrupoAccesoWebID) && c.Activo == true && c.ClienteID.Equals(lClienteID) select c).ToList();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                listaDatos = null;
            }

            return listaDatos;
        }

        public long getIDByGrupo (string sGrupo)
        {
            long lGrupoID;

            try
            {
                lGrupoID = (from c in Context.GruposAccesosWeb where c.GrupoAcceso == sGrupo select c.GrupoAccesoWebID).First();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                lGrupoID = 0;
            }

            return lGrupoID;
        }

        public string getURLByGrupoAcceso (string sGrupoAcceso)
        {
            string sURL;

            try
            {
                sURL = (from c in Context.GruposAccesosWeb where c.GrupoAcceso == sGrupoAcceso select c.URL).First();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                sURL = null;
            }

            return sURL;
        }
    }
}