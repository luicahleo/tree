using Microsoft.AspNetCore.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Threading.Tasks;
using TreeCore.BackEnd.Model.Entity.ProductCatalog;
using TreeCore.BackEnd.Service.Services.ProductCatalog;
using TreeCore.BackEnd.ServiceDependencies.Services;
using TreeCore.Shared.DTO.ProductCatalog;

namespace TreeCore.BackEnd.TestUnit
{
    [TestClass]
    public class GetProductTypeTestUnit
    {
        public Mock<GetDependencies<ProductTypeDTO, ProductTypeEntity>> _dependencies;
        public GetProductType Subject;
        public string Code = "ty";
        public int CoreProductTypeID = 1;

        public GetProductTypeTestUnit()
        {
            Mock<GetDependencies<ProductTypeDTO, ProductTypeEntity>> dependencies = new Mock<GetDependencies<ProductTypeDTO, ProductTypeEntity>>();

            Mock<IHttpContextAccessor> httpContextAccessor = new Mock<IHttpContextAccessor>();
            Mock<IHeaderDictionary> headerDictionary = new Mock<IHeaderDictionary>();
            httpContextAccessor.Setup(a => a.HttpContext.Request.Headers)
                .Returns(headerDictionary.Object);

            dependencies.Setup(a => a.GetItemByCode(Code, Settings.Settings.Client))
            .Returns(Task.FromResult(new ProductTypeEntity(CoreProductTypeID, Code, "Nombre", "Descripcion", true, false)));

            _dependencies = dependencies;

            Subject = new GetProductType(_dependencies.Object, httpContextAccessor.Object);
        }

        [TestMethod]
        public async Task Test_allCorrect_ThenSuccess()
        {
            var state = new GetProductTypeTestUnit();
            var result = await state.Subject.GetItemByCode(state.Code, Settings.Settings.Client);

            Assert.IsTrue(result.Success);
            Assert.AreEqual(Code, result.Value.Code);
        }

        [TestMethod]
        public async Task Test_User_NonExistent_ThenError()
        {
            var state = new GetProductTypeTestUnit();
            var result = await state.Subject.GetItemByCode("noExistent", Settings.Settings.Client);

            Assert.IsFalse(result.Success);
            Assert.AreEqual("Not Found", result.Errors[0].Message);
        }
    }
}
