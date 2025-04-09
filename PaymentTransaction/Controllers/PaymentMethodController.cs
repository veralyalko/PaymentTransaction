using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PaymentTransaction.CustomActionFilters;
using PaymentTransaction.Data;
using PaymentTransaction.Models.Domain;
using PaymentTransaction.Models.DTO;
using PaymentTransaction.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Http;

namespace PaymentTransactions.Controllers
{
  
    // https://localhost:7042/api/paymentmethod
    [Route("api/[controller]")]
    [ApiController]
    [DisableAutomaticValidationForController]
    public class PaymentMethodController: ControllerBase
    {
        private readonly PaymentTransactionDbContext dbContext;  
        private readonly IPaymentMethodRepository paymentMethodRepository;

        private readonly IMapper mapper;
        public PaymentMethodController(PaymentTransactionDbContext dbContext, IPaymentMethodRepository paymentMethodRepository, 
          IMapper mapper)
        {
            this.dbContext = dbContext;
            this.paymentMethodRepository = paymentMethodRepository;
            this.mapper = mapper;
        }

        // Get All Payment Methods
        // GET: https://localhost:7042/api/paymentmethod
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            // Get Data from Database - Domain Models
            var paymentMethods = await paymentMethodRepository.GetAllAsync();

            // Map Domains to DTOs (automapper)
            var paymentMethodDto = mapper.Map<List<PaymentMethodDto>>(paymentMethods);
            
            // Return DTO
            return Ok(paymentMethodDto);

        }

        // Get Payment Method by ID
        // GET: https://localhost:7042/api/paymentmethods/{id}
        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            // Get PaymentMethod Model From DB
            var paymentMethod = await paymentMethodRepository.GetByIdAsync(id);
            if(paymentMethod == null)
            {
                return NotFound();
            }

            // Map Domains to DTOs (automapper)
            var paymentMethodDto = mapper.Map<PaymentMethodDto>(paymentMethod);

            // Return DTO
            return Ok(paymentMethodDto);

        }

        // POST To create a new payment method type
        // POST: https://localhost:7042/api/paymentmethod
        [HttpPost]
        public async Task<IActionResult> Create([FromBody]  AddPaymentMethodRequestDto addPaymentMethodRequestDto)
        {
            if (ModelState.IsValid) {
              // Map DTO to Model (automapper)
              var paymentMethodDomainModel = mapper.Map<PaymentMethod>(addPaymentMethodRequestDto);


              // Use Domain MOdel to create Payment Method
              paymentMethodDomainModel = await paymentMethodRepository.CreateAsync(paymentMethodDomainModel);

              // Map Domains to DTOs (automapper)
              var paymentMethodDto = mapper.Map<PaymentMethodDto>(paymentMethodDomainModel);

              return CreatedAtAction(nameof(GetById), new { id = paymentMethodDto.PaymentMethodId}, paymentMethodDto);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        // Update Payment Method
        // PUT: https://localhost:7042/api/paymentmethod/{id}
        [HttpPut]
        [Route("{id:Guid}")]
        public async Task<IActionResult> Update([FromRoute] Guid id, 
          [FromBody] UpdatePaymentMethodRequestDto updatePaymentMethodRequestDto)
        {
          if (ModelState.IsValid) {
              // Map DTO to Model (automapper)
              var paymentMethodDomainModel = mapper.Map<PaymentMethod>(updatePaymentMethodRequestDto);

              // Check for Payment Method exists
              paymentMethodDomainModel = await paymentMethodRepository.UpdateAsync(id, paymentMethodDomainModel);

              if (paymentMethodDomainModel == null)
              {
                  return NotFound();
              }

              // Map Domains to DTOs (automapper)
              var paymentMethodDto = mapper.Map<PaymentMethodDto>(paymentMethodDomainModel);

              return Ok(paymentMethodDto);
          }
          else
          {
            return BadRequest(ModelState);
          }
        }

        // Delete Payment Method
        // DELETE: https://localhost:7042/api/paymentmethod/{id}
        [HttpDelete]
        [Route("{id:Guid}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            // Check for PaymentMethod exists
            var paymentMethodDomainModel = await paymentMethodRepository.DeleteAsync(id);

            if (paymentMethodDomainModel == null)
            {
                return NotFound();
            }

            // Return deleted PaymentMethod
            // Map Domains to DTOs (automapper)
            var paymentMethodDto = mapper.Map<PaymentMethodDto>(paymentMethodDomainModel);

            return Ok(paymentMethodDto);
        }
    }

}