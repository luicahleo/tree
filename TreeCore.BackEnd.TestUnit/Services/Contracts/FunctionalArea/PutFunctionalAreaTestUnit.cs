using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TreeCore.BackEnd.TestUnit.Services.Contracts
{
    [TestClass]
    public class PutFunctionalAreaTestUnit
    {
        //public Mock<IGetFunctionalAreaDependencies> _getDependencies;
        //public Mock<IPutFunctionalAreaDependencies> _putDependencies;
        //public PutFunctionalArea Subject;
        //public readonly FunctionalAreaEntity functionalAreaEntity;

        //public PutFunctionalAreaTestUnit()
        //{
        //    functionalAreaEntity = new FunctionalAreaEntity(null, null, "Test1", "Test1", "Test Put", true, false);
        //    Mock<IPutFunctionalAreaDependencies> putDependencies = new Mock<IPutFunctionalAreaDependencies>();
        //    Mock<IGetFunctionalAreaDependencies> getDependencies = new Mock<IGetFunctionalAreaDependencies>();

        //    Mock<IHttpContextAccessor> httpContextAccessor = new Mock<IHttpContextAccessor>();
        //    Mock<IHeaderDictionary> headerDictionary = new Mock<IHeaderDictionary>();
        //    httpContextAccessor.Setup(a => a.HttpContext.Request.Headers)
        //        .Returns(headerDictionary.Object);

        //    _putDependencies = putDependencies;
        //    _getDependencies = getDependencies;

        //    Subject = new PutFunctionalArea(_putDependencies.Object, _getDependencies.Object, httpContextAccessor.Object);

        //    _putDependencies.Setup(a => a.UpdateFunctionalArea(It.IsAny<FunctionalAreaEntity>()))
        //    .Returns(Subject.SaveFunctionalArea(functionalAreaEntity));
        //}

        //[TestMethod]
        //public async Task Test_allCorrect_ThenSuccess()
        //{
        //    var state = new PutFunctionalAreaTestUnit();
        //    var result = await state.Subject.SaveFunctionalArea(state.functionalAreaEntity);

        //    Assert.IsTrue(result.Success);
        //    Assert.AreEqual("Test1", result.Value.Codigo);
        //    Assert.AreEqual("Test Put", result.Value.Descripcion);
        //}
    }
}
