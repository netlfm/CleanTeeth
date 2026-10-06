using AutoMapper;
using CleanTeeth.Application.Features.Treatments.Queries.GetTreatmentDetail;
using CleanTeeth.Application.Features.Treatments.Queries.GetTreatmentList;
using CleanTeeth.Domain.Entities;

namespace CleanTeeth.Application.Features.Treatments;

public class TreatmentProfile : Profile
{
    public TreatmentProfile()
    {
        CreateMap<Treatment, GetTreatmentDetailResponse>()
        .ForMember(
            dest => dest.PatientName,
            opt => opt.MapFrom(src => src.Appointment!.Patient!.Name))
        .ForMember(
            dest => dest.DentistName,
            opt => opt.MapFrom(src => src.Appointment!.Dentist!.Name))
        .ForMember(
            dest => dest.DentalOfficeName,
            opt => opt.MapFrom(src => src.Appointment!.DentalOffice!.Name));

        CreateMap<Treatment, GetTreatmentListResponse>()
        .ForMember(
            dest => dest.PatientName,
            opt => opt.MapFrom(src => src.Appointment!.Patient!.Name))
        .ForMember(
            dest => dest.DentistName,
            opt => opt.MapFrom(src => src.Appointment!.Dentist!.Name))
        .ForMember(
            dest => dest.DentalOfficeName,
            opt => opt.MapFrom(src => src.Appointment!.DentalOffice!.Name));
    }
}
