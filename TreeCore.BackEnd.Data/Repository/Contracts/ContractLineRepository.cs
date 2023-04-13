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

namespace TreeCore.BackEnd.Data.Repository.Contracts
{

    public class ContractLineRepository : BaseRepository<ContractLineEntity>
    {
        public override Table Table => TableNames.ContractLine;
        public Table Contract => TableNames.Contract;
        public Table ContractLineType => TableNames.ContractLineType;

        public Table Currency => TableNames.Currency;

        private ContractLineTaxesRepository contractLineTaxesRepository;
        private ContractLineEntidadRepository contractLineEntidadesRepository;
        public ContractLineRepository(TransactionalWrapper conexion) : base(conexion)
        {
            contractLineTaxesRepository = new ContractLineTaxesRepository(conexion);
            contractLineEntidadesRepository = new ContractLineEntidadRepository(conexion);
        }

        public override async Task<ContractLineEntity> InsertSingle(ContractLineEntity obj)
        {
            string sql = $"insert into {Table.Name} " +
                $"({nameof(ContractLineEntity.CodigoDetalle)}," +
                //$"{nameof(ContractLineEntity.Nombre)}, " +
                $"{nameof(ContractLineEntity.oAlquiler.AlquilerID)}, " +
                $"{nameof(ContractLineEntity.oAlquilerConcepto.AlquilerConceptoID)}, " +
                $"{ nameof(ContractLineEntity.Periodicidad)}," +
                $"{nameof(ContractLineEntity.Importe)}," +
                $"{nameof(ContractLineEntity.oMoneda.MonedaID)}, " +
                $"{nameof(ContractLineEntity.Descripcion)}, " +
                $"{nameof(ContractLineEntity.FechaPrimerPago)}, " +
                $"{nameof(ContractLineEntity.FechaUltimoPago)}, " +
                $"{ nameof(ContractLineEntity.FechaProximoPago)}," +
                $"{ nameof(ContractLineEntity.NumeroCuotas)}," +
                $"{ nameof(ContractLineEntity.TotalImporte)}," +
                $"{ nameof(ContractLineEntity.TotalConProrrogas)}," +
                $"{ nameof(ContractLineEntity.AplicaProrrogaAutomatica)}," +
                $"{ nameof(ContractLineEntity.Prorrateo)}," +
                $"{ nameof(ContractLineEntity.PagoAnticipado)}," +

            #region REAJUSTE
                $"TipoReajuste," +
                $"{ nameof(ContractLineEntity.oReajuste.Inflacion.InflacionID)}," +
                $"{ nameof(ContractLineEntity.oReajuste.CantidadFija)}," +
                $"{ nameof(ContractLineEntity.oReajuste.PorcentajeFijo)}," +
                $"AjustesPeriodicidad," +
                $"FechaPrimeraRevision," +
                $"FechaUltimaRevision," +
                $"FechaProximaRevision)" +
            #endregion
                $"values(@{ nameof(ContractLineEntity.CodigoDetalle)}," +
                // $"@{ nameof(ContractLineEntity.Nombre)}," +
                $"@{nameof(ContractLineEntity.oAlquiler.AlquilerID)}," +
                $"@{nameof(ContractLineEntity.oAlquilerConcepto.AlquilerConceptoID)}," +
                $"@{nameof(ContractLineEntity.Periodicidad)}, " +
                $"@{ nameof(ContractLineEntity.Importe)}," +
                $"@{nameof(ContractLineEntity.oMoneda.MonedaID)}," +
                $"@{nameof(ContractLineEntity.Descripcion)}," +
                $"@{nameof(ContractLineEntity.FechaPrimerPago)}," +
                $"@{nameof(ContractLineEntity.FechaUltimoPago)}, " +
                $"@{nameof(ContractLineEntity.FechaProximoPago)}, " +
                $"@{ nameof(ContractLineEntity.NumeroCuotas)}," +
                $"@{ nameof(ContractLineEntity.TotalImporte)}," +
                $"@{nameof(ContractLineEntity.TotalConProrrogas)}," +
                $"@{nameof(ContractLineEntity.AplicaProrrogaAutomatica)}," +
                $"@{nameof(ContractLineEntity.Prorrateo)}," +
                $"@{nameof(ContractLineEntity.PagoAnticipado)}," +
            #region REAJUSTE
                $"@{ nameof(ContractLineEntity.oReajuste.Tipo)}," +
                $"@{ nameof(ContractLineEntity.oReajuste.Inflacion.InflacionID)}," +
                $"@{ nameof(ContractLineEntity.oReajuste.CantidadFija)}," +
                $"@{ nameof(ContractLineEntity.oReajuste.PorcentajeFijo)}," +
                $"@{ nameof(ContractLineEntity.oReajuste.Periodicidad)}," +
                $"@{ nameof(ContractLineEntity.oReajuste.FechaInicioReajuste)}," +
                $"@{ nameof(ContractLineEntity.oReajuste.FechaFinReajuste)}," +
                $"@{ nameof(ContractLineEntity.oReajuste.FechaProximaReajuste)});" +
            #endregion


               $"SELECT SCOPE_IDENTITY();";
            DbConnection connection = await _conexionWrapper.GetConnectionAsync();
            var sqlTran = await _conexionWrapper.GetTransactionAsync();
            var newId = (await connection.QueryAsync<int>(sql, new
            {
                CodigoDetalle = obj.CodigoDetalle,
                //  NombreContrato = obj.Nombre,
                AlquilerID = obj.oAlquiler.AlquilerID,
                AlquilerConceptoID = obj.oAlquilerConcepto.AlquilerConceptoID,
                Periodicidad = obj.Periodicidad,
                Importe = obj.Importe,
                MonedaID = obj.oMoneda.MonedaID,
                Descripcion = obj.Descripcion,
                FechaPrimerPago = obj.FechaPrimerPago,
                FechaUltimoPago = obj.FechaUltimoPago,
                FechaProximoPago = obj.FechaProximoPago,
                NumeroCuotas = obj.NumeroCuotas,
                TotalImporte = obj.TotalImporte,
                TotalConProrrogas = obj.TotalConProrrogas,
                AplicaProrrogaAutomatica = obj.AplicaProrrogaAutomatica,
                PagoAnticipado = obj.PagoAnticipado,
                Prorrateo = obj.Prorrateo,
                TipoReajuste = (obj.oReajuste != null) ? obj.oReajuste.Tipo : null,
                InflacionID = (obj.oReajuste.Inflacion != null) ? obj.oReajuste.Inflacion.InflacionID : null,
                CantidadFija = (obj.oReajuste != null) ? obj.oReajuste.CantidadFija : null,
                PorcentajeFijo = (obj.oReajuste != null) ? obj.oReajuste.PorcentajeFijo : null,
                AjustesPeriodicidad = (obj.oReajuste != null) ? obj.oReajuste.Periodicidad : null,
                FechaPrimeraRevision = (obj.oReajuste != null) ? obj.oReajuste.FechaInicioReajuste : null,
                FechaUltimaRevision = (obj.oReajuste != null) ? obj.oReajuste.FechaFinReajuste : null,
                FechaProximaRevision = (obj.oReajuste != null) ? obj.oReajuste.FechaProximaReajuste : null,


            }, sqlTran)).First();

            return ContractLineEntity.UpdateId(obj, newId);
        }
        public override async Task<ContractLineEntity> UpdateSingle(ContractLineEntity obj)
        {
            string sql = $"update {Table.Name} set {nameof(ContractLineEntity.CodigoDetalle)} = @{nameof(ContractLineEntity.CodigoDetalle)}, " +
                $" {nameof(ContractLineEntity.oAlquiler.AlquilerID)} =   @{nameof(ContractLineEntity.oAlquiler.AlquilerID)}, " +
                $" {nameof(ContractLineEntity.oAlquilerConcepto.AlquilerConceptoID)} =   @{nameof(ContractLineEntity.oAlquilerConcepto.AlquilerConceptoID)}, " +
                $" {nameof(ContractLineEntity.Periodicidad)} =   @{nameof(ContractLineEntity.Periodicidad)}, " +
                $" {nameof(ContractLineEntity.oMoneda.MonedaID)} =   @{nameof(ContractLineEntity.oMoneda.MonedaID)}, " +
                $" {nameof(ContractLineEntity.Importe)} =   @{nameof(ContractLineEntity.Importe)}, " +
                $" {nameof(ContractLineEntity.Descripcion)} =   @{nameof(ContractLineEntity.Descripcion)}, " +
                $" {nameof(ContractLineEntity.FechaPrimerPago)} =   @{nameof(ContractLineEntity.FechaPrimerPago)}, " +
                $" {nameof(ContractLineEntity.FechaUltimoPago)} =   @{nameof(ContractLineEntity.FechaUltimoPago)}, " +
                $" {nameof(ContractLineEntity.FechaProximoPago)} =   @{nameof(ContractLineEntity.FechaProximoPago)}, " +
                $" {nameof(ContractLineEntity.NumeroCuotas)} =   @{nameof(ContractLineEntity.NumeroCuotas)}, " +
                $" {nameof(ContractLineEntity.TotalImporte)} =   @{nameof(ContractLineEntity.TotalImporte)}, " +
                $" {nameof(ContractLineEntity.TotalConProrrogas)} =   @{nameof(ContractLineEntity.TotalConProrrogas)}, " +
                $" {nameof(ContractLineEntity.AplicaProrrogaAutomatica)} =   @{nameof(ContractLineEntity.AplicaProrrogaAutomatica)}, " +
                $" {nameof(ContractLineEntity.Prorrateo)} =   @{nameof(ContractLineEntity.Prorrateo)}, " +
                $" {nameof(ContractLineEntity.PagoAnticipado)} =   @{nameof(ContractLineEntity.PagoAnticipado)}, " +
            #region REAJUSTE
                $" TipoReajuste =   @TipoReajuste ," +
                $" {nameof(ContractLineEntity.oReajuste.Inflacion.InflacionID)} =   @{nameof(ContractLineEntity.oReajuste.Inflacion.InflacionID)} ," +
                $" {nameof(ContractLineEntity.oReajuste.CantidadFija)} =   @{nameof(ContractLineEntity.oReajuste.CantidadFija)}," +
                $" {nameof(ContractLineEntity.oReajuste.PorcentajeFijo)} =   @{nameof(ContractLineEntity.oReajuste.PorcentajeFijo)}, " +
                $" AjustesPeriodicidad =   @AjustesPeriodicidad ," +
                $" FechaPrimeraRevision =   @FechaPrimeraRevision ," +
                $" FechaUltimaRevision =   @FechaUltimaRevision ," +
                $" FechaProximaRevision =   @FechaProximaRevision " +
            #endregion
                $" where {nameof(ContractLineEntity.AlquilerDetalleID)} = @{nameof(ContractLineEntity.AlquilerDetalleID)} ";



            DbConnection connection = await _conexionWrapper.GetConnectionAsync();
            var sqlTran = await _conexionWrapper.GetTransactionAsync();
            IEnumerable<ContractLineTaxesEntity> resulttax = await contractLineTaxesRepository.UpdateList(obj.oAlquileresDetallesImpuestos, obj.AlquilerDetalleID.ToString());
            IEnumerable<ContractLineEntidadEntity> resultentidad = await contractLineEntidadesRepository.UpdateList(obj.oAlquileresDetallesEntidades, obj.AlquilerDetalleID.ToString());
            var filasafectadas = (await connection.ExecuteAsync(sql, new
            {
                AlquilerDetalleID = obj.AlquilerDetalleID,
                CodigoDetalle = obj.CodigoDetalle,
                //  NombreContrato = obj.Nombre,
                AlquilerID = obj.oAlquiler.AlquilerID,
                AlquilerConceptoID = obj.oAlquilerConcepto.AlquilerConceptoID,
                Periodicidad = obj.Periodicidad,
                Importe = obj.Importe,
                MonedaID = obj.oMoneda.MonedaID,
                Descripcion = obj.Descripcion,
                FechaPrimerPago = obj.FechaPrimerPago,
                FechaUltimoPago = obj.FechaUltimoPago,
                FechaProximoPago = obj.FechaProximoPago,
                NumeroCuotas = obj.NumeroCuotas,
                TotalImporte = obj.TotalImporte,
                TotalConProrrogas = obj.TotalConProrrogas,
                AplicaProrrogaAutomatica = obj.AplicaProrrogaAutomatica,
                PagoAnticipado = obj.PagoAnticipado,
                Prorrateo = obj.Prorrateo,
                TipoReajuste = (obj.oReajuste != null) ? obj.oReajuste.Tipo : null,
                InflacionID = (obj.oReajuste.Inflacion != null) ? obj.oReajuste.Inflacion.InflacionID : null,
                CantidadFija = (obj.oReajuste != null) ? obj.oReajuste.CantidadFija : null,
                PorcentajeFijo = (obj.oReajuste != null) ? obj.oReajuste.PorcentajeFijo : null,
                AjustesPeriodicidad = (obj.oReajuste != null) ? obj.oReajuste.Periodicidad : null,
                FechaPrimeraRevision = (obj.oReajuste != null) ? obj.oReajuste.FechaInicioReajuste : null,
                FechaUltimaRevision = (obj.oReajuste != null) ? obj.oReajuste.FechaFinReajuste : null,
                FechaProximaRevision = (obj.oReajuste != null) ? obj.oReajuste.FechaProximaReajuste : null,

            }, sqlTran));
            return obj;
        }

