using TreeCore.Clases;

namespace TreeAPI.API.TBO.Interfaces
{
    /*
     * Listado de clases que se asignan a la propiedad Data del objeto TreeDataObject. 
     * Si La clase no aparece en esta lista seguramente falle al serializar
     */
    /*[
        XmlInclude(typeof(List<TreeIS.Clases.Global.Sites>)),
        XmlInclude(typeof(List<MaintenanceCorrectiveTasks>)),
        XmlInclude(typeof(List<MaintenanceTasks>)),
        XmlInclude(typeof(List<TreeDataObject>)),
        XmlInclude(typeof(List<TreeIS.Clases.Financial.Invoices>)),
        XmlInclude(typeof(List<Legalization>)),
        XmlInclude(typeof(List<TreeIS.Clases.SelfService.Site>))

    ]*/
    public class TBOResponse : InfoResponse
    {
        public TBOResponse() : base() { }
    }
}