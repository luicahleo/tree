using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TreeCore.BackEnd.TestUnit.Services.Contracts
{
    [TestClass]
    public class PostFunctionalAreaTestUnit
    {
        //public Mock<IPostFunctionalAreaDependencies> _postDependencies;
        //public Mock<IGetFunctionalAreaDependencies> _getDependencies;
        //public Mock<IPutFunctionalAreaDependencies> _putDependencies;
        //public PostFunctionalArea Subject;
        //public readonly FunctionalAreaDTO functionalAreaDTO;

        //public PostFunctionalAreaTestUnit()
        //{
        //    functionalAreaDTO = new FunctionalAreaDTO() { Code = "Test1", Name = "Test1", Description = "Test Post", Active = true, Default = false };
        //    Mock<IPostFunctionalAreaDependencies> postDependencies = new Mock<IPostFunctionalAreaDependencies>();
        //    Mock<IPutFunctionalAreaDependencies> putDependencies = new Mock<IPutFunctionalAreaDependencies>();
        //    Mock<IGetFunctionalAreaDependencies> getDependencies = new Mock<IGetFunctionalAreaDependencies>();

        //    Mock<IHttpContextAccessor> httpContextAccessor = new Mock<IHttpContextAccessor>();
        //    Mock<IHeaderDictionary> headerDictionary = new Mock<IHeaderDictionary>();
        //    httpContextAccessor.Setup(a => a.HttpContext.Request.Headers)
        //        .Returns(headerDictionary.Object);

        //    _postDependencies = postDependencies;
        //    _putDependencies = putDependencies;
        //    _getDependencies = getDependencies;

        //    Subject = new PostFunctionalArea(_postDependencies.Object, _getDependencies.Object, _putDependencies.Object, httpContextAccessor.Object);

        //    _postDependencies.Setup(a => a.InsertItem(It.IsAny<FunctionalAreaEntity>()))
        //    .Returns(Subject.SaveItem(functionalAreaDTO));
        //}

        //[TestMethod]
        //public async Task Test_allCorrect_ThenSuccess()
        //{
        //    var state = new PostFunctionalAreaTestUnit();
        //    var result = await state.Subject.Create(state.functionalAreaDTO);

        //    Assert.IsTrue(result.Success);
        //    Assert.AreEqual("Test1", result.Value.Code);
        //    Assert.AreEqual("Test Post", result.Value.Description);
        //}
    }
}
