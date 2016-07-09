using AutoMapper;
using MS.Gamification.Models;
using MS.Gamification.ViewModels;

namespace MS.Gamification
    {
    public static class MapperConfig
        {
        public static void RegisterMaps()
            {
            Mapper.Initialize(cfg =>
                {
                cfg.CreateMap<CreateChallengeViewModel, Challenge>();
                });
            }
        }
    }