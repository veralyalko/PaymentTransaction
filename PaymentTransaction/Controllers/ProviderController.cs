using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PaymentTransaction.Data;
using PaymentTransaction.Models.Domain;
using PaymentTransaction.Models.DTO;

namespace PatmentTransactions.AddControllers
{
  // https://localhost:7042/api/providers
  [Route("api/[controller]")]
  [ApiController]
  public class ProviderController: ControllerBase
  {
    private readonly PaymentTransactionDbContext dbContext;  

    public ProviderController(PaymentTransactionDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    // Get All Providers
    // GET: https://localhost:7042/api/providers
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
      // Get Data from Database - Domain Models
      var providers = await dbContext.Provider.ToListAsync();

      // Map Domains to DTOs
      var providerDto = new List<ProviderDto>();
      foreach (var provider in providers) 
      {
          providerDto.Add(new ProviderDto()
          {
              ProviderId = provider.ProviderId,
              ProviderName = provider.ProviderName
          });
      }

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
      var provider = await dbContext.Provider.FirstOrDefaultAsync(x => x.ProviderId == id);
      if(provider == null)
      {
        return NotFound();
      }

      // Map Provider Model to Provider DTO
      var providerDto = new ProviderDto
      {
           ProviderId = provider.ProviderId,
            ProviderName = provider.ProviderName
      };

       // Return DTO
      return Ok(providerDto);

    }

    // POST To create a new Provider
    // POST: https://localhost:7042/api/providers
    [HttpPost]
    public async Task<IActionResult> Create([FromBody]  AddProviderRequestDto addProviderRequestDto)
    {
      // Map DTO to Model
      var providerDomainModel = new Provider
      {
        ProviderName = addProviderRequestDto.ProviderName,
      };

      // Use Domain MOdel to create Provider
      await dbContext.Provider.AddAsync(providerDomainModel);
      await dbContext.SaveChangesAsync();

      // Map Model back to DTO
      var providerDto =  new ProviderDto{
        ProviderId = providerDomainModel.ProviderId,
        ProviderName = providerDomainModel.ProviderName
      };

      return CreatedAtAction(nameof(GetById), new { id = providerDto.ProviderId}, providerDto);
    }

    // Update provider
    // PUT: https://localhost:7042/api/providers/{id}
    [HttpPut]
    [Route("{id:Guid}")]
    public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateProviderRequestDto UpdateProviderRequestDto)
    {
      // Check for provider exists
      var providerDomainModel = await dbContext.Provider.FirstOrDefaultAsync(x => x.ProviderId == id);

      if (providerDomainModel == null)
      {
        return NotFound();
      }

      // Map Dto to Model
      providerDomainModel.ProviderName = UpdateProviderRequestDto.ProviderName;

      await dbContext.SaveChangesAsync();

      // Convert Mode to DTO
      var providerDto = new ProviderDto
      {
        ProviderId = providerDomainModel.ProviderId,
        ProviderName = providerDomainModel.ProviderName
      };

      return Ok(providerDto);
    }

    // Delete provider
    // DELETE: https://localhost:7042/api/providers/{id}
    [HttpDelete]
    [Route("{id:Guid}")]
    public async Task<IActionResult> Delete([FromRoute] Guid id)
    {
      // Check for provider exists
      var providerDomainModel = await dbContext.Provider.FirstOrDefaultAsync(x => x.ProviderId == id);

      if (providerDomainModel == null)
      {
        return NotFound();
      }

      dbContext.Provider.Remove(providerDomainModel);
      await dbContext.SaveChangesAsync();

      // Return deleted provider
      // Map Model to DTO

      // Convert Mode to DTO
      var providerDto = new ProviderDto
      {
        ProviderId = providerDomainModel.ProviderId,
        ProviderName = providerDomainModel.ProviderName
      };

      return Ok(providerDto);
    }
  }

}