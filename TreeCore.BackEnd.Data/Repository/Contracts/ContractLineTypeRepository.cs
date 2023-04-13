using Dapper;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using TreeCore.BackEnd.Data.Repository.Query;
using TreeCore.BackEnd.Model.Entity.Contracts;
using TreeCore.Shared.Data.Db;

namespace TreeCore.BackEnd.Data.Repository.Contracts
{
    public class ContractLineTypeRepository : BaseRepository<ContractLineTypeEntity>
    {
        public override Table Table => TableNames.ContractLineType;

        public ContractLineTypeRepository(TransactionalWrapper conexion) : base(conexion)
        {
        }

        public override async Task<ContractLineTypeEntity> InsertSingle(ContractLineTypeEntity obj)
        {
            string sql = $"insert into {Table.Name} ({nameof(ContractLineTypeEntity.ClienteID)},{nameof(ContractLineTypeEntity.Codigo)}, {nameof(ContractLineTypeEntity.AlquilerConcepto)}, {nameof(ContractLineTypeEntity.Descripcion)}, {nameof(ContractLineTypeEntity.Activo)}, " +
                $"{ nameof(ContractLineTypeEntity.Defecto)}," + $"{ nameof(ContractLineTypeEntity.EsPagoUnico)}," + $"{ nameof(ContractLineTypeEntity.EsAlquilerBase)}," + $"{ nameof(ContractLineTypeEntity.ImplicaDeuda)}," + $"{ nameof(ContractLineTypeEntity.EsFianza)}," + $"{ nameof(ContractLineTypeEntity.EsPagoAdicional)}," + $"{ nameof(ContractLineTypeEntity.EsInfraestructura)}," + $"{ nameof(ContractLineTypeEntity.OpcionCompra)}," + $"{ nameof(ContractLineTypeEntity.OpcionAmortizacion)}," + $"{ nameof(ContractLineTypeEntity.EsEstadistica)}," + $"{ nameof(ContractLineTypeEntity.OpcionValorResidual)}," + $"{ nameof(ContractLineTypeEntity.OpcionSubarrendamiento)}," + $"{ nameof(ContractLineTypeEntity.ObjetivoUnico)}," + $"{ nameof(ContractLineTypeEntity.ContarAlquiler)}," + $"{ nameof(ContractLineTypeEntity.ContarSuministro)}," + $"{ nameof(ContractLineTypeEntity.Torrero)}," + $"{ nameof(ContractLineTypeEntity.Sharing)}," + $"{ nameof(ContractLineTypeEntity.Variable)}," + $"{ nameof(ContractLineTypeEntity.PagoAnticipadoAuto)}," + $"{ nameof(ContractLineTypeEntity.EsCobro)})" +
                $"values (@{nameof(ContractLineTypeEntity.ClienteID)},@{nameof(ContractLineTypeEntity.Codigo)}, @{nameof(ContractLineTypeEntity.AlquilerConcepto)}, @{nameof(ContractLineTypeEntity.Descripcion)}, @{nameof(ContractLineTypeEntity.Activo)}, " +
                $" @{ nameof(ContractLineTypeEntity.Defecto)}," + $" @{ nameof(ContractLineTypeEntity.EsPagoUnico)}," + $" @{ nameof(ContractLineTypeEntity.EsAlquilerBase)}," + $" @{ nameof(ContractLineTypeEntity.ImplicaDeuda)}," + $" @{ nameof(ContractLineTypeEntity.EsFianza)}," + $" @{ nameof(ContractLineTypeEntity.EsPagoAdicional)}," + $" @{ nameof(ContractLineTypeEntity.EsInfraestructura)}," + $" @{ nameof(ContractLineTypeEntity.OpcionCompra)}," + $" @{ nameof(ContractLineTypeEntity.OpcionAmortizacion)}," + $" @{ nameof(ContractLineTypeEntity.EsEstadistica)}," + $" @{ nameof(ContractLineTypeEntity.OpcionValorResidual)}," + $" @{ nameof(ContractLineTypeEntity.OpcionSubarrendamiento)}," + $" @{ nameof(ContractLineTypeEntity.ObjetivoUnico)}," + $" @{ nameof(ContractLineTypeEntity.ContarAlquiler)}," + $" @{ nameof(ContractLineTypeEntity.ContarSuministro)}," + $" @{ nameof(ContractLineTypeEntity.Torrero)}," + $" @{ nameof(ContractLineTypeEntity.Sharing)}," + $" @{ nameof(ContractLineTypeEntity.Variable)}," + $" @{ nameof(ContractLineTypeEntity.PagoAnticipadoAuto)}," + $" @{ nameof(ContractLineTypeEntity.EsCobro)});" +
                $"SELECT SCOPE_IDENTITY();";
            DbConnection connection = await _conexionWrapper.GetConnectionAsync();
            var sqlTran = await _conexionWrapper.GetTransactionAsync();
            var newId = (await connection.QueryAsync<int>(sql, new
            {
                ClienteID = obj.ClienteID,
                Codigo = obj.Codigo,
                AlquilerConcepto = obj.AlquilerConcepto,
                Descripcion = obj.Descripcion,
                Activo = obj.Activo,
                Defecto = obj.Defecto,
                EsPagoUnico = obj.EsPagoUnico,
                EsAlquilerBase = obj.EsAlquilerBase,
                ImplicaDeuda = false,
                EsFianza = false,
                EsPagoAdicional = false,
                EsInfraestructura = false,
                OpcionCompra = false,
                OpcionAmortizacion = false,
                EsEstadistica = false,
                OpcionValorResidual = false,
                OpcionSubarrendamiento = false,
                ObjetivoUnico = false,
                ContarAlquiler = false,
                ContarSuministro = false,
                Torrero = false,
                Sharing = false,
                Variable = false,
                PagoAnticipadoAuto = false,
                EsCobro = obj.EsCobro
            }, sqlTran)).First();

            return ContractLineTypeEntity.UpdateId(obj, newId);
        }
        public override async Task<ContractLineTypeEntity> UpdateSingle(ContractLineTypeEntity obj)
        {
            string sql = $"update {Table.Name} set {nameof(ContractLineTypeEntity.Codigo)} = @{nameof(ContractLineTypeEntity.Codigo)}, " +
                $" {nameof(ContractLineTypeEntity.AlquilerConcepto)} =   @{nameof(ContractLineTypeEntity.AlquilerConcepto)}, " +
                $" {nameof(ContractLineTypeEntity.Descripcion)} =  @{nameof(ContractLineTypeEntity.Descripcion)}, " +
                $" {nameof(ContractLineTypeEntity.Activo)} = @{nameof(ContractLineTypeEntity.Activo)}, " +
                $" {nameof(ContractLineTypeEntity.Defecto)} =  @{ nameof(ContractLineTypeEntity.Defecto)}, " +
                $" {nameof(ContractLineTypeEntity.EsPagoUnico)} =  @{ nameof(ContractLineTypeEntity.EsPagoUnico)}, " +
                $" {nameof(ContractLineTypeEntity.EsAlquilerBase)} =  @{ nameof(ContractLineTypeEntity.EsAlquilerBase)}, " +
                $" {nameof(ContractLineTypeEntity.EsCobro)} =  @{ nameof(ContractLineTypeEntity.EsCobro)} " +
                $" where {nameof(ContractLineTypeEntity.AlquilerConceptoID)} = @{nameof(ContractLineTypeEntity.AlquilerConceptoID)} ";

            DbConnection connection = await _conexionWrapper.GetConnectionAsync();
            var sqlTran = await _conexionWrapper.GetTransactionAsync();
            var filasafectadas = (await connection.ExecuteAsync(sql, new
            {
                Codigo = obj.Codigo,
                AlquilerConcepto = obj.AlquilerConcepto,
                Descripcion = obj.Descripcion,
                Activo = obj.Activo,
                Defecto = obj.Defecto,
                EsPagoUnico = obj.EsPagoUnico,
                EsAlquilerBase = obj.EsAlquilerBase,
                EsCobro = obj.EsCobro,
                AlquilerConceptoID = obj.AlquilerConceptoID
            }, sqlTran));
            return obj;
        }

