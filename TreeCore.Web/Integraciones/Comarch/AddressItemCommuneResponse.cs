using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TreeCore.Integraciones.Comarch
{
    public class AddressItemCommuneResponse
    {
        public AddressItemCommuneResponse() : base() { }

        /// <summary>
        ///OBLIGATORIO - 
        ///0: OSS Internal error. Error interno del OSS.
        //101: Invalid credentials for this operation. Credenciales inválidas para esta operación.
        //102: Object already created. Localidad/comuna enviado ya se encuentra creado.
        ///200: OK. Operación exitosa
        //103: Missing Mandatory attribute ($attribute_name) for creation operation. Falta atributo obligatorio para la creación del objeto
        /// </summary>
        public string CODE_RESPUESTA { get; set; }

        /// <summary>
        ///OBLIGATORIO - 
        ///0: OSS Internal error. Error interno del OSS.
        //101: Invalid credentials for this operation. Credenciales inválidas para esta operación.
        //102: Object already created. Localidad/comuna enviado ya se encuentra creado.
        ///200: OK. Operación exitosa
        //103: Missing Mandatory attribute ($attribute_name) for creation operation. Falta atributo obligatorio para la creación del objeto
        /// </summary>
        public string MENSAJE_RESPUESTA { get; set; }
    }
}