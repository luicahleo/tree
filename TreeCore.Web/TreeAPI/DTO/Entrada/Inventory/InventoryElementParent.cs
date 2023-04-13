using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;
using TreeAPI.DTO.Interfaces;

namespace TreeAPI.DTO.Entrada.Inventory
{
    public class InventoryElementParent : TreeObject
    {

        #region CONSTRUCTOR

        public InventoryElementParent()
        : base()
        {

        }

        #endregion

        #region PARAMETROS

        [XmlElementAttribute(IsNullable = false)]
        public string sNumberInventory { get; set; }

        [XmlElementAttribute(IsNullable = false)]
        public string ElementType { get; set; }

        #endregion

    }
}