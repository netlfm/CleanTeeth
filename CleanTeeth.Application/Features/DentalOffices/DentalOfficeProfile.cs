using AutoMapper;
using CleanTeeth.Application.Features.DentalOffices.Queries.GetDentalOfficeDetail;
using CleanTeeth.Application.Features.DentalOffices.Queries.GetDentalOfficeList;
using CleanTeeth.Domain.Entities;

namespace CleanTeeth.Application.Features.DentalOffices;

public class DentalOfficeProfile : Profile
{
    public DentalOfficeProfile()
    {
        CreateMap<DentalOffice, GetDentalOfficeDetailResponse>()
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email.Value))
            .ForMember(dest => dest.Phone, opt => opt.MapFrom(src => src.Phone.Value));


        CreateMap<DentalOffice, GetDentalOfficeListResponse>()
            .ForMember(dest => dest.Phone, opt => opt.MapFrom(src => src.Phone.Value))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email.Value));
    }

}
