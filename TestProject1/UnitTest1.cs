using EvertekBackend.Controllers;
using EvertekBackend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

//using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Data.Entity;
//using Assert = Xunit.Assert;
//using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace TestProject1
{
    public class UnitTest1
    {
        [Microsoft.VisualStudio.TestTools.UnitTesting.TestClass]
        public class EmployeeControllerTests
        {
            private EmployeeController _controller;
            private Mock<DataContext> _dataContextMock;            
            private Mock<ILogger<EmployeeController>> _iLoggerMock;


            [Microsoft.VisualStudio.TestTools.UnitTesting.TestInitialize]
            public void Initialize()
            {
                _dataContextMock = new Mock<DataContext>();
                _iLoggerMock = new Mock<ILogger<EmployeeController>>();
                _controller = new EmployeeController(_dataContextMock.Object, _iLoggerMock.Object);
            }

            [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethod]
            public async Task GetAllEmployees_ReturnsOk_WhenEmployeesExist()
            {
                // Arrange
                var employees = new List<Employee>
                {
                    new Employee { IdEmployee = 1, Names = "John" },
                    new Employee { IdEmployee = 2, Names = "Jane" },
                };

                _dataContextMock.Setup(x => x.Employee.ToListAsync()).ReturnsAsync(employees);

                // Act
                var result = await _controller.GetAllEmployees();

                // Assert
                Microsoft.VisualStudio.TestTools.UnitTesting.Assert.IsInstanceOfType(result, typeof(OkObjectResult));

                var okResult = result as OkObjectResult;
                var returnedEmployees = okResult.Value as List<Employee>;

                Microsoft.VisualStudio.TestTools.UnitTesting.Assert.AreEqual(2, returnedEmployees.Count);
                Microsoft.VisualStudio.TestTools.UnitTesting.Assert.AreEqual("John", returnedEmployees[0].Names);
                Microsoft.VisualStudio.TestTools.UnitTesting.Assert.AreEqual("Jane", returnedEmployees[1].Names);
            }

            [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethod]
            public async Task GetAllEmployees_ReturnsNotFound_WhenNoEmployeesExist()
            {
                // Arrange
                _dataContextMock.Setup(x => x.Employee.ToListAsync()).ReturnsAsync((List<Employee>)null);

                // Act
                var result = await _controller.GetAllEmployees();

                // Assert
                Microsoft.VisualStudio.TestTools.UnitTesting.Assert.IsInstanceOfType(result, typeof(NotFoundResult));
            }

            [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethod]
            public async Task GetAllEmployees_ReturnsStatusCode404_WhenExceptionThrown()
            {
                // Arrange
                _dataContextMock.Setup(x => x.Employee.ToListAsync()).ThrowsAsync(new Exception());

                // Act
                var result = await _controller.GetAllEmployees();

                // Assert
                Microsoft.VisualStudio.TestTools.UnitTesting.Assert.IsInstanceOfType(result, typeof(ObjectResult));

                var objectResult = result as ObjectResult;

                Microsoft.VisualStudio.TestTools.UnitTesting.Assert.AreEqual(404, objectResult.StatusCode);
                Microsoft.VisualStudio.TestTools.UnitTesting.Assert.AreEqual("Hubo un problema buscando a todos los empleados", objectResult.Value);
            }
        }

    }
}