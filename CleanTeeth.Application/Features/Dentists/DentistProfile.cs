using AutoMapper;
using CleanTeeth.Application.Features.Dentists.Queries.GetDentistDetail;
using CleanTeeth.Application.Features.Dentists.Queries.GetDentistList;
using CleanTeeth.Domain.Entities;

namespace CleanTeeth.Application.Features.Dentists;

public class DentistProfile : Profile
{
    public DentistProfile()
    {
        CreateMap<Dentist, GetDentistListResponse>()
        .ForMember(dest => dest.Phone, opt => opt.MapFrom(src => src.Phone.Value))
        .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email.Value));


        CreateMap<Dentist, GetDentistDetailResponse>()
        .ForMember(dest => dest.Phone, opt => opt.MapFrom(src => src.Phone.Value))
        .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email.Value));

    }
}
