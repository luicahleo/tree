using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using TreeCore.BackEnd.Model.Entity.ProductCatalog;
using TreeCore.BackEnd.Service.Services.ProductCatalog;

namespace TreeCore.BackEnd.TestUnit.Services.ProductCatalog
{
    [TestClass]
    public class DeleteCatalogTypeTestUnit
    {
        //public Mock<IGetCatalogTypeDependencies> _getDependencies;
        //public Mock<IDeleteCatalogTypeDependencies> _deleteDependencies;
        public DeleteCatalogType Subject;
        public readonly CatalogTypeEntity catalogTypeEntity;

        public DeleteCatalogTypeTestUnit()
        {
            //catalogTypeEntity = new CatalogTypeEntity(null, null, "Test1", "Test1", "Test Put", true, false);
            //Mock<IDeleteCatalogTypeDependencies> deleteDependencies = new Mock<IDeleteCatalogTypeDependencies>();
            //Mock<IGetCatalogTypeDependencies> getDependencies = new Mock<IGetCatalogTypeDependencies>();

            //Mock<IHttpContextAccessor> httpContextAccessor = new Mock<IHttpContextAccessor>();
            //Mock<IHeaderDictionary> headerDictionary = new Mock<IHeaderDictionary>();
            //httpContextAccessor.Setup(a => a.HttpContext.Request.Headers)
            //    .Returns(headerDictionary.Object);

            //_deleteDependencies = deleteDependencies;
            //_getDependencies = getDependencies;

            //Subject = new DeleteCatalogType(_deleteDependencies.Object, _getDependencies.Object, httpContextAccessor.Object);

            //_deleteDependencies.Setup(a => a.DeleteCatalogType(It.IsAny<CatalogTypeEntity>()))
            //.Returns(Task.FromResult(Result.Success(1)));
        }

        [TestMethod]
        public async Task Test_allCorrect_ThenSuccess()
        {
            //var stateDel = new DeleteCatalogTypeTestUnit();
            //var result = await stateDel.Subject.Delete(stateDel.catalogTypeEntity);

            //Assert.IsTrue(result.Success);
            //Assert.AreEqual(1, result.Value);
        }
    }
}
