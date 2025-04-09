using FluentValidation;
using PaymentTransaction.Models.DTO;

namespace PaymentTransaction.Validators
{
    public class UpdateTransactionRequestDtoValidator : AbstractValidator<UpdateTransactionRequestDto>
    {
        public UpdateTransactionRequestDtoValidator()
        {
            RuleFor(x => x.Amount)
                .GreaterThan(0).WithMessage("Amount must be a positive number.");

            RuleFor(x => x.PayerEmail)
                .NotEmpty().WithMessage("Payer email is required.")
                .EmailAddress().WithMessage("Payer email must be a valid email address.");

        }
    }
}