using FluentValidation;
using Microsoft.EntityFrameworkCore;
using PaymentTransaction.Data;
using PaymentTransaction.Models.DTO;

namespace PaymentTransaction.Validators
{
    public class AddTransactionViaProviderNameDtoValidator : AbstractValidator<AddTransactionViaProviderNameDto>
    {
        private readonly PaymentTransactionDbContext dbContext = null!;

        public AddTransactionViaProviderNameDtoValidator(PaymentTransactionDbContext dbContext)
        {
            this.dbContext = dbContext;

            RuleFor(x => x.Amount)
                .GreaterThan(0)
                .WithMessage("Amount must be a positive number.");

            RuleFor(x => x.PayerEmail)
                .NotEmpty()
                .WithMessage("Payer email is required.")
                .EmailAddress()
                .WithMessage("Payer email must be a valid email address.");

            RuleFor(x => x.CurrencyId)
                .MustAsync(CurrencyExists)
                .WithMessage("CurrencyId is not valid.");

            RuleFor(x => x.StatusId)
                .MustAsync(StatusExists)
                .WithMessage("StatusId is not valid.");

            RuleFor(x => x.PaymentMethodId)
                .MustAsync(PaymentMethodExists)
                .WithMessage("PaymentMethodId is not valid.");

            // RuleFor(x => x.ProviderId)
            //     .MustAsync(ProviderExists)
            //     .WithMessage("ProviderId is not valid.");
        }

        private async Task<bool> CurrencyExists(Guid id, CancellationToken ct) =>
            await dbContext.Currency.AnyAsync(c => c.CurrencyId == id, ct);

        private async Task<bool> StatusExists(Guid id, CancellationToken ct) =>
            await dbContext.Status.AnyAsync(s => s.StatusId == id, ct);

        private async Task<bool> PaymentMethodExists(Guid id, CancellationToken ct) =>
            await dbContext.PaymentMethod.AnyAsync(p => p.PaymentMethodId == id, ct);

        // private async Task<bool> ProviderExists(Guid id, CancellationToken ct) =>
        //     await dbContext.Provider.AnyAsync(p => p.ProviderId == id, ct);
    }
}

