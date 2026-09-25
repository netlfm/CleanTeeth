using AutoMapper;
using CleanTeeth.Application.Features.Patients.Queries.GetPatientDetail;
using CleanTeeth.Application.Features.Patients.Queries.GetPatientList;
using CleanTeeth.Domain.Entities;

namespace CleanTeeth.Application.Features.Patients;

public class PatientProfile : Profile
{
    public PatientProfile()
    {
        CreateMap<Patient, GetPatientDetailResponse>()
        .ForMember(dest => dest.Phone, opt => opt.MapFrom(src => src.Phone.Value))
        .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email.Value));

        CreateMap<Patient, GetPatientListResponse>()
        .ForMember(dest => dest.Phone, opt => opt.MapFrom(src => src.Phone.Value))
        .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email.Value));
    }
}
