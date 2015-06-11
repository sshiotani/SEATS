using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Web;
using Moq;
using System.Security.Principal;

namespace CcaRegistrationDf.Controllers
{
    [TestClass]
    public class CCAsControllerTests
    {
        [TestMethod]
        public void Index_InputsUserChecksRole_RedirectsUserToAdmin()
        {
            var context = new Mock<HttpContextBase>();
            var mockIdentity = new Mock<IIdentity>();
            context.SetupGet(x => x.User.Identity).Returns(mockIdentity.Object);
            mockIdentity.Setup(x => x.Name).Returns("test_name");
        }
    }
}
