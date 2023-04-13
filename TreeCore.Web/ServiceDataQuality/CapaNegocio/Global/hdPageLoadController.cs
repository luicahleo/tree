using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Tree.Linq.GenericExtensions;
using TreeCore.Data;
using Newtonsoft.Json;
using Ext.Net;
using log4net;
using System.Reflection;

namespace CapaNegocio
{
    public class hdPageLoadController
    {
        protected static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private Hidden hidden;

        public hdPageLoadController(Hidden hd)
            : base()
        { 
            hidden = hd;
            if (hidden.Value == null || hidden.Value.ToString() == "")
            {
                List<HdToken> listaTk = new List<HdToken>();
                hidden.Value = JsonConvert.SerializeObject(listaTk);
            }
        }

        public void SetValor(string key, string valor) {
            try
            {
                List<HdToken> listaTk = JsonConvert.DeserializeObject<List<HdToken>>(hidden.Value.ToString());
                HdToken oTk = listaTk.Where(c => c.Key == key).FirstOrDefault();
                listaTk.Add(new HdToken
                {
                    Key = key,
                    Valor = valor
                });
                hidden.Value = JsonConvert.SerializeObject(listaTk);
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
        }

        public string GetValor(string key) {
            string value = "";
            try
            {
                List<HdToken> listaTk = JsonConvert.DeserializeObject<List<HdToken>>(hidden.Value.ToString());
                HdToken oTk = listaTk.Where(c => c.Key == key).FirstOrDefault();
                if (oTk != null)
                {
                    value = oTk.Valor;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                value = "";
            }
            return value;
        }
        private class HdToken
        {
            public string Key;
            public string Valor;
        }

    }
}
