using KloiaCase.DataAccess;
using KloiaCase.DataAccess.Services;
using KloiaCase.Domain;
using KloiaCase.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.EntityFrameworkCore;

namespace KloiaCase.ReviewService.Controllers.V1
{
    [ApiController]
    [Route("api/v" + Version + "/review")]
    public class ReviewV1Controller : ControllerBase
    {
        private const string Version = "1";

        private INetCoreMicroServicesDBContext _dBContext;

        private IReviewService _ReviewService;
        private IArticleService _ArticleService;

        public ReviewV1Controller(INetCoreMicroServicesDBContext dBContext, IReviewService reviewService, IArticleService articleService)
        {
            _dBContext = dBContext;
            _ReviewService = reviewService;
            _ArticleService = articleService;
        }

        [HttpGet]
        [EnableQuery]
        public async Task<IActionResult> Get()
        {
            var list = _ReviewService.Get();
            return Ok(list);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var review = _ReviewService.GetById(id);

            if (review == null)
                return NotFound();

            return Ok(review);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ReviewEntity review)
        {
            var article = _ArticleService.GetById(review.ArticleId);

            //When creating a review, the review microservice should check the existence of the article provided
            if (article == null)
                return ValidationProblem("There is no article with provided ArticleId!");

            var reviewId = await _ReviewService.Create(review);

            return Ok(reviewId);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var review = _ReviewService.GetById(id);

            if (review == null)
                return NotFound();

            var deleteReturn = await _ReviewService.Delete(review);

            return Ok(deleteReturn);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] ReviewEntity updateReview)
        {
            var reviewId = await _ReviewService.Update(id, updateReview);

            if (reviewId == 0)
                return NotFound();

            return Ok(reviewId);
        }
    }
}