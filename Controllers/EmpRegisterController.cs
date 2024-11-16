using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using System.ComponentModel.DataAnnotations;
using System.Net;
using WebApplication1.Model;
using WebApplication1.Repository;
using WebApplication1.Validations;
using static WebApplication1.Controllers.EmpRegisterController;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EmpRegisterController : ControllerBase
    {
       public Repo repository { get; set; }
        public List<Employee> emp;

        public EmpRegisterController(IMongoDatabase database)
        {
            repository = new Repo(database);
            
        }
        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> RegisterEmployee([FromBody] Employee employeeRequest)
        {
            var response = new ApiBaseResponse<Employee>
            {
                Context = new ResponseContext
                {
                    TransactionId = Guid.NewGuid().ToString(),
                    StatusCode = (int)HttpStatusCode.BadRequest,
                    Timestamp = DateTime.UtcNow.Ticks
                }
            };

            try
            {
                // Validate the incoming request
                if (employeeRequest == null)
                {
                    throw new ArgumentNullException(nameof(employeeRequest), "Invalid request body.");
                }

                 ValidationsRequest.ValidateBody(employeeRequest);
                
               
                await repository.CreateEmployeeAsync(employeeRequest);

                response.Context.StatusCode = (int)HttpStatusCode.OK;
                response.Context.Message = "Employee registered successfully!";
                emp = new List<Employee>();
                emp.Add(employeeRequest);
                response.Response = emp;

                return Ok(response);
            }
            catch (ArgumentNullException ex)
            {
                response.Context.Message = ex.Message;
                return StatusCode((int)HttpStatusCode.BadRequest, response);
            }
            catch (Exception ex)
            {
                response.Context.StatusCode = (int)HttpStatusCode.InternalServerError;
                response.Context.Message = "An error occurred during employee registration.";
                return StatusCode((int)HttpStatusCode.InternalServerError, response);
            }
        }

        // Get Employee by ID
        [HttpGet]
        [Route("get/{id}")]
        public async Task<IActionResult> GetEmployeeById(long id)
        {
            var response = new ApiBaseResponse<Employee>
            {
                Context = new ResponseContext
                {
                    TransactionId = Guid.NewGuid().ToString(),
                    StatusCode = (int)HttpStatusCode.NotFound,
                    Timestamp = DateTime.UtcNow.Ticks
                }
            };

            try
            {
                var employee = await repository.GetEmployeeByIdAsync(id);

                if (employee == null)
                {
                    response.Context.Message = "Employee not found.";
                    return NotFound(response);
                }

                response.Context.StatusCode = (int)HttpStatusCode.OK;
                response.Context.Message = "Employee retrieved successfully!";
                emp = new List<Employee>();
                emp.Add(employee);
                response.Response = emp;

                return Ok(response);
            }
            catch (ArgumentNullException ex)
            {
                response.Context.Message = ex.Message;
                return StatusCode((int)HttpStatusCode.BadRequest, response);
            }
            catch (Exception ex)
            {
                response.Context.StatusCode = (int)HttpStatusCode.InternalServerError;
                response.Context.Message = "An error occurred while fetching employee details.";
                return StatusCode((int)HttpStatusCode.InternalServerError, response);
            }
        }

        // Get All Employees
        [HttpGet]
        [Route("getall")]
        public async Task<IActionResult> GetAllEmployees()
        {
            var response = new ApiBaseResponse<List<Employee>>
            {
                Context = new ResponseContext
                {
                    TransactionId = Guid.NewGuid().ToString(),
                    StatusCode = (int)HttpStatusCode.OK,
                    Timestamp = DateTime.UtcNow.Ticks
                }
            };

            try
            {
                var employees = await repository.GetAllEmployeesAsync();

                response.Context.Message = "Employees retrieved successfully!";
                
                response.Response = employees;

                return Ok(response);
            }
            catch (ArgumentNullException ex)
            {
                response.Context.Message = ex.Message;
                return StatusCode((int)HttpStatusCode.BadRequest, response);
            }
            catch (Exception ex)
            {
                response.Context.StatusCode = (int)HttpStatusCode.InternalServerError;
                response.Context.Message = "An error occurred while fetching employees.";
                return StatusCode((int)HttpStatusCode.InternalServerError, response);
            }
        }

        // Update Employee
        [HttpPut]
        [Route("update/{id}")]
        public async Task<IActionResult> UpdateEmployee(long id, [FromBody] Employee employeeRequest)
        {
            var response = new ApiBaseResponse<Employee>
            {
                Context = new ResponseContext
                {
                    TransactionId = Guid.NewGuid().ToString(),
                    StatusCode = (int)HttpStatusCode.BadRequest,
                    Timestamp = DateTime.UtcNow.Ticks
                }
            };

            try
            {
                if (employeeRequest == null)
                {
                    throw new ArgumentNullException(nameof(employeeRequest), "Invalid request body.");
                }

                employeeRequest.Id = id; // Ensure the ID matches the route parameter
                await repository.UpdateEmployeeAsync(id, employeeRequest);

                response.Context.StatusCode = (int)HttpStatusCode.OK;
                response.Context.Message = "Employee updated successfully!";
                emp = new List<Employee>();
                emp.Add(employeeRequest);
                response.Response = emp;

                return Ok(response);
            }
            catch (ArgumentNullException ex)
            {
                response.Context.Message = ex.Message;
                return StatusCode((int)HttpStatusCode.BadRequest, response);
            }
            catch (Exception ex)
            {
                response.Context.StatusCode = (int)HttpStatusCode.InternalServerError;
                response.Context.Message = "An error occurred while updating the employee.";
                return StatusCode((int)HttpStatusCode.InternalServerError, response);
            }
        }

        // Delete Employee
        [HttpDelete]
        [Route("delete/{id}")]
        public async Task<IActionResult> DeleteEmployee(long id)
        {
            var response = new ApiBaseResponse<string>
            {
                Context = new ResponseContext
                {
                    TransactionId = Guid.NewGuid().ToString(),
                    StatusCode = (int)HttpStatusCode.NotFound,
                    Timestamp = DateTime.UtcNow.Ticks
                }
            };

            try
            {
                var employee = await repository.GetEmployeeByIdAsync(id);

                if (employee == null)
                {
                    response.Context.Message = "Employee not found.";
                    return NotFound(response);
                }

                await repository.DeleteEmployeeAsync(id);

                response.Context.StatusCode = (int)HttpStatusCode.OK;
                response.Context.Message = "Employee deleted successfully!";
                emp = new List<Employee>();
                var employe = new Employee();
                employee.Id = id;  
                emp.Add(employe);
                response.Response = emp;

                return Ok(response);
            }
            catch (ArgumentNullException ex)
            {
                response.Context.Message = ex.Message;
                return StatusCode((int)HttpStatusCode.BadRequest, response);
            }
            catch (Exception ex)
            {
                response.Context.StatusCode = (int)HttpStatusCode.InternalServerError;
                response.Context.Message = "An error occurred while deleting the employee.";
                return StatusCode((int)HttpStatusCode.InternalServerError, response);
            }
        }
    }

    
}
