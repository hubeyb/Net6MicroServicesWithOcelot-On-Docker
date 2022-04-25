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

        public ReviewsService(INetCoreMicroServicesDBContext dBContext)
        {
            _dBContext = dBContext;
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

        public async Task<int> Create(ReviewEntity review)
        {
            _dBContext.Review.Add(review);
            await _dBContext.SaveChanges();
            return review.Id;
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
            review.Article = updateReview.Article;

            await _dBContext.SaveChanges();

            return review.Id;
        }
    }
}
