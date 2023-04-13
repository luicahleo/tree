using Microsoft.AspNetCore.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Threading.Tasks;
using TreeCore.BackEnd.Model.Entity.Companies;
using TreeCore.BackEnd.Service.Services.Companies;

namespace TreeCore.BackEnd.TestUnit.Services.Companies
{
    [TestClass]
    public class PutCompanyTypeTestUnit
    {
        //public Mock<IGetCompanyTypeDependencies> _getDependencies;
        //public Mock<IPutCompanyTypeDependencies> _putDependencies;
        public PutCompanyType Subject;
        public readonly CompanyTypeEntity companyTypeEntity;

        public PutCompanyTypeTestUnit()
        {
            companyTypeEntity = new CompanyTypeEntity(null, null, "Test1", "Test1", "Test Put", true, false);
            //Mock<IPutCompanyTypeDependencies> putDependencies = new Mock<IPutCompanyTypeDependencies>();
            //Mock<IGetCompanyTypeDependencies> getDependencies = new Mock<IGetCompanyTypeDependencies>();

            Mock<IHttpContextAccessor> httpContextAccessor = new Mock<IHttpContextAccessor>();
            Mock<IHeaderDictionary> headerDictionary = new Mock<IHeaderDictionary>();
            httpContextAccessor.Setup(a => a.HttpContext.Request.Headers)
                .Returns(headerDictionary.Object);

            //_putDependencies = putDependencies;
            //_getDependencies = getDependencies;

            //Subject = new PutCompanyType(_putDependencies.Object, _getDependencies.Object, httpContextAccessor.Object);

            //_putDependencies.Setup(a => a.UpdateCompanyType(It.IsAny<CompanyTypeEntity>()))
            //.Returns(Subject.SaveCompanyType(companyTypeEntity));
        }

        [TestMethod]
        public async Task Test_allCorrect_ThenSuccess()
        {
            var state = new PutCompanyTypeTestUnit();
            //var result = await state.Subject.SaveCompanyType(state.companyTypeEntity);

            //Assert.IsTrue(result.Success);
            //Assert.AreEqual("Test1", result.Value.Codigo);
            //Assert.AreEqual("Test Put", result.Value.Descripcion);
        }
    }
}