        public override async Task<ContractLineEntity> GetItemByCode(string code, int alquilerID)
        {
            DbConnection connection = await _conexionWrapper.GetConnectionAsync();
            var sqlTran = await _conexionWrapper.GetTransactionAsync();
            string sql = $"select * from {Table.Name} C " +
                $"inner join {Contract.Name} CT on C.{Contract.ID} = CT.{Contract.ID} " +
                $"left join {ContractLineType.Name} TP on C.{ContractLineType.ID} = TP.{ContractLineType.ID} " +
                $"left join {Currency.Name} TI on C.{Currency.ID}= TI.{Currency.ID} " +

                $" where C.{Table.Code} = @code" +
                $" and C.{Contract.ID}=@AlquilerID";

            var result = await connection.QueryAsync<ContractLineEntity, PriceReadjustment, ContractEntity, ContractLineTypeEntity, CurrencyEntity, ContractLineEntity>(sql,
                 (contractlineEntity, priceReadjustment, contractEntity, contractlineTypeEntity, currencyEntity) =>
                 {
                     contractlineEntity.oAlquiler = contractEntity;

                     if (priceReadjustment != null)
                     {
                         contractlineEntity.oReajuste = priceReadjustment;
                     }
                     contractlineEntity.oAlquilerConcepto = contractlineTypeEntity;
                     contractlineEntity.oMoneda = currencyEntity;


                     return contractlineEntity;
                 },
                 new { code = code, AlquilerID = alquilerID }, sqlTran, true, splitOn: $"TipoReajuste,{Contract.ID},{ContractLineType.ID},{Currency.ID}");
            if (result.FirstOrDefault() != null && result.FirstOrDefault().AlquilerDetalleID != null)
            {

                result.FirstOrDefault().oAlquileresDetallesImpuestos = contractLineTaxesRepository.GetListbyContractLineDetails(result.FirstOrDefault().AlquilerDetalleID.Value).Result;
                result.FirstOrDefault().oAlquileresDetallesEntidades = contractLineEntidadesRepository.GetListbyContractLineDetails(result.FirstOrDefault().AlquilerDetalleID.Value).Result;
            }


            return result.FirstOrDefault();
        }

