using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using TreeCore.Data;

namespace CapaNegocio
{
    public class BancosController : GeneralBaseController<Bancos, TreeCoreContext>, IBasica<Bancos>
    {
        public BancosController()
            : base()
        { }

        public bool RegistroVinculado(long conflictividadID)
        {
            bool existe = true;
         

            return existe;
        }

        public bool RegistroDuplicado(string Banco, long clienteID)
        {
            bool isExiste = false;
            List<Bancos> datos;

            datos = (from c in Context.Bancos where (c.Banco == Banco && c.ClienteID == clienteID) select c).ToList<Bancos>();

            if (datos.Count > 0)
            {
                isExiste = true;
            }

            return isExiste;
        }

        public bool RegistroDefecto(long bancoID)
        {
            Bancos dato;

            dato = (from c 
                    in Context.Bancos 
                    where c.Defecto && 
                            c.BancoID == bancoID 
                    select c).First();

            return (dato != null);
        }

        public Bancos GetDefault(long ClienteID)
        {
            Bancos banco;
            try
            {
                banco = (from c 
                         in Context.Bancos 
                         where c.Defecto && 
                                c.ClienteID == ClienteID 
                         select c).First();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                banco = null;
            }
            return banco;
        }

        public Bancos GetBancoByNombre(string sNombre)
        {
            Bancos datos = null;
            List<Bancos> lista = null;
            try
            {
                datos = new Bancos();
                lista = (from c in Context.Bancos where (c.Banco == sNombre) select c).ToList();
                if (lista != null && lista.Count > 0)
                {
                    datos = lista.ElementAt(0);
                }
                else
                {
                    datos = null;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                datos = null;
            }
            return datos;
        }
    }
}