using BusinessLayer.Model.Interfaces;
using System.Collections.Generic;
using AutoMapper;
using BusinessLayer.Model.Models;
using DataAccessLayer.Model.Interfaces;
using DataAccessLayer.Model.Models;
using System.Threading.Tasks;
using log4net;
using System;

namespace BusinessLayer.Services
{
    public class CompanyService : ICompanyService
    {
        private readonly ICompanyRepository _companyRepository;
        private readonly IMapper _mapper;
        private readonly ILog _logger;

        public CompanyService(ICompanyRepository companyRepository, IMapper mapper)
        {
            _companyRepository = companyRepository;
            _mapper = mapper;
            _logger = LogManager.GetLogger(typeof(CompanyService));
        }

        public async Task<IEnumerable<CompanyInfo>> GetAllCompaniesAsync()
        {
            try
            {
                var res = await _companyRepository.GetAllAsync();
                return _mapper.Map<IEnumerable<CompanyInfo>>(res);
            }
            catch (Exception ex)
            {
                _logger.Error("Error retrieving all companies", ex);
                throw; 
            }
        }

        public async Task<CompanyInfo> GetCompanyByCodeAsync(string companyCode)
        {
            try
            {
                var result = await _companyRepository.GetByCodeAsync(companyCode);
                return _mapper.Map<CompanyInfo>(result);
            }
            catch (Exception ex)
            {
                _logger.Error($"Error retrieving company with code {companyCode}", ex);
                throw; 
            }
        }

        public async Task<bool> SaveCompanyAsync(CompanyInfo companyInfo)
        {
            try
            {
                // Check if the company already exists
                var existingCompany = await _companyRepository.GetByCodeAsync(companyInfo.CompanyCode);

                if (existingCompany == null)
                {
                    return false; // Company not found
                }

                var company = _mapper.Map<Company>(companyInfo);
                return await _companyRepository.SaveCompanyAsync(company);
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
                return await _companyRepository.DeleteCompanyAsync(companyCode);
            }
            catch (Exception ex)
            {
                _logger.Error($"Error deleting company with code {companyCode}", ex);
                throw; 
            }
        }
    }
}
