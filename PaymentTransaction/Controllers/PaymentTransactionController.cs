using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace PatmentTransactions.AddControllers
{
  // https://localhost:7042/api/transactions
  [Route("api/[controller]")]
  [ApiController]

  public class PaymentTransactionController: ControllerBase
  {

    // GET: https://localhost:7042/api/transactions
    [HttpGet]
    public IActionResult GetAllTransactions()
    {
        string[] transactionNames = new string[] { "transaction1", "transaction2", "transaction3"};
        return Ok(transactionNames);

    }

  }

}