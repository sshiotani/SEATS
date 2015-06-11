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
    //    [TestMethod]
    //    public async Task CcaInterface_LooksUpCcaFromDbMapstoView_ReturnsCorrectMapping()
    //    {
    //        //Arrange
    //        var data = new List<CCA> 
    //        { 
    //            new CCA
    //            { 
    //                ID = 1,
    //                StudentID = 2,
    //                OnlineCourseID = 5,
    //                CourseCreditID  = 8,
    //                ApplicationSubmissionDate = new DateTime(2015,1,2)
    //            }, 
    //            new CCA 
    //            {
    //                ID = 3,
    //                StudentID = 4,
    //                OnlineCourseID = 6,
    //                CourseCreditID = 9,
    //                ApplicationSubmissionDate = new DateTime(2015,3,4)
    //            }, 
    //            new CCA
    //            { 
    //                ID = 19,
    //                StudentID = 20,
    //                OnlineCourseID = 7,
    //                CourseCreditID = 10,
    //                ApplicationSubmissionDate = new DateTime(2015,5,6)
    //            }
    //        }.AsQueryable(); 

    //        var mockSet = new Mock<DbSet<CCA>>();
    //        mockSet.As<IDbAsyncEnumerable<CCA>>()
    //            .Setup(m => m.GetAsyncEnumerator())
    //            .Returns(new TestDbAsyncEnumerator<CCA>(data.GetEnumerator()));

    //        mockSet.As<IQueryable<CCA>>()
    //            .Setup(m => m.Provider)
    //            .Returns(new TestDbAsyncQueryProvider<CCA>(data.Provider)); 
 
           
    //        mockSet.As<IQueryable<CCA>>().Setup(m => m.Expression).Returns(data.Expression);
    //        mockSet.As<IQueryable<CCA>>().Setup(m => m.ElementType).Returns(data.ElementType);
    //        mockSet.As<IQueryable<CCA>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator()); 

    //        var mockContext = new Mock<SeatsContext>();
    //        mockContext.Setup(m => m.CCAs).Returns(mockSet.Object);

    //        var controller = new AdminController(mockContext.Object);
    //        controller.Index();

    //        //mockSet.Verify(m => m.Add(It.IsAny<Blog>()), Times.Once());
    //        //mockContext.Verify(m => m.SaveChanges(), Times.Once()); 

    //        //Act

    //        var result = await controller.CcaInterface() as ViewResult;
    //        var model = result.Model as List<UsoeCcaViewModel>;

    //        //Assert 
    //        Assert.AreEqual(3, model.Count);
            
    //        Assert.AreEqual(1, model[0].CcaID);
    //        Assert.AreEqual(3, model[1].CcaID); 
    //        Assert.AreEqual(19, model[2].CcaID);
    //        Assert.AreEqual(2, model[0].StudentID);
    //        Assert.AreEqual(4, model[1].StudentID);
    //        Assert.AreEqual(20, model[2].StudentID);
    //        Assert.AreEqual(5, model[0].OnlineCourseID);
    //        Assert.AreEqual(6, model[1].OnlineCourseID);
    //        Assert.AreEqual(7, model[2].OnlineCourseID);
    //        Assert.AreEqual(8, model[0].CourseCreditID);
    //        Assert.AreEqual(9, model[1].CourseCreditID);
    //        Assert.AreEqual(10, model[2].CourseCreditID);
    //        Assert.AreEqual(0, DateTime.Compare(new DateTime(2015, 1, 2), model[0].ApplicationSubmissionDate));
    //        Assert.AreEqual(0, DateTime.Compare(new DateTime(2015, 3, 4), model[1].ApplicationSubmissionDate));
    //        Assert.AreEqual(0, DateTime.Compare(new DateTime(2015, 5, 6), model[2].ApplicationSubmissionDate));
    //        Assert.AreNotEqual("Error", result.ViewName);
            
    //    }

    //    [TestMethod]
    //    public async Task CcaInterface_LooksUpCcaGetUsoeEditItemFromDbMapstoView_ReturnsCorrectMapping()
    //    {
    //        //Arrange
    //        var data = new List<CCA> 
    //        { 
    //            new CCA
    //            { 
    //                CourseFee = 218M,
    //                BudgetPrimaryProvider = 650M,
    //                IsRemediation = true,
    //                IsBusinessAdministratorAcceptRejectEnrollment = true,
    //                IsCounselorSigned = true,
    //                IsCourseConsistentWithStudentSEOP = true,
    //                IsEnrollmentNoticeSent = true,
    //                PrimaryNotificationDate = new DateTime(2016,2,3),
    //                NotificationDate = new DateTime(2016,2,4),
    //                PriorDisbursementProvider = 200M,
    //                RecordNotes = "Notes for 1",
    //                RemediationPeriodBegins = new DateTime(2016,3,5),
    //                TotalDisbursementsProvider = 900M,
    //                Offset = 20M,
    //                Distribution = 300M,
    //                WithdrawalDate = new DateTime(2017,5,6),
    //                Grand_Total = 500M,
    //                Notes = "Notes"
    //            }, 
    //            new CCA 
    //            {
    //               CourseFee = 238M,
    //               BudgetPrimaryProvider = 655M,
    //               IsRemediation = false,
    //               IsEnrollmentNoticeSent = true,
    //               PrimaryNotificationDate = new DateTime(2016,2,6),
    //               NotificationDate = new DateTime(2016,2,7),
    //               PriorDisbursementProvider = 200M,
    //               RecordNotes = "Notes for 2",
    //               RemediationPeriodBegins = new DateTime(2016,3,8),
    //               TotalDisbursementsProvider = 1000M,
    //               Offset = 20M,
    //                Distribution = 300M,
    //                Grand_Total = 500M,
    //                Notes = "Notes"
    //            }, 
    //            new CCA
    //            { 
    //                CourseFee = 372M,
    //                BudgetPrimaryProvider = 660M,
    //                IsRemediation = false,
    //               IsEnrollmentNoticeSent = true,
    //               PriorDisbursementProvider = 0M,
    //               RecordNotes = "Notes for 3",
    //               RemediationPeriodBegins = new DateTime(2016,3,9),
    //               TotalDisbursementsProvider = 10000M,
    //               Offset = 20M,
    //                Distribution = 300M,
    //                Grand_Total = 500M,
    //                Notes = "Notes"
    //            }
    //        }.AsQueryable();

    //        var mockSet = new Mock<DbSet<CCA>>();
    //        mockSet.As<IDbAsyncEnumerable<CCA>>()
    //            .Setup(m => m.GetAsyncEnumerator())
    //            .Returns(new TestDbAsyncEnumerator<CCA>(data.GetEnumerator()));

    //        mockSet.As<IQueryable<CCA>>()
    //            .Setup(m => m.Provider)
    //            .Returns(new TestDbAsyncQueryProvider<CCA>(data.Provider));


    //        mockSet.As<IQueryable<CCA>>().Setup(m => m.Expression).Returns(data.Expression);
    //        mockSet.As<IQueryable<CCA>>().Setup(m => m.ElementType).Returns(data.ElementType);
    //        mockSet.As<IQueryable<CCA>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

    //        var mockContext = new Mock<SeatsContext>();
    //        mockContext.Setup(m => m.CCAs).Returns(mockSet.Object);

    //        var controller = new AdminController(mockContext.Object);
    //        controller.Index();

    //        //mockSet.Verify(m => m.Add(It.IsAny<Blog>()), Times.Once());
    //        //mockContext.Verify(m => m.SaveChanges(), Times.Once()); 

    //        //Act

    //        var result = await controller.CcaInterface() as ViewResult;
    //        var model = result.Model as List<UsoeCcaViewModel>;

    //        //Assert 
    //        Assert.AreEqual(3, model.Count);

    //        Assert.AreEqual(218M, model[0].CourseFee);
    //        Assert.AreEqual(238M, model[1].CourseFee);
    //        Assert.AreEqual(372M, model[2].CourseFee);
    //        Assert.AreEqual(650M, model[0].BudgetPrimaryProvider);
    //        Assert.AreEqual(655M, model[1].BudgetPrimaryProvider);
    //        Assert.AreEqual(660M, model[2].BudgetPrimaryProvider);
    //        Assert.AreEqual(true, model[0].IsRemediation);
    //        Assert.AreEqual(false, model[1].IsRemediation);
    //        Assert.AreEqual(false, model[2].IsRemediation);
    //        Assert.AreEqual(true, model[0].IsEnrollmentNoticeSent);
    //        Assert.AreEqual(true, model[1].IsEnrollmentNoticeSent);
    //        Assert.AreEqual(true, model[2].IsEnrollmentNoticeSent);
    //        Assert.AreEqual(0, DateTime.Compare(new DateTime(2016, 2, 3),(DateTime)model[0].PrimaryNotificationDate));
    //        Assert.AreEqual(0, DateTime.Compare(new DateTime(2016, 2, 6), (DateTime)model[1].PrimaryNotificationDate));
    //        Assert.AreEqual(null, model[2].PrimaryNotificationDate);
    //        Assert.AreEqual(0, DateTime.Compare(new DateTime(2016, 2, 4), (DateTime)model[0].NotificationDate));
    //        Assert.AreEqual(0, DateTime.Compare(new DateTime(2016, 2, 7), (DateTime)model[1].NotificationDate));
    //        Assert.AreEqual(null, model[2].NotificationDate);
    //        Assert.AreEqual(650M, model[0].BudgetPrimaryProvider);
    //        Assert.AreEqual(900M, model[0].TotalDisbursementsProvider);
    //        Assert.AreEqual("Notes for 1", model[0].RecordNotes);
    //        Assert.AreEqual(0, DateTime.Compare(new DateTime(2016, 3, 5), (DateTime)model[0].RemediationPeriodBegins));
    //        Assert.AreEqual(20M, model[0].Offset);
    //        Assert.AreEqual(300M, model[0].Distribution);
    //        Assert.AreEqual(0, DateTime.Compare(new DateTime(2017, 5, 6), (DateTime)model[0].WithdrawalDate));
    //        Assert.AreEqual(500M, model[0].Grand_Total);
    //        Assert.AreEqual("Notes", model[0].Notes);
    //        //Assert.AreNotEqual("Error", result.ViewName);

    //    }
    }
}
