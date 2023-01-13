using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using EvertekBackend.Models;
using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Protocols;
using System.Configuration;
using Microsoft.AspNetCore.Cors;
using System.Linq;
using Microsoft.Extensions.Logging;
using EvertekBackend.Models;

namespace EvertekBackend.Controllers
{
    [Route("Employee")]
    //[EnableCors("CorsPolicy")]    
    public class EmployeeController : Controller
    {
        private DataContext dataContext;
        private readonly ILogger<EmployeeController> _logger;

        public EmployeeController(DataContext _dataContext, ILogger<EmployeeController> logger)
        {
            dataContext = _dataContext;
            _logger = logger;
        }

        // GET: Employee/GetAllEmployees
        [HttpGet("GetAllEmployees")]
        public async Task<IActionResult> GetAllEmployees()
        {
            _logger.LogInformation("********************        Ingresando a GetAllEmployees          *****************");
            try
            {
                _logger.LogDebug("aa");
                _logger.LogInformation("+++++ Obteniendo todos los empleados de la base de datos  ++++++++");
                var result = await dataContext.Employees.ToListAsync();
                _logger.LogInformation("Resultados: ");
                _logger.LogInformation(result.Count().ToString());

                if (result != null)
                {
                    _logger.LogInformation("Se retorna Ok");
                    return Ok(result);
                }
                else
                {
                    _logger.LogInformation("-------- Error, Empleados no encontrados   --------");
                    return NotFound();
                }
            }
            catch (Exception e)
            {
                _logger.LogInformation("--------- Hubo una exepción: ");
                _logger.LogInformation(e.Message.ToString());
                Console.WriteLine(e.Message);
                return StatusCode(404, "Hubo un problema buscando a todos loswd empleado");
            }
        }

        // GET: Employee/GetEmployee/5
        [HttpGet("GetEmployeeByLastNames/{lastNames}")]
        public async Task<IActionResult> GetEmployeeByLastNames(string lastNames)
        {
            _logger.LogInformation("********************        Ingresando a GetEmployeeByLastNames          *****************");
            _logger.LogInformation("Se recibe el {lastNames}", lastNames);
            try
            {
                _logger.LogInformation("+++++ Obteniendo el empleado de la base de datos  ++++++++");
                var result = await dataContext.Employees.FirstOrDefaultAsync(x => x.LastNames == lastNames);                               

                if (result != null)
                {
                    LogEmployeeInformation(result);
                    _logger.LogInformation("Se retorna Ok");
                    return Ok(result);
                }
                else
                {
                    _logger.LogInformation("-------- Error, Empleado no encontrado   --------");
                    return NotFound();
                }
            }
            catch (Exception e)
            {
                _logger.LogInformation("--------- Hubo una exepción: ");
                _logger.LogInformation(e.Message.ToString());
                Console.WriteLine(e.Message);
                return StatusCode(404, "Hubo un problema buscando al empleado");
            }            
        }

        // GET: Employee/GetEmployee/5
        [HttpGet("GetEmployeeByIdEmployee/{IdEmployee}")]
        public async Task<IActionResult> GetEmployeeByIdEmployee(int IdEmployee)
        {
            _logger.LogInformation("********************        Ingresando a GetEmployeeByIdEmployee          *****************");
            _logger.LogInformation("Se recibe el {IdEmployee}", IdEmployee);
            try
            {
                _logger.LogInformation("+++++ Obteniendo el empleado de la base de datos  ++++++++");
                var result = await dataContext.Employees.FirstOrDefaultAsync(x => x.IdEmployee == IdEmployee);

                if (result != null)
                {
                    LogEmployeeInformation(result);
                    _logger.LogInformation("Se retorna Ok");
                    return Ok(result);
                }
                else
                {
                    _logger.LogInformation("-------- Error, Empleado no encontrado   --------");
                    return NotFound();
                }
            }
            catch (Exception e)
            {
                _logger.LogInformation("--------- Hubo una exepción: ");
                _logger.LogInformation(e.Message.ToString());
                Console.WriteLine(e.Message);
                return StatusCode(404, "Hubo un problema buscando al empleado");
            }
        }

        private void LogEmployeeInformation(Employees result)
        {            
            _logger.LogInformation("Resultados: ");
            _logger.LogInformation("Names {result.Names}", result.Names);
            _logger.LogInformation("LastNames {result.LastNames}", result.LastNames);
            _logger.LogInformation("PhotoLocation {result.PhotoLocation}", result.PhotoLocation);
            _logger.LogInformation("Married {result.Married}", result.Married);
            _logger.LogInformation("HasBrothersOrSisters {result.HasBrothersOrSisters}",
                result.HasBrothersOrSisters);
            _logger.LogInformation("DateOfBirth {result.DateOfBirth}", result.DateOfBirth);            
        }

