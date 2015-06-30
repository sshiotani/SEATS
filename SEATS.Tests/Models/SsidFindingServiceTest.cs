using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using SEATS.Models.Services;
using SEATS.Models;
using Newtonsoft.Json;


namespace SEATS.Tests.Models
{
    [TestClass]
    public class SsidFindingServiceTest
    {
       
        //[TestMethod, TestCategory("Unit")]
        //public async Task SsidFindingService_SubmitsMultipleResultResponseToHttpResponseHandler_ReturnsBadRequestMutipleResults()
        //{
        //    //Arrange
        //    WebApiConfiguration apiInputs = new WebApiConfiguration();
        //    apiInputs.ssidApiBase = "https://api-acsssid-stage.schools.utah.gov/ssid/";
        //    apiInputs.token = "745BD984-1807-42F0-AFC5-754097522498";

        //    SsidFindingService webservice = new SsidFindingService(apiInputs);

        //    HttpResponseMessage response = new HttpResponseMessage();
        //    response.StatusCode = HttpStatusCode.BadRequest;
        //    response.Content = new StringContent("Multiple Records Found.");

        //    //Act

        //    var result = await webservice.SsidWebApiResponseHandler(response);

        //    //Assert 

        //    Assert.AreEqual("Multiple Records Found.", result);
        //}

        //[TestMethod, TestCategory("Unit")]
        //public async Task SsidFindingService_SubmitsEmptyResponseToHttpResponseHandler_ReturnsExceptionUnableToReadResponse()
        //{
        //    //Arrange
        //    string result;

        //    WebApiConfiguration apiInputs = new WebApiConfiguration();
        //    apiInputs.ssidApiBase = "https://api-acsssid-stage.schools.utah.gov/ssid/";
        //    apiInputs.token = "745BD984-1807-42F0-AFC5-754097522498";

        //    SsidFindingService webservice = new SsidFindingService(apiInputs);

        //    HttpResponseMessage response = new HttpResponseMessage();

        //    //Act

        //    try
        //    {
        //        result = await webservice.SsidWebApiResponseHandler(response);
        //    }
        //    catch (Exception exception)
        //    {
        //        result = exception.Message;
        //    }

        //    //Assert 

        //    Assert.AreEqual("Error: Unable to read response content.", result);
        //}



        //[TestMethod, TestCategory("Unit")]
        //public async Task SsidFindingService_SubmitsBadRequestResponseToHttpResponseHandler_ReturnsExceptionUnhandledBadRequest()
        //{
        //    //Arrange
        //    string result;
        //    WebApiConfiguration apiInputs = new WebApiConfiguration();
        //    apiInputs.ssidApiBase = "https://api-acsssid-stage.schools.utah.gov/ssid/";
        //    apiInputs.token = "745BD984-1807-42F0-AFC5-754097522498";

        //    SsidFindingService webservice = new SsidFindingService(apiInputs);

        //    HttpResponseMessage response = new HttpResponseMessage();
        //    response.StatusCode = HttpStatusCode.BadRequest;

        //    //Act
        //    try
        //    {
        //        result = await webservice.SsidWebApiResponseHandler(response);
        //    }
        //    catch (Exception exception)
        //    {
        //        result = exception.Message;
        //    }

        //    //Assert 

        //    Assert.AreEqual("Error: Status Code - Bad Request.", result);
        //}

        //[TestMethod, TestCategory("Unit")]
        //public async Task SsidFindingService_SubmitsNotFoundResponseToHttpResponseHandler_ReturnsNoRecordsFound()
        //{
        //    //Arrange
        //    WebApiConfiguration apiInputs = new WebApiConfiguration();
        //    apiInputs.ssidApiBase = "https://api-acsssid-stage.schools.utah.gov/ssid/";
        //    apiInputs.token = "745BD984-1807-42F0-AFC5-754097522498";

        //    SsidFindingService webservice = new SsidFindingService(apiInputs);

        //    HttpResponseMessage response = new HttpResponseMessage();
        //    response.StatusCode = HttpStatusCode.NotFound;

        //    //Act

        //    var result = await webservice.SsidWebApiResponseHandler(response);

        //    //Assert 

        //    Assert.AreEqual("No Records Found.", result);
        //}

