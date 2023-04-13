using System;
using System.Xml.Serialization;
using TreeAPI.DTO.Salida.Maintenance;

namespace TreeAPI.DTO.Interfaces
{
    [
        //XmlInclude(typeof(TreeObject)),

        ////Financial
        //XmlInclude(typeof(Invoices)),
        //XmlInclude(typeof(InvoicesDetails)),
        //XmlInclude(typeof(InvoicesReceivedLine)),
        //XmlInclude(typeof(RecivedInvoice)),

        ////Global
        //XmlInclude(typeof(AccessRequest)),
        //XmlInclude(typeof(AcquisitionSARFSite)),
        //XmlInclude(typeof(ConditionAdjustments)),
        //XmlInclude(typeof(ConditionPayment)),
        //XmlInclude(typeof(ConditionRhythms)),
        //XmlInclude(typeof(ConditionsTaxes)),
        //XmlInclude(typeof(Contacts)),
        //XmlInclude(typeof(ContractsConditions)),
        //XmlInclude(typeof(LandLords)),
        //XmlInclude(typeof(LeaseContracts)),
        //XmlInclude(typeof(Payments)),
        //XmlInclude(typeof(PropertyIncidents)),
        //XmlInclude(typeof(Providers)),
        //XmlInclude(typeof(Sites)),
        //XmlInclude(typeof(List<AccessRequest>)),

        ////Install
        //XmlInclude(typeof(InstallCivilWork)),
        //XmlInclude(typeof(InstallTecnical)),

        ////Inventory
        //XmlInclude(typeof(InventoryAttributeConditions)),
        //XmlInclude(typeof(InventoryAttributes)),
        //XmlInclude(typeof(TreeIS.Clases.Inventory.InventoryElements)),
        //XmlInclude(typeof(InventoryStore)),
        //XmlInclude(typeof(List<TreeIS.Clases.Inventory.InventoryElements>)),
        //XmlInclude(typeof(List<InventoryAttributes>)),

        ////Legalizations
        //XmlInclude(typeof(Legalization)),
        //XmlInclude(typeof(List<Legalization>)),

        ////Maintenance
        //XmlInclude(typeof(Maintenance)),
        //XmlInclude(typeof(MaintenanceBudget)),
        //XmlInclude(typeof(MaintenanceCorrectiveTasks)),
        XmlInclude(typeof(MaintenanceIncidence)),
        //XmlInclude(typeof(MaintenanceTasks)),
        XmlInclude(typeof(SiteCorrectiveMaintenance)),
        //XmlInclude(typeof(List<Maintenance>)),
        //XmlInclude(typeof(List<SiteCorrectiveMaintenance>)),

        ////SelfService
        //XmlInclude(typeof(TreeIS.Clases.SelfService.InventoryElement)),
        //XmlInclude(typeof(SelfService)),
        //XmlInclude(typeof(SiteRequest)),
        //XmlInclude(typeof(List<TreeIS.Clases.SelfService.InventoryElement>)),
        //XmlInclude(typeof(List<SiteRequest>)),

        ////SpaceRequest
        //XmlInclude(typeof(SpaceRequest)),
        //XmlInclude(typeof(List<SpaceRequest>))

    ]
    public class TreeDataObject
    {

        #region CONSTRUCTORES

        public TreeDataObject()
        {

        }

        #endregion

        #region PARAMENTROS

        public long lObjectType { get; set; }

        public TreeObject Data { get; set; }

        public string sUser { get; set; }

        public string dCreationDate
        {
            get => CreationDate.ToString();
            set
            {
                CreationDate = DateTime.Parse(value);
            }
        }

        private DateTime CreationDate { get; set; }

        #endregion

        #region Getters and Setters
        public void SetCreationDate(DateTime CreationDate)
        {
            this.CreationDate = CreationDate;
        }

        public DateTime GetCreationDate()
        {
            return CreationDate;
        }
        #endregion

    }
}