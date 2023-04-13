
Import-Module -Name Microsoft.PowerShell.Security

Write-Output "############### START PROJECT DATA CONTEXT UPDATE ###############"

function HandleErrors {
    $msg = $Error[0].Exception.Message
    $exc = $Error[0].Exception
   
    Write-Output $msg
}

$CurrentPath = Split-Path -Path $MyInvocation.MyCommand.Definition -Parent #$PSScriptRoot + "\"
$CurrentPath += "\"

$returnState = $true
$storedPasswordFile = $CurrentPath + ".\storedPassword.tmp"
$storedUserFile = $CurrentPath + ".\storedUser.tmp"
$Credential = $false
$User = "sa"


# check if old user name and password exist
if( (Test-Path $storedUserFile) -eq $true -and
    (Test-Path $storedPasswordFile) -eq $true )
{
    # create credentials
    $Credential = New-Object -TypeName System.Management.Automation.PSCredential -ArgumentList (Get-Content $storedUserFile), (Get-Content $storedPasswordFile | ConvertTo-SecureString)

    if([string]::IsNullOrEmpty($Credential.UserName) -or 
        [string]::IsNullOrEmpty($Credential.Password))
    {
        Write-Output "ERROR: Nombre usuario y/o clave no puede ser vacío"
        if( (Test-Path $storedUserFile) -eq $true )
        {
            Remove-Item $storedUserFile
        }
    
        if( (Test-Path $storedPasswordFile) -eq $true )
        {
            Remove-Item $storedPasswordFile
        }
    
        return $false
    }
}
else
{
    # ask for credentials
    $Credential = $host.ui.PromptForCredential("Credenciales de SQL", "Por favor introduzca su nombre de usuario y contraseña.", $null, $null)#, [System.Management.Automation.PSCredentialTypes]::Domain, [System.Management.Automation.PSCredentialUIOptions]::Default)

    $lastErr = $?

    if( $lastErr -eq $false )
    {
	    HandleErrors
	    return $false
    }

    if([string]::IsNullOrEmpty($Credential.UserName) -or 
        [string]::IsNullOrEmpty($Credential.Password))
    {
        Write-Output "ERROR: Nombre usuario y/o clave no puede ser vacío"
        return $false
    }
}

$userName = "";
$userPass = "";
$plainPassword = "";

if(![string]::IsNullOrEmpty($Credential.UserName) -and 
   ![string]::IsNullOrEmpty($Credential.Password))
{
    $userName = $Credential.UserName;
    $userPass = $Credential.Password;

    $BSTR = [System.Runtime.InteropServices.Marshal]::SecureStringToBSTR($userPass)            
    $plainPassword = [System.Runtime.InteropServices.Marshal]::PtrToStringAuto($BSTR, $userPass.Length)  

    # Free the pointer
    [Runtime.InteropServices.Marshal]::ZeroFreeBSTR($BSTR)
}

$lastErr = $?

if( $lastErr -eq $false )
{
	HandleErrors
	return $false
}

# sql metal call 
$sqlMetalExe = $CurrentPath + ".\SqlMetal.exe"
$pdcPath = $CurrentPath + "..\ProjectDataContext.cs"

$sqlMetalCmdArgList = $null
$sqlMetalCmdArgList = @(
	"/conn:""server='80.28.131.53,9433';database='Sites';user='$($userName)';password='$($plainPassword)'""",
	"/code:$($pdcPath)",
	"/namespace:""Sites.Data""",
    "/context:""SitesContext""",
	"/language:csharp",
    "/views",
    "/functions",
    "/sprocs"
)

#$sqlMetalCmdArgList = @(
#	"/server:""80.28.131.53,9433""",
#    "/database:""Sites""",
#	"/code:$($pdcPath)",
#	"/namespace:""Sites.Data""",
#    "/context:""SitesContext""",
#	"/language:csharp",
#    "/views",
#    "/functions",
#    "/sprocs"
#)

#$sqlMetalCmdArgList = @(
#	"/server:""80.28.131.53,9433""",
#    "/database:""Sites""",
#    "/user:$($userName)",
#    "/password:$($plainPassword)",
#	"/code:$($pdcPath)",
#	"/namespace:""Sites.Data""",
#    "/context:""SitesContext""",
#	"/language:csharp",
#    "/views",
#    "/functions",
#    "/sprocs"
#)

$runGetResult = & $sqlMetalExe $sqlMetalCmdArgList | Write-Output

#Write-Output $runGetResult

