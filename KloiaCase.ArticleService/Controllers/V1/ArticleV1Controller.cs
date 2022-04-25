
using KloiaCase.DataAccess;
using KloiaCase.DataAccess.Services;
using KloiaCase.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.EntityFrameworkCore;

namespace KloiaCase.ArticleService.Controllers.V1
{
    [ApiController]
    [Route("api/v" + Version + "/article")]
    public class ArticleV1Controller : ControllerBase
    {
        private const string Version = "1";

        private INetCoreMicroServicesDBContext _dBContext;
        private IArticleService _ArticleService;
        private IReviewService _ReviewService;

        public ArticleV1Controller(INetCoreMicroServicesDBContext dBContext, IArticleService articleService)
        {
            _dBContext = dBContext;
            _ArticleService = articleService;
        }

        [HttpGet]
        [EnableQuery]
        public async Task<IActionResult> Get()
        {
            var list = _ArticleService.Get();

            return Ok(list);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var article = _ArticleService.GetById(id);

            if (article == null)
                return NotFound();

            return Ok(article);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ArticleEntity article)
        {
            var articleId = await _ArticleService.Create(article);

            return Ok(articleId);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var article = _ArticleService.GetById(id);

            if (article == null)
                return NotFound();

            //When deleting an article, the article microservice should check that there should be no reviews under that article
            if (article.Reviews.Any())
                return ValidationProblem("This Article has reviews!");

            var deleteReturn = await _ArticleService.Delete(article);

            return Ok(deleteReturn);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] ArticleEntity updateArticle)
        {
            var reviewId = await _ArticleService.Update(id, updateArticle);

            var article = _dBContext.Article.Where(a => a.Id == id).FirstOrDefault();

            if (reviewId == 0)
                return NotFound();

            return Ok(reviewId);
        }
    }
}
