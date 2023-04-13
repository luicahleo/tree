using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TreeCore.Shared.DTO.ValueObject;

namespace TreeCore.Shared.DTO.ProductCatalog
{
    public class CatalogDTO : BaseDTO
    {
        [Required] public string Code { get; set; }
        [Required] public string Name { get; set; }
        [Required] public string Description { get; set; }
        [Required] public string CatalogTypeCode { get; set; }
        [Required] public string CurrencyCode { get; set; }
        [Required] public string LifecycleStatusCode { get; set; }
        [Required] public string SupplierCompanyCode { get; set; }
        [Required][DataType(DataType.Date)] public DateTime StartDate { get; set; }
        [Required][DataType(DataType.Date)] public DateTime EndDate { get; set; }
        public PriceReadjustmentDTO PricesReadjustment { get ; set ; }
        public List<CatalogAssignedProductsDTO> LinkedProducts { get; set; }
        public List<string> LinkedCompanies { get; set; }
        [Editable(false)] [DataType(DataType.Date)] public DateTime? CreationDate { get; set; }
        [Editable(false)] [DataType(DataType.Date)] public DateTime? LastModificationDate { get; set; }
        [Editable(false)] public string CreationUserEmail { get; set; }
        [Editable(false)] public string LastModificationUserEmail { get; set; }

        public CatalogDTO()
        {
            map = new Dictionary<string, string>();
            map.Add(nameof(Code).ToLower(), "codigo");
            map.Add(nameof(Name).ToLower(), "nombre");
            map.Add(nameof(Description).ToLower(), "descripcion");
            map.Add(nameof(CatalogTypeCode).ToLower(), "codigo");
            map.Add(nameof(CurrencyCode).ToLower(), "monedaid");
            map.Add(nameof(LifecycleStatusCode).ToLower(), "coreproductcatalogestadoglobalid");
            map.Add(nameof(SupplierCompanyCode).ToLower(), "entidadid");
            map.Add(nameof(StartDate).ToLower(), "fechainiciovigencia");
            map.Add(nameof(EndDate).ToLower(), "fechafinvigencia");
            map.Add(nameof(PricesReadjustment).ToLower(), "reajustePrecios");
            map.Add(nameof(LinkedProducts).ToLower(), "productosAsignados");
            map.Add(nameof(LinkedCompanies).ToLower(), "entidadesAsignados");
            map.Add(nameof(CreationDate).ToLower(), "fechacreacion");
            map.Add(nameof(LastModificationDate).ToLower(), "fechaultimamodificacion");
            map.Add(nameof(CreationUserEmail).ToLower(), "usuariocreadorid");
            map.Add(nameof(LastModificationUserEmail).ToLower(), "usuariomodificadorid");
        }
    }
}