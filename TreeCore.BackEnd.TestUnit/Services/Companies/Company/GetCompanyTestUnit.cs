using Microsoft.AspNetCore.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Threading.Tasks;
using TreeCore.BackEnd.Model.Entity.Companies;
using TreeCore.BackEnd.Service.Services.Companies;
using TreeCore.BackEnd.ServiceDependencies.Services;
using TreeCore.Shared.DTO.Companies;

namespace TreeCore.BackEnd.TestUnit
{
    [TestClass]
    public class GetCompanyTestUnit
    {
        public Mock<GetDependencies<CompanyDTO, CompanyEntity>> _dependencies;
        public GetCompany Subject;
        public string Code = "Test1";
        public int CoreCompanyID = 1;
        public readonly CompanyTypeEntity companyType;
        public readonly CompanyEntity company;

        public GetCompanyTestUnit()
        {
            Mock<GetDependencies<CompanyDTO, CompanyEntity>> dependencies = new Mock<GetDependencies<CompanyDTO, CompanyEntity>>();

            Mock<IHttpContextAccessor> httpContextAccessor = new Mock<IHttpContextAccessor>();
            Mock<IHeaderDictionary> headerDictionary = new Mock<IHeaderDictionary>();
            httpContextAccessor.Setup(a => a.HttpContext.Request.Headers)
                .Returns(headerDictionary.Object);

            companyType = new CompanyTypeEntity(null,null,"test", "test", "test", false, true);
            //dependencies.Setup(a => a.GetItemByCode(Code))
            //.Returns(Task.FromResult(new CompanyEntity(null, 14, "Test1", "Test Put", "test", "test", "test", true, companyType)));

            _dependencies = dependencies;

            Subject = new GetCompany(_dependencies.Object, httpContextAccessor.Object);
        }

        [TestMethod]
        public async Task Test_allCorrect_ThenSuccess()
        {
            var state = new GetCompanyTestUnit();
            var result = await state.Subject.GetItemByCode(state.Code, Settings.Settings.Client);

            Assert.IsTrue(result.Success);
            Assert.AreEqual(Code, result.Value.Code);
        }

        [TestMethod]
        public async Task Test_Company_NonExistent_ThenError()
        {
            var state = new GetCompanyTestUnit();
            var result = await state.Subject.GetItemByCode("noExistent", Settings.Settings.Client);

            Assert.IsFalse(result.Success);
            Assert.AreEqual("Not Found", result.Errors[0].Message);
        }
    }
}

