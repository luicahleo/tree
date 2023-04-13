using Microsoft.AspNetCore.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Threading.Tasks;
using TreeCore.BackEnd.Model.Entity.Companies;
using TreeCore.BackEnd.Model.Entity.General;
using TreeCore.BackEnd.Model.Entity.ProductCatalog;
using TreeCore.BackEnd.Service.Services.ProductCatalog;
using TreeCore.BackEnd.ServiceDependencies.Services;
using TreeCore.Shared.DTO.Companies;
using TreeCore.Shared.DTO.General;
using TreeCore.Shared.DTO.ProductCatalog;

namespace TreeCore.BackEnd.TestUnit.Services.ProductCatalog
{
    [TestClass]
    public class PutProductTestUnit
    {
        public Mock<GetDependencies<ProductDTO, ProductEntity>> _getDependencies;
        public Mock<PutDependencies<ProductEntity>> _putDependencies;
        public Mock<GetDependencies<UserDTO, UserEntity>> _getUserDependencies;
        public Mock<GetDependencies<CompanyDTO, CompanyEntity>> _getCompanyDependencies;
        public Mock<GetDependencies<ProductTypeDTO, ProductTypeEntity>> _getProductTypeDependencies;
        public PutProduct Subject;
        public readonly ProductEntity productEntity;
        public readonly CompanyEntity company;
        public readonly ProductTypeEntity productType;

        public PutProductTestUnit()
        {
            //productEntity = new ProductEntity(null, "Test1", "Test Put", company, productType, 1, null, null, null, null, null, DateTime.Now, DateTime.Now, null);
            Mock<PutDependencies<ProductEntity>> putDependencies = new Mock<PutDependencies<ProductEntity>>();
            Mock<GetDependencies<ProductDTO, ProductEntity>> getDependencies = new Mock<GetDependencies<ProductDTO, ProductEntity>>();
            Mock<GetDependencies<CompanyDTO, CompanyEntity>> getCompanyDependencies = new Mock<GetDependencies<CompanyDTO, CompanyEntity>>();
            Mock<GetDependencies<UserDTO, UserEntity>> getUserDependencies = new Mock<GetDependencies<UserDTO, UserEntity>>();
            Mock<GetDependencies<ProductTypeDTO, ProductTypeEntity>> getProductTypeDependencies = new Mock<GetDependencies<ProductTypeDTO, ProductTypeEntity>>();

            Mock<IHttpContextAccessor> httpContextAccessor = new Mock<IHttpContextAccessor>();
            Mock<IHeaderDictionary> headerDictionary = new Mock<IHeaderDictionary>();
            httpContextAccessor.Setup(a => a.HttpContext.Request.Headers)
                .Returns(headerDictionary.Object);

            _putDependencies = putDependencies;
            _getDependencies = getDependencies;
            _getCompanyDependencies = getCompanyDependencies;
            _getProductTypeDependencies = getProductTypeDependencies;
            _getUserDependencies = getUserDependencies;

            Subject = new PutProduct(_putDependencies.Object, httpContextAccessor.Object, _getDependencies.Object, _getCompanyDependencies.Object, 
                _getProductTypeDependencies.Object, _getUserDependencies.Object);

            _putDependencies.Setup(a => a.Update(It.IsAny<ProductEntity>()))
            .Returns(Subject.SaveItem(productEntity));
        }

        [TestMethod]
        public async Task Test_allCorrect_ThenSuccess()
        {
            var state = new PutProductTestUnit();
            var result = await state.Subject.SaveItem(state.productEntity);

            Assert.IsTrue(result.Success);
            Assert.AreEqual("Test1", result.Value.Codigo);
            Assert.AreEqual("Test Put", result.Value.Nombre);
        }
    }
}
