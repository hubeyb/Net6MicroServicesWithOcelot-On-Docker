using KloiaCase.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KloiaCase.DataAccess
{
    public class NetCoreMicroServicesDBContext : DBContextBase, INetCoreMicroServicesDBContext
    {

        public NetCoreMicroServicesDBContext(DbContextOptions options)
            : base(options)
        {
        }

        public DbSet<ArticleEntity> Article { get; set; }
        public DbSet<ReviewEntity> Review { get; set; }

        public async Task<int> SaveChanges()
        {
            return await base.SaveChangesAsync();
        }
    }
}
