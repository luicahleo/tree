using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TreeCore.BackEnd.TestUnit.Services.Contracts
{
    [TestClass]
    public class PutContractTypeTestUnit
    {
        //public Mock<IGetContractTypeDependencies> _getDependencies;
        //public Mock<IPutContractTypeDependencies> _putDependencies;
        //public PutContractType Subject;
        //public readonly ContractTypeEntity contractTypeEntity;

        //public PutContractTypeTestUnit()
        //{
        //    contractTypeEntity = new ContractTypeEntity(null, null, "Test1", "Test1", "Test Put", true, false);
        //    Mock<IPutContractTypeDependencies> putDependencies = new Mock<IPutContractTypeDependencies>();
        //    Mock<IGetContractTypeDependencies> getDependencies = new Mock<IGetContractTypeDependencies>();

        //    Mock<IHttpContextAccessor> httpContextAccessor = new Mock<IHttpContextAccessor>();
        //    Mock<IHeaderDictionary> headerDictionary = new Mock<IHeaderDictionary>();
        //    httpContextAccessor.Setup(a => a.HttpContext.Request.Headers)
        //        .Returns(headerDictionary.Object);

        //    _putDependencies = putDependencies;
        //    _getDependencies = getDependencies;

        //    Subject = new PutContractType(_putDependencies.Object, _getDependencies.Object, httpContextAccessor.Object);

        //    _putDependencies.Setup(a => a.UpdateContractType(It.IsAny<ContractTypeEntity>()))
        //    .Returns(Subject.SaveContractType(contractTypeEntity));
        //}

        //[TestMethod]
        //public async Task Test_allCorrect_ThenSuccess()
        //{
        //    var state = new PutContractTypeTestUnit();
        //    var result = await state.Subject.SaveContractType(state.contractTypeEntity);

        //    Assert.IsTrue(result.Success);
        //    Assert.AreEqual("Test1", result.Value.Codigo);
        //    Assert.AreEqual("Test Put", result.Value.Descripcion);
        //}
    }
}
