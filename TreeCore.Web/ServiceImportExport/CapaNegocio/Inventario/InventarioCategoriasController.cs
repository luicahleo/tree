using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using TreeCore.Data;
using System.Data.SqlClient;

namespace CapaNegocio
{
    public class InventarioCategoriasController : GeneralBaseController<InventarioCategorias, TreeCoreContext>, IBasica<InventarioCategorias>
    {
        public InventarioCategoriasController()
            : base()
        {

        }

        public bool RegistroDefecto(long id)
        {
            InventarioCategorias dato = new InventarioCategorias();
            InventarioCategoriasController cController = new InventarioCategoriasController();
            bool defecto = false;

            dato = cController.GetItem("Defecto == true && InventarioCategoriaID == " + id);

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

        public bool RegistroDuplicado(string nombre, long clienteID)
        {
            throw new NotImplementedException();
        }

        public bool RegistroVinculado(long id)
        {
            bool existe = true;


            return existe;
        }

        public List<InventarioCategorias> GetCategoriasByCategoriasIDs(List<long> listaIDs)
        {
            List<InventarioCategorias> listaDatos;
            try
            {
                listaDatos = (from c in Context.InventarioCategorias where listaIDs.Contains(c.InventarioCategoriaID) select c).ToList();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                listaDatos = null;
            }
            return listaDatos;
        }

        public List<InventarioCategorias> GetInventarioCategoriasByTipoEmplazamiento(string EmplazamientoTipoID, long clienteID)
        {
            long lEmplazamientoTipoID;
            List<InventarioCategorias> listaDatos;
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
                if (lEmplazamientoTipoID != 0)
                {
                    listaDatos = (from c in Context.InventarioCategorias
                                  join
vin in Context.CoreInventarioCategoriasAtributosCategorias on c.InventarioCategoriaID equals vin.InventarioCategoriaID
                                  join
atr in Context.CoreInventarioCategoriasAtributosCategoriasConfiguracionesAtributos on vin.CoreInventarioCategoriaAtributoCategoriaConfiguracionID equals atr.CoreInventarioCategoriaAtributoCategoriaConfiguracionID
                                  where (c.EmplazamientoTipoID == lEmplazamientoTipoID || !(c.EmplazamientoTipoID.HasValue)) && c.ClienteID == clienteID
                                  select c).Distinct().ToList();
                }
                else
                {
                    listaDatos = (from c in Context.InventarioCategorias
                                  join
vin in Context.CoreInventarioCategoriasAtributosCategorias on c.InventarioCategoriaID equals vin.InventarioCategoriaID
                                  join
atr in Context.CoreInventarioCategoriasAtributosCategoriasConfiguracionesAtributos on vin.CoreInventarioCategoriaAtributoCategoriaConfiguracionID equals atr.CoreInventarioCategoriaAtributoCategoriaConfiguracionID
                                  where c.EmplazamientoTipoID == null && c.ClienteID == clienteID
                                  select c).Distinct().ToList();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                listaDatos = null;
            }
            return listaDatos;
        }

        public List<InventarioCategorias> GetInventarioCategoriasByTipoEmplazamientoSoloActivos(string EmplazamientoTipoID, long clienteID, bool soloActivos)
        {
            long lEmplazamientoTipoID;
            List<InventarioCategorias> listaDatos;
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
                if (lEmplazamientoTipoID != 0)
                {
                    if (soloActivos)
                    {
                        listaDatos = (from c in Context.InventarioCategorias where (c.EmplazamientoTipoID == lEmplazamientoTipoID || !(c.EmplazamientoTipoID.HasValue)) && c.ClienteID == clienteID && c.Activo select c).OrderBy(x => x.InventarioCategoria).ToList();
                    }
                    else
                    {
                        listaDatos = (from c in Context.InventarioCategorias where (c.EmplazamientoTipoID == lEmplazamientoTipoID || !(c.EmplazamientoTipoID.HasValue)) && c.ClienteID == clienteID select c).OrderBy(x => x.InventarioCategoria).ToList();
                    }
                }
                else
                {
                    if (soloActivos)
                    {
                        listaDatos = (from c in Context.InventarioCategorias where c.EmplazamientoTipoID == null && c.ClienteID == clienteID && c.Activo select c).OrderBy(x => x.InventarioCategoria).ToList();
                    }
                    else
                    {
                        listaDatos = (from c in Context.InventarioCategorias where c.EmplazamientoTipoID == null && c.ClienteID == clienteID select c).OrderBy(x => x.InventarioCategoria).ToList();
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                listaDatos = null;
            }
            return listaDatos;
        }

        public List<InventarioCategorias> GetInventarioCategoriasByTipoEmplazamiento2(long? EmplazamientoTipoID, long clienteID)
        {
            List<InventarioCategorias> listaDatos;
            try
            {
                if (EmplazamientoTipoID != null && EmplazamientoTipoID != 0)
                {
                    listaDatos = (from c in Context.InventarioCategorias where (c.EmplazamientoTipoID == EmplazamientoTipoID || !(c.EmplazamientoTipoID.HasValue)) && c.ClienteID == clienteID && c.Activo
                                 select c).ToList();
                }
                else
                {
                    listaDatos = (from c in Context.InventarioCategorias where c.EmplazamientoTipoID == null && c.ClienteID == clienteID && c.Activo
                                   select c).ToList();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                listaDatos = null;
            }
            return listaDatos;
        }


        public List<InventarioCategorias> GetInventarioCategoriasByTipoEmplazamientoListaCategoria(long? EmplazamientoTipoID, long clienteID, List<InventarioCategorias> ListaInventarioCategorias)
        {
            List<InventarioCategorias> listaDatos;
            try
            {
                if (EmplazamientoTipoID != null && EmplazamientoTipoID != 0)
                {
                    listaDatos = (from c in Context.InventarioCategorias where (c.EmplazamientoTipoID == EmplazamientoTipoID || !(c.EmplazamientoTipoID.HasValue)) && c.ClienteID == clienteID && c.Activo select c).ToList();
                }
                else
                {
                    listaDatos = (from c in Context.InventarioCategorias where c.EmplazamientoTipoID == null && c.ClienteID == clienteID && c.Activo select c).ToList();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                listaDatos = null;
            }
            return listaDatos;
        }
        public List<InventarioCategorias> GetInventarioCategoriasActivas(long clienteID)
        {
            List<InventarioCategorias> listaDatos;
            try
            {
                listaDatos = (from c in Context.InventarioCategorias where c.ClienteID == clienteID && c.Activo select c).ToList();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                listaDatos = null;
            }
            return listaDatos;
        }
        public List<InventarioCategorias> GetInventarioCategoriasLista(List<long> listaCategorias)
        {
            List<InventarioCategorias> listaDatos;
            try
            {
                listaDatos = (from c in Context.InventarioCategorias where listaCategorias.Contains(c.InventarioCategoriaID) select c).ToList();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                listaDatos = null;
            }
            return listaDatos;
        }

        public List<InventarioCategorias> GetActivos(long clienteID)
        {
            List<InventarioCategorias> listaDatos;
            try
            {
                listaDatos = (from c in Context.InventarioCategorias where c.Activo && c.ClienteID == clienteID orderby c.InventarioCategoria select c).ToList();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                listaDatos = null;
            }
            return listaDatos;
        }

        public List<InventarioCategorias> GetInventarioCategoriasComunesYPorEmplazamiento(string EmplazamientoTipoID, long clienteID)
        {
            long lEmplazamientoTipoID;
            List<InventarioCategorias> listaDatos;
            try
            {
                lEmplazamientoTipoID = long.Parse(EmplazamientoTipoID);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                lEmplazamientoTipoID = 0;
            }
            try
            {

                listaDatos = (from c in Context.InventarioCategorias where (c.EmplazamientoTipoID == lEmplazamientoTipoID || c.EmplazamientoTipoID == null) && c.ClienteID == clienteID select c).ToList();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                listaDatos = null;
            }
            return listaDatos;
        }

        public List<InventarioCategorias> GetInventarioCategoriasNoAsociadasComunesYPorEmplazamiento(string EmplazamientoTipoID, long clienteID, long lCategoriaID)
        {
            long lEmplazamientoTipoID;
            List<long> listaIDSociados;
            List<InventarioCategorias> listaDatos;
            try
            {
                lEmplazamientoTipoID = long.Parse(EmplazamientoTipoID);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                lEmplazamientoTipoID = 0;
            }
            try
            {
                listaIDSociados = (from c in Context.InventarioCategoriasVinculaciones where (lCategoriaID != 0) ? c.InventarioCategoriaPadreID == lCategoriaID : !c.InventarioCategoriaPadreID.HasValue && (!c.EmplazamientoTipoID.HasValue || c.EmplazamientoTipoID == lEmplazamientoTipoID) select c.InventarioCategoriaID).ToList();
                listaDatos = (from c in Context.InventarioCategorias where (c.EmplazamientoTipoID == lEmplazamientoTipoID || c.EmplazamientoTipoID == null) && c.ClienteID == clienteID && !listaIDSociados.Contains(c.InventarioCategoriaID) select c).ToList();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                listaDatos = null;
            }
            return listaDatos;
        }

        public bool RegistroDuplicado(string nombre, string codigo, long? EmplazamientoTipo, long clienteID)
        {
            List<InventarioCategorias> listaDatos;
            try
            {
                listaDatos = (from c in Context.InventarioCategorias where (c.InventarioCategoria == nombre || c.Codigo == codigo) && c.EmplazamientoTipoID == EmplazamientoTipo && c.ClienteID == clienteID select c).ToList();

                if (listaDatos.Count > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return true;
            }
        }

        public bool NombreDuplicado(string nombre, long? EmplazamientoTipo, long clienteID)
        {
            List<InventarioCategorias> listaDatos;
            try
            {
                listaDatos = (from c in Context.InventarioCategorias where c.InventarioCategoria == nombre /*&& (c.EmplazamientoTipoID == EmplazamientoTipo || c.EmplazamientoTipoID == null)*/ && c.ClienteID == clienteID select c).ToList();

                if (listaDatos.Count > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return true;
            }
        }

        public bool CodigoDuplicadoGeneradorCodigos(string codigo, long? EmplazamientoTipo)
        {
            List<InventarioElementos> listaDatos;
            try
            {
                listaDatos = (from c in Context.InventarioElementos where c.NumeroInventario == codigo /*&& (c.EmplazamientoTipoID == EmplazamientoTipo || c.EmplazamientoTipoID == null) && c.ClienteID == clienteID*/ select c).ToList();

                if (listaDatos.Count > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return true;
            }
        }

        public bool CodigoDuplicado(string codigo, long? EmplazamientoTipo, long clienteID)
        {
            List<InventarioCategorias> listaDatos;
            try
            {
                listaDatos = (from c in Context.InventarioCategorias where c.Codigo == codigo /*&& (c.EmplazamientoTipoID == EmplazamientoTipo || c.EmplazamientoTipoID == null)*/ && c.ClienteID == clienteID select c).ToList();

                if (listaDatos.Count > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return true;
            }
        }

        public bool TieneRegistroAsociado(long categoriaID)
        {
            List<InventarioCategorias> listaDatosHijos;
            List<InventarioElementos> listaDatosUsos;
            try
            {
                listaDatosHijos = (from c in Context.InventarioCategorias where c.InventarioCategoriaPadreID == categoriaID select c).ToList();
                listaDatosUsos = (from c in Context.InventarioElementos where c.InventarioCategoriaID == categoriaID select c).ToList();
                if (listaDatosHijos.Count > 0 || listaDatosUsos.Count > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return true;
            }
        }

        public bool TieneRegistroAsociadoActivar(long categoriaID)
        {
            List<InventarioCategorias> listaDatosHijos;
            try
            {
                listaDatosHijos = (from c in Context.InventarioCategorias where c.InventarioCategoriaPadreID == categoriaID && c.Activo select c).ToList();
                if (listaDatosHijos.Count > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return true;
            }
        }

        public List<InventarioCategorias> GetCategoriasHijas(long? catPadreID, long? tipoEmplazamientoID)
        {
            List<InventarioCategorias> listaDatos;
            try
            {
                if (catPadreID != 0)
                {
                    listaDatos = (from c in Context.InventarioCategorias
                                  join vin in Context.InventarioCategoriasVinculaciones on c.InventarioCategoriaID equals vin.InventarioCategoriaID
                                  join atr in Context.InventarioAtributos on c.InventarioCategoriaID equals atr.InventarioCategoriaID
                                  where c.Activo && vin.InventarioCategoriaPadreID == catPadreID && (vin.EmplazamientoTipoID == tipoEmplazamientoID || !(vin.EmplazamientoTipoID.HasValue))
                                  select c).ToList();
                }
                else
                {
                    listaDatos = (from c in Context.InventarioCategorias
                                  join vin in Context.InventarioCategoriasVinculaciones on c.InventarioCategoriaID equals vin.InventarioCategoriaID
                                  join atr in Context.InventarioAtributos on c.InventarioCategoriaID equals atr.InventarioCategoriaID
                                  where c.Activo && vin.InventarioCategoriaPadreID == null && (vin.EmplazamientoTipoID == tipoEmplazamientoID || !(vin.EmplazamientoTipoID.HasValue))
                                  select c).ToList();
                }

            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                listaDatos = null;
            }
            return listaDatos;
        }

        public List<InventarioCategorias> GetAllCategoriasHijas(long? catPadreID, long? tipoEmplazamientoID, bool bActivo)
        {
            List<InventarioCategorias> listaDatos;
            List<long> listaIDs;
            try
            {
                if (catPadreID != null)
                {
                    listaIDs = (from c in Context.InventarioCategorias
                                join vin in Context.InventarioCategoriasVinculaciones on c.InventarioCategoriaID equals vin.InventarioCategoriaID
                                where
vin.InventarioCategoriaPadreID == catPadreID &&
(vin.EmplazamientoTipoID == tipoEmplazamientoID || !(vin.EmplazamientoTipoID.HasValue))
                                select c.InventarioCategoriaID).ToList();
                }
                else
                {
                    listaIDs = (from c in Context.InventarioCategorias
                                join vin in Context.InventarioCategoriasVinculaciones on c.InventarioCategoriaID equals vin.InventarioCategoriaID
                                where
!(vin.InventarioCategoriaPadreID.HasValue) &&
                                (tipoEmplazamientoID.HasValue) ? (vin.EmplazamientoTipoID == tipoEmplazamientoID) : !(vin.EmplazamientoTipoID.HasValue)
                                select c.InventarioCategoriaID).ToList();
                }
                if (bActivo)
                {
                    listaDatos = (from c in Context.InventarioCategorias where listaIDs.Contains(c.InventarioCategoriaID) && c.Activo select c).ToList(); ;
                }
                else
                {
                    listaDatos = (from c in Context.InventarioCategorias where listaIDs.Contains(c.InventarioCategoriaID) select c).ToList();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                listaDatos = null;
            }
            return listaDatos;
        }

        public List<InventarioCategorias> GetAllCategoriasNoVinculadas(long? catPadreID, long? tipoEmplazamientoID)
        {
            List<InventarioCategorias> listaDatos;
            List<long> listaIDs;
            try
            {
                if (catPadreID != null)
                {
                    listaIDs = (from c in Context.InventarioCategorias join vin in Context.InventarioCategoriasVinculaciones on c.InventarioCategoriaID equals vin.InventarioCategoriaID where vin.InventarioCategoriaPadreID == catPadreID && (tipoEmplazamientoID.HasValue) ? (vin.EmplazamientoTipoID == tipoEmplazamientoID || !(vin.EmplazamientoTipoID.HasValue)) : !(vin.EmplazamientoTipoID.HasValue) select c.InventarioCategoriaID).ToList();
                }
                else
                {
                    listaIDs = (from c in Context.InventarioCategorias join vin in Context.InventarioCategoriasVinculaciones on c.InventarioCategoriaID equals vin.InventarioCategoriaID where !(vin.InventarioCategoriaPadreID.HasValue) && (tipoEmplazamientoID.HasValue) ? (vin.EmplazamientoTipoID == tipoEmplazamientoID || !(vin.EmplazamientoTipoID.HasValue)) : !(vin.EmplazamientoTipoID.HasValue) select c.InventarioCategoriaID).ToList();
                }
                listaDatos = (from c in Context.InventarioCategorias where !(listaIDs.Contains(c.InventarioCategoriaID)) select c).ToList();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                listaDatos = null;
            }
            return listaDatos;
        }

        public List<long?> GetIDsCategoriasPadres(long lCategoriaID, long lEmplazamientoID)
        {
            List<long?> listaIDs;
            try
            {
                EmplazamientosTipos oEmpl = (from c in Context.EmplazamientosTipos where c.EmplazamientoTipoID == lEmplazamientoID select c).FirstOrDefault();
                listaIDs = (from c in Context.InventarioCategoriasVinculaciones where c.InventarioCategoriaID == lCategoriaID && (!(c.EmplazamientoTipoID.HasValue) || c.EmplazamientoTipoID == oEmpl.EmplazamientoTipoID) select c.InventarioCategoriaPadreID).ToList();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                listaIDs = new List<long?>();
            }
            return listaIDs;
        }

        public List<InventarioCategorias> GetCategoriasPadres(long lCategoriaID, long lEmplazamientoID)
        {
            List<long?> listaIDs;
            List<InventarioCategorias> listaDatos;
            try
            {
                EmplazamientosTipos oEmpl = (from c in Context.EmplazamientosTipos where c.EmplazamientoTipoID == lEmplazamientoID select c).FirstOrDefault();
                if (oEmpl == null)
                {
                    listaIDs = (from c in Context.InventarioCategoriasVinculaciones where c.InventarioCategoriaID == lCategoriaID && (!(c.EmplazamientoTipoID.HasValue)) select c.InventarioCategoriaPadreID).ToList();
                }
                else
                {
                    listaIDs = (from c in Context.InventarioCategoriasVinculaciones where c.InventarioCategoriaID == lCategoriaID && (!(c.EmplazamientoTipoID.HasValue) || c.EmplazamientoTipoID == oEmpl.EmplazamientoTipoID) select c.InventarioCategoriaPadreID).ToList();
                }
                listaDatos = (from c in Context.InventarioCategorias where listaIDs.Contains(c.InventarioCategoriaID) select c).ToList();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                listaDatos = new List<InventarioCategorias>();
            }
            return listaDatos;
        }

        public bool IsCategoriasByEmplazamientoTipo(long tipoID)
        {
            // Local variables
            List<InventarioCategorias> lista = null;
            bool bRes = false;
            try
            {
                lista = (from c in Context.InventarioCategorias where (c.EmplazamientoTipoID == tipoID) select c).ToList();
                if (lista != null && lista.Count > 0)
                {
                    bRes = true;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }
            return bRes;
        }

        public void AddExcelTab(DocumentFormat.OpenXml.Spreadsheet.Workbook workbook, string tabName, uint sheetId, List<InventarioAtributos> listAtributos, string codEmp, string nomElem, string codElem, string codElemPadre, string estadoInventado, string entidad, string cuadroTexto_Texto)
        {
            var sheetPart = workbook.WorkbookPart.AddNewPart<DocumentFormat.OpenXml.Packaging.WorksheetPart>();
            var sheetData = new DocumentFormat.OpenXml.Spreadsheet.SheetData();
            sheetPart.Worksheet = new DocumentFormat.OpenXml.Spreadsheet.Worksheet(sheetData);

            DocumentFormat.OpenXml.Spreadsheet.Sheets sheets = workbook.WorkbookPart.Workbook.GetFirstChild<DocumentFormat.OpenXml.Spreadsheet.Sheets>();
            string relationshipId = workbook.WorkbookPart.GetIdOfPart(sheetPart);

            DocumentFormat.OpenXml.Spreadsheet.Sheet sheet = new DocumentFormat.OpenXml.Spreadsheet.Sheet()
            {
                Id = workbook.WorkbookPart.GetIdOfPart(sheetPart),
                SheetId = sheetId,
                Name = tabName
            };

            sheets.Append(sheet);

            // column headings          
            DocumentFormat.OpenXml.Spreadsheet.Row FilaCabecera = new DocumentFormat.OpenXml.Spreadsheet.Row();
            DocumentFormat.OpenXml.Spreadsheet.Row FilaCabeceraComponente = new DocumentFormat.OpenXml.Spreadsheet.Row();
            DocumentFormat.OpenXml.Spreadsheet.Row FilaCabeceraValoresPosibles = new DocumentFormat.OpenXml.Spreadsheet.Row();

            //De forma estática añadimos las columnas de CodigoPadre-Codigo-NombreCategoria-CodigoCategoria
            DocumentFormat.OpenXml.Spreadsheet.Cell cellCodigoSitio = new DocumentFormat.OpenXml.Spreadsheet.Cell();
            cellCodigoSitio.DataType = DocumentFormat.OpenXml.Spreadsheet.CellValues.String;
            cellCodigoSitio.CellValue = new DocumentFormat.OpenXml.Spreadsheet.CellValue(codEmp);
            FilaCabecera.AppendChild(cellCodigoSitio);

            DocumentFormat.OpenXml.Spreadsheet.Cell cellNombreCategoria = new DocumentFormat.OpenXml.Spreadsheet.Cell();
            cellNombreCategoria.DataType = DocumentFormat.OpenXml.Spreadsheet.CellValues.String;
            cellNombreCategoria.CellValue = new DocumentFormat.OpenXml.Spreadsheet.CellValue(nomElem);
            FilaCabecera.AppendChild(cellNombreCategoria);

            DocumentFormat.OpenXml.Spreadsheet.Cell cellCodigoCategoria = new DocumentFormat.OpenXml.Spreadsheet.Cell();
            cellCodigoCategoria.DataType = DocumentFormat.OpenXml.Spreadsheet.CellValues.String;
            cellCodigoCategoria.CellValue = new DocumentFormat.OpenXml.Spreadsheet.CellValue(codElem);
            FilaCabecera.AppendChild(cellCodigoCategoria);

            DocumentFormat.OpenXml.Spreadsheet.Cell cellCodigoSitioPadre = new DocumentFormat.OpenXml.Spreadsheet.Cell();
            cellCodigoSitioPadre.DataType = DocumentFormat.OpenXml.Spreadsheet.CellValues.String;
            cellCodigoSitioPadre.CellValue = new DocumentFormat.OpenXml.Spreadsheet.CellValue(codElemPadre);
            //FilaCabecera.AppendChild(cellCodigoSitioPadre);

            DocumentFormat.OpenXml.Spreadsheet.Cell cellEstadoInventario = new DocumentFormat.OpenXml.Spreadsheet.Cell();
            cellEstadoInventario.DataType = DocumentFormat.OpenXml.Spreadsheet.CellValues.String;
            cellEstadoInventario.CellValue = new DocumentFormat.OpenXml.Spreadsheet.CellValue(estadoInventado);
            FilaCabecera.AppendChild(cellEstadoInventario);

            DocumentFormat.OpenXml.Spreadsheet.Cell cellEntidad = new DocumentFormat.OpenXml.Spreadsheet.Cell();
            cellEntidad.DataType = DocumentFormat.OpenXml.Spreadsheet.CellValues.String;
            cellEntidad.CellValue = new DocumentFormat.OpenXml.Spreadsheet.CellValue(entidad);
            FilaCabecera.AppendChild(cellEntidad);

            #region Insertadas 4 celdas vacías para el componente

            DocumentFormat.OpenXml.Spreadsheet.Cell cell = new DocumentFormat.OpenXml.Spreadsheet.Cell();
            cell.DataType = DocumentFormat.OpenXml.Spreadsheet.CellValues.String;
            cell.CellValue = new DocumentFormat.OpenXml.Spreadsheet.CellValue(cuadroTexto_Texto);
            FilaCabeceraComponente.AppendChild(cell);

            cell = new DocumentFormat.OpenXml.Spreadsheet.Cell();
            cell.DataType = DocumentFormat.OpenXml.Spreadsheet.CellValues.String;
            cell.CellValue = new DocumentFormat.OpenXml.Spreadsheet.CellValue(cuadroTexto_Texto);
            FilaCabeceraComponente.AppendChild(cell);

            cell = new DocumentFormat.OpenXml.Spreadsheet.Cell();
            cell.DataType = DocumentFormat.OpenXml.Spreadsheet.CellValues.String;
            cell.CellValue = new DocumentFormat.OpenXml.Spreadsheet.CellValue(cuadroTexto_Texto);
            FilaCabeceraComponente.AppendChild(cell);

            cell = new DocumentFormat.OpenXml.Spreadsheet.Cell();
            cell.DataType = DocumentFormat.OpenXml.Spreadsheet.CellValues.String;
            cell.CellValue = new DocumentFormat.OpenXml.Spreadsheet.CellValue(cuadroTexto_Texto);
            FilaCabeceraComponente.AppendChild(cell);

            cell = new DocumentFormat.OpenXml.Spreadsheet.Cell();
            cell.DataType = DocumentFormat.OpenXml.Spreadsheet.CellValues.String;
            cell.CellValue = new DocumentFormat.OpenXml.Spreadsheet.CellValue(cuadroTexto_Texto);
            FilaCabeceraComponente.AppendChild(cell);

            cell = new DocumentFormat.OpenXml.Spreadsheet.Cell();
            cell.DataType = DocumentFormat.OpenXml.Spreadsheet.CellValues.String;
            cell.CellValue = new DocumentFormat.OpenXml.Spreadsheet.CellValue(cuadroTexto_Texto);
            FilaCabeceraComponente.AppendChild(cell);

            #endregion

            #region Insertamos 4 celdas vacías para los valores posibles

            cell = new DocumentFormat.OpenXml.Spreadsheet.Cell();
            cell.DataType = DocumentFormat.OpenXml.Spreadsheet.CellValues.String;
            cell.CellValue = new DocumentFormat.OpenXml.Spreadsheet.CellValue("");
            FilaCabeceraValoresPosibles.AppendChild(cell);

            cell = new DocumentFormat.OpenXml.Spreadsheet.Cell();
            cell.DataType = DocumentFormat.OpenXml.Spreadsheet.CellValues.String;
            cell.CellValue = new DocumentFormat.OpenXml.Spreadsheet.CellValue("");
            FilaCabeceraValoresPosibles.AppendChild(cell);

            cell = new DocumentFormat.OpenXml.Spreadsheet.Cell();
            cell.DataType = DocumentFormat.OpenXml.Spreadsheet.CellValues.String;
            cell.CellValue = new DocumentFormat.OpenXml.Spreadsheet.CellValue("");
            FilaCabeceraValoresPosibles.AppendChild(cell);

            cell = new DocumentFormat.OpenXml.Spreadsheet.Cell();
            cell.DataType = DocumentFormat.OpenXml.Spreadsheet.CellValues.String;
            cell.CellValue = new DocumentFormat.OpenXml.Spreadsheet.CellValue("");
            FilaCabeceraValoresPosibles.AppendChild(cell);

            cell = new DocumentFormat.OpenXml.Spreadsheet.Cell();
            cell.DataType = DocumentFormat.OpenXml.Spreadsheet.CellValues.String;
            cell.CellValue = new DocumentFormat.OpenXml.Spreadsheet.CellValue("");
            FilaCabeceraValoresPosibles.AppendChild(cell);

            cell = new DocumentFormat.OpenXml.Spreadsheet.Cell();
            cell.DataType = DocumentFormat.OpenXml.Spreadsheet.CellValues.String;
            cell.CellValue = new DocumentFormat.OpenXml.Spreadsheet.CellValue("");
            FilaCabeceraValoresPosibles.AppendChild(cell);

            #endregion

            foreach (InventarioAtributos atributo in listAtributos)
            {
                cell = new DocumentFormat.OpenXml.Spreadsheet.Cell();
                cell.DataType = DocumentFormat.OpenXml.Spreadsheet.CellValues.String;

                if (atributo.Obligatorio == true)
                {
                    cell.CellValue = new DocumentFormat.OpenXml.Spreadsheet.CellValue(atributo.NombreAtributo);
                }
                else
                {
                    cell.CellValue = new DocumentFormat.OpenXml.Spreadsheet.CellValue(atributo.NombreAtributo);
                }

                FilaCabecera.AppendChild(cell);

                DocumentFormat.OpenXml.Spreadsheet.Cell cellComponente = new DocumentFormat.OpenXml.Spreadsheet.Cell();
                cellComponente.DataType = DocumentFormat.OpenXml.Spreadsheet.CellValues.String;
                cellComponente.CellValue = new DocumentFormat.OpenXml.Spreadsheet.CellValue(atributo.Componentes.Componente + " - " + atributo.TiposDatos.TipoDato);
                FilaCabeceraComponente.AppendChild(cellComponente);

                DocumentFormat.OpenXml.Spreadsheet.Cell cellValores = new DocumentFormat.OpenXml.Spreadsheet.Cell();

                if (atributo.TiposDatos.Codigo.ToUpper() == Comun.TiposDatos.Lista.ToUpper() || atributo.TiposDatos.Codigo.ToUpper() == Comun.TiposDatos.ListaMultiple.ToUpper())
                {
                    if (atributo.NombreTabla != null && atributo.TablaIndice != null && atributo.NombreTabla != "" && atributo.TablaIndice != "")
                    {
                        InventarioAtributosController cInventarioComponentes = new InventarioAtributosController();
                        string valoresCombo = cInventarioComponentes.GetValoresStringComboBoxByColumnaModeloDatosID((long)atributo.ColumnaModeloDatoID, atributo.TablaValor);

                        cellValores.DataType = DocumentFormat.OpenXml.Spreadsheet.CellValues.String;
                        cellValores.CellValue = new DocumentFormat.OpenXml.Spreadsheet.CellValue(valoresCombo);
                        FilaCabeceraValoresPosibles.AppendChild(cellValores);
                    }
                    else
                    {
                        cellValores.DataType = DocumentFormat.OpenXml.Spreadsheet.CellValues.String;
                        cellValores.CellValue = new DocumentFormat.OpenXml.Spreadsheet.CellValue(atributo.ValoresPosibles);
                        FilaCabeceraValoresPosibles.AppendChild(cellValores);
                    }
                }
                else if (atributo.TiposDatos.Codigo.ToUpper() == Comun.TiposDatos.Booleano.ToUpper())
                {
                    cellValores.DataType = DocumentFormat.OpenXml.Spreadsheet.CellValues.String;
                    cellValores.CellValue = new DocumentFormat.OpenXml.Spreadsheet.CellValue("True;False");
                    FilaCabeceraValoresPosibles.AppendChild(cellValores);
                }
                else if (atributo.TiposDatos.Codigo.ToUpper() == Comun.TiposDatos.Fecha.ToUpper())
                {
                    cellValores.DataType = DocumentFormat.OpenXml.Spreadsheet.CellValues.String;
                    cellValores.CellValue = new DocumentFormat.OpenXml.Spreadsheet.CellValue("yyyymmdd");
                    FilaCabeceraValoresPosibles.AppendChild(cellValores);
                }
                else
                {
                    cellValores.DataType = DocumentFormat.OpenXml.Spreadsheet.CellValues.String;
                    cellValores.CellValue = new DocumentFormat.OpenXml.Spreadsheet.CellValue("");
                    FilaCabeceraValoresPosibles.AppendChild(cellValores);
                }
            }

            //sheetData.AppendChild(FilaCabeceraComponente);
            //sheetData.AppendChild(FilaCabeceraValoresPosibles);
            sheetData.AppendChild(FilaCabecera);
        }

        public void ExportarModeloDatos(List<InventarioCategorias> lCategorias, string archivo, string codEmp, string nomElem, string codElem, string codElemPadre, string estadoInventario, string entidad, string cuadroTexto_Texto)
        {
            InventarioAtributosController cAtributos = new InventarioAtributosController();

            using (var workbook = DocumentFormat.OpenXml.Packaging.SpreadsheetDocument.Create(archivo, DocumentFormat.OpenXml.SpreadsheetDocumentType.Workbook))
            {
                var workbookPart = workbook.AddWorkbookPart();
                workbook.WorkbookPart.Workbook = new DocumentFormat.OpenXml.Spreadsheet.Workbook();
                workbook.WorkbookPart.Workbook.Sheets = new DocumentFormat.OpenXml.Spreadsheet.Sheets();

                uint sheetId = 0;

                foreach (InventarioCategorias dato in lCategorias)
                {
                    List<InventarioAtributos> listaAtributos = new List<InventarioAtributos>();
                    listaAtributos = cAtributos.GetAtributosFromCategoriaActivos(dato.InventarioCategoriaID);

                    AddExcelTab(workbook.WorkbookPart.Workbook, dato.InventarioCategoria, ++sheetId, listaAtributos, codEmp, nomElem, codElem, codElemPadre, estadoInventario, entidad, cuadroTexto_Texto);
                }
            }
        }

        public long? GetInventarioCategoriaIDByNombre(String nombre)
        {
            long? lTipoID = 0;
            List<long> lTipos = null;

            lTipos = (from c in Context.InventarioCategorias where c.InventarioCategoria == nombre select c.InventarioCategoriaID).ToList<long>();

            if (lTipos != null && lTipos.Count > 0)
            {
                lTipoID = lTipos.ElementAt(0);
            }

            return lTipoID;

        }

        public long GetInventarioCategoriaIDByNombreAPI(String nombre)
        {
            long lTipoID = 0;
            List<long> lTipos = null;

            lTipos = (from c in Context.InventarioCategorias where c.InventarioCategoria == nombre select c.InventarioCategoriaID).ToList<long>();

            if (lTipos != null && lTipos.Count > 0)
            {
                lTipoID = lTipos.ElementAt(0);
            }

            return lTipoID;

        }

        public InventarioCategorias GetInventarioCategoriaIDByNombre(string nombre, long clienteID)
        {
            InventarioCategorias cat;

            try
            {
                cat = (from c in Context.InventarioCategorias
                       where c.InventarioCategoria == nombre &&
                            c.ClienteID == clienteID
                       select c).First();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                cat = null;
            }
            return cat;
        }

        #region EXPORTAR ELEMENTOS EMPLAZAMIENTO

        public void ExportarCategoriasFromEmplazamiento(long lemplazamientoID, string archivo, TreeCore.Page.BasePageExtNet paginaRef)
        {
            InventarioAtributosController cAtributos = new InventarioAtributosController();
            List<Vw_InventarioElementosReducida> listaElementos;
            List<InventarioCategorias> listaCategorias;
            List<long> listaIDsCategorias;
            if (lemplazamientoID != 0)
            {
                listaElementos = (from c in Context.Vw_InventarioElementosReducida where c.EmplazamientoID == lemplazamientoID select c).ToList();
                listaIDsCategorias = (from c in Context.InventarioElementos where c.EmplazamientoID == lemplazamientoID select c.InventarioCategoriaID).ToList();
            }
            else
            {
                listaElementos = (from c in Context.Vw_InventarioElementosReducida select c).ToList();
                listaIDsCategorias = (from c in Context.InventarioElementos select c.InventarioCategoriaID).ToList();
            }
            listaCategorias = (from c in Context.InventarioCategorias where listaIDsCategorias.Contains(c.InventarioCategoriaID) select c).ToList();
            using (var workbook = DocumentFormat.OpenXml.Packaging.SpreadsheetDocument.Create(archivo, DocumentFormat.OpenXml.SpreadsheetDocumentType.Workbook))
            {
                var workbookPart = workbook.AddWorkbookPart();
                workbook.WorkbookPart.Workbook = new DocumentFormat.OpenXml.Spreadsheet.Workbook();
                workbook.WorkbookPart.Workbook.Sheets = new DocumentFormat.OpenXml.Spreadsheet.Sheets();

                uint sheetId = 0;
                if (listaElementos.Count > 0 && listaCategorias.Count > 0)
                {

                    foreach (InventarioCategorias dato in listaCategorias)
                    {
                        List<InventarioAtributos> listaAtributos = new List<InventarioAtributos>();
                        listaAtributos = cAtributos.GetAtributosFromCategoriaActivos(dato.InventarioCategoriaID);

                        if (listaAtributos.Count > 0)
                        {
                            AddExcelTabExportarCategoriasFromEmplazamiento(workbook.WorkbookPart.Workbook, ++sheetId, dato, listaAtributos, listaElementos.FindAll(ele => ele.InventarioCategoriaID == dato.InventarioCategoriaID).ToList(), paginaRef);
                        }
                    }
                }
            }
        }

        protected void AddExcelTabExportarCategoriasFromEmplazamiento(DocumentFormat.OpenXml.Spreadsheet.Workbook workbook, uint sheetId, InventarioCategorias oCategorias, List<InventarioAtributos> listaAtributos, List<Vw_InventarioElementosReducida> listaElementos, TreeCore.Page.BasePageExtNet paginaRef)
        {
            var sheetPart = workbook.WorkbookPart.AddNewPart<DocumentFormat.OpenXml.Packaging.WorksheetPart>();
            List<InventarioElementosAtributos> listaAtributosElementos;
            InventarioElementosAtributos oAtributosElementos;
            var sheetData = new DocumentFormat.OpenXml.Spreadsheet.SheetData();
            sheetPart.Worksheet = new DocumentFormat.OpenXml.Spreadsheet.Worksheet(sheetData);
            DocumentFormat.OpenXml.Spreadsheet.Row NuevaFila;
            DocumentFormat.OpenXml.Spreadsheet.Cell NuevaCelda;

            DocumentFormat.OpenXml.Spreadsheet.Sheets sheets = workbook.WorkbookPart.Workbook.GetFirstChild<DocumentFormat.OpenXml.Spreadsheet.Sheets>();
            string relationshipId = workbook.WorkbookPart.GetIdOfPart(sheetPart);

            DocumentFormat.OpenXml.Spreadsheet.Sheet sheet = new DocumentFormat.OpenXml.Spreadsheet.Sheet()
            {
                Id = workbook.WorkbookPart.GetIdOfPart(sheetPart),
                SheetId = sheetId,
                Name = oCategorias.InventarioCategoria
            };
            sheets.Append(sheet);

            #region AÑADIR CABECERAS

            //Añadimos las cabeceras a el Tab
            /*Orden => Codigo - Nombre - Codigo Padre - Estado - Valores Dinamicos*/
            NuevaFila = new DocumentFormat.OpenXml.Spreadsheet.Row();

            //Codigo
            NuevaCelda = new DocumentFormat.OpenXml.Spreadsheet.Cell();
            NuevaCelda.DataType = DocumentFormat.OpenXml.Spreadsheet.CellValues.String;
            NuevaCelda.CellValue = new DocumentFormat.OpenXml.Spreadsheet.CellValue(paginaRef.GetGlobalResource("strCodigo"));
            NuevaFila.AppendChild(NuevaCelda);

            //Nombre
            NuevaCelda = new DocumentFormat.OpenXml.Spreadsheet.Cell();
            NuevaCelda.DataType = DocumentFormat.OpenXml.Spreadsheet.CellValues.String;
            NuevaCelda.CellValue = new DocumentFormat.OpenXml.Spreadsheet.CellValue(paginaRef.GetGlobalResource("strNombre"));
            NuevaFila.AppendChild(NuevaCelda);

            //Codigo Padre
            NuevaCelda = new DocumentFormat.OpenXml.Spreadsheet.Cell();
            NuevaCelda.DataType = DocumentFormat.OpenXml.Spreadsheet.CellValues.String;
            NuevaCelda.CellValue = new DocumentFormat.OpenXml.Spreadsheet.CellValue(paginaRef.GetGlobalResource("strCodigoPadre"));
            NuevaFila.AppendChild(NuevaCelda);

            //Estados
            NuevaCelda = new DocumentFormat.OpenXml.Spreadsheet.Cell();
            NuevaCelda.DataType = DocumentFormat.OpenXml.Spreadsheet.CellValues.String;
            NuevaCelda.CellValue = new DocumentFormat.OpenXml.Spreadsheet.CellValue(paginaRef.GetGlobalResource("strEstado"));
            NuevaFila.AppendChild(NuevaCelda);

            #region Valores Dinamicos Cabecera
            foreach (var atr in listaAtributos)
            {
                NuevaCelda = new DocumentFormat.OpenXml.Spreadsheet.Cell();
                NuevaCelda.DataType = DocumentFormat.OpenXml.Spreadsheet.CellValues.String;
                NuevaCelda.CellValue = new DocumentFormat.OpenXml.Spreadsheet.CellValue(atr.NombreAtributo);
                NuevaFila.AppendChild(NuevaCelda);
            }
            #endregion

            sheetData.AppendChild(NuevaFila);

            #endregion

            foreach (var oElemento in listaElementos)
            {
                #region AÑADIR NUEVA FILA

                //Añadimos las cabeceras a el Tab
                /*Orden => Codigo - Nombre - Codigo Padre - Estado - Valores Dinamicos*/
                NuevaFila = new DocumentFormat.OpenXml.Spreadsheet.Row();

                //Codigo
                NuevaCelda = new DocumentFormat.OpenXml.Spreadsheet.Cell();
                NuevaCelda.DataType = DocumentFormat.OpenXml.Spreadsheet.CellValues.String;
                NuevaCelda.CellValue = new DocumentFormat.OpenXml.Spreadsheet.CellValue(oElemento.NumeroInventario);
                NuevaFila.AppendChild(NuevaCelda);

                //Nombre
                NuevaCelda = new DocumentFormat.OpenXml.Spreadsheet.Cell();
                NuevaCelda.DataType = DocumentFormat.OpenXml.Spreadsheet.CellValues.String;
                NuevaCelda.CellValue = new DocumentFormat.OpenXml.Spreadsheet.CellValue(oElemento.Nombre);
                NuevaFila.AppendChild(NuevaCelda);

                //Codigo Padre
                NuevaCelda = new DocumentFormat.OpenXml.Spreadsheet.Cell();
                NuevaCelda.DataType = DocumentFormat.OpenXml.Spreadsheet.CellValues.String;
                NuevaCelda.CellValue = new DocumentFormat.OpenXml.Spreadsheet.CellValue(oElemento.NumeroInventarioPadre);
                NuevaFila.AppendChild(NuevaCelda);

                //Estados
                NuevaCelda = new DocumentFormat.OpenXml.Spreadsheet.Cell();
                NuevaCelda.DataType = DocumentFormat.OpenXml.Spreadsheet.CellValues.String;
                NuevaCelda.CellValue = new DocumentFormat.OpenXml.Spreadsheet.CellValue(oElemento.EstadoInventarioElemento);
                NuevaFila.AppendChild(NuevaCelda);

                #region Valores Dinamicos Cabecera

                listaAtributosElementos = (from c in Context.InventarioElementosAtributos where c.InventarioElementoID == oElemento.InventarioElementoID select c).ToList();

                foreach (var atr in listaAtributos)
                {
                    try
                    {
                        oAtributosElementos = listaAtributosElementos.FindAll(atrEl => atrEl.InventarioAtributoID == atr.InventarioAtributoID).First();
                        if (oAtributosElementos != null)
                        {
                            NuevaCelda = new DocumentFormat.OpenXml.Spreadsheet.Cell();
                            NuevaCelda.DataType = DocumentFormat.OpenXml.Spreadsheet.CellValues.String;
                            NuevaCelda.CellValue = new DocumentFormat.OpenXml.Spreadsheet.CellValue(oAtributosElementos.Valor);
                            NuevaFila.AppendChild(NuevaCelda);
                        }
                    }
                    catch (Exception ex)
                    {
                        NuevaCelda = new DocumentFormat.OpenXml.Spreadsheet.Cell();
                        NuevaCelda.DataType = DocumentFormat.OpenXml.Spreadsheet.CellValues.String;
                        NuevaCelda.CellValue = new DocumentFormat.OpenXml.Spreadsheet.CellValue("");
                        NuevaFila.AppendChild(NuevaCelda);
                    }
                }

                #endregion

                sheetData.AppendChild(NuevaFila);

                #endregion
            }


        }

        #endregion

        #region EXPORTAR ELEMENTOS CLIENTE

        public void ExportarCategoriasFromCliente(long lClienteID, string archivo, TreeCore.Page.BasePageExtNet paginaRef)
        {
            InventarioAtributosController cAtributos = new InventarioAtributosController();
            List<Vw_InventarioElementosReducida> listaElementos;
            List<InventarioCategorias> listaCategorias;
            List<long> listaIDsCategorias;
            listaCategorias = (from c in Context.InventarioCategorias where c.ClienteID == lClienteID select c).ToList();

            using (var workbook = DocumentFormat.OpenXml.Packaging.SpreadsheetDocument.Create(archivo, DocumentFormat.OpenXml.SpreadsheetDocumentType.Workbook))
            {
                var workbookPart = workbook.AddWorkbookPart();
                workbook.WorkbookPart.Workbook = new DocumentFormat.OpenXml.Spreadsheet.Workbook();
                workbook.WorkbookPart.Workbook.Sheets = new DocumentFormat.OpenXml.Spreadsheet.Sheets();

                uint sheetId = 0;
                if (listaCategorias.Count > 0)
                {
                    foreach (InventarioCategorias dato in listaCategorias)
                    {
                        List<InventarioAtributos> listaAtributos = new List<InventarioAtributos>();
                        listaAtributos = cAtributos.GetAtributosFromCategoriaActivos(dato.InventarioCategoriaID);

                        if (listaAtributos.Count > 0)
                        {
                            AddExcelTabExportarCategoriasFromCliente(workbook.WorkbookPart.Workbook, ++sheetId, dato, listaAtributos, paginaRef);
                        }
                    }
                }
            }
        }

        protected void AddExcelTabExportarCategoriasFromCliente(DocumentFormat.OpenXml.Spreadsheet.Workbook workbook, uint sheetId, InventarioCategorias oCategorias, List<InventarioAtributos> listaAtributos, TreeCore.Page.BasePageExtNet paginaRef)
        {
            var sheetPart = workbook.WorkbookPart.AddNewPart<DocumentFormat.OpenXml.Packaging.WorksheetPart>();
            List<InventarioElementosAtributos> listaAtributosElementos;
            InventarioElementosAtributos oAtributosElementos;
            List<Vw_InventarioElementosReducida> listaElementos;
            var sheetData = new DocumentFormat.OpenXml.Spreadsheet.SheetData();
            sheetPart.Worksheet = new DocumentFormat.OpenXml.Spreadsheet.Worksheet(sheetData);
            DocumentFormat.OpenXml.Spreadsheet.Row NuevaFila;
            DocumentFormat.OpenXml.Spreadsheet.Cell NuevaCelda;

            DocumentFormat.OpenXml.Spreadsheet.Sheets sheets = workbook.WorkbookPart.Workbook.GetFirstChild<DocumentFormat.OpenXml.Spreadsheet.Sheets>();
            string relationshipId = workbook.WorkbookPart.GetIdOfPart(sheetPart);

            listaElementos = (from c in Context.Vw_InventarioElementosReducida where c.InventarioCategoriaID == oCategorias.InventarioCategoriaID select c).ToList();

            DocumentFormat.OpenXml.Spreadsheet.Sheet sheet = new DocumentFormat.OpenXml.Spreadsheet.Sheet()
            {
                Id = workbook.WorkbookPart.GetIdOfPart(sheetPart),
                SheetId = sheetId,
                Name = oCategorias.InventarioCategoria
            };
            sheets.Append(sheet);

            #region AÑADIR CABECERAS

            //Añadimos las cabeceras a el Tab
            /*Orden => Codigo - Nombre - Codigo Padre - Estado - Valores Dinamicos*/
            NuevaFila = new DocumentFormat.OpenXml.Spreadsheet.Row();

            //Codigo
            NuevaCelda = new DocumentFormat.OpenXml.Spreadsheet.Cell();
            NuevaCelda.DataType = DocumentFormat.OpenXml.Spreadsheet.CellValues.String;
            NuevaCelda.CellValue = new DocumentFormat.OpenXml.Spreadsheet.CellValue(paginaRef.GetGlobalResource("strCodigo"));
            NuevaFila.AppendChild(NuevaCelda);

            //Nombre
            NuevaCelda = new DocumentFormat.OpenXml.Spreadsheet.Cell();
            NuevaCelda.DataType = DocumentFormat.OpenXml.Spreadsheet.CellValues.String;
            NuevaCelda.CellValue = new DocumentFormat.OpenXml.Spreadsheet.CellValue(paginaRef.GetGlobalResource("strNombre"));
            NuevaFila.AppendChild(NuevaCelda);

            //Codigo Padre
            NuevaCelda = new DocumentFormat.OpenXml.Spreadsheet.Cell();
            NuevaCelda.DataType = DocumentFormat.OpenXml.Spreadsheet.CellValues.String;
            NuevaCelda.CellValue = new DocumentFormat.OpenXml.Spreadsheet.CellValue(paginaRef.GetGlobalResource("strCodigoPadre"));
            NuevaFila.AppendChild(NuevaCelda);

            //Estados
            NuevaCelda = new DocumentFormat.OpenXml.Spreadsheet.Cell();
            NuevaCelda.DataType = DocumentFormat.OpenXml.Spreadsheet.CellValues.String;
            NuevaCelda.CellValue = new DocumentFormat.OpenXml.Spreadsheet.CellValue(paginaRef.GetGlobalResource("strEstado"));
            NuevaFila.AppendChild(NuevaCelda);

            #region Valores Dinamicos Cabecera
            foreach (var atr in listaAtributos)
            {
                NuevaCelda = new DocumentFormat.OpenXml.Spreadsheet.Cell();
                NuevaCelda.DataType = DocumentFormat.OpenXml.Spreadsheet.CellValues.String;
                NuevaCelda.CellValue = new DocumentFormat.OpenXml.Spreadsheet.CellValue(atr.NombreAtributo);
                NuevaFila.AppendChild(NuevaCelda);
            }
            #endregion

            sheetData.AppendChild(NuevaFila);

            #endregion

            foreach (var oElemento in listaElementos)
            {
                #region AÑADIR NUEVA FILA

                //Añadimos las cabeceras a el Tab
                /*Orden => Codigo - Nombre - Codigo Padre - Estado - Valores Dinamicos*/
                NuevaFila = new DocumentFormat.OpenXml.Spreadsheet.Row();

                //Codigo
                NuevaCelda = new DocumentFormat.OpenXml.Spreadsheet.Cell();
                NuevaCelda.DataType = DocumentFormat.OpenXml.Spreadsheet.CellValues.String;
                NuevaCelda.CellValue = new DocumentFormat.OpenXml.Spreadsheet.CellValue(oElemento.NumeroInventario);
                NuevaFila.AppendChild(NuevaCelda);

                //Nombre
                NuevaCelda = new DocumentFormat.OpenXml.Spreadsheet.Cell();
                NuevaCelda.DataType = DocumentFormat.OpenXml.Spreadsheet.CellValues.String;
                NuevaCelda.CellValue = new DocumentFormat.OpenXml.Spreadsheet.CellValue(oElemento.Nombre);
                NuevaFila.AppendChild(NuevaCelda);

                //Codigo Padre
                NuevaCelda = new DocumentFormat.OpenXml.Spreadsheet.Cell();
                NuevaCelda.DataType = DocumentFormat.OpenXml.Spreadsheet.CellValues.String;
                NuevaCelda.CellValue = new DocumentFormat.OpenXml.Spreadsheet.CellValue(oElemento.NumeroInventarioPadre);
                NuevaFila.AppendChild(NuevaCelda);

                //Estados
                NuevaCelda = new DocumentFormat.OpenXml.Spreadsheet.Cell();
                NuevaCelda.DataType = DocumentFormat.OpenXml.Spreadsheet.CellValues.String;
                NuevaCelda.CellValue = new DocumentFormat.OpenXml.Spreadsheet.CellValue(oElemento.EstadoInventarioElemento);
                NuevaFila.AppendChild(NuevaCelda);

                #region Valores Dinamicos Cabecera

                listaAtributosElementos = (from c in Context.InventarioElementosAtributos where c.InventarioElementoID == oElemento.InventarioElementoID select c).ToList();

                foreach (var atr in listaAtributos)
                {
                    try
                    {
                        oAtributosElementos = listaAtributosElementos.FindAll(atrEl => atrEl.InventarioAtributoID == atr.InventarioAtributoID).First();
                        if (oAtributosElementos != null)
                        {
                            NuevaCelda = new DocumentFormat.OpenXml.Spreadsheet.Cell();
                            NuevaCelda.DataType = DocumentFormat.OpenXml.Spreadsheet.CellValues.String;
                            NuevaCelda.CellValue = new DocumentFormat.OpenXml.Spreadsheet.CellValue(oAtributosElementos.Valor);
                            NuevaFila.AppendChild(NuevaCelda);
                        }
                    }
                    catch (Exception ex)
                    {
                        NuevaCelda = new DocumentFormat.OpenXml.Spreadsheet.Cell();
                        NuevaCelda.DataType = DocumentFormat.OpenXml.Spreadsheet.CellValues.String;
                        NuevaCelda.CellValue = new DocumentFormat.OpenXml.Spreadsheet.CellValue("");
                        NuevaFila.AppendChild(NuevaCelda);
                    }
                }

                #endregion

                sheetData.AppendChild(NuevaFila);

                #endregion
            }


        }

        #endregion

        public bool ExistsCategory(string categoryElement, long clienteID)
        {
            bool existe = false;

            try
            {
                existe = (from c in Context.InventarioCategorias
                          where c.InventarioCategoria == categoryElement &&
                                c.ClienteID == clienteID
                          select c).Count() > 0;
            }
            catch (Exception ex)
            {
                existe = false;
                log.Error(ex.Message);
            }

            return existe;
        }

        public bool EsCategoriaPadre(long inventarioCategoriaID)
        {
            bool bEsCategoriaPadre = (from c in Context.InventarioCategorias
                                      where c.InventarioCategoriaPadreID == inventarioCategoriaID
                                      select c).Count() > 0;

            return bEsCategoriaPadre;
        }

        public bool EsCategoriaPadrePermitida(long inventarioCategoriaID, long CategoriaPadreID, long EmplazamientoID)
        {
            bool bEsCategoriaPadre = false;
            try
            {
                Emplazamientos oEmplazamiento = (from c in Context.Emplazamientos where c.EmplazamientoID == EmplazamientoID select c).FirstOrDefault();
                bEsCategoriaPadre = (from c in Context.InventarioCategoriasVinculaciones
                                     where c.InventarioCategoriaPadreID == CategoriaPadreID && c.InventarioCategoriaID == inventarioCategoriaID && (c.EmplazamientoTipoID == oEmplazamiento.EmplazamientoTipoID || !(c.EmplazamientoTipoID.HasValue))
                                     select c).Count() > 0;
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return false;
            }

            return bEsCategoriaPadre;
        }

        public List<InventarioCategorias> GetByElementos(List<long> listIdElementosInventario)
        {
            List<InventarioCategorias> lista;
            try
            {
                lista = (from c in Context.InventarioCategorias
                         join elem in Context.InventarioElementos on c.InventarioCategoriaID equals elem.InventarioCategoriaID
                         where listIdElementosInventario.Contains(elem.InventarioElementoID)
                         select c).ToList();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                lista = null;
            }
            return lista;
        }

        public InventarioCategorias GetCategoriaByNombre(string sNombre)
        {
            InventarioCategorias oDato;
            try
            {
                oDato = (from c in Context.InventarioCategorias
                         where c.InventarioCategoria == sNombre
                         select c).First();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                oDato = null;
            }
            return oDato;
        }
        public InventarioCategorias GetCategoriaByCodigo(string sCodigo)
        {
            InventarioCategorias oDato;
            try
            {
                oDato = (from c in Context.InventarioCategorias
                         where c.Codigo == sCodigo
                         select c).First();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                oDato = null;
            }
            return oDato;
        }

        public List<InventarioCategorias> GetInventarioCategoriasByTipoEmplazamientoDiagrama(string EmplazamientoTipoID, long clienteID)
        {
            long lEmplazamientoTipoID;
            List<InventarioCategorias> listaDatos;
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
                if (lEmplazamientoTipoID != 0)
                {
                    listaDatos = (from c in Context.InventarioCategorias
                                  join
vin in Context.InventarioCategoriasVinculaciones on c.InventarioCategoriaID equals vin.InventarioCategoriaID
                                  where (c.EmplazamientoTipoID == lEmplazamientoTipoID || (c.EmplazamientoTipoID == null)) && c.ClienteID == clienteID 
                                  && vin.InventarioCategoriaPadreID == null
                                  select c).Distinct().ToList();
                }
                else
                {
                    listaDatos = (from c in Context.InventarioCategorias
                                  join
vin in Context.InventarioCategoriasVinculaciones on c.InventarioCategoriaID equals vin.InventarioCategoriaID
                                  where (c.EmplazamientoTipoID == lEmplazamientoTipoID || (c.EmplazamientoTipoID == null)) && c.ClienteID == clienteID
                                  && vin.InventarioCategoriaPadreID == null
                                  select c).Distinct().ToList();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                listaDatos = null;
            }
            return listaDatos;
        }

        public List<long> GetListByTipoEmplazamientoDiagramaPadre(string EmplazamientoTipoID, long clienteID, List<long> Padre)
        {
            long lEmplazamientoTipoID;
            List<long> listaDatos;
            
            
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
                
                if (lEmplazamientoTipoID != 0)
                {
                    listaDatos = (from c in Context.InventarioCategorias
                                  join
                                   vin in Context.InventarioCategoriasVinculaciones on c.InventarioCategoriaID equals vin.InventarioCategoriaID
                                  where (c.EmplazamientoTipoID == lEmplazamientoTipoID || (c.EmplazamientoTipoID == null)) && c.ClienteID == clienteID 
                                  && Padre.Contains(Convert.ToInt64(vin.InventarioCategoriaPadreID))
                                  select c.InventarioCategoriaID).Distinct().ToList();
                }
                else
                {
                    listaDatos = (from c in Context.InventarioCategorias
                                  join
vin in Context.InventarioCategoriasVinculaciones on c.InventarioCategoriaID equals vin.InventarioCategoriaID
                                  where (c.EmplazamientoTipoID == lEmplazamientoTipoID || (c.EmplazamientoTipoID == null)) && c.ClienteID == clienteID
                                  && Padre.Contains(Convert.ToInt64(vin.InventarioCategoriaPadreID))
                                  select c.InventarioCategoriaID).Distinct().ToList();
                }
                
                

              
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                listaDatos = null;
            }
            return listaDatos;
        }

        public List<InventarioCategorias> GetInventarioCategoriasByTipoEmplazamientoDiagramaPadre(List<long> Padre)
        {
            long lEmplazamientoTipoID;
            List<InventarioCategorias> listaDatos;


            
            try
            {
                    listaDatos = (from c in Context.InventarioCategorias where  Padre.Contains(Convert.ToInt64(c.InventarioCategoriaID))
                                  select c).Distinct().ToList();
                
                 
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                listaDatos = null;
            }
            return listaDatos;
        }

        public List<InventarioCategorias> GetVinculadas(long categoriaID)
        {
            List<InventarioCategorias> vinculadas;
            try
            {
                /*vinculadas = (from c in Context.InventarioCategorias
                              join vin1 in Context.InventarioCategoriasVinculaciones on c.InventarioCategoriaID equals vin1.InventarioCategoriaID into vin11
                              from vin111 in vin11.DefaultIfEmpty()
                              join vin2 in Context.InventarioCategoriasVinculaciones on c.InventarioCategoriaID equals vin2.InventarioCategoriaPadreID into vin22
                              from vin222 in vin22.DefaultIfEmpty()
                              where
                                (Convert.ToInt32(vin222.TipoRelacion) == (int)Comun.TiposVinculaciones.Rel_1_N ||
                                    Convert.ToInt32(vin222.TipoRelacion) == (int)Comun.TiposVinculaciones.Rel_1_1 ||
                                    Convert.ToInt32(vin111.TipoRelacion) == (int)Comun.TiposVinculaciones.Rel_N_1 ||
                                    Convert.ToInt32(vin111.TipoRelacion) == (int)Comun.TiposVinculaciones.Rel_1_1)
                                &&
                                ((vin111.InventarioCategoriaPadreID.HasValue && vin111.InventarioCategoriaPadreID.Value == categoriaID) ||
                                    vin222.InventarioCategoriaID == categoriaID)

                              select c).GroupBy(c => c.InventarioCategoriaID).Select(c => c.First()).ToList();*/

                vinculadas = (from c in Context.InventarioCategorias 
                              join vin in Context.InventarioCategoriasVinculaciones on c.InventarioCategoriaID equals vin.InventarioCategoriaPadreID
                              join cat in Context.InventarioCategorias on vin.InventarioCategoriaID equals cat.InventarioCategoriaID
                              where 
                                    c.InventarioCategoriaID == categoriaID && 
                                    (Convert.ToInt32(vin.TipoRelacion) == (int)Comun.TiposVinculaciones.Rel_1_1 || Convert.ToInt32(vin.TipoRelacion) == (int)Comun.TiposVinculaciones.Rel_N_1)
                              select cat).Union(
                                from c in Context.InventarioCategorias 
                                join vin in Context.InventarioCategoriasVinculaciones on c.InventarioCategoriaID equals vin.InventarioCategoriaID
                                join cat in Context.InventarioCategorias on vin.InventarioCategoriaPadreID equals cat.InventarioCategoriaID
                                where 
                                    c.InventarioCategoriaID==categoriaID &&
                                    (Convert.ToInt32(vin.TipoRelacion) == (int)Comun.TiposVinculaciones.Rel_1_1 || Convert.ToInt32(vin.TipoRelacion) == (int)Comun.TiposVinculaciones.Rel_1_N)
                                select cat
                                ).ToList();
            }
            catch(Exception ex)
            {
                log.Error(ex);
                vinculadas = new List<InventarioCategorias>();
            }

            return vinculadas;
        }

    }
}