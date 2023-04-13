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
    public class InventoryTemplates : TreeObject
    {

        #region CONSTRUCTOR

        public InventoryTemplates()
        : base()
        {

        }

        #endregion

        #region PARAMETROS

        [Required]
        public string sTemplateName { get; set; }

        [Required]
        public string sAttributeCategory { get; set; }

        #endregion
    }
}