        private void LogEmployeeInformationWithId(Employees result)
        {
            _logger.LogInformation("IdEmployee: {result.IdEmployee}", result.IdEmployee);
            LogEmployeeInformation(result);
        }
      

        // POST: Employee/SaveEmployee       
        [HttpPost("SaveEmployee")]
        public async Task<ActionResult<Employees>> SaveEmployee([FromBody] Employees employee)
        {
            _logger.LogInformation("********************        Ingresando a SaveEmployee          *****************");
            _logger.LogInformation("Se recibe el employee: ");
            LogEmployeeInformationWithId(employee);
            
            if (employee == null)
            {
                _logger.LogInformation("-------- Error, Empleado no encontrado   --------");
                return NotFound();
            }

            try
            {
                _logger.LogInformation("+++++ Guardando el empleado en la base de datos  ++++++++");
                dataContext.Employees.Add(employee);
                await dataContext.SaveChangesAsync();                
                _logger.LogInformation("Se retorna Ok");
                return Ok(employee);
            }
            catch (Exception e)
            {
                _logger.LogInformation("--------- Hubo una exepción: ");
                _logger.LogInformation(e.Message.ToString());
                Console.WriteLine(e.Message);
                return StatusCode(404, "Hubo un problema guardando al empleado");
            }            
        }

        // UPDATE: Employee/ActualizarColaborador        
        [HttpPatch("UpdateEmployee")]
        public async Task<ActionResult<Employees>> UpdateEmployee([FromBody] Employees employee)
        {
            _logger.LogInformation("********************        Ingresando a UpdateEmployee          *****************");
            _logger.LogInformation("Se recibe el employee: ");
            LogEmployeeInformationWithId(employee);

            try
            {
                _logger.LogInformation("+++++ Buscando el empleado en la base de datos  ++++++++");
                var result = await dataContext.Employees.FirstOrDefaultAsync(b => b.IdEmployee == employee.IdEmployee);

                if (result == null)
                {
                    _logger.LogInformation("-------- Error, Empleado no encontrado   --------");
                    return NotFound();
                }                               
                    
                result.Names = employee.Names;
                result.LastNames = employee.LastNames;
                result.PhotoLocation = employee.PhotoLocation;
                result.Married = employee.Married;
                result.DateOfBirth = employee.DateOfBirth;
                result.HasBrothersOrSisters = employee.HasBrothersOrSisters;

                dataContext.Entry(result).State = Microsoft.EntityFrameworkCore.EntityState.Modified;

                _logger.LogInformation("+++++++++++++++++++        Actualizando empleado    +++++++++++++");
                var operation = dataContext.Employees.Update(result);

                if (result == null)
                {
                    _logger.LogInformation("---------   Hubo un problema actualizando el empleado  --------- ");
                    return NotFound();
                }

                await dataContext.SaveChangesAsync();
                _logger.LogInformation("Empleado actualizado. Se retorna Ok");
                return Ok(result);
            }
            catch (Exception e)
            {
                _logger.LogInformation("--------- Hubo una exepción: ");
                _logger.LogInformation(e.Message.ToString());
                Console.WriteLine(e.Message);
                return StatusCode(404, "Hubo un problema al actualizar el colaborador");
            }
        }

        // DELETE: Employee/DeleteEmployee/5
        [HttpDelete("DeleteEmployee/{id}")]
        public async Task<ActionResult<Employees>> DeleteEmployee(int id)
        {
            _logger.LogInformation("********************        Ingresando a DeleteEmployee          *****************");
            _logger.LogInformation("Se recibe el {id}", id);

            try
            {
                _logger.LogInformation("+++++ Buscando el empleado en la base de datos  ++++++++");
                var employee = await dataContext.Employees.FindAsync(id);

                if (employee == null)
                {
                    _logger.LogInformation("-------- Error, Empleado no encontrado   --------");
                    return NotFound();
                }

                dataContext.Employees.Remove(employee);
                await dataContext.SaveChangesAsync();
                _logger.LogInformation("Empleado elimnado. Se retorna Ok");
                return Ok(employee);
            }
            catch (Exception e)
            {
                _logger.LogInformation("--------- Hubo una exepción: ");
                _logger.LogInformation(e.Message.ToString());
                Console.WriteLine(e.Message);
                return StatusCode(404, "Hubo un problema al eliminar el colaborador");
            }
        }

    }
}
