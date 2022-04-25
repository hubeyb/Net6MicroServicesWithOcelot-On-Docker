using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace KloiaCase.DataAccess
{
    public abstract class DBContextBase : DbContext
    {
        protected readonly IConfiguration Configuration;

        public DBContextBase(DbContextOptions options, IConfiguration configuration) 
            : base(options)
        {
            Configuration = configuration;
        }
    }
}