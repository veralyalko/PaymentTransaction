using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PaymentTransaction.CustomActionFilters;
using PaymentTransaction.Data;
using PaymentTransaction.Models.Domain;
using PaymentTransaction.Models.DTO;
using PaymentTransaction.Repositories;
using System.Text.Json;

namespace PaymentTransactions.Controllers
{
  // https://localhost:7042/api/providers
  [Route("api/[controller]")]
  [ApiController]
  [DisableAutomaticValidationForController]
  // [Authorize]
  public class ProviderController: ControllerBase
  {
    private readonly PaymentTransactionDbContext dbContext;  
    private readonly IProviderRepository providerRepository;

    private readonly IMapper mapper;
    public ProviderController(PaymentTransactionDbContext dbContext, IProviderRepository providerRepository, 
      IMapper mapper)
    {
        this.dbContext = dbContext;
        this.providerRepository = providerRepository;
        this.mapper = mapper;
    }

    // Get All Providers
    // GET: https://localhost:7042/api/providers
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
      // Get Data from Database - Domain Models
      var providers = await providerRepository.GetAllAsync();

      // Map Domains to DTOs
      // var providerDto = new List<ProviderDto>();
      // foreach (var provider in providers) 
      // {
      //     providerDto.Add(new ProviderDto()
      //     {
      //         ProviderId = provider.ProviderId,
      //         ProviderName = provider.ProviderName
      //     });
      // }

      // Map Domains to DTOs (automapper)
      var providerDto = mapper.Map<List<ProviderDto>>(providers);
      
      // Return DTO
      return Ok(providerDto);

    }

    // Get Provider by ID
    // GET: https://localhost:7042/api/providers/{id}
    [HttpGet]
    [Route("{id:Guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
      // Get Provider Model From DB
      var provider = await providerRepository.GetByIdAsync(id);
      if(provider == null)
      {
        return NotFound();
      }

      // // Map Provider Model to Provider DTO
      // var providerDto = new ProviderDto
      // {
      //      ProviderId = provider.ProviderId,
      //       ProviderName = provider.ProviderName
      // };

      // Map Domains to DTOs (automapper)
      var providerDto = mapper.Map<ProviderDto>(provider);

       // Return DTO
      return Ok(providerDto);

    }

    // POST To create a new Provider
    // POST: https://localhost:7042/api/providers
    [HttpPost]
    public async Task<IActionResult> Create([FromBody]  AddProviderRequestDto addProviderRequestDto)
    {

      if(ModelState.IsValid)
      {
      // Map DTO to Model (automapper)
      var providerDomainModel = mapper.Map<Provider>(addProviderRequestDto);
      
      // Serialize MetadataJson to JSON if not null
      if (addProviderRequestDto.MetadataJson != null)
      {
        providerDomainModel.MetadataJson = JsonSerializer.Serialize(addProviderRequestDto.MetadataJson);
      }

      // Use Domain MOdel to create Provider
      providerDomainModel = await providerRepository.CreateAsync(providerDomainModel);

      // Map Domains to DTOs (automapper)
      var providerDto = mapper.Map<ProviderDto>(providerDomainModel);

      return CreatedAtAction(nameof(GetById), new { id = providerDto.ProviderId}, providerDto);
      }
      else
      {
        return BadRequest(ModelState);
      }
    }

    // Update provider
    // PUT: https://localhost:7042/api/providers/{id}
    [HttpPut]
    [Route("{id:Guid}")]
    public async Task<IActionResult> Update([FromRoute] Guid id, 
      [FromBody] UpdateProviderRequestDto updateProviderRequestDto)
    {
      if (ModelState.IsValid) {
        // Map DTO to Model (automapper)
        var providerDomainModel = mapper.Map<Provider>(updateProviderRequestDto);

        // Check for provider exists
        // var providerDomainModel = await dbContext.Provider.FirstOrDefaultAsync(x => x.ProviderId == id);
        providerDomainModel = await providerRepository.UpdateAsync(id, providerDomainModel);

        if (providerDomainModel == null)
        {
          return NotFound();
        }

        // Map Domains to DTOs (automapper)
        var providerDto = mapper.Map<ProviderDto>(providerDomainModel);

        return Ok(providerDto);
      }
      else
      {
        return BadRequest(ModelState);
      }
    }

    // Delete provider
    // DELETE: https://localhost:7042/api/providers/{id}
    [HttpDelete]
    [Route("{id:Guid}")]
    public async Task<IActionResult> Delete([FromRoute] Guid id)
    {
      // Check for provider exists
      // var providerDomainModel = await dbContext.Provider.FirstOrDefaultAsync(x => x.ProviderId == id);
      var providerDomainModel = await providerRepository.DeleteAsync(id);

      if (providerDomainModel == null)
      {
        return NotFound();
      }

      // Return deleted provider
      // Map Domains to DTOs (automapper)
      var providerDto = mapper.Map<ProviderDto>(providerDomainModel);

      return Ok(providerDto);
    }
  }

}