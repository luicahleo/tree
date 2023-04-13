using Dapper;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using TreeCore.BackEnd.Data.Repository.Query;
using TreeCore.BackEnd.Model.Entity.Contracts;
using TreeCore.BackEnd.Model.Entity.General;
using TreeCore.BackEnd.Model.Entity.Sites;
using TreeCore.Shared.Data.Db;
using TreeCore.Shared.Data.Query;
using TreeCore.BackEnd.Model.ValueObject;
using TreeCore.BackEnd.Data.Repository.General;

namespace TreeCore.BackEnd.Data.Repository.Contracts
{

    public class ContractRepository : BaseRepository<ContractEntity>
    {
        public override Table Table => TableNames.Contract;
        public Table TableContractGroup => TableNames.ContractGroup;
        public Table ContractType => TableNames.ContractType;
        public Table ContractState => TableNames.ContractState;
        public Table Sites => TableNames.Sites;
        public Table Currency => TableNames.Currency;
        private ContractLineRepository _contractLineRepository;
        private ContractLineTaxesRepository contractLineTaxesRepository;
        private ContractLineEntidadRepository contractLineEntidadesRepository;
        private UserRepository userRepository;
        private ContractHistoryRepository _contractHistoryRepository;

        public ContractRepository(TransactionalWrapper conexion) : base(conexion)
        {
            _contractLineRepository = new ContractLineRepository(conexion);
            contractLineTaxesRepository = new ContractLineTaxesRepository(conexion);
            contractLineEntidadesRepository = new ContractLineEntidadRepository(conexion);
            userRepository = new UserRepository(conexion);
            _contractHistoryRepository= new ContractHistoryRepository(conexion);
        }

