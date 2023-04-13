using CapaNegocio;
using Ext.Net;
using log4net;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TreeCore.APIClient;
using TreeCore.Page;
using TreeCore.Shared.DTO.Companies;
using TreeCore.Shared.DTO.General;
using TreeCore.Shared.ROP;

namespace TreeCore.Modulos.ThirdParty
{
    public partial class EntidadesV2Form : BasePageExtNet
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public List<long> listaFuncionalidades = new List<long>();
        BaseAPIClient<CompanyDTO> ApiClient;
        private CompanyDTO companyDTO;

        #region EVENTOS DE PAGINA

        private void Page_Init(object sender, EventArgs e)
        {
            ApiClient = new BaseAPIClient<CompanyDTO>(TOKEN_API);

            if (!IsPostBack && !RequestManager.IsAjaxRequest)
            {
                listaFuncionalidades = ((List<long>)(this.Session["FUNCIONALIDADES"]));

                ResourceManagerOperaciones(ResourceManagerTreeCore);

                #region REGISTRO DE ESTADISTICAS

                EstadisticasController cEstadisticas = new EstadisticasController();
                cEstadisticas.EscribeEstadisticaAccion(Usuario.UsuarioID, ClienteID, Request.Url.Segments[Request.Url.Segments.Length - 1], true, GetGlobalResource(Comun.CommentEstadisticaPageInit), "strVisitarPagina");
                log.Info(GetGlobalResource(Comun.LogEstadisticasAgregadas));

                #endregion

                if (Request.QueryString["CompanyCode"] != null && Request.QueryString["CompanyCode"] != "")
                {
                    hdCompanyCode.SetValue(Request.QueryString["CompanyCode"]);
                    companyDTO = ApiClient.GetByCode(Request.QueryString["CompanyCode"]).Result.Value;
                }
                else
                {
                    hdCompanyCode.SetValue("");
                    companyDTO = new CompanyDTO();
                    companyDTO.LinkedAddresses = new List<CompanyAddressDTO>();
                    companyDTO.LinkedBankAccount = new List<BankAccountDTO>();
                    companyDTO.LinkedPaymentMethodCode = new List<CompanyAssignedPaymentMethodsDTO>();
                    companyDTO.Active = true;
                }
                hdObjeto.Value = JsonConvert.SerializeObject(companyDTO);

                CargarSecciones();

            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            sPagina = "EntidadesV2.aspx";
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

        #region TIPOS ENTIDAD

        protected void storeTiposEntidad_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                try
                {
                    BaseAPIClient<CompanyTypeDTO> ApiClient = new BaseAPIClient<CompanyTypeDTO>(TOKEN_API);
                    var lista = ApiClient.GetList().Result;
                    if (lista != null)
                    {
                        storeTiposEntidad.DataSource = lista.Value;
                        storeTiposEntidad.DataBind();
                    }
                }

                catch (Exception ex)
                {
                    log.Error(ex.Message);
                }
            }
        }

        #endregion

        #region TIPOS CONTRIBUYENTES

        protected void storeTiposContrubuyentes_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                try
                {
                    BaseAPIClient<TaxpayerTypeDTO> apiCl = new BaseAPIClient<TaxpayerTypeDTO>(TOKEN_API);
                    var lista = apiCl.GetList().Result;
                    if (lista != null)
                    {
                        storeTiposContrubuyentes.DataSource = lista.Value;
                        storeTiposContrubuyentes.DataBind();
                    }
                }