        public override async Task<int> Delete(ContractLineEntity obj)
        {
            DbConnection connection = await _conexionWrapper.GetConnectionAsync();
            var sqlTran = await _conexionWrapper.GetTransactionAsync();
            string sql = $"delete from {Table.Name} Where {Table.ID} = @key";
            int numReg;
            try
            {
                var resultlinetax = await contractLineTaxesRepository.DeleteByContractLine((int)obj.AlquilerDetalleID.Value);
                var resultlineentidad = await contractLineEntidadesRepository.DeleteByContractLine((int)obj.AlquilerDetalleID.Value);
                numReg = await connection.ExecuteAsync(sql, new
                {
                    key = obj.AlquilerDetalleID
                }, sqlTran);
            }
            catch (System.Exception)
            {
                numReg = 0;
            }
            return numReg;
        }


        public override async Task<IEnumerable<ContractLineEntity>> GetList(int Client, List<Filter> filters, List<string> orders, string direction, int pageSize = -1, int pageIndex = -1)
        {
            DbConnection connection = await _conexionWrapper.GetConnectionAsync();
            var sqlTran = await _conexionWrapper.GetTransactionAsync();
            Dictionary<string, object> dicParam = new Dictionary<string, object>();
            int countFilter = 0;

            string sqlRelation = $" C inner join {Contract.Name} CG on C.{Contract.ID} = CG.{Contract.ID} " +
                $"left join {ContractLineType.Name} CT on C.{ContractLineType.ID} = CT.{ContractLineType.ID} " +
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
                    sqlOrder += $"{(string.IsNullOrEmpty(sqlOrder) ? " order by C." : ", ")} {column}";
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

            string query = $"select TotalItems = COUNT(*) OVER(), * from {Table.Name} {sqlRelation} {sqlFilter} {sqlOrder} {sqlPagination}";

            var result = await connection.QueryAsync<ContractLineEntity, PriceReadjustment, ContractEntity, ContractLineTypeEntity, CurrencyEntity, ContractLineEntity>(sqlRelation,
                 (contractlineEntity, priceReadjustment, contractEntity, contractlineTypeEntity, currencyEntity) =>
                 {
                     contractlineEntity.oAlquiler = contractEntity;

                     if (priceReadjustment != null)
                     {
                         contractlineEntity.oReajuste = priceReadjustment;
                     }
                     contractlineEntity.oAlquilerConcepto = contractlineTypeEntity;
                     contractlineEntity.oMoneda = currencyEntity;


                     return contractlineEntity;
                 },
                 dicParam, sqlTran, true, splitOn: $"TipoReajuste,{Contract.ID},{ContractLineType.ID},{Currency.ID}");

            return result.ToList();
        }


