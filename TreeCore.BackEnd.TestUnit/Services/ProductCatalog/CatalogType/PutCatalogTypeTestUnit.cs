using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TreeCore.BackEnd.TestUnit.Services.ProductCatalog
{
    [TestClass]
    public class PutCatalogTypeTestUnit
    {
        //public Mock<IGetCatalogTypeDependencies> _getDependencies;
        //public Mock<IPutCatalogTypeDependencies> _putDependencies;
        //public PutCatalogType Subject;
        //public readonly CatalogTypeEntity catalogTypeEntity;

        //public PutCatalogTypeTestUnit()
        //{
        //    catalogTypeEntity = new CatalogTypeEntity(null, null, "Test1", "Test1", "Test Put", true, false);
        //    Mock<IPutCatalogTypeDependencies> putDependencies = new Mock<IPutCatalogTypeDependencies>();
        //    Mock<IGetCatalogTypeDependencies> getDependencies = new Mock<IGetCatalogTypeDependencies>();

        //    Mock<IHttpContextAccessor> httpContextAccessor = new Mock<IHttpContextAccessor>();
        //    Mock<IHeaderDictionary> headerDictionary = new Mock<IHeaderDictionary>();
        //    httpContextAccessor.Setup(a => a.HttpContext.Request.Headers)
        //        .Returns(headerDictionary.Object);

        //    _putDependencies = putDependencies;
        //    _getDependencies = getDependencies;

        //    Subject = new PutCatalogType(_putDependencies.Object, _getDependencies.Object, httpContextAccessor.Object);

        //    _putDependencies.Setup(a => a.UpdateCatalogType(It.IsAny<CatalogTypeEntity>()))
        //    .Returns(Subject.SaveCatalogType(catalogTypeEntity));
        //}

        //[TestMethod]
        //public async Task Test_allCorrect_ThenSuccess()
        //{
        //    var state = new PutCatalogTypeTestUnit();
        //    var result = await state.Subject.SaveCatalogType(state.catalogTypeEntity);

        //    Assert.IsTrue(result.Success);
        //    Assert.AreEqual("Test1", result.Value.Codigo);
        //    Assert.AreEqual("Test Put", result.Value.Descripcion);
        //}
    }
}
