using System;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TreeCore.Shared.DTO.General;
using TreeCore.Shared.DTO.ValueObject;
using Swashbuckle.AspNetCore.Annotations;

namespace TreeCore.Shared.DTO.Contracts
{
    public class ContractDTO : BaseDTO
    {

        [Required] public string Code { get; set; }

        [Required] public string Name { get; set; }

        [Required] public string Description { get; set; }

        [Required] public string SiteCode { get; set; }

        [Required] public string ContractStatusCode { get; set; }

        [Required] public string CurrencyCode { get; set; }

        [Required] public string ContractGroupCode { get; set; }

        [Required] public string ContractTypeCode { get; set; }

        public string MasterContractNumber { get; set; }

        [Required] public DateTime ExecutionDate { get; set; }

        [Required] public DateTime StartDate { get; set; }

        [Required] public DateTime FirstEndDate { get; set; }

        //public int Duration { get; set; }

        [Required] public bool ClosedAtExpiration { get; set; }

        [Editable(false)]
        [DataType(DataType.Date)]
        public DateTime? CreationDate { get; set; }

        [Editable(false)]
        [DataType(DataType.Date)]
        public DateTime? LastModificationDate { get; set; }

        [Editable(false)]
        public string CreationUserEmail { get; set; }

        [Editable(false)]
        public string LastModificationUserEmail { get; set; }


        public RenewalClauseDTO RenewalClause { get; set; }

        public List<ContractLineDTO> contractline { get; set; }

        public ContractDTO()
        {
            map = new Dictionary<string, string>();
            map.Add(nameof(Code).ToLower(), "numcontrato");
            map.Add(nameof(Name).ToLower(), "nombrecontrtato");
            map.Add(nameof(ContractStatusCode).ToLower(), "alquilerestadoid");
            map.Add(nameof(SiteCode).ToLower(), "emplazamientoid");
            map.Add(nameof(CurrencyCode).ToLower(), "monedaid");
            map.Add(nameof(ContractGroupCode).ToLower(), "alquilerTipocontratacionid");
            map.Add(nameof(ContractTypeCode).ToLower(), "alquilertipocontratoid");
            map.Add(nameof(Description).ToLower(), "comentariosgenerales");
            map.Add(nameof(MasterContractNumber).ToLower(), "codigocontratomarco");
            map.Add(nameof(ExecutionDate).ToLower(), "fechafirmacontrato");
            map.Add(nameof(StartDate).ToLower(), "fechainiciocontrato");
            map.Add(nameof(FirstEndDate).ToLower(), "fechaFincontrato");
           // map.Add(nameof(Duration).ToLower(), "cadencias");
            map.Add(nameof(ClosedAtExpiration).ToLower(), "FechaVencimientoActual");
            map.Add(nameof(RenewalClause).ToLower(), "ajusteRenovaciones");

        }
    }
}
