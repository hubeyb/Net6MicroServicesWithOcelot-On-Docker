using KloiaCase.DataAccess;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KloiaCase.Tests
{
    public class MockDBContext
    {
        public static NetCoreMicroServicesDBContext GetMockDBContext()
        {
            var options = new DbContextOptionsBuilder<NetCoreMicroServicesDBContext>()
                                    .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                                    .Options;

            var databaseContext = new NetCoreMicroServicesDBContext(options);
            databaseContext.Database.EnsureCreated();

            return databaseContext;
        }
    }
}