        #region CONTRACT LINE BY CONTRACT
        public async Task<IEnumerable<ContractLineEntity>> InsertList(IEnumerable<ContractLineEntity> obj, string alquilerID)
        {
            foreach (ContractLineEntity item in obj)
            {
                var result = await InsertSingleWithContract(item, alquilerID);
            };
            return obj;
        }

        public async Task<ContractLineEntity> InsertSingleWithContract(ContractLineEntity obj, string alquilerID)
        {
            string sql = $"insert into {Table.Name} " +
               $"({nameof(ContractLineEntity.CodigoDetalle)}," +
               //$"{nameof(ContractLineEntity.Nombre)}, " +
               $"{nameof(ContractLineEntity.oAlquiler.AlquilerID)}, " +
               $"{nameof(ContractLineEntity.oAlquilerConcepto.AlquilerConceptoID)}, " +
               $"{ nameof(ContractLineEntity.Periodicidad)}," +
               $"{nameof(ContractLineEntity.Importe)}," +
               $"{nameof(ContractLineEntity.oMoneda.MonedaID)}, " +
               $"{nameof(ContractLineEntity.Descripcion)}, " +
               $"{nameof(ContractLineEntity.FechaPrimerPago)}, " +
               $"{nameof(ContractLineEntity.FechaUltimoPago)}, " +
               $"{ nameof(ContractLineEntity.FechaProximoPago)}," +
               $"{ nameof(ContractLineEntity.NumeroCuotas)}," +
               $"{ nameof(ContractLineEntity.TotalImporte)}," +
               $"{ nameof(ContractLineEntity.TotalConProrrogas)}," +
               $"{ nameof(ContractLineEntity.AplicaProrrogaAutomatica)}," +
               $"{ nameof(ContractLineEntity.Prorrateo)}," +
               $"{ nameof(ContractLineEntity.PagoAnticipado)}," +

            #region REAJUSTE
                $"TipoReajuste," +
               $"{ nameof(ContractLineEntity.oReajuste.Inflacion.InflacionID)}," +
               $"{ nameof(ContractLineEntity.oReajuste.CantidadFija)}," +
               $"{ nameof(ContractLineEntity.oReajuste.PorcentajeFijo)}," +
               $"AjustesPeriodicidad," +
               $"FechaPrimeraRevision," +
               $"FechaUltimaRevision," +
               $"FechaProximaRevision," +
            #endregion
               $"TotalImpuestos," +
               $"ImporteMensual," +
               $"ImporteAnual," +
               $"ActivoPrincipal," +
               $"Activo)" +

                $"values(@{ nameof(ContractLineEntity.CodigoDetalle)}," +
               // $"@{ nameof(ContractLineEntity.Nombre)}," +
               $"@{nameof(ContractLineEntity.oAlquiler.AlquilerID)}," +
               $"@{nameof(ContractLineEntity.oAlquilerConcepto.AlquilerConceptoID)}," +
               $"@{nameof(ContractLineEntity.Periodicidad)}, " +
               $"@{ nameof(ContractLineEntity.Importe)}," +
               $"@{nameof(ContractLineEntity.oMoneda.MonedaID)}," +
               $"@{nameof(ContractLineEntity.Descripcion)}," +
               $"@{nameof(ContractLineEntity.FechaPrimerPago)}," +
               $"@{nameof(ContractLineEntity.FechaUltimoPago)}, " +
               $"@{nameof(ContractLineEntity.FechaProximoPago)}, " +
               $"@{ nameof(ContractLineEntity.NumeroCuotas)}," +
               $"@{ nameof(ContractLineEntity.TotalImporte)}," +
               $"@{nameof(ContractLineEntity.TotalConProrrogas)}," +
               $"@{nameof(ContractLineEntity.AplicaProrrogaAutomatica)}," +
               $"@{nameof(ContractLineEntity.Prorrateo)}," +
               $"@{nameof(ContractLineEntity.PagoAnticipado)}," +
            #region REAJUSTE
               $"@TipoReajuste," +
               $"@{ nameof(ContractLineEntity.oReajuste.Inflacion.InflacionID)}," +
               $"@{ nameof(ContractLineEntity.oReajuste.CantidadFija)}," +
               $"@{ nameof(ContractLineEntity.oReajuste.PorcentajeFijo)}," +
               $"@AjustesPeriodicidad," +
               $"@FechaPrimeraRevision," +
               $"@FechaUltimaRevision," +
               $"@FechaProximaRevision," +
            #endregion
               $"@TotalImpuestos," +
               $"@ImporteMensual," +
               $"@ImporteAnual," +
               $"@ActivoPrincipal," +
               $"@Activo);" +



            $"SELECT SCOPE_IDENTITY();";
            DbConnection connection = await _conexionWrapper.GetConnectionAsync();
            var sqlTran = await _conexionWrapper.GetTransactionAsync();
            var newId = (await connection.QueryAsync<int>(sql, new
            {
                CodigoDetalle = obj.CodigoDetalle,
                //  NombreContrato = obj.Nombre,
                AlquilerID = alquilerID,
                AlquilerConceptoID = obj.oAlquilerConcepto.AlquilerConceptoID,
                Periodicidad = obj.Periodicidad,
                Importe = obj.Importe,
                MonedaID = obj.oMoneda.MonedaID,
                Descripcion = obj.Descripcion,
                FechaPrimerPago = obj.FechaPrimerPago,
                FechaUltimoPago = obj.FechaUltimoPago,
                FechaProximoPago = obj.FechaProximoPago,
                NumeroCuotas = obj.NumeroCuotas,
                TotalImporte = obj.TotalImporte,
                TotalConProrrogas = obj.TotalConProrrogas,
                AplicaProrrogaAutomatica = obj.AplicaProrrogaAutomatica,
                PagoAnticipado = obj.PagoAnticipado,
                Prorrateo = obj.Prorrateo,
                TipoReajuste = (obj.oReajuste != null) ? obj.oReajuste.Tipo : null,
                InflacionID = (obj.oReajuste.Inflacion != null) ? obj.oReajuste.Inflacion.InflacionID : null,
                CantidadFija = (obj.oReajuste != null) ? obj.oReajuste.CantidadFija : null,
                PorcentajeFijo = (obj.oReajuste != null) ? obj.oReajuste.PorcentajeFijo : null,
                AjustesPeriodicidad = (obj.oReajuste != null) ? obj.oReajuste.Periodicidad : null,
                FechaPrimeraRevision = (obj.oReajuste != null) ? obj.oReajuste.FechaInicioReajuste : null,
                FechaUltimaRevision = (obj.oReajuste != null) ? obj.oReajuste.FechaFinReajuste : null,
                FechaProximaRevision = (obj.oReajuste != null) ? obj.oReajuste.FechaProximaReajuste : null,
                Activo = true,
                ImporteMensual = 0,
                TotalImpuestos = 0,
                ImporteAnual = 0,
                ActivoPrincipal = false,


            }, sqlTran)).First();

            IEnumerable<ContractLineTaxesEntity> result = await contractLineTaxesRepository.InsertList(obj.oAlquileresDetallesImpuestos, newId.ToString());

            IEnumerable<ContractLineEntidadEntity> resultentidad = await contractLineEntidadesRepository.InsertList(obj.oAlquileresDetallesEntidades, newId.ToString());
            return ContractLineEntity.UpdateId(obj, newId);

        }


