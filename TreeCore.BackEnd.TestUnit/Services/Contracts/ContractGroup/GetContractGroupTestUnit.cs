using Microsoft.AspNetCore.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Threading.Tasks;
using TreeCore.BackEnd.Model.Entity.Contracts;
using TreeCore.BackEnd.Service.Services.Contracts;
using TreeCore.BackEnd.ServiceDependencies.Services;
using TreeCore.Shared.DTO.Contracts;



namespace TreeCore.BackEnd.TestUnit.Services.Contracts
{

    [TestClass]
    public class GetContractGroupTestUnit
    {
        public Mock<GetDependencies<ContractGroupDTO, ContractGroupEntity>> _dependencies;
        public GetContractGroup Subject;
        public string Code = "ty";
        public int CoreContractGroupID = 1;

        public GetContractGroupTestUnit()
        {
            Mock<GetDependencies<ContractGroupDTO, ContractGroupEntity>> dependencies = new Mock<GetDependencies<ContractGroupDTO, ContractGroupEntity>>();

            Mock<IHttpContextAccessor> httpContextAccessor = new Mock<IHttpContextAccessor>();
            Mock<IHeaderDictionary> headerDictionary = new Mock<IHeaderDictionary>();
            httpContextAccessor.Setup(a => a.HttpContext.Request.Headers)
                .Returns(headerDictionary.Object);

            dependencies.Setup(a => a.GetItemByCode(Code, Settings.Settings.Client))
            .Returns(Task.FromResult(new ContractGroupEntity(CoreContractGroupID, null, Code, "Nombre", "Descripcion", true, false)));

            _dependencies = dependencies;

            Subject = new GetContractGroup(_dependencies.Object, httpContextAccessor.Object);
        }

        [TestMethod]
        public async Task Test_allCorrect_ThenSuccess()
        {
            var state = new GetContractGroupTestUnit();
            var result = await state.Subject.GetItemByCode(state.Code, Settings.Settings.Client);

            Assert.IsTrue(result.Success);
            Assert.AreEqual(Code, result.Value.Code);
        }

        [TestMethod]
        public async Task Test_ContractGroup_NonExistent_ThenError()
        {
            var state = new GetContractGroupTestUnit();
            var result = await state.Subject.GetItemByCode("noExistent", Settings.Settings.Client);

            Assert.IsFalse(result.Success);
            Assert.AreEqual("Not ", result.Errors[0].Message);
        }
    }
}
