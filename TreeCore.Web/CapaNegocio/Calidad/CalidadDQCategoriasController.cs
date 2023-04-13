using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using TreeCore.Data;
using TreeCore.Clases;

namespace CapaNegocio
{
    public class DQCategoriasController : GeneralBaseController<DQCategorias, TreeCoreContext>, IGestionBasica<DQCategorias>
    {
        public DQCategoriasController()
            : base()
        { }

        public InfoResponse Add(DQCategorias oCategoria)
        {
            InfoResponse oResponse;
            try
            {
                if (RegistroDuplicado(oCategoria))
                {
                    oResponse = new InfoResponse
                    {
                        Result = false,
                        Code = "",
                        Description = Comun.LogRegistroExistente,
                        Data = null
                    };
                }
                else
                {
                    oResponse = AddEntity(oCategoria);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                oResponse = new InfoResponse
                {
                    Result = false,
                    Code = "",
                    Description = Comun.strMensajeGenerico,
                    Data = null
                };
            }
            return oResponse;
        }

        public InfoResponse Update(DQCategorias oCategoria)
        {
            InfoResponse oResponse;
            try
            {
                if (RegistroDuplicado(oCategoria))
                {
                    oResponse = new InfoResponse
                    {
                        Result = false,
                        Code = "",
                        Description = Comun.LogRegistroExistente,
                        Data = null
                    };
                }
                else
                {
                    oResponse = UpdateEntity(oCategoria);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                oResponse = new InfoResponse
                {
                    Result = false,
                    Code = "",
                    Description = Comun.strMensajeGenerico,
                    Data = null
                };
            }
            return oResponse;
        }

        public InfoResponse Delete(DQCategorias oCategoria)
        {
            InfoResponse oResponse;

            try
            {
                oResponse = DeleteEntity(oCategoria);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                oResponse = new InfoResponse
                {
                    Result = false,
                    Code = "",
                    Description = Comun.strMensajeGenerico,
                    Data = null
                };
            }
            return oResponse;
        }

        public InfoResponse ModificarActivar(DQCategorias oCategoria)
        {
            InfoResponse oResponse;
            try
            {
                oCategoria.Activa = !oCategoria.Activa;
                oResponse = Update(oCategoria);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                oResponse = new InfoResponse
                {
                    Result = false,
                    Code = "",
                    Description = Comun.strMensajeGenerico,
                    Data = null
                };
            }
            return oResponse;
        }

        public bool RegistroDuplicado(DQCategorias oCategoria)
        {
            bool isExiste = false;
            List<DQCategorias> datos;

            datos = (from c in Context.DQCategorias where (c.DQCategoria == oCategoria.DQCategoria && c.ClienteID == oCategoria.ClienteID && c.DQCategoriaID != oCategoria.DQCategoriaID) select c).ToList<DQCategorias>();

            if (datos.Count > 0)
            {
                isExiste = true;
            }

            return isExiste;
        }

        public bool RegistroVinculado(long DQCategoriaID)
        {
            bool bExiste = true;


            return bExiste;
        }

        public List<DQCategorias> GetAllActivos(long clienteID)
        {
            List<DQCategorias> lista = null;

            try
            {
                lista = (from c in Context.DQCategorias
                         where
                            c.Activa &&
                            c.ClienteID == clienteID
                         orderby c.DQCategoria
                         select c).ToList();
            }
            catch (Exception ex)
            {
                return lista;
                log.Error(ex.Message);
            }

            return lista;
        }

        public long getIDByName(string sName)
        {
            long lCategoriaID;

            try
            {
                lCategoriaID = (from c in Context.DQCategorias where c.DQCategoria == sName select c.DQCategoriaID).First();
            }
            catch (Exception ex)
            {
                lCategoriaID = 0;
                log.Error(ex.Message);
            }

            return lCategoriaID;
        }


    }
}