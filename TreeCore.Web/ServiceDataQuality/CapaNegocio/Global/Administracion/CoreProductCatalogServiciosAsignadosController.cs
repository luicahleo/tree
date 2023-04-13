using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using TreeCore.Data;
using Ext.Net;
using System.IO;
using Tree.Linq.GenericExtensions;
using System.Web;

namespace CapaNegocio
{
    public class CoreProductCatalogServiciosAsignadosController : GeneralBaseController<CoreProductCatalogServiciosAsignados, TreeCoreContext>
    {
        public CoreProductCatalogServiciosAsignadosController()
            : base()
        { }

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
    }
}