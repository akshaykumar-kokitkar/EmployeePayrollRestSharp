using System.Collections.Generic;
using System.Net;
using EmployeePayrollRestSharp;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using RestSharp;

namespace EmployeePayrollRestSharpTest
{
    [TestClass]
    public class RestSharpTestCase
    {
        RestClient client;

        [TestInitialize]
        public void Setup()
        {
            client = new RestClient("http://localhost:4000");
        }

        private RestResponse getEmployeeList()
        {
            // Arrange
            // Initialize the request object with proper method and endpoint
            RestRequest request = new RestRequest("/employees", Method.Get);

            // Act
            // Execute the request
            RestResponse response = client.ExecuteAsync(request).Result;
            return response;
        }

        //UC1
        [TestMethod]
        public void onCallingGETApi_ReturnEmployeeList()
        {
            RestResponse response = getEmployeeList();

            // Assert
            Assert.AreEqual(response.StatusCode, HttpStatusCode.OK); //System.Net namespace
            List<Employee> dataResponse = JsonConvert.DeserializeObject<List<Employee>>(response.Content);
            Assert.AreEqual(7, dataResponse.Count);

            foreach (Employee emp in dataResponse)
            {
                System.Console.WriteLine("id: " + emp.id + ", Name: " + emp.name + ", Salary: " + emp.salary);
            }
        }
    }
}