        public async Task<IEnumerable<ContractLineEntity>> GetListbyContract(int AlquilerID)
        {
            DbConnection connection = await _conexionWrapper.GetConnectionAsync();
            var sqlTran = await _conexionWrapper.GetTransactionAsync();

            string query = $"select * from {Table.Name} where {Contract.ID} = @alquilerID";

            var result = await connection.QueryAsync<ContractLineEntity>(query, new { alquilerID = AlquilerID }, sqlTran);

            return result.ToList();
        }
        public async Task<IEnumerable<ContractLineEntity>> GetListbyContractDetails(int AlquilerID)
        {
            DbConnection connection = await _conexionWrapper.GetConnectionAsync();
            var sqlTran = await _conexionWrapper.GetTransactionAsync();

            string sql = $"select * from {Table.Name} C " +
                $"inner join {Contract.Name} CT on C.{Contract.ID} = CT.{Contract.ID} " +
                $"inner join {ContractLineType.Name} TP on C.{ContractLineType.ID} = TP.{ContractLineType.ID} " +
                $"inner join {Currency.Name} TI on C.{Currency.ID}= TI.{Currency.ID} " +

                $" where C.{Contract.ID} = @AlquilerID";


            var result = await connection.QueryAsync<ContractLineEntity, PriceReadjustment, ContractEntity, ContractLineTypeEntity, CurrencyEntity, ContractLineEntity>(sql,
                (contractlineEntity, priceReadjustment, contractEntity, contractlineTypeEntity, currencyEntity) =>
                {
                    contractlineEntity.oAlquiler = contractEntity;

                    if (priceReadjustment != null)
                    {
                        contractlineEntity.oReajuste = priceReadjustment;
                    }
                    contractlineEntity.oAlquilerConcepto = contractlineTypeEntity;
                    contractlineEntity.oMoneda = currencyEntity;
                    return contractlineEntity;
                },
                 new { AlquilerID = AlquilerID }, sqlTran, true, splitOn: $"TipoReajuste,AlquilerID,AlquilerConceptoID,MonedaID");

            //IEnumerable<ContractLineTaxesEntity> contractlinetax;
            //IEnumerable<ContractLineEntidadEntity> contractlineentidad;

            string ids = "";
            result.ToList().ForEach(x =>
            {
                ids += $"{(string.IsNullOrEmpty(ids) ? "" : ",")}{x.AlquilerDetalleID}";

            });
            if (result != null && result.ToList().Count > 0)
            {
                result = await contractLineTaxesRepository.GetListbyContractLineDetails(result, ids);
                result = await contractLineEntidadesRepository.GetListbyContractLineDetails(result, ids);
            }

            //foreach (ContractLineEntity contractline in result)
            //{
            //    contractlinetax = await contractLineTaxesRepository.GetListbyContractLineDetails(contractline.AlquilerDetalleID.Value);
            //    if (contractlinetax != null)
            //    {
            //        contractline.oAlquileresDetallesImpuestos = contractlinetax;
            //    }
            //}

            //foreach (ContractLineEntity contractline in result)
            //{
            //    contractlineentidad = await contractLineEntidadesRepository.GetListbyContractLineDetails(contractline.AlquilerDetalleID.Value);
            //    if (contractlineentidad != null)
            //    {
            //        contractline.oAlquileresDetallesEntidades = contractlineentidad;
            //    }
            //}


            return result.ToList();
        }

