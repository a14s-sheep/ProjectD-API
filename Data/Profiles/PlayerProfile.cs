using AutoMapper;
using ProjectD_API.Data.Models;

namespace ProjectD_API.Data.Profiles
{
    public class PlayerProfile : Profile
    {
        public PlayerProfile()
        {
            CreateMap<Player, PlayerData>();

            CreateMap<PlayerData, Player>()
                // Player không có list => ignore
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
                .ForMember(dest => dest.DataId, opt => opt.MapFrom(src => src.DataId))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Level, opt => opt.MapFrom(src => src.Level))
                .ForMember(dest => dest.Experience, opt => opt.MapFrom(src => src.Experience))
                .ForMember(dest => dest.Health, opt => opt.MapFrom(src => src.Health))
                .ForMember(dest => dest.StatPoint, opt => opt.MapFrom(src => src.StatPoint))
                .ForMember(dest => dest.Power, opt => opt.MapFrom(src => src.Power))
                .ForMember(dest => dest.Agility, opt => opt.MapFrom(src => src.Agility))
                .ForMember(dest => dest.Vitality, opt => opt.MapFrom(src => src.Vitality))
                .ForMember(dest => dest.MaxHealth, opt => opt.MapFrom(src => src.MaxHealth))
                .ForMember(dest => dest.HealthRegen, opt => opt.MapFrom(src => src.HealthRegen))
                .ForMember(dest => dest.Armor, opt => opt.MapFrom(src => src.Armor))
                .ForMember(dest => dest.Damage, opt => opt.MapFrom(src => src.Damage))
                .ForMember(dest => dest.AttackSpeed, opt => opt.MapFrom(src => src.AttackSpeed))
                .ForMember(dest => dest.CritPower, opt => opt.MapFrom(src => src.CritPower))
                .ForMember(dest => dest.CritChance, opt => opt.MapFrom(src => src.CritChance))
                .ForMember(dest => dest.ArmorReduction, opt => opt.MapFrom(src => src.ArmorReduction))
                .ForMember(dest => dest.MoveSpeed, opt => opt.MapFrom(src => src.MoveSpeed))
                .ForMember(dest => dest.CurrentMap, opt => opt.MapFrom(src => src.CurrentMap))
                .ForMember(dest => dest.CurrentPositionX, opt => opt.MapFrom(src => src.CurrentPositionX))
                .ForMember(dest => dest.CurrentPositionY, opt => opt.MapFrom(src => src.CurrentPositionY))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt))
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => src.UpdatedAt));
        }
    }
}
