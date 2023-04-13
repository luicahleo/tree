using Microsoft.AspNetCore.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Threading.Tasks;
using TreeCore.BackEnd.Model.Entity.Companies;
using TreeCore.BackEnd.Model.Entity.ProductCatalog;
using TreeCore.BackEnd.Service.Services.ProductCatalog;
using TreeCore.BackEnd.ServiceDependencies.Services;
using TreeCore.Shared.DTO.ProductCatalog;
using TreeCore.Shared.ROP;

namespace TreeCore.BackEnd.TestUnit.Services.ProductCatalog
{
    [TestClass]
    public class DeleteProductTestUnit
    {
        public Mock<GetDependencies<ProductDTO, ProductEntity>> _getDependencies;
        public Mock<DeleteDependencies<ProductEntity>> _deleteDependencies;
        public DeleteProduct Subject;
        public readonly ProductEntity productEntity;
        public readonly CompanyEntity company;
        public readonly ProductTypeEntity productType;

        public DeleteProductTestUnit()
        {
            //productEntity = new ProductEntity(null, "Test1", "Test Put", company, productType, 1, null, null, null, null, null, DateTime.Now, DateTime.Now, null);
            Mock<DeleteDependencies<ProductEntity>> deleteDependencies = new Mock<DeleteDependencies<ProductEntity>>();
            Mock<GetDependencies<ProductDTO, ProductEntity>> getDependencies = new Mock<GetDependencies<ProductDTO, ProductEntity>>();

            Mock<IHttpContextAccessor> httpContextAccessor = new Mock<IHttpContextAccessor>();
            Mock<IHeaderDictionary> headerDictionary = new Mock<IHeaderDictionary>();
            httpContextAccessor.Setup(a => a.HttpContext.Request.Headers)
                .Returns(headerDictionary.Object);

            _deleteDependencies = deleteDependencies;
            _getDependencies = getDependencies;

            Subject = new DeleteProduct(_deleteDependencies.Object, _getDependencies.Object, httpContextAccessor.Object);

            _deleteDependencies.Setup(a => a.Delete(It.IsAny<ProductEntity>()))
            .Returns(Task.FromResult(Result.Success(1)));
        }

        [TestMethod]
        public async Task Test_allCorrect_ThenSuccess()
        {
            var stateDel = new DeleteProductTestUnit();
            var result = await stateDel.Subject.Delete(stateDel.productEntity.Codigo, Settings.Settings.Client);

            Assert.IsTrue(result.Success);
            Assert.AreEqual(1, result.Value);
        }
    }
}
