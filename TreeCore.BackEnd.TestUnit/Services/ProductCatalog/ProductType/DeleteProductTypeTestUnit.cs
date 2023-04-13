using Microsoft.AspNetCore.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Threading.Tasks;
using TreeCore.BackEnd.Model.Entity.ProductCatalog;
using TreeCore.BackEnd.Service.Services.ProductCatalog;
using TreeCore.BackEnd.ServiceDependencies.Services;
using TreeCore.Shared.DTO.ProductCatalog;
using TreeCore.Shared.ROP;

namespace TreeCore.BackEnd.TestUnit.Services.ProductTypeCatalog
{
    [TestClass]
    public class DeleteProductTypeTestUnit
    {
        public Mock<GetDependencies<ProductTypeDTO, ProductTypeEntity>> _getDependencies;
        public Mock<DeleteDependencies<ProductTypeEntity>> _deleteDependencies;
        public DeleteProductType Subject;
        public readonly ProductTypeEntity productTypeEntity;

        public DeleteProductTypeTestUnit()
        {
            productTypeEntity = new ProductTypeEntity(null, "Test", "Nombre", "Descripcion", true, false);
            Mock<DeleteDependencies<ProductTypeEntity>> deleteDependencies = new Mock<DeleteDependencies<ProductTypeEntity>>();
            Mock<GetDependencies<ProductTypeDTO, ProductTypeEntity>> getDependencies = new Mock<GetDependencies<ProductTypeDTO, ProductTypeEntity>>();

            Mock<IHttpContextAccessor> httpContextAccessor = new Mock<IHttpContextAccessor>();
            Mock<IHeaderDictionary> headerDictionary = new Mock<IHeaderDictionary>();
            httpContextAccessor.Setup(a => a.HttpContext.Request.Headers)
                .Returns(headerDictionary.Object);

            _deleteDependencies = deleteDependencies;
            _getDependencies = getDependencies;

            Subject = new DeleteProductType(_deleteDependencies.Object, _getDependencies.Object, httpContextAccessor.Object);

            _deleteDependencies.Setup(a => a.Delete(It.IsAny<ProductTypeEntity>()))
            .Returns(Task.FromResult(Result.Success(1)));
        }

        [TestMethod]
        public async Task Test_allCorrect_ThenSuccess()
        {
            var stateDel = new DeleteProductTypeTestUnit();
            var result = await stateDel.Subject.Delete(stateDel.productTypeEntity.Codigo, Settings.Settings.Client);

            Assert.IsTrue(result.Success);
            Assert.AreEqual(1, result.Value);
        }
    }
}

