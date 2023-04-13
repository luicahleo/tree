using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Xml;
using System.Xml.Serialization;

namespace TreeAPI.DTO.Interfaces
{
    [Serializable]
    [XmlInclude(typeof(TreeObject))]
    public abstract class TreeObject
    {
        public TreeObject()
        {
        }

        public string ToXML()
        {
            try
            {
                using (var stringwriter = new System.IO.StringWriter())
                {
                    var serializer = new XmlSerializer(this.GetType());
                    serializer.Serialize(stringwriter, this);
                    return stringwriter.ToString();
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

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
            string temp = string.Empty;
            try
            {
                foreach (XmlNode xmlNode in node)
                {
                    string propertyName = xmlNode.Name;
                    string propertyValue = xmlNode.InnerXml;

                    temp = "'" + propertyName + "' - '" + propertyValue + "'";

                    PropertyInfo pi = t.GetType().GetProperty(propertyName);

                    if (pi != null)
                    {
                        #region List<string>
                        if (pi.PropertyType.Equals(typeof(List<string>)))
                        {
                            List<string> newList;
                            if (pi.GetValue(t) != null)
                            {
                                newList = (List<string>)pi.GetValue(t);
                            }
                            else
                            {
                                newList = new List<string>();
                            }

                            newList.Add(propertyValue);
                            pi.SetValue(t, newList);
                        }
                        #endregion

                        #region List<InvoicesReceivedLine>
                        /*else if (pi.PropertyType.Equals(typeof(List<InvoicesReceivedLine>)))
                        {
                            XmlNodeList xmlNodeList = xmlNode.SelectNodes("*");
                            List<InvoicesReceivedLine> newList;
                            if (pi.GetValue(t) != null)
                            {
                                newList = (List<InvoicesReceivedLine>)pi.GetValue(t);
                            }
                            else
                            {
                                newList = new List<InvoicesReceivedLine>();
                            }


                            InvoicesReceivedLine invoicesReceivedLines = new InvoicesReceivedLine();
                            foreach (XmlNode subNode in xmlNodeList)
                            {
                                string subNodePropertyName = subNode.Name;
                                string subNodePropertyValue = subNode.InnerXml;

                                PropertyInfo subPi = invoicesReceivedLines.GetType().GetProperty(subNodePropertyName);

                                if (subPi.PropertyType.Equals(typeof(List<string>)))
                                {
                                    List<string> newList2;
                                    if (subPi.GetValue(invoicesReceivedLines) != null)
                                    {
                                        newList2 = (List<string>)subPi.GetValue(invoicesReceivedLines);
                                    }
                                    else
                                    {
                                        newList2 = new List<string>();
                                    }

                                    newList2.Add(subNodePropertyValue);
                                    subPi.SetValue(invoicesReceivedLines, newList2);
                                }
                                else
                                {
                                    subPi.SetValue(invoicesReceivedLines, Convert.ChangeType(subNodePropertyValue, subPi.PropertyType), null);
                                }
                            }

                            newList.Add(invoicesReceivedLines);
                            pi.SetValue(t, newList);
                        }*/
                        #endregion

                        #region List<InvoicesLine>
                        /*else if (pi.PropertyType.Equals(typeof(List<InvoicesLine>)))
                        {
                            XmlNodeList xmlNodeList = xmlNode.SelectNodes("*");
                            List<InvoicesLine> newList;
                            if (pi.GetValue(t) != null)
                            {
                                newList = (List<InvoicesLine>)pi.GetValue(t);
                            }
                            else
                            {
                                newList = new List<InvoicesLine>();
                            }


                            InvoicesLine invoicesLines = new InvoicesLine();
                            foreach (XmlNode subNode in xmlNodeList)
                            {
                                string subNodePropertyName = subNode.Name;
                                string subNodePropertyValue = subNode.InnerXml;

                                PropertyInfo subPi = invoicesLines.GetType().GetProperty(subNodePropertyName);

                                if (subPi.PropertyType.Equals(typeof(List<string>)))
                                {
                                    List<string> newList2;
                                    if (subPi.GetValue(invoicesLines) != null)
                                    {
                                        newList2 = (List<string>)subPi.GetValue(invoicesLines);
                                    }
                                    else
                                    {
                                        newList2 = new List<string>();
                                    }

                                    newList2.Add(subNodePropertyValue);
                                    subPi.SetValue(invoicesLines, newList2);
                                }
                                else
                                {
                                    subPi.SetValue(invoicesLines, Convert.ChangeType(subNodePropertyValue, subPi.PropertyType), null);
                                }
                            }

                            newList.Add(invoicesLines);
                            pi.SetValue(t, newList);
                        }*/
                        #endregion

                        #region List<InventoryAttributes>
                        /*else if (pi.PropertyType.Equals(typeof(List<InventoryAttributes>)))
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
                        }*/
                        #endregion

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
                string error = temp + " --> " + ex.Message;
                t = default(T);
            }

            return t;
        }

        protected bool DateIsValid(string date)
        {
            DateTime d;
            bool dateIsValid = DateTime.TryParseExact(date, Comun.FORMATO_FECHA, CultureInfo.InvariantCulture, DateTimeStyles.None, out d);

            return dateIsValid;
        }
    }
}