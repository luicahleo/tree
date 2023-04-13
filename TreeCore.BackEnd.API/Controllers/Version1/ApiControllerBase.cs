using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Reflection;
using System.Security.Claims;
using System.Threading.Tasks;
using TreeCore.BackEnd.Model.Entity;
using TreeCore.BackEnd.Service.Mappers;
using TreeCore.BackEnd.Service.Services;
using TreeCore.Shared.Data.Query;
using TreeCore.Shared.DTO;
using TreeCore.Shared.DTO.Query;
using TreeCore.Shared.ROP;
using TreeCore.Shared.Utilities.Auth;

namespace TreeCore.BackEnd.API.Controllers
{
    //[Authorize]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ApiControllerBase<DTO, Entity, Mapper> : ControllerBase, IGetController<DTO>
        where DTO : BaseDTO
        where Entity : BaseEntity
        where Mapper : BaseMapper<DTO, Entity>
    {
        protected readonly GetObjectService<DTO, Entity, Mapper> _getObjectService;

        public ApiControllerBase(GetObjectService<DTO, Entity, Mapper> getObjectService)
        {
            _getObjectService = getObjectService;
        }

        /// <summary>
        /// Get list of objects
        /// </summary>
        /// <param name="query"></param>
        /// <returns>List of objects</returns>
        [HttpGet()]
        public virtual async Task<ResultDto<IEnumerable<DTO>>> GetList([FromBody] QueryDTO query)
        {
            List<Filter> filters = new List<Filter>();
            List<string> orders = new List<string>();

            if (query != null && query.Filters != null)
            {
                query.Filters.ForEach(q => {

                    filters.Add(FilterDTOToFilter(q));

                });

                query.Order.ForEach(o => {
                    PropertyInfo pInfoField = typeof(DTO).GetProperty(o, BindingFlags.Instance | BindingFlags.Public | BindingFlags.IgnoreCase);

                    if (pInfoField != null)
                    {
                        orders.Add(pInfoField.Name.ToLower());
                    }
                });
            }

            return (await _getObjectService.GetList(Client, EmailUser, filters, orders, query.Direction, query.pageSize, query.pageIndex)).MapDto(x => x);
        }


        protected Filter FilterDTOToFilter(FilterDTO fDto)
        {
            Filter result = null;
            

            if (fDto.Filters != null && fDto.Filters.Count > 0 && fDto.Filters[0] != null)
            {
                result = new Filter(fDto.Type, new List<Filter>());
                fDto.Filters.ForEach(f => {
                    result.Filters.Add(FilterDTOToFilter(f));
                });
            }
            else if (!string.IsNullOrEmpty(fDto.Field) && !string.IsNullOrEmpty(fDto.Operator))
            {
                PropertyInfo pInfoField = typeof(DTO).GetProperty(fDto.Field, BindingFlags.Instance | BindingFlags.Public | BindingFlags.IgnoreCase);
                FieldInfo finfoOperator = typeof(Operators).GetField(fDto.Operator);
                if (pInfoField != null && finfoOperator != null)
                {
                    string filed = pInfoField.Name.ToLower();
                    string Operator = finfoOperator.GetRawConstantValue().ToString();

                    result = new Filter(filed.ToLower(), Operator, fDto.Value, fDto.Type, null);
                }
            }

            return result;
        }


        /// <summary>
        /// Get object by code
        /// </summary>
        /// <param name="code">Code of object</param>
        /// <returns>{Controller}</returns>
        [HttpGet("{code}")]
        public virtual async Task<ResultDto<DTO>> Get(string code)
        {
            return (await _getObjectService.GetItemByCode(code, Client)).MapDto(x => x);
        }

        /// <summary>
        /// Get Client ID of user
        /// </summary>
        protected int Client
        {
            get
            {
                int ClienteID = -1;
                var identity = HttpContext.User.Identity as ClaimsIdentity;
                if (identity != null)
                {
                    IEnumerable<Claim> claims = identity.Claims;
                    // or
                    var sClienteID = identity.FindFirst(JwtParams.CLIENT_ID).Value;

                    if (!int.TryParse(sClienteID, out ClienteID))
                    {
                        throw new System.Exception("clientNotFound");
                    }
                }

                return ClienteID;
            }
        }

        /// <summary>
        /// Get Email of user
        /// </summary>
        protected string EmailUser
        {
            get
            {
                string EmailUser = null;
                var identity = HttpContext.User.Identity as ClaimsIdentity;
                if (identity != null)
                {
                    EmailUser = identity.FindFirst(ClaimTypes.Email).Value;
                }
                return EmailUser;
            }
        }

    }
}
