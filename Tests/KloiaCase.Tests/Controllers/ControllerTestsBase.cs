using KloiaCase.DataAccess;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KloiaCase.Tests.Controllers
{
    public abstract class ControllerTestsBase<TController> where TController : ControllerBase
    {
        public TController Controller { get; set; }
        public INetCoreMicroServicesDBContext dBContext;

        public abstract TController CreateController();
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