        public async Task<IEnumerable<ContractEntity>> GetListbyContractDetails(IEnumerable<ContractEntity> result, string IDS)
        {
            DbConnection connection = await _conexionWrapper.GetConnectionAsync();
            var sqlTran = await _conexionWrapper.GetTransactionAsync();

            string sql = $"select * from {Table.Name} C " +
                $"inner join {Contract.Name} CT on C.{Contract.ID} = CT.{Contract.ID} " +
                $"inner join {ContractLineType.Name} TP on C.{ContractLineType.ID} = TP.{ContractLineType.ID} " +
                $"inner join {Currency.Name} TI on C.{Currency.ID}= TI.{Currency.ID} " +

                $" where C.{Contract.ID} in ({IDS})";

            List<ContractLineEntity> listAssigned = new List<ContractLineEntity>();
            var result2 = await connection.QueryAsync<ContractLineEntity, PriceReadjustment, ContractEntity, ContractLineTypeEntity, CurrencyEntity, ContractLineEntity>(sql,
                (contractlineEntity, priceReadjustment, contractEntity, contractlineTypeEntity, currencyEntity) =>
                {
                    //listAssigned = new List<ContractLineEntity>();
                    if (contractlineEntity != null)
                    {
                        contractlineEntity.oAlquiler = contractEntity;

                        if (priceReadjustment != null)
                        {
                            contractlineEntity.oReajuste = priceReadjustment;
                        }
                        contractlineEntity.oAlquilerConcepto = contractlineTypeEntity;
                        contractlineEntity.oMoneda = currencyEntity;
                        result.Where(contractEntity => contractEntity.AlquilerID == contractlineEntity.oAlquiler.AlquilerID).ToList().ForEach(con =>
                          {
                              if (con.oAlquileresDetalles == null)
                              {
                                  listAssigned = new List<ContractLineEntity>();
                              }
                              listAssigned.Add(contractlineEntity);
                              con.oAlquileresDetalles = listAssigned;
                          });

                    }

                    return contractlineEntity;
                },
                 new { }, sqlTran, true, splitOn: $"TipoReajuste,AlquilerID,AlquilerConceptoID,MonedaID");



            string ids = "";
            result2.ToList().ForEach(x =>
            {
                ids += $"{(string.IsNullOrEmpty(ids) ? "" : ",")}{x.AlquilerDetalleID}";

            });
            if (result2 != null && result2.ToList().Count > 0)
            {
                result2 = await contractLineTaxesRepository.GetListbyContractLineDetails(result2, ids);
                result2 = await contractLineEntidadesRepository.GetListbyContractLineDetails(result2, ids);
            }


            return result.ToList();
        }

