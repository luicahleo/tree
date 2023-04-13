using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using TreeCore.Data;
using TreeCore.Clases;

namespace CapaNegocio
{
    public class DQGroupsController : GeneralBaseController<DQGroups, TreeCoreContext>, IGestionBasica<DQGroups>
    {
        public DQGroupsController()
            : base()
        { }

        public InfoResponse Add(DQGroups oGrupo)
        {
            InfoResponse oResponse;
            try
            {
                if (RegistroDuplicado(oGrupo))
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
                    oResponse = AddEntity(oGrupo);
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

        public InfoResponse Update(DQGroups oGrupo)
        {
            InfoResponse oResponse;
            try
            {
                if (RegistroDuplicado(oGrupo))
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
                    oResponse = UpdateEntity(oGrupo);
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

        public InfoResponse Delete(DQGroups oGrupo)
        {
            InfoResponse oResponse;
            try
            {
                oResponse = DeleteEntity(oGrupo);
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

        public InfoResponse ModificarActivar(DQGroups oGrupo)
        {
            InfoResponse oResponse;
            try
            {
                oGrupo.Activo = !oGrupo.Activo;
                oResponse = Update(oGrupo);
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

        public bool RegistroDuplicado(DQGroups oGrupo)
        {
            bool isExiste = false;
            List<DQGroups> datos;

            datos = (from c in Context.DQGroups where (c.DQGroup == oGrupo.DQGroup && c.ClienteID == oGrupo.ClienteID && c.DQGroupID != oGrupo.DQGroupID) select c).ToList<DQGroups>();

            if (datos.Count > 0)
            {
                isExiste = true;
            }

            return isExiste;
        }

        public bool RegistroVinculado(long DQGroupID)
        {
            bool bExiste = true;


            return bExiste;
        }

        public List<Vw_DQGroups> getAllGroupActivos()
        {
            List<Vw_DQGroups> listaDatos = null;

            listaDatos = (from c in Context.Vw_DQGroups where c.Activo == true select c).ToList();

            return listaDatos;
        }

        public long getIDByName(string sGrupo)
        {
            long lGroupID = 0;

            lGroupID = (from c in Context.Vw_DQGroups where c.DQGroup == sGrupo select c.DQGroupID).First();

            return lGroupID;
        }

        public List<long> GetGroupsActivos()
        {
            List<long> listaID;

            try
            {
                listaID = (from c in Context.DQGroups where c.Activo select c.DQGroupID).ToList();
            }
            catch (Exception ex)
            {
                listaID = null;
                log.Error(ex.Message);
            }

            return listaID;
        }
    }
}