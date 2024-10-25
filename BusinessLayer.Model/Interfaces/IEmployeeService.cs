using System.Collections.Generic;
using System.Threading.Tasks;
using BusinessLayer.Model.Models; // Adjust as necessary

namespace BusinessLayer.Model.Interfaces
{
    public interface IEmployeeService
    {
        Task<IEnumerable<EmployeeInfo>> GetAllEmployeesAsync();
        Task<EmployeeInfo> GetEmployeeByCodeAsync(string employeeCode);
        Task<bool> SaveEmployeeAsync(EmployeeInfo employee);
        Task<bool> UpdateEmployeeAsync(EmployeeInfo employee);
        Task<bool> DeleteEmployeeAsync(string employeeCode);
    }
}
