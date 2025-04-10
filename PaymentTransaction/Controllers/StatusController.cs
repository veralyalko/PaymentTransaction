using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PaymentTransaction.CustomActionFilters;
using PaymentTransaction.Data;
using PaymentTransaction.Models.Domain;
using PaymentTransaction.Models.DTO;
using PaymentTransaction.Repositories;

namespace PaymentTransactions.Controllers
{
    // https://localhost:7042/api/status
    [Route("api/[controller]")]
    [ApiController]
    [DisableAutomaticValidationForController]
    public class StatusController: ControllerBase
    {
        private readonly PaymentTransactionDbContext dbContext;  
        private readonly IStatusRepository statusRepository;

        private readonly IMapper mapper;
        public StatusController(PaymentTransactionDbContext dbContext, IStatusRepository statusRepository, 
          IMapper mapper)
        {
            this.dbContext = dbContext;
            this.statusRepository = statusRepository;
            this.mapper = mapper;
        }

        // Get All Statuses
        // GET: https://localhost:7042/api/status
        [HttpGet]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetAll()
        {
            // // Test GLobal Exception working: TEST ONLY. COMMENT OUT
            // throw new Exception("This is Demo exception");

            // Get Data from Database - Domain Models
            var status = await statusRepository.GetAllAsync();

            // Map Domains to DTOs (automapper)
            var statusDto = mapper.Map<List<StatusDto>>(status);
            
            // Return DTO
            return Ok(statusDto);

        }

        // Get Status by ID
        // GET: https://localhost:7042/api/status/{id}
        [HttpGet]
        [Route("{id:Guid}")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetById(Guid id)
        {
            // Get Status Model From DB
            var status = await statusRepository.GetByIdAsync(id);
            if(status == null)
            {
                return NotFound();
            }

            // Map Domains to DTOs (automapper)
            var statusDto = mapper.Map<StatusDto>(status);

            // Return DTO
            return Ok(statusDto);

        }

        // POST To create a new Status
        // POST: https://localhost:7042/api/status
        [HttpPost]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> Create([FromBody]  AddStatusRequestDto addStatusRequestDto)
        {
            if (ModelState.IsValid) 
            {
              // Map DTO to Model (automapper)
              var statusDomainModel = mapper.Map<Status>(addStatusRequestDto);


              // Use Domain MOdel to create Provider
              statusDomainModel = await statusRepository.CreateAsync(statusDomainModel);

              // Map Domains to DTOs (automapper)
              var statusDto = mapper.Map<StatusDto>(statusDomainModel);

              return CreatedAtAction(nameof(GetById), new { id = statusDto.StatusId}, statusDto);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        // Update status
        // PUT: https://localhost:7042/api/status/{id}
        [HttpPut]
        [Route("{id:Guid}")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> Update([FromRoute] Guid id, 
          [FromBody] UpdateStatusRequestDto updateStatusRequestDto)
        {
            if (ModelState.IsValid)
            {
                // Map DTO to Model (automapper)
                var statusDomainModel = mapper.Map<Status>(updateStatusRequestDto);

                // Check for status exists
                statusDomainModel = await statusRepository.UpdateAsync(id, statusDomainModel);

                if (statusDomainModel == null)
                {
                    return NotFound();
                }

                // Map Domains to DTOs (automapper)
                var statusDto = mapper.Map<StatusDto>(statusDomainModel);

                return Ok(statusDto);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        // Delete status
        // DELETE: https://localhost:7042/api/status/{id}
        [HttpDelete]
        [Route("{id:Guid}")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            // Check for status exists
            var statusDomainModel = await statusRepository.DeleteAsync(id);

            if (statusDomainModel == null)
            {
                return NotFound();
            }

            // Return deleted status
            // Map Domains to DTOs (automapper)
            var statusDto = mapper.Map<StatusDto>(statusDomainModel);

            return Ok(statusDto);
        }
    }

}