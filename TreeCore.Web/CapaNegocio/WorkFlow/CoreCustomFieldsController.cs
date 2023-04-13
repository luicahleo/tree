using System;
using System.Collections.Generic;
using System.Linq;
using Tree.Linq.GenericExtensions;
using TreeCore.Data;
using System.Transactions;

namespace CapaNegocio
{
    public class CoreCustomFieldsController : GeneralBaseController<CoreCustomFields, TreeCoreContext>
    {
        public CoreCustomFieldsController()
            : base()
        { }

        public bool NombreValido(long lClienteID, string Nombre) {
            bool valido = true;
            CoreModulosController cModulos = new CoreModulosController();
            cModulos.SetDataContext(this.Context);
            try
            {
                var oModulo = cModulos.GetModulo(Comun.MODULO_WORKFLOW);
                var oAtr = (from c in Context.CoreAtributosConfiguraciones
                                where c.CoreModuloID == oModulo.CoreModuloID && c.ClienteID == lClienteID && c.Codigo == Nombre
                                select c).FirstOrDefault();
                if (oAtr != null)
                {
                    valido = false;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                valido = false;
            }
            return valido;        
        }

        public List<CoreCustomFields> GetCustomFields(long lClienteID, bool Activo)
        {
            List<CoreCustomFields> listaDatos;
            try
            {
                if (Activo)
                {
                    listaDatos = (from c in Context.CoreCustomFields
                                  join atr in Context.CoreAtributosConfiguraciones on c.CoreAtributoConfiguracionID equals atr.CoreAtributoConfiguracionID
                                  where atr.Activo && atr.ClienteID == lClienteID
                                  select c).ToList();
                }
                else
                {
                    listaDatos = (from c in Context.CoreCustomFields
                                  join atr in Context.CoreAtributosConfiguraciones on c.CoreAtributoConfiguracionID equals atr.CoreAtributoConfiguracionID
                                  where atr.ClienteID == lClienteID
                                  select c).ToList();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                listaDatos = null;
            }
            return listaDatos;
        }

        public CoreAtributosConfiguraciones getAtributoByCodigo (long lCustomFielID)
        {
            CoreAtributosConfiguraciones oDato;

            try
            {
                oDato = (from c in Context.CoreCustomFields
                         join atr in Context.CoreAtributosConfiguraciones on c.CoreAtributoConfiguracionID equals atr.CoreAtributoConfiguracionID
                         where c.CoreCustomFieldID == lCustomFielID
                         select atr).First();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                oDato = null;
            }

            return oDato;
        }

        public CoreCustomFields CreateCustomFields(string Nombre, long lTipoDatoID, long lClienteID)
        {
            CoreCustomFields oDato = null;
            CoreModulosController cModulos = new CoreModulosController();
            cModulos.SetDataContext(this.Context);
            CoreAtributosConfiguracionesController cAtrConf = new CoreAtributosConfiguracionesController();
            cAtrConf.SetDataContext(this.Context);

            using (TransactionScope trans = new TransactionScope(TransactionScopeOption.RequiresNew, new TimeSpan(0)))
            {
                try
                {
                    var oModulo = cModulos.GetModulo(Comun.MODULO_WORKFLOW);
                    if (oModulo != null)
                    {
                        CoreAtributosConfiguraciones oAtr = new CoreAtributosConfiguraciones { 
                            TipoDatoID = lTipoDatoID,
                            Nombre = Nombre,
                            Codigo = Nombre,
                            Activo = true,
                            ValoresPosibles = "",
                            ClienteID = lClienteID,
                            CoreModuloID = oModulo.CoreModuloID
                        };
                        if ((oAtr = cAtrConf.AddItem(oAtr)) != null)
                        {
                            oDato = new CoreCustomFields {
                                CoreAtributoConfiguracionID = oAtr.CoreAtributoConfiguracionID
                            };
                            if ((oDato = AddItem(oDato)) == null)
                            {
                                trans.Dispose();
                                oDato = null;
                            }
                        }
                        else
                        {
                            trans.Dispose();
                            oDato = null;
                        }
                    }
                    trans.Complete();
                }
                catch (Exception ex)
                {
                    trans.Dispose();
                    log.Error(ex.Message);
                    oDato = null;
                }
            }
            return oDato;
        }

        public CoreCustomFields getCustomByAtributo (long lAtributoID)
        {
            CoreCustomFields oCustom;

            try
            {
                oCustom = (from c in Context.CoreCustomFields where c.CoreAtributoConfiguracionID == lAtributoID select c).First();
            }
            catch(Exception ex)
            {
                log.Error(ex.Message);
                oCustom = null;
            }

            return oCustom;
        }

    }
}