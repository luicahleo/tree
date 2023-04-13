using CapaNegocio;
using Ext.Net;
using log4net;
using System;
using System.Collections.Generic;
using System.Reflection;
using TreeCore.Page;
using NCrontab;
using System.Linq;

namespace TreeCore.Componentes
{
    public partial class ProgramadorTareasCron : BaseUserControl
    {


        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);



        protected void Page_Load(object sender, EventArgs e)
        {

            hdClienteID = (Hidden)X.GetCmp("hdCliID");
            hdMinDate.Value = DateTime.MinValue;
        }


        #region STORES


        #endregion

        public DateTime FechaInicio
        {
            get { return DateTime.Parse(txtFechaInicio.Value.ToString()); }
            set { this.txtFechaInicio.Value = value; }
        }
        public DateTime FechaFin
        {
            get { return DateTime.Parse(txtFechaFin.Value.ToString()); ; }
            set { this.txtFechaFin.Value = value; }
        }

        public string CronFormat
        {
            get { return txtCronFormat.Value.ToString(); }
            set { this.txtCronFormat.Value = value; }
        }
        public string Frecuencias
        {
            get { return cmbFrecuencia.Value.ToString(); }
            set { this.cmbFrecuencia.SetValue(value); }
        }
        public long DiaCadaMes
        {
            get { return long.Parse(txtDiaCadaMes.Value.ToString()); }
            set { this.txtDiaCadaMes.Value = value; }
        }

        public List<string> Dias
        {
            get
            {
                List<string> lista = new List<string>();
                foreach (var item in cmbDias.SelectedItems.ToList())
                {
                    lista.Add(item.Value.ToString());
                }
                return lista;

            }
            set { this.cmbDias.SetValue(value); }
        }

        public List<string> Meses
        {
            get
            {
                List<string> lista = new List<string>();
                foreach (var item in cmbMeses.SelectedItems.ToList())
                {
                    lista.Add(item.Value.ToString());
                }
                return lista;

            }
            set { this.cmbMeses.SetValue(value); }
        }

        //public string Dias
        //{
        //    get { return cmbDias.Value.ToString(); }
        //    set
        //    {
        //        string[] valores = value.Split(',');
        //        foreach (var item in valores)
        //        {
        //            this.cmbDias.SelectItem(item);
        //        }
        //    }
        //}

        //public string Dias
        //{
        //    get { return cmbDias.Value.ToString(); }
        //    set { this.cmbDias.Value = value; }
        //}
        //public string Meses
        //{
        //    get { return cmbMeses.Value.ToString(); }
        //    set { this.cmbMeses.Value = value; }
        //}
        //public long MesInicio
        //{
        //    get { return long.Parse(cmbMesInicio.Value.ToString()); }
        //    set { this.cmbMesInicio.Value = value; }
        //}
        public string TipoFrecuencia
        {
            get { return cmbTipoFrecuencia.Value.ToString(); }
            set { this.cmbTipoFrecuencia.Value = value; }
        }