if( $runGetResult -contains "Error :" )
{
	return $false
}

$lastErr = $?

if( $lastErr -eq $false )
{
	HandleErrors

    if( (Test-Path $storedUserFile) -eq $true )
    {
        Remove-Item $storedUserFile
    }

    if( (Test-Path $storedPasswordFile) -eq $true )
    {
        Remove-Item $storedPasswordFile
    }

	return $false
}

#store user and pass
$userName | Out-File $storedUserFile
$userPass | ConvertFrom-SecureString | Out-File $storedPasswordFile

Write-Output "### RUNNING ChanceProcedureDateType ###"

# procedure chance
$chanceProcedureExe = $CurrentPath + ".\ChanceProcedureDateType.exe"
$chanceProcedureCmdArgList = @( """$($pdcPath)""",
	"""ISingleResult<Sp_EmplazamientosByPrpyectos_GetResult>|ISingleResult<Vw_Emplazamientos>""",
	"""ISingleResult<Sp_EmplazamientosFueraDePrpyectos_GetResult>|ISingleResult<Vw_Emplazamientos>""",
	"""ISingleResult<Sp_EmplazamientosByProyectosOperador_GetResult>|ISingleResult<Vw_Emplazamientos>""",
    """ISingleResult<Sp_EmplazamientosFueraDeProyectosOperador_GetResult>|ISingleResult<Vw_Emplazamientos>""",
	"""ISingleResult<Sp_SaringEmplazamientosByPrpyectos_GetResult>|ISingleResult<Vw_SharingEmplazamientos>""",
    """ISingleResult<Sp_CityEmplazamientosByProyectos_GetResult>|ISingleResult<Vw_CityEmplazamientos>""",
    """ISingleResult<Sp_AdquisicionesEmplazamientosByProyectos_GetResult>|ISingleResult<Vw_AdquisicionesEmplazamientos>""",
    """ISingleResult<sp_EmplazamientosCercanosVista_GetResult>|ISingleResult<Vw_Emplazamientos>""",
    """ISingleResult<Sp_GlobalEmplazamientosCercanosByClienteProyectoRentaMenorResult>|ISingleResult<Vw_Emplazamientos>""",
    """ISingleResult<Sp_GlobalEmplazamientosCercanosByClienteProyectoRentaMayorResult>|ISingleResult<Vw_Emplazamientos>""",
    """ISingleResult<Sp_GlobalEmplazamientosCercanosByClienteProyectoRentaIgualResult>|ISingleResult<Vw_Emplazamientos>""",
    """ISingleResult<Sp_GlobalEmplazamientosCercanosByClienteProyectoRentaRangoResult>|ISingleResult<Vw_Emplazamientos>""",
    """ISingleResult<Sp_GlobalEmplazamientosRentaMenorResult>|ISingleResult<Vw_Emplazamientos>""",
    """ISingleResult<Sp_GlobalEmplazamientosRentaMayorResult>|ISingleResult<Vw_Emplazamientos>""",
    """ISingleResult<Sp_GlobalEmplazamientosRentaIgualResult>|ISingleResult<Vw_Emplazamientos>""",
    """ISingleResult<Sp_GlobalEmplazamientosRentaRangoResult>|ISingleResult<Vw_Emplazamientos>"""
)

$runGetResult = & $chanceProcedureExe $chanceProcedureCmdArgList | Write-Output

#Write-Output $runGetResult

$lastErr = $?

if( $lastErr -eq $false )
{
	HandleErrors
	return $false
}

Write-Output "### COMPLETE ChanceProcedureDateType ###"
Write-Output ""

# Split file

Write-Output "### RUNNING ProjectDataContextSplitter ###"
Write-Output ""

$pdcSplitExe = $CurrentPath + ".\PDCSplitter\ProjectDataContextSplitter.exe"
$pdcSplitCmdArgList = @(
	$pdcPath
)

$runGetResult = & $pdcSplitExe $pdcSplitCmdArgList | Write-Output

$lastErr = $?

if( $lastErr -eq $false )
{
	HandleErrors
	return $false
}

Write-Output "### COMPLETE ProjectDataContextSplitter ###"
Write-Output ""

if( (Test-Path $storedUserFile) -eq $true )
{    
    Remove-Item $pdcPath
}

$lastErr = $?

if( $lastErr -eq $false )
{
	HandleErrors
	return $false
}

Write-Output "############### END PROJECT DATA CONTEXT UPDATE ###############"

return $true
