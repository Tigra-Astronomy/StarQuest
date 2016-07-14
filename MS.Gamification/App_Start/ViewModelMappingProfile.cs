// This file is part of the MS.Gamification project
// 
// File: ViewModelMappingProfile.cs  Created: 2016-07-13@23:20
// Last modified: 2016-07-14@00:42

using AutoMapper;
using MS.Gamification.Models;
using MS.Gamification.ViewModels;

namespace MS.Gamification.App_Start
    {
    public class ViewModelMappingProfile : Profile
        {
        public ViewModelMappingProfile()
            {
            CreateMap<CreateChallengeViewModel, Challenge>().ReverseMap();
            CreateMap<Observation, SubmitObservationViewModel>().ReverseMap();
            CreateMap<Observation, ObservationDetailsViewModel>()
                .ForMember(m => m.UserName, m => m.MapFrom(s => s.User.UserName));
            }
        }
    }