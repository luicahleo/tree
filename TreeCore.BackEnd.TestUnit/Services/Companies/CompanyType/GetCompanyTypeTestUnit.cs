using Microsoft.AspNetCore.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Threading.Tasks;
using TreeCore.BackEnd.Model.Entity.Companies;
using TreeCore.BackEnd.Service.Services.Companies;

namespace TreeCore.BackEnd.TestUnit
{
    [TestClass]
    public class GetCompanyTypeTestUnit
    {
        public Mock<GetCompanyType> _dependencies;
        public GetCompanyType Subject;
        public string Code = "ty";
        public int CoreCompanyTypeID = 1;

        public GetCompanyTypeTestUnit()
        {
            Mock<GetCompanyType> dependencies = new Mock<GetCompanyType>();

            Mock<IHttpContextAccessor> httpContextAccessor = new Mock<IHttpContextAccessor>();
            Mock<IHeaderDictionary> headerDictionary = new Mock<IHeaderDictionary>();
            httpContextAccessor.Setup(a => a.HttpContext.Request.Headers)
                .Returns(headerDictionary.Object);

            //dependencies.Setup(a => a.GetItemByCode(Code))
            //.Returns(Task.FromResult(new CompanyTypeEntity(CoreCompanyTypeID, 1, Code, "Nombre", "Descripcion", true, false)));

            _dependencies = dependencies;

            //Subject = new GetCompanyType(_dependencies.Object, httpContextAccessor.Object);
        }

        [TestMethod]
        public async Task Test_allCorrect_ThenSuccess()
        {
            var state = new GetCompanyTypeTestUnit();
            var result = await state.Subject.GetItemByCode(state.Code, Settings.Settings.Client);

            Assert.IsTrue(result.Success);
            Assert.AreEqual(Code, result.Value.Code);
        }

        [TestMethod]
        public async Task Test_User_NonExistent_ThenError()
        {
            var state = new GetCompanyTypeTestUnit();
            var result = await state.Subject.GetItemByCode("NoExiste", Settings.Settings.Client);

            Assert.IsFalse(result.Success);
            Assert.AreEqual("Not Found", result.Errors[0].Message);
        }
    }
}
