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
    public class PostCompanyTestUnit
    {
        public Mock<PostDependencies<CompanyEntity>> _postDependencies;
        public Mock<GetDependencies<CompanyDTO, CompanyEntity>> _getDependencies;
        public Mock<PutDependencies<CompanyEntity>> _putDependencies;
        public Mock<GetDependencies<CompanyTypeDTO, CompanyTypeEntity>> _getCompanyTypeDependencies;
        public PostCompany Subject;
        public readonly CompanyEntity companyEntity;
        public readonly CompanyTypeEntity companyTypeEntity;

        public PostCompanyTestUnit()
        {
            companyTypeEntity = new CompanyTypeEntity(null, null, "test", "test", "test", false, true);
            //companyEntity = new CompanyEntity(null, null, "Test1", "Test1", "test", "test", "sa", true, companyTypeEntity);
            Mock<PostDependencies<CompanyEntity>> postDependencies = new Mock<PostDependencies<CompanyEntity>>();
            Mock<PutDependencies<CompanyEntity>> putDependencies = new Mock<PutDependencies<CompanyEntity>>();
            Mock<GetDependencies<CompanyDTO, CompanyEntity>> getDependencies = new Mock<GetDependencies<CompanyDTO, CompanyEntity>>();
            Mock<GetDependencies<CompanyTypeDTO, CompanyTypeEntity>> getCompanyTypeDependencies = new Mock<GetDependencies<CompanyTypeDTO, CompanyTypeEntity>>();

            Mock<IHttpContextAccessor> httpContextAccessor = new Mock<IHttpContextAccessor>();
            Mock<IHeaderDictionary> headerDictionary = new Mock<IHeaderDictionary>();
            httpContextAccessor.Setup(a => a.HttpContext.Request.Headers)
                .Returns(headerDictionary.Object);

            _postDependencies = postDependencies;
            _putDependencies = putDependencies;
            _getDependencies = getDependencies;
            _getCompanyTypeDependencies = getCompanyTypeDependencies;

            //Subject = new PostCompany(_postDependencies.Object, _getDependencies.Object, _getCompanyTypeDependencies.Object, httpContextAccessor.Object);

            _postDependencies.Setup(a => a.Create(It.IsAny<CompanyEntity>()))
            .Returns(Subject.SaveItem(companyEntity));
        }

        [TestMethod]
        public async Task Test_allCorrect_ThenSuccess()
        {
            var state = new PostCompanyTestUnit();
            var result = await state.Subject.SaveItem(state.companyEntity);

            Assert.IsTrue(result.Success);
            Assert.AreEqual("Test1", result.Value.Codigo);
            Assert.AreEqual("Code Company Type", result.Value.EntidadesTipos.Codigo);
        }
    }
}
