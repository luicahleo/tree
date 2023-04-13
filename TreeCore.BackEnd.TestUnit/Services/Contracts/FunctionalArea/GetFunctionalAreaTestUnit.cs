using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TreeCore.BackEnd.TestUnit.Services.Contracts
{
    [TestClass]
    public class GetFunctionalAreaTestUnit
    {
        //public Mock<IGetFunctionalAreaDependencies> _dependencies;
        //public GetFunctionalArea Subject;
        //public string Code = "ty";
        //public int CoreFunctionalAreaID = 1;

        //public GetFunctionalAreaTestUnit()
        //{
        //    Mock<IGetFunctionalAreaDependencies> dependencies = new Mock<IGetFunctionalAreaDependencies>();

        //    Mock<IHttpContextAccessor> httpContextAccessor = new Mock<IHttpContextAccessor>();
        //    Mock<IHeaderDictionary> headerDictionary = new Mock<IHeaderDictionary>();
        //    httpContextAccessor.Setup(a => a.HttpContext.Request.Headers)
        //        .Returns(headerDictionary.Object);

        //    dependencies.Setup(a => a.GetItemByCode(Code))
        //    .Returns(Task.FromResult(new FunctionalAreaEntity(CoreFunctionalAreaID, null, Code, "Nombre", "Descripcion", true, false)));

        //    _dependencies = dependencies;

        //    Subject = new GetFunctionalArea(_dependencies.Object, httpContextAccessor.Object);
        //}

        //[TestMethod]
        //public async Task Test_allCorrect_ThenSuccess()
        //{
        //    var state = new GetFunctionalAreaTestUnit();
        //    var result = await state.Subject.GetItemByCode(state.Code);

        //    Assert.IsTrue(result.Success);
        //    Assert.AreEqual(Code, result.Value.Code);
        //}

        //[TestMethod]
        //public async Task Test_User_NonExistent_ThenError()
        //{
        //    var state = new GetFunctionalAreaTestUnit();
        //    var result = await state.Subject.GetItemByCode("noExistent");

        //    Assert.IsFalse(result.Success);
        //    Assert.AreEqual("Not Found", result.Errors[0].Message);
        //}
    }
}