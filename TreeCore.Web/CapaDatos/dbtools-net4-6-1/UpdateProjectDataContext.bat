echo off
cls

sqlmetal /conn:"server='192.168.21.209'; database='Sites'; user id='sa'; password='*eHdlMdH.pw82#'" /code:"..\ProjectDataContext.cs" /namespace:"TreeCore.Data" /context:"TreeCoreContext" /language:csharp /views /functions /sprocs
echo.
ChanceProcedureDateType.exe "..\ProjectDataContext.cs" "ISingleResult<Sp_EmplazamientosByPrpyectos_GetResult>|ISingleResult<Vw_Emplazamientos>" "ISingleResult<Sp_EmplazamientosFueraDePrpyectos_GetResult>|ISingleResult<Vw_Emplazamientos>" "ISingleResult<Sp_EmplazamientosByProyectosOperador_GetResult>|ISingleResult<Vw_Emplazamientos>" "ISingleResult<Sp_EmplazamientosFueraDeProyectosOperador_GetResult>|ISingleResult<Vw_Emplazamientos>" "ISingleResult<Sp_SaringEmplazamientosByPrpyectos_GetResult>|ISingleResult<Vw_SharingEmplazamientos>" "ISingleResult<Sp_CityEmplazamientosByProyectos_GetResult>|ISingleResult<Vw_CityEmplazamientos>" "ISingleResult<Sp_AdquisicionesEmplazamientosByProyectos_GetResult>|ISingleResult<Vw_AdquisicionesEmplazamientos>" "ISingleResult<sp_EmplazamientosCercanosVista_GetResult>|ISingleResult<Vw_Emplazamientos>" "ISingleResult<Sp_GlobalEmplazamientosCercanosByClienteProyectoRentaMenorResult>|ISingleResult<Vw_Emplazamientos>" "ISingleResult<Sp_GlobalEmplazamientosCercanosByClienteProyectoRentaMayorResult>|ISingleResult<Vw_Emplazamientos>" "ISingleResult<Sp_GlobalEmplazamientosCercanosByClienteProyectoRentaIgualResult>|ISingleResult<Vw_Emplazamientos>" "ISingleResult<Sp_GlobalEmplazamientosCercanosByClienteProyectoRentaRangoResult>|ISingleResult<Vw_Emplazamientos>" "ISingleResult<Sp_GlobalEmplazamientosRentaMenorResult>|ISingleResult<Vw_Emplazamientos>" "ISingleResult<Sp_GlobalEmplazamientosRentaMayorResult>|ISingleResult<Vw_Emplazamientos>" "ISingleResult<Sp_GlobalEmplazamientosRentaIgualResult>|ISingleResult<Vw_Emplazamientos>" "ISingleResult<Sp_GlobalEmplazamientosRentaRangoResult>|ISingleResult<Vw_Emplazamientos>"
echo Recuerde cambiar los namespaces de entities 
echo.
echo splitting file
"PDCSplitter/ProjectDataContextSplitter.exe" "..\ProjectDataContext.cs"
echo done!
echo Removing original
del "..\ProjectDataContext.cs"
echo done!
pause