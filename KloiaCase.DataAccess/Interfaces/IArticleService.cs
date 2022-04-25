using KloiaCase.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KloiaCase.DataAccess.Services
{
    public interface IArticleService
    {
        List<ArticleEntity> Get();

        ArticleEntity GetById(int id);

        Task<int> Create(ArticleEntity article);

        Task<bool> Delete(ArticleEntity article);

        Task<int> Update(int id, ArticleEntity updateArticle);
    }
}
