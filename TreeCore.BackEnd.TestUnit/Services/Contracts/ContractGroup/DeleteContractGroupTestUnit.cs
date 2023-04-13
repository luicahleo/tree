using Microsoft.AspNetCore.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Threading.Tasks;
using TreeCore.BackEnd.Model.Entity.Contracts;
using TreeCore.BackEnd.Service.Services.Contracts;
using TreeCore.BackEnd.ServiceDependencies.Services;
using TreeCore.Shared.DTO.Contracts;
using TreeCore.Shared.ROP;

namespace TreeCore.BackEnd.TestUnit.Services.Contracts
{
    [TestClass]
    public class DeleteContractGroupTestUnit
    {
        public Mock<GetDependencies<ContractGroupDTO, ContractGroupEntity>> _getDependencies;
        public Mock<DeleteDependencies<ContractGroupEntity>> _deleteDependencies;
        public DeleteContractGroup Subject;
        public readonly ContractGroupEntity ContractGroupEntity;
        
        public readonly ContractGroupEntity ContractGroup;

        public DeleteContractGroupTestUnit()
        {
            //ContractGroupEntity = new ContractGroupEntity(null, "Test1", "Test Put", company, ContractGroupType, 1, null, null, null, null, null, DateTime.Now, DateTime.Now, null);
            Mock<DeleteDependencies<ContractGroupEntity>> deleteDependencies = new Mock<DeleteDependencies<ContractGroupEntity>>();
            Mock<GetDependencies<ContractGroupDTO, ContractGroupEntity>> getDependencies = new Mock<GetDependencies<ContractGroupDTO, ContractGroupEntity>>();

            Mock<IHttpContextAccessor> httpContextAccessor = new Mock<IHttpContextAccessor>();
            Mock<IHeaderDictionary> headerDictionary = new Mock<IHeaderDictionary>();
            httpContextAccessor.Setup(a => a.HttpContext.Request.Headers)
                .Returns(headerDictionary.Object);

            _deleteDependencies = deleteDependencies;
            _getDependencies = getDependencies;

            Subject = new DeleteContractGroup(_deleteDependencies.Object, _getDependencies.Object, httpContextAccessor.Object);

            _deleteDependencies.Setup(a => a.Delete(It.IsAny<ContractGroupEntity>()))
            .Returns(Task.FromResult(Result.Success(1)));
        }

        [TestMethod]
        public async Task Test_allCorrect_ThenSuccess()
        {
            var stateDel = new DeleteContractGroupTestUnit();
            var result = await stateDel.Subject.Delete(stateDel.ContractGroupEntity.codigo, Settings.Settings.Client);

            Assert.IsTrue(result.Success);
            Assert.AreEqual(1, result.Value);
        }
    }

}
