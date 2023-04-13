using TreeAPI.DTO.Interfaces;

namespace TreeAPI.DTO.Entrada.Global
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


        public string sAttributeCode { get; set; }

        public string sAttributeValue { get; set; }

        public string sAttributeCategory { get; set; }

        #endregion

    }
}