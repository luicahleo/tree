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


public class EmplazamientosSeguimientosGenerico
{
    private long emplazamientosSeguimientosGenericoID;
    private string documentoID;
    private string proyectoTipo;
    private DateTime fecha;
    private string emplazamientoNombre;
    private string emplazamientoCodigo;
    private string emplazamientoEstado_esES;
    private string nombreUsuario;
    private string nombre;
    private string apellidos;
    private string email;
    private string nota;
    private string cambios;
    private Boolean activo;
    private Boolean interno;
    private Boolean soporte;
    private string empresaproveedora;
    private string alias;


    public string Alias { get => alias; set => alias = value; }
    public long EmplazamientosSeguimientosGenericoID { get => emplazamientosSeguimientosGenericoID; set => emplazamientosSeguimientosGenericoID = value; }
    public string DocumentoID { get => documentoID; set => documentoID = value; }
    public string ProyectoTipo { get => proyectoTipo; set => proyectoTipo = value; }
    public DateTime Fecha { get => fecha; set => fecha = value; }
    public string EmplazamientoNombre { get => emplazamientoNombre; set => emplazamientoNombre = value; }
    public string EmplazamientoCodigo { get => emplazamientoCodigo; set => emplazamientoCodigo = value; }
    public string EmplazamientoEstado_esES { get => emplazamientoEstado_esES; set => emplazamientoEstado_esES = value; }
    public string NombreUsuario { get => nombreUsuario; set => nombreUsuario = value; }
    public string Nombre { get => nombre; set => nombre = value; }
    public string Apellidos { get => apellidos; set => apellidos = value; }
    public string Email { get => email; set => email = value; }
    public string Nota { get => nota; set => nota = value; }
    public string Cambios { get => cambios; set => cambios = value; }
    public bool Activo { get => activo; set => activo = value; }

    public bool Interno { get => interno; set => interno = value; }
    public bool Soporte { get => soporte; set => soporte = value; }
    public string EmpresaProveedora { get => empresaproveedora; set => empresaproveedora = value; }

    public EmplazamientosSeguimientosGenerico()
        : base()
    {

    }

    public EmplazamientosSeguimientosGenerico(long EmplazamientosSeguimientosGenericoID, string DocumentoID, string ProyectoTipo,
        DateTime Fecha, string EmplazamientoNombre, string EmplazamientoCodigo, string EmplazamientoEstado_esES, string NombreUsuario, string Nombre, string Apellidos, string Email,
        string Nota, string Cambios, Boolean Activo, Boolean Interno, Boolean Soporte, string EmpresaProveedora,string Alias)
        : base()
    {
        this.EmplazamientosSeguimientosGenericoID = EmplazamientosSeguimientosGenericoID;
        this.DocumentoID = DocumentoID;
        this.ProyectoTipo = ProyectoTipo;
        this.Fecha = Fecha;
        this.EmplazamientoNombre = EmplazamientoNombre;
        this.EmplazamientoCodigo = EmplazamientoCodigo;
        this.EmplazamientoEstado_esES = EmplazamientoEstado_esES;
        this.NombreUsuario = NombreUsuario;
        this.Nombre = Nombre;
        this.Apellidos = Apellidos;
        this.Email = Email;
        this.Nota = Nota;
        this.Cambios = Cambios;
        this.Activo = Activo;
        this.Interno = Interno;
        this.Soporte = Soporte;
        this.EmpresaProveedora = EmpresaProveedora;
        this.Alias = Alias;
    }
}
