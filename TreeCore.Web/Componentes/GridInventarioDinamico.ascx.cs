using CapaNegocio;
using Ext.Net;
using log4net;
using System;
using System.Collections.Generic;
using TreeCore.Page;
using System.Reflection;
using System.Linq;
using System.Data;

namespace TreeCore.Componentes
{
    public partial class GridInventarioDinamico : BaseUserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //ReconfigurarGridPanelDinamico(2);

        }

        private AbstractStore GetOfficeStore()
        {
            Store store = new Store();
            //store.Data = this.GetOfficesData();
            store.Fields.Add("City", "TotalEmployees", "Manager");
            return store;
        }

        private AbstractStore GetStore(DataTable datos)
        {
            Store store = new Store();
            store.Data = datos.Rows;

            foreach (DataColumn column in datos.Columns)
            {
                store.Fields.Add(column.ColumnName);
            }

            return store;
        }

        private ColumnBase[] GetColumns(DataTable datos)
        {

            ColumnBase[] columnas = new ColumnBase[datos.Columns.Count];
            int i = 0;
            foreach (DataColumn column in datos.Columns)
            {
                columnas[i] = new Column()
                {
                    Text = column.ColumnName,
                    DataIndex = column.ColumnName,
                    Flex = 1
                };

                i++;
            }

            return columnas;
        }


    }
}