        [DirectMethod()]
        public DirectResponse GenerarCron()
        {
            DirectResponse direct = new DirectResponse();
            CoreServiciosFrecuenciasController CCoreServiciosFrecuencias = new CoreServiciosFrecuenciasController();
            long lCliID = 0;
            string Parte1 = "0";
            string Parte2 = "0";
            string Parte3 = "";
            string Parte4 = "";
            string Parte5 = "";

            try
            {
                if (cmbFrecuencia.SelectedItem != null)
                {

                    if (cmbFrecuencia.Value.ToString() == "Diario")
                    {

                        Parte5 = "*";
                        Parte3 = "*";
                        Parte4 = "*";

                    }
                    else if (cmbFrecuencia.Value.ToString() == "DiaLaborable")
                    {
                        Parte5 = "1,2,3,4,5";
                        Parte3 = "*";
                        Parte4 = "*";
                    }
                    else if (cmbFrecuencia.Value.ToString() == "Semanal")
                    {
                        string numeroDia = "";
                        DateTime Fecha = (DateTime)txtFechaInicio.Value;
                        String DiaSemana = Fecha.DayOfWeek.ToString();

                        if (DiaSemana == GetGlobalResource("strDomingo") || DiaSemana == "0")
                        {
                            numeroDia = "0/7";
                        }
                        else if (DiaSemana == GetGlobalResource("strLunes") || DiaSemana == "1")
                        {
                            numeroDia = "1/7";
                        }
                        else if (DiaSemana == GetGlobalResource("strMartes") || DiaSemana == "2")
                        {
                            numeroDia = "2/7";
                        }
                        else if (DiaSemana == GetGlobalResource("strMiercoles") || DiaSemana == "3")
                        {
                            numeroDia = "3/7";
                        }
                        else if (DiaSemana == GetGlobalResource("strJueves") || DiaSemana == "4")
                        {
                            numeroDia = "4/7";
                        }
                        else if (DiaSemana == GetGlobalResource("strViernes") || DiaSemana == "5")
                        {
                            numeroDia = "5/7";
                        }
                        else if (DiaSemana == GetGlobalResource("strSabado") || DiaSemana == "6")
                        {
                            numeroDia = "6/7";
                        }

                        Parte5 = numeroDia;
                        Parte3 = "*";
                        Parte4 = "*";

                    }
                    else if (cmbFrecuencia.Value.ToString() == "Mensual")
                    {
                        DateTime Fecha = (DateTime)txtFechaInicio.Value;
                        String Mes = Fecha.Month.ToString();
                        String DiaDelMes = Fecha.Day.ToString();

                        Parte4 = "*/1";
                        Parte3 = DiaDelMes;
                        Parte5 = "*";
                    }
                    else if (cmbFrecuencia.Value.ToString() == "SemanalCustom")
                    {
                        string Cadena = "";
                        long numeroDia = 0;
                        string Valor = cmbDias.RawValue.ToString();
                        string[] valores = Valor.Split(',');
                        foreach (var item in valores)
                        {
                            if ((item.ToString()).Trim() == GetGlobalResource("strDomingo") || item.ToString() == "0")
                            {
                                numeroDia = 0;
                            }
                            else if ((item.ToString()).Trim() == GetGlobalResource("strLunes") || item.ToString() == "1")
                            {
                                numeroDia = 1;
                            }
                            else if ((item.ToString()).Trim() == GetGlobalResource("strMartes") || item.ToString() == "2")
                            {
                                numeroDia = 2;
                            }
                            else if ((item.ToString()).Trim() == GetGlobalResource("strMiercoles") || item.ToString() == "3")
                            {
                                numeroDia = 3;
                            }
                            else if ((item.ToString()).Trim() == GetGlobalResource("strJueves") || item.ToString() == "4")
                            {
                                numeroDia = 4;
                            }
                            else if ((item.ToString()).Trim() == GetGlobalResource("strViernes") || item.ToString() == "5")
                            {
                                numeroDia = 5;
                            }
                            else if ((item.ToString()).Trim() == GetGlobalResource("strSabado") || item.ToString() == "6")
                            {
                                numeroDia = 6;
                            }

                            Cadena = Cadena + numeroDia.ToString() + ",";
                            char[] quitar = { ',', };
                            Parte5 = Cadena.TrimEnd(quitar);
                        }

                        Parte3 = "*";
                        Parte4 = "*";

                    }
                    else if (cmbFrecuencia.Value.ToString() == "MensualCustom")
                    {
                        string Cadena = "";
                        string ValorReal = "";
                        if (cmbTipoFrecuencia.RawValue.ToString() != "")
                        {
                            if (cmbTipoFrecuencia.RawValue.ToString() == GetGlobalResource("strCadaMes"))
                            {
                                ValorReal = "1/1";
                            }
                            else if (cmbTipoFrecuencia.RawValue.ToString() == GetGlobalResource("strCadaDosMeses"))
                            {
                                ValorReal = "1/2";
                            }
                            else if (cmbTipoFrecuencia.RawValue.ToString() == GetGlobalResource("strCadaTresMeses"))
                            {
                                ValorReal = "1/3";
                            }
                            else if (cmbTipoFrecuencia.RawValue.ToString() == GetGlobalResource("strCadaSeisMeses"))
                            {
                                ValorReal = "1/6";
                            }
                            else if (cmbTipoFrecuencia.RawValue.ToString() == GetGlobalResource("strCadaAño"))
                            {
                                ValorReal = "1/12";
                            }                            

                            Parte3 = txtDiaCadaMes.Value.ToString();
                            Parte4 = ValorReal;
                            Parte5 = "*";
                        }
                        else
                        {
                            long numeroMes = 0;
                            string Valor = cmbMeses.RawValue.ToString();
                            string[] valores = Valor.Split(',');

                            foreach (var item in valores)
                            {
                                if ((item.ToString()).Trim() == GetGlobalResource("strEnero") || item.ToString() == "1")
                                {
                                    numeroMes = 1;
                                }
                                else if ((item.ToString()).Trim() == GetGlobalResource("strFebrero") || item.ToString() == "2")
                                {
                                    numeroMes = 2;
                                }
                                else if ((item.ToString()).Trim() == GetGlobalResource("strMarzo") || item.ToString() == "3")
                                {
                                    numeroMes = 3;
                                }
                                else if ((item.ToString()).Trim() == GetGlobalResource("strAbril") || item.ToString() == "4")
                                {
                                    numeroMes = 4;
                                }
                                else if ((item.ToString()).Trim() == GetGlobalResource("strMayo") || item.ToString() == "5")
                                {
                                    numeroMes = 5;
                                }
                                else if ((item.ToString()).Trim() == GetGlobalResource("strJunio") || item.ToString() == "6")
                                {
                                    numeroMes = 6;
                                }
                                else if ((item.ToString()).Trim() == GetGlobalResource("strJulio") || item.ToString() == "7")
                                {
                                    numeroMes = 7;
                                }
                                else if ((item.ToString()).Trim() == GetGlobalResource("strAgosto") || item.ToString() == "8")
                                {
                                    numeroMes = 8;
                                }
                                else if ((item.ToString()).Trim() == GetGlobalResource("strSeptiembre") || item.ToString() == "9")
                                {
                                    numeroMes = 9;
                                }
                                else if ((item.ToString()).Trim() == GetGlobalResource("strOctubre") || item.ToString() == "10")
                                {
                                    numeroMes = 10;
                                }
                                else if ((item.ToString()).Trim() == GetGlobalResource("strNoviembre") || item.ToString() == "11")
                                {
                                    numeroMes = 11;
                                }
                                else if ((item.ToString()).Trim() == GetGlobalResource("strDiciembre") || item.ToString() == "12")
                                {
                                    numeroMes = 12;
                                }

                                Cadena = Cadena + numeroMes.ToString() + ",";
                                char[] quitar = { ',', };
                                Parte4 = Cadena.TrimEnd(quitar);

                                Parte3 = txtDiaCadaMes.Value.ToString();
                                Parte5 = "*";
                            }

                        }
                    }
                    else
                    {
                        direct.Success = false;
                        direct.Result = GetGlobalResource(Comun.strMensajeGenerico);
                        return direct;
                    }

                }
                else
                {
                    direct.Success = false;
                    direct.Result = GetGlobalResource(Comun.strMensajeGenerico);
                    return direct;
                }


                string cronFormat = Parte1 + " " + Parte2 + " " + Parte3 + " " + Parte4 + " " + Parte5;
                txtCronFormat.Value = cronFormat;

                var s = CrontabSchedule.Parse(cronFormat);

                if ((DateTime)txtFechaFin.Value != DateTime.MinValue)
                {
                    var occurrences = s.GetNextOccurrences((DateTime)txtFechaInicio.Value, (DateTime)txtFechaFin.Value);
                    txtPrevisualizar.Text = string.Join(Environment.NewLine, (from t in occurrences select $"{t:ddd, dd MMM yyyy}").Take(5));
                }
                else
                {
                    var occurrences = s.GetNextOccurrences((DateTime)txtFechaInicio.Value, DateTime.Now.AddYears(7));
                    txtPrevisualizar.Text = string.Join(Environment.NewLine, (from t in occurrences select $"{t:ddd, dd MMM yyyy}").Take(5));
                }

            }
            catch (Exception ex)
            {
                direct.Success = false;
                direct.Result = GetGlobalResource(Comun.strMensajeGenerico);
                log.Error(ex.Message);
                return direct;
            }

            direct.Success = true;
            direct.Result = "";

            return direct;

        }
    }

}
