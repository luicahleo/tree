using Microsoft.AspNetCore.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Threading.Tasks;
using TreeCore.BackEnd.Model.Entity.Contracts;
using TreeCore.BackEnd.Service.Services.Contracts;
using TreeCore.BackEnd.ServiceDependencies.Services;
using TreeCore.Shared.DTO.Contracts;
namespace TreeCore.BackEnd.TestUnit.Services.Contracts
{
    [TestClass]
    public class PostContractGroupTestUnit
    {
        public Mock<PostDependencies<ContractGroupEntity>> _postDependencies;
        public Mock<GetDependencies<ContractGroupDTO, ContractGroupEntity>> _getDependencies;
        public Mock<PutDependencies<ContractGroupEntity>> _putDependencies;
        public PostContractGroup Subject;
        public readonly ContractGroupEntity ContractGroupEntity;

        public PostContractGroupTestUnit()
        {
            //ContractGroupEntity = new ContractGroupEntity(null, null, "Test1", "Test Put", true, false,false);
            Mock<PostDependencies<ContractGroupEntity>> postDependencies = new Mock<PostDependencies<ContractGroupEntity>>();
            Mock<PutDependencies<ContractGroupEntity>> putDependencies = new Mock<PutDependencies<ContractGroupEntity>>();
            Mock<GetDependencies<ContractGroupDTO, ContractGroupEntity>> getDependencies = new Mock<GetDependencies<ContractGroupDTO, ContractGroupEntity>>();

            Mock<IHttpContextAccessor> httpContextAccessor = new Mock<IHttpContextAccessor>();
            Mock<IHeaderDictionary> headerDictionary = new Mock<IHeaderDictionary>();
            httpContextAccessor.Setup(a => a.HttpContext.Request.Headers)
                .Returns(headerDictionary.Object);

            _postDependencies = postDependencies;
            _putDependencies = putDependencies;
            _getDependencies = getDependencies;

            Subject = new PostContractGroup(_postDependencies.Object, _getDependencies.Object , _putDependencies.Object, httpContextAccessor.Object);

            _postDependencies.Setup(a => a.Create(It.IsAny<ContractGroupEntity>()))
            .Returns(Subject.SaveItem(ContractGroupEntity));
        }

        [TestMethod]
        public async Task Test_allCorrect_ThenSuccess()
        {
            var state = new PostContractGroupTestUnit();
            var result = await state.Subject.SaveItem(state.ContractGroupEntity);

            Assert.IsTrue(result.Success);
            Assert.AreEqual("Test1", result.Value.codigo);
            Assert.AreEqual("Test Post", result.Value.Descripcion);
        }
    }
}

