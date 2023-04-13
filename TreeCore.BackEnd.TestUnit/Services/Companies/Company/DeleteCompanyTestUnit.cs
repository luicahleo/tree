using Microsoft.AspNetCore.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Threading.Tasks;
using TreeCore.BackEnd.Model.Entity.Companies;
using TreeCore.BackEnd.Service.Services.Companies;
using TreeCore.BackEnd.ServiceDependencies.Services;
using TreeCore.Shared.DTO.Companies;
using TreeCore.Shared.ROP;

namespace TreeCore.BackEnd.TestUnit.Services.Companies
{
    [TestClass]
    public class DeleteCompanyTestUnit
    {
        public Mock<GetDependencies<CompanyDTO, CompanyEntity>> _getDependencies;
        public Mock<DeleteDependencies<CompanyEntity>> _deleteDependencies;
        public DeleteCompany Subject;
        public readonly CompanyEntity companyEntity;
        public readonly CompanyTypeEntity companyType;

        public DeleteCompanyTestUnit()
        {
            //companyEntity = new CompanyEntity(null,14, "Test1", "Test Put", "test","test","test",true, companyType);
            Mock<DeleteDependencies<CompanyEntity>> deleteDependencies = new Mock<DeleteDependencies<CompanyEntity>>();
            Mock<GetDependencies<CompanyDTO, CompanyEntity>> getDependencies = new Mock<GetDependencies<CompanyDTO, CompanyEntity>>();

            Mock<IHttpContextAccessor> httpContextAccessor = new Mock<IHttpContextAccessor>();
            Mock<IHeaderDictionary> headerDictionary = new Mock<IHeaderDictionary>();
            httpContextAccessor.Setup(a => a.HttpContext.Request.Headers)
                .Returns(headerDictionary.Object);

            _deleteDependencies = deleteDependencies;
            _getDependencies = getDependencies;

            Subject = new DeleteCompany(_deleteDependencies.Object, _getDependencies.Object, httpContextAccessor.Object);

            _deleteDependencies.Setup(a => a.Delete(It.IsAny<CompanyEntity>()))
            .Returns(Task.FromResult(Result.Success(1)));
        }

        [TestMethod]
        public async Task Test_allCorrect_ThenSuccess()
        {
            var stateDel = new DeleteCompanyTestUnit();
            var result = await stateDel.Subject.Delete(stateDel.companyEntity.Codigo, Settings.Settings.Client);

            Assert.IsTrue(result.Success);
            Assert.AreEqual(1, result.Value);
        }
    }
}
