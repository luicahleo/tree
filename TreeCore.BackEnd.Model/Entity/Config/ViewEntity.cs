using System;

namespace TreeCore.BackEnd.Model.Entity.Config
{
    public class ViewEntity : BaseEntity
    {
        public readonly int? CoreGestionVistaID;
        public readonly string Nombre;
        public readonly int UsuarioID;
        public readonly string JsonColumnas;
        public readonly string JsonFiltros;
        public readonly string Pagina;
        public readonly string Icono;
        public readonly DateTime FechaCreacion;
        public readonly DateTime FechaUltimaModificacion;
        public readonly bool Defecto;

        public ViewEntity(int? coreGestionVistaID, string nombre, int usuarioID, string jsonColumnas, string jsonFiltros, string pagina, string icono, DateTime fechaCreacion, DateTime fechaUltimaModificacion, bool defecto)
        {
            CoreGestionVistaID = coreGestionVistaID;
            Nombre = nombre;
            UsuarioID = usuarioID;
            JsonColumnas = jsonColumnas;
            JsonFiltros = jsonFiltros;
            Pagina = pagina;
            Icono = icono;
            FechaCreacion = fechaCreacion;
            FechaUltimaModificacion = fechaUltimaModificacion;
            Defecto = defecto;
        }

        protected ViewEntity()
        {
            
        }

        public static ViewEntity Create(string nombre, int usuarioID, string jsonColumnas, string jsonFiltros, string icono, DateTime fechaCreacion, DateTime fechaUltimaModificacion, string pagina, bool defecto)
            => new ViewEntity(null, nombre, usuarioID, jsonColumnas, jsonFiltros, pagina, icono, fechaCreacion, fechaUltimaModificacion, defecto);
        public static ViewEntity UpdateId(ViewEntity profile, int id) 
            => new ViewEntity(id, profile.Nombre, profile.UsuarioID, profile.JsonColumnas, profile.JsonFiltros, profile.Pagina, profile.Icono, profile.FechaCreacion, profile.FechaUltimaModificacion, profile.Defecto);
    }
}
