using KloiaCase.DataAccess.Models;
using KloiaCase.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KloiaCase.DataAccess.Services
{
    public class ArticlesService : IArticleService
    {
        private INetCoreMicroServicesDBContext _dBContext;

        public ArticlesService(INetCoreMicroServicesDBContext dBContext)
        {
            _dBContext = dBContext;
        }

        public List<ArticleEntity> Get()
        {
            var list = _dBContext.Article.ToList();
            return list;
        }

        public ArticleEntity? GetById(int id)
        {
            var article = _dBContext.Article.Include(i => i.Reviews).Where(a => a.Id == id).FirstOrDefault();
            return article;
        }

        public async Task<int> Create(ArticleEntity article)
        {
            _dBContext.Article.Add(article);
           var articleId =  await _dBContext.SaveChanges();
            return article.Id;
        }

        public async Task<DeleteResponseServiceModel> Delete(int id)
        {
            var article = GetById(id);
            var response = new DeleteResponseServiceModel();

            if (article == null)
            {
                response.ValidationdMessage = "Article Not Found!";
                response.OperationSuccess = false;
            }
            else
            {
                //When deleting an article, the article microservice should check that there should be no reviews under that article
                if (article.Reviews.Any())
                {
                    response.ValidationdMessage = "This Article has reviews!";
                    response.OperationSuccess = false;
                }
                else
                {
                    _dBContext.Article.Remove(article);
                    await _dBContext.SaveChanges();
                    response.OperationSuccess = true;
                }
            }

            return response;
        }

        public async Task<int> Update(int id, ArticleEntity updateArticle)
        {
            var article = GetById(id);

            if (article == null)
                return 0;

            article.Title = updateArticle.Title;
            article.Author = updateArticle.Author;
            article.ArticleContent = updateArticle.ArticleContent;
            article.PublishDate = updateArticle.PublishDate;
            article.StarCount = updateArticle.StarCount;
            article.Reviews = updateArticle.Reviews;

            await _dBContext.SaveChanges();

            return article.Id;
        }
    }
}