        public override async Task<ContractEntity> InsertSingle(ContractEntity obj)
        {
            string sql = $"insert into {Table.Name} " +
                $"({nameof(ContractEntity.NumContrato)}," +
                $"{nameof(ContractEntity.NombreContrato)}, " +
                $"{nameof(ContractEntity.oAlquilerEstado.AlquilerEstadoID)}, " +
                $"{nameof(ContractEntity.oAlquilerTipoContrato.AlquilerTipoContratoID)}, " +
                $"{nameof(ContractEntity.oSite.EmplazamientoID)}, " +
                $"{ nameof(ContractEntity.oMoneda.MonedaID)}," +
                $"{nameof(ContractEntity.oAlquilerTipoContratacion.AlquilerTipoContratacionID)}," +
                $"{nameof(ContractEntity.ComentariosGenerales)}, " +
                $"{nameof(ContractEntity.ContratoMarcoID)}, " +
                $"{nameof(ContractEntity.FechaFirmaContrato)}, " +
                $"{nameof(ContractEntity.FechaInicioContrato)}, " +
                $"{ nameof(ContractEntity.FechaFinContrato)}," +
                $"{ nameof(ContractEntity.Cadencias)}," +
                $"{ nameof(ContractEntity.oAjusteRenovaciones.ProrrogaAutomatica)}," +
                $"{ nameof(ContractEntity.oAjusteRenovaciones.CadenciaProrrogaAutomatica)}," +
                $"{ nameof(ContractEntity.oAjusteRenovaciones.NumProrrogasMaximas)}," +
                $"{ nameof(ContractEntity.oAjusteRenovaciones.ProrrogasConsumidas)}," +
                $"{ nameof(ContractEntity.oAjusteRenovaciones.FechaFinContratoAuxiliar)}," +
                $"{ nameof(ContractEntity.oAjusteRenovaciones.FechaEfectivaFinContrato)}," +
                $"{nameof(ContractEntity.oAjusteRenovaciones.FechaNotificacionRenovacion)}," +
                $"{nameof(ContractEntity.oAjusteRenovaciones.NumdiasNotificacion)}," +
                $"{ nameof(ContractEntity.Propietario)}," +
                $"{nameof(ContractEntity.FechaCreacion)}, " +
                $"{nameof(ContractEntity.FechaModificacion)}, " +
                $"CreadoPorID, " +
                $"ModificadoPorID, " +
                $"{ nameof(ContractEntity.TerminacionPrevistaPrimeraVentana)})" +
                $"values(@{ nameof(ContractEntity.NumContrato)}," +
                $"@{ nameof(ContractEntity.NombreContrato)}," +
                $"@{nameof(ContractEntity.oAlquilerEstado.AlquilerEstadoID)}," +
                $"@{nameof(ContractEntity.oAlquilerTipoContrato.AlquilerTipoContratoID)}," +
                $"@{nameof(ContractEntity.oSite.EmplazamientoID)}, " +
                $"@{ nameof(ContractEntity.oMoneda.MonedaID)}," +
                $"@{nameof(ContractEntity.oAlquilerTipoContratacion.AlquilerTipoContratacionID)}," +
                $"@{nameof(ContractEntity.ComentariosGenerales)}," +
                $"@{nameof(ContractEntity.ContratoMarcoID)}," +
                $"@{nameof(ContractEntity.FechaFirmaContrato)}, " +
                $"@{nameof(ContractEntity.FechaInicioContrato)}, " +
                $"@{ nameof(ContractEntity.FechaFinContrato)}," +
                $"@{ nameof(ContractEntity.Cadencias)}," +
                $"@{nameof(ContractEntity.oAjusteRenovaciones.ProrrogaAutomatica)}," +
                $"@{nameof(ContractEntity.oAjusteRenovaciones.CadenciaProrrogaAutomatica)}," +
                $"@{nameof(ContractEntity.oAjusteRenovaciones.NumProrrogasMaximas)}," +
                $"@{nameof(ContractEntity.oAjusteRenovaciones.ProrrogasConsumidas)}," +
                $"@{nameof(ContractEntity.oAjusteRenovaciones.FechaFinContratoAuxiliar)}," +
                $"@{nameof(ContractEntity.oAjusteRenovaciones.FechaEfectivaFinContrato)}," +
                $"@{nameof(ContractEntity.oAjusteRenovaciones.FechaNotificacionRenovacion)}," +
                $"@{nameof(ContractEntity.oAjusteRenovaciones.NumdiasNotificacion)}," +
                $"@{ nameof(ContractEntity.Propietario)}," +
                $"@{nameof(ContractEntity.FechaCreacion)}, " +
                $"@{nameof(ContractEntity.FechaModificacion)}, " +
                $"@CreadoPorID, " +
                $"@ModificadoPorID, " +
                $"@{ nameof(ContractEntity.TerminacionPrevistaPrimeraVentana)});" +

               $"SELECT SCOPE_IDENTITY();";
            DbConnection connection = await _conexionWrapper.GetConnectionAsync();
            var sqlTran = await _conexionWrapper.GetTransactionAsync();
            var newId = (await connection.QueryAsync<int>(sql, new
            {
                NumContrato = obj.NumContrato,
                NombreContrato = obj.NombreContrato,
                AlquilerEstadoID = obj.oAlquilerEstado.AlquilerEstadoID,
                AlquilerTipoContratoID = obj.oAlquilerTipoContrato.AlquilerTipoContratoID,
                EmplazamientoID = obj.oSite.EmplazamientoID,
                MonedaID = obj.oMoneda.MonedaID,
                AlquilerTipoContratacionID = obj.oAlquilerTipoContratacion.AlquilerTipoContratacionID,
                ComentariosGenerales = obj.ComentariosGenerales,
                ContratoMarcoID = obj.ContratoMarcoID,
                FechaFirmaContrato = obj.FechaFirmaContrato,
                FechaInicioContrato = obj.FechaInicioContrato,
                FechaFinContrato = obj.FechaFinContrato,
                Cadencias = obj.Cadencias,
                Propietario = obj.Propietario,
                ProrrogaAutomatica = obj.oAjusteRenovaciones.ProrrogaAutomatica,
                CadenciaProrrogaAutomatica = obj.oAjusteRenovaciones.CadenciaProrrogaAutomatica,
                NumProrrogasMaximas = obj.oAjusteRenovaciones.NumProrrogasMaximas,
                ProrrogasConsumidas = obj.oAjusteRenovaciones.ProrrogasConsumidas,
                FechaFinContratoAuxiliar = obj.oAjusteRenovaciones.FechaFinContratoAuxiliar,
                FechaEfectivaFinContrato = obj.oAjusteRenovaciones.FechaEfectivaFinContrato,
                FechaNotificacionRenovacion = obj.oAjusteRenovaciones.FechaNotificacionRenovacion,
                NumdiasNotificacion = obj.oAjusteRenovaciones.NumdiasNotificacion,
                FechaCreacion = obj.FechaCreacion,
                FechaModificacion = obj.FechaModificacion,
                CreadoPorID = (obj.CreadoPor != null) ? obj.CreadoPor.UsuarioID : null,
                ModificadoPorID = (obj.ModificadoPor != null) ? obj.ModificadoPor.UsuarioID : null,
                terminacionPrevistaPrimeraVentana = obj.TerminacionPrevistaPrimeraVentana,


            }, sqlTran)).First();

            IEnumerable<ContractLineEntity> result = await _contractLineRepository.InsertList(obj.oAlquileresDetalles, newId.ToString());

            return ContractEntity.UpdateId(obj, newId);
        }
        public override async Task<ContractEntity> UpdateSingle(ContractEntity obj)
        {
            string sql = $"update {Table.Name} set {nameof(ContractEntity.NumContrato)} = @{nameof(ContractEntity.NumContrato)}, " +
                $" {nameof(ContractEntity.NombreContrato)} =   @{nameof(ContractEntity.NombreContrato)}, " +
                $" {nameof(ContractEntity.oAlquilerEstado.AlquilerEstadoID)} =   @{nameof(ContractEntity.oAlquilerEstado.AlquilerEstadoID)}, " +
                $" {nameof(ContractEntity.oAlquilerTipoContratacion.AlquilerTipoContratacionID)} =   @{nameof(ContractEntity.oAlquilerTipoContratacion.AlquilerTipoContratacionID)}, " +
                $" {nameof(ContractEntity.oSite.EmplazamientoID)} =   @{nameof(ContractEntity.oSite.EmplazamientoID)}, " +
                $" {nameof(ContractEntity.oMoneda.MonedaID)} =   @{nameof(ContractEntity.oMoneda.MonedaID)}, " +
                $" {nameof(ContractEntity.oAlquilerTipoContrato.AlquilerTipoContratoID)} =   @{nameof(ContractEntity.oAlquilerTipoContrato.AlquilerTipoContratoID)}, " +
                $" {nameof(ContractEntity.ComentariosGenerales)} =   @{nameof(ContractEntity.ComentariosGenerales)}, " +
                $" {nameof(ContractEntity.ContratoMarcoID)} =   @{nameof(ContractEntity.ContratoMarcoID)}, " +
                $" {nameof(ContractEntity.FechaFirmaContrato)} =   @{nameof(ContractEntity.FechaFirmaContrato)}, " +
                $" {nameof(ContractEntity.FechaInicioContrato)} =   @{nameof(ContractEntity.FechaInicioContrato)}, " +
                $" {nameof(ContractEntity.FechaFinContrato)} =   @{nameof(ContractEntity.FechaFinContrato)}, " +
                $" ModificadoPorID =   @ModificadoPorID, " +
                $" {nameof(ContractEntity.FechaModificacion)} =   @{nameof(ContractEntity.FechaModificacion)}, " +
                $" {nameof(ContractEntity.oAjusteRenovaciones.ProrrogaAutomatica)} =   @{nameof(ContractEntity.oAjusteRenovaciones.ProrrogaAutomatica)}, " +
                $" {nameof(ContractEntity.oAjusteRenovaciones.CadenciaProrrogaAutomatica)} =   @{nameof(ContractEntity.oAjusteRenovaciones.CadenciaProrrogaAutomatica)}, " +
                $" {nameof(ContractEntity.oAjusteRenovaciones.NumProrrogasMaximas)} =   @{nameof(ContractEntity.oAjusteRenovaciones.NumProrrogasMaximas)}, " +
                $" {nameof(ContractEntity.oAjusteRenovaciones.ProrrogasConsumidas)} =   @{nameof(ContractEntity.oAjusteRenovaciones.ProrrogasConsumidas)}, " +
                $" {nameof(ContractEntity.oAjusteRenovaciones.FechaFinContratoAuxiliar)} =   @{nameof(ContractEntity.oAjusteRenovaciones.FechaFinContratoAuxiliar)}, " +
                $" {nameof(ContractEntity.oAjusteRenovaciones.FechaEfectivaFinContrato)} =   @{nameof(ContractEntity.oAjusteRenovaciones.FechaEfectivaFinContrato)}, " +
                $" {nameof(ContractEntity.oAjusteRenovaciones.FechaNotificacionRenovacion)} =   @{nameof(ContractEntity.oAjusteRenovaciones.FechaNotificacionRenovacion)}, " +
                $" {nameof(ContractEntity.oAjusteRenovaciones.NumdiasNotificacion)} =   @{nameof(ContractEntity.oAjusteRenovaciones.NumdiasNotificacion)} " +
                $" where {nameof(ContractEntity.AlquilerID)} = @{nameof(ContractEntity.AlquilerID)} ";



            DbConnection connection = await _conexionWrapper.GetConnectionAsync();

            var sqlTran = await _conexionWrapper.GetTransactionAsync();
            IEnumerable<ContractLineEntity> result = await _contractLineRepository.UpdateList(obj.oAlquileresDetalles, obj.AlquilerID.ToString());
            var filasafectadas = (await connection.ExecuteAsync(sql, new
            {
                AlquilerID = obj.AlquilerID,
                NumContrato = obj.NumContrato,
                NombreContrato = obj.NombreContrato,
                AlquilerEstadoID = obj.oAlquilerEstado.AlquilerEstadoID,
                AlquilerTipoContratoID = obj.oAlquilerTipoContrato.AlquilerTipoContratoID,
                EmplazamientoID = obj.oSite.EmplazamientoID,
                MonedaID = obj.oMoneda.MonedaID,
                AlquilerTipoContratacionID = obj.oAlquilerTipoContratacion.AlquilerTipoContratacionID,
                ComentariosGenerales = obj.ComentariosGenerales,
                ContratoMarcoID = obj.ContratoMarcoID,
                FechaFirmaContrato = obj.FechaFirmaContrato,
                FechaInicioContrato = obj.FechaInicioContrato,
                FechaFinContrato = obj.FechaFinContrato,
                Cadencias = obj.Cadencias,
                Propietario = obj.Propietario,
                ProrrogaAutomatica = obj.oAjusteRenovaciones.ProrrogaAutomatica,
                CadenciaProrrogaAutomatica = obj.oAjusteRenovaciones.CadenciaProrrogaAutomatica,
                NumProrrogasMaximas = obj.oAjusteRenovaciones.NumProrrogasMaximas,
                ProrrogasConsumidas = obj.oAjusteRenovaciones.ProrrogasConsumidas,
                FechaFinContratoAuxiliar = obj.oAjusteRenovaciones.FechaFinContratoAuxiliar,
                FechaEfectivaFinContrato = obj.oAjusteRenovaciones.FechaEfectivaFinContrato,
                FechaNotificacionRenovacion = obj.oAjusteRenovaciones.FechaNotificacionRenovacion,
                NumdiasNotificacion = obj.oAjusteRenovaciones.NumdiasNotificacion,
                FechaCreacion = obj.FechaCreacion,
                FechaModificacion = obj.FechaModificacion,
                ModificadoPorID = (obj.ModificadoPor != null) ? obj.ModificadoPor.UsuarioID : null,
                terminacionPrevistaPrimeraVentana = obj.TerminacionPrevistaPrimeraVentana,

            }, sqlTran));

            ContractHistoryEntity contracthistoryresult = await _contractHistoryRepository.InsertSingleByContractID(obj.HistorialContrato, obj.AlquilerID.ToString());
            return obj;
        }

