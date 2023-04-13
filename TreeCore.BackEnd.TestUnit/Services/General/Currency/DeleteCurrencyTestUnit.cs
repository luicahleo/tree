using Microsoft.AspNetCore.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Threading.Tasks;
using TreeCore.BackEnd.Model.Entity.General;
using TreeCore.BackEnd.Service.Services.General;
using TreeCore.BackEnd.ServiceDependencies.Services;
using TreeCore.Shared.DTO.General;
using TreeCore.Shared.ROP;

namespace TreeCore.BackEnd.TestUnit.Services.CurrencyCatalog
{
    [TestClass]
    public class DeleteCurrencyTestUnit
    {
        public Mock<GetDependencies<CurrencyDTO, CurrencyEntity>> _getDependencies;
        public Mock<DeleteDependencies<CurrencyEntity>> _deleteDependencies;
        public DeleteCurrency Subject;
        public readonly CurrencyEntity currencyEntity;

        public DeleteCurrencyTestUnit()
        {
            currencyEntity = new CurrencyEntity(null, null, "Test", "Symbol", 1, 1, DateTime.Now, true, false);
            Mock<DeleteDependencies<CurrencyEntity>> deleteDependencies = new Mock<DeleteDependencies<CurrencyEntity>>();
            Mock<GetDependencies<CurrencyDTO, CurrencyEntity>> getDependencies = new Mock<GetDependencies<CurrencyDTO, CurrencyEntity>>();

            Mock<IHttpContextAccessor> httpContextAccessor = new Mock<IHttpContextAccessor>();
            Mock<IHeaderDictionary> headerDictionary = new Mock<IHeaderDictionary>();
            httpContextAccessor.Setup(a => a.HttpContext.Request.Headers)
                .Returns(headerDictionary.Object);

            _deleteDependencies = deleteDependencies;
            _getDependencies = getDependencies;

            Subject = new DeleteCurrency(_deleteDependencies.Object, _getDependencies.Object, httpContextAccessor.Object);

            _deleteDependencies.Setup(a => a.Delete(It.IsAny<CurrencyEntity>()))
            .Returns(Task.FromResult(Result.Success(1)));
        }

        [TestMethod]
        public async Task Test_allCorrect_ThenSuccess()
        {
            //var stateDel = new DeleteCurrencyTestUnit();
            //var result = await stateDel.Subject.Delete(stateDel.currencyEntity);

            //Assert.IsTrue(result.Success);
            //Assert.AreEqual(1, result.Value);
        }
    }
}
