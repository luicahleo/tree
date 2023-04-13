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

    public class PutView : PutObjectService<ViewDTO, ViewEntity, ViewDTOMapper>
    {
        private readonly GetDependencies<ViewDTO, ViewEntity> _getDependency;
        private readonly GetDependencies<UserDTO, UserEntity> _getUserDependency;

        public PutView(PutDependencies<ViewEntity> putDependency,
            GetDependencies<ViewDTO, ViewEntity> getDependency,
            GetDependencies<UserDTO, UserEntity> getUserDependency,
            IHttpContextAccessor httpcontextAccessor) :
            base(httpcontextAccessor, putDependency, new ViewValidation())
        {
            _getDependency = getDependency;
            _getUserDependency = getUserDependency;
        }
        public override async Task<Result<ViewEntity>> ValidateEntity(ViewDTO view, int Client, string code, string email)
        {
            List<Error> lErrors = new List<Error>();
            IEnumerable<ViewEntity> listView;
            List<Filter> listFilters = new List<Filter>();
            List<Filter> listFiltersDefault = new List<Filter>();
            Filter filter;

            var user = await _getUserDependency.GetItemByCode(email, Client);
            //ViewEntity? taxpayerType = await _getDependency.GetItemByCode(code, Client);
            //ViewEntity viewFinal = new ViewEntity(taxpayerType.CoreGestionVistaID, view.Code, taxpayerType.UsuarioID, JsonConvert.SerializeObject(view.Columns), JsonConvert.SerializeObject(view.Filters), view.Page, view.Default);

            filter = new Filter(nameof(ViewDTO.Code), Operators.eq, code);
            listFilters.Add(filter);
            listFilters.Add(new Filter(nameof(ViewEntity.UsuarioID), Operators.eq, user.UsuarioID));
            listFilters.Add(new Filter(nameof(ViewDTO.Page), Operators.eq, view.Page));
            listView = await _getDependency.GetList(Client, listFilters, null, null, -1, -1);

            ViewEntity? taxpayerType = listView.First();
            ViewEntity viewFinal = new ViewEntity(taxpayerType.CoreGestionVistaID, view.Code, taxpayerType.UsuarioID, JsonConvert.SerializeObject(view.Columns), JsonConvert.SerializeObject(view.Filters), view.Page, view.Icon, taxpayerType.FechaCreacion, DateTime.Now, view.Default);

            if (listView != null && listView.ListOrEmpty().Count > 1)
            {
                lErrors.Add(Error.Create(_traduccion.CodeView + " " + $"{view.Code}" + " " + _errorTraduccion.AlreadyExist + "."));
            }

            if (view.Default)
            {
                filter = new Filter(nameof(ViewDTO.Default), Operators.eq, true);
                listFiltersDefault.Add(filter);
                listFiltersDefault.Add(new Filter(nameof(ViewEntity.UsuarioID), Operators.eq, user.UsuarioID));
                listFiltersDefault.Add(new Filter(nameof(ViewDTO.Page), Operators.eq, view.Page));

                listView = await _getDependency.GetList(Client, listFiltersDefault, null, null, -1, -1);
                if (listView != null && listView.ToList().Count > 0 && listView.FirstOrDefault().CoreGestionVistaID != viewFinal.CoreGestionVistaID)
                {
                    var Deft = listView.FirstOrDefault();
                    ViewEntity pType = new ViewEntity(Deft.CoreGestionVistaID, Deft.Nombre, Deft.UsuarioID, Deft.JsonColumnas, Deft.JsonFiltros, Deft.Pagina, Deft.Icono, Deft.FechaCreacion, Deft.FechaUltimaModificacion, false); ;
                    await _putDependencies.Update(pType);
                }
            }
            if (lErrors.Count > 0)
            {
                return Result.Failure<ViewEntity>(ImmutableArray.Create(lErrors.ToArray()));
            }
            return viewFinal;
        }
    }
}
