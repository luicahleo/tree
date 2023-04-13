using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace TreeCore.Clases
{
    public abstract class BaseIHttpHandler : IHttpHandler
    {
        public abstract bool IsReusable { get; }

        public abstract void ProcessRequest(HttpContext context);


        public string GetGlobalResource(string tag, CultureInfo cultureInfo)
        {
            string resource;

            object valor;
            try
            {

                valor = HttpContext.GetGlobalResourceObject(Comun.NOMBRE_FICHERO_RECURSOS, tag, cultureInfo);
                
                if (valor != null)
                {
                    resource = valor.ToString();
                }
                else
                {
                    resource = "";
                }
            }
            catch (Exception)
            {
                resource = "";
            }


            return resource; 
        }
        
    }
}