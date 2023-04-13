using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TreeCore.BackEnd.TestUnit.Services.Companies
{
    [TestClass]
    public class PostCompanyTypeTestUnit
    {
    //    public Mock<IPostCompanyTypeDependencies> _postDependencies;
    //    public Mock<IGetCompanyTypeDependencies> _getDependencies;
    //    public Mock<IPutCompanyTypeDependencies> _putDependencies;
    //    public PostCompanyType Subject;
    //    public readonly CompanyTypeDTO companyTypeDTO;

    //    public PostCompanyTypeTestUnit()
    //    {
    //        companyTypeDTO = new CompanyTypeDTO() { Code = "Test1", Name = "Test1", Description = "Test Post", Active = true, Default = false };
    //        Mock<IPostCompanyTypeDependencies> postDependencies = new Mock<IPostCompanyTypeDependencies>();
    //        Mock<IPutCompanyTypeDependencies> putDependencies = new Mock<IPutCompanyTypeDependencies>();
    //        Mock<IGetCompanyTypeDependencies> getDependencies = new Mock<IGetCompanyTypeDependencies>();

    //        Mock<IHttpContextAccessor> httpContextAccessor = new Mock<IHttpContextAccessor>();
    //        Mock<IHeaderDictionary> headerDictionary = new Mock<IHeaderDictionary>();
    //        httpContextAccessor.Setup(a => a.HttpContext.Request.Headers)
    //            .Returns(headerDictionary.Object);

    //        _postDependencies = postDependencies;
    //        _putDependencies = putDependencies;
    //        _getDependencies = getDependencies;

    //        Subject = new PostCompanyType(_postDependencies.Object, _getDependencies.Object, _putDependencies.Object, httpContextAccessor.Object);

    //        _postDependencies.Setup(a => a.InsertItem(It.IsAny<CompanyTypeEntity>()))
    //        .Returns(Subject.SaveItem(companyTypeDTO));
    //    }

    //    [TestMethod]
    //    public async Task Test_allCorrect_ThenSuccess()
    //    {
    //        var state = new PostCompanyTypeTestUnit();
    //        var result = await state.Subject.Create(state.companyTypeDTO);

    //        Assert.IsTrue(result.Success);
    //        Assert.AreEqual("Test1", result.Value.Code);
    //        Assert.AreEqual("Test Post", result.Value.Description);
    //    }
    }
}
