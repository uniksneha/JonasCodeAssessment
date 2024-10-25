using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using AutoMapper;
using BusinessLayer.Model.Interfaces;
using BusinessLayer.Model.Models;
using DataAccessLayer.Model.Models;
using WebApi.Models;
using log4net;
using System;

namespace WebApi.Controllers
{
    public class EmployeeController : ApiController
    {
        private readonly IEmployeeService _employeeService;
        private readonly IMapper _mapper;
        private readonly ILog _logger;

        public EmployeeController(IEmployeeService employeeService, IMapper mapper)
        {
            _employeeService = employeeService;
            _mapper = mapper;
            _logger = LogManager.GetLogger(typeof(EmployeeController));
        }

        // GET api/employee
        [HttpGet]
        public async Task<IHttpActionResult> GetAll()
        {
            try
            {
                var employees = await _employeeService.GetAllEmployeesAsync();
                var employeeDtos = _mapper.Map<IEnumerable<EmployeeDto>>(employees);
                return Ok(employeeDtos);
            }
            catch (Exception ex)
            {
                _logger.Error("Error retrieving all employees", ex);
                return InternalServerError();
            }
        }

        // GET api/employee/{employeeCode}
        [HttpGet]
        public async Task<IHttpActionResult> Get(string employeeCode)
        {
            try
            {
                var employee = await _employeeService.GetEmployeeByCodeAsync(employeeCode);
                if (employee == null)
                {
                    return NotFound();
                }

                var employeeDto = _mapper.Map<EmployeeDto>(employee);
                return Ok(employeeDto);
            }
            catch (Exception ex)
            {
                _logger.Error($"Error retrieving employee with code {employeeCode}", ex);
                return InternalServerError();
            }
        }

        // POST api/employee
        [HttpPost]
        public async Task<IHttpActionResult> Post([FromBody] EmployeeDto employeeDto)
        {
            if (employeeDto == null)
            {
                return BadRequest("Invalid data.");
            }

            try
            {
                var employeeInfo = _mapper.Map<EmployeeInfo>(employeeDto);
                var result = await _employeeService.SaveEmployeeAsync(employeeInfo);

                if (result)
                {
                    return CreatedAtRoute("DefaultApi", new { id = employeeInfo.EmployeeCode }, employeeDto);
                }

                return Conflict();
            }
            catch (Exception ex)
            {
                _logger.Error("Error saving employee", ex);
                return InternalServerError();
            }
        }

        // PUT api/employee/{employeeCode}
        [HttpPut]
        public async Task<IHttpActionResult> Put(string employeeCode, [FromBody] EmployeeDto employeeDto)
        {
            if (employeeDto == null || employeeDto.EmployeeCode != employeeCode)
            {
                return BadRequest("Invalid data.");
            }

            try
            {
                var employeeInfo = _mapper.Map<EmployeeInfo>(employeeDto);
                var result = await _employeeService.UpdateEmployeeAsync(employeeInfo);

                if (result)
                {
                    return Ok(employeeDto);
                }

                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.Error($"Error updating employee with code {employeeCode}", ex);
                return InternalServerError();
            }
        }

        // DELETE api/employee/{employeeCode}
        [HttpDelete]
        public async Task<IHttpActionResult> Delete(string employeeCode)
        {
            try
            {
                var result = await _employeeService.DeleteEmployeeAsync(employeeCode);

                if (result)
                {
                    return StatusCode(HttpStatusCode.NoContent);
                }

                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.Error($"Error deleting employee with code {employeeCode}", ex);
                return InternalServerError();
            }
        }
    }
}