        public override async Task<ContractEntity> GetItemByCode(string code, int Client)
        {
            DbConnection connection = await _conexionWrapper.GetConnectionAsync();
            var sqlTran = await _conexionWrapper.GetTransactionAsync();
            string sql = $"select C.NombreContrato as NombreContrato,C.ComentariosGenerales as ComentariosGenerales,C.FechaFirmaContrato as FechaFirmaContrato,C.FechaInicioContrato as FechaInicioContrato,C.FechaFinContrato as FechaFinContrato,C.FechaCreacion as FechaCreacion,C.FechaModificacion as FechaModificacion,* from {Table.Name} C " +
                $"inner join {TableContractGroup.Name} CT on C.{TableContractGroup.ID} = CT.{TableContractGroup.ID} " +
                $"left join {ContractType.Name} TP on C.{ContractType.ID} = TP.{ContractType.ID} " +
                $"left join {ContractState.Name} TI on C.{ContractState.ID}= TI.{ContractState.ID} " +
                $"left join {Sites.Name} TS on C.{Sites.ID}= TS.{Sites.ID} " +
                $"left join {Currency.Name} TM on C.{Currency.ID}= TM.{Currency.ID} " +
                $" where C.{Table.Code} = @code";

            var result = await connection.QueryAsync<ContractEntity, RenewalClause,  ContractGroupEntity, ContractTypeEntity, ContractStatusEntity, SiteEntity, CurrencyEntity, ContractEntity>(sql,
                 (contractEntity, renewalClause,  contractGroupEntity, contractTypeEntity, contractStateEntity, siteEntity, currencyEntity) =>
                 {
                     contractEntity.oAlquilerTipoContratacion = contractGroupEntity;

                     if (renewalClause != null)
                     {
                         contractEntity.oAjusteRenovaciones = renewalClause;
                     }
                     contractEntity.oAlquilerTipoContrato = contractTypeEntity;
                     contractEntity.oAlquilerEstado = contractStateEntity;
                     contractEntity.oSite = siteEntity;

                     contractEntity.oMoneda = currencyEntity;


                     return contractEntity;
                 },
                 new { code = code }, sqlTran, true, splitOn: $"ProrrogaAutomatica,{TableContractGroup.ID},{ContractType.ID},{ContractState.ID},{Sites.ID},{Currency.ID}");

            if (result.FirstOrDefault() != null)
            {
                result.FirstOrDefault().oAlquileresDetalles = _contractLineRepository.GetListbyContractDetails(result.FirstOrDefault().AlquilerID.Value).Result;
                result.FirstOrDefault().CreadoPor = userRepository.GetItemByCompany(result.FirstOrDefault().AlquilerID.ToString(), Table, "CreadoPorID").Result;
                result.FirstOrDefault().ModificadoPor = userRepository.GetItemByCompany(result.FirstOrDefault().AlquilerID.ToString(), Table, "ModificadoPorID").Result;
            }
            
            return result.FirstOrDefault();
        }

