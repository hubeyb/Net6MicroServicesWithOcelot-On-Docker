using KloiaCase.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KloiaCase.DataAccess
{
    public interface INetCoreMicroServicesDBContext
    {
        DbSet<ArticleEntity> Article { get; set; }
        DbSet<ReviewEntity> Review { get; set; }

        Task<int> SaveChanges();
    }
}
