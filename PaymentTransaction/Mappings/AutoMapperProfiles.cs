using AutoMapper;
using PaymentTransaction.Models.DTO;
using PaymentTransaction.Models.Domain;

namespace PaymentTransaction.Mappings
{
    public class AutoMapperProfiles: Profile
    {
        public AutoMapperProfiles()
        {
            // Provider automapper
            CreateMap<Provider, ProviderDto>().ReverseMap();
            CreateMap<AddProviderRequestDto, Provider>().ReverseMap();
            CreateMap<UpdateProviderRequestDto, Provider>().ReverseMap();

            // Currency Automapper
            CreateMap<Currency, CurrencyDto>().ReverseMap();
            CreateMap<AddCurrencyRequestDto, Currency>().ReverseMap();
            CreateMap<UpdateCurrencyRequestDto, Currency>().ReverseMap();

            // Payment Method Automapper
            CreateMap<PaymentMethod, PaymentMethodDto>().ReverseMap();
            CreateMap<AddPaymentMethodRequestDto, PaymentMethod>().ReverseMap();
            CreateMap<UpdatePaymentMethodRequestDto, PaymentMethod>().ReverseMap();

            // Status Automapper
            CreateMap<Status, StatusDto>().ReverseMap();
            CreateMap<AddStatusRequestDto, Status>().ReverseMap();
            CreateMap<UpdateStatusRequestDto, Status>().ReverseMap();

            // Payment Transaction Automapper
            CreateMap<Transaction, TransactionDto>().ReverseMap();
            CreateMap<AddTransactionRequestDto, Transaction>().ReverseMap();
            CreateMap<AddTransactionViaProviderNameDto, Transaction>().ReverseMap();
            CreateMap<UpdateTransactionRequestDto, Transaction>().ReverseMap();
        }

    }
}
