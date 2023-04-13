using Microsoft.AspNetCore.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Threading.Tasks;
using TreeCore.BackEnd.Model.Entity.General;
using TreeCore.BackEnd.Service.Services.General;
using TreeCore.BackEnd.ServiceDependencies.Services;
using TreeCore.Shared.DTO.General;

namespace TreeCore.BackEnd.TestUnit.Services.ProductCatalog
{
    [TestClass]
    public class PostCurrencyTestUnit
    {
        public Mock<PostDependencies<CurrencyEntity>> _postDependencies;
        public Mock<GetDependencies<CurrencyDTO, CurrencyEntity>> _getDependencies;
        public Mock<PutDependencies<CurrencyEntity>> _putDependencies;
        public PostCurrency Subject;
        public readonly CurrencyDTO currencyDTO;

        public PostCurrencyTestUnit()
        {
            currencyDTO = new CurrencyDTO() { Code = "Test1", Symbol = "T", DollarChange = 1, EuroChange = 1, Active = true, Default = false };
            Mock<PostDependencies<CurrencyEntity>> postDependencies = new Mock<PostDependencies<CurrencyEntity>>();
            Mock<PutDependencies<CurrencyEntity>> putDependencies = new Mock<PutDependencies<CurrencyEntity>>();
            Mock<GetDependencies<CurrencyDTO, CurrencyEntity>> getDependencies = new Mock<GetDependencies<CurrencyDTO, CurrencyEntity>>();

            Mock<IHttpContextAccessor> httpContextAccessor = new Mock<IHttpContextAccessor>();
            Mock<IHeaderDictionary> headerDictionary = new Mock<IHeaderDictionary>();
            httpContextAccessor.Setup(a => a.HttpContext.Request.Headers)
                .Returns(headerDictionary.Object);

            _postDependencies = postDependencies;
            _putDependencies = putDependencies;
            _getDependencies = getDependencies;

            //Subject = new PostCurrency(_postDependencies.Object, httpContextAccessor.Object, _getDependencies.Object, _putDependencies.Object);

            //_postDependencies.Setup(a => a.Create(It.IsAny<CurrencyEntity>()))
            //.Returns(Subject.SaveItem(currencyDTO));
        }

        [TestMethod]
        public async Task Test_allCorrect_ThenSuccess()
        {
            var state = new PostCurrencyTestUnit();
            //var result = await state.Subject.Create(state.currencyDTO, Settings.Settings.Client);

            //Assert.IsTrue(result.Success);
            //Assert.AreEqual("Test1", result.Value.Code);
            //Assert.AreEqual("T", result.Value.Symbol);
        }
    }
}
