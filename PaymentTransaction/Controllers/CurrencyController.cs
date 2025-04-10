using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PaymentTransaction.CustomActionFilters;
using PaymentTransaction.Data;
using PaymentTransaction.Models.Domain;
using PaymentTransaction.Models.DTO;
using PaymentTransaction.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace PaymentTransactions.Controllers
{
  
  // https://localhost:7042/api/currency
  [Route("api/[controller]")]
  [ApiController]
  [DisableAutomaticValidationForController]
  public class CurrencyController: ControllerBase
  {
      private readonly PaymentTransactionDbContext dbContext;  
      private readonly ICurrencyRepository currencyRepository;

      private readonly IMapper mapper;
      public CurrencyController(PaymentTransactionDbContext dbContext, ICurrencyRepository currencyRepository, 
        IMapper mapper)
      {
          this.dbContext = dbContext;
          this.currencyRepository = currencyRepository;
          this.mapper = mapper;
      }

      // Get All Currencies
      // GET: https://localhost:7042/api/currencies
      [HttpGet]
      // [Authorize]
      [MapToApiVersion("1.0")]
      public async Task<IActionResult> GetAll()
      {
          // Get Data from Database - Domain Models
          var currencies = await currencyRepository.GetAllAsync();

          // Map Domains to DTOs (automapper)
          var currencyDto = mapper.Map<List<CurrencyDto>>(currencies);
          
          // Return DTO
          return Ok(currencyDto);

      }

      // Get Currency by ID
      // GET: https://localhost:7042/api/currencies/{id}
      [HttpGet]
      [Route("{id:Guid}")]
      [MapToApiVersion("1.0")]
      public async Task<IActionResult> GetById(Guid id)
      {
          // Get Currency Model From DB
          var currency = await currencyRepository.GetByIdAsync(id);
          if(currency == null)
          {
            return NotFound();
          }

          // Map Domains to DTOs (automapper)
          var currencyDto = mapper.Map<CurrencyDto>(currency);

          // Return DTO
          return Ok(currencyDto);

      }

      // POST To create a new currency
      // POST: https://localhost:7042/api/currencies
      [HttpPost]
      [MapToApiVersion("1.0")]
      public async Task<IActionResult> Create([FromBody]  AddCurrencyRequestDto addCurrencyRequestDto)
      {
          if (ModelState.IsValid)
          {
            // Map DTO to Model (automapper)
            var currencyDomainModel = mapper.Map<Currency>(addCurrencyRequestDto);


            // Use Domain MOdel to create Currency
            currencyDomainModel = await currencyRepository.CreateAsync(currencyDomainModel);

            // Map Domains to DTOs (automapper)
            var currencyDto = mapper.Map<CurrencyDto>(currencyDomainModel);

            return CreatedAtAction(nameof(GetById), new { id = currencyDto.CurrencyId}, currencyDto);
          }
          else
          {
            return BadRequest(ModelState);
          }
      }

      // Update currency
      // PUT: https://localhost:7042/api/currencies/{id}
      [HttpPut]
      [Route("{id:Guid}")]
      [MapToApiVersion("1.0")]
      public async Task<IActionResult> Update([FromRoute] Guid id, 
        [FromBody] UpdateCurrencyRequestDto updateCurrencyRequestDto)
      {
          if (ModelState.IsValid) 
          {
              // Map DTO to Model (automapper)
              var currencyDomainModel = mapper.Map<Currency>(updateCurrencyRequestDto);

              // Check for currency exists
              currencyDomainModel = await currencyRepository.UpdateAsync(id, currencyDomainModel);

              if (currencyDomainModel == null)
              {
                return NotFound();
              }

              // Map Domains to DTOs (automapper)
              var currencyDto = mapper.Map<CurrencyDto>(currencyDomainModel);

              return Ok(currencyDto);
          }
          else
          {
              return BadRequest(ModelState);
          }
      }

      // Delete currency
      // DELETE: https://localhost:7042/api/currencies/{id}
      [HttpDelete]
      [Route("{id:Guid}")]
      [MapToApiVersion("1.0")]
      public async Task<IActionResult> Delete([FromRoute] Guid id)
      {
          // Check for currency exists
          var currencyDomainModel = await currencyRepository.DeleteAsync(id);

          if (currencyDomainModel == null)
          {
            return NotFound();
          }

          // Return deleted currency
          // Map Domains to DTOs (automapper)
          var currencyDto = mapper.Map<CurrencyDto>(currencyDomainModel);

          return Ok(currencyDto);
      }
  }

}