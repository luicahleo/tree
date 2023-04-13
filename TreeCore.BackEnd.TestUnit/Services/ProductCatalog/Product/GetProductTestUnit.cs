using Microsoft.AspNetCore.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Threading.Tasks;
using TreeCore.BackEnd.Model.Entity.Companies;
using TreeCore.BackEnd.Model.Entity.ProductCatalog;
using TreeCore.BackEnd.Service.Services.Companies;
using TreeCore.BackEnd.Service.Services.ProductCatalog;
using TreeCore.BackEnd.ServiceDependencies.Services;
using TreeCore.Shared.DTO.ProductCatalog;

namespace TreeCore.BackEnd.TestUnit
{
    [TestClass]
    public class GetProductTestUnit
    {
        public Mock<GetDependencies<ProductDTO, ProductEntity>> _dependencies;
        public Mock<GetDependencies<ProductDetailsDTO, ProductEntity>> _detailsDependencies;
        public Mock<GetCompany> _getCompany;
        public Mock<GetProductType> _getProductType;
        public GetProduct Subject;
        public string Code = "test";
        public int CoreProductID = 1;
        public readonly ProductTypeEntity productType;
        public readonly CompanyEntity company;

        public GetProductTestUnit()
        {
            Mock<GetDependencies<ProductDTO, ProductEntity>> dependencies = new Mock<GetDependencies<ProductDTO, ProductEntity>>();

            Mock<IHttpContextAccessor> httpContextAccessor = new Mock<IHttpContextAccessor>();
            Mock<IHeaderDictionary> headerDictionary = new Mock<IHeaderDictionary>();
            httpContextAccessor.Setup(a => a.HttpContext.Request.Headers)
                .Returns(headerDictionary.Object);

            //dependencies.Setup(a => a.GetItemByCode(Code))
            //.Returns(Task.FromResult(new ProductEntity(CoreProductID, Code, "Nombre", company, productType, 1, null, null, null, null, null, DateTime.Now, DateTime.Now, null)));

            _dependencies = dependencies;

            Subject = new GetProduct(_dependencies.Object, httpContextAccessor.Object, _detailsDependencies.Object, _getCompany.Object, _getProductType.Object);
        }

        [TestMethod]
        public async Task Test_allCorrect_ThenSuccess()
        {
            var state = new GetProductTestUnit();
            var result = await state.Subject.GetItemByCode(state.Code, Settings.Settings.Client);

            Assert.IsTrue(result.Success);
            Assert.AreEqual(Code, result.Value.Code);
        }

        [TestMethod]
        public async Task Test_Product_NonExistent_ThenError()
        {
            var state = new GetProductTestUnit();
            var result = await state.Subject.GetItemByCode("noExistent", Settings.Settings.Client);

            Assert.IsFalse(result.Success);
            Assert.AreEqual("Not Found", result.Errors[0].Message);
        }
    }
}

