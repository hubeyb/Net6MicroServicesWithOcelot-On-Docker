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
        public void Review_Get_Should_Return_5_Review()
        {
            //Arrange In MockDependencies()

            //Act
            var reviews = Controller.Get().Result;
            var response = (List<ReviewEntity>)((OkObjectResult)reviews).Value;

            //Assert
            Assert.IsNotNull(reviews);
            Assert.AreEqual(5, response.Count);
        }

        [TestMethod]
        public void Review_GetById_Should_Return_Correct_Review_With_Id_1()
        {
            //Arrange In MockDependencies()
            var id = 1;

            //Act
            var review = Controller.GetById(id).Result;
            var response = (ReviewEntity)((OkObjectResult)review).Value;

            //Assert
            Assert.IsNotNull(response);
            Assert.AreEqual(id, response.Id);
            Assert.AreEqual($"Reviewer test 1", response.Reviewer);
            Assert.AreEqual($"Review Content test 1", response.ReviewContent);
        }

        [TestMethod]
        public void Review_Create_Should_Return_Review_With_Same_Id()
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
            var createResponse = Controller.Create(review).Result;

            //Assert
            Assert.IsNotNull(createResponse);

            var reviewId = (int)((OkObjectResult)createResponse).Value;
            Assert.AreEqual(id, reviewId);
        }

        [TestMethod]
        public void Review_Create_Should_Return_Review_No_Article_With_Provided_ArticleId_Validation_Error()
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
            var createResponse = Controller.Create(review).Result;

            //Assert
            Assert.IsNotNull(createResponse);

            var responseDetail = ((ValidationProblemDetails)((ObjectResult)createResponse).Value).Detail;
            Assert.AreEqual("There is no article with provided ArticleId!", responseDetail);
        }

        [TestMethod]
        public void Review_Delete_Should_Return_True()
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
            var createResponse = Controller.Create(review).Result;
            var reviewDeleteResponse = Controller.Delete(id).Result;

            //Assert
            Assert.IsNotNull(reviewDeleteResponse);

            var response = (bool)((OkObjectResult)reviewDeleteResponse).Value;

            Assert.AreEqual(true, response);
        }

        [TestMethod]
        public void Review_Update_Should_Return_ReviewId_1()
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
            var createResponse = Controller.Update(1, review).Result;

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

            _ReviewService = new ReviewsService(dBContext);
            _ArticleService = new ArticlesService(dBContext);

            Controller = CreateController();
        }
    }
}

