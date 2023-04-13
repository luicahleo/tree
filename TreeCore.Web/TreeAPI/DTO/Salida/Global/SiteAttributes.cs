using TreeAPI.DTO.Interfaces;

namespace TreeAPI.DTO.Salida.Global
{
    public class SiteAttributes : TreeObject
    {

        #region CONSTRUCTOR

        public SiteAttributes()
        : base()
        {

        }

        #endregion

        #region PARAMETROS

        public string sSiteCode { get; set; }

        public string sAttributeCode { get; set; }

        public string sAttributeName { get; set; }
        
        public string sAttributeValue { get; set; }

        public string sCategoryAttribute { get; set; }

        #endregion

    }
}