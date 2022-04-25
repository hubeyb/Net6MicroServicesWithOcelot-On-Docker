using KloiaCase.ArticleService.Controllers.V1;
using KloiaCase.Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rhino.Mocks;
using KloiaCase.DataAccess;
using KloiaCase.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using KloiaCase.DataAccess.Services;
//using Microsoft.EntityFrameworkCore;

namespace KloiaCase.Tests.Controllers.V1
{
    [TestClass]
    public class ArticleV1ControllerTests : ControllerTestsBase<ArticleV1Controller>
    {
        public IReviewService _ReviewService;
        public IArticleService _ArticleService;

        public override ArticleV1Controller CreateController()
        {
            return new ArticleV1Controller(dBContext, _ArticleService);
        }

        [TestMethod]
        public void Article_Get_Should_Return_1_Article()
        {
            var articles = _ArticleService.Get();
            Assert.IsNotNull(articles);
            Assert.AreEqual(1, articles.Count);
        }

        [TestMethod]
        public void Article_GetById_Should_Return_Article_With_Id_1()
        {
            var articles = _ArticleService.GetById(1);
            Assert.IsNotNull(articles);
            Assert.AreEqual(1, articles.Id);
        }

        [TestMethod]
        public void Article_Create_Should_Return_Article_With_Id_1()
        {
            var articles = _ArticleService.Create(new ArticleEntity());
            Assert.IsNotNull(articles);
            Assert.AreEqual(1, articles.Id);
        }

        [TestMethod]
        public void Article_Delete_Should_Return_True()
        {
            var articleDeleteResponse = _ArticleService.Delete(new ArticleEntity());
            Assert.IsNotNull(articleDeleteResponse);
            Assert.AreEqual(true, articleDeleteResponse);
        }

        [TestMethod]
        public void Article_Update_Should_Return_ArticleId_1()
        {
            var articleUpdateResponse = _ArticleService.Update(1, new ArticleEntity());
            Assert.IsNotNull(articleUpdateResponse);
            Assert.AreEqual(1, articleUpdateResponse);
        }

        protected override void MockDependencies()
        {
            var options = new DbContextOptionsBuilder<NetCoreMicroServicesDBContext>().Options;

            var configPath = AppContext.BaseDirectory;
            var configuration = new ConfigurationBuilder()
                                .SetBasePath(configPath)
                                .AddJsonFile("appsettings.json", optional: true)
                                .AddEnvironmentVariables()
                                .Build();

            dBContext = new NetCoreMicroServicesDBContext(options, configuration);

            _ReviewService =  MockRepository.GenerateMock<IReviewService>();
            _ArticleService = MockRepository.GenerateMock<IArticleService>();

            var article = new ArticleEntity()
            {
                Id = 1,
                ArticleContent = "ArticleContent test 1",
                Author = "Author test 1",
                PublishDate = DateTime.Now,
                StarCount = 5,
                Title = "Title test 1",
                Reviews = new List<ReviewEntity>() {
                    new ReviewEntity() {
                        ArticleId = 1,
                        Reviewer = "",
                        ReviewContent = "Review Content test 1"

                    }
                }
            };

            var articleList = new List<ArticleEntity>();
            articleList.Add(article);

            _ArticleService.Stub(s => s.Get()).Return(articleList);
            _ArticleService.Stub(s => s.GetById(Arg<int>.Is.Anything)).Return(article);
            _ArticleService.Stub(s => s.Create(Arg<ArticleEntity>.Is.Anything)).Return(Task.FromResult(article.Id));
            _ArticleService.Stub(s => s.Delete(Arg<ArticleEntity>.Is.Anything)).Return(Task.FromResult(true));
            _ArticleService.Stub(s => s.Update(Arg<int>.Is.Anything, Arg<ArticleEntity>.Is.Anything)).Return(Task.FromResult(article.Id));
        }
    }

}

