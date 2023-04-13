using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Threading.Tasks;
using TreeCore.BackEnd.Model.Entity.Contracts;
using TreeCore.BackEnd.Model.Entity.General;
using TreeCore.BackEnd.Model.Entity.Sites;
using TreeCore.BackEnd.Model.ValueObject;
using TreeCore.BackEnd.Service.Mappers;
using TreeCore.BackEnd.Service.Validations.Contracts;
using TreeCore.BackEnd.ServiceDependencies.Services;
using TreeCore.Shared.Data.Query;
using TreeCore.Shared.DTO.Contracts;
using TreeCore.Shared.DTO.General;
using TreeCore.Shared.DTO.Query;
using TreeCore.Shared.DTO.Sites;
using TreeCore.Shared.ROP;
using TreeCore.Shared.Utilities.Enum;
using Newtonsoft.Json;

namespace TreeCore.BackEnd.Service.Services.Contracts
{

    public class PutContract : PutObjectService<ContractDTO, ContractEntity, ContractDTOMapper>
    {
        private readonly GetDependencies<ContractDTO, ContractEntity> _getDependency;
        private readonly GetDependencies<SiteDTO, SiteEntity> _getDependencySites;
        private readonly GetDependencies<ContractStatusDTO, ContractStatusEntity> _getDependencyContractState;
        private readonly GetDependencies<ContractGroupDTO, ContractGroupEntity> _getDependencyContractGroup;
        private readonly GetDependencies<ContractTypeDTO, ContractTypeEntity> _getDependencyContractType;
        private readonly GetDependencies<CurrencyDTO, CurrencyEntity> _getDependencyCurrency;
        private readonly PutContractLine _putContractLineDependency;
        private readonly GetDependencies<UserDTO, UserEntity> _getUserDependency;



