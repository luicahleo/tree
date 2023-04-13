using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using NLog;

public class EscritorLogs
{
    public string NombreFichero;
    private Logger log;

    public EscritorLogs(string sNombre) {
        log = LogManager.GetCurrentClassLogger();
        string sNombreArchivoLog = sNombre + "_" + DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString("d2") + DateTime.Now.Day.ToString("d2") + ".log";
        string directorioLogs = TreeCore.DirectoryMapping.GetImportLogDirectory();
        directorioLogs = Path.Combine(directorioLogs, sNombreArchivoLog);

        if (!File.Exists(directorioLogs))
        {
            using (StreamWriter file = File.CreateText(directorioLogs))
            {
                file.Close();
            }
        }
        NombreFichero = directorioLogs;
    }

    public void EscribeLogs(string sLinea)
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
            log.Error(ex.Message);
        }
    }

}
