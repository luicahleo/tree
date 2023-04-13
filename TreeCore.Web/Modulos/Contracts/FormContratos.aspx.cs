using CapaNegocio;
using Ext.Net;
using log4net;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using TreeCore.APIClient;
using TreeCore.Page;
using TreeCore.Shared.DTO.Companies;
using TreeCore.Shared.DTO.Contracts;
using TreeCore.Shared.DTO.General;
using TreeCore.Shared.DTO.Query;
using TreeCore.Shared.ROP;

namespace TreeCore.Modulos.Contracts
{
    public partial class FormContratos : BasePageExtNet
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public List<long> listaFuncionalidades = new List<long>();
        BaseAPIClient<ContractDTO> ApiClient;
        private ContractDTO contractDTO;

        #region EVENTOS DE PAGINA

        private void Page_Init(object sender, EventArgs e)
        {
            ApiClient = new BaseAPIClient<ContractDTO>(TOKEN_API);
            if (!IsPostBack && !RequestManager.IsAjaxRequest)
            {
                listaFuncionalidades = ((List<long>)(this.Session["FUNCIONALIDADES"]));

                ResourceManagerOperaciones(ResourceManagerTreeCore);

                #region REGISTRO DE ESTADISTICAS

                EstadisticasController cEstadisticas = new EstadisticasController();
                cEstadisticas.EscribeEstadisticaAccion(Usuario.UsuarioID, ClienteID, Request.Url.Segments[Request.Url.Segments.Length - 1], true, GetGlobalResource(Comun.CommentEstadisticaPageInit), "strVisitarPagina");
                log.Info(GetGlobalResource(Comun.LogEstadisticasAgregadas));

                #endregion

                if (Request.QueryString["ContractCode"] != null && Request.QueryString["ContractCode"] != "")
                {
                    hdContractCode.SetValue(Request.QueryString["ContractCode"]);
                    contractDTO = ApiClient.GetByCode(Request.QueryString["ContractCode"]).Result.Value;
                }
                else
                {
                    hdContractCode.SetValue("");
                    contractDTO = new ContractDTO();
                    contractDTO.contractline = new List<ContractLineDTO>();
                    if (Request.QueryString["SiteCode"] != null && Request.QueryString["SiteCode"] != "")
                    {
                        contractDTO.SiteCode = Request.QueryString["SiteCode"];
                    }
                    else
                    {
                        contractDTO.SiteCode = "";
                    }
                }
                hdObjeto.Value = JsonConvert.SerializeObject(contractDTO);

                CargarSecciones();

            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            sPagina = "Contratos.aspx";
            funtionalities = new System.Collections.Hashtable() {
            { "Read", new List<ComponentBase> { } },
            { "Download", new List<ComponentBase> { }},
            { "Post", new List<ComponentBase> { }},
            { "Put", new List<ComponentBase> { }},
            { "Delete", new List<ComponentBase> { }}
        };
        }

        #endregion

        #region STORES

        #region ESTADOS

        protected void storeEstados_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                try
                {
                    BaseAPIClient<ContractStatusDTO> apiCl = new BaseAPIClient<ContractStatusDTO>(TOKEN_API);
                    var lista = apiCl.GetList().Result;
                    if (lista != null)
                    {
                        storeEstados.DataSource = lista.Value;
                        storeEstados.DataBind();
                    }
                }

                catch (Exception ex)
                {
                    log.Error(ex.Message);
                }
            }
        }

        #endregion

        #region TIPOS

        protected void storeTipos_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                try
                {
                    BaseAPIClient<ContractTypeDTO> apiCl = new BaseAPIClient<ContractTypeDTO>(TOKEN_API);
                    var lista = apiCl.GetList().Result;
                    if (lista != null)
                    {
                        storeTipos.DataSource = lista.Value;
                        storeTipos.DataBind();
                    }
                }

                catch (Exception ex)
                {
                    log.Error(ex.Message);
                }
            }
        }

        #endregion

        #region GRUPOS

        protected void storeGrupos_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                try
                {
                    BaseAPIClient<ContractGroupDTO> apiCl = new BaseAPIClient<ContractGroupDTO>(TOKEN_API);
                    var lista = apiCl.GetList().Result;
                    if (lista != null)
                    {
                        storeGrupos.DataSource = lista.Value;
                        storeGrupos.DataBind();
                    }
                }

                catch (Exception ex)
                {
                    log.Error(ex.Message);
                }
            }
        }

        #endregion

        #region MONEDAS

        protected void storeMonedas_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                try
                {
                    BaseAPIClient<CurrencyDTO> apiCl = new BaseAPIClient<CurrencyDTO>(TOKEN_API);
                    var lista = apiCl.GetList().Result;
                    if (lista != null)
                    {
                        storeMonedas.DataSource = lista.Value;
                        storeMonedas.DataBind();
                    }
                }

                catch (Exception ex)
                {
                    log.Error(ex.Message);
                }
            }
        }

        #endregion

        #region COMPANY

        protected void storeCompany_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                try
                {
                    BaseAPIClient<CompanyDTO> apiCl = new BaseAPIClient<CompanyDTO>(TOKEN_API);
                    var lista = apiCl.GetList().Result;
                    if (lista != null)
                    {
                        storeCompany.DataSource = lista.Value.Where(x => x.LinkedBankAccount != null && x.LinkedBankAccount.Count > 0 && x.LinkedPaymentMethodCode != null && x.LinkedPaymentMethodCode.Count > 0);
                        storeCompany.DataBind();
                    }
                }

                catch (Exception ex)
                {
                    log.Error(ex.Message);
                }
            }
        }

        #endregion

        #region LINE TYPE

        protected void storeLineType_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                try
                {
                    BaseAPIClient<ContractLineTypeDTO> apiCl = new BaseAPIClient<ContractLineTypeDTO>(TOKEN_API);
                    var lista = apiCl.GetList().Result;
                    if (lista != null)
                    {
                        storeLineType.DataSource = lista.Value;
                        storeLineType.DataBind();
                    }
                }

                catch (Exception ex)
                {
                    log.Error(ex.Message);
                }
            }
        }

        #endregion

        #region TAXES

        protected void storeTaxes_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                try
                {
                    BaseAPIClient<TaxesDTO> apiCl = new BaseAPIClient<TaxesDTO>(TOKEN_API);
                    var lista = apiCl.GetList().Result;
                    if (lista != null)
                    {
                        storeTaxes.DataSource = lista.Value;
                        storeTaxes.DataBind();
                    }
                }

                catch (Exception ex)
                {
                    log.Error(ex.Message);
                }
            }
        }

        #endregion

        #region INFLATION

        protected void storeInflation_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                try
                {
                    BaseAPIClient<InflationDTO> apiCl = new BaseAPIClient<InflationDTO>(TOKEN_API);
                    var lista = apiCl.GetList().Result;
                    if (lista != null)
                    {
                        storeInflation.DataSource = lista.Value;
                        storeInflation.DataBind();
                    }
                }

                catch (Exception ex)
                {
                    log.Error(ex.Message);
                }
            }
        }

        #endregion
        
        #region PAYMENT METHODS

        protected void storePaymentMethods_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                try
                {
                    BaseAPIClient<PaymentMethodsDTO> apiCl = new BaseAPIClient<PaymentMethodsDTO>(TOKEN_API);
                    var lista = apiCl.GetList().Result;
                    if (lista != null)
                    {
                        storePaymentMethods.DataSource = lista.Value;
                        storePaymentMethods.DataBind();
                    }
                }

                catch (Exception ex)
                {
                    log.Error(ex.Message);
                }
            }
        }

        #endregion

        #region BANK ACCOUNTS

        protected void storeBankAccounts_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                try
                {
                    storeBankAccounts.DataSource = new List<BankAccountDTO>();
                    storeBankAccounts.DataBind();
                }

                catch (Exception ex)
                {
                    log.Error(ex.Message);
                }
            }
        }

        #endregion

        #endregion

        #region DIRECT METHOD

        [DirectMethod]
        public DirectResponse UploadContract()
        {
            DirectResponse direct = new DirectResponse
            {
                Result = "",
                Success = true
            };
            try
            {
                ResultDto<ContractDTO> result;
                contractDTO = JsonConvert.DeserializeObject<ContractDTO>(hdObjeto.Value.ToString());
                //contractDTO.Duration = ((contractDTO.FirstEndDate.Year - contractDTO.StartDate.Year) * 12) + contractDTO.FirstEndDate.Month - contractDTO.StartDate.Month;
                if (hdContractCode.Value.ToString() == "")
                {
                    result = ApiClient.AddEntity(contractDTO).Result;
                }
                else
                {
                    result = ApiClient.UpdateEntity(hdContractCode.Value.ToString(), contractDTO).Result;
                }
                if (result.Success)
                {
                    if (hdContractCode.Value.ToString() == "")
                    {
                        log.Info(GetGlobalResource(Comun.LogAgregacionRealizada));
                    }
                    else
                    {
                        log.Info(GetGlobalResource(Comun.LogActualizacionRealizada));
                    }
                    hdContractCode.SetValue(result.Value.Code);
                }
                else
                {
                    direct.Success = false;
                    direct.Result = result.Errors[0].Message;
                    return direct;
                }
            }
            catch (Exception ex)
            {
                direct.Success = false;
                direct.Result = GetGlobalResource(Comun.strMensajeGenerico);
                log.Error(ex.Message);
            }
            return direct;
        }

        #endregion

        #region FUNCTIONS

        public void CargarSecciones()
        {
            try
            {
                NodeCollection nodes = new NodeCollection();
                Node nodoRaiz = new Node
                {
                    Text = GetGlobalResource("strContracts"),
                    Expanded = true
                };
                nodes.Add(nodoRaiz);


                #region DATAMODEL

                Node oNodoDataModel = new Node
                {
                    Text = GetGlobalResource("jsModeloDatos"),
                    Expanded = false,
                    Expandable = true,
                    Leaf = true,
                    NodeID = $"NodDataModel"
                };

                oNodoDataModel.CustomAttributes.Add(new ConfigItem("Panel", "formDataModel"));
                oNodoDataModel.CustomAttributes.Add(new ConfigItem("LineCode", "#None"));
                oNodoDataModel.CustomAttributes.Add(new ConfigItem("CompanyCode", "#None"));
                oNodoDataModel.CustomAttributes.Add(new ConfigItem("OriginalCode", "#None"));

                nodoRaiz.Children.Add(oNodoDataModel);

                #endregion

                #region RENEWAL

                Node oNodoRenewal = new Node
                {
                    Text = GetGlobalResource("strProrroga"),
                    Expanded = false,
                    Expandable = true,
                    Leaf = true,
                    NodeID = $"NodRenewal"
                };

                oNodoRenewal.CustomAttributes.Add(new ConfigItem("Panel", "formRenewal"));
                oNodoRenewal.CustomAttributes.Add(new ConfigItem("LineCode", "#None"));
                oNodoRenewal.CustomAttributes.Add(new ConfigItem("CompanyCode", "#None"));
                oNodoRenewal.CustomAttributes.Add(new ConfigItem("OriginalCode", "#None"));

                nodoRaiz.Children.Add(oNodoRenewal);

                #endregion

                #region LINES

                Node oNodoLines;
                Node oNodoLine;
                Node oNodeLinePriceReajusment;
                Node oNodeLineCompanies;
                Node oNodeLineCompany;
                Node oNodeLineTaxes;

                oNodoLines = new Node
                {
                    Text = GetGlobalResource("strLines"),
                    Expanded = false,
                    Expandable = true,
                    Leaf = false
                };

                oNodoLines.CustomAttributes.Add(new ConfigItem("Panel", "formLines"));
                oNodoLines.CustomAttributes.Add(new ConfigItem("LineCode", "#None"));
                oNodoLines.CustomAttributes.Add(new ConfigItem("CompanyCode", "#None"));
                oNodoLines.CustomAttributes.Add(new ConfigItem("OriginalCode", "#None"));

                nodoRaiz.Children.Add(oNodoLines);

                if (contractDTO.contractline != null && contractDTO.contractline.Count > 0)
                {
                    foreach (var oLine in contractDTO.contractline)
                    {
                        oNodoLine = new Node
                        {
                            Text = oLine.Code,
                            Expanded = false,
                            Expandable = true,
                            Leaf = false,
                            NodeID = $"NodLine{oLine.Code}"
                        };

                        oNodoLine.CustomAttributes.Add(new ConfigItem("Panel", "formLine"));
                        oNodoLine.CustomAttributes.Add(new ConfigItem("LineCode", oLine.Code));
                        oNodoLine.CustomAttributes.Add(new ConfigItem("CompanyCode", "#None"));
                        oNodoLine.CustomAttributes.Add(new ConfigItem("OriginalCode", oLine.Code));

                        oNodeLinePriceReajusment = new Node
                        {
                            Text = GetGlobalResource("strReajustes"),
                            Expanded = false,
                            Expandable = false,
                            Leaf = true,
                            NodeID = $"NodLinePriceReadjustment{oLine.Code}"
                        };

                        oNodeLinePriceReajusment.CustomAttributes.Add(new ConfigItem("Panel", "formLinePriceReadjustment"));
                        oNodeLinePriceReajusment.CustomAttributes.Add(new ConfigItem("LineCode", oLine.Code));
                        oNodeLinePriceReajusment.CustomAttributes.Add(new ConfigItem("CompanyCode", "#None"));
                        oNodeLinePriceReajusment.CustomAttributes.Add(new ConfigItem("OriginalCode", "#None"));

                        oNodoLine.Children.Add(oNodeLinePriceReajusment);

                        oNodeLineCompanies = new Node
                        {
                            Text = GetGlobalResource("strBeneficiarios"),
                            Expanded = false,
                            Expandable = true,
                            Leaf = false
                        };

                        oNodeLineCompanies.CustomAttributes.Add(new ConfigItem("Panel", "formCompanies"));
                        oNodeLineCompanies.CustomAttributes.Add(new ConfigItem("LineCode", oLine.Code));
                        oNodeLineCompanies.CustomAttributes.Add(new ConfigItem("CompanyCode", "#None"));
                        oNodeLineCompanies.CustomAttributes.Add(new ConfigItem("OriginalCode", "#None"));

                        oNodoLine.Children.Add(oNodeLineCompanies);

                        if (oLine.Payees != null)
                        {
                            foreach (var oCompany in oLine.Payees)
                            {
                                oNodeLineCompany = new Node
                                {
                                    Text = oCompany.CompanyCode,
                                    Expanded = true,
                                    Expandable = false,
                                    Leaf = true,
                                    NodeID = $"NodLineCompany{oLine.Code}{oCompany.CompanyCode}"
                                };

                                oNodeLineCompany.CustomAttributes.Add(new ConfigItem("Panel", "formLineCompany"));
                                oNodeLineCompany.CustomAttributes.Add(new ConfigItem("LineCode", oLine.Code));
                                oNodeLineCompany.CustomAttributes.Add(new ConfigItem("CompanyCode", oCompany.CompanyCode));
                                oNodeLineCompany.CustomAttributes.Add(new ConfigItem("OriginalCode", oCompany.CompanyCode));

                                oNodeLineCompanies.Children.Add(oNodeLineCompany);
                            }
                        }

                        oNodeLineTaxes = new Node
                        {
                            Text = GetGlobalResource("strImpuestos"),
                            Expanded = false,
                            Expandable = false,
                            Leaf = true,
                            NodeID = $"NodLineTaxes{oLine.Code}"
                        };

                        oNodeLineTaxes.CustomAttributes.Add(new ConfigItem("Panel", "formLineTaxes"));
                        oNodeLineTaxes.CustomAttributes.Add(new ConfigItem("LineCode", oLine.Code));
                        oNodeLineTaxes.CustomAttributes.Add(new ConfigItem("CompanyCode", "#None"));
                        oNodeLineTaxes.CustomAttributes.Add(new ConfigItem("OriginalCode", "#None"));

                        oNodoLine.Children.Add(oNodeLineTaxes);

                        oNodoLines.Children.Add(oNodoLine);
                    }
                }

                #endregion

                gridSecciones.SetRootNode(nodoRaiz);

            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }
        }

        #endregion

    }
}