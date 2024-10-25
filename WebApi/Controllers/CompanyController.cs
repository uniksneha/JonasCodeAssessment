using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using AutoMapper;
using BusinessLayer.Model.Interfaces;
using BusinessLayer.Model.Models;
using WebApi.Models;
using log4net;

namespace WebApi.Controllers
{
    public class CompanyController : ApiController
    {
        private readonly ICompanyService _companyService;
        private readonly IMapper _mapper;
        private readonly ILog _logger;

        public CompanyController(ICompanyService companyService, IMapper mapper)
        {
            _companyService = companyService;
            _mapper = mapper;
            _logger = LogManager.GetLogger(typeof(CompanyController));
        }

        // GET api/company
        [HttpGet]
        public async Task<IHttpActionResult> GetAll()
        {
            try
            {
                var items = await _companyService.GetAllCompaniesAsync();
                var companyDtos = _mapper.Map<IEnumerable<CompanyDto>>(items);
                return Ok(companyDtos);
            }
            catch (Exception ex)
            {
                _logger.Error("Error retrieving all companies", ex);
                return InternalServerError();
            }
        }

        // GET api/company/{companyCode}
        [HttpGet]
        public async Task<IHttpActionResult> Get(string companyCode)
        {
            try
            {
                var item = await _companyService.GetCompanyByCodeAsync(companyCode);
                if (item == null)
                {
                    return NotFound();
                }

                var companyDto = _mapper.Map<CompanyDto>(item);
                return Ok(companyDto);
            }
            catch (Exception ex)
            {
                _logger.Error($"Error retrieving company with code {companyCode}", ex);
                return InternalServerError();
            }
        }

        // POST api/company
        [HttpPost]
        public async Task<IHttpActionResult> Post([FromBody] CompanyDto companyDto)
        {
            if (companyDto == null)
            {
                return BadRequest("Invalid data.");
            }

            try
            {
                var companyInfo = _mapper.Map<CompanyInfo>(companyDto);
                var result = await _companyService.SaveCompanyAsync(companyInfo);

                if (result)
                {
                    return CreatedAtRoute("DefaultApi", new { id = companyInfo.CompanyCode }, companyDto);
                }

                return Conflict();
            }
            catch (Exception ex)
            {
                _logger.Error("Error saving company", ex);
                return InternalServerError();
            }
        }

        // PUT api/company/{id}
        [HttpPut]
        public async Task<IHttpActionResult> Put(int id, [FromBody] CompanyDto companyDto)
        {
            if (companyDto == null || companyDto.CompanyCode != id.ToString())
            {
                return BadRequest("Invalid data.");
            }

            try
            {
                var companyInfo = _mapper.Map<CompanyInfo>(companyDto);
                var result = await _companyService.SaveCompanyAsync(companyInfo);

                if (result)
                {
                    return Ok(companyDto);
                }

                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.Error($"Error updating company with id {id}", ex);
                return InternalServerError();
            }
        }

        // DELETE api/company/{id}
        [HttpDelete]
        public async Task<IHttpActionResult> Delete(int id)
        {
            try
            {
                var result = await _companyService.DeleteCompanyAsync(id.ToString());

                if (result)
                {
                    return StatusCode(HttpStatusCode.NoContent);
                }

                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.Error($"Error deleting company with id {id}", ex);
                return InternalServerError();
            }
        }
    }
}
