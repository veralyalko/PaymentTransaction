using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PaymentTransaction.Data;
using PaymentTransaction.Models.Domain;
using PaymentTransaction.Models.DTO;
using PaymentTransaction.Repositories;

namespace PatmentTransactions.AddControllers
{
  // https://localhost:7042/api/transactions
  [Route("transactions")]
  [ApiController]
  public class PaymentTransactionController: ControllerBase
  {
    private readonly PaymentTransactionDbContext dbContext;  
    private readonly ITransactionRepository transactionRepository;
    private readonly IProviderRepository providerRepository;
    private readonly IMapper mapper;

    public PaymentTransactionController(PaymentTransactionDbContext dbContext, 
        ITransactionRepository transactionRepository, 
        IProviderRepository providerRepository,
        IMapper mapper)
    {
        this.dbContext = dbContext;
        this.transactionRepository = transactionRepository;
        this.providerRepository = providerRepository;
        this.mapper = mapper;
    }

    // POST /ingest/{providerName}
    [HttpPost("~/ingest/{providerName}")]
    public async Task<IActionResult> CreateForProviderAsync(string providerName, [FromBody] AddTransactionViaProviderNameDto addTransactionViaProviderNameDto)
    {
        // Accepts raw transaction payloads from a specific provider
        var provider = await providerRepository.GetByNameAsync(providerName);
        if (provider == null)
        {
            return NotFound($"Provider '{providerName}' not found.");
        }

        // Map DTO to Model (automapper)
        var transactionDomainModel = mapper.Map<Transaction>(addTransactionViaProviderNameDto);
        transactionDomainModel.Timestamp = DateTime.UtcNow;
        transactionDomainModel.ProviderId = provider.ProviderId;


        // Use Domain MOdel to create Transaction
        transactionDomainModel = await transactionRepository.CreateForProviderAsync(providerName, transactionDomainModel);

        // Map Domains to DTOs (automapper)
        var transactionDto = mapper.Map<TransactionDto>(transactionDomainModel);

        return CreatedAtAction(nameof(GetById), new { id = transactionDto.Id}, transactionDto);
    }

    // POST To create a new Transaction
    // POST: https://localhost:7042/transactions
    [HttpPost]
    public async Task<IActionResult> Create([FromBody]  AddTransactionRequestDto addTransactionRequestDto)
    {
      // Map DTO to Model (automapper)
      var transactionDomainModel = mapper.Map<Transaction>(addTransactionRequestDto);
      transactionDomainModel.Timestamp = DateTime.UtcNow;

      // Use Domain MOdel to create Transaction
      transactionDomainModel = await transactionRepository.CreateAsync(transactionDomainModel);

      // Map Domains to DTOs (automapper)
      var transactionDto = mapper.Map<TransactionDto>(transactionDomainModel);

      return CreatedAtAction(nameof(GetById), new { id = transactionDto.Id}, transactionDto);
    }


    // Get All transactions
    // GET: https://localhost:7042/transactions
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
      // Get Data from Database - Domain Models
      var transaction = await transactionRepository.GetAllAsync();

      // Map Domains to DTOs (automapper)
      var transactionDto = mapper.Map<List<TransactionDto>>(transaction);
      
      // Return DTO
      return Ok(transactionDto);

    }

    // Get Transaction by ID
    // GET: https://localhost:7042/transactions/{id}
    // GET /transactions/{id}
    [HttpGet("{id:Guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
      // Get Transaction Model From DB
      var transaction = await transactionRepository.GetByIdAsync(id);
      if(transaction == null)
      {
        return NotFound();
      }

      // Map Domains to DTOs (automapper)
      var transactionDto = mapper.Map<TransactionDto>(transaction);

       // Return DTO
      return Ok(transactionDto);

    }

    // Update transaction
    // PUT: https://localhost:7042/transactions/{id}
    // PUT /transactions/{id}
    [HttpPut("{id:Guid}")]
    public async Task<IActionResult> Update([FromRoute] Guid id, 
      [FromBody] UpdateTransactionRequestDto updateTransactionRequestDto)
    {
      // Map DTO to Model (automapper)
      var transactionDomainModel = mapper.Map<Transaction>(updateTransactionRequestDto);

      // Check for transaction exists
      transactionDomainModel = await transactionRepository.UpdateAsync(id, transactionDomainModel);

      if (transactionDomainModel == null)
      {
        return NotFound();
      }

      // Map Domains to DTOs (automapper)
      var transactionDto = mapper.Map<TransactionDto>(transactionDomainModel);

      return Ok(transactionDto);
    }

    // Delete transaction
    // DELETE: https://localhost:7042/transactions/{id}
    // DELETE /transactions/{id}
    [HttpDelete("{id:Guid}")]
    public async Task<IActionResult> Delete([FromRoute] Guid id)
    {
      // Check for transaction exists
      var transactionDomainModel = await transactionRepository.DeleteAsync(id);

      if (transactionDomainModel == null)
      {
        return NotFound();
      }

      // Return deleted transaction
      // Map Domains to DTOs (automapper)
      var transactionDto = mapper.Map<TransactionDto>(transactionDomainModel);

      return Ok(transactionDto);
    }
  }

}