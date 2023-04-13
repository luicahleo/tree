using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TreeCore.Integraciones.Comarch
{
    public class AddressItemCityResponse
    {
        public AddressItemCityResponse(): base() { }

        /// <summary>
        ///OBLIGATORIO - 
        ///0: OSS Internal error. Error interno del OSS.
        ///101: Invalid credentials for this operation.Credenciales inválidas para esta operación.
        ///103: City already created. Ciudad enviada ya se encuentra creado.
        ///200: OK. Operación exitosa
        //104: Missing mandatory attribute – operation
        ///105: Missing mandatory attribute – abbreviation
        ///106: Missing mandatory attribute – abbreviationregion
        ///107: Missing mandatory attribute – masterregionname
        ///108: Missing attribute - name

        /// </summary>
        public string CODE_RESPUESTA { get; set; }

        /// <summary>
        /// OBLIGATORIO - Descripción del código de respuesta enviado
        ///0: OSS Internal error. Error interno del OSS.
        ///101: Invalid credentials for this operation.Credenciales inválidas para esta operación.
        ///103: City already created. Ciudad enviada ya se encuentra creado.
        ///200: OK. Operación exitosa
        //104: Missing mandatory attribute – operation
        ///105: Missing mandatory attribute – abbreviation
        ///106: Missing mandatory attribute – abbreviationregion
        ///107: Missing mandatory attribute – masterregionname
        ///108: Missing attribute - name
        /// </summary>
        public string MENSAJE_RESPUESTA { get; set; }
    }
}