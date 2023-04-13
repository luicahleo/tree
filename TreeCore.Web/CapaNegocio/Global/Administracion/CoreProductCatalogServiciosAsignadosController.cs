using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using TreeCore.Data;
using Ext.Net;
using System.IO;
using Tree.Linq.GenericExtensions;
using System.Web;
using System.Data.SqlClient;
using Newtonsoft.Json.Linq;
using TreeCore.Clases;
using System.Globalization;
using Newtonsoft.Json;

namespace CapaNegocio
{
    public class CoreProductCatalogServiciosAsignadosController : GeneralBaseController<CoreProductCatalogServiciosAsignados, TreeCoreContext>, IGestionBasica<CoreProductCatalogServiciosAsignados>
    {
        public CoreProductCatalogServiciosAsignadosController()
            : base()
        { }
        public InfoResponse Add(CoreProductCatalogServiciosAsignados oEntidad)
        {
            InfoResponse oResponse = new InfoResponse();
            try
            {


                oResponse = AddEntity(oEntidad);

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

        public InfoResponse Update(CoreProductCatalogServiciosAsignados oEntidad)
        {
            InfoResponse oResponse = new InfoResponse();
            try
            {

                oResponse = UpdateEntity(oEntidad);

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
        
        public InfoResponse Delete(CoreProductCatalogServiciosAsignados oEntidad)
        {
            InfoResponse oResponse = new InfoResponse();
            try
            {

                oResponse = DeleteEntity(oEntidad);

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

        public bool RegistroDuplicado(CoreProductCatalogServiciosAsignados oBanco)
        {
            bool isExiste = false;
            List<CoreProductCatalogServiciosAsignados> datos;

            datos = (from c in Context.CoreProductCatalogServiciosAsignados where (c.CoreProductCatalogServicioAsignadoID != oBanco.CoreProductCatalogServicioAsignadoID) select c).ToList<CoreProductCatalogServiciosAsignados>();

            if (datos.Count > 0)
            {
                isExiste = true;
            }

            return isExiste;
        }
        public InfoResponse AnadirServicios(CoreProductCatalogServiciosAsignados servicio, long catalogoID, long servicioID, bool check, long usuarioID, bool agregar)
        {
            CoreProductCatalogsController cCatalogos = new CoreProductCatalogsController();
            CoreProductCatalogServiciosPacksAsignadosController cPacks = new CoreProductCatalogServiciosPacksAsignadosController();
            InfoResponse oResponse = new InfoResponse();
            cCatalogos.SetDataContext(this.Context);
            cPacks.SetDataContext(this.Context);

            List<long> listaIDs = getItemsFiltradosByCatalogoID(catalogoID);
            CoreProductCatalogs oCatalogo = cCatalogos.GetItem(catalogoID);
            oCatalogo.FechaUltimaModificacion = DateTime.Now;
            oCatalogo.UsuarioModificadorID = usuarioID;
            cCatalogos.Add(oCatalogo);
            if (check)
            {
                if (listaIDs.Contains(servicioID))
                {
                    Vw_CoreProductCatalogServiciosPacksAsignados oPackAsign = cPacks.getItemByServicioID(servicioID);

                    if (oPackAsign != null)
                    {

                        oResponse.Description = "This service is already assigned to the pack: " + oPackAsign.Nombre;
                        oResponse.Result = false;
                        //direct.Success = false;
                        //direct.Result = "Duplicado";
                        //return direct;
                    }
                }
                else
                {
                    if (agregar)
                    {
                        oResponse = Add(servicio);

                    }
                    else
                    {
                        oResponse = Update(servicio);
                    }


                }
            }
            else
            {
                oResponse = Delete(servicio);
                
            }

            if (oResponse.Result)
            {
                oResponse = SubmitChanges();

            }
            else
            {
                DiscardChanges();
            }

            return oResponse;
        }

        public CoreProductCatalogServiciosAsignados getItemByCatalogoID(long lCatalogoID)
        {
            CoreProductCatalogServiciosAsignados oDato = null;

            try
            {
                oDato = (from c in Context.CoreProductCatalogServiciosAsignados
                         where c.CoreProductCatalogID == lCatalogoID
                         select c).First();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                oDato = null;
            }

            return oDato;
        }

        public Vw_CoreProductCatalogServiciosAsignados getItemByServicioID(long lServicioID)
        {
            Vw_CoreProductCatalogServiciosAsignados oDato = null;

            try
            {
                oDato = (from c in Context.Vw_CoreProductCatalogServiciosAsignados
                         where c.CoreProductCatalogServicioID == lServicioID
                         select c).First();
            }
            catch (Exception)
            {
                oDato = null;
            }

            return oDato;
        }

        public List<Vw_CoreProductCatalogServiciosAsignados> getItemsByServicioID(long lServicioID)
        {
            List<Vw_CoreProductCatalogServiciosAsignados> oDato = null;

            try
            {
                oDato = (from c in Context.Vw_CoreProductCatalogServiciosAsignados
                         where c.CoreProductCatalogServicioID == lServicioID
                         select c).ToList();
            }
            catch (Exception ex)
            {
                oDato = null;
            }

            return oDato;
        }

        public List<Vw_CoreProductCatalogServiciosAsignados> getItemsByCatalogoID(long lCatalogoID)
        {
            List<Vw_CoreProductCatalogServiciosAsignados> oDato = null;

            try
            {
                oDato = (from c in Context.Vw_CoreProductCatalogServiciosAsignados
                         where c.CoreProductCatalogID == lCatalogoID
                         select c).ToList();
            }
            catch (Exception ex)
            {
                oDato = null;
            }

            return oDato;
        }
        public long getMonedaID(long lCatalogoID)
        {
            long oDato = 0;
            try
            {
                oDato = (from c in Context.CoreProductCatalogs
                         where c.CoreProductCatalogID == lCatalogoID
                         select c.MonedaID).First();
            }
            catch (Exception ex)
            {
                oDato = 0;
            }

            return oDato;
        }

        public string getMonedaByCatalogoID(long lCatalogoID)
        {
            string sMoneda = null;

            try
            {
                sMoneda = (from c in Context.Vw_CoreProductCatalogServiciosAsignados
                           where c.CoreProductCatalogID == lCatalogoID
                           select c.Moneda).First();

                switch (sMoneda)
                {
                    case "EURO":
                        sMoneda = "€";
                        break;
                    case "DOLAR":
                        sMoneda = "$";
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                sMoneda = null;
            }

            return sMoneda;
        }

        public void ExportarProductCatalogPrecios(string sArchivo, string sNombre, List<String> listaTraducciones, List<String> listaColumnas)
        {
            using (var workbook = DocumentFormat.OpenXml.Packaging.SpreadsheetDocument.Create(sArchivo, DocumentFormat.OpenXml.SpreadsheetDocumentType.Workbook))
            {
                var workbookPart = workbook.AddWorkbookPart();
                workbook.WorkbookPart.Workbook = new DocumentFormat.OpenXml.Spreadsheet.Workbook();
                workbook.WorkbookPart.Workbook.Sheets = new DocumentFormat.OpenXml.Spreadsheet.Sheets();

                //Sheets
                var sheetPart = workbook.WorkbookPart.AddNewPart<DocumentFormat.OpenXml.Packaging.WorksheetPart>();
                var sheetData = new DocumentFormat.OpenXml.Spreadsheet.SheetData();
                sheetPart.Worksheet = new DocumentFormat.OpenXml.Spreadsheet.Worksheet(sheetData);

                DocumentFormat.OpenXml.Spreadsheet.Sheets sheets = workbook.WorkbookPart.Workbook.GetFirstChild<DocumentFormat.OpenXml.Spreadsheet.Sheets>();
                string relationshipId = workbook.WorkbookPart.GetIdOfPart(sheetPart);

                DocumentFormat.OpenXml.Spreadsheet.Sheet sheet = new DocumentFormat.OpenXml.Spreadsheet.Sheet()
                {
                    Id = workbook.WorkbookPart.GetIdOfPart(sheetPart),
                    SheetId = 1,
                    Name = sNombre
                };

                sheets.Append(sheet);

                //Heading
                DocumentFormat.OpenXml.Spreadsheet.Row FilaValoresPosibles = new DocumentFormat.OpenXml.Spreadsheet.Row();
                DocumentFormat.OpenXml.Spreadsheet.Row FilaCabecera = new DocumentFormat.OpenXml.Spreadsheet.Row();
                DocumentFormat.OpenXml.Spreadsheet.Row Fila = new DocumentFormat.OpenXml.Spreadsheet.Row();

                foreach (String sColumn in listaColumnas)
                {
                    DocumentFormat.OpenXml.Spreadsheet.Cell cell = new DocumentFormat.OpenXml.Spreadsheet.Cell();
                    cell.DataType = DocumentFormat.OpenXml.Spreadsheet.CellValues.String;
                    cell.CellValue = new DocumentFormat.OpenXml.Spreadsheet.CellValue(sColumn);
                    FilaCabecera.AppendChild(cell);
                }

                sheetData.AppendChild(FilaCabecera);

                foreach (string sValor in listaTraducciones)
                {
                    DocumentFormat.OpenXml.Spreadsheet.Cell cell = new DocumentFormat.OpenXml.Spreadsheet.Cell();
                    cell.DataType = DocumentFormat.OpenXml.Spreadsheet.CellValues.String;
                    cell.CellValue = new DocumentFormat.OpenXml.Spreadsheet.CellValue(sValor);
                    Fila.AppendChild(cell);

                    if (Fila.ChildElements.Count == listaColumnas.Count)
                    {
                        sheetData.AppendChild(Fila);
                        Fila = new DocumentFormat.OpenXml.Spreadsheet.Row();
                    }
                }

                workbook.Close();
            }
        }


        public List<Vw_CoreProductCatalogServiciosAsignados> GetAllServiciosAsigadosByCatalogoID(long clienteID, long catalogoID)
        {
            List<Vw_CoreProductCatalogServiciosAsignados> listaDatos = new List<Vw_CoreProductCatalogServiciosAsignados>();
            try
            {
                DataTable result;
                #region CADENA CONEXIÓN
#if SERVICESETTINGS
            string connectionString = System.Configuration.ConfigurationManager.AppSettings["Conexion"];
#elif TREEAPI
            string connectionString = TreeAPI.Properties.Settings.Default.Conexion;
#else
                string connectionString = TreeCore.Properties.Settings.Default.Conexion;
#endif
                #endregion
                using (var conn = new SqlConnection(connectionString))
                using (var cmd = new SqlCommand("sp_CoreProductCatalogServiciosByProductCatalogID", conn)
                {
                    CommandType = System.Data.CommandType.StoredProcedure
                })
                {
                    cmd.Parameters.AddWithValue("@ProductCatalogID", catalogoID);

                    var da = new SqlDataAdapter(cmd);
                    var ds = new DataSet();
                    da.Fill(ds);
                    result = ds.Tables[0];
                }
                long monedaID = getMonedaID(catalogoID);
                string simbolo = (from c in Context.Monedas
                                  where c.MonedaID == monedaID
                                  select c.Simbolo).First();
                Vw_CoreProductCatalogServiciosAsignados d;
                foreach (DataRow row in result.Rows)
                {
                    d = new Vw_CoreProductCatalogServiciosAsignados();
                    d.CoreProductCatalogServicioID = (long)row[0];
                    if (row[2].GetType().Name != "DBNull")
                    {
                        d.NombreCatalogServicio = (string)row[2];
                    }
                    if (row[3].GetType().Name != "DBNull")
                    {
                        d.CantidadCatalogServicio = (double)row[3];
                    }
                    if (row[4].GetType().Name != "DBNull")
                    {
                        d.CoreProductCatalogServicioTipoID = (long)row[4];

                    }
                    if (row[5].GetType().Name != "DBNull")
                    {
                        d.CoreProductCatalogServicioAsignadoID = (long)row[5];
                    }
                    if (row[6].GetType().Name != "DBNull")
                    {
                        d.Precio = (double)row[6];
                    }
                    if (row[7].GetType().Name != "DBNull")
                    {
                        d.NombreCatalogServicioTipo = (string)row[7];
                    }

                    d.Simbolo = simbolo + " / " + (string)row[8];
                    listaDatos.Add(d);
                }
            }
            catch (Exception ex)
            {
                listaDatos = null;
            }

            return listaDatos;
        }


        public CoreProductCatalogServiciosAsignados getServicioAsignadoIDByCatalogoIDYServicioID(long lCatalogoID, long servicioID)
        {
            CoreProductCatalogServiciosAsignados oDato;

            try
            {
                oDato = (from c in Context.CoreProductCatalogServiciosAsignados
                         where c.CoreProductCatalogID == lCatalogoID && c.CoreProductCatalogServicioID == servicioID
                         select c).First();
            }
            catch (Exception)
            {
                oDato = null;
            }

            return oDato;
        }

        public List<Vw_CoreProductCatalogServiciosAsignados> AplicarFiltroInternoFormulario(int pageSize, int curPage, out int total, DataSorter[] sorters, string s,
            Hidden hdStringBuscador, Hidden hdIDServicioBuscador, bool bDescarga, Ext.Net.GridHeaderContainer columnModel, string listaExcluidos, long catalogoID, long clienteID)
        {
            //List<JsonObject> items = new List<JsonObject>();
            //List<string> listaVacia = new List<string>();

            //string textoBuscado = (!hdStringBuscador.IsEmpty) ? Convert.ToString(hdStringBuscador.Value) : null;
            //long? IdBuscado = (!hdIDEmplazamientoBuscador.IsEmpty) ? Convert.ToInt64(hdIDEmplazamientoBuscador.Value) : new long?();


            //DataTable dt = GetAllServiciosAsigadosByCatalogoID(clienteID, catalogoID, textoBuscado, IdBuscado);

            //List<JsonObject> sites = new List<JsonObject>();
            //sites = GetAllServiciosAsigadosByCatalogoID(14, 2);
            //if (dt != null)
            //{
            //sites = getListJsonObject(dt, bDescarga, columnModel, listaExcluidos);
            //}


            //break;
            //}

            total = 0;
            Vw_CoreProductCatalogServiciosAsignados oServicio = new Vw_CoreProductCatalogServiciosAsignados();
            JsonObject oJson = new JsonObject();
            List<JsonObject> listaServicios = new List<JsonObject>();
            List<Vw_CoreProductCatalogServiciosAsignados> listaResultados = new List<Vw_CoreProductCatalogServiciosAsignados>();

            string textoBuscado = (!hdStringBuscador.IsEmpty) ? Convert.ToString(hdStringBuscador.Value) : null;
            long? IdBuscado = (!hdIDServicioBuscador.IsEmpty) ? Convert.ToInt64(hdIDServicioBuscador.Value) : new long?();

            if (textoBuscado != null || IdBuscado != null)
            {
                if (IdBuscado != null)
                {
                    oServicio = GetItem<Vw_CoreProductCatalogServiciosAsignados>("CoreProductCatalogServicioID=" + IdBuscado);
                    oJson = new JsonObject();

                    if (bDescarga && columnModel != null)
                    {
                        for (int i = 0; i < columnModel.Columns.Count; i++)
                        {
                            if (!listaExcluidos.Contains(columnModel.Columns[i].DataIndex))
                            {
                                string sValor = columnModel.Columns[i].DataIndex;
                                System.Reflection.PropertyInfo propiedad = oServicio.GetType().GetProperty(sValor);

                                if (propiedad.GetValue(oServicio, null) != null)
                                {
                                    if ((propiedad.GetValue(oServicio, null)).GetType().ToString() == "System.DateTime")
                                    {
                                        string sContenido = ((DateTime)(propiedad.GetValue(oServicio, null))).ToString(Comun.FORMATO_FECHA, CultureInfo.InvariantCulture);
                                        oJson.Add(columnModel.Columns[i].DataIndex, sContenido);
                                    }
                                    else
                                    {
                                        oJson.Add(columnModel.Columns[i].DataIndex, propiedad.GetValue(oServicio, null));
                                    }
                                }
                            }
                        }
                    }
                    else
                    {

                    }

                    listaResultados.Add(oServicio);
                }
                else if (textoBuscado != null)
                {
                    listaResultados = getListaByFiltro(textoBuscado);
                }
            }
            else
            {
                listaResultados = GetItemsList<Vw_CoreProductCatalogServiciosAsignados>();
                listaResultados = GetAllServiciosAsigadosByCatalogoID(clienteID, catalogoID);
                listaResultados = listaResultados.OrderBy(c => c.NombreCatalogServicio).OrderBy(c => c.NombreCatalogServicioTipo).ToList();
            }



            total = listaServicios.Count;

            return listaResultados;


        }
        public List<Vw_CoreProductCatalogServiciosAsignados> getListaByFiltro(string sTexto)
        {
            List<Vw_CoreProductCatalogServiciosAsignados> listaDatos = null;

            try
            {
                listaDatos = (from c in Context.Vw_CoreProductCatalogServiciosAsignados where c.CodigoProductCatalogServicio.ToString().Contains(sTexto) || c.NombreCatalogServicio.ToString().Contains(sTexto) select c).ToList();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                listaDatos = null;
            }

            return listaDatos;
        }

        public Vw_CoreProductCatalogServiciosAsignados getItemByNombreServicio(string sNombreServicio)
        {
            Vw_CoreProductCatalogServiciosAsignados oDato = null;

            try
            {
                oDato = (from c in Context.Vw_CoreProductCatalogServiciosAsignados where c.NombreCatalogServicio == sNombreServicio select c).First();
            }
            catch (Exception ex)
            {
                oDato = null;
                log.Error(ex.Message);
            }

            return oDato;
        }

        public List<long> getItemsFiltradosByCatalogoID(long lCatalogoID)
        {
            List<long> listaIDs = null;

            try
            {
                listaIDs = (from servAsign in Context.CoreProductCatalogServiciosPacksAsignados
                            join packs in Context.CoreProductCatalogPacksAsignados on servAsign.CoreProductCatalogPackID equals packs.CoreProductCatalogPackID
                            where packs.CoreProductCatalogID == lCatalogoID
                            select servAsign.CoreProductCatalogServicioID).Distinct<long>().ToList();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                listaIDs = null;
            }

            return listaIDs;
        }

        public long getValorByValoresID(long lServicioID, long lCatalogoID)
        {
            long lServicioAsignID;

            try
            {
                lServicioAsignID = (from c in Context.CoreProductCatalogServiciosAsignados where c.CoreProductCatalogServicioID == lServicioID && c.CoreProductCatalogID == lCatalogoID select c.CoreProductCatalogServicioAsignadoID).First();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                lServicioAsignID = 0;
            }

            return lServicioAsignID;
        }

        public List<long> GetAllServiciosByCatalogoID(long lCatalogoID)
        {
            List<long> listaIDs;

            try
            {
                listaIDs = (from c in Context.CoreProductCatalogServiciosAsignados where c.CoreProductCatalogID == lCatalogoID select c.CoreProductCatalogServicioID).ToList();
            }
            catch (Exception ex)
            {
                listaIDs = null;
            }

            return listaIDs;
        }

    }

}