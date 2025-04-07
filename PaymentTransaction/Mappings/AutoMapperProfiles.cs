using AutoMapper;
using PaymentTransaction.Models.DTO;
using PaymentTransaction.Models.Domain;

namespace PaymentTransaction.Mappings
{
  public class AutoMapperProfiles: Profile
  {
    public AutoMapperProfiles()
    {
      CreateMap<Provider, ProviderDto>().ReverseMap();
      CreateMap<AddProviderRequestDto, Provider>().ReverseMap();
      CreateMap<UpdateProviderRequestDto, Provider>().ReverseMap();
    }

  }
}
