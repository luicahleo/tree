using ExcelDataReader;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TreeCore.Shared.DTO.Companies;
using TreeCore.APIClient;
using TreeCore.Shared.DTO.ProductCatalog;
using TreeCore.Shared.DTO.ImportExport;
using System.Collections;
using System.Threading;

namespace TreeCore.BackEnd.WorkerServices.Imports
{
    public class ProductImport : IBaseImport
    {
        public ProductImport(ImportTaskDTO task) : base(task)
        {
        }

        public async override Task LoadFile(string sruta)
        {
            _correcto = true;
            string sRuta = TreeCore.DirectoryMapping.GetImportFilesDirectory();
            sRuta = Path.Combine(sRuta, sruta);
            try
            {
                logCarga.EscribeLogs($"Start Import {_traduccion.Product} {_task.Code}");

                int iCargados = 0;
                int iActualizados = 0;
                int iErrores = 0;

                System.IO.StreamReader datos = new System.IO.StreamReader(sRuta);
                IExcelDataReader excelReader = ExcelReaderFactory.CreateOpenXmlReader(datos.BaseStream);
                System.Globalization.NumberFormatInfo prov = new System.Globalization.NumberFormatInfo();
                prov.NumberDecimalSeparator = ".";

                DataSet result = excelReader.AsDataSet(new ExcelDataSetConfiguration()
                {
                    ConfigureDataTable = (tableReader) => new ExcelDataTableConfiguration()
                    {
                        UseHeaderRow = true
                    }
                });

                #region Controllers

                var cProduct = new BaseAPIClient<ProductDTO>();
                var fullListProduct = (await cProduct.GetList()).Value;

                #endregion

                DataTable tablaProductos = result.Tables[0];
                DataTable tablaVinculaciones = result.Tables[1];

                #region UPLOAD LINKED

                Dictionary<string, List<string>> DicProducts = new Dictionary<string, List<string>>();
                List<string> PacksCodes = new List<string>();
                List<string> ProductsCodes = new List<string>();                

                foreach (DataRow row in tablaVinculaciones.Rows)
                {
                    string sPackCode = row[0].ToString();
                    string sProductsCode = row[1].ToString();
                    PacksCodes.Add(sPackCode);
                    ProductsCodes.Add(sProductsCode);
                    if (ProductsCodes.Contains(sPackCode))
                    {
                        AddError(sPackCode, _errorTraduccion.ErrorPack);
                    }
                    if (PacksCodes.Contains(sProductsCode))
                    {
                        AddError(sPackCode, _errorTraduccion.ErrorPack);
                    }
                    List<string> listProductsAso = DicProducts[sPackCode];
                    if (listProductsAso == null)
                    {
                        listProductsAso = new List<string>();
                    }
                    listProductsAso.Add(sProductsCode);
                    DicProducts[sPackCode] = listProductsAso;
                }

                #endregion

                #region UPLOAD DATA

                List<string> listaCods = new List<string>();
                Dictionary<string, ProductDTO> listProducts = new Dictionary<string, ProductDTO>();
                List<WaitHandle> waitHandles = new List<WaitHandle>();

                foreach (DataRow row in tablaProductos.Rows)
                {
                    var handle = new EventWaitHandle(false, EventResetMode.ManualReset);
                    var thread = new Thread(() =>
                    {
                        string sCode = row[0].ToString();
                        string sName = row[1].ToString();
                        float sAmount = 0;
                        if (!float.TryParse(row[2].ToString(), out sAmount))
                        {
                            AddError(sCode, _errorTraduccion.FormatError);
                        }
                        string sCompanyCode = row[3].ToString();
                        string sProducTypeCode = row[4].ToString();

                        ProductDTO oDTO = new ProductDTO
                        {
                            Code = sCode,
                            Name = sName,
                            Amount = sAmount,
                            ProductTypeCode = sProducTypeCode
                        };
                        if (DicProducts.ContainsKey(sCode))
                        {
                            oDTO.LinkedProducts = DicProducts[sCode];
                        }
                        listProducts[sCode] = oDTO;
                        listaCods.Add(sCode);
                        handle.Set();
                    });
                    waitHandles.Add(handle);
                    thread.Start();
                }
                WaitHandle.WaitAll(waitHandles.ToArray());

                #endregion

                #region ADD DATA

                foreach (var oProducto in ProductsCodes)
                {
                    ProductDTO product = listProducts[oProducto];
                    if (product != null)
                    {
                        listaCods.Remove(oProducto);
                        if (listaErrores[oProducto] == null)
                        {
                            if ((from c in fullListProduct where c.Code == product.Code select c).FirstOrDefault() == null)
                            {
                                var oResult = await cProduct.AddEntity(product);
                                if (oResult.Success)
                                {
                                    iCargados++;
                                    fullListProduct.Add(oResult.Value);
                                }
                                else
                                {
                                    iErrores++;
                                    logCarga.EscribeLogs($"Errors Whit Product {oProducto}");
                                    foreach (var Error in oResult.Errors)
                                    {
                                        logCarga.EscribeLogs(Error.Message);
                                    }
                                }
                            }
                            else
                            {
                                var oResult = await cProduct.UpdateEntity(product.Code, product);
                                if (oResult.Success)
                                {
                                    iActualizados++;
                                }
                                else
                                {
                                    iErrores++;
                                    logCarga.EscribeLogs($"Errors Whit Product {oProducto}");
                                    foreach (var Error in oResult.Errors)
                                    {
                                        logCarga.EscribeLogs(Error.Message);
                                    }
                                }
                            }
                        }
                        else
                        {
                            iErrores++;
                            logCarga.EscribeLogs($"Errors Whit Product {oProducto}");
                            foreach (var Error in (List<string>)listaErrores[oProducto])
                            {
                                logCarga.EscribeLogs(Error);
                            }
                        }
                    }
                }

                foreach (var oProducto in listaCods)
                {
                    ProductDTO product = listProducts[oProducto];
                    if (product != null)
                    {
                        if (!listaErrores.ContainsKey(oProducto))
                        {
                            if ((from c in fullListProduct where c.Code == product.Code select c).FirstOrDefault() == null)
                            {
                                var oResult = await cProduct.AddEntity(product);
                                if (oResult.Success)
                                {
                                    iCargados++;
                                    fullListProduct.Add(oResult.Value);
                                }
                                else
                                {
                                    iErrores++;
                                    logCarga.EscribeLogs($"Errors Whit Product {oProducto}");
                                    foreach (var Error in oResult.Errors)
                                    {
                                        logCarga.EscribeLogs(Error.Message);
                                    }
                                }
                            }
                            else
                            {
                                var oResult = await cProduct.UpdateEntity(product.Code, product);
                                if (oResult.Success)
                                {
                                    iActualizados++;
                                }
                                else
                                {
                                    iErrores++;
                                    logCarga.EscribeLogs($"Errors Whit Product {oProducto}");
                                    foreach (var Error in oResult.Errors)
                                    {
                                        logCarga.EscribeLogs(Error.Message);
                                    }
                                }
                            }
                        }
                        else
                        {
                            iErrores++;
                            logCarga.EscribeLogs($"Errors Whit Product {oProducto}");
                            foreach (var Error in (List<string>)listaErrores[oProducto])
                            {
                                logCarga.EscribeLogs(Error);
                            }
                        }
                    }
                }
                #endregion

                logCarga.EscribeLogs("-------------------------");
                logCarga.EscribeLogs("TOTAL PRODUCT CATALOG SERVICES" + ": " + tablaProductos.Rows.Count);
                logCarga.EscribeLogs("PRODUCT CATALOG SERVICES ADDED" + ": " + iCargados.ToString());
                logCarga.EscribeLogs("PRODUCT CATALOG SERVICES UPDATED" + ": " + iActualizados.ToString());
                logCarga.EscribeLogs("PRODUCT CATALOG SERVICES ERROR" + ": " + iErrores.ToString());
                logCarga.EscribeLogs("END PRODUCT CATALOG SERVICES");
                logCarga.EscribeLogs("-------------------------");
            }
            catch (Exception ex)
            {
                _correcto = false;
                log.Error(ex.Message);
            }
            await base.LoadFile(sRuta);
        }
    }
}
