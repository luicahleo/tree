using Microsoft.AspNetCore.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Threading.Tasks;
using TreeCore.BackEnd.Model.Entity.General;
using TreeCore.BackEnd.Service.Services.General;
using TreeCore.BackEnd.ServiceDependencies.Services;
using TreeCore.Shared.DTO.General;

namespace TreeCore.BackEnd.TestUnit.Services.ProductCatalog
{
    [TestClass]
    public class PutCurrencyTestUnit
    {
        public Mock<GetDependencies<CurrencyDTO, CurrencyEntity>> _getDependencies;
        public Mock<PutDependencies<CurrencyEntity>> _putDependencies;
        public PutCurrency Subject;
        public readonly CurrencyEntity currencyEntity;

        public PutCurrencyTestUnit()
        {
            currencyEntity = new CurrencyEntity(null, null, "Test1", "Test Put", 1, 1, DateTime.Now, true, false);
            Mock<PutDependencies<CurrencyEntity>> putDependencies = new Mock<PutDependencies<CurrencyEntity>>();
            Mock<GetDependencies<CurrencyDTO, CurrencyEntity>> getDependencies = new Mock<GetDependencies<CurrencyDTO, CurrencyEntity>>();

            Mock<IHttpContextAccessor> httpContextAccessor = new Mock<IHttpContextAccessor>();
            Mock<IHeaderDictionary> headerDictionary = new Mock<IHeaderDictionary>();
            httpContextAccessor.Setup(a => a.HttpContext.Request.Headers)
                .Returns(headerDictionary.Object);

            _putDependencies = putDependencies;
            _getDependencies = getDependencies;

            //Subject = new PutCurrency(_putDependencies.Object, httpContextAccessor.Object, _getDependencies.Object);

            _putDependencies.Setup(a => a.Update(It.IsAny<CurrencyEntity>()))
            .Returns(Subject.SaveItem(currencyEntity));
        }

        [TestMethod]
        public async Task Test_allCorrect_ThenSuccess()
        {
            var state = new PutCurrencyTestUnit();
            var result = await state.Subject.SaveItem(state.currencyEntity);

            Assert.IsTrue(result.Success);
            Assert.AreEqual("Test1", result.Value.Moneda);
            Assert.AreEqual("Test Put", result.Value.Simbolo);
        }
    }
}

