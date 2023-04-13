using System.Xml.Serialization;
using TreeAPI.DTO.Interfaces;

namespace TreeAPI.DTO.Salida.Inventory
{
    public class InventoryAttributes : TreeObject
    {

        #region CONSTRUCTOR

        public InventoryAttributes()
        : base()
        {

        }

        #endregion

        #region PARAMETROS

        [XmlElementAttribute(IsNullable = false)]
        public string sElementCode { get; set; }

        [XmlElementAttribute(IsNullable = false)]
        public string sAttributeCode { get; set; }

        [XmlElementAttribute(IsNullable = false)]
        public string sAttributeName { get; set; }

        [XmlElementAttribute(IsNullable = false)]
        public string sAttributeValue { get; set; }

        public string sCategoryAttribute { get; set; }

        [XmlElementAttribute(IsNullable = false)]
        public string sCreator { get; set; }

        [XmlElementAttribute(IsNullable = false)]
        public string dCreationDate { get; set; }

        #endregion
    }
}