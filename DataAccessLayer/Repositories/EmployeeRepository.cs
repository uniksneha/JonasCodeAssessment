using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using log4net;
using DataAccessLayer.Model.Interfaces;
using DataAccessLayer.Model.Models;

namespace DataAccessLayer.Repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly IDbWrapper<Employee> _employeeDbWrapper;
        private readonly ILog _logger;

        public EmployeeRepository(IDbWrapper<Employee> employeeDbWrapper)
        {
            _employeeDbWrapper = employeeDbWrapper;
            _logger = LogManager.GetLogger(typeof(EmployeeRepository));
        }

        public async Task<IEnumerable<Employee>> FindAllEmployeeAsync()
        {
            try
            {
                return await _employeeDbWrapper.FindAllAsync();
            }
            catch (Exception ex)
            {
                _logger.Error("Error retrieving all employees", ex);
                throw; 
            }
        }

        public async Task<Employee> FindEmployeeAsync(string employeeCode)
        {
            try
            {
                var results = await _employeeDbWrapper.FindAsync(e => e.EmployeeCode.Equals(employeeCode));
                return results.FirstOrDefault();
            }
            catch (Exception ex)
            {
                _logger.Error($"Error retrieving employee with code {employeeCode}", ex);
                throw; 
            }
        }

        public async Task<bool> SaveEmployeeAsync(Employee employee)
        {
            try
            {
                employee.LastModified = DateTime.UtcNow; // Set LastModified to current time
                return await _employeeDbWrapper.InsertAsync(employee) || await _employeeDbWrapper.UpdateAsync(employee);
            }
            catch (Exception ex)
            {
                _logger.Error("Error saving employee", ex);
                throw; 
            }
        }

        public async Task<bool> DeleteEmployeeAsync(string employeeCode)
        {
            try
            {
                return await _employeeDbWrapper.DeleteAsync(e => e.EmployeeCode.Equals(employeeCode));
            }
            catch (Exception ex)
            {
                _logger.Error($"Error deleting employee with code {employeeCode}", ex);
                throw; 
            }
        }
    }
}
