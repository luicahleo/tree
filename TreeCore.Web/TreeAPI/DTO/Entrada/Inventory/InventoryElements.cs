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
    public class InventoryElements : TreeObject
    {

        #region CONSTRUCTOR

        public InventoryElements()
        : base()
        {

        }

        #endregion

        #region PARAMETROS

        [XmlIgnore]
        [JsonIgnore]
        public long? lIdentifierLong { get; set; }

        [XmlElementAttribute(IsNullable = true)]
        [JsonIgnore]
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

        [Required]
        public string sElementCode { get; set; }

        /*[XmlElementAttribute(IsNullable = false)]
         public string bActive { get; set; }

         [XmlElementAttribute(IsNullable = false)]
         public string dAcquisitionDate { get; set; }

         [XmlElementAttribute(IsNullable = false)]
         public string dPurchaseOrder { get; set; }*/

        //[XmlElementAttribute(IsNullable = true)]
        //public InventoryElementParent ParentElement { get; set; }

        [Required]
        public string sCategory { get; set; }

        /*[XmlElementAttribute(IsNullable = false)]
        public string sCreator { get; set; } */

        [Required]
        public List<InventoryAttributes> Attributes { get; set; }

        [XmlElementAttribute(IsNullable = true)]
        public List<InventoryTemplates> Templates { get; set; }

        /*[XmlIgnore]
        public string dClientID { get; set; }*/

        [Required]
        public string sSiteCode { get; set; }

        [Required]
        public string sName { get; set; }

        /*[XmlElementAttribute(IsNullable = true)]
        public string sNumberInventory { get; set; }
        */
        [Required]
        public string sStatus { get; set; }

        //[XmlElementAttribute(IsNullable = true)]
        //public bool bTemplate { get; set; }

        [Required]
        public string sCustomer { get; set; }

        #endregion

        #region XML
        public static T FromXmlNodeWithAttributes<T>(XmlNode[] node) where T : new()
        {
            // Local variables
            T t = default(T);

            if (node.Length == 0)
            {
                return t;
            }
            else
            {
                t = new T();
            }
            string temp = null;
            try
            {
                foreach (XmlNode xmlNode in node)
                {
                    string propertyName = xmlNode.Name;
                    string propertyValue = xmlNode.InnerXml;

                    temp = propertyName + " " + propertyValue;

                    PropertyInfo pi = t.GetType().GetProperty(propertyName);

                    if (pi != null)
                    {
                        if (pi.PropertyType.Equals(typeof(List<InventoryAttributes>)))
                        {
                            XmlNodeList xmlNodeList = xmlNode.SelectNodes("*");
                            List<InventoryAttributes> newList;
                            if (pi.GetValue(t) != null)
                            {
                                newList = (List<InventoryAttributes>)pi.GetValue(t);
                            }
                            else
                            {
                                newList = new List<InventoryAttributes>();
                            }


                            InventoryAttributes inventoryAttribute = new InventoryAttributes();
                            foreach (XmlNode subNode in xmlNodeList)
                            {
                                string subNodePropertyName = subNode.Name;
                                string subNodePropertyValue = subNode.InnerXml;

                                PropertyInfo subPi = inventoryAttribute.GetType().GetProperty(subNodePropertyName);
                                subPi.SetValue(inventoryAttribute, Convert.ChangeType(subNodePropertyValue, subPi.PropertyType), null);

                            }
                            newList.Add(inventoryAttribute);
                            pi.SetValue(t, newList);
                        }
                        else
                        {
                            pi.SetValue(t, Convert.ChangeType(propertyValue, pi.PropertyType), null);
                        }
                    }

                    else // search if such field exists
                    {
                        FieldInfo fi = t.GetType().GetField(propertyName);

                        if (fi != null)
                        {
                            fi.SetValue(t, Convert.ChangeType(propertyValue, fi.FieldType));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                string error = temp + " " + ex;
                t = default(T);
            }

            return t;
        }
        #endregion

    }
}