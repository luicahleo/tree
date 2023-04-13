using Microsoft.AspNetCore.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Threading.Tasks;
using TreeCore.BackEnd.Model.Entity.General;
using TreeCore.BackEnd.Service.Services.General;
using TreeCore.BackEnd.ServiceDependencies.Services;
using TreeCore.Shared.DTO.General;

namespace TreeCore.BackEnd.TestUnit
{
    [TestClass]
    public class GetCurrencyTestUnit
    {
        public Mock<GetDependencies<CurrencyDTO, CurrencyEntity>> _dependencies;
        public GetCurrency Subject;
        public string Currency = "Test";
        public int CurrencyID = 1;

        public GetCurrencyTestUnit()
        {
            Mock<GetDependencies<CurrencyDTO, CurrencyEntity>> dependencies = new Mock<GetDependencies<CurrencyDTO, CurrencyEntity>>();

            Mock<IHttpContextAccessor> httpContextAccessor = new Mock<IHttpContextAccessor>();
            Mock<IHeaderDictionary> headerDictionary = new Mock<IHeaderDictionary>();
            httpContextAccessor.Setup(a => a.HttpContext.Request.Headers)
                .Returns(headerDictionary.Object);

            dependencies.Setup(a => a.GetItemByCode(Currency, Settings.Settings.Client))
            .Returns(Task.FromResult(new CurrencyEntity(CurrencyID, null, Currency, "Symbol", 1, 1, DateTime.Now, true, false)));

            _dependencies = dependencies;

            Subject = new GetCurrency(_dependencies.Object, httpContextAccessor.Object);
        }

        [TestMethod]
        public async Task Test_allCorrect_ThenSuccess()
        {
            var state = new GetCurrencyTestUnit();
            var result = await state.Subject.GetItemByCode(state.Currency, Settings.Settings.Client);

            Assert.IsTrue(result.Success);
            Assert.AreEqual(Currency, result.Value.Code);
        }

        [TestMethod]
        public async Task Test_User_NonExistent_ThenError()
        {
            var state = new GetCurrencyTestUnit();
            var result = await state.Subject.GetItemByCode("noExistent", Settings.Settings.Client);

            Assert.IsFalse(result.Success);
            Assert.AreEqual("Not Found", result.Errors[0].Message);
        }
    }
}

