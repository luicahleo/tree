using ExcelDataReader;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TreeCore.APIClient;
using TreeCore.Shared.DTO.Auth;
using TreeCore.Shared.DTO.Contracts;
using TreeCore.Shared.DTO.ImportExport;
using TreeCore.Shared.DTO.ValueObject;
using TreeCore.Shared.Utilities.Enum;

namespace TreeCore.BackEnd.WorkerServices.Imports
{
    public class ContractsImport : IBaseImport
    {
        public ContractsImport(ImportTaskDTO task) : base(task)
        {
        }

        public async override Task LoadFile(string sruta)
        {
            _correcto = true;
            string sRuta = TreeCore.DirectoryMapping.GetImportFilesDirectory();
            sRuta = Path.Combine(sRuta, sruta);
            try
            {
                logCarga.EscribeLogs($"Start Import Contracts");

                int iCargados = 0;
                int iActualizados = 0;
                int iErrores = 0;
                int iErroresCodes = 0;

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

                #region CONTROLLERS

                var cToken = new BaseAPIClient<TokenDTO>();
                TokenDTO oToken = cToken.Login("treeservices@atrebo.com", "Atrebo.2022").Result.Value;

                var cContract = new BaseAPIClient<ContractDTO>(oToken.AccessToken);
                var fullListContract = (await cContract.GetList()).Value;



                #endregion

                DataTable table = null;
                DataTable tableContractLine = null;
                DataTable tableContractLineTaxes = null;
                DataTable tableContractLineCompanies = null;

                if (result.Tables.Count > 1)
                {
                    table = result.Tables[0];
                    tableContractLine = result.Tables[1];
                    tableContractLineTaxes = result.Tables[2];
                    tableContractLineCompanies = result.Tables[3];
                }

                #region UPLOAD CONTRACT LINE TAXES 
         
               
                //Dictionary<string, Dictionary<string, List<ContractLineTaxesDTO>>> DicContractLineTaxesPadre = new Dictionary<string, Dictionary<string, List<ContractLineTaxesDTO>>>();
                
                //if (tableContractLineTaxes != null)
                //{
                //    foreach (DataRow row in tableContractLineTaxes.Rows)
                //    {
                //        string sContractCode = row[0].ToString();
                //        string sContractLineCode = row[1].ToString();

                //        string sTaxCode = row[2].ToString();
                //        if (sContractCode == "" || sContractCode == null)
                //        {
                //            AddError(sContractCode, _errorTraduccion.FormatError + ": Code is empty");
                //            iErroresCodes++;
                //        }
                //        if (sContractLineCode == "" || sContractLineCode == null)
                //        {
                //            AddError(sContractCode, _errorTraduccion.FormatError + ": contract line Code is empty");
                //        }
                //        if (sTaxCode == "" || sTaxCode == null)
                //        {
                //            AddError(sContractCode, _errorTraduccion.FormatError + ": Tax Code is empty");
                //        }

                //        ContractLineTaxesDTO oContractLineTaxes = new ContractLineTaxesDTO()
                //        {
                //            TaxCode = sTaxCode,

                //        };

                //        #region DICTIONARY
                //        List<ContractLineTaxesDTO> listcontractTaxesAux = new List<ContractLineTaxesDTO>();
                //        if (DicContractLineTaxesHijo.Count == 0  )
                //        {
                //            listcontractTaxesAux.Add(oContractLineTaxes);
                //            DicContractLineTaxesHijo[sContractLineCode] = listcontractTaxesAux;
                //            DicContractLineTaxesPadre[sContractCode] = DicContractLineTaxesHijo;


                //        }
                //        else
                //        {
                //            if (DicContractLineTaxesPadre.ContainsKey(sContractCode) && DicContractLineTaxesPadre[sContractCode].ContainsKey(sContractLineCode))
                //            {
                                
                //                listcontractTaxesAux = DicContractLineTaxesPadre[sContractCode][sContractLineCode];
                //                listcontractTaxesAux.Add(oContractLineTaxes);
                //                DicContractLineTaxesHijo[sContractLineCode] = listcontractTaxesAux;
                //                DicContractLineTaxesPadre[sContractCode] = DicContractLineTaxesHijo;


                //            }
                //            else
                //            {
                //                listcontractTaxesAux = new List<ContractLineTaxesDTO>();
                //                listcontractTaxesAux.Add(oContractLineTaxes);
                //                DicContractLineTaxesHijo[sContractLineCode] = listcontractTaxesAux;
                //                DicContractLineTaxesPadre[sContractCode] = DicContractLineTaxesHijo;
                //            }
                            

                //        }

                //        #endregion
                //    }
                //}
                

                #endregion

                #region UPLOAD CONTRACT LINE COMPANIES 

                
                //Dictionary<string, Dictionary<string, List<ContractLineEntidadDTO>>> DicContractLineEntidad = new Dictionary<string, Dictionary<string, List<ContractLineEntidadDTO>>>();
                
                //if(tableContractLineCompanies != null)
                //{
                //    foreach (DataRow row in tableContractLineCompanies.Rows)
                //    {
                //        string sContractCode = row[0].ToString();
                //        string sContractLineCode = row[1].ToString();
                //        string sCompanyCode = row[2].ToString();
                //        string sPaymentMethodCode = row[3].ToString();
                //        string sBankAcountCode = row[4].ToString();
                //        string sCurrencyCode = row[5].ToString();
                //        float fPercent = 0;
                //        if (row[6].ToString() != "" && !float.TryParse(row[6].ToString(), out fPercent))
                //        {
                //            AddError(sContractCode, _errorTraduccion.FormatError + ": " + sContractLineCode + " Percent:" + row[6].ToString());
                //        }
                //        if (sContractCode == "" || sContractCode == null)
                //        {
                //            //AddError(sContractCode, _errorTraduccion.FormatError + ": Code is empty");
                //            iErroresCodes++;
                //        }
                //        if (sContractLineCode == "" || sContractLineCode == null)
                //        {
                //            AddError(sContractCode, _errorTraduccion.FormatError + ": contract line Code is empty");
                //        }
                //        if (sCurrencyCode == "" || sCurrencyCode == null)
                //        {
                //            AddError(sContractCode, _errorTraduccion.FormatError + ": Currency code is empty");
                //        }
                //        if (sCompanyCode == "" || sCompanyCode == null)
                //        {
                //            AddError(sContractCode, _errorTraduccion.FormatError + ": Company code is empty");
                //        }
                //        if (sPaymentMethodCode == "" || sPaymentMethodCode == null)
                //        {
                //            AddError(sContractCode, _errorTraduccion.FormatError + ": Payment Methode code is empty");
                //        }

                //        ContractLineEntidadDTO oContractLineentidad = new ContractLineEntidadDTO()
                //        {
                //            CompanyCode = sCompanyCode,
                //            PaymentMethodCode = sPaymentMethodCode,
                //            BankAcountCode = sBankAcountCode,
                //            currencyCode = sCurrencyCode,
                //            Percent = fPercent,

                //        };

                //        #region DICTIONARY

                //        List<ContractLineEntidadDTO> listconContractLineEntidadDTO = new List<ContractLineEntidadDTO>();
                //        if (DicContractLineEntidaes.Count == 0 || !DicContractLineEntidaes.ContainsKey(sContractLineCode))
                //        {
                //            listconContractLineEntidadDTO.Add(oContractLineentidad);
                //            DicContractLineEntidaes[sContractLineCode] = listconContractLineEntidadDTO;
                //            DicContractLineEntidadPadre[sContractCode] = DicContractLineEntidaes;
                //        }
                //        else
                //        {
                //            listconContractLineEntidadDTO = DicContractLineEntidaes[sContractLineCode];
                //            listconContractLineEntidadDTO.Add(oContractLineentidad);
                //            DicContractLineEntidaes[sContractLineCode] = listconContractLineEntidadDTO;
                //            DicContractLineEntidadPadre[sContractCode] = DicContractLineEntidaes;

                //        }

                //        #endregion
                //    }
                //}

                #endregion

                #region UPLOAD CONTRACT LINE

                Dictionary<string, List<ContractLineDTO>> DicContractLine = new Dictionary<string, List<ContractLineDTO>>();

                if (tableContractLine != null)
                {
                    foreach (DataRow row in tableContractLine.Rows)
                    {


                       
                        string expresion = "";
                        #region CONTRACT LINE TAXES
                        Dictionary<string, List<ContractLineTaxesDTO>> DicContractLineTaxes = new Dictionary<string, List<ContractLineTaxesDTO>>();
                        if (tableContractLineTaxes != null)
                        {
                           
                            tableContractLineTaxes.Columns[0].ColumnName = "contractCode";
                            tableContractLineTaxes.Columns[1].ColumnName = "contractLine";

                            expresion = tableContractLineTaxes.Columns[0].ColumnName + " = " + "'" + row[0].ToString() + "'" + " and " + tableContractLineTaxes.Columns[1].ColumnName + " = " + "'" + row[1].ToString() + "'";
                            DataRow[] rowcontractlineTaxex = tableContractLineTaxes.Select(expresion);


                           

                            if (rowcontractlineTaxex.Length>0)
                            {
                                foreach (DataRow item in rowcontractlineTaxex)
                                {
                                    string sContractCode = item[0].ToString();
                                    string sContractLineCode = item[1].ToString();

                                    string sTaxCode = item[2].ToString();
                                    if (sContractCode == "" || sContractCode == null)
                                    {
                                        AddError(sContractCode, _errorTraduccion.FormatError + ": Code is empty");
                                        iErroresCodes++;
                                    }
                                    if (sContractLineCode == "" || sContractLineCode == null)
                                    {
                                        AddError(sContractCode, _errorTraduccion.FormatError + ": contract line Code is empty");
                                    }
                                    if (sTaxCode == "" || sTaxCode == null)
                                    {
                                        AddError(sContractCode, _errorTraduccion.FormatError + ": Tax Code is empty");
                                    }

                                    ContractLineTaxesDTO oContractLineTaxes = new ContractLineTaxesDTO()
                                    {
                                        TaxCode = sTaxCode,

                                    };

                                    #region DICTIONARY
                                    List<ContractLineTaxesDTO> listcontractTaxesAux = new List<ContractLineTaxesDTO>();
                                    if (DicContractLineTaxes.Count == 0 || DicContractLineTaxes.ContainsKey(sContractLineCode) == false)
                                    {
                                        listcontractTaxesAux.Add(oContractLineTaxes);
                                        DicContractLineTaxes[sContractLineCode] = listcontractTaxesAux;
                                    }
                                    else
                                    {
                                        listcontractTaxesAux = DicContractLineTaxes[sContractLineCode];
                                        listcontractTaxesAux.Add(oContractLineTaxes);
                                        DicContractLineTaxes[sContractLineCode] = listcontractTaxesAux;
                                    }

                                    #endregion
                                }
                            }
                           
                        }
                        #endregion

                        #region CONTRACT COMPANIES
                        Dictionary<string, List<ContractLineEntidadDTO>> DicContractLineEntidaes = new Dictionary<string, List<ContractLineEntidadDTO>>();
                        if (tableContractLineCompanies != null)
                        {
                            tableContractLineCompanies.Columns[0].ColumnName = "contractCode";
                            tableContractLineCompanies.Columns[1].ColumnName = "contractLine";

                            expresion = tableContractLineCompanies.Columns[0].ColumnName + " = " + "'" + row[0].ToString() + "'" + " and " + tableContractLineCompanies.Columns[1].ColumnName + " = " + "'" + row[1].ToString() + "'";
                            DataRow[] rowcontractlinecompanies = tableContractLineCompanies.Select(expresion);
                            if (rowcontractlinecompanies.Length > 0)
                            {
                                foreach (DataRow item in rowcontractlinecompanies)
                                {
                                    string sContractCode = item[0].ToString();
                                    string sContractLineCode = item[1].ToString();
                                    string sCompanyCode = item[2].ToString();
                                    string sPaymentMethodCode = item[3].ToString();
                                    string sBankAcountCode = item[4].ToString();
                                    string sCurrencyCodepayees = item[5].ToString();
                                    float fPercent = 0;
                                    if (item[6].ToString() != "" && !float.TryParse(item[6].ToString(), out fPercent))
                                    {
                                        AddError(sContractCode, _errorTraduccion.FormatError + ": " + sContractLineCode + " Percent:" + item[6].ToString());
                                    }
                                    if (sContractCode == "" || sContractCode == null)
                                    {
                                        //AddError(sContractCode, _errorTraduccion.FormatError + ": Code is empty");
                                        iErroresCodes++;
                                    }
                                    if (sContractLineCode == "" || sContractLineCode == null)
                                    {
                                        AddError(sContractCode, _errorTraduccion.FormatError + ": contract line Code is empty");
                                    }
                                    if (sCurrencyCodepayees == "" || sCurrencyCodepayees == null)
                                    {
                                        AddError(sContractCode, _errorTraduccion.FormatError + ": Currency code is empty");
                                    }
                                    if (sCompanyCode == "" || sCompanyCode == null)
                                    {
                                        AddError(sContractCode, _errorTraduccion.FormatError + ": Company code is empty");
                                    }
                                    if (sPaymentMethodCode == "" || sPaymentMethodCode == null)
                                    {
                                        AddError(sContractCode, _errorTraduccion.FormatError + ": Payment Methode code is empty");
                                    }

                                    ContractLineEntidadDTO oContractLineentidad = new ContractLineEntidadDTO()
                                    {
                                        CompanyCode = sCompanyCode,
                                        PaymentMethodCode = sPaymentMethodCode,
                                        BankAcountCode = sBankAcountCode,
                                        currencyCode = sCurrencyCodepayees,
                                        Percent = fPercent,

                                    };

                                    #region DICTIONARY

                                    List<ContractLineEntidadDTO> listconContractLineEntidadDTO = new List<ContractLineEntidadDTO>();
                                    if (DicContractLineEntidaes.Count == 0 || !DicContractLineEntidaes.ContainsKey(sContractLineCode))
                                    {
                                        listconContractLineEntidadDTO.Add(oContractLineentidad);
                                        DicContractLineEntidaes[sContractLineCode] = listconContractLineEntidadDTO;
                                        
                                    }
                                    else
                                    {
                                        listconContractLineEntidadDTO = DicContractLineEntidaes[sContractLineCode];
                                        listconContractLineEntidadDTO.Add(oContractLineentidad);
                                        DicContractLineEntidaes[sContractLineCode] = listconContractLineEntidadDTO;
                                       

                                    }

                                    #endregion
                                }
                            }
                        }
                        #endregion

                        #region CONTRACT LINE
                        string scontractCode = row[0].ToString();
                        string scontractLineCode = row[1].ToString();
                        string sDescription = row[2].ToString();
                        string sLineType = row[3].ToString();

                        int iFrecuency = 0;
                        if (row[4].ToString() != "" && !int.TryParse(row[4].ToString(), out iFrecuency))
                        {
                            AddError(scontractCode, _errorTraduccion.FormatError + ": " + scontractLineCode + " Frecuency:" + row[4].ToString());
                        }
                        float fValue = 0;
                        if (row[5].ToString() != "" && !float.TryParse(row[5].ToString(), out fValue))
                        {
                            AddError(scontractCode, _errorTraduccion.FormatError + ": " + scontractLineCode + " Value:" + row[5].ToString());
                        }
                        string sCurrencyCode = row[6].ToString();
                        
                        DateTime dValidFrom = new DateTime();
                        if (!DateTime.TryParse(row[7].ToString(), out dValidFrom))
                        {
                            AddError(scontractCode, _errorTraduccion.FormatError + ": " + scontractLineCode + " ValidFrom:" + row[7].ToString());
                        }
                        DateTime dValidTo = new DateTime();
                        if (!DateTime.TryParse(row[8].ToString(), out dValidTo))
                        {
                            AddError(scontractCode, _errorTraduccion.FormatError + ": " + scontractLineCode + " ValidTo:" + row[8].ToString());
                        }
                        DateTime dNextPayment = new DateTime();
                        if (!DateTime.TryParse(row[9].ToString(), out dNextPayment))
                        {
                            AddError(scontractCode, _errorTraduccion.FormatError + ": " + scontractLineCode + " NextPayment:" + row[9].ToString());
                        }

                        bool bApplyRenewals = false;
                        if (!bool.TryParse(row[10].ToString(), out bApplyRenewals))
                        {
                            AddError(scontractCode, _errorTraduccion.FormatError + ": " + scontractLineCode + " ApplyRenewals:" + row[10].ToString());
                        }
                        bool bApportionment = false;
                        if (!bool.TryParse(row[11].ToString(), out bApportionment))
                        {
                            AddError(scontractCode, _errorTraduccion.FormatError + ": " + scontractLineCode + " Apportionment:" + row[11].ToString());
                        }
                        bool bPrepaid = false;
                        if (!bool.TryParse(row[12].ToString(), out bPrepaid))
                        {
                            AddError(scontractCode, _errorTraduccion.FormatError + ": " + scontractLineCode + " Prepaid:" + row[12].ToString());
                        }

                        string sType = row[13].ToString();
                        if (sType == "" || sType == null)
                        {
                            AddError(scontractCode, _errorTraduccion.FormatError + ": Price Readjustment Type is empty");
                        }
                        if (sType != PReadjustment.sWithoutIncrements && sType != PReadjustment.sFixedPercentege && sType != PReadjustment.sFixedAmount && sType != PReadjustment.sPCI )
                        {
                            AddError(scontractCode, _errorTraduccion.FormatError + ": Price Readjustment Type : "+ sType);
                        }
                        string sInflacion = row[14].ToString();
                        float fFixedAmount = 0;
                        if (row[15].ToString() != "" && !float.TryParse(row[15].ToString(), out fFixedAmount) && sType != PReadjustment.sWithoutIncrements)
                        {
                            AddError(scontractCode, _errorTraduccion.FormatError + ": " + scontractLineCode + " FixedAmount:" + row[15].ToString());
                        }
                        float fFixedPercentage = 0;
                        if (row[16].ToString() != "" && !float.TryParse(row[16].ToString(), out fFixedPercentage) && sType != PReadjustment.sWithoutIncrements)
                        {
                            AddError(scontractCode, _errorTraduccion.FormatError + ": " + scontractLineCode + " FixedPercentage:" + row[16].ToString());
                        }
                        int iFrencuencia = 0;
                        if (row[17].ToString() != "" && !int.TryParse(row[17].ToString(), out iFrencuencia) && sType != PReadjustment.sWithoutIncrements)
                        {
                            AddError(scontractCode, _errorTraduccion.FormatError + ": " + scontractLineCode + " Frencuencia:" + row[17].ToString());
                        }
                        DateTime dstartDate = new DateTime();
                        if (row[18].ToString() != "" && !DateTime.TryParse(row[18].ToString(), out dstartDate) && sType != PReadjustment.sWithoutIncrements)
                        {
                            AddError(scontractCode, _errorTraduccion.FormatError + ": " + scontractLineCode + " dstartDate:" + row[18].ToString());
                        }
                        DateTime dnextDate = new DateTime();
                        if (row[19].ToString() != "" && !DateTime.TryParse(row[19].ToString(), out dnextDate) && sType != PReadjustment.sWithoutIncrements)
                        {
                            AddError(scontractCode, _errorTraduccion.FormatError + ": " + scontractLineCode + " dnextDate:" + row[19].ToString());
                        }
                        DateTime dEndDate = new DateTime();
                        if (row[20].ToString() != "" && !DateTime.TryParse(row[20].ToString(), out dEndDate) && sType != PReadjustment.sWithoutIncrements)
                        {
                            AddError(scontractCode, _errorTraduccion.FormatError + ": " + scontractLineCode + " dEndDate:" + row[20].ToString());
                        }
                        if (scontractCode == "" || scontractCode == null)
                        {
                            iErroresCodes++;
                        }

                        if (scontractLineCode == "" || scontractLineCode == null)
                        {
                            AddError(scontractCode, _errorTraduccion.FormatError + ": Contract  line code is empty");
                        }

                        if (sDescription == "" || sDescription == null)
                        {
                            AddError(scontractCode, _errorTraduccion.FormatError + ": Description line  is empty");
                        }
                        if (sLineType == "" || sLineType == null)
                        {
                            AddError(scontractCode, _errorTraduccion.FormatError + ": Contract Line Type  Code   is empty");
                        }
                        PriceReadjustmentDTO OpriceReadjustment = new PriceReadjustmentDTO()
                        {
                            Type = sType,
                            CodeInflation = sInflacion,
                            FixedAmount = fFixedAmount,
                            FixedPercentage = fFixedPercentage,
                            Frequency = iFrecuency,
                            StartDate = (dstartDate != DateTime.MinValue && dstartDate != new DateTime()) ? dstartDate : null,
                            NextDate = (dnextDate != DateTime.MinValue && dnextDate != new DateTime()) ? dnextDate : null,
                            EndDate = (dEndDate != DateTime.MinValue && dEndDate != new DateTime()) ? dEndDate : null,
                        };

                        List<ContractLineEntidadDTO> listContractLinecompaniesDto = new List<ContractLineEntidadDTO>();
                        if (DicContractLineEntidaes.ContainsKey(scontractLineCode) )
                        {
                            listContractLinecompaniesDto = DicContractLineEntidaes[scontractLineCode];
                        }
                        List<ContractLineTaxesDTO> listContractLinetaxesDto = new List<ContractLineTaxesDTO>();
                        if (DicContractLineTaxes.ContainsKey(scontractLineCode) )
                        {
                            listContractLinetaxesDto = DicContractLineTaxes[scontractLineCode];
                        }
                        ContractLineDTO oContractLineDTO = new ContractLineDTO()
                        {
                            Code = scontractLineCode,

                            Description = sDescription,
                            lineTypeCode = sLineType,
                            Frequency = iFrecuency,
                            Value = fValue,
                            CurrencyCode = sCurrencyCode,
                            ValidFrom = dValidFrom,
                            ValidTo = dValidTo,
                            NextPaymentDate = dNextPayment,
                            ApplyRenewals = bApplyRenewals,
                            Apportionment = bApportionment,
                            Prepaid = bPrepaid,
                            PricesReadjustment = OpriceReadjustment,
                            ContractLineTaxes = listContractLinetaxesDto,
                            Payees = listContractLinecompaniesDto,
                        };

                        #region DICTIONARY

                        List<ContractLineDTO> listcontractlines = new List<ContractLineDTO>();
                        if (DicContractLine.Count == 0 || DicContractLine.ContainsKey(scontractCode) == false)
                        {
                            listcontractlines.Add(oContractLineDTO);
                            DicContractLine[scontractCode] = listcontractlines;
                        }
                        else
                        {
                            listcontractlines = DicContractLine[scontractCode];
                            listcontractlines.Add(oContractLineDTO);
                            DicContractLine[scontractCode] = listcontractlines;

                        }

                        #endregion

                        #endregion
                    }
                }

                #endregion

                #region UPLOAD DATA

                List<ContractDTO> listContract = new List<ContractDTO>();
                List<WaitHandle> waitHandles = new List<WaitHandle>();

                if (table != null)
                {
                    foreach (DataRow row in table.Rows)
                    {
                        var handle = new EventWaitHandle(false, EventResetMode.ManualReset);
                        var thread = new Thread(() =>
                        {
                            string sCode = row[0].ToString();
                            string sName = row[1].ToString();
                            string sDescription = row[2].ToString();
                            string sSiteCode = row[3].ToString();
                            string sContractStatusCode = row[4].ToString();
                            string sCurrencyCode = row[5].ToString();
                            string sContractGroupCode = row[6].ToString();
                            string sContractTypeCode = row[7].ToString();
                            string sMasterContractNumber = row[8].ToString();
                            DateTime dateExecutionDate = new DateTime();

                          

                            if (!DateTime.TryParse(row[9].ToString(), out dateExecutionDate))
                            {
                                AddError(sCode, _errorTraduccion.FormatError + ": " + " ExecutionDate:" + row[9].ToString());
                            }
                            DateTime dateStartDate = new DateTime();
                            if (!DateTime.TryParse(row[10].ToString(), out dateStartDate))
                            {
                                AddError(sCode, _errorTraduccion.FormatError + ": " + " StartDate:" + row[10].ToString());
                            }
                            DateTime datefirstEndDate = new DateTime();
                            if (!DateTime.TryParse(row[11].ToString(), out datefirstEndDate))
                            {
                                AddError(sCode, _errorTraduccion.FormatError + ": " + " firstEndDate:" + row[11].ToString());
                            }
                            //int iDuration = 0;
                            //if (!int.TryParse(row[12].ToString(), out iDuration))
                            //{
                            //    AddError(sCode, _errorTraduccion.FormatError + ": " + " Duration:" + row[12].ToString());
                            //}
                            bool bCloseAtExpiration = false;
                            if (!bool.TryParse(row[12].ToString(), out bCloseAtExpiration))
                            {
                                AddError(sCode, _errorTraduccion.FormatError + ": " + " CloseAtExpiration:" + row[12].ToString());
                            }
                            string sType = row[13].ToString();
                            if (sType == "" || sType == null)
                            {
                                AddError(sCode, _errorTraduccion.FormatError + ": Renewal Clause Type  is empty");
                            }
                            if (sType != RenewalReadjustment.sAuto && sType != RenewalReadjustment.sOptional && sType != RenewalReadjustment.sPreviousNegotiation)
                            {
                                AddError(sCode, _errorTraduccion.FormatError + ": Renewal Clause Type : " + sType);
                            }
                           
                           

                            int IFrecuency = 0;
                            if (row[14].ToString() != "" && !int.TryParse(row[14].ToString(), out IFrecuency))
                            {
                                AddError(sCode, _errorTraduccion.FormatError + ": " + " Frecuency:" + row[14].ToString());
                            }
                            int ItotalRenewalNumber = 0;
                            if (row[15].ToString() != "" && !int.TryParse(row[15].ToString(), out ItotalRenewalNumber))
                            {
                                AddError(sCode, _errorTraduccion.FormatError + ": " + " Total Renewal Number:" + row[15].ToString());
                            }
                           
                            //if (row[17].ToString() != "" && !int.TryParse(row[17].ToString(), out ICurrentRenewalNumber))
                            //{
                            //    AddError(sCode, _errorTraduccion.FormatError + ": " + " Current Renewal Number:" + row[17].ToString());
                            //}
                            int INotificationDays = 0;
                            if (row[16].ToString() != "" && !int.TryParse(row[16].ToString(), out INotificationDays))
                            {
                                AddError(sCode, _errorTraduccion.FormatError + ": " + " Notification Days:" + row[16].ToString());
                            }
                           
                            //if (row[19].ToString() != "" && !DateTime.TryParse(row[19].ToString(), out dateRenewalDate))
                            //{
                            //    AddError(sCode, _errorTraduccion.FormatError + ": " + " RenewalDate:" + row[19].ToString());
                            //}
                            DateTime dateNotificacionDate = new DateTime();
                            if (row[17].ToString() != "" && !DateTime.TryParse(row[17].ToString(), out dateNotificacionDate))
                            {
                                AddError(sCode, _errorTraduccion.FormatError + ": " + " NotificacionDate:" + row[17].ToString());
                            }
                           
                            //if (row[21].ToString() != "" && !DateTime.TryParse(row[21].ToString(), out dateExpirationDate))
                            //{
                            //    AddError(sCode, _errorTraduccion.FormatError + ": " + " ExpirationDate:" + row[21].ToString());
                            //}
                            if (sCode == "" || sCode == null)
                            {
                                iErroresCodes++;
                            }

                            if (sName == "" || sName == null)
                            {
                                AddError(sCode, _errorTraduccion.FormatError + ": Name  code is empty");
                            }
                            if (sDescription == "" || sDescription == null)
                            {
                                AddError(sCode, _errorTraduccion.FormatError + ": Description   is empty");
                            }
                            if (sSiteCode == "" || sSiteCode == null)
                            {
                                AddError(sCode, _errorTraduccion.FormatError + ": SiteCode   is empty");
                            }
                            if (sContractStatusCode == "" || sContractStatusCode == null)
                            {
                                AddError(sCode, _errorTraduccion.FormatError + ": Contract Status Code   is empty");
                            }
                            if (sContractGroupCode == "" || sContractGroupCode == null)
                            {
                                AddError(sCode, _errorTraduccion.FormatError + ": Contract Group Code   is empty");
                            }
                            if (sCurrencyCode == "" || sCurrencyCode == null)
                            {
                                AddError(sCode, _errorTraduccion.FormatError + ": Currency  Code   is empty");
                            }
                            if (sContractTypeCode == "" || sContractTypeCode == null)
                            {
                                AddError(sCode, _errorTraduccion.FormatError + ": Contract Type  Code   is empty");
                            }
                           
                            RenewalClauseDTO oRenewalClause = new RenewalClauseDTO()
                            {
                                Type = sType,
                                Frequencymonths = IFrecuency,
                                TotalRenewalNumber = ItotalRenewalNumber,
                                CurrentRenewalNumber = null,
                                NotificationNumberDays = INotificationDays,
                                RenewalExpirationDate = null,
                                Renewalnotificationdate = (dateNotificacionDate != DateTime.MinValue && dateNotificacionDate != new DateTime()) ? dateNotificacionDate : null,
                                ContractExpirationDate = null,
                            };

                            List<ContractLineDTO> listContractLinesDto = new List<ContractLineDTO>();
                            if (DicContractLine.ContainsKey(sCode))
                            {
                                listContractLinesDto = DicContractLine[sCode];
                            }

                            ContractDTO oDTO = new ContractDTO
                            {
                                Code = sCode,
                                Name = sName,
                                Description = sDescription,
                                SiteCode = sSiteCode,
                                ContractStatusCode = sContractStatusCode,
                                CurrencyCode = sCurrencyCode,
                                ContractGroupCode = sContractGroupCode,
                                ContractTypeCode = sContractTypeCode,
                                MasterContractNumber = sMasterContractNumber,
                                ExecutionDate = dateExecutionDate,
                                StartDate = dateStartDate,
                                FirstEndDate = datefirstEndDate,
                                
                                ClosedAtExpiration = bCloseAtExpiration,
                                RenewalClause = oRenewalClause,
                                contractline = listContractLinesDto,

                            };
                            listContract.Add(oDTO);
                            handle.Set();
                        });
                        waitHandles.Add(handle);
                        thread.Start();
                    }
                    WaitHandle.WaitAll(waitHandles.ToArray());
                }

                #endregion

                #region ADD DATA

                foreach (var oContract in listContract)
                {

                    if (oContract.Code != "" && listaErrores[oContract.Code] == null)
                    {
                        if ((from c in fullListContract where c.Code == oContract.Code select c).FirstOrDefault() == null)
                        {
                            var oResult = await cContract.AddEntity(oContract);
                            if (oResult.Success)
                            {
                                iCargados++;
                                fullListContract.Add(oResult.Value);
                            }
                            else
                            {
                                iErrores++;
                                logCarga.EscribeLogs($"Errors With Contract {oContract.Code}");
                                foreach (var Error in oResult.Errors)
                                {
                                    logCarga.EscribeLogs(Error.Message);
                                }
                            }
                        }
                        else
                        {
                            var oResult = await cContract.UpdateEntity(oContract.Code, oContract);
                            if (oResult.Success)
                            {
                                iActualizados++;
                            }
                            else
                            {
                                iErrores++;
                                logCarga.EscribeLogs($"Errors With Contract {oContract.Code}");
                                foreach (var Error in oResult.Errors)
                                {
                                    logCarga.EscribeLogs(Error.Message);
                                }
                            }
                        }
                    }
                    else if(oContract.Code != "")
                    {
                        iErrores++;
                        logCarga.EscribeLogs($"Errors With Contract {oContract.Code}");
                        foreach (var Error in (List<string>)listaErrores[oContract.Code])
                        {
                            logCarga.EscribeLogs(Error);
                        }
                    }
                    
                }

                #endregion

                logCarga.EscribeLogs("-------------------------");

                if (table == null)
                {
                    logCarga.EscribeLogs("TOTAL CONTRACTS: 0");
                }
                else
                {
                    logCarga.EscribeLogs("TOTAL CONTRACTS" + ": " + table.Rows.Count);
                }
                
                logCarga.EscribeLogs("CONTRACTS ADDED" + ": " + iCargados.ToString());
                logCarga.EscribeLogs("CONTRACTS UPDATED" + ": " + iActualizados.ToString());
                logCarga.EscribeLogs("CONTRACTS ERROR" + ": " + iErrores.ToString());
                logCarga.EscribeLogs("CONTRACTS CODES EMPTIES" + ": " + iErroresCodes.ToString());
                logCarga.EscribeLogs("END CONTRACTS");
                logCarga.EscribeLogs("-------------------------");
                datos.Close();
            }
            catch (Exception ex)
            {
                System.IO.StreamReader datos = new System.IO.StreamReader(sRuta);
                datos.Close();
                _correcto = false;
                log.Error(ex.Message);
                logCarga.EscribeLogs(ex.Message);
            }
            await base.LoadFile(sRuta);
        }
    }
}