        public override async Task<ContractLineTypeEntity> GetItemByCode(string code, int Client)
        {
            DbConnection connection = await _conexionWrapper.GetConnectionAsync();
            var sqlTran = await _conexionWrapper.GetTransactionAsync();

            return await connection.QueryFirstOrDefaultAsync<ContractLineTypeEntity>($"select * from {Table.Name} where Codigo = @code AND {nameof(ContractLineTypeEntity.ClienteID)} = @{nameof(ContractLineTypeEntity.ClienteID)}", new
            {
                code = code,
                ClienteID = Client
            }, sqlTran);

        }

        public override async Task<int> Delete(ContractLineTypeEntity obj)
        {
            DbConnection connection = await _conexionWrapper.GetConnectionAsync();
            var sqlTran = await _conexionWrapper.GetTransactionAsync();
            string sql = $"delete from {Table.Name} Where {Table.ID} = @key AND {nameof(ContractLineTypeEntity.ClienteID)} = @{nameof(ContractLineTypeEntity.ClienteID)}";
            int numReg;
            try
            {
                numReg = await connection.ExecuteAsync(sql, new
                {
                    key = obj.AlquilerConceptoID,
                    ClienteID = obj.ClienteID
                }, sqlTran);
            }
            catch (System.Exception)
            {
                numReg = 0;
            }
            return numReg;
        }

    }
}
