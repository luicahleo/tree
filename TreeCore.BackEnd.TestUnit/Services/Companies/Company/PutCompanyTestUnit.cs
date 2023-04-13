using Microsoft.AspNetCore.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Threading.Tasks;
using TreeCore.BackEnd.Model.Entity.Companies;
using TreeCore.BackEnd.Service.Services.Companies;
using TreeCore.BackEnd.ServiceDependencies.Services;
using TreeCore.Shared.DTO.Companies;

namespace TreeCore.BackEnd.TestUnit.Services.Companies
{
    [TestClass]
    public class PutCompanyTestUnit
    {
        public Mock<GetDependencies<CompanyDTO, CompanyEntity>> _getDependencies;
        public Mock<PutDependencies<CompanyEntity>> _putDependencies;
        public Mock<GetDependencies<CompanyTypeDTO, CompanyTypeEntity>> _getCompanyTypeDependencies;
        public PutCompany Subject;
        public readonly CompanyEntity companyEntity;
        public readonly CompanyEntity company;
        public readonly CompanyTypeEntity companyType;

        public PutCompanyTestUnit()
        {
            companyType = new CompanyTypeEntity(null, null, "test", "test", "test", false, true);
            //companyEntity = new CompanyEntity(null, 14, "Test1", "Test Put", "test", "test", "test", true, companyType);
            Mock<PutDependencies<CompanyEntity>> putDependencies = new Mock<PutDependencies<CompanyEntity>>();
            Mock<GetDependencies<CompanyDTO, CompanyEntity>> getDependencies = new Mock<GetDependencies<CompanyDTO, CompanyEntity>>();
            Mock<GetDependencies<CompanyTypeDTO, CompanyTypeEntity>> getCompanyTypeDependencies = new Mock<GetDependencies<CompanyTypeDTO, CompanyTypeEntity>>();

            Mock<IHttpContextAccessor> httpContextAccessor = new Mock<IHttpContextAccessor>();
            Mock<IHeaderDictionary> headerDictionary = new Mock<IHeaderDictionary>();
            httpContextAccessor.Setup(a => a.HttpContext.Request.Headers)
                .Returns(headerDictionary.Object);

            _putDependencies = putDependencies;
            _getDependencies = getDependencies;
            _getCompanyTypeDependencies = getCompanyTypeDependencies;

            //Subject = new PutCompany(_putDependencies.Object, _getDependencies.Object, _getCompanyTypeDependencies.Object, httpContextAccessor.Object);

            _putDependencies.Setup(a => a.Update(It.IsAny<CompanyEntity>()))
            .Returns(Subject.SaveItem(companyEntity));
        }

        [TestMethod]
        public async Task Test_allCorrect_ThenSuccess()
        {
            var state = new PutCompanyTestUnit();
            var result = await state.Subject.SaveItem(state.companyEntity);

            Assert.IsTrue(result.Success);
            Assert.AreEqual("Test1", result.Value.Codigo);
            Assert.AreEqual("Test Put", result.Value.Nombre);
        }
    }
}
