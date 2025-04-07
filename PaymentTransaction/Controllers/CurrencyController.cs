using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace PatmentTransactions.AddControllers
{
  // https://localhost:7042/api/currencies
  [Route("api/[controller]")]
  [ApiController]

  public class CurrencyController: ControllerBase
  {

    // GET: https://localhost:7042/api/currencies
    [HttpGet]
    public IActionResult GetAllProviders()
    {

        string[] currencyNames = new string[] { "USD", "EUR", "JPY","GBP", "AUD", "UAH"};
        return Ok(currencyNames);

    }

  }

}