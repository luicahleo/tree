using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TreeCore.BackEnd.TestUnit.Services.Contracts
{
    [TestClass]
    public class PostContractTypeTestUnit
    {
        //public Mock<IPostContractTypeDependencies> _postDependencies;
        //public Mock<IGetContractTypeDependencies> _getDependencies;
        //public Mock<IPutContractTypeDependencies> _putDependencies;
        //public PostContractType Subject;
        //public readonly ContractTypeDTO contractTypeDTO;

        //public PostContractTypeTestUnit()
        //{
        //    contractTypeDTO = new ContractTypeDTO() { Code = "Test1", Name = "Test1", Description = "Test Post", Active = true, Default = false };
        //    Mock<IPostContractTypeDependencies> postDependencies = new Mock<IPostContractTypeDependencies>();
        //    Mock<IPutContractTypeDependencies> putDependencies = new Mock<IPutContractTypeDependencies>();
        //    Mock<IGetContractTypeDependencies> getDependencies = new Mock<IGetContractTypeDependencies>();

        //    Mock<IHttpContextAccessor> httpContextAccessor = new Mock<IHttpContextAccessor>();
        //    Mock<IHeaderDictionary> headerDictionary = new Mock<IHeaderDictionary>();
        //    httpContextAccessor.Setup(a => a.HttpContext.Request.Headers)
        //        .Returns(headerDictionary.Object);

        //    _postDependencies = postDependencies;
        //    _putDependencies = putDependencies;
        //    _getDependencies = getDependencies;

        //    Subject = new PostContractType(_postDependencies.Object, _getDependencies.Object, _putDependencies.Object, httpContextAccessor.Object);

        //    _postDependencies.Setup(a => a.InsertItem(It.IsAny<ContractTypeEntity>()))
        //    .Returns(Subject.SaveItem(contractTypeDTO));
        //}

        //[TestMethod]
        //public async Task Test_allCorrect_ThenSuccess()
        //{
        //    var state = new PostContractTypeTestUnit();
        //    var result = await state.Subject.Create(state.contractTypeDTO);

        //    Assert.IsTrue(result.Success);
        //    Assert.AreEqual("Test1", result.Value.Code);
        //    Assert.AreEqual("Test Post", result.Value.Description);
        //}
    }
}
