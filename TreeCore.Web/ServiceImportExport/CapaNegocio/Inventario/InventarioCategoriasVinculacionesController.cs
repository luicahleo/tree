using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using TreeCore.Data;

namespace CapaNegocio
{
    public class InventarioCategoriasVinculacionesController : GeneralBaseController<InventarioCategoriasVinculaciones, TreeCoreContext>
    {
        public InventarioCategoriasVinculacionesController()
            : base()
        {

        }

        public List<InventarioCategoriasVinculaciones> GetRelacionesFromEmplazamientoTipo(long? lEmplazamientoTipoID)
        {
            List<InventarioCategoriasVinculaciones> listaDatos;
            try
            {
                listaDatos = (from c in Context.InventarioCategoriasVinculaciones where !(c.EmplazamientoTipoID.HasValue) || c.EmplazamientoTipoID == lEmplazamientoTipoID select c).ToList();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                listaDatos = null;
            }
            return listaDatos;
        }

        public InventarioCategoriasVinculaciones GetVinculacionDefecto(long CategoriaID, long? CategoriaPadreID, long? lEmplazamientoTipoID)
        {
            List<InventarioCategoriasVinculaciones> listaDatos;
            InventarioCategoriasVinculaciones oDato;
            try
            {
                listaDatos = (from c in Context.InventarioCategoriasVinculaciones 
                              where 
                                    !(c.EmplazamientoTipoID.HasValue) && 
                                    c.Activo && 
                                    ((CategoriaPadreID != null) ? c.InventarioCategoriaPadreID == CategoriaPadreID : !(c.InventarioCategoriaPadreID.HasValue)) && 
                                    c.InventarioCategoriaID == CategoriaID 
                              select c).ToList();
                if (listaDatos.Count == 0)
                {
                    listaDatos = (from c in Context.InventarioCategoriasVinculaciones 
                                  where 
                                        c.EmplazamientoTipoID == lEmplazamientoTipoID && 
                                        c.Activo && 
                                        ((CategoriaPadreID != null) ? c.InventarioCategoriaPadreID == CategoriaPadreID : !(c.InventarioCategoriaPadreID.HasValue)) && 
                                        c.InventarioCategoriaID == CategoriaID 
                                  select c).ToList();
                    if (listaDatos.Count == 0)
                    {
                        oDato = null;
                    }
                    else
                    {
                        oDato = listaDatos.FirstOrDefault();
                    }
                }
                else
                {
                    oDato = listaDatos.FirstOrDefault();
                }

            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                oDato = null;
            }
            return oDato;
        }
        public InventarioCategoriasVinculaciones GetVinculacionDefecto(long CategoriaID, long? CategoriaPadreID, long? lEmplazamientoTipoID, List<InventarioCategoriasVinculaciones> listaVinculaciones)
        {
            List<InventarioCategoriasVinculaciones> listaDatos;
            InventarioCategoriasVinculaciones oDato;
            try
            {
                listaDatos = (from c in listaVinculaciones 
                              where 
                                    !(c.EmplazamientoTipoID.HasValue) && 
                                    c.Activo && 
                                    ((CategoriaPadreID != null) ? c.InventarioCategoriaPadreID == CategoriaPadreID : !(c.InventarioCategoriaPadreID.HasValue)) && 
                                    c.InventarioCategoriaID == CategoriaID 
                              select c).ToList();
                if (listaDatos.Count == 0)
                {
                    listaDatos = (from c in listaVinculaciones
                                  where 
                                        c.EmplazamientoTipoID == lEmplazamientoTipoID && 
                                        c.Activo && 
                                        ((CategoriaPadreID != null) ? c.InventarioCategoriaPadreID == CategoriaPadreID : !(c.InventarioCategoriaPadreID.HasValue)) && 
                                        c.InventarioCategoriaID == CategoriaID 
                                  select c).ToList();
                    if (listaDatos.Count == 0)
                    {
                        oDato = null;
                    }
                    else
                    {
                        oDato = listaDatos.First();
                    }
                }
                else
                {
                    oDato = listaDatos.First();
                }

            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                oDato = null;
            }
            return oDato;
        }

