using Ext.Net;

using System.IO;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Text;
using System.Web;
using System.Runtime.InteropServices;
using System.Collections;
using System.Globalization;
using System.Reflection;
using CapaNegocio;
using TreeCore.Data;
using TreeCore;


public class Meses
{
    private int numeroMes = 0;
    private DateTime fechaInicial;
    private DateTime fechaFinal;

    public Meses()
        : base()
    {

    }
    public void SetNumeroMes(int mes)
    {
        numeroMes = mes;
        fechaInicial = GetFechaInicialFromMonthNumber(DateTime.Today.Year, mes);
        fechaFinal = GetFechaFinalFromMonthNumber(DateTime.Today.Year, mes);
    }

    public int GetNumeroMes()
    {
        return numeroMes;
    }
    public void SetFechaInicial(DateTime inicial)
    {
        fechaInicial = inicial;
    }

    public DateTime GetFechaInicial()
    {
        return fechaInicial;
    }

    public void SetFechaFinal(DateTime final)
    {
        fechaFinal = final;
    }

    public DateTime GetFechaFinal()
    {
        return fechaFinal;
    }

    public DateTime GetFechaInicialFromMonthNumber(int year, int mes)
    {
        DateTime jan1 = new DateTime(year, mes, 1);

        return jan1;
    }

    public DateTime GetFechaFinalFromMonthNumber(int year, int mes)
    {
        // Local variables
        DateTime ultimoDia = new DateTime(year, mes, DateTime.DaysInMonth(year, mes));

        return ultimoDia;
    }

    public string getMonthNameByNumber(int mes)
    {
        // Local variables
        string sMes = null;

        switch (mes)
        {
            case 1:
                sMes = "ENERO";
                break;
            case 2:
                sMes = "FEBRERO";
                break;
            case 3:
                sMes = "MARZO";
                break;
            case 4:
                sMes = "ABRIL";
                break;
            case 5:
                sMes = "MAYO";
                break;
            case 6:
                sMes = "JUNIO";
                break;
            case 7:
                sMes = "JULIO";
                break;
            case 8:
                sMes = "AGOSTO";
                break;
            case 9:
                sMes = "SEPTIEMBRE";
                break;
            case 10:
                sMes = "OCTUBRE";
                break;
            case 11:
                sMes = "NOVIEMBRE";
                break;
            case 12:
                sMes = "DICIEMBRE";
                break;
            default:
                sMes = "";
                break;
        }

        // Returns the result
        return sMes;
    }

    public int GetMonthDiff(DateTime fechaInicial, DateTime fechaFinal)
    {
        int cantidadMeses = 0;

        if (fechaInicial.Year != 0001 && fechaInicial != null && fechaFinal.Year != 0001 && fechaFinal != null)
        {
            cantidadMeses = Math.Abs((fechaFinal.Month - fechaInicial.Month) + 12 * (fechaFinal.Year - fechaInicial.Year));
        }
        return cantidadMeses;

        //int retVal = 0;

        //// Calculate the number of years represented and multiply by 12
        //// Substract the month number from the total
        //// Substract the difference of the second month and 12 from the total


        //retVal = (fechaFinal.Year - fechaInicial.Year) * 12;
        //retVal = retVal - fechaFinal.Month;
        //retVal = retVal - (12 - fechaInicial.Month);

        //return Math.Abs (retVal);
    }

    public int GetMonthDiff2(DateTime fechaInicial, DateTime fechaFinal)
    {
        int cantidadMeses = 0;
        int mesAdicional = 0;

        if (fechaInicial.Year != 0001 && fechaInicial != null && fechaFinal.Year != 0001 && fechaFinal != null)
        {
            if (fechaFinal.Day >= fechaInicial.Day)
            {
                mesAdicional = 1;
            }
            cantidadMeses = mesAdicional + Math.Abs((fechaFinal.Month - fechaInicial.Month) + 12 * (fechaFinal.Year - fechaInicial.Year));

        }
        return cantidadMeses;

    }

    public DateTime sumarMesesAFecha(DateTime fecha, int cantidadMeses)
    {
        //ReCalcula la fecha de fin de contrato a partir de la cantidad de meses definida
        DateTime fechaFinal = fecha;

        if (fecha.Year > 1 && fecha != null && cantidadMeses >= 0)
        {
            fechaFinal = fecha.AddMonths(cantidadMeses);
        }

        return fechaFinal;
    }

    public List<string> GetMesesFromRangeByName(DateTime fechaInicial, DateTime fechaFinal)
    {
        // Local variables
        List<string> lista = new List<string>();
        DateTime fechaAuxiliar = new DateTime();
        int mes = 0;
        int i = 0;
        int iMax = 0;
        int iYear = 0;
        string sYear = null;

        // Go over the months between the two dates
        fechaAuxiliar = fechaInicial;
        iMax = GetMonthDiff(fechaInicial, fechaFinal);
        iYear = fechaInicial.Year;
        sYear = iYear.ToString();

        while (i < iMax)
        {
            mes = fechaAuxiliar.Month;
            lista.Add(getMonthNameByNumber(mes) + " " + sYear);
            mes = mes + 1;
            if (mes > 12)
            {
                mes = 1;
                iYear = iYear + 1;
                sYear = iYear.ToString();
            }
            i = i + 1;
        }

        // Returns the result
        return lista;
    }

    public List<int> GetNumeroMesesFromRangeByName(DateTime fechaInicial, DateTime fechaFinal)
    {
        // Local variables
        List<int> lista = new List<int>();
        DateTime fechaAuxiliar = new DateTime();
        int mes = 0;
        int i = 0;
        int iMax = 0;
        int iYear = 0;
        string sYear = null;

        // Go over the months between the two dates
        fechaAuxiliar = fechaInicial;
        iMax = GetMonthDiff(fechaInicial, fechaFinal);
        iYear = fechaInicial.Year;
        sYear = iYear.ToString();
        mes = fechaAuxiliar.Month;

        while (i < iMax)
        {
            lista.Add(mes);
            mes = mes + 1;
            if (mes > 12)
            {
                mes = 1;
                iYear = iYear + 1;
                sYear = iYear.ToString();
            }
            i = i + 1;
        }

        // Returns the result
        return lista;
    }

    public string GetNombreMesByNumero(int mes, int year)
    {
        // Local variables
        string sRes = null;

        sRes = getMonthNameByNumber(mes) + " " + year.ToString();

        // Returns value
        return sRes;
    }


}
