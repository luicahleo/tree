using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TreeCore.BackEnd.TestUnit.Services.Contracts
{
    [TestClass]
    public class PutContractGroupTestUnit
    {
        //public Mock<IGetContractGroupDependencies> _getDependencies;
        //public Mock<IPutContractGroupDependencies> _putDependencies;
        //public PutContractGroup Subject;
        //public readonly ContractGroupEntity ContractGroupEntity;

        //public PutContractGroupTestUnit()
        //{
        //    ContractGroupEntity = new ContractGroupEntity(null, null, "Test1", "Test1", "Test Put", true, false);
        //    Mock<IPutContractGroupDependencies> putDependencies = new Mock<IPutContractGroupDependencies>();
        //    Mock<IGetContractGroupDependencies> getDependencies = new Mock<IGetContractGroupDependencies>();

        //    Mock<IHttpContextAccessor> httpContextAccessor = new Mock<IHttpContextAccessor>();
        //    Mock<IHeaderDictionary> headerDictionary = new Mock<IHeaderDictionary>();
        //    httpContextAccessor.Setup(a => a.HttpContext.Request.Headers)
        //        .Returns(headerDictionary.Object);

        //    _putDependencies = putDependencies;
        //    _getDependencies = getDependencies;

        //    Subject = new PutContractGroup(_putDependencies.Object, _getDependencies.Object, httpContextAccessor.Object);

        //    _putDependencies.Setup(a => a.UpdateContractGroup(It.IsAny<ContractGroupEntity>()))
        //    .Returns(Subject.SaveContractGroup(ContractGroupEntity));
        //}

        //[TestMethod]
        //public async Task Test_allCorrect_ThenSuccess()
        //{
        //    var state = new PutContractGroupTestUnit();
        //    var result = await state.Subject.SaveContractGroup(state.ContractGroupEntity);

        //    Assert.IsTrue(result.Success);
        //    Assert.AreEqual("Test1", result.Value.Codigo);
        //    Assert.AreEqual("Test Put", result.Value.Descripcion);
        //}
    }
}
