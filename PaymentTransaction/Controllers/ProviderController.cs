using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace PatmentTransactions.AddControllers
{
  // https://localhost:7042/api/providers
  [Route("api/[controller]")]
  [ApiController]

  public class ProviderController: ControllerBase
  {

    // GET: https://localhost:7042/api/providers
    [HttpGet]
    public IActionResult GetAllProviders()
    {
        string[] providerNames = new string[] { "PayPal", "Trustly", "Venmo"};
        return Ok(providerNames);

    }

  }

}