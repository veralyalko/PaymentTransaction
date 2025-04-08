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
    }

  }
}
