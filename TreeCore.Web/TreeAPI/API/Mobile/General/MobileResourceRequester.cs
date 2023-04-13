using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Resources;

namespace TreeAPI.API.Mobile.General
{
    /// <summary>
    /// Resource helper class for the API class
    /// </summary>
    public class MobileResourceRequester
    {
        // default
        private System.Resources.ResourceManager ResManager = null;
        private string Locale = "es-ES";

        // default constructors
        public MobileResourceRequester()
        {
        }

        public MobileResourceRequester(ResourceManager rm, string locale)
        {
            ResManager = rm;
            Locale = locale;
        }

        // get a string
        public string GetString(string nameLookup)
        {
            string result = nameLookup;
            try
            {
                if (ResManager != null)
                {
                    result = ResManager.GetString(nameLookup, new System.Globalization.CultureInfo(Locale));
                }
            }
            catch (SystemException ex)
            {
                System.Console.Write(ex);

                result = nameLookup;
            }

            return result;
        }

        public string GetString(string nameLookup, string locale)
        {
            string result = nameLookup;
            try
            {
                if (ResManager != null)
                {
                    result = ResManager.GetString(nameLookup, new System.Globalization.CultureInfo(locale));
                }
            }
            catch (SystemException ex)
            {
                System.Console.Write(ex);

                result = nameLookup;
            }

            return result;
        }
    }
}
