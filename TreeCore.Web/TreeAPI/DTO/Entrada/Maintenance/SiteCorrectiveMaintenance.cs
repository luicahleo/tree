using Newtonsoft.Json;
using System;
using TreeAPI.DTO.Interfaces;

namespace TreeAPI.DTO.Entrada.Maintenance
{
    public class SiteCorrectiveMaintenance : TreeObject
    {

        #region CONSTRUCTOR

        public SiteCorrectiveMaintenance()
        : base()
        {
        }

        #endregion

        #region PARAMENTROS

        //[XmlIgnore]
        [JsonIgnore]
        public long? lIdentifierLong { get; set; }

        //[XmlElementAttribute(IsNullable = true)]
        public string lIdentifier
        {
            get
            {
                return (lIdentifierLong.HasValue) ? lIdentifierLong.ToString() : string.Empty;
            }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    this.lIdentifierLong = long.Parse(value);
                }
            }
        }

        //[XmlElementAttribute(IsNullable = false)]
        public string sSiteCode { get; set; }

        //[XmlElementAttribute(IsNullable = false)]
        public string sProject { get; set; }

        //[XmlElementAttribute(IsNullable = false)]
        public string sConflictLevel { get; set; }

        //[XmlElementAttribute(IsNullable = false)]
        public string sTypology { get; set; }

        //[XmlElementAttribute(IsNullable = false)]
        public bool bActive { get; set; }

        //[XmlElementAttribute(IsNullable = true)]
        public string sMaintenanceIncident { get; set; }

        //[XmlElementAttribute(IsNullable = true)]
        public string sAgency { get; set; }

        //[XmlElementAttribute(IsNullable = true)]
        public string sUserEmail { get; set; }
        
        //[XmlElementAttribute(IsNullable = true)]
        public string sSeverity { get; set; }

        #endregion

    }
}