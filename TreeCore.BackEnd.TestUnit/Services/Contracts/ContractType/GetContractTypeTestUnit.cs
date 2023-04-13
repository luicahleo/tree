using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TreeCore.BackEnd.TestUnit.Services.Contracts
{
    [TestClass]
    public class GetContractTypeTestUnit
    {
        //public Mock<IGetContractTypeDependencies> _dependencies;
        //public GetContractType Subject;
        //public string Code = "ty";
        //public int CoreContractTypeID = 1;

        //public GetContractTypeTestUnit()
        //{
        //    Mock<IGetContractTypeDependencies> dependencies = new Mock<IGetContractTypeDependencies>();

        //    Mock<IHttpContextAccessor> httpContextAccessor = new Mock<IHttpContextAccessor>();
        //    Mock<IHeaderDictionary> headerDictionary = new Mock<IHeaderDictionary>();
        //    httpContextAccessor.Setup(a => a.HttpContext.Request.Headers)
        //        .Returns(headerDictionary.Object);

        //    dependencies.Setup(a => a.GetItemByCode(Code))
        //    .Returns(Task.FromResult(new ContractTypeEntity(CoreContractTypeID, null, Code, "Nombre", "Descripcion", true, false)));

        //    _dependencies = dependencies;

        //    Subject = new GetContractType(_dependencies.Object, httpContextAccessor.Object);
        //}

        //[TestMethod]
        //public async Task Test_allCorrect_ThenSuccess()
        //{
        //    var state = new GetContractTypeTestUnit();
        //    var result = await state.Subject.GetItemByCode(state.Code);

        //    Assert.IsTrue(result.Success);
        //    Assert.AreEqual(Code, result.Value.Code);
        //}

        //[TestMethod]
        //public async Task Test_User_NonExistent_ThenError()
        //{
        //    var state = new GetContractTypeTestUnit();
        //    var result = await state.Subject.GetItemByCode("noExistent");

        //    Assert.IsFalse(result.Success);
        //    Assert.AreEqual("Not Found", result.Errors[0].Message);
        //}
    }
}
