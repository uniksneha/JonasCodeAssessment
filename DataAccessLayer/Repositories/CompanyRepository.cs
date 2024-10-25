using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using log4net;
using DataAccessLayer.Model.Interfaces;
using DataAccessLayer.Model.Models;

namespace DataAccessLayer.Repositories
{
    public class CompanyRepository : ICompanyRepository
    {
        private readonly IDbWrapper<Company> _companyDbWrapper;
        private readonly ILog _logger;

        public CompanyRepository(IDbWrapper<Company> companyDbWrapper)
        {
            _companyDbWrapper = companyDbWrapper;
            _logger = LogManager.GetLogger(typeof(CompanyRepository));
        }

        public async Task<IEnumerable<Company>> GetAllAsync()
        {
            try
            {
                return await _companyDbWrapper.FindAllAsync();
            }
            catch (Exception ex)
            {
                _logger.Error("Error retrieving all companies", ex);
                throw; 
            }
        }

        public async Task<Company> GetByCodeAsync(string companyCode)
        {
            try
            {
                var results = await _companyDbWrapper.FindAsync(c => c.CompanyCode.Equals(companyCode));
                return results.FirstOrDefault();
            }
            catch (Exception ex)
            {
                _logger.Error($"Error retrieving company with code {companyCode}", ex);
                throw; 
            }
        }

        public async Task<bool> SaveCompanyAsync(Company company)
        {
            try
            {
                var itemRepo = _companyDbWrapper.Find(t =>
                    t.SiteId.Equals(company.SiteId) && t.CompanyCode.Equals(company.CompanyCode))?.FirstOrDefault();

                if (itemRepo != null)
                {
                    // Update existing company
                    itemRepo.CompanyName = company.CompanyName;
                    itemRepo.AddressLine1 = company.AddressLine1;
                    itemRepo.AddressLine2 = company.AddressLine2;
                    itemRepo.AddressLine3 = company.AddressLine3;
                    itemRepo.Country = company.Country;
                    itemRepo.EquipmentCompanyCode = company.EquipmentCompanyCode;
                    itemRepo.FaxNumber = company.FaxNumber;
                    itemRepo.PhoneNumber = company.PhoneNumber;
                    itemRepo.PostalZipCode = company.PostalZipCode;
                    itemRepo.LastModified = company.LastModified;
                    return _companyDbWrapper.Update(itemRepo);
                }

                // Insert new company
                return await _companyDbWrapper.InsertAsync(company);
            }
            catch (Exception ex)
            {
                _logger.Error("Error saving company", ex);
                throw; 
            }
        }

        public async Task<bool> DeleteCompanyAsync(string companyCode)
        {
            try
            {
                return await _companyDbWrapper.DeleteAsync(c => c.CompanyCode.Equals(companyCode));
            }
            catch (Exception ex)
            {
                _logger.Error($"Error deleting company with code {companyCode}", ex);
                throw;
            }
        }
    }
}
