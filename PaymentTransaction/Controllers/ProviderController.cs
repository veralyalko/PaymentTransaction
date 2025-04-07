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
    public IActionResult GetAll()
    {
      // Get Data from Database - Domain Models
      var providers = dbContext.Provider.ToList();

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
    public IActionResult GetById(Guid id)
    {
      // Get Provider Model From DB
      var provider = dbContext.Provider.FirstOrDefault(x => x.ProviderId == id);
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

  }

}