        public async Task<int> DeleteByContract(int AlquilerID)
        {
            DbConnection connection = await _conexionWrapper.GetConnectionAsync();
            var sqlTran = await _conexionWrapper.GetTransactionAsync();
            string sql = $"delete from {Table.Name} Where {Contract.ID} = @key";
            int numReg;
            try
            {
                numReg = await connection.ExecuteAsync(sql, new
                {
                    key = AlquilerID
                }, sqlTran);
            }
            catch (System.Exception)
            {
                numReg = 0;
            }
            return numReg;
        }


        public async Task<IEnumerable<ContractLineEntity>> UpdateList(IEnumerable<ContractLineEntity> obj, string alquilerID)
        {
            IEnumerable<ContractLineEntity> listaExiste = GetListbyContract(int.Parse(alquilerID)).Result;

            #region Remove linked ContractLine

            foreach (ContractLineEntity contractLine in listaExiste)
            {
                int idBorrar = compruebaExiste(obj, contractLine);
                if (idBorrar != 0)
                {
                    var result = await Delete(contractLine);
                }

            }

            #endregion

            #region Add linked ContractLine
            if (obj != null)
            {
                foreach (ContractLineEntity contractline in obj.ToList())
                {
                    if (contieneNombre(listaExiste, contractline) == 0)
                    {

                        var result = await InsertSingleWithContract(contractline, alquilerID);
                    }
                    else
                    {
                        var result = await UpdateSingle(contractline);
                    }


                };
            }
            #endregion
            return obj;
        }

        private int contieneNombre(IEnumerable<ContractLineEntity> listaExiste, ContractLineEntity obj)
        {
            foreach (ContractLineEntity contractline in listaExiste)
            {
                if (contractline.CodigoDetalle == obj.CodigoDetalle)
                {
                    return contractline.AlquilerDetalleID.Value;
                }
            }
            return 0;
        }
        private int compruebaExiste(IEnumerable<ContractLineEntity> listaExiste, ContractLineEntity obj)
        {
            foreach (ContractLineEntity contractline in listaExiste)
            {
                if (contractline.CodigoDetalle == obj.CodigoDetalle)
                {
                    return 0;
                }
            }

            return obj.AlquilerDetalleID.Value;
        }
        #endregion
    }
}
