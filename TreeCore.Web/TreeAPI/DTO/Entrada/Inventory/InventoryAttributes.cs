using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Xml;
using System.Xml.Serialization;
using TreeAPI.DTO.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace TreeAPI.DTO.Entrada.Inventory
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
        public string sAttributeName { get; set; }

        [XmlElementAttribute(IsNullable = false)]
        public string sAttributeValue { get; set; }

        #endregion
    }
}