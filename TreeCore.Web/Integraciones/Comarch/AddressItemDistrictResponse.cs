using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TreeCore.Integraciones.Comarch
{
    public class AddressItemDistrictResponse
    {
        public AddressItemDistrictResponse() : base() { }

        /// <summary>
        ///OBLIGATORIO - 
        ///0: OSS Internal error.Error interno del OSS.
        ///101: Invalid credentials for this operation. Credenciales inválidas para esta operación.
        ///109: District already created. Localidad/comuna enviado ya se encuentra creado.
        ///200: OK Operación exitosa
        ///110: Mandatory attribute is missing – addressitemtype
        ///111: Mandatory attribute is missing – masterabbreviation
        ///112: Mandatory attribute is missing – abbreviationdistrict
        ///113: Mandatory attribute is missing – MasterDistrict
        /// </summary>
        public string CODE_RESPUESTA { get; set; }

        /// <summary>
        /// OBLIGATORIO - Descripción del código de respuesta enviado
        ///0: OSS Internal error.Error interno del OSS.
        ///101: Invalid credentials for this operation. Credenciales inválidas para esta operación.
        ///109: District already created. Localidad/comuna enviado ya se encuentra creado.
        ///200: OK Operación exitosa
        ///110: Mandatory attribute is missing – addressitemtype
        ///111: Mandatory attribute is missing – masterabbreviation
        ///112: Mandatory attribute is missing – abbreviationdistrict
        ///113: Mandatory attribute is missing – MasterDistrict
        /// </summary>
        public string MENSAJE_RESPUESTA { get; set; }
    }
}