using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using TreeCore.BackEnd.Service.Services.ProductCatalog;

namespace TreeCore.BackEnd.TestUnit
{
    [TestClass]
    public class GetCatalogTypeTestUnit
    {
        public GetCatalogType Subject;
        public string Code = "ty";
        public int CoreCatalogTypeID = 1;

        public GetCatalogTypeTestUnit()
        {
            //Mock<IGetCatalogTypeDependencies> dependencies = new Mock<IGetCatalogTypeDependencies>();

            //Mock<IHttpContextAccessor> httpContextAccessor = new Mock<IHttpContextAccessor>();
            //Mock<IHeaderDictionary> headerDictionary = new Mock<IHeaderDictionary>();
            //httpContextAccessor.Setup(a => a.HttpContext.Request.Headers)
            //    .Returns(headerDictionary.Object);

            //dependencies.Setup(a => a.GetItemByCode(Code))
            //.Returns(Task.FromResult(new CatalogTypeEntity(CoreCatalogTypeID, null, Code, "Nombre", "Descripcion", true, false)));

            //_dependencies = dependencies;

            //Subject = new GetCatalogType(_dependencies.Object, httpContextAccessor.Object);
        }

        [TestMethod]
        public async Task Test_allCorrect_ThenSuccess()
        {
            var state = new GetCatalogTypeTestUnit();
            var result = await state.Subject.GetItemByCode(state.Code, Settings.Settings.Client);

            Assert.IsTrue(result.Success);
            Assert.AreEqual(Code, result.Value.Code);
        }

        [TestMethod]
        public async Task Test_User_NonExistent_ThenError()
        {
            var state = new GetCatalogTypeTestUnit();
            var result = await state.Subject.GetItemByCode("noExistent", Settings.Settings.Client);

            Assert.IsFalse(result.Success);
            Assert.AreEqual("Not ", result.Errors[0].Message);
        }
    }
}