                catch (Exception ex)
                {
                    log.Error(ex.Message);
                }
            }
        }

        #endregion

        #region TIPOS NIF

        protected void storeTiposNIF_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                try
                {
                    BaseAPIClient<TaxIdentificationNumberCategoryDTO> apiCl = new BaseAPIClient<TaxIdentificationNumberCategoryDTO>(TOKEN_API);
                    var lista = apiCl.GetList().Result;
                    if (lista != null)
                    {
                        storeTiposNIF.DataSource = lista.Value;
                        storeTiposNIF.DataBind();
                    }
                }

                catch (Exception ex)
                {
                    log.Error(ex.Message);
                }
            }
        }

        #endregion

        #region CONDICIONES PAGOS

        protected void storeCondicionesPagos_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                try
                {
                    BaseAPIClient<PaymentTermDTO> apiCl = new BaseAPIClient<PaymentTermDTO>(TOKEN_API);
                    var lista = apiCl.GetList().Result;
                    if (lista != null)
                    {
                        storeCondicionesPagos.DataSource = lista.Value;
                        storeCondicionesPagos.DataBind();
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

        #region BANCOS

        protected void storeBancos_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                try
                {
                    BaseAPIClient<BankDTO> ApiClient = new BaseAPIClient<BankDTO>(TOKEN_API);
                    var lista = ApiClient.GetList().Result;
                    if (lista != null)
                    {
                        storeBancos.DataSource = lista.Value;
                        storeBancos.DataBind();
                    }
                }

                catch (Exception ex)
                {
                    log.Error(ex.Message);
                }
            }
        }

        #endregion

        #region PAISES

        protected void storePaises_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                try
                {
                    BaseAPIClient<CountryDTO> ApiClient = new BaseAPIClient<CountryDTO>(TOKEN_API);
                    var lista = ApiClient.GetList().Result;
                    if (lista != null)
                    {
                        storePaises.DataSource = lista.Value;
                        storePaises.DataBind();
                    }
                }

                catch (Exception ex)
                {
                    log.Error(ex.Message);
                }
            }
        }

        #endregion

        #region METODOS DE PAGOS

        protected void storePaymentMethods_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                try
                {
                    BaseAPIClient<PaymentMethodsDTO> ApiClient = new BaseAPIClient<PaymentMethodsDTO>(TOKEN_API);
                    var lista = ApiClient.GetList().Result;
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

        #region ROLES DE COMPANIA

        private class Rol
        {
            public string Name { get; set; }
            public string FieldDTO { get; set; }
        }

        protected void storeRoleCompanies_Refresh(object sender, Ext.Net.StoreReadDataEventArgs e)
        {
            if (RequestManager.IsAjaxRequest)
            {
                try
                {
                    var lista = new List<Rol>();
                    lista.Add(new Rol { Name = GetGlobalResource("strPropietario"),
                        FieldDTO = "Owner"
                    });
                    lista.Add(new Rol { Name = GetGlobalResource("strProveedor"),
                        FieldDTO = "Supplier"
                    });
                    lista.Add(new Rol { Name = GetGlobalResource("strEsCliente"),
                        FieldDTO = "Customer"
                    });
                    lista.Add(new Rol { Name = GetGlobalResource("strBeneficiario"),
                        FieldDTO = "Payee"
                    });
                    storeRoleCompanies.DataSource = lista;
                    storeRoleCompanies.DataBind();

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
        public DirectResponse UploadCompany()
        {
            DirectResponse direct = new DirectResponse
            {
                Result = "",
                Success = true
            };
            try
            {
                ResultDto<CompanyDTO> result;
                companyDTO = JsonConvert.DeserializeObject<CompanyDTO>(hdObjeto.Value.ToString());
                if (hdCompanyCode.Value.ToString() == "")
                {
                    result = ApiClient.AddEntity(companyDTO).Result;
                }
                else
                {
                    result = ApiClient.UpdateEntity(hdCompanyCode.Value.ToString(), companyDTO).Result;
                }
                if (result.Success)
                {
                    if (hdCompanyCode.Value.ToString() == "")
                    {
                        log.Info(GetGlobalResource(Comun.LogAgregacionRealizada));
                    }
                    else
                    {
                        log.Info(GetGlobalResource(Comun.LogActualizacionRealizada));
                    }
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
                    Text = GetGlobalResource("strEntidades"),
                    Expanded = true
                };
                nodes.Add(nodoRaiz);

                #region DATAMODEL

                Node oNodoDataModel = new Node
                {
                    Text = GetGlobalResource("strInfoGeneral"),
                    Expanded = false,
                    Expandable = true,
                    Leaf = true
                };

                oNodoDataModel.CustomAttributes.Add(new ConfigItem("Panel", "formDataModel"));
                oNodoDataModel.CustomAttributes.Add(new ConfigItem("AddressCode", "#None"));
                oNodoDataModel.CustomAttributes.Add(new ConfigItem("BankCode", "#None"));

                nodoRaiz.Children.Add(oNodoDataModel);

                #endregion

                #region FINANCIERO

                Node oNodoFinantial = new Node
                {
                    Text = GetGlobalResource("strInfoFinanciera"),
                    Expanded = false,
                    Expandable = true,
                    Leaf = true
                };

                oNodoFinantial.CustomAttributes.Add(new ConfigItem("Panel", "formFinantial"));
                oNodoFinantial.CustomAttributes.Add(new ConfigItem("LineCode", "#None"));
                oNodoFinantial.CustomAttributes.Add(new ConfigItem("CompanyCode", "#None"));

                nodoRaiz.Children.Add(oNodoFinantial);

                #endregion

                #region ADICIONAL

                Node oNodoAdditional = new Node
                {
                    Text = GetGlobalResource("strInfoAdicional"),
                    Expanded = false,
                    Expandable = true,
                    Leaf = true
                };

                oNodoAdditional.CustomAttributes.Add(new ConfigItem("Panel", "formAdditionalInfo"));
                oNodoAdditional.CustomAttributes.Add(new ConfigItem("LineCode", "#None"));
                oNodoAdditional.CustomAttributes.Add(new ConfigItem("CompanyCode", "#None"));

                //nodoRaiz.Children.Add(oNodoAdditional);

                #endregion

                #region METODOS DE PAGOS

                Node oNodoMetodosPagos = new Node
                {
                    Text = GetGlobalResource("strMetodosPagos"),
                    Expanded = false,
                    Expandable = true,
                    Leaf = true
                };

                oNodoMetodosPagos.CustomAttributes.Add(new ConfigItem("Panel", "formPaymentMethods"));
                oNodoMetodosPagos.CustomAttributes.Add(new ConfigItem("LineCode", "#None"));
                oNodoMetodosPagos.CustomAttributes.Add(new ConfigItem("CompanyCode", "#None"));

                nodoRaiz.Children.Add(oNodoMetodosPagos);

                #endregion

                #region CUENTAS BANCARIAS

                Node oNodoCuentasBancarias;
                Node oNodeCuentaBancaria;

                oNodoCuentasBancarias = new Node
                {
                    Text = GetGlobalResource("strBankAccounts"),
                    Expanded = false,
                    Expandable = true,
                    Leaf = false,
                    NodeID = "nodeCuentasBancarias"
                };

                oNodoCuentasBancarias.CustomAttributes.Add(new ConfigItem("Panel", "formAllBank"));
                oNodoCuentasBancarias.CustomAttributes.Add(new ConfigItem("AddressCode", "#None"));
                oNodoCuentasBancarias.CustomAttributes.Add(new ConfigItem("BankCode", "#None"));

                if (companyDTO.LinkedBankAccount == null || companyDTO.LinkedBankAccount.Count == 0)
                {
                    //oNodeCuentaBancaria = new Node
                    //{
                    //    Text = GetGlobalResource("strBankAccount"),
                    //    Expanded = false,
                    //    Expandable = true,
                    //    Leaf = true
                    //};

                    //oNodeCuentaBancaria.CustomAttributes.Add(new ConfigItem("Panel", "formBank"));
                    //oNodeCuentaBancaria.CustomAttributes.Add(new ConfigItem("AddressCode", "#None"));
                    //oNodeCuentaBancaria.CustomAttributes.Add(new ConfigItem("BankCode", "0"));

                    //oNodoCuentasBancarias.Children.Add(oNodeCuentaBancaria);
                }
                else
                {
                    foreach (var oLine in companyDTO.LinkedBankAccount)
                    {
                        oNodeCuentaBancaria = new Node
                        {
                            Text = oLine.Code,
                            Expanded = false,
                            Expandable = true,
                            Leaf = true
                        };

                        oNodeCuentaBancaria.CustomAttributes.Add(new ConfigItem("Panel", "formBank"));
                        oNodeCuentaBancaria.CustomAttributes.Add(new ConfigItem("AddressCode", "#None"));
                        oNodeCuentaBancaria.CustomAttributes.Add(new ConfigItem("BankCode", oLine.Code));

                        oNodoCuentasBancarias.Children.Add(oNodeCuentaBancaria);
                    }
                }

                nodoRaiz.Children.Add(oNodoCuentasBancarias);

                #endregion

                #region DIRECCIONES

                Node oNodoDirecciones;
                Node oNodeDireccion;

                oNodoDirecciones = new Node
                {
                    Text = GetGlobalResource("strDirecciones"),
                    Expanded = false,
                    Expandable = true,
                    Leaf = false,
                    NodeID = "nodeDirecciones"
                };

                oNodoDirecciones.CustomAttributes.Add(new ConfigItem("Panel", "formAllAddresses"));
                oNodoDirecciones.CustomAttributes.Add(new ConfigItem("AddressCode", "#None"));
                oNodoDirecciones.CustomAttributes.Add(new ConfigItem("BankCode", "#None"));

                if (companyDTO.LinkedAddresses == null || companyDTO.LinkedAddresses.Count == 0)
                {
                    //oNodeDireccion = new Node
                    //{
                    //    Text = GetGlobalResource("strDireccion"),
                    //    Expanded = false,
                    //    Expandable = true,
                    //    Leaf = true
                    //};

                    //oNodeDireccion.CustomAttributes.Add(new ConfigItem("Panel", "formAddresses"));
                    //oNodeDireccion.CustomAttributes.Add(new ConfigItem("AddressCode", "0"));
                    //oNodeDireccion.CustomAttributes.Add(new ConfigItem("BankCode", "#None"));

                    //oNodoDirecciones.Children.Add(oNodeDireccion);
                }
                else
                {
                    foreach (var oLine in companyDTO.LinkedAddresses)
                    {
                        oNodeDireccion = new Node
                        {
                            Text = oLine.Code,
                            Expanded = false,
                            Expandable = true,
                            Leaf = true
                        };

                        oNodeDireccion.CustomAttributes.Add(new ConfigItem("Panel", "formAddresses"));
                        oNodeDireccion.CustomAttributes.Add(new ConfigItem("AddressCode", oLine.Code));
                        oNodeDireccion.CustomAttributes.Add(new ConfigItem("BankCode", "#None"));

                        oNodoDirecciones.Children.Add(oNodeDireccion);
                    }
                }

                nodoRaiz.Children.Add(oNodoDirecciones);

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