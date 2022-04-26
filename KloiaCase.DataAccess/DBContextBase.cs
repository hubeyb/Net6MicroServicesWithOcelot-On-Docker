using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace KloiaCase.DataAccess
{
    public abstract class DBContextBase : DbContext
    {
        public DBContextBase(DbContextOptions options)
            : base(options)
        {
        }
    }
}