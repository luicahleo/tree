using Microsoft.AspNetCore.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Threading.Tasks;
using TreeCore.BackEnd.Model.Entity.Contracts;
using TreeCore.BackEnd.Service.Services.Contracts;
using TreeCore.BackEnd.ServiceDependencies.Services;
using TreeCore.Shared.DTO.Contracts;

namespace TreeCore.BackEnd.TestUnit
{
    [TestClass]
    public class GetContractStateTestUnit
    {
        public Mock<GetDependencies<ContractStatusDTO, ContractStatusEntity>> _dependencies;
        public GetContractStatus Subject;
        public string Code = "Test1";
        public int CoreContractStateID = 1;
        public readonly ContractStatusEntity ContractStateType;
        public readonly ContractStatusEntity ContractState;

        public GetContractStateTestUnit()
        {
            Mock<GetDependencies<ContractStatusDTO, ContractStatusEntity>> dependencies = new Mock<GetDependencies<ContractStatusDTO, ContractStatusEntity>>();

            Mock<IHttpContextAccessor> httpContextAccessor = new Mock<IHttpContextAccessor>();
            Mock<IHeaderDictionary> headerDictionary = new Mock<IHeaderDictionary>();
            httpContextAccessor.Setup(a => a.HttpContext.Request.Headers)
                .Returns(headerDictionary.Object);

            ContractStateType = new ContractStatusEntity(null, null, "test", "test", "test", false, true,false);
            //dependencies.Setup(a => a.GetItemByCode(Code))
            //.Returns(Task.FromResult(new ContractStateEntity(null, 14, "Test1", "Test Put", "test", "test", "test", true, ContractStateType)));

            _dependencies = dependencies;

            Subject = new GetContractStatus(_dependencies.Object, httpContextAccessor.Object);
        }

        [TestMethod]
        public async Task Test_allCorrect_ThenSuccess()
        {
            var state = new GetContractStateTestUnit();
            var result = await state.Subject.GetItemByCode(state.Code, Settings.Settings.Client);

            Assert.IsTrue(result.Success);
            Assert.AreEqual(Code, result.Value.Code);
        }

        [TestMethod]
        public async Task Test_ContractState_NonExistent_ThenError()
        {
            var state = new GetContractStateTestUnit();
            var result = await state.Subject.GetItemByCode("noExistent", Settings.Settings.Client);

            Assert.IsFalse(result.Success);
            Assert.AreEqual("Not Found", result.Errors[0].Message);
        }
    }
}

