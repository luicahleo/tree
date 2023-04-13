namespace TreeCore.BackEnd.Data.Repository.Query
{
    internal class TableNames
    {
        public static readonly Table Bank = new("Bancos", "CodigoBanco", "BancoID");
        public static readonly Table BankAccount = new("EntidadesCuentasBancarias", "Codigo", "EntidadCuentaBancariaID");
        public static readonly Table BusinessProcess = new("CoreBusinessProcess", "Codigo", "CoreBusinessProcessID");
        public static readonly Table BusinessProcessAssignedWorkflows = new("CoreBusinessProcessWorkflows", "", "CoreBusinessProcessWorkflowID");
        public static readonly Table BusinessProcessType = new("CoreBusinessProcessTipos", "Codigo", "CoreBusinessProcessTipoID");
        public static readonly Table Catalog = new("CoreProductCatalogs", "Codigo", "CoreProductCatalogID");
        public static readonly Table CatalogAssignedCompanies = new("CoreProductCatalogsEntidades", "", "CoreProductCatalogEntidadID");
        public static readonly Table CatalogAssignedProducts = new("CoreProductCatalogServiciosAsignados", "", "CoreProductCatalogServicioAsignadoID");
        public static readonly Table CatalogLifecycleStatus = new("CoreProductCatalogEstadosGlobales", "Codigo", "CoreProductCatalogEstadoGlobalID");
        public static readonly Table CatalogType = new("CoreProductCatalogTipos", "Codigo", "CoreProductCatalogTipoID");
        public static readonly Table Company = new("Entidades", "Codigo", "EntidadID");
        public static readonly Table CompanyAddress = new("EntidadesDirecciones", "Codigo", "EntidadDireccionID");
        public static readonly Table CompanyPaymentMethods = new("EntidadesMetodosPagos", "", "EntidadMetodoPagoID");
        public static readonly Table CompanyType = new("EntidadesTipos", "Codigo", "EntidadTipoID");
        public static readonly Table Contract = new("Alquileres", "NumContrato", "AlquilerID");
        public static readonly Table ContractGroup = new("AlquileresTiposContrataciones", "Codigo", "AlquilerTipoContratacionID");
        public static readonly Table ContractHistroy = new("AlquileresHistoricosCompletos", "AlquilerID", "AlquilerHistoricoCompletoID");
        public static readonly Table ContractLine = new("AlquileresDetalles", "CodigoDetalle","AlquilerDetalleID");
        public static readonly Table ContractLineEntidades = new("AlquileresDetallesEntidades", "Codigo", "AlquilerDetalleEntidadID");
        public static readonly Table ContractLineTaxes = new("AlquileresDetallesImpuestos", "Codigo", "AlquilerDetalleImpuestoID");
        public static readonly Table ContractLineType = new("AlquileresConceptos", "Codigo","AlquilerConceptoID");
        public static readonly Table ContractState = new("AlquileresEstados", "codigo", "AlquilerEstadoID");
        public static readonly Table ContractType = new("AlquileresTiposContratos", "Codigo", "AlquilerTipoContratoID");
        public static readonly Table Country = new("Paises", "Pais", "PaisID");
        public static readonly Table Cron = new("CoreServiciosFrecuencias", "Nombre", "CoreServicioFrecuenciaID");
        public static readonly Table Currency = new("Monedas", "Moneda", "MonedaID");
        public static readonly Table FunctionalArea = new("AreasFuncionales", "Codigo");
        public static readonly Table ImportTask = new("DocumentosCargas", "DocumentoCarga", "DocumentoCargaID");
        public static readonly Table ImportType = new("DocumentosCargasPlantillas", "DocumentoCargaPlantilla", "DocumentoCargaPlantillaID");
        public static readonly Table Inflation = new("Inflaciones", "Codigo", "InflacionID"); 		
        public static readonly Table Inventory = new("InventarioElementos", "NumeroInventario", "InventarioElementoID"); 		
        public static readonly Table PaymentMethods = new("MetodosPagos", "CodigoMetodoPago", "MetodoPagoID");
        public static readonly Table PaymentTerm = new("CondicionesPagos", "Codigo", "CondicionPagoID");
        public static readonly Table Product = new("CoreProductCatalogServicios", "Codigo", "CoreProductCatalogServicioID");
        public static readonly Table ProductEntityLinked = new("CoreProductCatalogServiciosEntidades", "", "CoreProductCatalogServicioEntidadID");
        public static readonly Table ProductProductLinked = new("CoreProductCatalogServiciosServiciosAsignados", "", "CoreProductCatalogServiciosServicioAsignadoID");
        public static readonly Table ProductType = new("CoreProductCatalogServiciosTipos", "Codigo", "CoreProductCatalogServicioTipoID");
        public static readonly Table Profile = new("Perfiles", "Perfil_esES", "PerfilID");
        public static readonly Table Project = new("CoreProjects", "Codigo", "CoreProjectID");
        public static readonly Table ProjectLifeCycleStatus = new("CoreProjectLifeCycleStatus", "Codigo", "CoreProjectLifeCycleStatusID");
        public static readonly Table Program = new("CorePrograms", "Codigo", "CoreProgramID");
        public static readonly Table Rol = new("Roles", "Codigo", "RolID");
        public static readonly Table RolProfiles = new("RolesPerfiles", "", "RolPerfilID");
        public static readonly Table Sites = new("Emplazamientos", "Codigo", "EmplazamientoID");
        public static readonly Table StatusRolReading = new("CoreEstadosRolesLectura", "", "CoreEstadoRolLecturaID");
        public static readonly Table StatusRolWriting = new("CoreEstadosRolesEscrituras", "", "CoreEstadoRolEscrituraID");
        public static readonly Table Taxes = new("Impuestos", "Codigo", "ImpuestoID");
        public static readonly Table TaxIdentificationNumberCategory = new("SAPTiposNIF", "Codigo", "SAPTipoNIFID");
        public static readonly Table TaxpayerType = new("TiposContribuyentes", "Codigo", "TipoContribuyenteID");
        public static readonly Table User = new("Usuarios", "EMail", "UsuarioID");
        public static readonly Table View = new("CoreGestionVistas", "Nombre", "CoreGestionVistaID");
		public static readonly Table Workflow = new("CoreWorkflows", "Codigo", "CoreWorkFlowID");
        public static readonly Table WorkflowAssignedRoles = new("CoreWorkflowsRoles", "", "CoreWorkFlowRolID");
        public static readonly Table WorkFlowNextStatus = new("CoreEstadosSiguientes", "", "CoreEstadoSiguienteID");
        public static readonly Table WorkFlowStatus = new("CoreEstados", "Codigo", "CoreEstadoID");
        public static readonly Table WorkFlowStatusGroup = new("EstadosAgrupaciones", "Codigo", "EstadoAgrupacionID");
        public static readonly Table WorkOrders = new("CoreWorkOrders", "Codigo", "CoreWorkOrderID");
        public static readonly Table WorkOrderLifecycleStatus = new("CoreWorkOrderLifeCycleStatus", "Codigo", "CoreWorkOrderLifeCycleStatusID");
        public static readonly Table WorkOrderTrackingStatus = new("CoreWorkOrderTrackingStatus", "Codigo", "CoreWorkOrderTrackingStatusID");


    }
}
