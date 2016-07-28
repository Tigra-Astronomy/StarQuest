﻿// This file is part of the MS.Gamification project
// 
// File: ViewModelMappingProfile.cs  Created: 2016-07-16@04:48
// Last modified: 2016-07-28@18:11

using AutoMapper;
using MS.Gamification.Models;
using MS.Gamification.ViewModels;
using MS.Gamification.ViewModels.Mission;
using MS.Gamification.ViewModels.UserAdministration;

namespace MS.Gamification.App_Start
    {
    public class ViewModelMappingProfile : Profile
        {
        public ViewModelMappingProfile()
            {
            CreateMap<CreateChallengeViewModel, Challenge>()
                .ForMember(m => m.Id, m => m.Ignore())
                .ForMember(m => m.Category, m => m.Ignore())
                .ForMember(m => m.MissionTrack, m => m.Ignore())
                .ReverseMap();
            CreateMap<Observation, SubmitObservationViewModel>()
                .ForMember(m => m.ObservationDateTimeLocal, m => m.MapFrom(s => s.ObservationDateTimeUtc.ToLocalTime()))
                .ForMember(m => m.ValidationImages, m => m.Ignore())
                .ReverseMap();
            CreateMap<Observation, ObservationDetailsViewModel>()
                .ForMember(m => m.UserName, m => m.MapFrom(s => s.User.UserName));
            CreateMap<ApplicationUser, ManageUserViewModel>()
                .ForMember(m => m.AccountLocked, m => m.MapFrom(s => s.LockoutEnabled))
                .ForMember(m => m.EmailVerified, m => m.MapFrom(s => s.EmailConfirmed))
                .ForMember(m => m.HasValidPassword, m => m.ResolveUsing(r => !string.IsNullOrWhiteSpace(r.PasswordHash)))
                .ForMember(m => m.Roles, m => m.Ignore())
                .ForMember(m => m.RoleToAdd, m => m.Ignore());
            CreateMap<BatchObservationViewModel, Observation>()
                .ForMember(m => m.Challenge, m => m.Ignore())
                .ForMember(m => m.ExpectedImage, m => m.Ignore())
                .ForMember(m => m.Status, m => m.Ignore())
                .ForMember(m => m.Id, m => m.Ignore())
                .ForMember(m => m.User, m => m.Ignore())
                .ForMember(m => m.UserId, m => m.Ignore());
            CreateMap<Challenge, ChallengeViewModel>()
                .ForMember(m=>m.HasObservation,m=>m.Ignore());
            CreateMap<MissionTrack, TrackProgressViewModel>()
                .ForMember(m=>m.PercentComplete,m=>m.Ignore());
            CreateMap<MissionLevel, LevelProgressViewModel>()
                .ForMember(m=>m.Unlocked,m=>m.Ignore())
                .ForMember(m=>m.OverallProgressPercent,m=>m.Ignore());
            CreateMap<Mission, MissionProgressViewModel>()
                .ForMember(m => m.MissionTitle, m => m.MapFrom(s => s.Title))
                .ForMember(m => m.Levels, m => m.MapFrom(s => s.MissionLevels));
            }
        }
    }