        public override async Task<int> Delete(ContractEntity obj)
        {
            DbConnection connection = await _conexionWrapper.GetConnectionAsync();
            var sqlTran = await _conexionWrapper.GetTransactionAsync();
            IEnumerable<ContractLineEntity> contractline;

            contractline = await _contractLineRepository.GetListbyContractDetails(obj.AlquilerID.Value);
            foreach (ContractLineEntity item in contractline)
            {
                var resultlinetax = await contractLineTaxesRepository.DeleteByContractLine((int)item.AlquilerDetalleID.Value);
                var resultlineentidad = await contractLineEntidadesRepository.DeleteByContractLine((int)item.AlquilerDetalleID.Value);
            }
            var resultline = await _contractLineRepository.DeleteByContract((int)obj.AlquilerID);
            string sql = $"delete from {Table.Name} Where {Table.ID} = @key";
            int numReg;
            try
            {
                numReg = await connection.ExecuteAsync(sql, new
                {
                    key = obj.AlquilerID
                }, sqlTran);
            }
            catch (System.Exception)
            {
                numReg = 0;
            }
            return numReg;
        }


        public override async Task<IEnumerable<ContractEntity>> GetList(int Client, List<Filter> filters, List<string> orders, string direction, int pageSize = -1, int pageIndex = -1)
        {
            DbConnection connection = await _conexionWrapper.GetConnectionAsync();
            var sqlTran = await _conexionWrapper.GetTransactionAsync();
            Dictionary<string, object> dicParam = new Dictionary<string, object>();
            int countFilter = 0;

            string sqlRelation = $" C inner join {TableContractGroup.Name} CG on C.{TableContractGroup.ID} = CG.{TableContractGroup.ID} " +
                $"left join {ContractType.Name} CT on C.{ContractType.ID} = CT.{ContractType.ID} " +
                $"left join {ContractState.Name} CS on C.{ContractState.ID}= CS.{ContractState.ID} " +
                $"left join {Sites.Name} S on C.{Sites.ID}= S.{Sites.ID} " +
                $"left join {Currency.Name} M on C.{Currency.ID}= M.{Currency.ID} ";

            string sqlFilter = "";
            if (filters != null && filters.Count > 0)
            {
                sqlFilter = " WHERE ";
                sqlFilter += Filter.BuilFilters(filters, Filter.Types.AND, countFilter, out countFilter, dicParam, out dicParam, "C");
            }

            string sqlOrder = "";
            if (orders != null && orders.Count > 0)
            {
                foreach (string column in orders)
                {
                    sqlOrder += $"{(string.IsNullOrEmpty(sqlOrder) ? " order by C." : ", ")}{column}";
                }
            }
            else
            {
                sqlOrder = $" order by C.{Table.Code} ";
            }

            string sqlPagination = "";
            if (pageSize != -1 && pageIndex != -1)
            {
                sqlPagination = $"OFFSET {(pageIndex - 1) * pageSize} ROWS FETCH NEXT {pageSize} ROWS ONLY";
            }

            string query = $"select TotalItems = COUNT(*) OVER(), C.NombreContrato as NombreContrato,C.ComentariosGenerales as ComentariosGenerales,C.FechaFirmaContrato as FechaFirmaContrato,C.FechaInicioContrato as FechaInicioContrato,C.FechaFinContrato as FechaFinContrato,C.FechaCreacion as FechaCreacion,C.FechaModificacion as FechaModificacion, * from {Table.Name} {sqlRelation} {sqlFilter} {sqlOrder} {sqlPagination}";

            var result = await connection.QueryAsync<ContractEntity, RenewalClause, ContractGroupEntity, ContractTypeEntity, ContractStatusEntity, SiteEntity, CurrencyEntity, ContractEntity>(query,
                 (contractEntity, renewalClause, contractGroupEntity, contractTypeEntity, contractStateEntity, siteEntity, currencyEntity) =>
                 {
                     contractEntity.oAlquilerTipoContratacion = contractGroupEntity;
                     if (renewalClause != null)
                     {
                         contractEntity.oAjusteRenovaciones = renewalClause;
                     }
                     contractEntity.oAlquilerTipoContrato = contractTypeEntity;
                     contractEntity.oAlquilerEstado = contractStateEntity;

                     contractEntity.oSite = siteEntity;
                     contractEntity.oMoneda = currencyEntity;
                     return contractEntity;
                 },
                 dicParam, sqlTran, true, splitOn: $"ProrrogaAutomatica,{TableContractGroup.ID},{ContractType.ID},{ContractState.ID},{Sites.ID},{Currency.ID}");

            string ids = "";
            result.ToList().ForEach(x =>
            {
                ids += $"{(string.IsNullOrEmpty(ids) ? "" : ",")}{x.AlquilerID}";

            });
            if (result != null && result.ToList().Count > 0)
            {
                result = await _contractLineRepository.GetListbyContractDetails(result, ids);
            }
                
            //IEnumerable<ContractLineEntity> contractline;
            //foreach (ContractEntity contract in result)
            //{
            //    contractline = await _contractLineRepository.GetListbyContractDetails(contract.AlquilerID.Value);
            //    if (contractline != null)
            //    {
            //        contract.oAlquileresDetalles = contractline;
            //        IEnumerable<ContractLineTaxesEntity> contractlinetax;
            //        foreach (ContractLineEntity item in contractline)
            //        {
            //            contractlinetax = await contractLineTaxesRepository.GetListbyContractLineDetails(item.AlquilerDetalleID.Value);
            //            if (contractlinetax != null)
            //            {
            //                item.oAlquileresDetallesImpuestos = contractlinetax;
            //            }
            //        }
            //        IEnumerable<ContractLineEntidadEntity> contractlineentidad;
            //        foreach (ContractLineEntity item in contractline)
            //        {
            //            contractlineentidad = await contractLineEntidadesRepository.GetListbyContractLineDetails(item.AlquilerDetalleID.Value);
            //            if (contractlineentidad != null)
            //            {
            //                item.oAlquileresDetallesEntidades = contractlineentidad;
            //            }
            //        }
            //    }


            //}
            UserEntity userCreate;
            UserEntity userModify;
            foreach (ContractEntity contract in result)
            {


                userCreate = await userRepository.GetItemByCompany(contract.AlquilerID.ToString(), Table, "CreadoPorID");
                userModify = await userRepository.GetItemByCompany(contract.AlquilerID.ToString(), Table, "ModificadoPorID");
                if (userCreate != null)
                {
                    contract.CreadoPor = userCreate;

                }
                if (userModify != null)
                {
                    contract.ModificadoPor = userModify;
                }


            }

            return result.ToList();
        }
    }
}
