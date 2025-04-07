using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace PatmentTransactions.AddControllers
{
  // https://localhost:7042/api/paymentmethod
  [Route("api/[controller]")]
  [ApiController]

  public class PaymentMethodController: ControllerBase
  {

    // GET: https://localhost:7042/api/currencies
    [HttpGet]
    public IActionResult GetAllPaymentMethods()
    {

        string[] currencyNames = new string[] { "CreditCard", "ACH", "Wallet"};
        return Ok(currencyNames);

    }

  }

}