        public InventarioCategoriasVinculaciones GetVinculacion(long CategoriaID, long? CategoriaPadreID, long? lEmplazamientoTipoID)
        {
            List<InventarioCategoriasVinculaciones> oListaDato;
            InventarioCategoriasVinculaciones oDato;
            try
            {
                oListaDato = (from c in Context.InventarioCategoriasVinculaciones
                              where
     (CategoriaPadreID.HasValue) ? (c.InventarioCategoriaPadreID == CategoriaPadreID) : !(c.InventarioCategoriaPadreID.HasValue) &&
     (lEmplazamientoTipoID.HasValue) ? (c.EmplazamientoTipoID == lEmplazamientoTipoID) : !(c.EmplazamientoTipoID.HasValue)
                              select c).ToList();
                oDato = oListaDato.FindAll(item => item.InventarioCategoriaID == CategoriaID).FirstOrDefault();
                //                oDato = (from c in Context.InventarioCategoriasVinculaciones
                //                         where
                //c.InventarioCategoriaPadreID == CategoriaPadreID &&
                //(c.EmplazamientoTipoID == lEmplazamientoTipoID)&&
                //c.InventarioCategoriaID == CategoriaID
                //                         select c).ToList();

            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                oDato = null;
            }
            return oDato;
        }
        public InventarioCategoriasVinculaciones GetVinculacionGlobal(long CategoriaID, long? CategoriaPadreID, long? lEmplazamientoTipoID)
        {
            List<InventarioCategoriasVinculaciones> oListaDato;
            InventarioCategoriasVinculaciones oDato;
            try
            {
                oListaDato = (from c in Context.InventarioCategoriasVinculaciones
                              where
     (CategoriaPadreID.HasValue) ? (c.InventarioCategoriaPadreID == CategoriaPadreID) : !(c.InventarioCategoriaPadreID.HasValue) &&
     (lEmplazamientoTipoID.HasValue) ? (c.EmplazamientoTipoID == lEmplazamientoTipoID) : !(c.EmplazamientoTipoID.HasValue)
                              select c).ToList();
                oDato = oListaDato.FindAll(item => item.InventarioCategoriaID == CategoriaID).FirstOrDefault();
                //                oDato = (from c in Context.InventarioCategoriasVinculaciones
                //                         where
                //c.InventarioCategoriaPadreID == CategoriaPadreID &&
                //(c.EmplazamientoTipoID == lEmplazamientoTipoID)&&
                //c.InventarioCategoriaID == CategoriaID
                //                         select c).ToList();

            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                oDato = null;
            }
            return oDato;
        }

        public List<InventarioCategoriasVinculaciones> GetVinculacionesFromCategoria(long lInventarioCategoriasID)
        {
            List<InventarioCategoriasVinculaciones> listaDatos;
            try
            {
                listaDatos = (from c in Context.InventarioCategoriasVinculaciones where c.InventarioCategoriaID == lInventarioCategoriasID select c).ToList(); ;
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                listaDatos = null;
            }
            return listaDatos;
        }