        public PutContract(PutDependencies<ContractEntity> putDependency, GetDependencies<ContractDTO, ContractEntity> getDependency,
            GetDependencies<SiteDTO, SiteEntity> getDependencySites,
            GetDependencies<ContractStatusDTO, ContractStatusEntity> getDependencyContractState,
            GetDependencies<ContractGroupDTO, ContractGroupEntity> getDependencyContractGroup,
            GetDependencies<ContractTypeDTO, ContractTypeEntity> getDependencyContractType,
            GetDependencies<CurrencyDTO, CurrencyEntity> getDependencyCurrency,
            GetDependencies<UserDTO, UserEntity> getUserDependency,
             PutContractLine putContractLineDependency,

        IHttpContextAccessor httpcontextAccessor) :
            base(httpcontextAccessor, putDependency, new ContractValidation())
        {
            _getDependency = getDependency;
            _getDependencySites = getDependencySites;
            _getDependencyContractState = getDependencyContractState;
            _getDependencyContractGroup = getDependencyContractGroup;
            _getDependencyContractType = getDependencyContractType;
            _getDependencyCurrency = getDependencyCurrency;
            _putContractLineDependency = putContractLineDependency;
            _getUserDependency = getUserDependency;



        }
        public override async Task<Result<ContractEntity>> ValidateEntity(ContractDTO ocontractDTO, int Client, string code, string email = "")
        {
            List<Filter> listFilters = new List<Filter>();
            List<Filter> listFiltersDefault = new List<Filter>();
            Filter filter;
            List<Error> lErrors = new List<Error>();

            SiteEntity osite = await _getDependencySites.GetItemByCode(ocontractDTO.SiteCode, Client);
            ContractStatusEntity oContractState = await _getDependencyContractState.GetItemByCode(ocontractDTO.ContractStatusCode, Client);
            ContractGroupEntity oContractGroup = await _getDependencyContractGroup.GetItemByCode(ocontractDTO.ContractGroupCode, Client);
            ContractTypeEntity oContractType = await _getDependencyContractType.GetItemByCode(ocontractDTO.ContractTypeCode, Client);
            CurrencyEntity oCurrency = await _getDependencyCurrency.GetItemByCode(ocontractDTO.CurrencyCode, Client);

            if (osite == null)
            {
                lErrors.Add(Error.Create(_traduccion.CodeSite + " " + $"{ocontractDTO.SiteCode}" + " " + _errorTraduccion.NotFound + "."));
            }
            if (oContractState == null && ocontractDTO.ContractStatusCode != "")
            {
                lErrors.Add(Error.Create(_traduccion.CodeContractState + " " + $"{ocontractDTO.ContractStatusCode}" + " " + _errorTraduccion.NotFound + "."));
            }
            if (oContractGroup == null && ocontractDTO.ContractGroupCode != "")
            {
                lErrors.Add(Error.Create(_traduccion.CodeContractGroup + " " + $"{ocontractDTO.ContractGroupCode}" + " " + _errorTraduccion.NotFound + "."));
            }
            if (oContractType == null && ocontractDTO.ContractTypeCode != "")
            {
                lErrors.Add(Error.Create(_traduccion.CodeContractType + " " + $"{ocontractDTO.ContractTypeCode}" + " " + _errorTraduccion.NotFound + "."));
            }
            if (oCurrency == null && ocontractDTO.CurrencyCode != "")
            {
                lErrors.Add(Error.Create(_traduccion.Currency + " " + $"{ocontractDTO.CurrencyCode}" + " " + _errorTraduccion.NotFound + "."));
            }
            UserEntity user = await _getUserDependency.GetItemByCode(email, Client);
            if (user == null)
            {
                lErrors.Add(Error.Create(_traduccion.Email + " " + $"{email}" + " " + _errorTraduccion.NotFound + "."));
            }

            var ContractIdentty = await _getDependency.GetItemByCode(code, Client);
           // ContractEntity? contract = await _getDependency.GetItemByCode(code, Client);
            if (ContractIdentty == null)
			{
				//lErrors.Add(Error.Create(_traduccion.CodeContract + " " + $"{ocontractDTO.Code}" + " " + _errorTraduccion.NotFound + "."));
                return Result.Failure<ContractEntity>(_traduccion.CodeContract + " " + $"{code}" + " " + _errorTraduccion.NotFound + ".");
			}

            #region VALIDACION HISTORICO
            
            string DatosJson = "";
            DateTime FechaModificacion = new DateTime();
            int? AlquilerID = null;

            Result<ContractEntity> contractAux = new Result<ContractEntity>();
            if (ContractIdentty != null)
            {
                contractAux = ContractIdentty;
                
                DatosJson = JsonConvert.SerializeObject((await contractAux.Async().MapAsync(x => _mapper.Map(x))).Valor);
                FechaModificacion = Convert.ToDateTime(ContractIdentty.FechaModificacion);
                
            }
            ContractHistoryEntity contractHistory = new ContractHistoryEntity(null, AlquilerID, true, FechaModificacion, "", false, false, DatosJson, user.UsuarioID);

            #endregion

         


            #region OBJETCT VALUE

            bool btipo = false;
            int? iMeses = null;
            int? iTotalProrrogas = null;
            int? iProrrogasConsumidas = null;
            int? iDiasNotificaciones = null;
            DateTime? dateFechaProrroga = null;
            DateTime? dateFechaNotificacionProrrogas = null;
            DateTime? dateFechaEfectivaFinContrato = null;

            Meses cMeses = new Meses();

            if (ocontractDTO.RenewalClause != null)
            {
                if (ocontractDTO.RenewalClause.Type == RenewalReadjustment.sAuto)
                {
                    if (ocontractDTO.RenewalClause.Frequencymonths == null || ocontractDTO.RenewalClause.Frequencymonths == 0)
                    {
                        lErrors.Add(Error.Create(_traduccion.CodeContractFrequencyMonth + " " + $"{ocontractDTO.RenewalClause.Frequencymonths}" + " " + _errorTraduccion.FormatError + "."));
                    }
                    if (ocontractDTO.RenewalClause.TotalRenewalNumber == null || ocontractDTO.RenewalClause.TotalRenewalNumber == 0)
                    {
                        lErrors.Add(Error.Create(_traduccion.CodeContractRenewalNumber + " " + $"{ocontractDTO.RenewalClause.TotalRenewalNumber}" + " " + _errorTraduccion.FormatError + "."));
                    }
                   
                    
                    
                    if(ocontractDTO.RenewalClause.TotalRenewalNumber!=null && ocontractDTO.RenewalClause.TotalRenewalNumber!=0 && ocontractDTO.RenewalClause.Frequencymonths!=null && ocontractDTO.RenewalClause.Frequencymonths != 0)
                    {
                        if (ocontractDTO.FirstEndDate > DateTime.Now)
                        {
                            iProrrogasConsumidas = 0;
                            dateFechaProrroga = ocontractDTO.FirstEndDate;
                        }
                        else
                        {
                             iProrrogasConsumidas = cMeses.GetMonthDiff2(ocontractDTO.FirstEndDate, DateTime.Now)/ ocontractDTO.RenewalClause.Frequencymonths;
                            if (iProrrogasConsumidas == 0 )
                            {
                                iProrrogasConsumidas = 1;
                            }
                            dateFechaProrroga = cMeses.sumarMesesAFecha(ocontractDTO.FirstEndDate, (int)(iProrrogasConsumidas * ocontractDTO.RenewalClause.Frequencymonths));
                            if(dateFechaProrroga< DateTime.Now)
                            {
                                iProrrogasConsumidas++;
                                dateFechaProrroga = cMeses.sumarMesesAFecha(ocontractDTO.FirstEndDate, (int)(iProrrogasConsumidas * ocontractDTO.RenewalClause.Frequencymonths));

                            }
                        }
                        iMeses = ocontractDTO.RenewalClause.Frequencymonths;
                        iTotalProrrogas = ocontractDTO.RenewalClause.TotalRenewalNumber;
                       
                        int? cantidadMeses = iTotalProrrogas * iMeses;
                        dateFechaEfectivaFinContrato = cMeses.sumarMesesAFecha(ocontractDTO.FirstEndDate, (int)cantidadMeses);
                    }
                    btipo = true;
                    if (ocontractDTO.RenewalClause.Renewalnotificationdate != null)
                    {
                        dateFechaNotificacionProrrogas = ocontractDTO.RenewalClause.Renewalnotificationdate;
                    }
                    if (ocontractDTO.RenewalClause.NotificationNumberDays != null)
                    {
                        iDiasNotificaciones = ocontractDTO.RenewalClause.NotificationNumberDays;
                    }
                    

                }
                else if (ocontractDTO.RenewalClause.Type == RenewalReadjustment.sOptional)
                {
                    if (ocontractDTO.RenewalClause.Frequencymonths == null || ocontractDTO.RenewalClause.Frequencymonths == 0)
                    {
                        lErrors.Add(Error.Create(_traduccion.CodeContractFrequencyMonth + " " + $"{ocontractDTO.RenewalClause.Frequencymonths}" + " " + _errorTraduccion.FormatError + "."));
                    }
                    if (ocontractDTO.RenewalClause.TotalRenewalNumber == null || ocontractDTO.RenewalClause.TotalRenewalNumber == 0)
                    {
                        lErrors.Add(Error.Create(_traduccion.CodeContractRenewalNumber + " " + $"{ocontractDTO.RenewalClause.TotalRenewalNumber}" + " " + _errorTraduccion.FormatError + "."));
                    }
                   
                    if (ocontractDTO.RenewalClause.TotalRenewalNumber != null && ocontractDTO.RenewalClause.TotalRenewalNumber != 0 && ocontractDTO.RenewalClause.Frequencymonths != null && ocontractDTO.RenewalClause.Frequencymonths != 0)
                    {
                        if (ocontractDTO.FirstEndDate > DateTime.Now)
                        {
                            iProrrogasConsumidas = 0;
                            dateFechaProrroga = ocontractDTO.FirstEndDate;
                        }
                        else
                        {
                            iProrrogasConsumidas = cMeses.GetMonthDiff2(ocontractDTO.FirstEndDate, DateTime.Now) / ocontractDTO.RenewalClause.Frequencymonths;
                            if (iProrrogasConsumidas == 0)
                            {
                                iProrrogasConsumidas = 1;
                            }
                            dateFechaProrroga = cMeses.sumarMesesAFecha(ocontractDTO.FirstEndDate, (int)(iProrrogasConsumidas * ocontractDTO.RenewalClause.Frequencymonths));

                        }
                        iMeses = ocontractDTO.RenewalClause.Frequencymonths;
                        iTotalProrrogas = ocontractDTO.RenewalClause.TotalRenewalNumber;

                        int? cantidadMeses = iTotalProrrogas * iMeses;
                        dateFechaEfectivaFinContrato = cMeses.sumarMesesAFecha(ocontractDTO.FirstEndDate, (int)cantidadMeses);
                    }
                    btipo = true;
                    if (ocontractDTO.RenewalClause.Renewalnotificationdate != null)
                    {
                        dateFechaNotificacionProrrogas = ocontractDTO.RenewalClause.Renewalnotificationdate;
                    }
                    if (ocontractDTO.RenewalClause.NotificationNumberDays != null)
                    {
                        iDiasNotificaciones = ocontractDTO.RenewalClause.NotificationNumberDays;
                    }
                }
                else if (ocontractDTO.RenewalClause.Type == RenewalReadjustment.sPreviousNegotiation)
                {
                    btipo = false;
                    iMeses = null;
                    iTotalProrrogas = null;
                    iProrrogasConsumidas = null;
                    iDiasNotificaciones = null;
                    dateFechaProrroga = null;
                    dateFechaNotificacionProrrogas = null;
                    dateFechaEfectivaFinContrato = null;

                }
                else
                {
                    if (ocontractDTO.RenewalClause.Type != null)
                    {
                        lErrors.Add(Error.Create(_traduccion.Prorrogas + " " + $"{ocontractDTO.RenewalClause.Type}" + " " + _errorTraduccion.FormatError + "."));
                    }
                }

            }
            else
            {
                btipo = false;
                iMeses = null;
                iTotalProrrogas = null;
                iProrrogasConsumidas = null;
                iDiasNotificaciones = null;
                dateFechaProrroga = null;
                dateFechaNotificacionProrrogas = null;
                dateFechaEfectivaFinContrato = null;
            }
            RenewalClause renew = new RenewalClause(btipo, iMeses, iTotalProrrogas, iProrrogasConsumidas, iDiasNotificaciones, dateFechaProrroga, dateFechaNotificacionProrrogas, dateFechaEfectivaFinContrato);
            #endregion

            #region VALIDACION CONTRACT LINES
            List<ContractLineEntity> listContractLine = new List<ContractLineEntity>();
            if (ocontractDTO.contractline != null && ocontractDTO.contractline.Count > 0)
            {
                Result<List<ContractLineEntity>> listContractLineValidity = await _putContractLineDependency.ValidateEntity(ocontractDTO.contractline, Client, ocontractDTO.Code);
                if (listContractLineValidity.Success)
                {
                    listContractLine = listContractLineValidity.Value;
                }
                else
                {
                    lErrors.AddRange(listContractLineValidity.Errors);
                }

            }
            #endregion
            ContractEntity contractEntity = new ContractEntity(ContractIdentty.AlquilerID, ocontractDTO.Code, ocontractDTO.Name,
                                                               oContractState != null ? oContractState : null,
                                                               oContractGroup != null ? oContractGroup : null,
                                                               osite != null ? osite : null,
                                                               oCurrency != null ? oCurrency : null,
                                                               oContractType != null ? oContractType : null,
                                                               ocontractDTO.Description,
                                                               null,
                                                               ocontractDTO.ExecutionDate,
                                                               ocontractDTO.StartDate,
                                                               ocontractDTO.FirstEndDate,
                                                               ((ocontractDTO.FirstEndDate.Year - ocontractDTO.StartDate.Year) * 12) + ocontractDTO.FirstEndDate.Month - ocontractDTO.StartDate.Month,
                                                               ocontractDTO.FirstEndDate,
                                                               ocontractDTO.ClosedAtExpiration,
                                                               "",
                                                               renew,
                                                               false,
                                                               listContractLine,
                                                               DateTime.Now, DateTime.Now, user, user, contractHistory);



            filter = new Filter(nameof(ContractDTO.Code), Operators.eq, ocontractDTO.Code);
            listFilters.Add(filter);

            Task<IEnumerable<ContractEntity>> listContracts = _getDependency.GetList(Client, listFilters, null, null, -1, -1);
            if (listContracts.Result != null && listContracts.Result.ListOrEmpty().Count > 0 &&
                listContracts.Result.ListOrEmpty()[0].AlquilerID != contractEntity.AlquilerID)
            {
                lErrors.Add(Error.Create(_traduccion.CodeContract + " " + $"{ocontractDTO.Code}" + " " + _errorTraduccion.AlreadyExist + "."));
            }


            if (lErrors.Count > 0)
            {
                return Result.Failure<ContractEntity>(ImmutableArray.Create(lErrors.ToArray()));
            }
            else
            {
                return contractEntity;
            }
        }

    }
}
