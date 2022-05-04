using KloiaCase.DataAccess.Models;
using KloiaCase.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KloiaCase.DataAccess.Services
{
    public class ReviewsService : IReviewService
    {
        private INetCoreMicroServicesDBContext _dBContext;
        private IArticleService _ArticleService;

        public ReviewsService(INetCoreMicroServicesDBContext dBContext, IArticleService articleService)
        {
            _dBContext = dBContext;
            _ArticleService = articleService;
        }

        public List<ReviewEntity> Get()
        {
            var list = _dBContext.Review.ToList();
            return list;
        }

        public ReviewEntity? GetById(int id)
        {
            return _dBContext.Review.Where(a => a.Id == id).FirstOrDefault();
        }

        public async Task<CreateResponseServiceModel> Create(ReviewEntity review)
        {
            var response = new CreateResponseServiceModel();
            var article = _ArticleService.GetById(review.ArticleId);

            //When creating a review, the review microservice should check the existence of the article provided
            if (article == null)
            {
                response.ValidationdMessage = "There is no article with provided ArticleId!";
                response.OperationSuccess = false;
            }
            else
            {
                _dBContext.Review.Add(review);
                await _dBContext.SaveChanges();

                response.CreatedEntityId = review.Id;
                response.OperationSuccess = true;
            }

            return response;
        }

        public async Task<bool> Delete(ReviewEntity review)
        {
            _dBContext.Review.Remove(review);
            await _dBContext.SaveChanges();
            return true;
        }

        public async Task<int> Update(int id, ReviewEntity updateReview)
        {
            var review = GetById(id);

            if (review == null)
                return 0;

            review.ArticleId = updateReview.ArticleId;
            review.Reviewer = updateReview.Reviewer;
            review.ReviewContent = updateReview.ReviewContent;

            await _dBContext.SaveChanges();

            return review.Id;
        }
    }
}