        public List<Vw_InventarioCategoriasVinculaciones> GetVwVinculacionesFromCategoriaPadre(long? lInventarioCategoriasID, long? EmplazamientoTipoID, bool Activos)
        {
            List<Vw_InventarioCategoriasVinculaciones> listaDatos;
            try
            {
                if (Activos)
                {
                    listaDatos = (from c in Context.Vw_InventarioCategoriasVinculaciones where (lInventarioCategoriasID != null) ? c.InventarioCategoriaPadreID == lInventarioCategoriasID : !c.InventarioCategoriaPadreID.HasValue && (c.EmplazamientoTipoID == EmplazamientoTipoID || !c.EmplazamientoTipoID.HasValue) && c.Activo select c).ToList();

                }
                else
                {
                    listaDatos = (from c in Context.Vw_InventarioCategoriasVinculaciones where (lInventarioCategoriasID != null) ? c.InventarioCategoriaPadreID == lInventarioCategoriasID : !c.InventarioCategoriaPadreID.HasValue && (c.EmplazamientoTipoID == EmplazamientoTipoID || !c.EmplazamientoTipoID.HasValue) select c).ToList();

                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                listaDatos = null;
            }
            return listaDatos;
        }

        public List<InventarioCategoriasVinculaciones> GetVinculacionesFromCategoriaEmplazamientoTipo(long lInventarioCategoriasID, long emplazamientoTipoID)
        {
            List<InventarioCategoriasVinculaciones> listaDatos;
            try
            {
                if (emplazamientoTipoID != null && emplazamientoTipoID != 0)
                {
                    listaDatos = (from c in Context.InventarioCategoriasVinculaciones where (c.InventarioCategoriaID == lInventarioCategoriasID && c.EmplazamientoTipoID == emplazamientoTipoID) || c.EmplazamientoTipoID == null select c).ToList(); ;
                }
                else
                {
                    listaDatos = (from c in Context.InventarioCategoriasVinculaciones where c.InventarioCategoriaID == lInventarioCategoriasID && c.EmplazamientoTipoID == null select c).ToList(); ;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                listaDatos = null;
            }
            return listaDatos;
        }

        public List<InventarioCategoriasVinculaciones> GetVinculacionesFromCategoriaEmplazamientoTipoDiagrama( long emplazamientoTipoID)
        {
            List<InventarioCategoriasVinculaciones> listaDatos;
            try
            {
                if (emplazamientoTipoID != null && emplazamientoTipoID != 0)
                {
                    listaDatos = (from c in Context.InventarioCategoriasVinculaciones where (( c.EmplazamientoTipoID == emplazamientoTipoID) || c.EmplazamientoTipoID == null) 
                                   && c.Activo == true select c).Distinct().ToList(); ;
                }
                else
                {
                    listaDatos = (from c in Context.InventarioCategoriasVinculaciones  where  c.EmplazamientoTipoID == null 
                                  && c.Activo == true
                                  select c).Distinct().ToList(); ;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                listaDatos = null;
            }
            return listaDatos;
        }

        public List<long> GetListByTipoEmplazamientoTipoVinculacionesDiagrama(string EmplazamientoTipoID, long clienteID, List<long> InventarioTipoVinculacionesID)
        {
            long lEmplazamientoTipoID;
            List<long> listaDatos;
            long lInventarioTipoVinculacionID;
            List<InventarioCategoriasVinculacionesTiposVinculaciones> ListaTipoEmpVin = null;


            try
            {
                lEmplazamientoTipoID = long.Parse(EmplazamientoTipoID);
            }
            catch (Exception)
            {
                lEmplazamientoTipoID = 0;
            }
           

            try
            {
                listaDatos = (from c in Context.InventarioCategoriasVinculacionesTiposVinculaciones
                              where InventarioTipoVinculacionesID.Contains( c.InventarioTipoVinculacionID)
                              select c.InventarioCategoriaVinculacionID).Distinct().ToList();
            }
            catch (Exception)
            {
                listaDatos = null;
            }

            try
            {

                if (lEmplazamientoTipoID != 0)
                {
                    listaDatos = (from c in  Context.InventarioCategoriasVinculaciones 
                                  where (c.EmplazamientoTipoID == lEmplazamientoTipoID || (c.EmplazamientoTipoID == null)) //&& c.ClienteID == clienteID
                                  && listaDatos.Contains(Convert.ToInt64(c.InventarioCategoriaVinculacionID))
                                  select c.InventarioCategoriaVinculacionID).Distinct().ToList();
                }
                else
                {
                    listaDatos = (from c in  Context.InventarioCategoriasVinculaciones 
                                  where (c.EmplazamientoTipoID == null) //&& c.ClienteID == clienteID
                                  && listaDatos.Contains(Convert.ToInt64(c.InventarioCategoriaVinculacionID))
                                  select c.InventarioCategoriaVinculacionID).Distinct().ToList();
                }

                /*if (lEmplazamientoTipoID != 0)
                {
                    listaDatos = (from c in Context.InventarioCategorias
                                  join
                                   vin in Context.InventarioCategoriasVinculaciones on c.InventarioCategoriaID equals vin.InventarioCategoriaID
                                  where (c.EmplazamientoTipoID == lEmplazamientoTipoID || (c.EmplazamientoTipoID == null)) && c.ClienteID == clienteID
                                  && listaDatos.Contains(Convert.ToInt64(vin.InventarioCategoriaVinculacionID))
                                  select c.InventarioCategoriaID).Distinct().ToList();
                }
                else
                {
                    listaDatos = (from c in Context.InventarioCategorias
                                  join
vin in Context.InventarioCategoriasVinculaciones on c.InventarioCategoriaID equals vin.InventarioCategoriaID
                                  where  (c.EmplazamientoTipoID == null) && c.ClienteID == clienteID
                                  && listaDatos.Contains(Convert.ToInt64(vin.InventarioCategoriaVinculacionID))
                                  select c.InventarioCategoriaID).Distinct().ToList();
                }*/




            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                listaDatos = null;
            }
            return listaDatos;
        }


        public List<long> GetListTipoVinculacionesDiagrama(string InventarioTipoVinculacionID)
        {
            
            List<long> listaDatos;
            long lInventarioTipoVinculacionID;
        
            try
            {
                lInventarioTipoVinculacionID = long.Parse(InventarioTipoVinculacionID);
            }
            catch (Exception)
            {
                lInventarioTipoVinculacionID = 0;
            }

            try
            {
                listaDatos = (from c in Context.InventarioCategoriasVinculacionesTiposVinculaciones
                              where c.InventarioTipoVinculacionID == lInventarioTipoVinculacionID
                              select c.InventarioCategoriaVinculacionID).Distinct().ToList();
            }
            catch (Exception)
            {
                listaDatos = null;
            }
            return listaDatos;

        }


        public List<InventarioCategoriasVinculaciones> GetListByVinculacionesDiagrama(List<long> ListaVinculaciones)
        {
            List<InventarioCategoriasVinculaciones> listaDatos;
            try
            {

               
                    listaDatos = (from c in Context.InventarioCategoriasVinculaciones where ListaVinculaciones.Contains(Convert.ToInt64(c.InventarioCategoriaVinculacionID))
                                  select c).Distinct().ToList();
             }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                listaDatos = null;
            }
            return listaDatos;
        }

        public bool ComprobarCambioVinculacion(int TipoVinculacion, long CategoriaVinculacionID, long? CategoriaPadreID, out List<string> listaCodigosInvalidos) {
            List<long> listLong;
            List<long?> listLongN;
            List<string> listCodigo = new List<string>(); ;
            bool valido = true;
            try
            {
                switch (TipoVinculacion)
                {
                    case (int)Comun.TiposVinculaciones.Rel_1_1:
                        if (!CategoriaPadreID.HasValue)
                        {
                            listLongN = (from c in Context.Vw_InventarioElementosVinculaciones
                                                  where c.InventarioCategoriaVinculacionID == CategoriaVinculacionID
                                                  group c by c.EmplazamientoID into agrupaciones
                                                  where agrupaciones.Count() > 1
                                                  select agrupaciones.Key).ToList();
                            if (listLongN.Count > 0)
                            {
                                valido = false;
                                listCodigo = (from c in Context.Vw_InventarioElementosVinculaciones
                                                                where listLongN.Contains(c.EmplazamientoID) && c.InventarioCategoriaVinculacionID == CategoriaVinculacionID
                                                                select c.NumeroInventario).ToList();
                            }
                        }
                        else
                        {
                            listLongN = (from c in Context.Vw_InventarioElementosVinculaciones
                                                  where c.InventarioCategoriaVinculacionID == CategoriaVinculacionID
                                                  group c by c.InventarioElementoPadreID into agrupaciones
                                                  where agrupaciones.Count() > 1
                                                  select agrupaciones.Key).ToList();
                            if (listLongN.Count > 0)
                            {
                                valido = false;
                                listCodigo = (from c in Context.Vw_CoreInventarioElementos
                                              where listLongN.Contains(c.InventarioElementoID)
                                              select c.NumeroInventario).ToList();
                            }
                            else
                            {
                                listLong = (from c in Context.Vw_InventarioElementosVinculaciones
                                                   where c.InventarioCategoriaVinculacionID == CategoriaVinculacionID
                                                   group c by c.InventarioElementoID into agrupaciones
                                                   where agrupaciones.Count() > 1
                                                   select agrupaciones.Key).ToList<long>();
                                if (listLong.Count > 0)
                                {
                                    valido = false;
                                    listCodigo = (from c in Context.Vw_CoreInventarioElementos
                                                  where listLong.Contains(c.InventarioElementoID)
                                                  select c.NumeroInventario).ToList();
                                }
                            }
                        }
                        break;
                    case (int)Comun.TiposVinculaciones.Rel_1_N:
                        if (!CategoriaPadreID.HasValue)
                        {
                            valido = true;
                        }
                        else
                        {
                            listLong = (from c in Context.Vw_InventarioElementosVinculaciones
                                        where c.InventarioCategoriaVinculacionID == CategoriaVinculacionID
                                        group c by c.InventarioElementoID into agrupaciones
                                        where agrupaciones.Count() > 1
                                        select agrupaciones.Key).ToList<long>();
                            if (listLong.Count > 0)
                            {
                                valido = false;
                                listCodigo = (from c in Context.Vw_CoreInventarioElementos
                                              where listLong.Contains(c.InventarioElementoID)
                                              select c.NumeroInventario).ToList();
                            }
                        }
                        break;
                    case (int)Comun.TiposVinculaciones.Rel_N_1:
                        listLongN = (from c in Context.Vw_InventarioElementosVinculaciones
                                     where c.InventarioCategoriaVinculacionID == CategoriaVinculacionID
                                     group c by c.InventarioElementoPadreID into agrupaciones
                                     where agrupaciones.Count() > 1
                                     select agrupaciones.Key).ToList();
                        if (listLongN.Count > 0)
                        {
                            valido = false;
                            listCodigo = (from c in Context.Vw_CoreInventarioElementos
                                                            where listLongN.Contains(c.InventarioElementoID)
                                                            select c.NumeroInventario).ToList();
                        }
                        break;
                    case (int)Comun.TiposVinculaciones.Rel_N_M:
                        valido = true;
                        break;
                    default:
                        valido = true;
                        break;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                valido = false;
            }
            listaCodigosInvalidos = listCodigo;
            return valido;
        }

    }
}