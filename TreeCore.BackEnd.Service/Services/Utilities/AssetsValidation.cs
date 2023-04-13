using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TreeCore.BackEnd.Model.Entity.Contracts;
using TreeCore.BackEnd.Model.Entity.Inventory;
using TreeCore.BackEnd.Model.Entity.Sites;
using TreeCore.BackEnd.Model.Entity.WorkOrders;
using TreeCore.BackEnd.Service.Mappers.WorkOrders;
using TreeCore.BackEnd.Service.Services.Contracts;
using TreeCore.BackEnd.Service.Services.Inventory;
using TreeCore.BackEnd.Service.Services.Sites;
using TreeCore.BackEnd.ServiceDependencies.Services;
using TreeCore.Shared.DTO.Contracts;
using TreeCore.Shared.DTO.Inventory;
using TreeCore.Shared.DTO.Sites;
using TreeCore.Shared.DTO.WorkOrders;
using TreeCore.Shared.ROP;

namespace TreeCore.BackEnd.Service.Services.Utilities
{
    internal class AssetsValidation : BaseObjectService<WorkOrderDTO, WorkOrderEntity, WorkOrderDTOMapper>
    {
        private readonly GetDependencies<InventoryDTO, InventoryEntity> _getInventory;
        private readonly GetDependencies<SiteDTO, SiteEntity> _getSite;
        private readonly GetDependencies<ContractDTO, ContractEntity> _getContract;
        public AssetsValidation(
            GetDependencies<InventoryDTO, InventoryEntity> getInventory,
            GetDependencies<SiteDTO, SiteEntity> getSite,
            GetDependencies<ContractDTO, ContractEntity> getContract,
            IHttpContextAccessor httpcontextAccessor) : base(httpcontextAccessor, null)
        {
            _getInventory = getInventory;
            _getSite = getSite;
            _getContract = getContract;
        }

        public async Task<List<Error>> ValidateAsset(string ObjectCode, string AssetCode, int client) {
            List<Error> lErrors = new List<Error>();
            switch (ObjectCode)
            {
                case "Inventory":
                    lErrors.AddRange(await ValidateInventory(AssetCode, client));
                    break;
                case "Site":
                    lErrors.AddRange(await ValidateSite(AssetCode, client));
                    break;
                case "Contract":
                    lErrors.AddRange(await ValidateContrat(AssetCode, client));
                    break;
                default:
                    lErrors.Add(Error.Create(_traduccion.Currency + " " + $"{ObjectCode}" + " " + _errorTraduccion.NotFound + "."));
                    break;
            }
            return lErrors;
        }

        public async Task<List<Error>> ValidateInventory(string AssetCode, int client) {
            List<Error> lErrors = new List<Error>();
            var inv = await _getInventory.GetItemByCode(AssetCode, client);
            if (inv == null)
            {
                lErrors.Add(Error.Create(_traduccion.Currency + " " + $"{AssetCode}" + " " + _errorTraduccion.NotFound + "."));
            }
            return lErrors;
        }

        public async Task<List<Error>> ValidateSite(string AssetCode, int client) {
            List<Error> lErrors = new List<Error>();
            var inv = await _getInventory.GetItemByCode(AssetCode, client);
            if (inv == null)
            {
                lErrors.Add(Error.Create(_traduccion.Currency + " " + $"{AssetCode}" + " " + _errorTraduccion.NotFound + "."));
            }
            return lErrors;
        }

        public async Task<List<Error>> ValidateContrat(string AssetCode, int client) {
            List<Error> lErrors = new List<Error>();
            var inv = await _getInventory.GetItemByCode(AssetCode, client);
            if (inv == null)
            {
                lErrors.Add(Error.Create(_traduccion.Currency + " " + $"{AssetCode}" + " " + _errorTraduccion.NotFound + "."));
            }
            return lErrors;
        }

    }
}
