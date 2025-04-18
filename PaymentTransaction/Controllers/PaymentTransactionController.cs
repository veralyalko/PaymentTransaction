using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PaymentTransaction.CustomActionFilters;
using PaymentTransaction.Data;
using PaymentTransaction.Models.Domain;
using PaymentTransaction.Models.DTO;
using PaymentTransaction.Repositories;
using PaymentTransaction.Attributes;
using Microsoft.AspNetCore.Authorization;
using FluentValidation;

namespace PaymentTransactions.Controllers
{
  
    // https://localhost:7042/api/transactions
    [Route("transactions")]
    [ApiController]
    [ApiVersion("1.0")]
    public class PaymentTransactionController: ControllerBase
    {
        private readonly PaymentTransactionDbContext dbContext;  
        private readonly ITransactionRepository transactionRepository;
        private readonly IProviderRepository providerRepository;
        private readonly IMapper mapper;
        private readonly IValidator<AddTransactionViaProviderNameDto> _validator;
        private readonly ILogger<ProviderController> logger;

        public PaymentTransactionController(PaymentTransactionDbContext dbContext, 
            ITransactionRepository transactionRepository, 
            IProviderRepository providerRepository,
            IMapper mapper,
            IValidator<AddTransactionViaProviderNameDto> validator,
            ILogger<ProviderController> logger)
        {
            this.dbContext = dbContext;
            this.transactionRepository = transactionRepository;
            this.providerRepository = providerRepository;
            this.mapper = mapper;
            _validator = validator;
            this.logger = logger;
        }

        
        // o Maps incoming data to your normalized format
        // o Stores the transaction in the database
        // o Returns a success or error message

        // Accepts raw transaction payloads from a specific provider
        [HttpPost("~/ingest/{providerName}")]
        [RequiresIdempotencyKeyHeader]
        [TypeFilter(typeof(IdempotencyFilter))]
        [MapToApiVersion("1.0")]
        // [ValidateModel] will use fluent validator instead
        public async Task<IActionResult> CreateForProviderAsync(
            string providerName,
            [FromBody] AddTransactionViaProviderNameDto addTransactionViaProviderNameDto)
        {
  
            logger.LogInformation("Payment Transaction Controller: CreateForProviderAsync endpoint is hit. Serilog is working {Time}", DateTime.UtcNow);

            // Add Manual FluentValidation for async rules
            var validationResult = await _validator.ValidateAsync(addTransactionViaProviderNameDto);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors.Select(e => new
                {
                    e.PropertyName,
                    e.ErrorMessage
                }));
            }
            // Cleaned-up version: idempotency is now handled by the filter!

            // Validate providerName
            if (string.IsNullOrWhiteSpace(providerName))
            {
                return BadRequest("Provider name is required.");
            }

            if (providerName.Length > 50)
            {
                return BadRequest("Provider name must be 50 characters or fewer.");
            }

            var provider = await providerRepository.GetByNameAsync(providerName);
            if (provider == null)
            {
                return NotFound($"Provider '{providerName}' not found.");
            }

            var transactionDomainModel = mapper.Map<Transaction>(addTransactionViaProviderNameDto);
            transactionDomainModel.Timestamp = DateTime.UtcNow;
            transactionDomainModel.ProviderId = provider.ProviderId;

            // Get the idempotency key from headers again (trusting filter already validated it)
            var idempotencyKey = Request.Headers["Idempotency-Key"].ToString();
            transactionDomainModel.IdempotencyKey = idempotencyKey;

            transactionDomainModel = await transactionRepository.CreateForProviderAsync(providerName, transactionDomainModel);

            var newTransactionDto = mapper.Map<TransactionDto>(transactionDomainModel);

            return CreatedAtAction(nameof(GetById), new { id = newTransactionDto.Id }, newTransactionDto);
        }

        // POST To create a new Transaction
        // POST: https://localhost:7042/transactions
        [HttpPost]
        [MapToApiVersion("1.0")]
        // [ValidateModel]
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


        // GET /transactions
        // Returns all normalized transactions with optional filters:
        // o ProviderName
        // o Status
        // o Date range (from/to)
        // Get All transactions

        // GET: https://localhost:7042/transactions
        [HttpGet]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetAll(
          [FromQuery] string? filterOn,
          [FromQuery] string? filterQuery,
          [FromQuery] DateTime? fromDate,
          [FromQuery] DateTime? toDate,
          [FromQuery] string? sortBy,
          [FromQuery] bool isAssending
          )
        {
          // Logging test
          logger.LogInformation("Payment Transaction Controller: GetAll endpoint is hit. Serilog is working {Time}", DateTime.UtcNow);

          // Get Data from Database - Domain Models
          var transaction = await transactionRepository.GetAllAsync(filterOn, filterQuery, fromDate, toDate, sortBy, isAssending);

          // Map Domains to DTOs (automapper)
          var transactionDto = mapper.Map<List<TransactionDto>>(transaction);
          
          // Return DTO
          return Ok(transactionDto);

        }

        // Get Transaction by ID
        // GET: https://localhost:7042/transactions/{id}
        // GET /transactions/{id}
        [HttpGet("{id:Guid}")]
        [MapToApiVersion("1.0")]
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
        [ValidateModel]
        [MapToApiVersion("1.0")]
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
        [MapToApiVersion("1.0")]
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

        // GET /summary
        // Returns:
        // o Total number of transactions
        // o Total volume per provider
        // o Breakdown of statuses (Pending, Completed, Failed)

        // GET /transactions/summary
        // [HttpGet("summary")]
        [HttpPost("~/summary")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetSummary()
        {
            var summary = await transactionRepository.GetSummaryAsync();
            return Ok(summary);
        }

        // Simulate webhook-like behavior where providers "push" transaction payloads
        [HttpPost("webhook/{providerName}")]
        [AllowAnonymous]
        [RequiresIdempotencyKeyHeader]
        [TypeFilter(typeof(IdempotencyFilter))]
        [ValidateModel]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> ReceiveWebhookAsync(
            string providerName,
            [FromBody] AddTransactionViaProviderNameDto transactionDto)
        {
            // Validate provider name
            if (string.IsNullOrWhiteSpace(providerName))
            {
                return BadRequest("Provider name is required.");
            }

            if (providerName.Length > 50)
            {
                return BadRequest("Provider name must be 50 characters or fewer.");
            }

            // Get provider
            var provider = await providerRepository.GetByNameAsync(providerName);
            if (provider == null)
            {
                return NotFound($"Provider '{providerName}' not found.");
            }

            // Get the Idempotency-Key (trusted to exist and be valid due to filter)
            var idempotencyKey = Request.Headers["Idempotency-Key"].ToString();

            var transactionDomainModel = mapper.Map<Transaction>(transactionDto);
            transactionDomainModel.Timestamp = DateTime.UtcNow;
            transactionDomainModel.ProviderId = provider.ProviderId;
            transactionDomainModel.IdempotencyKey = idempotencyKey;

            transactionDomainModel = await transactionRepository.CreateForProviderAsync(providerName, transactionDomainModel);

            var newTransactionDto = mapper.Map<TransactionDto>(transactionDomainModel);

            return CreatedAtAction(nameof(GetById), new { id = newTransactionDto.Id }, newTransactionDto);
        }

    }

}