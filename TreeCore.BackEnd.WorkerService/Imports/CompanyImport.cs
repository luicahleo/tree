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
using TreeCore.Shared.DTO.Companies;
using TreeCore.Shared.DTO.General;
using TreeCore.Shared.DTO.ImportExport;
using TreeCore.Shared.DTO.ValueObject;

namespace TreeCore.BackEnd.WorkerServices.Imports
{
    public class CompanyImport : IBaseImport
    {
        public CompanyImport(ImportTaskDTO task) : base(task)
        {
        }

        public async override Task LoadFile(string sruta)
        {
            _correcto = true;
            string sRuta = TreeCore.DirectoryMapping.GetImportFilesDirectory();
            sRuta = Path.Combine(sRuta, sruta);
            try
            {
                logCarga.EscribeLogs($"Start Import Companies:  {_task.Code}");

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

                var cCompany = new BaseAPIClient<CompanyDTO>(oToken.AccessToken);
                var fullListProduct = (await cCompany.GetList()).Value;

                #endregion

                DataTable tableCompany = null;
                DataTable tableBankAccount = null;
                DataTable tableAddress = null;
                DataTable tablePaymentMethod = null;

                if (result.Tables.Count > 1)
                {
                    tableCompany = result.Tables[0];
                    tableBankAccount = result.Tables[1];
                    tableAddress = result.Tables[2];
                    tablePaymentMethod = result.Tables[3];
                }

                #region UPLOAD BANK ACCOUNT

                Dictionary<string, List<BankAccountDTO>> dicBankAccount = new Dictionary<string, List<BankAccountDTO>>();

                if (tableBankAccount != null)
                {
                    foreach (DataRow row in tableBankAccount.Rows)
                    {
                        string sCompanyCode = row[0].ToString();
                        string sBankCode = row[1].ToString();
                        string Code = row[2].ToString();
                        string sIBAN = row[3].ToString();
                        string sDescription = row[4].ToString();
                        string sSWIFT = row[5].ToString();

                        #region MESSAGES

                        if (sCompanyCode == "" || sCompanyCode == null)
                        {
                            iErroresCodes++;
                        }

                        if (sBankCode == "" || sBankCode == null)
                        {
                            AddError(sCompanyCode, _errorTraduccion.FormatError + ": Bank Code is empty");
                        }

                        if (Code == "" || Code == null)
                        {
                            AddError(sCompanyCode, _errorTraduccion.FormatError + ": Bank Account Code is empty");
                        }

                        if (sIBAN == "" || sIBAN == null)
                        {
                            AddError(sCompanyCode, _errorTraduccion.FormatError + ": IBAN is empty");
                        }

                        if (sDescription == "" || sDescription == null)
                        {
                            AddError(sCompanyCode, _errorTraduccion.FormatError + ": Description is empty");
                        }

                        if (sSWIFT == "" || sSWIFT == null)
                        {
                            AddError(sCompanyCode, _errorTraduccion.FormatError + ": SWIFT is empty");
                        }

                        #endregion

                        BankAccountDTO oBankAccount = new BankAccountDTO()
                        {
                            BankCode = sBankCode,
                            Code = Code,
                            IBAN = sIBAN,
                            Description = sDescription,
                            SWIFT = sSWIFT
                        };

                        List<BankAccountDTO> listBAccount = new List<BankAccountDTO>();
                        if (dicBankAccount.Count == 0 || !dicBankAccount.ContainsKey(sCompanyCode))
                        {
                            listBAccount.Add(oBankAccount);
                            dicBankAccount[sCompanyCode] = listBAccount;
                        }
                        else
                        {
                            listBAccount = dicBankAccount[sCompanyCode];
                            listBAccount.Add(oBankAccount);
                            dicBankAccount[sCompanyCode] = listBAccount;
                        }
                    }
                }

                #endregion

                #region UPLOAD ADDRESS

                Dictionary<string, List<CompanyAddressDTO>> dicAddress = new Dictionary<string, List<CompanyAddressDTO>>();

                if (tableAddress != null)
                {
                    foreach (DataRow row in tableAddress.Rows)
                    {
                        string sCompanyCode = row[0].ToString();
                        string sCode = row[1].ToString();
                        string sName = row[2].ToString();
                        string sDefault = row[3].ToString();
                        string sAddress1 = row[4].ToString();
                        string sAddress2 = row[5].ToString();
                        string sPostalCode = row[6].ToString();
                        string sLocality = row[7].ToString();
                        string sSubLocality = row[8].ToString();
                        string sCountry = row[9].ToString();

                        #region MESSAGES

                        if (sCompanyCode == "" || sCompanyCode == null)
                        {
                            iErroresCodes++;
                        }

                        if (sCode == "" || sCode == null)
                        {
                            AddError(sCompanyCode, _errorTraduccion.FormatError + ": Address Code is empty");
                        }

                        if (sName == "" || sName == null)
                        {
                            AddError(sCompanyCode, _errorTraduccion.FormatError + ": Address Name is empty");
                        }

                        Boolean bDefault = false;
                        if (sDefault != "" && !Boolean.TryParse(sDefault, out bDefault))
                        {
                            AddError(sCompanyCode, _errorTraduccion.FormatError + ": Default is not in the correct format.");
                        }
                        else if (sDefault == "")
                        {
                            AddError(sCompanyCode, _errorTraduccion.FormatError + ": Default is empty");
                        }

                        #endregion

                        AddressDTO oAddress = new AddressDTO()
                        {
                            Address1 = sAddress1,
                            Address2 = sAddress2,
                            PostalCode = sPostalCode,
                            Locality = sLocality,
                            Sublocality = sSubLocality,
                            Country = sCountry
                        };

                        CompanyAddressDTO oCompanyAddress = new CompanyAddressDTO()
                        {
                            Code = sCode,
                            Name = sName,
                            Default = bDefault,
                            Address = oAddress
                        };

                        List<CompanyAddressDTO> listAddress = new List<CompanyAddressDTO>();
                        if (dicAddress.Count == 0 || !dicAddress.ContainsKey(sCompanyCode))
                        {
                            listAddress.Add(oCompanyAddress);
                            dicAddress[sCompanyCode] = listAddress;
                        }
                        else
                        {
                            listAddress = dicAddress[sCompanyCode];
                            listAddress.Add(oCompanyAddress);
                            dicAddress[sCompanyCode] = listAddress;
                        }
                    }
                }

                #endregion

                #region UPLOAD PAYMENT METHODS

                Dictionary<string, List<CompanyAssignedPaymentMethodsDTO>> dicPaymentMethod = new Dictionary<string, List<CompanyAssignedPaymentMethodsDTO>>();

                if (tablePaymentMethod != null)
                {
                    foreach (DataRow row in tablePaymentMethod.Rows)
                    {
                        string sCompanyCode = row[0].ToString();
                        string sPaymentMethodCode = row[1].ToString();
                        string sDefault = row[2].ToString();

                        #region MESSAGES

                        if (sCompanyCode == "" || sCompanyCode == null)
                        {
                            iErroresCodes++;
                        }

                        Boolean bDefault = false;
                        if (sDefault != "" && !Boolean.TryParse(sDefault, out bDefault))
                        {
                            AddError(sCompanyCode, _errorTraduccion.FormatError + ": Default is not in the correct format.");
                        }

                        #endregion

                        CompanyAssignedPaymentMethodsDTO oPaymentMethod = new CompanyAssignedPaymentMethodsDTO()
                        {
                            PaymentMethodCode = sPaymentMethodCode,
                            Default = bDefault
                        };

                        List<CompanyAssignedPaymentMethodsDTO> listPaymentMethod = new List<CompanyAssignedPaymentMethodsDTO>();
                        if (dicPaymentMethod.Count == 0 || !dicPaymentMethod.ContainsKey(sCompanyCode))
                        {
                            listPaymentMethod.Add(oPaymentMethod);
                            dicPaymentMethod[sCompanyCode] = listPaymentMethod;
                        }
                        else
                        {
                            listPaymentMethod = dicPaymentMethod[sCompanyCode];
                            listPaymentMethod.Add(oPaymentMethod);
                            dicPaymentMethod[sCompanyCode] = listPaymentMethod;
                        }
                    }
                }

                #endregion

                #region UPLOAD DATA

                List<CompanyDTO> listCompany = new List<CompanyDTO>();
                List<WaitHandle> waitHandles = new List<WaitHandle>();

                if (tableCompany != null)
                {
                    foreach (DataRow row in tableCompany.Rows)
                    {
                        var handle = new EventWaitHandle(false, EventResetMode.ManualReset);
                        var thread = new Thread(() =>
                        {
                            List<BankAccountDTO> listAccount = new List<BankAccountDTO>();
                            List<CompanyAddressDTO> listAddress = new List<CompanyAddressDTO>();
                            List<CompanyAssignedPaymentMethodsDTO> listPaymentMethod = new List<CompanyAssignedPaymentMethodsDTO>();

                            string sCode = row[0].ToString();
                            string sName = row[1].ToString();
                            string sAlias = row[2].ToString();
                            string sEmail = row[3].ToString();
                            string sPhone = row[4].ToString();
                            string sActive = row[5].ToString();
                            string sOwner = row[6].ToString();
                            string sSupplier = row[7].ToString();
                            string sCustomer = row[8].ToString();
                            string sPayee = row[9].ToString();
                            string sTaxIdentificationNumber = row[10].ToString();
                            string sCompanyTypeCode = row[11].ToString();
                            string sTaxpayerTypeCode = row[12].ToString();
                            string sTaxIdentificationNumberCategoryCode = row[13].ToString();
                            string sPaymentTermCode = row[14].ToString();
                            string sCurrencyCode = row[15].ToString();

                            if (dicBankAccount.ContainsKey(sCode))
                            {
                                listAccount = dicBankAccount[sCode];
                            }

                            if (dicAddress.ContainsKey(sCode))
                            {
                                listAddress = dicAddress[sCode];
                            }

                            if (dicPaymentMethod.ContainsKey(sCode))
                            {
                                listPaymentMethod = dicPaymentMethod[sCode];
                            }

                            #region MESSAGES

                            if (sCode == "" || sCode == null)
                            {
                                iErroresCodes++;
                            }

                            if (sName == "" || sName == null)
                            {
                                AddError(sCode, _errorTraduccion.FormatError + ": Name is empty");
                            }

                            if (sCompanyTypeCode == "" || sCompanyTypeCode == null)
                            {
                                AddError(sCode, _errorTraduccion.FormatError + ": Company Type Code is empty");
                            }

                            if (sCurrencyCode == "" || sCurrencyCode == null)
                            {
                                AddError(sCode, _errorTraduccion.FormatError + ": Currency Code is empty");
                            }

                            if (sTaxIdentificationNumberCategoryCode == "" || sTaxIdentificationNumberCategoryCode == null)
                            {
                                AddError(sCode, _errorTraduccion.FormatError + ": VAT Registration Type Code is empty");
                            }

                            Boolean bActive = false;
                            if (sActive != "" && !Boolean.TryParse(sActive, out bActive))
                            {
                                AddError(sCode, _errorTraduccion.FormatError + ": Active is not in the correct format.");
                            }
                            else if (sActive == "")
                            {
                                AddError(sCode, _errorTraduccion.FormatError + ": Active is empty");
                            }

                            Boolean bOwner = false;
                            if (sOwner != "" && !Boolean.TryParse(sOwner, out bOwner))
                            {
                                AddError(sCode, _errorTraduccion.FormatError + ": Owner is not in the correct format.");
                            }
                            else if (sOwner == "")
                            {
                                AddError(sCode, _errorTraduccion.FormatError + ": Owner is empty");
                            }

                            Boolean bSupplier = false;
                            if (sSupplier != "" && !Boolean.TryParse(sSupplier, out bSupplier))
                            {
                                AddError(sCode, _errorTraduccion.FormatError + ": Supplier is not in the correct format.");
                            }
                            else if (sSupplier == "")
                            {
                                AddError(sCode, _errorTraduccion.FormatError + ": Supplier is empty");
                            }

                            Boolean bCustomer = false;
                            if (sCustomer != "" && !Boolean.TryParse(sCustomer, out bCustomer))
                            {
                                AddError(sCode, _errorTraduccion.FormatError + ": Customer is not in the correct format.");
                            }
                            else if (sCustomer == "")
                            {
                                AddError(sCode, _errorTraduccion.FormatError + ": Customer is empty");
                            }

                            Boolean bPayee = false;
                            if (sPayee != "" && !Boolean.TryParse(sPayee, out bPayee))
                            {
                                AddError(sCode, _errorTraduccion.FormatError + ": Payee is not in the correct format.");
                            }
                            else if (sPayee == "")
                            {
                                AddError(sCode, _errorTraduccion.FormatError + ": Payee is empty");
                            }

                            #endregion

                            CompanyDTO oDTO = new CompanyDTO
                            {
                                Code = sCode,
                                Name = sName,
                                Alias = sAlias,
                                Email = sEmail,
                                Phone = sPhone,
                                Active = bActive,
                                Owner = bOwner,
                                Supplier = bSupplier,
                                Customer = bCustomer,
                                Payee = bPayee,
                                TaxIdentificationNumber = sTaxIdentificationNumber,
                                CompanyTypeCode = sCompanyTypeCode,
                                TaxpayerTypeCode = sTaxpayerTypeCode,
                                TaxIdentificationNumberCategoryCode = sTaxIdentificationNumberCategoryCode,
                                PaymentTermCode = sPaymentTermCode,
                                CurrencyCode = sCurrencyCode,
                                LinkedAddresses = listAddress,
                                LinkedBankAccount = listAccount,
                                LinkedPaymentMethodCode = listPaymentMethod
                            };

                            listCompany.Add(oDTO);
                            handle.Set();
                        });
                        waitHandles.Add(handle);
                        thread.Start();
                    }
                    WaitHandle.WaitAll(waitHandles.ToArray());
                }

                #endregion

                #region ADD DATA

                CompanyDTO Aux;

                foreach (var oCompany in listCompany)
                {
                    if (oCompany.Code != "" && listaErrores[oCompany.Code] == null)
                    {
                        if ((Aux = (from c in fullListProduct where c.Code == oCompany.Code select c).FirstOrDefault()) == null)
                        {
                            var oResult = await cCompany.AddEntity(oCompany);
                            if (oResult.Success)
                            {
                                iCargados++;
                                fullListProduct.Add(oResult.Value);
                            }
                            else
                            {
                                iErrores++;
                                logCarga.EscribeLogs($"Errors With Company {oCompany.Code}");
                                foreach (var Error in oResult.Errors)
                                {
                                    logCarga.EscribeLogs(Error.Message);
                                }
                            }
                        }
                        else
                        {
                            oCompany.Active = Aux.Active;
                            var oResult = await cCompany.UpdateEntity(oCompany.Code, oCompany);
                            if (oResult.Success)
                            {
                                iActualizados++;
                            }
                            else
                            {
                                iErrores++;
                                logCarga.EscribeLogs($"Errors With Company {oCompany.Code}");
                                foreach (var Error in oResult.Errors)
                                {
                                    logCarga.EscribeLogs(Error.Message);
                                }
                            }
                        }
                    }
                    else if (oCompany.Code != "")
                    {
                        iErrores++;
                        logCarga.EscribeLogs($"Errors With Company {oCompany.Code}");
                        foreach (var Error in (List<string>)listaErrores[oCompany.Code])
                        {
                            logCarga.EscribeLogs(Error);
                        }
                    }
                }

                #endregion

                logCarga.EscribeLogs("-------------------------");
                if (tableCompany == null)
                {
                    logCarga.EscribeLogs("TOTAL COMPANIES : 0");
                }
                else
                {
                    logCarga.EscribeLogs("TOTAL COMPANIES" + ": " + tableCompany.Rows.Count);
                }

                logCarga.EscribeLogs("COMPANIES ADDED" + ": " + iCargados.ToString());
                logCarga.EscribeLogs("COMPANIES UPDATED" + ": " + iActualizados.ToString());
                logCarga.EscribeLogs("COMPANIES ERROR" + ": " + iErrores.ToString());
                logCarga.EscribeLogs("COMPANIES CODES EMPTIES" + ": " + iErroresCodes.ToString());
                logCarga.EscribeLogs("END COMPANIES");
                logCarga.EscribeLogs("-------------------------");
                datos.Close();
            }
            catch (Exception ex)
            {
                System.IO.StreamReader datos = new System.IO.StreamReader(sRuta);
                datos.Close();

                _correcto = false;
                log.Error(ex.Message);
            }
            await base.LoadFile(sRuta);
        }
    }
}
