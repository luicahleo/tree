using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TreeCore.Integraciones.Comarch
{
    public class AddressItemRegionResponse
    {
        public AddressItemRegionResponse() : base() { }

        /// <summary>
        ///OBLIGATORIO - 
        ///0: OSS Internal error. Error interno del OSS.
        ///101: Invalid credentials for this operation. Credenciales inválidas para esta operación.
        ///102: Region already created. Departamento enviado ya se encuentra creado.
        ///200: OK. Operación exitosa
        /// </summary>
        public string CODE_RESPUESTA { get; set; }

        /// <summary>
        /// OBLIGATORIO - Descripción del código de respuesta enviado
        ///0: OSS Internal error. Error interno del OSS.
        ///101: Invalid credentials for this operation. Credenciales inválidas para esta operación.
        ///102: Region already created. Departamento enviado ya se encuentra creado.
        ///200: OK. Operación exitosa
        /// </summary>
        public string MENSAJE_RESPUESTA { get; set; }
    }
}