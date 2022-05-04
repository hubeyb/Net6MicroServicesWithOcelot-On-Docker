using KloiaCase.DataAccess;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KloiaCase.Tests.UnitTests
{
    public abstract class UnitTestsBase
    {
        public INetCoreMicroServicesDBContext dBContext;

        protected virtual void MockDependencies()
        {
        }

        [TestInitialize]
        public void Initialize()
        {
            MockDependencies();
        }
    }
}
