using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

static class EscritorLogs
{
    private static string RutaArchivo;
    private static string NombreFichero;

    public static void SetRuta(string sRuta)
    {
        RutaArchivo = sRuta;
    }

    public static bool SeleccionarArchivo(string sNombre)
    {
        try
        {
            string sNombreArchivoLog = sNombre + "_" + DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString("d2") + DateTime.Now.Day.ToString("d2") + ".log";
            string directorioLogs = TreeCore.DirectoryMapping.GetImportLogDirectory();
            directorioLogs = Path.Combine(directorioLogs, sNombreArchivoLog);

            if (!File.Exists(directorioLogs))
            {
                using (StreamWriter file = File.CreateText(directorioLogs))
                {
                    file.Close();
                }

                NombreFichero = directorioLogs;
            }
            else
            {
                NombreFichero = directorioLogs;
            }
        }
        catch (Exception)
        {
            return false;
        }
        return true;
    }

    public static bool EscribeLogs(string sLinea)
    {
        try
        {
            using (StreamWriter file = File.AppendText(NombreFichero))
            {
                file.WriteLine(sLinea);
            }
        }
        catch (Exception ex)
        {
            return false;
        }
        return true;
    }

}
