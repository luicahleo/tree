using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using TreeCore.BackEnd.Service.Services.ProductCatalog;
using TreeCore.Shared.DTO.ProductCatalog;

namespace TreeCore.BackEnd.TestUnit.Services.ProductCatalog
{
    [TestClass]
    public class PostCatalogTypeTestUnit
    {
        public PostCatalogType Subject;
        public readonly CatalogTypeDTO catalogTypeDTO;

        public PostCatalogTypeTestUnit()
        {
            //catalogTypeDTO = new CatalogTypeDTO() { Code = "Test1", Name = "Test1", Description = "Test Post", Active = true, Default = false };
            //Mock<IPostCatalogTypeDependencies> postDependencies = new Mock<IPostCatalogTypeDependencies>();
            //Mock<IPutCatalogTypeDependencies> putDependencies = new Mock<IPutCatalogTypeDependencies>();
            //Mock<IGetCatalogTypeDependencies> getDependencies = new Mock<IGetCatalogTypeDependencies>();

            //Mock<IHttpContextAccessor> httpContextAccessor = new Mock<IHttpContextAccessor>();
            //Mock<IHeaderDictionary> headerDictionary = new Mock<IHeaderDictionary>();
            //httpContextAccessor.Setup(a => a.HttpContext.Request.Headers)
            //    .Returns(headerDictionary.Object);

            //_postDependencies = postDependencies;
            //_putDependencies = putDependencies;
            //_getDependencies = getDependencies;

            //Subject = new PostCatalogType(_postDependencies.Object, _getDependencies.Object, _putDependencies.Object, httpContextAccessor.Object);

            //_postDependencies.Setup(a => a.InsertItem(It.IsAny<CatalogTypeEntity>()))
            //.Returns(Subject.SaveItem(catalogTypeDTO));
        }

        [TestMethod]
        public async Task Test_allCorrect_ThenSuccess()
        {
            var state = new PostCatalogTypeTestUnit();
            //var result = await state.Subject.Create(state.catalogTypeDTO, Settings.Settings.Client);

            //Assert.IsTrue(result.Success);
            //Assert.AreEqual("Test1", result.Value.Code);
            //Assert.AreEqual("Test Post", result.Value.Description);
        }
    }
}
