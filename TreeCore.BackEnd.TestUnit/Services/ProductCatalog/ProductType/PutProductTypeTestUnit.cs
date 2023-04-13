using Microsoft.AspNetCore.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Threading.Tasks;
using TreeCore.BackEnd.Model.Entity.ProductCatalog;
using TreeCore.BackEnd.Service.Services.ProductCatalog;
using TreeCore.BackEnd.ServiceDependencies.Services;
using TreeCore.Shared.DTO.ProductCatalog;

namespace TreeCore.BackEnd.TestUnit.Services.ProductCatalog
{
    [TestClass]
    public class PutProductTypeTestUnit
    {
        public Mock<GetDependencies<ProductTypeDTO, ProductTypeEntity>> _getDependencies;
        public Mock<PutDependencies<ProductTypeEntity>> _putDependencies;
        public PutProductType Subject;
        public readonly ProductTypeEntity productTypeEntity;

        public PutProductTypeTestUnit()
        {
            productTypeEntity = new ProductTypeEntity(null, "Test1", "Test1", "Test Put", true, false);
            Mock<PutDependencies<ProductTypeEntity>> putDependencies = new Mock<PutDependencies<ProductTypeEntity>>();
            Mock<GetDependencies<ProductTypeDTO, ProductTypeEntity>> getDependencies = new Mock<GetDependencies<ProductTypeDTO, ProductTypeEntity>>();

            Mock<IHttpContextAccessor> httpContextAccessor = new Mock<IHttpContextAccessor>();
            Mock<IHeaderDictionary> headerDictionary = new Mock<IHeaderDictionary>();
            httpContextAccessor.Setup(a => a.HttpContext.Request.Headers)
                .Returns(headerDictionary.Object);

            _putDependencies = putDependencies;
            _getDependencies = getDependencies;

            Subject = new PutProductType(_putDependencies.Object, _getDependencies.Object, httpContextAccessor.Object);

            _putDependencies.Setup(a => a.Update(It.IsAny<ProductTypeEntity>()))
            .Returns(Subject.SaveItem(productTypeEntity));
        }

        [TestMethod]
        public async Task Test_allCorrect_ThenSuccess()
        {
            var state = new PutProductTypeTestUnit();
            var result = await state.Subject.SaveItem(state.productTypeEntity);

            Assert.IsTrue(result.Success);
            Assert.AreEqual("Test1", result.Value.Codigo);
            Assert.AreEqual("Test Put", result.Value.Descripcion);
        }
    }
}
