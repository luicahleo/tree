using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Threading.Tasks;
using TreeCore.BackEnd.Model.Entity.Config;
using TreeCore.BackEnd.Model.Entity.General;
using TreeCore.BackEnd.Service.Mappers.Config;
using TreeCore.BackEnd.Service.Mappers.General;
using TreeCore.BackEnd.Service.Validations;
using TreeCore.BackEnd.ServiceDependencies.Services;
using TreeCore.Shared.Data.Query;
using TreeCore.Shared.DTO.Config;
using TreeCore.Shared.DTO.General;
using TreeCore.Shared.DTO.Query;
using TreeCore.Shared.ROP;
using Newtonsoft.Json;
using System.Linq;
using System;

namespace TreeCore.BackEnd.Service.Services.Config
{
    public class PostView : PostObjectService<ViewDTO, ViewEntity, ViewDTOMapper>
    {

        private readonly GetDependencies<ViewDTO, ViewEntity> _getDependency;
        private readonly PutDependencies<ViewEntity> _putDependency;
        private readonly GetDependencies<UserDTO, UserEntity> _getUserDependency;

        public PostView(PostDependencies<ViewEntity> postDependency,
            GetDependencies<ViewDTO, ViewEntity> getDependency,
            PutDependencies<ViewEntity> putDependency,
            GetDependencies<UserDTO, UserEntity> getUserDependency,
            IHttpContextAccessor httpcontextAccessor) : base(httpcontextAccessor, postDependency, new ViewValidation())
        {
            _getDependency = getDependency;
            _putDependency = putDependency;
            _getUserDependency = getUserDependency;
        }

        public override async Task<Result<ViewEntity>> ValidateEntity(ViewDTO oEntidad, int Client, string email, string code = "")
        {
            List<Error> lErrors = new List<Error>();
            List<Filter> listFilters = new List<Filter>();
            List<Filter> listFiltersDefault = new List<Filter>();
            var user = await _getUserDependency.GetItemByCode(email, Client);
            Filter filter;
            ViewEntity banco = ViewEntity.Create(oEntidad.Code, (int)user.UsuarioID, JsonConvert.SerializeObject(oEntidad.Columns), JsonConvert.SerializeObject(oEntidad.Filters), oEntidad.Icon, DateTime.Now, DateTime.Now, oEntidad.Page, oEntidad.Default);

            filter = new Filter(nameof(ViewDTO.Code), Operators.eq, oEntidad.Code);
            listFilters.Add(filter);
            listFilters.Add(new Filter(nameof(ViewEntity.UsuarioID), Operators.eq, user.UsuarioID));
            listFilters.Add(new Filter(nameof(ViewDTO.Page), Operators.eq, oEntidad.Page));
            Task<IEnumerable<ViewEntity>> listView = _getDependency.GetList(Client, listFilters, null, null, -1, -1);
            if (listView.Result != null && listView.Result.ListOrEmpty().Count > 0)
            {
                lErrors.Add(Error.Create(_traduccion.CodeView + " " + $"{oEntidad.Code}" + " " + _errorTraduccion.AlreadyExist + "."));
            }
            else
            {
                if (oEntidad.Default)
                {
                    filter = new Filter(nameof(ViewDTO.Default), Operators.eq, true);
                    listFiltersDefault.Add(filter);
                    listFiltersDefault.Add(new Filter(nameof(ViewEntity.UsuarioID), Operators.eq, user.UsuarioID));
                    listFiltersDefault.Add(new Filter(nameof(ViewDTO.Page), Operators.eq, oEntidad.Page));

                    listView = _getDependency.GetList(Client, listFiltersDefault, null, null, -1, -1);
                    if (listView.Result != null && listView.Result.ListOrEmpty().Count > 0)
                    {
                        var Deft = listView.Result.FirstOrDefault();
                        ViewEntity pType = new ViewEntity(Deft.CoreGestionVistaID, Deft.Nombre, Deft.UsuarioID, Deft.JsonColumnas, Deft.JsonFiltros, Deft.Pagina, Deft.Icono, Deft.FechaCreacion, Deft.FechaUltimaModificacion, false); ;
                        await _putDependency.Update(pType);
                    }
                }
            }
            if (lErrors.Count > 0)
            {
                return Result.Failure<ViewEntity>(ImmutableArray.Create(lErrors.ToArray()));
            }
            {
                return banco;
            }
        }
    }
}