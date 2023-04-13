using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TreeCore.Integraciones.Comarch
{
    public class LocationResponse
    {
        public LocationResponse() : base() { }

        /// <summary>
        ///OBLIGATORIO - 
        ///0: OSS Internal error. Error interno del OSS.
        ///101: Invalid credentials for this operation.Credenciales inválidas para esta operación.
        ///114: Location already created.Departamento enviado ya se encuentra creado.
        ///200: OK Operación exitosa
        ///115: Location already created
        ///116: Missing attribute – legacyId
        ///117: Missing attribute – name
        ///118: Missing attribute – MasterAddress
        ///119: Missing attribute – LocationStatus
        ///120: Missing attribute – PEPCodeYear
        ///121: Missing attribute – PEPCodeSociety
        ///122: Missing attribute - PEPCodeN1
        ///123: Missing attribute – Latitud
        ///124: Missing attribute – Longitude.
        ///125: Missing attribute - DetailInterest.MasterContactUnit.Name9
        ///126: Missing attribute – MasterAddress
        ///127: Missing attribute – MasterCityArea.Name
        ///128: Missing attribute – MasterAddress
        ///129: Invalid coordinates format
        /// </summary>
        public string CODE_RESPUESTA { get; set; }

        /// <summary>
        /// OBLIGATORIO - Descripción del código de respuesta enviado
        ///0: OSS Internal error. Error interno del OSS.
        ///101: Invalid credentials for this operation.Credenciales inválidas para esta operación.
        ///114: Location already created.Departamento enviado ya se encuentra creado.
        ///200: OK Operación exitosa
        ///115: Location already created
        ///116: Missing attribute – legacyId
        ///117: Missing attribute – name
        ///118: Missing attribute – MasterAddress
        ///119: Missing attribute – LocationStatus
        ///120: Missing attribute – PEPCodeYear
        ///121: Missing attribute – PEPCodeSociety
        ///122: Missing attribute - PEPCodeN1
        ///123: Missing attribute – Latitud
        ///124: Missing attribute – Longitude.
        ///125: Missing attribute - DetailInterest.MasterContactUnit.Name9
        ///126: Missing attribute – MasterAddress
        ///127: Missing attribute – MasterCityArea.Name
        ///128: Missing attribute – MasterAddress
        ///129: Invalid coordinates format
        /// </summary>
        public string MENSAJE_RESPUESTA { get; set; }
    }
}