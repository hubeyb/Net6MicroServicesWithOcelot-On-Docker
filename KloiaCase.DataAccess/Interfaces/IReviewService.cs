using KloiaCase.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KloiaCase.DataAccess.Services
{
    public interface IReviewService
    {
        List<ReviewEntity> Get();

        ReviewEntity? GetById(int id);

        Task<int> Create(ReviewEntity article);

        Task<bool> Delete(ReviewEntity review);

        Task<int> Update(int id, ReviewEntity updateArticle);
    }
}
