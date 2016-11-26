// This file is part of the MS.Gamification project
// 
// File: ViewModelMappingProfile.cs  Created: 2016-07-16@04:48
// Last modified: 2016-08-20@02:00

using AutoMapper;
using MS.Gamification.Areas.Admin.ViewModels;
using MS.Gamification.Areas.Admin.ViewModels.MissionLevels;
using MS.Gamification.Areas.Admin.ViewModels.MissionTracks;
using MS.Gamification.Areas.Admin.ViewModels.UserAdministration;
using MS.Gamification.Models;
using MS.Gamification.ViewModels;
using MS.Gamification.ViewModels.Mission;
using MS.Gamification.ViewModels.Moderation;

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
                .ForMember(m => m.EquipmentPicker, m => m.Ignore())
                .ForMember(m => m.SeeingPicker, m => m.Ignore())
                .ForMember(m => m.TransparencyPicker, m => m.Ignore())
                .ReverseMap()
                .ForMember(m => m.ObservationDateTimeUtc, m => m.MapFrom(s => s.ObservationDateTimeLocal.ToUniversalTime()));
            CreateMap<Observation, ObservationDetailsViewModel>()
                .ForMember(m => m.UserName, m => m.MapFrom(s => s.User.UserName));
            CreateMap<Observation, ModerationQueueItem>()
                .ForMember(m => m.DateTime, m => m.MapFrom(s => s.ObservationDateTimeUtc))
                .ForMember(m => m.ObservationId, m => m.MapFrom(s => s.Id))
                .ForMember(m => m.UserName, m => m.MapFrom(s => s.User.UserName));
            CreateMap<ApplicationUser, ManageUserViewModel>()
                .ForMember(m=>m.Selected, m=>m.UseValue(false))
                .ForMember(m => m.AccountLocked, m => m.MapFrom(s => s.LockoutEnabled))
                .ForMember(m => m.EmailVerified, m => m.MapFrom(s => s.EmailConfirmed))
                .ForMember(m => m.HasValidPassword, m => m.ResolveUsing(r => !string.IsNullOrWhiteSpace(r.PasswordHash)))
                .ForMember(m => m.Roles, m => m.Ignore())
                .ForMember(m => m.RoleToAdd, m => m.Ignore())
                .ForMember(m => m.RolePicker, m => m.Ignore());
            CreateMap<BatchObservationViewModel, Observation>()
                .ForMember(m => m.Challenge, m => m.Ignore())
                .ForMember(m => m.ExpectedImage, m => m.Ignore())
                .ForMember(m => m.Status, m => m.Ignore())
                .ForMember(m => m.Id, m => m.Ignore())
                .ForMember(m => m.User, m => m.Ignore())
                .ForMember(m => m.UserId, m => m.Ignore());
            CreateMap<Challenge, ChallengeViewModel>()
                .ForMember(m => m.HasObservation, m => m.Ignore());
            CreateMap<MissionTrack, TrackProgressViewModel>()
                .ForMember(m => m.PercentComplete, m => m.Ignore());
            CreateMap<MissionLevel, LevelProgressViewModel>()
                .ForMember(m => m.Unlocked, m => m.Ignore())
                .ForMember(m => m.OverallProgressPercent, m => m.Ignore());
            CreateMap<Mission, MissionProgressViewModel>()
                .ForMember(m => m.MissionTitle, m => m.MapFrom(s => s.Title))
                .ForMember(m => m.Levels, m => m.MapFrom(s => s.MissionLevels));
            CreateMap<MissionTrackViewModel, MissionTrack>()
                .ForMember(m => m.Challenges, m => m.Ignore())
                .ForMember(m => m.Id, m => m.Ignore())
                .ForMember(m => m.Badge, m => m.Ignore())
                .ForMember(m => m.MissionLevel, m => m.Ignore())
                .ReverseMap();
            CreateMap<MissionLevelViewModel, MissionLevel>()
                .ForMember(m => m.Tracks, m => m.Ignore())
                .ForMember(m => m.Mission, m => m.Ignore())
                .ReverseMap();
            }
        }
    }