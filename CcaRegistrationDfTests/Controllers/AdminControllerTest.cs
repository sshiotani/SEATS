using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CcaRegistrationDf.Models;
using CcaRegistrationDf.Controllers;
using System.Collections.Generic;
using Moq;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Threading.Tasks;


using System.Linq;
using SEATS_1.Tests.Models;
using System.Web.Mvc;

namespace CcaRegistrationDf.Tests.Controllers
{
    [TestClass]
    public class AdminControllerTest
    {
        [TestMethod]
        public async Task Index_LooksUpCcaFromDbMapToViewModel_ReturnsCorrectMapping()
        {
            //Arrange

            var controller = new AdminController();

            //Act
            var result = await controller.CcaInterface() as ViewResult;
            var model = result.Model as List<CCA>;

            //Assert 
            Assert.AreNotEqual("Error", result.ViewName);

            
        }
    }
}