        //[TestMethod, TestCategory("Unit")]
        //public async Task SsidFindingService_SubmitsServerErrorResponseToHttpResponseHandler_ReturnsServerError()
        //{
        //    //Arrange
        //    WebApiConfiguration apiInputs = new WebApiConfiguration();
        //    apiInputs.ssidApiBase = "https://api-acsssid-stage.schools.utah.gov/ssid/";
        //    apiInputs.token = "745BD984-1807-42F0-AFC5-754097522498";

        //    SsidFindingService webservice = new SsidFindingService(apiInputs);

        //    HttpResponseMessage response = new HttpResponseMessage();
        //    response.StatusCode = HttpStatusCode.InternalServerError;
        //    string result;

        //    //Act
        //    try
        //    {
        //        result = await webservice.SsidWebApiResponseHandler(response);
        //    }
        //    catch (Exception exception)
        //    {
        //        result = exception.Message;
        //    }

        //    //Assert 

        //    Assert.AreEqual("Error from SSID server.", result);
        //}

        //[TestMethod, TestCategory("Unit")]
        //public void SsidFindingService_BuildsSSIDRequestStringFromStudentViewModelwithSSID_ReturnsApiRequest()
        //{
        //    //Arrange
        //    WebApiConfiguration apiInputs = new WebApiConfiguration();
        //    apiInputs.ssidApiBase = "https://api-acsssid-stage.schools.utah.gov/ssid/";
        //    apiInputs.token = "745BD984-1807-42F0-AFC5-754097522498";

        //    SsidFindingService webservice = new SsidFindingService(apiInputs);

        //    StudentViewModel student = new StudentViewModel
        //    {
        //       StudentNumber = 1858768,
        //        StudentLastName = "STUDENT",
        //        StudentFirstName = "TEST",
        //        StudentDOB = new DateTime(1994, 1, 1),
        //        //GradeLevel = 12,
        //        //Gender = 'M'
        //    };

        //    //Act
        //    string request = webservice.BuildRequest(student);

        //    //Assert
        //    Assert.AreEqual("1858768/STUDENT/TEST/1994-01-01/U", request);
        //}

        //[TestMethod, TestCategory("Unit")]
        //public void SsidFindingService_BuildsSSIDRequestStringFromStudentViewModelNoSSID_ReturnsRequestString()
        //{
        //    //Arrange
        //    WebApiConfiguration apiInputs = new WebApiConfiguration();
        //    apiInputs.ssidApiBase = "https://api-acsssid-stage.schools.utah.gov/ssid/";
        //    apiInputs.token = "745BD984-1807-42F0-AFC5-754097522498";

        //    SsidFindingService webservice = new SsidFindingService(apiInputs);

        //    StudentViewModel student = new StudentViewModel
        //    {
        //        StudentNumber = 0,
        //        StudentLastName = "STUDENT",
        //        StudentFirstName = "TEST",
        //        StudentDOB = new DateTime(1994, 1, 1),
        //        //GradeLevel = 12,
        //        //Gender = 'M'
        //    };

        //    //Act
        //    string request = webservice.BuildRequest(student);

        //    //Assert
        //    Assert.AreEqual("0/STUDENT/TEST/1994-01-01/U", request);
        //}

        //[TestMethod, TestCategory("Unit")]
        //public void SsidFindingService_BuildsSSIDRequestStringFromStudentViewModelMultiIsTrue_ReturnsRequestString()
        //{
        //    //Arrange
        //    WebApiConfiguration apiInputs = new WebApiConfiguration();
        //    apiInputs.ssidApiBase = "https://api-acsssid-stage.schools.utah.gov/ssid/";
        //    apiInputs.token = "745BD984-1807-42F0-AFC5-754097522498";

        //    SsidFindingService webservice = new SsidFindingService(apiInputs);

        //    StudentViewModel student = new StudentViewModel
        //    {
        //        StudentNumber = 0,
        //        StudentLastName = "STUDENT",
        //        StudentFirstName = "TEST",
        //        StudentDOB = new DateTime(1994, 1, 1),
        //        //GradeLevel = 12,
        //        //Gender = 'M'
        //    };

        //    //Act
        //    string request = webservice.BuildRequest(student, true);

        //    //Assert
        //    Assert.AreEqual("0/STUDENT/TEST/1994-01-01/U?mult=true", request);
        //}

