using AutoMapper;
using CleanTeeth.Application.Features.Appointments.Queries.GetAppointmentDelail;
using CleanTeeth.Application.Features.Appointments.Queries.GetAppointmentList;
using CleanTeeth.Domain.Entities;

namespace CleanTeeth.Application.Features.Appointments;

public class AppointmentProfile : Profile
{
    public AppointmentProfile()
    {
        CreateMap<Appointment, GetAppointmentDetailResponse>()
            .ForMember(
                dest => dest.PatientName,
                opt => opt.MapFrom(src => src.Patient!.Name))
            .ForMember(
                dest => dest.DentistName,
                opt => opt.MapFrom(src => src.Dentist!.Name))
            .ForMember(
                dest => dest.DentalOfficeName,
                opt => opt.MapFrom(src => src.DentalOffice!.Name))
            .ForMember(
                dest => dest.Status,
                opt => opt.MapFrom(src => src.Status.ToString()))
            .ForMember(
                dest => dest.StartDate,
                opt => opt.MapFrom(src => src.TimeInterval.Start))
            .ForMember(
                dest => dest.EndDate,
                opt => opt.MapFrom(src => src.TimeInterval.End));
        CreateMap<Appointment, GetAppointmentListResponse>()
            .ForMember(
                dest => dest.PatientName,
                opt => opt.MapFrom(src => src.Patient!.Name))
            .ForMember(
                dest => dest.DentistName,
                opt => opt.MapFrom(src => src.Dentist!.Name))
            .ForMember(
                dest => dest.DentalOfficeName,
                opt => opt.MapFrom(src => src.DentalOffice!.Name))
            .ForMember(
                dest => dest.Status,
                opt => opt.MapFrom(src => src.Status.ToString()))
            .ForMember(
                dest => dest.StartDate,
                opt => opt.MapFrom(src => src.TimeInterval.Start))
            .ForMember(
                dest => dest.EndDate,
                opt => opt.MapFrom(src => src.TimeInterval.End));
    }
}
