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

namespace KloiaCase.Tests.UnitTests
{
    [TestClass]
    public class ArticleUnitTests : UnitTestsBase
    {
        public IArticleService _ArticleService;

        [TestMethod]
        public void Article_Get_Should_Return_5_Article()
        {
            //Arrange In MockDependencies()

            //Act
            var articles = _ArticleService.Get();
            //Assert
            Assert.IsNotNull(articles);
            Assert.AreEqual(5, articles.Count);
        }

        [TestMethod]
        public void Article_GetById_Should_Return_Correct_Article_With_Id_1()
        {
            //Arrange In MockDependencies()
            var id = 1;

            //Act
            var article = _ArticleService.GetById(id);

            //Assert
            Assert.IsNotNull(article);
            Assert.AreEqual(id, article.Id);
            Assert.AreEqual($"Title test 1", article.Title);
            Assert.AreEqual($"ArticleContent test 1", article.ArticleContent);
            Assert.AreEqual($"Author test 1", article.Author);
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
            var articleId = _ArticleService.Create(article).Result;

            //Assert
            Assert.IsNotNull(articleId);
            Assert.AreEqual(id, articleId);
        }

        [TestMethod]
        public void Article_Delete_Should_Return_Has_Reviews_Validation_Error()
        {
            //Arrange In MockDependencies()
            var id = 1;

            //Act
            var articleDeleteResponse = _ArticleService.Delete(id).Result;

            //Assert
            Assert.IsNotNull(articleDeleteResponse);

            Assert.AreEqual("This Article has reviews!", articleDeleteResponse.ValidationdMessage);
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
            _ArticleService.Create(article);
            var articleDeleteResponse = _ArticleService.Delete(id).Result;

            //Assert
            Assert.IsNotNull(articleDeleteResponse);
            Assert.AreEqual(true, articleDeleteResponse.OperationSuccess);
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
            var updateResponse = _ArticleService.Update(1, article).Result;

            //Assert
            Assert.IsNotNull(updateResponse);

            var articleResponse = _ArticleService.GetById(id);

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
        }
    }
}