        ///// <summary>
        /////These tests are integration tests for the stage SSID Web Api Server.
        /////They make actual calls to the server.
        ///// If they fail make sure the stage SSID Web Api server is functioning and Test credentials are still valid.
        /////Must be run from SSID service approved IP address.
        ///// </summary>
        //[TestMethod, TestCategory("Integration")]
        //public void Integration_SsidFindingService_TestSSIDWebApiDirectly_ReturnSSID()
        //{
        //    WebApiConfiguration apiInputs = new WebApiConfiguration();
        //    apiInputs.ssidApiBase = "https://api-acsssid-stage.schools.utah.gov/ssid/";
        //    string request = "student/test/1994-01-01/m";
        //    apiInputs.token = "745BD984-1807-42F0-AFC5-754097522498";

        //    using (HttpClient client = new HttpClient())
        //    {
        //        client.BaseAddress = new Uri(apiInputs.ssidApiBase);

        //        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(apiInputs.token);

        //        var response = client.GetAsync(request).Result;

        //        var result = response.Content.ReadAsStringAsync().Result;

        //        Assert.IsTrue(response.IsSuccessStatusCode);
        //        Assert.AreEqual("1858768", result);
        //    }

        //}


        //[TestMethod, TestCategory("Integration")]
        //public async Task Integration_SsidFindingService_CallsGetSsidFromWebApi_StudentViewModelInfoInvalid_ReturnsNoResults()
        //{
        //    // Arrange
        //    WebApiConfiguration apiInputs = new WebApiConfiguration();
        //    apiInputs.ssidApiBase = "https://api-acsssid-stage.schools.utah.gov/ssid/";
        //    string request = "STUDENT/TEST/1994-01-01/F";
        //    apiInputs.token = "745BD984-1807-42F0-AFC5-754097522498";

        //    SsidFindingService webservice = new SsidFindingService(apiInputs);

        //    // Act
        //    var result = await webservice.GetSsidFromWebApi(request);

        //    // Assert
        //    Assert.IsNotNull(result);
        //    Assert.AreEqual("No Records Found.", result);
        //}

        //[TestMethod, TestCategory("Integration")]
        //public async Task Integration_SsidFindingService_CallsGetSsidFromWebApi_ReturnsMultpleResults()
        //{
        //    // Arrange
        //    WebApiConfiguration apiInputs = new WebApiConfiguration();
        //    apiInputs.ssidApiBase = "https://api-acsssid-stage.schools.utah.gov/ssid/";
        //    string request = "mouse/mickey/1997-11-11/m";
        //    apiInputs.token = "745BD984-1807-42F0-AFC5-754097522498";

        //    SsidFindingService webservice = new SsidFindingService(apiInputs);

        //    // Act
        //    var result = await webservice.GetSsidFromWebApi(request);

        //    // Assert
        //    Assert.IsNotNull(result);
        //    Assert.AreEqual("Multiple Records Found.", result);
        //}

        //[TestMethod, TestCategory("Integration")]
        //public async Task Integration_SsidFindingService_CallsGetSsidFromWebApi_ReturnsSingleResult()
        //{
        //    // Arrange
        //    WebApiConfiguration apiInputs = new WebApiConfiguration();
        //    apiInputs.ssidApiBase = "https://api-acsssid-stage.schools.utah.gov/ssid/";
        //    string request = "0/student/test/1994-01-01/m";
        //    apiInputs.token = "745BD984-1807-42F0-AFC5-754097522498";

        //    SsidFindingService webservice = new SsidFindingService(apiInputs);

        //    // Act
        //    var result = await webservice.GetSsidFromWebApi(request);


        //    // Assert
        //    Assert.IsNotNull(result);
        //    Assert.AreEqual("1858768", result);
        //}

        //[TestMethod, TestCategory("Integration")]
        //public async Task Integration_SsidFindingService_CallsGetSsidsFromWebApi_Returns3ResultsInJArray()
        //{
        //    // Arrange
        //    WebApiConfiguration apiInputs = new WebApiConfiguration();
        //    apiInputs.ssidApiBase = "https://api-acsssid-stage.schools.utah.gov/ssid/";
        //    string request = "mouse/mickey/1997-11-11/m?mult=true";
        //    apiInputs.token = "745BD984-1807-42F0-AFC5-754097522498";

        //    SsidFindingService webservice = new SsidFindingService(apiInputs);

        //    // Act
        //    var result = await webservice.GetSsidsFromWebApi(request);

        //    // Assert
        //    Assert.IsNotNull(result);
        //    Assert.AreEqual(3, result.Count);

        //}
    }
}

