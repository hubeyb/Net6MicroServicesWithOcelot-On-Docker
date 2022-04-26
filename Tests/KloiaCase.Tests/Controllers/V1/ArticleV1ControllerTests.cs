using KloiaCase.ArticleService.Controllers.V1;
using KloiaCase.Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KloiaCase.DataAccess;
using KloiaCase.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using KloiaCase.DataAccess.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace KloiaCase.Tests.Controllers.V1
{
    [TestClass]
    public class ArticleV1ControllerTests : ControllerTestsBase<ArticleV1Controller>
    {
        public IArticleService _ArticleService;

        public override ArticleV1Controller CreateController()
        {
            return new ArticleV1Controller(dBContext, _ArticleService);
        }

        [TestMethod]
        public void Article_Get_Should_Return_5_Article()
        {
            //Arrange In MockDependencies()

            //Act
            var articles = Controller.Get().Result;
            var response = (List<ArticleEntity>)((OkObjectResult)articles).Value;

            //Assert
            Assert.IsNotNull(articles);
            Assert.AreEqual(5, response.Count);
        }

        [TestMethod]
        public void Article_GetById_Should_Return_Correct_Article_With_Id_1()
        {
            //Arrange In MockDependencies()
            var id = 1;

            //Act
            var article = Controller.GetById(id).Result;
            var response = (ArticleEntity)((OkObjectResult)article).Value;

            //Assert
            Assert.IsNotNull(response);
            Assert.AreEqual(id, response.Id);
            Assert.AreEqual($"Title test 1", response.Title);
            Assert.AreEqual($"ArticleContent test 1", response.ArticleContent);
            Assert.AreEqual($"Author test 1", response.Author);
        }

        [TestMethod]
        public void Article_Create_Should_Return_Article_With_Same_Id()
        {
            //Arrange
            var id = 13;
            var article = new ArticleEntity()
            {
                Id = id,
                ArticleContent = $"ArticleContent test {id}",
                Author = $"Author test {id}",
                PublishDate = DateTime.Now,
                StarCount = 5 + id,
                Title = $"Title test {id}",
                Reviews = new List<ReviewEntity>() {
                                        new ReviewEntity() {
                                            ArticleId = id,
                                            Reviewer = "",
                                            ReviewContent = "Review Content test 1"
                                        }
                                    }
            };

            //Act
            var createResponse = Controller.Create(article).Result;

            //Assert
            Assert.IsNotNull(createResponse);

            var articleId = (int)((OkObjectResult)createResponse).Value;
            Assert.AreEqual(id, articleId);
        }

        [TestMethod]
        public void Article_Delete_Should_Return_Has_Reviews_Validation_Error()
        {
            //Arrange In MockDependencies()
            var id = 1;

            //Act
            var articleDeleteResponse = Controller.Delete(id).Result;

            //Assert
            Assert.IsNotNull(articleDeleteResponse);

            var responseDetail = ((ValidationProblemDetails)((ObjectResult)articleDeleteResponse).Value).Detail;

            Assert.AreEqual("This Article has reviews!", responseDetail);
        }

        [TestMethod]
        public void Article_Delete_Should_Return_True()
        {
            //Arrange
            var id = 15;
            var article = new ArticleEntity()
            {
                Id = id,
                ArticleContent = $"ArticleContent test {id}",
                Author = $"Author test {id}",
                PublishDate = DateTime.Now,
                StarCount = 5 + id,
                Title = $"Title test {id}"
            };

            //Act
            var createResponse = Controller.Create(article).Result;
            var articleDeleteResponse = Controller.Delete(id).Result;

            //Assert
            Assert.IsNotNull(articleDeleteResponse);

            var response = (bool)((OkObjectResult)articleDeleteResponse).Value;

            Assert.AreEqual(true, response);
        }

        [TestMethod]
        public void Article_Update_Should_Return_ArticleId_1()
        {
            //Arrange In MockDependencies()
            var id = 1;
            var article = new ArticleEntity()
            {
                Id = id,
                ArticleContent = $"ArticleContent update test {id}",
                Author = $"Author update test {id}",
                Title = $"Title update test {id}"
            };

            //Act
            var createResponse = Controller.Update(1, article).Result;

            //Assert
            Assert.IsNotNull(createResponse);

            var articleId = (int)((OkObjectResult)createResponse).Value;

            var response = Controller.GetById(id).Result;
            var articleResponse = (ArticleEntity)((OkObjectResult)response).Value;

            Assert.AreEqual(id, articleResponse.Id);
            Assert.AreEqual(article.Title, articleResponse.Title);
            Assert.AreEqual(article.ArticleContent, articleResponse.ArticleContent);
            Assert.AreEqual(article.Author, articleResponse.Author);
        }

        protected async override void MockDependencies()
        {
            //Arrange
            dBContext = MockDBContext.GetMockDBContext();

            var articleList = new List<ArticleEntity>();

            for (int id = 1; id <= 5; id++)
            {
                var article = new ArticleEntity()
                {
                    Id = id,
                    ArticleContent = $"ArticleContent test {id}",
                    Author = $"Author test {id}",
                    PublishDate = DateTime.Now,
                    StarCount = 5 + id,
                    Title = $"Title test {id}",
                    Reviews = new List<ReviewEntity>() {
                                        new ReviewEntity() {
                                            ArticleId = id,
                                            Reviewer = "",
                                            ReviewContent = "Review Content test 1"
                                        }
                                    }
                };

                articleList.Add(article);
            }

            dBContext.Article.AddRange(articleList);
            await dBContext.SaveChanges();

            _ArticleService = new ArticlesService(dBContext);
            Controller = CreateController();
        }

    }
}

