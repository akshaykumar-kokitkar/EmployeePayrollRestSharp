using System.Collections.Generic;
using System.Net;
using EmployeePayrollRestSharp;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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
            // Initialize the request object with method and endpoint
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
            Assert.AreEqual(response.StatusCode, HttpStatusCode.OK); 
            List<Employee> dataResponse = JsonConvert.DeserializeObject<List<Employee>>(response.Content);
            Assert.AreEqual(7, dataResponse.Count);

            foreach (Employee emp in dataResponse)
            {
                System.Console.WriteLine("id: " + emp.id + ", Name: " + emp.name + ", Salary: " + emp.salary);
            }
        }

        //UC2
        [TestMethod]
        public void OnCallingPostAPI_ReturnEmployeeObject()
        {
            // Arrange
            // endpoint and POST method
            RestRequest request = new RestRequest("/employees", Method.Post);
            JObject jObjectBody = new JObject();         
            jObjectBody.Add("name", "Clark");
            jObjectBody.Add("salary", "15000");

            // Added parameters to the request object, content-type and jObjectBody with the request
            request.AddParameter("application/json", jObjectBody, ParameterType.RequestBody);

            //Act
            RestResponse response = client.ExecuteAsync(request).Result;

            //Assert
            Assert.AreEqual(response.StatusCode, HttpStatusCode.Created);
            Employee dataResponse = JsonConvert.DeserializeObject<Employee>(response.Content);
            Assert.AreEqual("Clark", dataResponse.name);
            Assert.AreEqual("15000", dataResponse.salary);
            System.Console.WriteLine(response.Content);
        }

       //UC3
        [TestMethod]
        public void GivenMultipleEmployee_OnPost_ThenShouldReturnEmployeeList()
        {
            // Arrange
            List<Employee> employeeList = new List<Employee>();
            employeeList.Add(new Employee { name = "sahil", salary = "15000" });
            employeeList.Add(new Employee { name = "ajit", salary = "10000" });
            
            foreach (var emp in employeeList)
            {
                // POST method and endpoint
                RestRequest request = new RestRequest("/employees", Method.Post);
                JObject jsonObj = new JObject();
                jsonObj.Add("name", emp.name);
                jsonObj.Add("salary", emp.salary);
                
                request.AddParameter("application/json", jsonObj, ParameterType.RequestBody);

                //Act
                RestResponse response = client.ExecuteAsync(request).Result;

                //Assert
                Assert.AreEqual(response.StatusCode, HttpStatusCode.Created);
                Employee employee = JsonConvert.DeserializeObject<Employee>(response.Content);
                Assert.AreEqual(emp.name, employee.name);
                Assert.AreEqual(emp.salary, employee.salary);
                System.Console.WriteLine(response.Content);
            }
        }

    }
}