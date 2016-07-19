// This file is part of the MS.Gamification project
// 
// File: ViewModelMappingProfile.cs  Created: 2016-07-16@04:48
// Last modified: 2016-07-18@22:29

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
            CreateMap<ApplicationUser, ManageUserViewModel>()
                .ForMember(m => m.AccountLocked, m => m.MapFrom(s => s.LockoutEnabled))
                .ForMember(m => m.EmailVerified, m => m.MapFrom(s => s.EmailConfirmed))
                .ForMember(m => m.HasValidPassword, m => m.ResolveUsing(r => !string.IsNullOrWhiteSpace(r.PasswordHash)))
                .ForMember(m => m.Roles, m => m.Ignore())
                .ForMember(m => m.RoleToAdd, m => m.Ignore());
            }
        }
    }