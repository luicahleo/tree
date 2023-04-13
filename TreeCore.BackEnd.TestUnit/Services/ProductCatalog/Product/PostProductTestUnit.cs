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
    public class PostProductTestUnit
    {
        public Mock<PostDependencies<ProductEntity>> _postDependencies;
        public Mock<GetDependencies<ProductDTO, ProductEntity>> _getDependencies;
        public Mock<PutDependencies<ProductEntity>> _putDependencies;
        public Mock<GetDependencies<CompanyDTO, CompanyEntity>> _getCompanyDependencies;
        public Mock<GetDependencies<ProductTypeDTO, ProductTypeEntity>> _getProductTypeDependencies;
        public Mock<GetDependencies<UserDTO, UserEntity>> _getUserDependencies;
        public PostProduct Subject;
        public readonly ProductEntity productEntity;
        public readonly CompanyEntity company;
        public readonly ProductTypeEntity productType;

        public PostProductTestUnit()
        {
            //productEntity = new ProductEntity(null, "Test1", "Test Put", company, productType, 1, null, null, null, null, null, DateTime.Now, DateTime.Now, null);
            Mock<PostDependencies<ProductEntity>> postDependencies = new Mock<PostDependencies<ProductEntity>>();
            Mock<PutDependencies<ProductEntity>> putDependencies = new Mock<PutDependencies<ProductEntity>>();
            Mock<GetDependencies<ProductDTO, ProductEntity>> getDependencies = new Mock<GetDependencies<ProductDTO, ProductEntity>>();
            Mock<GetDependencies<CompanyDTO, CompanyEntity>> getCompanyDependencies = new Mock<GetDependencies<CompanyDTO, CompanyEntity>>();
            Mock<GetDependencies<ProductTypeDTO, ProductTypeEntity>> getProductTypeDependencies = new Mock<GetDependencies<ProductTypeDTO, ProductTypeEntity>>();
            Mock<GetDependencies<UserDTO, UserEntity>> getUserDependencies = new Mock<GetDependencies<UserDTO, UserEntity>>();

            Mock<IHttpContextAccessor> httpContextAccessor = new Mock<IHttpContextAccessor>();
            Mock<IHeaderDictionary> headerDictionary = new Mock<IHeaderDictionary>();
            httpContextAccessor.Setup(a => a.HttpContext.Request.Headers)
                .Returns(headerDictionary.Object);

            _postDependencies = postDependencies;
            _putDependencies = putDependencies;
            _getDependencies = getDependencies;

            _getCompanyDependencies = getCompanyDependencies;
            _getProductTypeDependencies = getProductTypeDependencies;
            _getUserDependencies = getUserDependencies;

            Subject = new PostProduct(_postDependencies.Object, _putDependencies.Object, _getDependencies.Object, httpContextAccessor.Object, 
                _getCompanyDependencies.Object, _getProductTypeDependencies.Object, _getUserDependencies.Object);

            _postDependencies.Setup(a => a.Create(It.IsAny<ProductEntity>()))
            .Returns(Subject.SaveItem(productEntity));
        }

        [TestMethod]
        public async Task Test_allCorrect_ThenSuccess()
        {
            var state = new PostProductTestUnit();
            //var result = await state.Subject.SaveItem(state.productEntity);

            //Assert.IsTrue(result.Success);
            //Assert.AreEqual("Test1", result.Value.Codigo);
            //Assert.AreEqual("Code Product Type", result.Value.CoreProductCatalogServiciosTipos.Codigo);
        }
    }
}
