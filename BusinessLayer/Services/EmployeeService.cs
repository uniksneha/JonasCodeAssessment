using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using BusinessLayer.Model.Interfaces;
using BusinessLayer.Model.Models;
using DataAccessLayer.Model.Interfaces;
using DataAccessLayer.Model.Models;
using log4net;

namespace BusinessLayer.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IMapper _mapper;
        private readonly ILog _logger;

        public EmployeeService(IEmployeeRepository employeeRepository, IMapper mapper)
        {
            _employeeRepository = employeeRepository;
            _mapper = mapper;
            _logger = LogManager.GetLogger(typeof(EmployeeService));
        }

        public async Task<IEnumerable<EmployeeInfo>> GetAllEmployeesAsync()
        {
            try
            {
                var employees = await _employeeRepository.FindAllEmployeeAsync();
                return _mapper.Map<IEnumerable<EmployeeInfo>>(employees);
            }
            catch (Exception ex)
            {
                _logger.Error("Error retrieving all employees", ex);
                throw; 
            }
        }

        public async Task<EmployeeInfo> GetEmployeeByCodeAsync(string employeeCode)
        {
            try
            {
                var employee = await _employeeRepository.FindEmployeeAsync(employeeCode);
                return _mapper.Map<EmployeeInfo>(employee);
            }
            catch (Exception ex)
            {
                _logger.Error($"Error retrieving employee with code {employeeCode}", ex);
                throw; 
            }
        }

        public async Task<bool> SaveEmployeeAsync(EmployeeInfo employeeInfo)
        {
            try
            {
              
                var employee = _mapper.Map<Employee>(employeeInfo);
                employee.LastModified = DateTime.UtcNow; // Set LastModified to current time
                return await _employeeRepository.SaveEmployeeAsync(employee);
            }
            catch (Exception ex)
            {
                _logger.Error("Error saving employee", ex);
                throw; 
            }
        }

        public async Task<bool> UpdateEmployeeAsync(EmployeeInfo employeeInfo)
        {
            try
            {
                
                var employee = _mapper.Map<Employee>(employeeInfo);
                employee.LastModified = DateTime.UtcNow; // Update LastModified
                return await _employeeRepository.SaveEmployeeAsync(employee);
            }
            catch (Exception ex)
            {
                _logger.Error("Error updating employee", ex);
                throw; 
            }
        }

        public async Task<bool> DeleteEmployeeAsync(string employeeCode)
        {
            try
            {
                return await _employeeRepository.DeleteEmployeeAsync(employeeCode);
            }
            catch (Exception ex)
            {
                _logger.Error($"Error deleting employee with code {employeeCode}", ex);
                throw; 
            }
        }
    }
}
