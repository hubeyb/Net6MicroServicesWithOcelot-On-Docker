using KloiaCase.ReviewService.Controllers.V1;
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
using KloiaCase.DataAccess.Models;

namespace KloiaCase.Tests.Controllers.V1
{
    [TestClass]
    public class ReviewV1ControllerTests : ControllerTestsBase<ReviewV1Controller>
    {
        public IReviewService _ReviewService;
        private IArticleService _ArticleService;

        public override ReviewV1Controller CreateController()
        {
            return new ReviewV1Controller(dBContext, _ReviewService, _ArticleService);
        }

        [TestMethod]
        public async Task Review_Get_Should_Return_5_Review()
        {
            //Arrange In MockDependencies()

            //Act
            var reviews = await Controller.Get();
            var response = (List<ReviewEntity>)((OkObjectResult)reviews).Value;

            //Assert
            Assert.IsNotNull(reviews);
            Assert.AreEqual(5, response.Count);
        }

        [TestMethod]
        public async Task Review_GetById_Should_Return_Correct_Review_With_Id_1()
        {
            //Arrange In MockDependencies()
            var id = 1;

            //Act
            var review = await Controller.GetById(id);
            var response = (ReviewEntity)((OkObjectResult)review).Value;

            //Assert
            Assert.IsNotNull(response);
            Assert.AreEqual(id, response.Id);
            Assert.AreEqual($"Reviewer test 1", response.Reviewer);
            Assert.AreEqual($"Review Content test 1", response.ReviewContent);
        }

        [TestMethod]
        public async Task Review_Create_Should_Return_Review_With_Same_Id()
        {
            //Arrange
            var id = 8;
            var review = new ReviewEntity()
            {
                Id = id,
                ArticleId = 1,
                Reviewer = $"Reviewer test {id}",
                ReviewContent = $"Review Content test {id}"
            };

            //Act
            var createResponse = await Controller.Create(review);

            //Assert
            Assert.IsNotNull(createResponse);

            var reviewCreateResponse = (CreateResponseServiceModel)((OkObjectResult)createResponse).Value;
            Assert.AreEqual(id, reviewCreateResponse.CreatedEntityId);
        }

        [TestMethod]
        public async Task Review_Create_Should_Return_Review_No_Article_With_Provided_ArticleId_Validation_Error()
        {
            //Arrange
            var id = 8;
            var review = new ReviewEntity()
            {
                Id = id,
                ArticleId = 23,
                Reviewer = $"Reviewer test {id}",
                ReviewContent = $"Review Content test {id}"
            };

            //Act
            var createResponse = await Controller.Create(review);

            //Assert
            Assert.IsNotNull(createResponse);

            var responseDetail = ((ValidationProblemDetails)((ObjectResult)createResponse).Value).Detail;
            Assert.AreEqual("There is no article with provided ArticleId!", responseDetail);
        }

        [TestMethod]
        public async Task Review_Delete_Should_Return_True()
        {
            //Arrange
            var id = 1;
            var review = new ReviewEntity()
            {
                Id = id,
                Reviewer = $"Reviewer test {id}",
                ReviewContent = $"Review Content test {id}"
            };

            //Act
            await Controller.Create(review);
            var reviewDeleteResponse = await Controller.Delete(id);

            //Assert
            Assert.IsNotNull(reviewDeleteResponse);

            var response = (bool)((OkObjectResult)reviewDeleteResponse).Value;

            Assert.AreEqual(true, response);
        }

        [TestMethod]
        public async Task Review_Update_Should_Return_ReviewId_1()
        {
            //Arrange In MockDependencies()
            var id = 1;
            var review = new ReviewEntity()
            {
                Id = id,
                Reviewer = $"Reviewer update test {id}",
                ReviewContent = $"Review Content update test {id}"
            };

            //Act
            var createResponse = await Controller.Update(1, review);

            //Assert
            Assert.IsNotNull(createResponse);

            var reviewId = (int)((OkObjectResult)createResponse).Value;

            var response = Controller.GetById(id).Result;
            var reviewResponse = (ReviewEntity)((OkObjectResult)response).Value;

            Assert.AreEqual(id, reviewResponse.Id);
            Assert.AreEqual(review.Reviewer, reviewResponse.Reviewer);
            Assert.AreEqual(review.ReviewContent, reviewResponse.ReviewContent);
        }

        protected async override void MockDependencies()
        {
            //Arrange
            dBContext = MockDBContext.GetMockDBContext();

            var reviewList = new List<ReviewEntity>();
            var article = new ArticleEntity()
            {
                Id = 1,
                ArticleContent = $"ArticleContent test 1",
                Author = $"Author test 1",
                PublishDate = DateTime.Now,
                StarCount = 5,
                Title = $"Title test 1"
            };

            dBContext.Article.Add(article);
            await dBContext.SaveChanges();

            for (int id = 1; id <= 5; id++)
            {
                var review = new ReviewEntity()
                {
                    Id = id,
                    Reviewer = $"Reviewer test {id}",
                    ReviewContent = $"Review Content test {id}"
                };

                reviewList.Add(review);
            }

            dBContext.Review.AddRange(reviewList);
            await dBContext.SaveChanges();

            _ArticleService = new ArticlesService(dBContext);
            _ReviewService = new ReviewsService(dBContext, _ArticleService);

            Controller = CreateController();
        }
    }
}

