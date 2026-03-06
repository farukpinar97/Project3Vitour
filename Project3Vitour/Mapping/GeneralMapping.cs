using AutoMapper;
using Project3Vitour.Dtos.BookingDtos;
using Project3Vitour.Dtos.CategoryDtos;
using Project3Vitour.Dtos.ContactDtos;
using Project3Vitour.Dtos.GuideDtos;
using Project3Vitour.Dtos.ReviewDtos;
using Project3Vitour.Dtos.TourDtos;
using Project3Vitour.Entities;

namespace Project3Vitour.Mapping
{
    public class GeneralMapping : Profile
    {
        public GeneralMapping()
        {
            // Category
            CreateMap<Category, CreateCategoryDto>().ReverseMap();
            CreateMap<Category, ResultCategoryDto>().ReverseMap();
            CreateMap<Category, UpdateCategoryDto>().ReverseMap();
            CreateMap<Category, GetCategoryByIdDto>().ReverseMap();

            // TourDayPlan — önce tanımlanmalı
            CreateMap<TourDayPlan, TourDayPlanDto>().ReverseMap();

            // Tour
            CreateMap<Tour, ResultTourDto>().ReverseMap();

            CreateMap<Tour, GetTourByIdDto>()
                .ForMember(d => d.DayPlans, o => o.MapFrom(s => s.DayPlans))
                .ReverseMap()
                .ForMember(d => d.DayPlans, o => o.MapFrom(s => s.DayPlans));

            CreateMap<Tour, CreateTourDto>()
                .ForMember(d => d.DayPlans, o => o.MapFrom(s => s.DayPlans))
                .ReverseMap()
                .ForMember(d => d.DayPlans, o => o.MapFrom(s => s.DayPlans));

            CreateMap<Tour, UpdateTourDto>()
                .ForMember(d => d.DayPlans, o => o.MapFrom(s => s.DayPlans))
                .ReverseMap()
                .ForMember(d => d.DayPlans, o => o.MapFrom(s => s.DayPlans));

            // Review
            CreateMap<TourReview, TourReviewDto>().ReverseMap();
            CreateMap<TourReview, ResultReviewDto>().ReverseMap();
            CreateMap<CreateReviewDto, TourReview>();

            // Contact
            CreateMap<ContactMessage, CreateContactMessageDto>().ReverseMap();

            // Guide
            CreateMap<Guide, ResultGuideDto>().ReverseMap();
            CreateMap<Guide, CreateGuideDto>().ReverseMap();
            CreateMap<Guide, UpdateGuideDto>().ReverseMap();

            // Booking
            CreateMap<CreateBookingDto, Booking>();
            CreateMap<Booking, ResultBookingDto>();
        }
    }
}